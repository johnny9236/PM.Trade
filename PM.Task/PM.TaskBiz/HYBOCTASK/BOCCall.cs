using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentManger;
using PM.PaymentProtocolModel.BankCommModel.BOC;
using PM.Utils;
using PM.Utils.Log;

namespace PM.TaskBiz.HYBOCTASK
{
    /// <summary>
    /// 海盐
    /// </summary>
    public class BOCCall : ITimerTaskCallBiz
    {
        private readonly string IbkNum = ConfigHelper.GetCustomCfg("HYBOC", "IbkNum");//联号
        private readonly string Termid = ConfigHelper.GetCustomCfg("HYBOC", "Termid");//企业前置机
        private readonly string CusOpr = ConfigHelper.GetCustomCfg("HYBOC", "CusOpr");//企业操作员代码
        private readonly string CustId = ConfigHelper.GetCustomCfg("HYBOC", "CustId");//企业在中行网银系统的客户编码
        private readonly string OprPwd = ConfigHelper.GetCustomCfg("HYBOC", "OprPwd");//企业在中行网银系统的客户编码

        public void TimerCall()
        {

            string token = string.Empty;
            if (!SingIn(ref token))
            {
                LogTxt.WriteEntry("签到失败:" + token, "中国银行签到协议报文");
                return; //签到失败          
            }
            Call(false, token);// 历史
            //Call(true, token);//当日

            //签退
            SignOut(token);
        }
        /// <summary>
        /// 获取回调实例
        /// </summary>
        /// <returns></returns>
        public ITimerTaskCallBack GetCallbackInterface()
        {
            return new BOCCallBack();
        }
        #region  private
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="IsToday">是否是当前时间</param>
        /// <param name="token">令牌</param>
        private void Call(bool IsToday, string token)
        {
            #region  业务处理
            var pageRecordCount = 50;
            var accountNo = PM.Utils.ConfigHelper.GetCustomCfg("HYBOC", "ACCNO");//主账号
            List<BOCQueryAccountDtlModel> queryList = new List<BOCQueryAccountDtlModel>();//总记录数

            #region  非当日查询 日期处理  去当前月中 小于10天的记录
            var dateTo = IsToday == true ? DateTime.Now : DateTime.Now.AddDays(-1);
            var dateFrom_temp = DateTime.Now.AddDays(-9);
            var dateFrom = IsToday == true ? DateTime.Now : (12 * (dateTo.Year - dateFrom_temp.Year) + (dateTo.Month - dateFrom_temp.Month) > 0 ? (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)) : dateFrom_temp);
            #endregion

            //入账明细  分页处理
            var requestQuery = new BOCQueryAccountDtl();
            requestQuery.BusinessFunNo = "HaiYanBOCDtl";
            requestQuery.Type = IsToday == true ? "2001" : "2002";//历史查询
            requestQuery.DatescopeFrom = dateFrom.ToString("yyyyMMdd");
            requestQuery.DatescopeTo = dateTo.ToString("yyyyMMdd");
            requestQuery.RecNum = pageRecordCount.ToString();
            requestQuery.Actacn = accountNo;//账号
            requestQuery.Trnid = DateTime.Now.ToString("yyMMddHHmmss");
            requestQuery.Direction = "1";//来账
            requestQuery.IbkNum = IbkNum;
            requestQuery.Termid = Termid;
            requestQuery.CusOpr = CusOpr;
            requestQuery.CustId = CustId;

            requestQuery.Token = token;
            var queryRes = (BOCQueryAccountDtlResult)(Manager.PaymentManager(requestQuery));
            if (!string.IsNullOrEmpty(queryRes.RspCod) && (queryRes.RspCod.ToLower() == "B001".ToLower() || queryRes.RspCod.ToLower() == "B002".ToLower() && queryRes.AccountDtlList.Count > 0))//成功
            {
                var queryList_temp = new List<BOCQueryAccountDtlModel>();//总记录数
                queryList_temp.AddRange(queryRes.AccountDtlList.FindAll(p => p.RspCod.ToLower() == "B001".ToLower()));//添加成功的记录
                #region 分页
                var remain = queryRes.NoteNum % pageRecordCount > 0 ? 1 : 0;//判断页数是否需要加1  根据余数  
                var forCount = queryRes.NoteNum / pageRecordCount + remain;
                for (int i = 1; i < forCount; i++)
                {
                    if (null != queryList_temp && (queryList_temp.Count < queryRes.NoteNum))
                    {
                        requestQuery.BegNum = (queryList_temp.Count + 1).ToString();
                        queryRes = (BOCQueryAccountDtlResult)(Manager.PaymentManager(requestQuery));
                        if (!string.IsNullOrEmpty(queryRes.RspCod) && (queryRes.RspCod.ToLower() == "B001".ToLower() || queryRes.RspCod.ToLower() == "B002".ToLower() && queryRes.AccountDtlList.Count > 0))//成功
                        {
                            queryList_temp.AddRange(queryRes.AccountDtlList.FindAll(p => p.RspCod.ToLower() == "B001".ToLower()));//添加成功的记录
                        }
                    }
                }
                if (queryList_temp.Count > 0)
                {
                    queryList.AddRange(queryList_temp);
                }
                #endregion
            }

            if (queryList.Count > 0)
            {
                //回调 
                GetCallbackInterface().CallBack(queryList);
            }
            #endregion
        }
        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        private bool SingIn(ref string token)
        {
            token = string.Empty;
            #region 签到
            var signIn = new BOCSignInRequest();
            signIn.BusinessFunNo = "HaiYanBOCSignIn";
            signIn.Trnid = DateTime.Now.ToString("yyMMddHHmmss");
            //signIn.TradeDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            signIn.CusOpr = CusOpr;
            signIn.Termid = Termid;
            signIn.CustId = CustId;
            signIn.OprPwd = OprPwd;
            signIn.CustDt = DateTime.Now.ToString("yyyyMMddHHmmss");
            var singInResult = (BOCSignInResponse)(Manager.PaymentManager(signIn));
            #endregion
            if (null != singInResult && singInResult.RspCod.ToLower() == "B001".ToLower())
            {
                token = singInResult.Token;
                if (!string.IsNullOrEmpty(token))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 签退
        /// </summary>
        /// <param name="token">令牌</param>
        private void SignOut(string token)
        {
            var signOut = new BOCSignOUtRequset();
            signOut.BusinessFunNo = "HaiYanBOCSignOut";
            signOut.CusOpr = CusOpr;
            signOut.Termid = Termid;
            signOut.CustId = CustId;
            signOut.Token = token;
            signOut.Trnid = DateTime.Now.ToString("yyMMddHHmmss");
            signOut.CustDt = DateTime.Now.ToString("yyyyMMddHHmmss");
            var singOutResult = (BOCSignOUtResponse)(Manager.PaymentManager(signOut));
            if (null != singOutResult && singOutResult.RspCod.ToLower() == "B001".ToLower())
            {
            }
            else
            {
                LogTxt.WriteEntry("签到失败:" + token, "中国银行签退协议报文");
            }
        }
        #endregion
    }
}
