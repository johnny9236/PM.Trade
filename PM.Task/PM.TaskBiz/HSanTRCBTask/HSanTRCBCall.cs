using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.AHQY;
using PM.PaymentManger;
using PM.PaymentProtocolModel.BankCommModel.HSanTRCB;

namespace PM.TaskBiz.HSanTRCBTask
{
    public class HSanTRCBCall : ITimerTaskCallBiz
    {
        public void TimerCall()
        {
            var dbEnter = new PM.TaskBiz.ORM.Pub_Entities();
            var sectionList = dbEnter.T_Pay_VirtualAccount.Where(p => p.Status == "0").ToList();

            HSanTRCBQueryOrRtnQueryAccountDtl queryInfo = null;
            foreach (var section in sectionList)
            {
                var sectionCode = section.SectionId.ToString();
                var projectCode = section.Projectid.ToString();
                var authCode = section.SerialKey;
                #region
                // 入账明细
                queryInfo = new HSanTRCBQueryOrRtnQueryAccountDtl();
                queryInfo.BusinessFunNo = "HSanTRCBBzjDtl";
                queryInfo.AuthCode = section.SerialKey;
                queryInfo.ItemNo = projectCode;
                queryInfo.ItemNoX = sectionCode;
                queryInfo.SeqNo = DateTime.Now.ToString("yyyyMMddHHmmss");
                queryInfo.TransDate = DateTime.Now.ToString("yyyyMMdd");
                queryInfo.TransTime = DateTime.Now.ToString("HHmmss");
                queryInfo.TransCode = "3011";
                var queryList = (HSanTRCBQueyResultModel)(Manager.PaymentManager(queryInfo));
                if (null != queryList && null != queryList.TRCBQueryList)
                {
                    Array.ForEach(queryList.TRCBQueryList.ToArray(), p =>
                             {
                                 p.BusniessType = "0";
                                 p.SectionCode = sectionCode;//标段code 
                                 p.AuthCode = authCode;
                                 p.BankType = "TRCB";
                             }
                             );
                    //回调 
                    GetCallbackInterface().CallBack(queryList.TRCBQueryList);
                }

                //退款明细
                queryInfo = new HSanTRCBQueryOrRtnQueryAccountDtl();
                queryInfo.BusinessFunNo = "HSanTRCBBZJRTNDtl";
                queryInfo.AuthCode = section.SerialKey;
                queryInfo.ItemNo = projectCode;
                queryInfo.ItemNoX = sectionCode;
                queryInfo.SeqNo = DateTime.Now.ToString("yyyyMMddHHmmss");
                queryInfo.TransDate = DateTime.Now.ToString("yyyyMMdd");
                queryInfo.TransTime = DateTime.Now.ToString("HHmmss");
                queryInfo.TransCode = "3051";
                var queryRtnList = (HSanTRCBQueryRtnResultModel)(Manager.PaymentManager(queryInfo));
                if (null != queryRtnList && null != queryRtnList.TRCBRtnQueryList)
                {
                    Array.ForEach(queryRtnList.TRCBRtnQueryList.ToArray(), p =>
                       {
                           p.BusniessType = "1";
                           p.SectionCode = sectionCode;//标段code 
                           p.AuthCode = authCode;
                           p.BankType = "TRCB";
                       }
                        );
                    //回调 
                    GetCallbackInterface().CallBack(queryRtnList.TRCBRtnQueryList);
                }
                #endregion
            }
        }

        /// <summary>
        /// 获取回调实例
        /// </summary>
        /// <returns></returns>
        public ITimerTaskCallBack GetCallbackInterface()
        {
            return new HSanTRCBCallBack();
        }
    }
}
