using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.Netbank;
using PM.Utils;
using PM.PaymentManger;

namespace PM.TaskBiz.NetBankTask
{
    public class NetBankQueryAccountCall : ITimerTaskCallBiz
    {
        public void TimerCall()
        {
            // 赋值
            var queryModel = new NetBankQueryStatementListModel();
            queryModel.BusinessFunNo = "1810";
            queryModel.StructCode = ConfigHelper.GetCustomCfg("NetBank", "InstitutionID");
            queryModel.QueryDate = "2013-08-07";// DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var queryList = (NetBankQueryStatementListModel)Manager.PaymentManager(queryModel);
            //回调
            GetCallbackInterface().CallBack(queryList);
        }

        public ITimerTaskCallBack GetCallbackInterface()
        {
            return new  NetBankQueryAccountCallBack();
        }
    }
}
