using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.LPSBBC;
using PM.TaskBiz.LPSBBCTask.ORM;
using PM.Utils.Log;

namespace PM.TaskBiz.LPSBBCTask
{
    /// <summary>
    /// 六盘水入账对比
    /// </summary>
    public class LPSBBCCallBack : ITimerTaskCallBack
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
        private bool IncrementToDB(List<BBCQueryAccountRtnModel> modelList)
        {
            bool rtn = false;

            T_LPSBBC enter = null;
            var dbEnter = new PM.TaskBiz.LPSBBCTask.ORM.PM_IntegratedEntities();
            var dbList = dbEnter.T_LPSBBC;           //获取数据库内容
            foreach (var lst in modelList)//获取
            {
                var chk = dbList.FirstOrDefault(p => p.ORDERID == lst.ORDERID);//订单号匹配
                if (chk == null)//增量添加
                {
                    enter = new T_LPSBBC();
                    enter.ID = Guid.NewGuid().ToString();
                    enter.MERCHANTID = lst.MERCHANTID;
                    enter.BRANCHID = lst.BRANCHID;
                    enter.POSID = lst.POSID;
                    enter.ORDERID = lst.ORDERID;
                    enter.ORDERDATE = lst.ORDERDATE;
                    enter.ACCDATE = lst.ACCDATE;
                    enter.AMOUNT = lst.AMOUNT;
                    enter.STATUSCODE = lst.STATUSCODE;
                    enter.STATUS = lst.STATUS;
                    enter.REFUND = lst.REFUND;
                    enter.SIGN = lst.SIGN;
                    enter.CreateTm = DateTime.Now;
                    dbEnter.T_LPSBBC.AddObject(enter);
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
                    LogTxt.WriteEntry("明细入账异常" + ex.Message, "六盘水建行查询");
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
            var dbEnter = new PM.TaskBiz.LPSBBCTask.ORM.PM_IntegratedEntities();
            var matchList = dbEnter.T_LPSBBC.Where(p => (p.IsMatch != 1 || p.IsMatch == null));//获取匹配表待匹配信息
            // var dbList = dbEnter.T_ZTB_MoneyPayment.Where(p => (p.IsCheck != 2 || p.IsCheck != 3 || p.IsCheck == null) );//入账表对应信息
            #region 匹配处理  优先规则是订单号匹配到
            foreach (var lst in matchList)//匹配
            {
                //var tradeNoStr = GetDbcNumStr(lst.ORDERID).ToLower();
                var chk = dbEnter.T_ZTB_MoneyPayment.FirstOrDefault(p => p.Out_trade_no.ToLower() == lst.ORDERID.ToLower());//根据订单号
                if (null != chk)//匹配到订单
                {
                    decimal.TryParse(lst.AMOUNT, out Amount);
                    if (chk.PayMoney == Amount)//匹配完成
                    {
                        lst.IsMatch = 1;//成功
                        if (lst.STATUS.Trim() == "1")
                            chk.IsCheck = 2;//入账成功
                        else if (lst.STATUS.Trim() == "0")
                            chk.IsCheck = 3;//入账失败
                        //else
                        //    chk.IsCheck = 1;//已经提交
                        //chk.ma = string.Empty;
                        DateTime dt = DateTime.MinValue;
                        DateTime.TryParseExact(lst.ORDERDATE, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None,
                  out  dt);
                        //chk.PayTime = dt;// 入账时间
                        dbEnter.T_LPSBBC.ApplyCurrentValues(lst);
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
