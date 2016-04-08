using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.PaymentProtocolModel.BankCommModel.BOC;
using PM.TaskBiz.ORM;
using PM.Utils.Log;
using PM.Utils;
using System.Web;
using PM.Utils.WebUtils;

namespace PM.TaskBiz.HYBOCTASK
{
    public class BOCCallBack : ITimerTaskCallBack
    {
        /// <summary>
        /// 回调处理
        /// </summary>
        /// <param name="dyObj"></param>
        public void CallBack(dynamic dyObj)
        {
            //   增量入库操作
            var haveIn = IncrementToDB(dyObj);
            //回调
            //  if (haveIn)  //控制暂时取消
            //匹配操作
            //  Match();
        }
        #region   private
        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="modelList">明细列表记录</param>
        /// <returns></returns>
        private object IncrementToDB(List<BOCQueryAccountDtlModel> modelList)
        {
            bool rtn = false;
            T_BOC enter = null;
            T_BOC_OUT enterOut = null;
            var dbEnter = new PM.TaskBiz.ORM.Pub_Entities();
            var dbInList = from c in dbEnter.T_BOC select c;
            var dbOutList = from c in dbEnter.T_BOC_OUT select c;
            //获取数据库内容
            foreach (var lst in modelList)
            {
                if (lst.Direction.Trim() == "1")
                {
                    #region  入账成功的交易
                    //通过凭证号
                    var chk = dbInList.FirstOrDefault(p => p.VouchNum == lst.VouchNum && p.PayActacn == lst.PayActacn && p.Mactacn == lst.Mactacn);
                    if (chk == null)//增量添加
                    {
                        if (lst.RspCod.ToLower() == "B001".ToLower())//只记录成功的记录 再次判断
                        {
                            enter = new T_BOC();
                            enter.ID = Guid.NewGuid().ToString();
                            enter.RspCod = lst.RspCod;
                            enter.RspMsg = lst.RspMsg;
                            enter.PayIbkNum = lst.PayIbkNum;
                            enter.PayIbkName = lst.PayIbkName;
                            enter.PayActacn = lst.PayActacn;
                            enter.PayAcntName = lst.PayAcntName;
                            enter.ReciveToIbkn = lst.ReciveToIbkn;
                            enter.ReciveActacn = lst.ReciveActacn;
                            enter.ReciveToName = lst.ReciveToName;
                            enter.ReciveToBank = lst.ReciveToBank;
                            enter.MactIbkn = lst.MactIbkn;
                            enter.Mactacn = lst.Mactacn;
                            enter.Mactname = lst.Mactname;
                            enter.MactBank = lst.MactBank;
                            enter.Vchnum = lst.Vchnum;
                            enter.TransId = lst.TransId;
                            enter.TxnDate = lst.TxnDate;
                            enter.TxnTime = lst.TxnTime;
                            enter.TxNamt = lst.TxNamt;
                            enter.Acctbal = lst.Acctbal;
                            enter.Avlbal = lst.Avlbal;
                            enter.FrzAmt = lst.FrzAmt;
                            enter.OverdrAmt = lst.OverdrAmt;
                            enter.AvloverdrAmt = lst.AvloverdrAmt;
                            enter.UseInfo = lst.UseInfo;
                            enter.FurInfo = lst.FurInfo;
                            enter.TransType = lst.TransType;
                            enter.BusType = lst.BusType;
                            enter.TrnCur = lst.TrnCur;
                            enter.Direction = lst.Direction;
                            enter.FeeAct = lst.FeeAct;
                            enter.FeeAmt = lst.FeeAmt;
                            enter.FeeCur = lst.FeeCur;
                            enter.ValDat = lst.ValDat;
                            enter.VouchTp = lst.VouchTp;
                            enter.VouchNum = lst.VouchNum;
                            enter.FxRate = lst.FxRate;
                            enter.InterInfo = lst.InterInfo;
                            enter.Reserve1 = lst.Reserve1;
                            enter.Reserve2 = lst.Reserve2;
                            enter.Reserve3 = lst.Reserve3;

                            enter.CreateTm = DateTime.Now;
                            dbEnter.T_BOC.AddObject(enter);
                            if (!rtn)
                            {
                                rtn = true;
                            }
                        }
                    }
                    #endregion
                }
                else if (lst.Direction.Trim() == "2")
                {
                    #region 出账成功的交易 目前不用
                    ////通过凭证号
                    //var chk = dbOutList.FirstOrDefault(p => p.VouchNum == lst.VouchNum && p.PayActacn == lst.PayActacn && p.Mactacn == lst.Mactacn);
                    //if (chk == null)//增量添加
                    //{
                    //    if (lst.RspCod.ToLower() == "B001".ToLower())//只记录成功的记录 再次判断
                    //    {
                    //        enterOut = new  T_BOC_OUT();
                    //        enterOut.ID = Guid.NewGuid().ToString();
                    //        enterOut.RspCod = lst.RspCod;
                    //        enterOut.RspMsg = lst.RspMsg;
                    //        enterOut.PayIbkNum = lst.PayIbkNum;
                    //        enterOut.PayIbkName = lst.PayIbkName;
                    //        enterOut.PayActacn = lst.PayActacn;
                    //        enterOut.PayAcntName = lst.PayAcntName;
                    //        enterOut.ReciveToIbkn = lst.ReciveToIbkn;
                    //        enterOut.ReciveActacn = lst.ReciveActacn;
                    //        enterOut.ReciveToName = lst.ReciveToName;
                    //        enterOut.ReciveToBank = lst.ReciveToBank;
                    //        enterOut.MactIbkn = lst.MactIbkn;
                    //        enterOut.Mactacn = lst.Mactacn;
                    //        enterOut.Mactname = lst.Mactname;
                    //        enterOut.MactBank = lst.MactBank;
                    //        enterOut.Vchnum = lst.Vchnum;
                    //        enterOut.TransId = lst.TransId;
                    //        enterOut.TxnDate = lst.TxnDate;
                    //        enterOut.TxnTime = lst.TxnTime;
                    //        enterOut.TxNamt = lst.TxNamt;
                    //        enterOut.Acctbal = lst.Acctbal;
                    //        enterOut.Avlbal = lst.Avlbal;
                    //        enterOut.FrzAmt = lst.FrzAmt;
                    //        enterOut.OverdrAmt = lst.OverdrAmt;
                    //        enterOut.AvloverdrAmt = lst.AvloverdrAmt;
                    //        enterOut.UseInfo = lst.UseInfo;
                    //        enterOut.FurInfo = lst.FurInfo;
                    //        enterOut.TransType = lst.TransType;
                    //        enterOut.BusType = lst.BusType;
                    //        enterOut.TrnCur = lst.TrnCur;
                    //        enterOut.Direction = lst.Direction;
                    //        enterOut.FeeAct = lst.FeeAct;
                    //        enterOut.FeeAmt = lst.FeeAmt;
                    //        enterOut.FeeCur = lst.FeeCur;
                    //        enterOut.ValDat = lst.ValDat;
                    //        enterOut.VouchTp = lst.VouchTp;
                    //        enterOut.VouchNum = lst.VouchNum;
                    //        enterOut.FxRate = lst.FxRate;
                    //        enterOut.InterInfo = lst.InterInfo;
                    //        enterOut.Reserve1 = lst.Reserve1;
                    //        enterOut.Reserve2 = lst.Reserve2;
                    //        enterOut.Reserve3 = lst.Reserve3;

                    //        enterOut.CreateTm = DateTime.Now;
                    //        dbEnter.T_BOC_OUT.t.AddObject(enterOut);
                    //        if (!rtn)
                    //        {
                    //            rtn = true;
                    //        }
                    //    }
                    //}
                    #endregion
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
                    LogTxt.WriteEntry("明细入账异常" + ex.Message, "海盐匹配");
                }

            }
            return rtn;

        }

        /// <summary>
        /// 匹配操作
        /// </summary>
        private void Match()
        {
            #region  回调准备
            var urlStr = ConfigHelper.GetCustomCfg("HYBOC", "BusinessUrl");
            var enCodingStr = ConfigHelper.GetCustomCfg("HYBOC", "enCoding");
            var chkStr = ConfigHelper.GetCustomCfg("HYBOC", "chkStr");//核对值
            var roundCount = Convert.ToInt32(ConfigHelper.GetCustomCfg("HYBOC", "roundCount"));//核对值
            if (string.IsNullOrEmpty(urlStr) || string.IsNullOrEmpty(enCodingStr) || string.IsNullOrEmpty(chkStr) || roundCount <= 0)
            {
                LogTxt.WriteEntry("回调地址或者编码等未设置，请设置", "海盐支付匹配");
                return;
            }
            var enCoding = Encoding.GetEncoding(enCodingStr);//回调编码
            #endregion

            bool haveMatch = false;//是否匹配 
            var dbEnter = new PM.TaskBiz.ORM.Pub_Entities();
            //Direction 来账  出账
            var matchList = dbEnter.T_BOC.Where(p => (p.Match != 1 || p.Match == null) && p.Direction == "1");
            #region 匹配处理  优先规则是订单号匹配到
            foreach (var lst in matchList)//匹配
            {
                var postStr = string.Format(
                    @"PayRealAccountName={0}&PayRealAccountNo={1}&PayRealBankName={2}
                        &ReceiveRealAccountName={3}&ReceiveRealAccountNo={4}&Amount={5}
                        &FeeAmount={6}&PrimaryID={7}&SlaveID={8}&TradeNo={9}
                        &SerialNumber={10}&LoanMark={11}&CostType={12}&PayDateTime={13}&PayDate={14}&PayTime={15}",
                   HttpUtility.UrlEncode(lst.PayAcntName ?? string.Empty, enCoding)
                   , lst.PayActacn
                   , HttpUtility.UrlEncode(lst.PayIbkName ?? string.Empty, enCoding) 
                   , HttpUtility.UrlEncode(lst.Mactname ?? string.Empty, enCoding)
                   , lst.Mactacn//lst.ReciveActacn 虚拟账号
                   , lst.TxNamt
                   , lst.FeeAmt//利息
                   , lst.ReciveActacn//主账号
                   ,    HttpUtility.UrlEncode(lst.ReciveToName ?? string.Empty, enCoding) //HttpUtility.UrlEncode(lst.Mactname ?? string.Empty, enCoding)//被代理账号名称
                   , lst.VouchNum
                   , lst.VouchNum
                   , lst.Direction == "1" ? "0" : "1"//借 0 贷1
                   , "BZJ"//费用类型
                   , lst.TxnDate + lst.TxnTime
                   , lst.TxnDate
                   , lst.TxnTime
                   );
                LogTxt.WriteEntry("回调信息" + urlStr + postStr, "海盐匹配回调");
                if (HttpTransfer.PostBackToBusinesss(postStr, urlStr, enCoding, chkStr, roundCount))//匹配完成
                {
                    lst.Match = 1;//成功  
                    dbEnter.T_BOC.ApplyCurrentValues(lst);
                    haveMatch = true;//初始化匹配状态 
                }
                else
                {
                    LogTxt.WriteEntry("回调匹配响应失败" + postStr, "海盐匹配回调");
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
                    LogTxt.WriteEntry("订单匹配处理异常" + ex.Message, "海盐匹配回调");
                }
            }
            #endregion
        }
        #endregion
    }
}
