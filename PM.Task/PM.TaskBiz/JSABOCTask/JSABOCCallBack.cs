using System;
using System.Collections.Generic;
using System.Text;
using PM.TaskBizInterface;
using System.Linq;
using PM.Utils.Log;
using PM.TaskBiz.JSABOCTask.ORM;
using PM.PaymentProtocolModel.BankCommModel.JSABOC;

namespace PM.TaskBiz.JSABOCTask
{
    /// <summary>
    /// 嘉善农行入账明细回调
    /// </summary>
    public class JSABOCCallBack : ITimerTaskCallBack
    {
        /// <summary>
        ///嘉善农行入账明细回调
        /// </summary>
        /// <param name="dyObj"></param>
        public void CallBack(dynamic dyObj)
        {
            //   增量入库操作
            var haveIn = IncrementToDB(dyObj);
            //回调
            //  if (haveIn)  //控制暂时取消
            //匹配操作
            Match(true);
            Match(false);
        }

        #region 增量入库
        /// <summary>
        /// 增量入库
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        private bool IncrementToDB(List<JSABOCRtnModel> modelList)
        {
            bool rtn = false;
            T_JSABOC enter = null;
            var dbEnter = new PM.TaskBiz.JSABOCTask.ORM.GovPmBidOnline_BusinessJSEntities();
            // var dbList = enter.T_JSABOC.Select(" IsMatch='0'  and TradeCode='ZTB1'");//获取数据库内容
            var dbList = dbEnter.T_JSABOC.Where(p => p.TradeCode == "ZTB1");              //获取数据库内容
            foreach (var lst in modelList)
            {
                var chk = dbList.FirstOrDefault(p => p.SerialNumber == lst.SerialNumber&&p.accountType==lst.AccountType);
                if (chk == null)//增量添加
                {
                    enter = new T_JSABOC();
                    enter.ID = Guid.NewGuid().ToString();
                    enter.TradeCode = lst.TradeCode;
                    enter.TradeStructNum = lst.TradeStructNum;
                    enter.DetailDataTime = lst.DetailDataTime;

                    enter.accountType = lst.AccountType;
                    enter.Used = lst.Use;
                    enter.PayAccDBBank = lst.PayAccDBBank;
                    enter.PayAccDbName = lst.PayAccDbName;
                    enter.PayAccNo = lst.PayAccNo;
                    enter.Amount = lst.Amount;
                    enter.Summary = lst.Summary;
                    enter.SerialNumber = lst.SerialNumber;
                    enter.CreateTm = DateTime.Now;
                    dbEnter.T_JSABOC.AddObject(enter);
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
                    LogTxt.WriteEntry("明细入账异常" + ex.Message, "嘉善农行查询");
                }

            }
            return rtn;
        }
        /// <summary>
        /// 匹配操作
        /// </summary>
        /// <param name="isBzj"></param>
        private void Match(bool isBzj)
        {
            bool haveMatch = false;//是否匹配
            bool isAbocMatch = false;// 是否是农行账号
            var dbEnter = new PM.TaskBiz.JSABOCTask.ORM.GovPmBidOnline_BusinessJSEntities();
            IQueryable<T_JSABOC> matchList = null;
            if (isBzj)
            {
                matchList = dbEnter.T_JSABOC.Where(p => p.TradeCode == "ZTB1" && (p.IsMatch != 1 || p.IsMatch == null) && p.accountType == "bzj");//获取匹配表待匹配信息
            }
            else
            {
                matchList = dbEnter.T_JSABOC.Where(p => p.TradeCode == "ZTB1" && (p.IsMatch != 1 || p.IsMatch == null) && p.accountType != "bzj");//获取匹配表待匹配信息

            }
            IQueryable<T_ZTB_BidMoneyPayReturn> dbList = null;
            if (isBzj)
            {
                dbList = dbEnter.T_ZTB_BidMoneyPayReturn.Where(p => (p.IsCheck != 2 || p.IsCheck != 3 || p.IsCheck == null) && p.BidMoneyType == "bzj");//入账表对应信息
            }
            else
            {
                dbList = dbEnter.T_ZTB_BidMoneyPayReturn.Where(p => (p.IsCheck != 2 || p.IsCheck != 3 || p.IsCheck == null) && (p.BidMoneyType != "bzj" || string.IsNullOrEmpty(p.BidMoneyType)));//入账表对应信息
            }

            #region 匹配处理  优先规则是订单号匹配到
            foreach (var lst in matchList)//匹配
            {
                var tradeNoStr = GetDbcNumStr(lst.Summary).ToLower();
                var chk = dbList.FirstOrDefault(p => p.ABOCPayOrderNo.ToLower() == tradeNoStr);//根据订单号
                if (null != chk)//匹配到订单
                {
                    LogTxt.WriteEntry("批评表ID---" + chk.Id + "  订单匹配订单号--" + tradeNoStr, "嘉善农行查询");
                    if (GetIsAboc(lst.PayAccDBBank))
                    {
                        //农行需要判断是否是15位账号   有可能加了省市
                        var chkPayAccNo = chk.PayAccNo.Trim().Length > 15 ? chk.PayAccNo.Trim().Substring(chk.PayAccNo.Trim().Length - 15) : chk.PayAccNo.Trim();
                        var matchPayAccNo = lst.PayAccNo.Trim().Length > 15 ? lst.PayAccNo.Trim().Substring(lst.PayAccNo.Trim().Length - 15) : lst.PayAccNo.Trim();
                        isAbocMatch = chkPayAccNo == matchPayAccNo;
                    }
                    else
                    {
                        isAbocMatch = chk.PayAccNo.Trim() == lst.PayAccNo.Trim();
                    }
                    if (isAbocMatch && chk.PayMoney == lst.Amount)//匹配完成
                    {
                        chk.Remark = string.Empty;
                        lst.IsMatch = 1;//成功
                        chk.IsCheck = 2;//入账成功
                        DateTime dt = DateTime.MinValue;
                        DateTime.TryParseExact(lst.DetailDataTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None,
                  out  dt);
                        chk.IincurredTime = dt;// 入账时间
                        dbEnter.T_JSABOC.ApplyCurrentValues(lst);
                        dbEnter.T_ZTB_BidMoneyPayReturn.ApplyCurrentValues(chk);
                        if (!haveMatch)
                            haveMatch = true;//初始化匹配状态
                    }
                    else//订单号匹配成功  账号或金额匹配不成功
                    {
                        if (chk.PayAccNo.Trim() == lst.PayAccNo.Trim())
                        {
                            chk.Remark = "金额不匹配";
                        }
                        else
                        {
                            chk.Remark = "账号不匹配";
                        }
                        //chk.IsCheck = 3;//入账不成功 
                        //lst.IsMatch = 1;//成功
                        dbEnter.T_ZTB_BidMoneyPayReturn.ApplyCurrentValues(chk);
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
                    LogTxt.WriteEntry("订单匹配处理异常" + ex.Message, "嘉善农行查询");
                }
            }
            #endregion
            #region  未匹配到订单号规则   只匹配账号(    是否需要 待定)
            haveMatch = false;//初始化匹配状态
            T_ZTB_BidMoneyPayReturn chkModel = null;
            IQueryable<T_JSABOC> newMatchLst = null;//matchList.Where(p => (p.IsMatch != 1 || p.IsMatch == null));//获取未匹配成功的记录


            if (isBzj)
            {
                newMatchLst = dbEnter.T_JSABOC.Where(p => p.TradeCode == "ZTB1" && (p.IsMatch != 1 || p.IsMatch == null) && p.accountType == "bzj");//获取匹配表待匹配信息
            }
            else
            {
                newMatchLst = dbEnter.T_JSABOC.Where(p => p.TradeCode == "ZTB1" && (p.IsMatch != 1 || p.IsMatch == null) && p.accountType != "bzj");//获取匹配表待匹配信息

            }

            foreach (var lst in newMatchLst)//匹配
            {

                if (GetIsAboc(lst.PayAccDbName))
                {
                    if (isBzj)
                    {
                        chkModel = dbList.FirstOrDefault(p => ((p.PayAccNo.Trim().Length > 15 ? p.PayAccNo.Trim().Substring(p.PayAccNo.Trim().Length - 15) : p.PayAccNo.Trim()) == (lst.PayAccNo.Trim().Length > 15 ? lst.PayAccNo.Trim().Substring(lst.PayAccNo.Trim().Length - 15) : lst.PayAccNo.Trim()) && p.PayAccDbName.Trim() == p.PayAccDbName.Trim() && p.PayMoney == lst.Amount) && p.BidMoneyType == "bzj");//如金额与账号匹配   订单号不匹配（是否需处理）

                    }
                    else
                    {
                        chkModel = dbList.FirstOrDefault(p => ((p.PayAccNo.Trim().Length > 15 ? p.PayAccNo.Trim().Substring(p.PayAccNo.Trim().Length - 15) : p.PayAccNo.Trim()) == (lst.PayAccNo.Trim().Length > 15 ? lst.PayAccNo.Trim().Substring(lst.PayAccNo.Trim().Length - 15) : lst.PayAccNo.Trim()) && p.PayAccDbName.Trim() == p.PayAccDbName.Trim() && p.PayMoney == lst.Amount) && p.BidMoneyType != "bzj");//如金额与账号匹配   订单号不匹配（是否需处理）

                    }
                }
                else
                {
                    if (isBzj)
                    {
                        chkModel = dbList.FirstOrDefault(p => p.PayAccNo == lst.PayAccNo && p.PayAccDbName.Trim() == p.PayAccDbName.Trim() && p.PayMoney == lst.Amount&&  p.BidMoneyType == "bzj");//如金额与账号匹配   订单号不匹配（是否需处理）
                    }else
                    {
                        chkModel = dbList.FirstOrDefault(p => p.PayAccNo == lst.PayAccNo && p.PayAccDbName.Trim() == p.PayAccDbName.Trim() && p.PayMoney == lst.Amount && p.BidMoneyType != "bzj");//如金额与账号匹配   订单号不匹配（是否需处理）
                 
                    }
                }
                if (null != chkModel)
                {
                    lst.IsMatch = 1;//成功
                    chkModel.IsCheck = 2;//入账成功
                    DateTime dt = DateTime.MinValue;
                    DateTime.TryParseExact(lst.DetailDataTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None,
                    out  dt);
                    chkModel.IincurredTime = dt;// 入账时间
                    dbEnter.T_JSABOC.ApplyCurrentValues(lst);
                    dbEnter.T_ZTB_BidMoneyPayReturn.ApplyCurrentValues(chkModel);
                    if (!haveMatch)
                        haveMatch = true;
                }
            }
            if (haveMatch)
            {
                try
                {
                    dbEnter.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry("匹配处理异常" + ex.Message, "嘉善农行查询");
                }
            }
            #endregion
        }
        #endregion
        #region private
        /// <summary>
        /// 是否农行判断
        /// </summary>
        /// <param name="bankName">开户行</param>
        /// <returns></returns>
        private bool GetIsAboc(string bankName)
        {
            if (bankName.IndexOf("农行") > -1)
            {
                return true;
            }
            else if (bankName.IndexOf("农业银行") > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
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
