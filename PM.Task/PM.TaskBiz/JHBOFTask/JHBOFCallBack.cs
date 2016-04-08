using System;
using System.Collections.Generic;
using System.Text;
using PM.Utils.Log;
using PM.TaskBizInterface;
using System.Linq;
using PM.PaymentProtocolModel.BankCommModel.JHBOF;
using PM.TaskBiz.JHBOFTask.ORM;
using PM.TaskBiz.JSABOCTask.ORM;
using PM.Utils;

namespace PM.TaskBusiness.JHBOFTask
{
    /// <summary>
    /// 回调执行业务
    /// </summary>
    public class JHBOFCallBack : ITimerTaskCallBack
    {
        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="dyObj"></param>
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
        private bool IncrementToDB(List<JHBofQueryResult> modelList)
        {
            bool rtn = false;
            T_JHBOF enter = null;
            var dbEnter = new PM.TaskBiz.JHBOFTask.ORM.jinhuaEntities();
            var dbList = dbEnter.T_JHBOF;           //获取数据库内容
            foreach (var lst in modelList)
            {
                var chk = dbList.FirstOrDefault(p => p.TradeNo == lst.TradeNo && p.CustomTradeNo == lst.CustomTradeNo && p.TradeDate == lst.TradeDate);
                if (chk == null)//增量添加
                {
                    enter = new T_JHBOF();
                    enter.ID = Guid.NewGuid().ToString();
                    enter.TradeNo = lst.TradeNo;
                    enter.TradeDate = lst.TradeDate;
                    enter.Amount = lst.Amount;
                    enter.AcountName = lst.AcountName;
                    enter.AcountNo = lst.AcountNo;
                    enter.CreateTm = DateTime.Now;
                    enter.Remark = lst.Remark;
                    enter.CustomTradeNo = lst.CustomTradeNo;
                    dbEnter.T_JHBOF.AddObject(enter);
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
                    LogTxt.WriteEntry("明细入账异常" + ex.Message, "金华交行查询");
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
            string payAccount = string.Empty;//支付账号（备注中）
            bool haveMatch = false;//是否匹配

            var dbEnter = new PM.TaskBiz.JHBOFTask.ORM.jinhuaEntities();
            var matchList = dbEnter.T_JHBOF.Where(p => (p.Match != 1 || p.Match == null)&&!string.IsNullOrEmpty(p.Remark));//获取匹配表待匹配信息
            var dbList = dbEnter.T_ZTB_MoneyPayment.Where(p => ((p.IsCheck != 2 && p.IsCheck != 3) || p.IsCheck == null) && p.PayPage == 4);//入账表对应信息
            #region 匹配处理  优先规则是订单号匹配到
            foreach (var lst in matchList)//需修改对象匹配 银行交易记录表
            {
                payAccount = string.Empty;//支付账号（备注中）
                var tradeNoStr = GetDbcNumStr(lst.Remark ?? string.Empty, out payAccount).ToLower();

                if (string.IsNullOrEmpty(tradeNoStr))//无订单号  不处理
                    continue;
                var chk = dbList.FirstOrDefault(p => p.PayOrderNo.ToLower() == tradeNoStr);//根据订单号
                if (null != chk)//匹配到订单
                {
                    #region
                    //  isBocMatch = (chk.BaseAccountNo.Trim() == lst.AcountNo.Trim()) || (chk.NormalAccountNo.Trim() == lst.AcountNo.Trim()); //是否是在基本及一般账号内
                    //  if (!isBocMatch && !string.IsNullOrEmpty(payAccount))
                    //  {
                    //      isBocMatch = (chk.BaseAccountNo.Trim() == payAccount.Trim()) || (chk.NormalAccountNo.Trim() == payAccount.Trim()); //是否是在基本及一般账号内(摘要中)
                    //  }
                    //  if (isBocMatch && chk.PayMoney == lst.Amount)//匹配完成
                    //  {
                    //      lst.Match = 1;//成功
                    //      chk.IsCheck = 2;//入账成功
                    //      DateTime dt = DateTime.MinValue;
                    //      DateTime.TryParseExact(lst.TradeDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None,
                    //out  dt);
                    //      chk.PayTime = dt;// 入账时间
                    //      dbEnter.T_JHBOF.ApplyCurrentValues(lst);
                    //      dbEnter.T_ZTB_MoneyPayment.ApplyCurrentValues(chk);
                    //      haveMatch = true;//初始化匹配状态
                    //  }
                    //  //else//订单号匹配成功  账号或金额匹配不成功
                    //  //{
                    //  //    if (chk.PayAccNo.Trim() == lst.PayAccNo.Trim())
                    //  //    {
                    //  //        chk.Remark = "账号不匹配";
                    //  //    }
                    //  //    else
                    //  //    {
                    //  //        chk.Remark = "金额不匹配";
                    //  //    }
                    //  //    chk.IsCheck = 3;//入账不成功 
                    //  // lst.IsMatch = 1;//成功
                    //  //    dbEnter.T_ZTB_BidMoneyPayReturn.ApplyCurrentValues(chk);
                    //  //}
                    #endregion
                    if (MatchOpration(dbEnter, chk, lst, payAccount) && !haveMatch)
                    {
                        haveMatch = true;
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
                    LogTxt.WriteEntry("订单匹配处理异常" + ex.Message, "嘉善农行查询");
                }
            }
            #endregion
            #region  未匹配到订单号规则   只匹配账号(    是否需要 待定)
            //haveMatch = false;//初始化匹配状态
            //T_ZTB_MoneyPayment chkModel = null;
            //var newMatchLst = matchList.Where(p => (p.Match != 1 || p.Match == null));//获取未匹配成功的记录
            //foreach (var lst in newMatchLst)//匹配
            //{
            //    decimal amount = (lst.Amount ?? 0) / 100;//银行金额到分
            //    payAccount = string.Empty;//支付账号（备注中）
            //    var tradeNoStr = GetDbcNumStr(lst.Remark ?? string.Empty, out payAccount).ToLower();
            //    chkModel = dbList.FirstOrDefault(p => (p.BaseAccountNo == lst.AcountNo.Trim() || p.NormalAccountNo.Trim() == lst.AcountNo.Trim()) && p.PayMoney == amount && p.PayPage == 4);//如金额与账号匹配   订单号不匹配（处理）
            //    if (chkModel == null && !string.IsNullOrEmpty(payAccount))
            //    {
            //        chkModel = dbList.FirstOrDefault(p => (p.BaseAccountNo == payAccount.Trim() || p.NormalAccountNo.Trim() == payAccount.Trim()) && p.PayMoney == amount);//摘要中的账户比对
            //    }
            //    if (null != chkModel)
            //    {
            //        lst.Match = 1;//成功
            //        chkModel.IsCheck = 2;//入账成功
            //        chkModel.Out_trade_no = lst.TradeNo;//银行产生订单号
            //        DateTime dt = DateTime.MinValue;
            //        DateTime.TryParseExact(lst.TradeDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None,
            //        out  dt);
            //        chkModel.PayTime = dt;// 入账时间
            //        dbEnter.T_JHBOF.ApplyCurrentValues(lst);
            //        dbEnter.T_ZTB_MoneyPayment.ApplyCurrentValues(chkModel);
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
        /// 匹配操作
        /// </summary>
        /// <param name="dbEnter">数据库对象</param>
        /// <param name="chk">入账记录</param>
        /// <param name="lst">银行明细记录</param>
        /// <param name="payAccount">银行摘要中获取支付账号(目前为空)</param>
        /// <returns></returns>
        private bool MatchOpration(PM.TaskBiz.JHBOFTask.ORM.jinhuaEntities dbEnter, T_ZTB_MoneyPayment chk, T_JHBOF lst, string payAccount)
        {
            decimal amount = (lst.Amount ?? 0) / 100;//银行金额到分
            bool haveMatch = false;
            bool isBocMatch = (chk.BaseAccountNo.Trim() == lst.AcountNo.Trim()); //是否是在基本内
            if (chk.PayMoney <= amount)//金额匹配成功
            {
                //if (!isBocMatch)//匹配基本账户 完全匹配(业务端处理)
                //{
                //    if (!string.IsNullOrEmpty(lst.AcountNo.Trim()))
                //    {
                //        #region 对账单中支付账户不为空
                //        #endregion
                //    }
                //    else
                //    {
                //        #region
                //        #endregion
                //    }
                //}
                lst.Match = 1;//成功
                chk.IsCheck = 2;//入账成功
                //chk.Out_trade_no = lst.TradeNo;//银行产生订单号
                chk.Out_trade_no = lst.ID;//银行记录表ID
                DateTime dt = DateTime.MinValue;
                DateTime.TryParseExact(lst.TradeDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None,
          out  dt);
                chk.PayTime = dt;// 入账时间
                haveMatch = true;//初始化匹配状态
            }
            return haveMatch;
            #region  原逻辑
            //  isBocMatch = (chk.BaseAccountNo.Trim() == lst.AcountNo.Trim()) || (chk.NormalAccountNo.Trim() == lst.AcountNo.Trim()); //是否是在基本及一般账号内
            //  if (!isBocMatch && !string.IsNullOrEmpty(payAccount))
            //  {
            //      isBocMatch = (chk.BaseAccountNo.Trim() == payAccount.Trim()) || (chk.NormalAccountNo.Trim() == payAccount.Trim()); //是否是在基本及一般账号内(摘要中)
            //  }
            //  if (isBocMatch && chk.PayMoney == amount)//匹配完成
            //  {
            //      lst.Match = 1;//成功

            //      chk.IsCheck = 2;//入账成功
            //      chk.Out_trade_no = lst.TradeNo;//银行产生订单号

            //      DateTime dt = DateTime.MinValue;
            //      DateTime.TryParseExact(lst.TradeDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None,
            //out  dt);
            //      chk.PayTime = dt;// 入账时间
            //      dbEnter.T_JHBOF.ApplyCurrentValues(lst);
            //      dbEnter.T_ZTB_MoneyPayment.ApplyCurrentValues(chk);
            //      haveMatch = true;//初始化匹配状态
            //  }
            //  //else//订单号匹配成功  账号或金额匹配不成功
            //  //{
            //  //    if (chk.PayAccNo.Trim() == lst.PayAccNo.Trim())
            //  //    {
            //  //        chk.Remark = "账号不匹配";
            //  //    }
            //  //    else
            //  //    {
            //  //        chk.Remark = "金额不匹配";
            //  //    }
            //  //    chk.IsCheck = 3;//入账不成功 
            //  // lst.IsMatch = 1;//成功
            //  //    dbEnter.T_ZTB_BidMoneyPayReturn.ApplyCurrentValues(chk);
            //  //}    
            //  return haveMatch;
            #endregion
        }


        /// <summary>
        /// 订单号转半角字符
        /// </summary>
        /// <param name="inputStr">输入字符</param>
        /// <param name="PayAccount">支付账号</param>
        /// <returns></returns>
        private string GetDbcNumStr(string inputStr, out  string PayAccount)
        {
            PayAccount = string.Empty;//支付账号
            string rtnStr = string.Empty;//订单号
            try
            {
                var sourceStr = PM.Utils.StringHelper.ToDBC(inputStr);
                sourceStr = sourceStr.Replace(" ", "");
                rtnStr = PM.Utils.StringHelper.GetNumberString(sourceStr, false);
                var strIndex = int.Parse(ConfigHelper.GetCustomCfg("JH", "OrderIndex"));
                var strLength = int.Parse(ConfigHelper.GetCustomCfg("JH", "OrderLength"));
                if (strIndex + 1 < rtnStr.Length)
                {
                    var allLastStr = rtnStr.Substring(strIndex);
                    strLength = strLength <= allLastStr.Length ? strLength : allLastStr.Length;
                    rtnStr = rtnStr.Substring(strIndex, strLength);
                }
                //var info = sourceStr.Split(',',';');//分割
                ////rtnStr = PM.Utils.StringHelper.GetNumberString(info[0]);
                //if (info.Length > 1)
                //{
                //    //PayAccount = PM.Utils.StringHelper.GetNumberString(info[1]);
                //    rtnStr = PM.Utils.StringHelper.GetNumberString(info[1],false);
                //}
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("订单号转半角处理异常" + ex.Message, "金华交行查询");
            }
            return rtnStr;
        }
        #endregion
    }
}
