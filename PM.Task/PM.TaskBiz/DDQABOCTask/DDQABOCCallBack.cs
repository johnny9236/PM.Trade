using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.DDQABOC;
using PM.TaskBiz.ORM;
using PM.Utils.Log;
using PM.Utils;
using System.Web;
using PM.Utils.WebUtils;
using System.Linq.Expressions;

namespace PM.TaskBiz.DDQABOCTask
{
    public class DDQABOCCallBack : ITimerTaskCallBack
    {
        public void CallBack(dynamic dyObj)
        {
            //   增量入库操作
            var haveIn = IncrementToDB(dyObj);
            //回调
            //  if (haveIn)  //控制暂时取消
            //匹配操作 
            Match();
        }

        #region 增量入库
        /// <summary>
        /// 增量入库
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        private bool IncrementToDB(List<DDQAccountDtl> modelList)
        {
            bool rtn = false;
            T_DDQABOC enter = null;
            var dbEnter = new PM.TaskBiz.ORM.Pub_Entities();
            var dbList = from c in dbEnter.T_DDQABOC select c;
            //获取数据库内容
            foreach (var lst in modelList)
            {
                var chk = dbList.FirstOrDefault(p => p.TrJrn == lst.TrJrn && p.AccNo == lst.AccNo && p.TrDate == lst.TrDate && p.TimeStab == lst.TimeStab && p.TrFrom == lst.TrFrom);
                if (chk == null)//增量添加
                {
                    #region
                    enter = new T_DDQABOC();
                    enter.ID = Guid.NewGuid().ToString();
                    enter.Abs = lst.Abs;
                    enter.AccName = lst.AccName;
                    enter.AccNo = lst.AccNo;
                    decimal amt = 0;
                    decimal.TryParse(lst.Amt, out  amt);
                    enter.Amt = amt;

                    enter.AmtIndex = lst.AmtIndex;
                    enter.Bal = lst.Bal;
                    enter.CreateTm = DateTime.Now;
                    enter.CshIndex = lst.CshIndex;
                    enter.Cur = lst.Cur;

                    enter.CustRef = lst.CustRef;
                    enter.ErrDate = lst.ErrDate;
                    enter.ErrVchNo = lst.ErrVchNo;
                    enter.OppAccNo = lst.OppAccNo;
                    enter.OppBkName = lst.OppBkName;

                    enter.OppCur = lst.OppCur;
                    enter.OppName = lst.OppName;
                    enter.OppProv = lst.OppProv;
                    enter.PostScript = lst.PostScript;
                    enter.PreAmt = lst.PreAmt;

                    enter.Prov = lst.Prov;
                    enter.Teller = lst.Teller;
                    enter.TimeStab = lst.TimeStab;
                    enter.TotChg = lst.TotChg;
                    enter.TransCode = lst.TransCode;

                    enter.TrBankNo = lst.TrBankNo;
                    enter.TrDate = lst.TrDate;
                    enter.TrFrom = lst.TrFrom;
                    enter.TrJrn = lst.TrJrn;
                    enter.TrType = lst.TrType;

                    enter.VchNo = lst.VchNo;
                    enter.VoucherBat = lst.VoucherBat;
                    enter.VoucherNo = lst.VoucherNo;
                    enter.VoucherProv = lst.VoucherProv;
                    enter.VoucherType = lst.VoucherType;

                    enter.CreateTm = DateTime.Now;
                    dbEnter.T_DDQABOC.AddObject(enter);
                    if (!rtn)
                    {
                        rtn = true;
                    }
                    #endregion
                }
            }
            if (rtn)
            {
                try
                {
                    dbEnter.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry("明细入账异常" + ex.Message, "掇刀区支付匹配");
                }

            }
            return rtn;
        }

        /// <summary>
        /// 匹配操作
        /// </summary>
        private void Match()
        {
            #region  回调准备
            var urlStr = ConfigHelper.GetCustomCfg("DDQ", "BusinessUrl");
            var enCodingStr = ConfigHelper.GetCustomCfg("DDQ", "enCoding");
            var chkStr = ConfigHelper.GetCustomCfg("DDQ", "chkStr");//核对值
            var roundCount = Convert.ToInt32(ConfigHelper.GetCustomCfg("DDQ", "roundCount"));//核对值
            if (string.IsNullOrEmpty(urlStr) || string.IsNullOrEmpty(enCodingStr) || string.IsNullOrEmpty(chkStr) || roundCount <= 0)
            {
                LogTxt.WriteEntry("回调地址或者编码等未设置，请设置", "掇刀区支付匹配");
                return;
            }
            var enCoding = Encoding.GetEncoding(enCodingStr);//回调编码
            #endregion
            string payAccount = string.Empty;//付款方账号
            bool haveMatch = false;//是否匹配 
            var dbEnter = new PM.TaskBiz.ORM.Pub_Entities();
            var matchList = dbEnter.T_DDQABOC.Where(p => (p.Match != 1 || p.Match == null)&&p.Amt>0);//获取匹配表待匹配信息 (获取借的标记   0借  1贷)
            #region 匹配处理  优先规则是订单号匹配到
            foreach (var lst in matchList)//匹配
            {
             
                payAccount = string.Empty;//付款方账号
                var orderNo = GetDbcNumStr(lst.PostScript, out payAccount);
                if (string.IsNullOrEmpty(orderNo))
                    continue;
                if (!string.IsNullOrEmpty(lst.OppAccNo))
                    payAccount = lst.OppAccNo;

                var postStr = string.Format(
                    @"PayRealAccountName={0}&PayRealAccountNo={1}&PayRealBankName={2}
                        &ReceiveRealAccountName={3}&ReceiveRealAccountNo={4}&Amount={5}
                        &FeeAmount={6}&PrimaryID={7}&SlaveID={8}&TradeNo={9}
                        &SerialNumber={10}&LoanMark={11}&CostType={12}&PayDateTime={13}&PayDate={14}&PayTime={15}",
                   HttpUtility.UrlEncode(lst.OppName, enCoding),
                   lst.OppAccNo ?? payAccount,
                   string.Empty,
                   string.Empty,
                   lst.AccNo,
                   lst.Amt,
                   lst.TotChg,
                   string.Empty,
                   string.Empty,
                   orderNo,
                   lst.TrJrn,
                   "0"
                   , "BZJ"
                   , lst.TrDate.Trim() + lst.TimeStab.Trim()
                   , lst.TrDate.Trim()
                   , lst.TimeStab.Trim()
                   );
                LogTxt.WriteEntry("回调信息" + urlStr + postStr, "掇刀区支付匹配");
                if (HttpTransfer.PostBackToBusinesss(postStr, urlStr, enCoding, chkStr, roundCount))//匹配完成
                {
                    lst.Match = 1;//成功  
                    dbEnter.T_DDQABOC.ApplyCurrentValues(lst);
                    haveMatch = true;//初始化匹配状态 
                }
                else
                {
                    LogTxt.WriteEntry("回调匹配响应失败" + postStr, "掇刀区支付匹配");
                }
            }
            if (haveMatch)//先保存一次
            {
                try
                {
                    dbEnter.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry("订单匹配处理异常" + ex.Message, "掇刀区支付匹配");
                }
            }
            #endregion
        }
        #endregion
        #region private

        /// <summary>
        /// 订单号转半角字符
        /// </summary>
        /// <param name="inputStr">输入字符</param>
        /// <returns></returns>
        private string GetDbcNumStr(string inputStr, out string payAccountNo)
        {
            var tradeNo = string.Empty;
            payAccountNo = string.Empty;
            try
            {
                var sourceStr = PM.Utils.StringHelper.ToDBC(inputStr);
                sourceStr = sourceStr.Replace(" ", "");
                var rtnStr = PM.Utils.StringHelper.GetNumberString(sourceStr, false);
                var strIndex = int.Parse(ConfigHelper.GetCustomCfg("DDQ", "OrderIndex"));
                var strLength = int.Parse(ConfigHelper.GetCustomCfg("DDQ", "OrderLength"));

                //if (strLength + strIndex >= rtnStr.Length)
                //{
                //    if (strIndex < rtnStr.Length)
                //    {
                //        tradeNo = rtnStr.Substring(strIndex);
                //    }
                //}
                if (strLength + strIndex <= rtnStr.Length)
                {
                    tradeNo = rtnStr.Substring(strIndex, strLength);
                    payAccountNo = rtnStr.Substring(strLength + strIndex);
                }
                //else
                //{
                //    tradeNo = rtnStr;
                //}
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("订单号转半角处理异常" + ex.Message, "掇刀区支付匹配");
            }
            return tradeNo;
        }
        #endregion






    }
}
