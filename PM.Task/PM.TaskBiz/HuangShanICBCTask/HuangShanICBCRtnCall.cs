using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.HSICBC;
using PM.PaymentManger;
using PM.Utils;
using PM.PaymentProtocolModel.BankCommModel;

namespace PM.TaskBiz.HuangShanICBC
{
    /// <summary>
    /// 黄山退款明细查询
    /// </summary>
    public class HuangShanICBCRtnCall : ITimerTaskCallBiz
    {
        public void TimerCall()
        {
            var dbEnter = new PM.TaskBiz.ORM.Pub_Entities();
            var sectionList = dbEnter.T_Pay_VirtualAccount.ToList();
            var acctNo = ConfigHelper.GetCustomCfg("HuangShan", "AcctNo");//母账号
            var doWhileCount =int.Parse( ConfigHelper.GetCustomCfg("HuangShan", "doWhileCount"));//分页循环最大次数
            HSICBCRtnQueryAccountDtl queryRtnInfo = null;
            List<ICBCRtnQueryInfo> allRtnList = new List<ICBCRtnQueryInfo>();//获取全部退款明细

            foreach (var section in sectionList)
            {
                var sectionCode = section.SectionId.ToString();
                var ProjectCode = section.Projectid.ToString();
                var authCode = section.SerialKey;
                #region 退款明细
                queryRtnInfo = new HSICBCRtnQueryAccountDtl();
                queryRtnInfo.BusinessFunNo = "";//业务功能号
                queryRtnInfo.AuthCode = section.SerialKey;
                queryRtnInfo.ItemNo = ProjectCode;
                queryRtnInfo.ItemNoX = sectionCode;
                queryRtnInfo.SeqNo = DateTime.Now.ToString("yyyyMMddHHmmss");
                queryRtnInfo.TransDate = DateTime.Now.ToString("yyyyMMdd");
                queryRtnInfo.TransTime = DateTime.Now.ToString("HHmmss");
                queryRtnInfo.TransCode = "6031";
                queryRtnInfo.StartDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");//开始时间
                queryRtnInfo.EndDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");//结束时间
                queryRtnInfo.StartNum = "";//其实笔数
                queryRtnInfo.EndNum = "50";//默认50
                queryRtnInfo.AcctNo = acctNo;//母账号
                HSICBCQueryRtnResultModel queryRtnList = (HSICBCQueryRtnResultModel)(Manager.PaymentManager(queryRtnInfo));
                if (null != queryRtnList && null != queryRtnList.ICBCRtnQueryList && queryRtnList.Result == "1" && queryRtnList.ICBCRtnQueryList.Count > 0)
                {
                    SetList(allRtnList, queryRtnList.ICBCRtnQueryList, sectionCode, authCode);
                    int curStartNum = int.Parse(queryRtnList.CurStartNum);
                    int curQueryNum = int.Parse(queryRtnList.CurQueryNum);
                    int queryTotalNum = int.Parse(queryRtnList.QueryTotalNum);
                    int i = 0;//防止死循环
                    //分页  
                    while (curStartNum + curQueryNum <= queryTotalNum)
                    {
                        queryRtnInfo.StartNum = (curStartNum + curQueryNum).ToString();
                        queryRtnList = (HSICBCQueryRtnResultModel)(Manager.PaymentManager(queryRtnInfo));
                        if (null != queryRtnList && null != queryRtnList.ICBCRtnQueryList && queryRtnList.Result == "1" && queryRtnList.ICBCRtnQueryList.Count > 0)
                        {
                            SetList(allRtnList, queryRtnList.ICBCRtnQueryList, sectionCode, authCode);
                            curStartNum = int.Parse(queryRtnList.CurStartNum);
                            curQueryNum = int.Parse(queryRtnList.CurQueryNum);
                        }
                        i++;
                        if (i > doWhileCount)//防止死循环
                        {
                            break;
                        }
                    }

                }
                #endregion
            }


            //处理
            if (allRtnList.Count > 0)
            {
                //回调 
                GetCallbackInterface().CallBack(allRtnList);
            }

        }

        public ITimerTaskCallBack GetCallbackInterface()
        {
            return new HuangShanICBCRtnCallBack();
        }

        #region    private
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="rtnContains">容器</param>
        /// <param name="queryRtn">查询数据</param>
        /// <param name="sectionCode">标段编码</param>
        /// <param name="authCode">授权码</param>
        private void SetList(List<ICBCRtnQueryInfo> rtnContains, List<ICBCRtnQueryInfo> queryRtn, string sectionCode, string authCode)
        {
            Array.ForEach(queryRtn.ToArray(), p =>
                       {
                           p.BusniessType = "1";
                           p.SectionCode = sectionCode;//标段code  
                           p.AuthCode = authCode;
                           p.BankType = "ICBC";
                       }
                        );
            if (queryRtn.Count > 0)
            {
                queryRtn.ForEach(
                    p =>
                    {
                        if (rtnContains.Contains(p))
                        {
                            rtnContains.Add(p);
                        }
                    }
                    );
            }
        }

        #endregion
    }
}
