using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.Utils.Log;
using PM.PaymentProtocolModel.BankCommModel.HuangMeiPostal;
using PM.TaskBiz.HuangMeiPostlTask.ORM;
using PM.Utils;
using System.Web;
using PM.Utils.WebUtils;

namespace PM.TaskBiz.HuangMeiPostlTask
{
    /// <summary>
    /// 回调执行任务
    /// </summary>
    public class HuangMeiPostlCallBack : ITimerTaskCallBack
    {
        /// <summary>
        /// 回调任务
        /// </summary>
        /// <param name="dyObj"></param>
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
        /// <param name="modelList">报文返回列表</param>
        /// <returns></returns>
        private bool IncrementToDB(List<HuangMeiQueryResult> modelList)
        {
            bool rtn = false;
            T_HMPostal enter = null;
            var dbEnter = new PM.TaskBiz.HuangMeiPostlTask.ORM.GovPublic_jinhuaEntities();
            // var dbList = enter.T_JSABOC.Select(" IsMatch='0'  and TradeCode='ZTB1'");//获取数据库内容
            var dbList =
                from c in dbEnter.T_HMPostal    select c;

             //from c in dbEnter.T_HMPostal  where(c.Match == 0 || c.Match == null) select c;
            //获取数据库内容
            foreach (var lst in modelList)
            {
                //var chk = dbList.FirstOrDefault(p => p.TradeSerialNumber == lst.TradeSerialNumber&&p.Amount==lst.Amount);
                var chk = dbList.FirstOrDefault(p => p.TradeDate == lst.TradeDate && p.AccountNo == lst.AccountNo && p.TradeSerialNumber == lst.TradeSerialNumber);
                if (chk == null)//增量添加
                {
                    enter = new T_HMPostal();
                    enter.ID = Guid.NewGuid().ToString();
                    enter.AccountNo = lst.AccountNo;
                    enter.TradeDate = lst.TradeDate;
                    enter.TradeSerialNumber = lst.TradeSerialNumber;

                    enter.ProofKind = lst.ProofKind;
                    enter.ProofNum = lst.ProofNum;
                    enter.Amount = lst.Amount;
                    enter.RemainAmount = lst.RemainAmount;
                    enter.LoanMark = lst.LoanMark;


                    enter.Oprationer = lst.Oprationer;
                    enter.Checker = lst.Checker;

                    enter.Authorizer = lst.Authorizer;
                    enter.CounterpartAccountNo = lst.CounterpartAccountNo;
                    enter.CounterpartAccountName = lst.CounterpartAccountName;
                    enter.Remark = lst.Remark;

                    enter.TradeWay = lst.TradeWay;
                    enter.TradeNo = lst.TradeNo;
                    enter.TimeStamp = lst.TimeStamp;
                    enter.DepartmentAccName = lst.DepartmentAccName;
                    enter.CreateTm = DateTime.Now;
                    dbEnter.T_HMPostal.AddObject(enter);
                    if (!rtn)
                    {
                        rtn = true;
                    }
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
                    LogTxt.WriteEntry("明细入账异常" + ex.Message, "黄梅支付匹配");
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
            var urlStr = ConfigHelper.GetCustomCfg("HM", "BusinessUrl");
            var enCodingStr = ConfigHelper.GetCustomCfg("HM", "enCoding");
            var chkStr = ConfigHelper.GetCustomCfg("HM", "chkStr");//核对值
            var roundCount = Convert.ToInt32(ConfigHelper.GetCustomCfg("HM", "roundCount"));//核对值
            if (string.IsNullOrEmpty(urlStr) || string.IsNullOrEmpty(enCodingStr) || string.IsNullOrEmpty(chkStr) || roundCount <= 0)
            {
                LogTxt.WriteEntry("回调地址或者编码等未设置，请设置", "黄梅支付匹配");
                return;
            }
            var enCoding = Encoding.GetEncoding(enCodingStr);//回调编码
            #endregion

            bool haveMatch = false;//是否匹配 
            var dbEnter = new PM.TaskBiz.HuangMeiPostlTask.ORM.GovPublic_jinhuaEntities();
            //var matchList = dbEnter.T_HMPostal.Where(p => (p.Match != 1 || p.Match == null) && !string.IsNullOrEmpty(p.Remark) && p.LoanMark == "1");//获取匹配表待匹配信息 (获取借的标记   0借  1贷)
            var matchList = dbEnter.T_HMPostal.Where(p => (p.Match != 1 || p.Match == null) && p.LoanMark == "1");//获取匹配表待匹配信息 (获取借的标记   0借  1贷)

            #region 匹配处理  优先规则是订单号匹配到
            foreach (var lst in matchList)//匹配
            {
                var remark = lst.Remark.Trim();
                string payAccount_remark = string.Empty;//备注中的支付账号
                var tradeno = GetTradNo(lst.Remark.Trim(), out payAccount_remark);//订单号

                var payRealAccountName = string.IsNullOrEmpty(lst.CounterpartAccountName) == true ? string.Empty : HttpUtility.UrlEncode(lst.CounterpartAccountName, enCoding);
                var payRealAccountNo = lst.CounterpartAccountNo ?? payAccount_remark;//获取付款账户  如果未获取到就取备注上的付款账户
                var payRealBankName = string.IsNullOrEmpty(lst.DepartmentAccName) == true ? string.Empty : HttpUtility.UrlEncode(lst.DepartmentAccName, enCoding);
                var amount = lst.Amount;
                var feeAmount = 0;
                var primaryID = string.Empty;
                var slaveID = string.Empty;
                var serialNumber = lst.TradeSerialNumber;
                var loanMark = lst.LoanMark;
                var payDateTime=lst.TimeStamp;


                var postStr = string.Format("PayRealAccountName={0}&PayRealAccountNo={1}&PayRealBankName={2}&Amount={3}&FeeAmount={4}&PrimaryID={5}&SlaveID={6}&TradeNo={7}&SerialNumber={8}&LoanMark={9}&PayDateTime={10}"
               , payRealAccountName
               , payRealAccountNo
               , payRealBankName
               , amount
               , feeAmount
               , primaryID
               , slaveID
               , tradeno
               , serialNumber
               , loanMark
               , payDateTime

               );
                LogTxt.WriteEntry("支付信息回调信息" + urlStr + postStr, "黄梅支付匹配回调");
                if (HttpTransfer.PostBackToBusinesss(postStr, urlStr, enCoding, chkStr, roundCount))//匹配完成
                {
                    lst.Match = 1;//成功  
                    dbEnter.T_HMPostal.ApplyCurrentValues(lst);
                    haveMatch = true;//初始化匹配状态 
                }
                else
                {
                    //var postBackStr = string.Format("PayRealAccountName={0}&PayRealAccountNo={1}&PayRealBankName={2}&Amount={3}&FeeAmount={4}&PrimaryID={5}&SlaveID={6}&TradeNo={7}&SerialNumber={8}}&LoanMark={9}",
                    //lst.CounterpartAccountName, lst.CounterpartAccountNo, lst.DepartmentAccName,
                    // lst.Amount, 0, string.Empty, string.Empty, lst.Remark, lst.TradeSerialNumber, lst.LoanMark);
                    LogTxt.WriteEntry("支付信息回调匹配响应失败" + postStr, "黄梅支付匹配");
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
                    LogTxt.WriteEntry("订单匹配处理异常" + ex.Message, "黄梅支付匹配");
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取订单号
        /// </summary>
        /// <param name="inputStr">输入字符</param>
        /// <param name="payAccountNo">支付账号</param>
        /// <returns></returns>
        private string GetTradNo(string inputStr, out string payAccountNo)
        {
            var tradeNo = string.Empty;
            payAccountNo = string.Empty;
            try
            {
                var sourceStr = PM.Utils.StringHelper.ToDBC(inputStr);
                sourceStr = sourceStr.Replace(" ", "");
                var rtnStr = PM.Utils.StringHelper.GetNumberString(sourceStr, false);
                var strIndex = int.Parse(ConfigHelper.GetCustomCfg("HM", "OrderIndex"));
                var strLength = int.Parse(ConfigHelper.GetCustomCfg("HM", "OrderLength"));

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
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("获取订单信息失败:" + ex.Message, "黄梅支付匹配");
            }
            return tradeNo;
        }
        #endregion
    }
}
