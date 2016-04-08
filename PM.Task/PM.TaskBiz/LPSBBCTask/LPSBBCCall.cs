using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.LPSBBC;
using PM.Utils;
using PM.PaymentManger;

namespace PM.TaskBiz.LPSBBCTask
{
    /// <summary>
    /// 六盘水建行 对账单
    /// </summary>
    public class LPSBBCCall : ITimerTaskCallBiz
    {
        public void TimerCall()
        {
            var queryInfoList = new List<BBCQueryAccountRtnModel>();//入账信息
            // 赋值
            var queryModel = new BBCQuery();
            queryModel.BusinessFunNo = "LPSBBCDSearch";
            queryModel.MERCHANTID = ConfigHelper.GetCustomCfg("LPS", "MERCHANTIDB2B");
            queryModel.POSID = ConfigHelper.GetCustomCfg("LPS", "MERCHANTIDB2B");
            queryModel.BRANCHID = ConfigHelper.GetCustomCfg("LPS", "MERCHANTIDB2B");
            queryModel.QUPWD = ConfigHelper.GetCustomCfg("LPS", "MERCHANTIDB2B");
            queryModel.TYPE = "0";//支付流水
            queryModel.TXCODE = "410408";
            queryModel.KIND = "1";//已结算流水
            queryModel.SEL_TYPE = "3";//XML页面形式
            queryModel.PAGE = "1";
            queryModel.ORDERDATE = DateTime.Now.ToString("yyyyMMdd");
            queryModel.QUPWD= ConfigHelper.GetCustomCfg("LPS", "QueryPWD");
               
            
            #region   查询成功的记录
            queryModel.STATUS = "1";//成功 //   失败  测试时候使用  分页
            BBCQueryRtn queryList = (BBCQueryRtn)Manager.PaymentManager(queryModel);
            if (null != queryList && queryList.RETURN_CODE == "000000")//操作成功
            {
                if (null != queryList && null != queryList.BBCQueryAccountList && queryList.BBCQueryAccountList.Count > 0)
                {
                    queryInfoList.AddRange(queryList.BBCQueryAccountList);
                }

                //分页
                if (null != queryList && (queryList.CURPAGE < queryList.PAGECOUNT))
                {
                    for (var i = 2; i <= queryList.PAGECOUNT; i++)
                    {
                        queryModel.PAGE = i.ToString();
                        queryList = (BBCQueryRtn)Manager.PaymentManager(queryModel);
                        if (queryList.RETURN_CODE == "000000")//操作成功
                        {
                            if (null != queryList && null != queryList.BBCQueryAccountList && queryList.BBCQueryAccountList.Count > 0)
                            {
                                queryInfoList.AddRange(queryList.BBCQueryAccountList);
                            }
                        }
                    }
                }
            }
            #endregion

            #region   查询成功的记录
            queryModel.STATUS = "0";//成功 //   
            queryList = (BBCQueryRtn)Manager.PaymentManager(queryModel);
            if (queryList.RETURN_CODE == "000000")//操作成功
            {
                if (null != queryList && null != queryList.BBCQueryAccountList && queryList.BBCQueryAccountList.Count > 0)
                {
                    queryInfoList.AddRange(queryList.BBCQueryAccountList);
                }

                //分页
                if (null != queryList && (queryList.CURPAGE < queryList.PAGECOUNT))
                {
                    for (var i = 2; i <= queryList.PAGECOUNT; i++)
                    {
                        queryModel.PAGE = i.ToString();
                        queryList = (BBCQueryRtn)Manager.PaymentManager(queryModel);
                        if (queryList.RETURN_CODE == "000000")//操作成功
                        {
                            if (null != queryList && null != queryList.BBCQueryAccountList && queryList.BBCQueryAccountList.Count > 0)
                            {
                                queryInfoList.AddRange(queryList.BBCQueryAccountList);
                            }
                        }
                    }
                }
            }
            #endregion
            //回调
            GetCallbackInterface().CallBack(queryList);
        }

        public ITimerTaskCallBack GetCallbackInterface()
        {
            return new LPSBBCCallBack();
        }
    }
}
