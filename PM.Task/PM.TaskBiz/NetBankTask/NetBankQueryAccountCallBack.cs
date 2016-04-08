using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.Netbank;
using PM.TaskBiz.NetBankTask.ORM;
using PM.Utils.Log;

namespace PM.TaskBiz.NetBankTask
{
    /// <summary>
    /// 回调处理
    /// </summary>
    public class NetBankQueryAccountCallBack : ITimerTaskCallBack
    {
        public void CallBack(dynamic dyObj)
        {
            //  增量入库操作
            var haveIn = IncrementToDB(dyObj);
            //回调
            //  if (haveIn)  //控制暂时取消
            //匹配操作
            Match();
        }
        #region
        /// <summary>
        /// 增量入库
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        private bool IncrementToDB(NetBankQueryStatementListModel model)
        {
            bool rtn = false;
            if (!model.Result)
                return false;
            T_NetBankAccountQuery enter = null;
            var dbEnter = new PM.TaskBiz.NetBankTask.ORM.Netbank_IntegratedEntities();
            var dbList = dbEnter.T_NetBankAccountQuery;           //获取数据库内容
            foreach (var lst in model.QueryResult)//获取
            {
                var chk = dbList.FirstOrDefault(p => p.TxSn.ToLower() == lst.TxSn.ToLower());//订单号匹配
                if (chk == null)//增量添加
                {
                    enter = new T_NetBankAccountQuery();
                    enter.ID = Guid.NewGuid().ToString();
                    enter.TxType = lst.TxType;
                    enter.TxSn = lst.TxSn;
                    enter.TxAmount = lst.TxAmount.ToString();
                    enter.InstitutionAmount = lst.InstitutionAmount.ToString();
                    enter.PaymentAmount = lst.PaymentAmoun.ToString();
                    enter.InstitutionFee = lst.InstitutionFee.ToString();
                    enter.Remark = lst.Remark;
                    enter.BankNotificationTime = lst.BankNotificationTime;
                    enter.SettlementFlag = lst.SettlementFlag;

                    enter.CreateTm = DateTime.Now;
                    dbEnter.T_NetBankAccountQuery.AddObject(enter);
                    if (!rtn)
                    {
                        rtn = true;
                    }
                }
            }
            if (rtn)
            {
                try
                {
                    dbEnter.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry("明细入账异常" + ex.Message, "六盘水银联查询");
                    rtn = false;
                }
            }
            return rtn;
        }
        /// <summary>
        /// 匹配操作
        /// </summary>
        private void Match()
        {
            bool haveMatch = false;//是否匹配 
            decimal Amount = 0;
            var dbEnter = new PM.TaskBiz.NetBankTask.ORM.Netbank_IntegratedEntities();
            var matchList = dbEnter.T_NetBankAccountQuery.Where(p => (p.IsMatch != 1 || p.IsMatch == null));//获取匹配表待匹配信息
            // var dbList = dbEnter.T_ZTB_MoneyPayment.Where(p => (p.IsCheck != 2 || p.IsCheck != 3 || p.IsCheck == null) );//入账表对应信息
            #region 匹配处理  优先规则是订单号匹配到
            foreach (var lst in matchList)//匹配
            {
                //var tradeNoStr = GetDbcNumStr(lst.ORDERID).ToLower();
                var chk = dbEnter.T_ZTB_MoneyPayment.FirstOrDefault(p => p.Out_trade_no.ToLower() == lst.TxSn.ToLower());//根据订单号
                if (null != chk)//匹配到订单
                {
                    decimal.TryParse(lst.InstitutionAmount, out Amount);
                    if (chk.PayMoney == Amount)//匹配完成
                    {
                        lst.IsMatch = 1;//成功 
                        chk.IsCheck = 2;//入账成功

                        //chk.ma = string.Empty;
                        DateTime dt = DateTime.MinValue;
                        DateTime.TryParseExact(lst.BankNotificationTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None,
                  out  dt);
                        //chk.PayTime = dt;// 入账时间
                        dbEnter.T_NetBankAccountQuery.ApplyCurrentValues(lst);
                        dbEnter.T_ZTB_MoneyPayment.ApplyCurrentValues(chk);
                        if (!haveMatch)
                            haveMatch = true;//初始化匹配状态
                    }
                    else//订单号匹配成功  账号或金额匹配不成功
                    {
                        // chk.mar = "金额不匹配";                       
                        dbEnter.T_ZTB_MoneyPayment.ApplyCurrentValues(chk);
                        if (!haveMatch)
                            haveMatch = true;//初始化匹配状态
                    }
                }
            }
            if (haveMatch)//先保存一次
            {
                try
                {
                    dbEnter.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry("订单匹配处理异常" + ex.Message, "六盘水交行查询");
                }
            }
            #endregion
            #region  未匹配到订单号规则   只匹配账号(    是否需要 待定)
            //haveMatch = false;//初始化匹配状态
            //T_ZTB_MoneyPayment chkModel = null;
            //var newMatchLst = matchList.Where(p => (p.IsMatch != 1 || p.IsMatch == null));//获取未匹配成功的记录
            //foreach (var lst in newMatchLst)//匹配
            //{
            //    chkModel = dbEnter.T_ZTB_MoneyPayment.FirstOrDefault(p => p.PayAccNo == lst.PayAccNo && p.PayAccDbName.Trim() == p.PayAccDbName.Trim() && p.PayMoney == lst.Amount);//如金额与账号匹配   订单号不匹配（是否需处理）

            //    if (null != chkModel)
            //    {
            //        lst.IsMatch = 1;//成功
            //        chkModel.IsCheck = 2;//入账成功
            //        DateTime dt = DateTime.MinValue;
            //        DateTime.TryParseExact(lst.DetailDataTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None,
            //        out  dt);
            //        chkModel.IincurredTime = dt;// 入账时间
            //        dbEnter.T_JSABOC.ApplyCurrentValues(lst);
            //        dbEnter.T_ZTB_BidMoneyPayReturn.ApplyCurrentValues(chkModel);
            //        if (!haveMatch)
            //            haveMatch = true;
            //    }
            //}
            //if (haveMatch)
            //{
            //    try
            //    {
            //        dbEnter.SaveChanges();
            //    }
            //    catch (Exception ex)
            //    {
            //        LogTxt.WriteEntry("匹配处理异常" + ex.Message, "嘉善农行查询");
            //    }
            //}
            #endregion
        }
        #endregion
        #region private
        /// <summary>
        /// 订单号转半角字符
        /// </summary>
        /// <param name="inputStr">输入字符</param>
        /// <returns></returns>
        private string GetDbcNumStr(string inputStr)
        {
            string rtnStr = string.Empty;
            try
            {
                var sourceStr = PM.Utils.StringHelper.ToDBC(inputStr);
                sourceStr = sourceStr.Replace(" ", "");
                rtnStr = PM.Utils.StringHelper.GetNumberString(sourceStr);
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("订单号转半角处理异常" + ex.Message, "嘉善农行查询");
            }
            return rtnStr;
        }
        #endregion
    }
}
