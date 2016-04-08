using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.HuangShi;
using PM.TaskBiz.HuangShiCCBTask.ORM;
using PM.Utils.Log;

namespace PM.TaskBiz.HuangShiCCBTask
{
    /// <summary>
    /// 回调执行黄石job
    /// </summary>
    public class HuangShiCCBCallBack : ITimerTaskCallBack
    {
        /// <summary>
        /// 黄石建设工程数据操作对象
        /// </summary>
        private GovPmBidOnline_BusinessEntities dbEnter = new GovPmBidOnline_BusinessEntities();

        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="dyObj"></param>
        public void CallBack(dynamic dyObj)
        {
            try
            {
                //增量入库
                bool isHaveIn = IncrementToDB(dyObj);
                //匹配
                DataMatch();
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("明细入账异常" + ex.Message, "HuangShiCCB保证金明细查询");
            }
        }

        #region 增量入库
        /// <summary>
        /// 查询明细结果增量入库
        /// </summary>
        /// <param name="modResponse">明细结果实体集合</param>
        /// <returns>是否入库成功</returns>
        private bool IncrementToDB(HuangShiDepositResponseModel modResponse)
        {
            bool isSaveSuccess = false;
            T_ZTB_DepositCCB modDepositCCB = null;
            
            var dbList = dbEnter.T_ZTB_DepositCCB.Where(p => p.TX_CODE == "ZTB2");
            if (dbList!=null)
            {
                foreach (var modlist in modResponse.ResponseListModel)
                {
                    var modHaved = dbList.FirstOrDefault(p => p.TRAN_SQ == modlist.TRAN_SQ);
                    if (modHaved==null)
                    {
                        modDepositCCB = new T_ZTB_DepositCCB();
                        modDepositCCB.ID = Guid.NewGuid();
                        modDepositCCB.REQUEST_SN = modResponse.REQUEST_SN;
                        modDepositCCB.TX_CODE = modResponse.TX_CODE;
                        modDepositCCB.CUST_ID = modResponse.CUST_ID;
                        modDepositCCB.RETURN_CODE = modResponse.RETURN_CODE;
                        modDepositCCB.RETURN_MSG = modResponse.RETURN_MSG;

                        modDepositCCB.TRAN_SQ = modlist.TRAN_SQ;
                        modDepositCCB.ACCOUNT = modlist.ACCOUNT;
                        modDepositCCB.ACCOUNT_NAME = modlist.ACCOUNT_NAME;
                        modDepositCCB.MONEY = modlist.MONEY;
                        modDepositCCB.TRAN_DATE = modlist.TRAN_DATE;
                        modDepositCCB.TRAN_TIME = modlist.TRAN_TIME;
                        modDepositCCB.OTHER_ACCOUNT = modlist.OTHER_ACCOUNT;
                        modDepositCCB.OTHER_ACCOUNT_NAME = modlist.OTHER_ACCOUNT_NAME;
                        modDepositCCB.ORDERS = modlist.ORDER;
                        modDepositCCB.CREATETIME = DateTime.Now;

                        dbEnter.T_ZTB_DepositCCB.AddObject(modDepositCCB);
                    }
                }
            }
            try
            {
                dbEnter.SaveChanges();
                isSaveSuccess = true;
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("明细入账异常" + ex.Message, "HuangShiCCB保证金明细查询");
            }
            return isSaveSuccess;
        }
        #endregion

        #region 匹配操作
        /// <summary>
        /// 匹配操作
        /// </summary>
        private void DataMatch()
        {
            bool isHaveMatch = false;//是否匹配
            var matchList = dbEnter.T_ZTB_DepositCCB.Where(p => p.TX_CODE == "ZTB2" && (p.ISMATCH != 1 || p.ISMATCH == null));//获取匹配表待匹配信息
            var dbList = dbEnter.T_ZTB_BidMoneyPayReturn.Where(p => (p.IsCheck != 2 || p.IsCheck != 3 || p.IsCheck == null) && p.BidMoneyType == "bzj");//入账表对应信息
            var dbpublic = new Gov_publicHSEntities();
            foreach (var modlist in matchList)
            {
                //根据订单号
                var chk = dbList.FirstOrDefault(p => p.OrderNum == modlist.ORDERS);
                if (chk!=null)
                {
                    if (chk.PayMoney==modlist.MONEY)
                    {
                        modlist.ISMATCH = 1;
                        chk.IsCheck = 2;
                        
                        isHaveMatch = true;
                        dbEnter.T_ZTB_DepositCCB.ApplyCurrentValues(modlist);
                        dbEnter.T_ZTB_BidMoneyPayReturn.ApplyCurrentValues(chk);
                    }
                }
            }
            if (isHaveMatch)
            {
                try
                {
                    dbEnter.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry("明细入账异常" + ex.Message, "HuangShiCCB保证金明细查询");
                }
            }
        }
        #endregion
    }
}
