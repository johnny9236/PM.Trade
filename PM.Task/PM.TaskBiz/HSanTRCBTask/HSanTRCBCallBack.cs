using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.TaskBiz.ORM;
using PM.PaymentProtocolModel.BankCommModel.AHQY;
using PM.Utils.Log;
using PM.Utils;
using System.Web;
using PM.Utils.WebUtils;
using PM.PaymentProtocolModel.BankCommModel;

namespace PM.TaskBiz.HSanTRCBTask
{
    public class HSanTRCBCallBack : ITimerTaskCallBack
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
        private bool IncrementToDB(List<BBCQueryInfo> modelList)
        {
            bool rtn = false;
            T_Margin enter = null;
            var dbEnter = new PM.TaskBiz.ORM.Pub_Entities();
            var dbList = from c in dbEnter.T_Margin select c;
            T_Margin chk = null;
            //获取数据库内容
            foreach (var lst in modelList)
            {
                 if (lst.BusniessType == "1")//退款
                {
                    if (lst.BackResult.Trim() == "1")//退款成功
                    {   //通过流水号+缴费类型+授权码（类似标段编码）
                        chk = dbList.FirstOrDefault(p => p.HstSeqNum == lst.HstSeqNum && p.BusniessType == lst.BusniessType && p.AuthCode == lst.AuthCode && p.BackResult == lst.BackResult&&p.BankType==lst.BankType);

                    }
                    else
                        continue;
                    }
                else
                {
                    chk = dbList.FirstOrDefault(p => p.HstSeqNum == lst.HstSeqNum && p.BusniessType == lst.BusniessType && p.AuthCode == lst.AuthCode&&p.BankType==lst.BankType);
                }
                 if (chk == null &&
                 ((!string.IsNullOrEmpty(lst.InAcct) && lst.BusniessType.Trim() == "0")//支付
                 || ((!string.IsNullOrEmpty(lst.BackResult) && lst.BackResult.Trim()=="1") && lst.BusniessType.Trim() == "1")//退款
                   )
                   )//增量添加
                 {
                     enter = new T_Margin();
                     enter.ID = Guid.NewGuid().ToString();

                     enter.HstSeqNum = lst.HstSeqNum;
                     enter.InAcct = lst.InAcct;
                     enter.InAmount = lst.InAmount;
                     enter.InDate = lst.InDate;
                     enter.InMemo = lst.InMemo;
                     enter.InName = lst.InName;
                     enter.InTime = lst.InTime;

                     enter.PunInst = lst.PunInst;
                     enter.BackResult = lst.BackResult;
                     enter.AddWord = lst.AddWord;
                     enter.Gernal = lst.Gernal;

                     enter.BusniessType = lst.BusniessType;
                     enter.AuthCode = lst.AuthCode;
                     enter.SectionCode = lst.SectionCode;
                     enter.BankType = lst.BankType;
                     enter.CreateTm = DateTime.Now;
                     dbEnter.T_Margin.AddObject(enter);
                     if (!rtn)
                     {
                         rtn = true;
                     }
                 }
                 else
                 {
                     //if (chk.Match == null || (chk.Match != null&&chk.Match==0))//未匹配成功  可以修改
                     //{
                     //    chk.InAcct = lst.InAcct;
                     //    chk.InAmount = lst.InAmount;
                     //    chk.InDate = lst.InDate;
                     //    chk.InMemo = lst.InMemo;
                     //    chk.InName = lst.InName;
                     //    chk.InTime = lst.InTime;

                     //    chk.PunInst = lst.PunInst;
                     //    chk.BackResult = lst.BackResult;
                     //    chk.AddWord = lst.AddWord;
                     //    chk.Gernal = lst.Gernal;

                     //    chk.QYType = lst.QYType;
                     //    chk.AuthCode = lst.AuthCode;
                     //    chk.SectionCode = lst.SectionCode;

                     //    chk.CreateTm = DateTime.Now;
                     //    dbEnter.T_AHQY.ApplyCurrentValues(chk);
                     //    if (!rtn)
                     //    {
                     //        rtn = true;
                     //    } 
                     //}

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
                    LogTxt.WriteEntry("明细入账异常" + ex.Message, "农商行匹配");
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
            var urlStr = ConfigHelper.GetCustomCfg("HSan", "BusinessUrl");
            var enCodingStr = ConfigHelper.GetCustomCfg("HSan", "enCoding");
            var chkStr = ConfigHelper.GetCustomCfg("HSan", "chkStr");//核对值
            var roundCount = Convert.ToInt32(ConfigHelper.GetCustomCfg("QY", "roundCount"));//核对值
            if (string.IsNullOrEmpty(urlStr) || string.IsNullOrEmpty(enCodingStr) || string.IsNullOrEmpty(chkStr) || roundCount <= 0)
            {
                LogTxt.WriteEntry("回调地址或者编码等未设置，请设置", "农商行匹配");
                return;
            }
            var enCoding = Encoding.GetEncoding(enCodingStr);//回调编码
            #endregion

            bool haveMatch = false;//是否匹配 
            var dbEnter = new PM.TaskBiz.ORM.Pub_Entities();
            var matchList = dbEnter.T_Margin.Where(p => (p.Match != 1 || p.Match == null)&&p.BankType.ToLower()=="TRCB".ToLower());//获取匹配表待匹配信息 (获取借的标记   0借  1贷)
            #region 匹配处理  优先规则是订单号匹配到
            foreach (var lst in matchList)//匹配
            {
                var postStr = string.Format(
                     @"PayRealAccountName={0}&PayRealAccountNo={1}&PayRealBankName={2}
                        &ReceiveRealAccountName={3}&ReceiveRealAccountNo={4}&Amount={5}
                        &FeeAmount={6}&PrimaryID={7}&SlaveID={8}&TradeNo={9}
                        &SerialNumber={10}&LoanMark={11}&CostType={12}&PayDateTime={13}&PayDate={14}&PayTime={15}&&BankType={16}",
                    HttpUtility.UrlEncode(lst.InName ?? string.Empty, enCoding)
                    , lst.InAcct.Trim()
                    , string.Empty
                    , HttpUtility.UrlEncode(lst.InName ?? string.Empty, enCoding)
                    , lst.InMemo.Trim()
                    , lst.InAmount
                    , lst.PunInst//利息
                    , lst.SectionCode.Trim()
                    , lst.AuthCode.Trim()
                    , lst.HstSeqNum.Trim()
                    , lst.HstSeqNum.Trim()
                    , lst.BusniessType.Trim()//借 0 贷1
                    , "BZJ"//费用类型
                    , lst.InDate.Trim() + lst.InTime.Trim()
                    , lst.InDate.Trim()
                    , lst.InTime.Trim()
                    ,lst.BankType
                    );
                LogTxt.WriteEntry("回调信息" + urlStr + postStr, "农商行匹配回调");
                if (HttpTransfer.PostBackToBusinesss(postStr, urlStr, enCoding, chkStr, roundCount))//匹配完成
                {
                    lst.Match = 1;//成功  
                    dbEnter.T_Margin.ApplyCurrentValues(lst);
                    haveMatch = true;//初始化匹配状态 
                }
                else
                { 
                    LogTxt.WriteEntry("回调匹配响应失败" + postStr, "农商行匹配回调");
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
                    LogTxt.WriteEntry("订单匹配处理异常" + ex.Message, "农商行匹配回调");
                }
            }
            #endregion
        }
        #endregion
    }
}
