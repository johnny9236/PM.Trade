using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.HuangMeiPostal;
using PM.Utils;
using PM.PaymentManger;

namespace PM.TaskBiz.HuangMeiPostlTask
{
    /// <summary>
    /// 任务调用
    /// </summary>
    public class HuangMeiPostlCall : ITimerTaskCallBiz
    {
        public void TimerCall()
        { 
            var queryModel = new HuangMeiQuery();
            queryModel.BusinessFunNo = "HuangMeiMatch";
            queryModel.MercCode = ConfigHelper.GetCustomCfg("HM", "MercCode");
            queryModel.AcctNo = ConfigHelper.GetCustomCfg("HM", "AcctNo");
            queryModel.BeginDate = DateTime.Now.AddDays(-5).ToString("yyyyMMdd");
            queryModel.EndDate = DateTime.Now.ToString("yyyyMMdd");
            //调用
            var queryList = (List<HuangMeiQueryResult>)Manager.PaymentManager(queryModel);
            //回调
            GetCallbackInterface().CallBack(queryList);
        }
        /// <summary>
        /// 回调对象实例化
        /// </summary>
        /// <returns></returns>
        public ITimerTaskCallBack GetCallbackInterface()
        {
            return new HuangMeiPostlCallBack();
        }
    }
}
