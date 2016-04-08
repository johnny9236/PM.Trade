using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.HSICBC;
using PM.PaymentManger;

namespace PM.TaskBiz.HuangShanICBC
{
    /// <summary>
    /// 黄山工商银行入账明细查询
    /// </summary>
    public class HuangShanICBCCall : ITimerTaskCallBiz
    {
        public void TimerCall()
        {
            var dbEnter = new PM.TaskBiz.ORM.Pub_Entities();
            var sectionList = dbEnter.T_Pay_VirtualAccount.ToList();

            HSICBCQueryAccountDtl queryInfo = null;
            foreach (var section in sectionList)
            {
                var sectionCode = section.SectionId.ToString();
                var ProjectCode = section.Projectid.ToString();
                var authCode = section.SerialKey;

                #region  入账明细
                queryInfo = new HSICBCQueryAccountDtl();
                queryInfo.BusinessFunNo = "";//功能号
                queryInfo.AuthCode = section.SerialKey;

                queryInfo.TransCode = "3011";
                queryInfo.SeqNo = DateTime.Now.ToString("yyyyMMddHHmmss");
                queryInfo.TransDate = DateTime.Now.ToString("yyyyMMdd");
                queryInfo.TransTime = DateTime.Now.ToString("HHmmss");
                queryInfo.ItemNo = ProjectCode;
                queryInfo.ItemNoX = sectionCode;

                var queryList = (HSICBCQueyResultModel)(Manager.PaymentManager(queryInfo));
                if (null != queryList && null != queryList.ICBCQueryList)
                {
                    Array.ForEach(queryList.ICBCQueryList.ToArray(), p =>
                             {
                                 p.BusniessType = "0";
                                 p.SectionCode = sectionCode;//标段code 
                                 p.AuthCode = authCode;
                                 p.BankType = "ICBC";
                             }
                             );
                    if (queryList.ICBCQueryList.Count > 0)
                    {
                        //回调 
                        GetCallbackInterface().CallBack(queryList.ICBCQueryList);
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// 回调对象
        /// </summary>
        /// <returns></returns>
        public ITimerTaskCallBack GetCallbackInterface()
        {
            return new HuangShanICBCCallBack();
        }
    }
}
