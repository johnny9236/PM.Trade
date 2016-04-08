using System;
using System.Collections.Generic;
using System.Text;
using PM.TaskBizInterface;
using PM.Utils.Log;
using PM.PaymentProtocolModel.BankCommModel.JHBOF;
using PM.PaymentManger;
using PM.TaskBiz.JHBOFTask.ORM;
using System.Linq;
using PM.Utils;

namespace PM.TaskBusiness.JHBOFTask
{
    public class JHBOFCall : ITimerTaskCallBiz
    {
        /// <summary>
        /// 调用任务
        /// </summary>
        public void TimerCall()
        {
            var queryInfo = new JHBOFQueryPayListModel();
            queryInfo.AccNo = ConfigHelper.GetCustomCfg("JH", "AcctNo");
            queryInfo.BusinessFunNo = "JHBOF";
        
            queryInfo.StartDate = DateTime.Now.AddDays(-15).ToString("yyyyMMdd");
            queryInfo.EndDate = DateTime.Now.ToString("yyyyMMdd"); 
            queryInfo.Use = "";//用途 
            //LogTxt.WriteEntry("TimerCall" + DateTime.Now.ToString("yyyyMMdd HHmmss"), "JHBOFCall");
            var queryList = (List<JHBofQueryResult>)(Manager.PaymentManager(queryInfo));

            //回调 
            GetCallbackInterface().CallBack(queryList);
        }
        /// <summary>
        /// 获取回调实例
        /// </summary>
        /// <returns></returns>
        public ITimerTaskCallBack GetCallbackInterface()
        {
            return new JHBOFCallBack();
        }
    }
}
