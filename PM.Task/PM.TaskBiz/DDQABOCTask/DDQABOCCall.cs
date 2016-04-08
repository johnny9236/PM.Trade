using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.DDQABOC;
using PM.PaymentManger;
using PM.Utils;

namespace PM.TaskBiz.DDQABOCTask
{
    /// <summary>
    /// 躲刀区 农行 入账明细处理
    /// </summary>
   public  class DDQABOCCall:ITimerTaskCallBiz
    { 
        public void TimerCall()
        {
            //入账明细
            DDQAccountQuery queryInfo = new DDQAccountQuery();
            queryInfo.BusinessFunNo = "DDQBzjDtl";
            queryInfo.AuthNo = "";
            queryInfo.DbAccNo = ConfigHelper.GetCustomCfg("DDQ", "Account");//借方账号
            // queryInfo.DbCur = "";//货币号
            queryInfo.DbProv = "湖北省";
            queryInfo.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmss");

            queryInfo.StartDate = DateTime.Now.AddDays(-15).ToString("yyyyMMdd");
            queryInfo.EndDate = DateTime.Now.ToString("yyyyMMdd");
            var queryList = (List<DDQAccountDtl>)(Manager.PaymentManager(queryInfo));

            if (null != queryList && queryList.Count > 0)
                //Array.ForEach(queryList.BBCQueryList.ToArray(), p => p.QYType = "0");
                //回调 
                GetCallbackInterface().CallBack(queryList);
        }

        public ITimerTaskCallBack GetCallbackInterface()
        {
            return new DDQABOCCallBack();
        }
    }
}
