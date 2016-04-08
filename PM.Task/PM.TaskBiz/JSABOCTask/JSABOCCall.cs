using System;
using System.Collections.Generic;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.JSABOC;
using PM.PaymentManger;
using PM.TaskBiz.JSABOCTask.ORM;
using System.Data.Objects;
using System.Linq;
using PM.Utils.Log;


namespace PM.TaskBiz.JSABOCTask
{
    /// <summary>
    /// 嘉善农行 入账明细处理
    /// </summary>
    public class JSABOCCall : ITimerTaskCallBiz
    {
        public void TimerCall()
        {
            List<JSABOCRtnModel> allList = new List<JSABOCRtnModel>();
            // 赋值
            JSABOCQueryAccountDtl queryModel = new JSABOCQueryAccountDtl();
            queryModel.BusinessFunNo = "ZTB1";
            queryModel.TradeStructNum = "001";
            queryModel.DetailDataTime =  DateTime.Now.ToString("yyyyMMdd");// "20130105";//
            List<JSABOCRtnModel> queryList = (List<JSABOCRtnModel>)Manager.PaymentManager(queryModel);
            if (queryList != null && queryList.Count() > 0)
            {
                queryList.ForEach(p => p.AccountType = "bzj");
                allList.AddRange(queryList);
            }

            //////////////////////////////////////////////////////////////////////////
            queryList = null;
            queryModel = new JSABOCQueryAccountDtl();
            queryModel.BusinessFunNo = "ZTB1";
            queryModel.TradeStructNum = "002";
            queryModel.DetailDataTime =  DateTime.Now.ToString("yyyyMMdd");// "20130105";//
            queryList = (List<JSABOCRtnModel>)Manager.PaymentManager(queryModel);
            if (queryList != null && queryList.Count() > 0)
            {
                queryList.ForEach(p => p.AccountType = "qt");
                allList.AddRange(queryList);
            }
            //回调
            GetCallbackInterface().CallBack(allList);
        }
        /// <summary>
        /// 获取回调实例
        /// </summary>
        /// <returns></returns>
        public ITimerTaskCallBack GetCallbackInterface()
        {
            return new JSABOCCallBack();
        } 
    }
}
