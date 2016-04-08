using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.HuangShi;
using PM.Utils;
using PM.PaymentManger;

namespace PM.TaskBiz.HuangShiCCBTask
{
    /// <summary>
    /// 调用任务
    /// </summary>
    public class HuangShiCCBCall : ITimerTaskCallBiz
    {
        public void TimerCall()
        {
            var hsModel = new HuangShiDepositQueryModel();
            hsModel.BusinessFunNo = "HuangShiMatch";
            hsModel.LANGUAGE = "CN";
            hsModel.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmsss");
            hsModel.CUST_ON = ConfigHelper.GetCustomCfg("HS", "CUST_ON");
            hsModel.OprationerID = ConfigHelper.GetCustomCfg("HS", "USER_ID");
            hsModel.PASSWORD = ConfigHelper.GetCustomCfg("HS", "PASSWORD");
            hsModel.TX_CODE = ConfigHelper.GetCustomCfg("HS", "TX_CODE");

            hsModel.ACCOUNT = ConfigHelper.GetCustomCfg("HS", "ACCOUNT");
            hsModel.START = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            hsModel.END = DateTime.Now.ToString("yyyyMMdd");
            int page = 1;
            int.TryParse(ConfigHelper.GetCustomCfg("HS", "PAGE"), out page);
            hsModel.PAGE = page;

            var responseModel = (HuangShiDepositResponseModel)Manager.PaymentManager(hsModel);

            //回调
            GetCallbackInterface().CallBack(responseModel);
        }
        /// <summary>
        /// 回调对象实例化
        /// </summary>
        /// <returns></returns>
        public ITimerTaskCallBack GetCallbackInterface()
        {
            return new HuangShiCCBCallBack();
        }
    }
}
