using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.AHQY;
using PM.PaymentManger;

namespace PM.TaskBiz.AHQYBBCTask
{
    public class QYBBCCall : ITimerTaskCallBiz
    {
        public void TimerCall()
        {
            var dbEnter = new PM.TaskBiz.ORM.Pub_Entities();
            var sectionList = dbEnter.T_Pay_VirtualAccount.Where(p=>p.Status=="0").ToList();

            QYBBCQueryOrRtnQueryAccountDtl queryInfo = null;
            foreach (var section in sectionList)
            {                 
                var sectionCode = section.SectionId.ToString();
                var projectCode = section.Projectid.ToString();
                var authCode = section.SerialKey;
                #region
               // 入账明细
                queryInfo = new QYBBCQueryOrRtnQueryAccountDtl();
                queryInfo.BusinessFunNo = "QYBzjDtl";
                queryInfo.AuthCode = section.SerialKey;
                queryInfo.ItemNo = projectCode;
                queryInfo.ItemNoX = sectionCode;
                queryInfo.SeqNo = DateTime.Now.ToString("yyyyMMddHHmmss");
                queryInfo.TransDate = DateTime.Now.ToString("yyyyMMdd");
                queryInfo.TransTime = DateTime.Now.ToString("HHmmss");
                queryInfo.TransCode = "3011";
                var queryList = (QYBBCQueyResultModel)(Manager.PaymentManager(queryInfo));
                if (null != queryList && null != queryList.BBCQueryList)
                {
                    Array.ForEach(queryList.BBCQueryList.ToArray(), p =>
                             {
                                 p.BusniessType = "0";
                                 p.SectionCode = sectionCode;//标段code 
                                 p.AuthCode = authCode;
                                 p.BankType = "BBC";
                             }
                             );
                    //回调 
                    GetCallbackInterface().CallBack(queryList.BBCQueryList);
                }

                //退款明细
                queryInfo = new QYBBCQueryOrRtnQueryAccountDtl();
                queryInfo.BusinessFunNo = "QYBZJRTNDtl";
                queryInfo.AuthCode = section.SerialKey;
                queryInfo.ItemNo = projectCode;
                queryInfo.ItemNoX = sectionCode;
                queryInfo.SeqNo = DateTime.Now.ToString("yyyyMMddHHmmss");
                queryInfo.TransDate = DateTime.Now.ToString("yyyyMMdd");
                queryInfo.TransTime = DateTime.Now.ToString("HHmmss");
                queryInfo.TransCode = "3051";
                var queryRtnList = (QYBBCQueryRtnResultModel)(Manager.PaymentManager(queryInfo));
                if (null != queryRtnList && null != queryRtnList.BBCRtnQueryList)
                {
                    Array.ForEach(queryRtnList.BBCRtnQueryList.ToArray(), p =>
                       {
                           p.BusniessType = "1";
                           p.SectionCode = sectionCode;//标段code 
                           p.AuthCode = authCode;
                           p.BankType = "BBC";
                       }
                        );
                    //回调 
                    GetCallbackInterface().CallBack(queryRtnList.BBCRtnQueryList);
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
            return new AHQYBBCTask.QYBBCCallBack();
        }
    }
}
