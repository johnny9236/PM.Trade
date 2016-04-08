using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PlaymentPersistence.ORM;
using PM.PaymentModel;
using PM.PaymentProtocolModel.BankCommModel.BOC;
using PM.Utils.Log;
using PM.PaymentManger;
using PM.PaymentProtocolModel;
using PM.Utils;

namespace PM.PlaymentPersistence.Payment.PersistenceBiz.HYCOB
{
    /// <summary>
    /// 海盐转账退款
    /// </summary>
    public class HYBOCPersistence
    {
        private readonly string IbkNum = ConfigHelper.GetCustomCfg("HYBOC", "IbkNum");//联号
        private readonly string Termid = ConfigHelper.GetCustomCfg("HYBOC", "Termid");//企业前置机
        private readonly string CusOpr = ConfigHelper.GetCustomCfg("HYBOC", "CusOpr");//企业操作员代码
        private readonly string CustId = ConfigHelper.GetCustomCfg("HYBOC", "CustId");//企业在中行网银系统的客户编码
        private readonly string OprPwd = ConfigHelper.GetCustomCfg("HYBOC", "OprPwd");//企业在中行网银系统的客户编码

        publicEntities enter = new publicEntities();//数据库对象
        /// <summary>
        /// 退保证金操作   (付款行行号作为银行联行号)
        /// 退款明细列表中  [通过]
        /// </summary>
        /// <param name="payRefundMode"></param>
        /// <returns></returns>
        public PayRefundModel DoRefundPay(PayRefundModel payRefundMode)
        {
            string token = string.Empty;
            if (!SingIn(ref token))
            {
                var payRst = new PayRefundModel();
                payRst.Remark = "签到失败";
                payRst.Result = "false";
                LogTxt.WriteEntry("签到失败:" + token, "中国银行签到协议报文");
                return payRst; //签到失败          
            }
            var rtn = RefundPay(payRefundMode, token);
            //签退
            SignOut(token);
            return rtn;
        }


        #region   private  method

        /// <summary>
        /// 退款处理
        /// </summary>
        /// <param name="payRefundMode"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private PayRefundModel RefundPay(PayRefundModel payRefundMode, string token)
        {
            string businessFunNo = string.Empty;

            BOCRefundRequset refundModel = null;
            BOCRefundRQDtl qyRefund = null;
            if (null != payRefundMode && null != payRefundMode.PayRefundDtl && payRefundMode.PayRefundDtl.Count > 0)
            {
                businessFunNo = payRefundMode.BusinessFunNo;//功能号 
                #region  发送准备
                refundModel = new BOCRefundRequset();
                refundModel.BusinessFunNo = businessFunNo;

                refundModel.CeitInfo = "";//不用赋值
                refundModel.TransType = ""; //

                refundModel.Trnid = DateTime.Now.ToString("yyMMddHHmmss");
                refundModel.Termid = Termid;//企业前置机
                refundModel.CustId = CustId;//企业在中行网银系统的客户编码
                refundModel.CusOpr = CusOpr;//企业操作员代码
                //refundModel.Trncod = ""; 
                refundModel.Token = token;//后台  
                refundModel.BOCRefundRQDtlLst = new List<BOCRefundRQDtl>();

                foreach (var refundPay in payRefundMode.PayRefundDtl)
                {
                    #region 报文对象赋值
                    qyRefund = new BOCRefundRQDtl();
                    //  qyRefund.Comacn = "";//手续费账号
                    qyRefund.FurInfo = refundPay.TradeInfo;//用途
                    qyRefund.InsId = refundPay.OrderNo;
                    qyRefund.ObssId = "";
                    qyRefund.PayActaCn = refundPay.PayAcountNo;
                    qyRefund.PayActNam = refundPay.PayAcountName;
                    qyRefund.PayFribkn = IbkNum;// refundPay.PayOpenBankNo;//使用联行号
                    qyRefund.Priolv = "0";//普通非加急
                    qyRefund.Receiveactacn = refundPay.ReceiptAcountNo;
                    qyRefund.ReceiveTobknm = refundPay.ReceiptAccountDbBank;
                    qyRefund.ReceiveToibkn = refundPay.ReceiptOpenBankNo;
                    qyRefund.ReceiveToibknoAddr = refundPay.ReceiptProvince;
                    qyRefund.ReceiveToName = refundPay.ReceiptAcountName;
                    qyRefund.TrfDate = refundPay.InDate;
                    qyRefund.TrnAmt = refundPay.PayMoney.ToString();
                    qyRefund.TrnCur = refundPay.PayCur ?? "CNY";


                    #endregion
                    // 订单信息处理
                    if (CreateOrderAndBusniess(payRefundMode, refundPay))
                        refundModel.BOCRefundRQDtlLst.Add(qyRefund);
                }
                #endregion
                //提交数据
                try
                {
                    enter.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry("异常" + ex.Message, "海盐退款异常");
                    return new PayRefundModel();//退回
                }
                //发送协议
                BOCRefundResponse refundRtn = Manager.PaymentManager(refundModel);

                if (null != refundRtn && null != refundRtn.RefundResponseDtlLst && refundRtn.RefundResponseDtlLst.Count > 0)
                    // 返回处理
                    payRefundMode = SetOrderAndBusniess(refundRtn);
            }
            return payRefundMode;

        }


        /// <summary>
        /// 设置业务相关信息
        /// </summary>
        /// <param name="payRefundMode">支付信息</param>
        /// <param name="payModel">一条明细对象</param>
        private bool CreateOrderAndBusniess(PayRefundModel payRefundMode, PayStartModel payModel)
        {
            try
            {
                #region  订单信息
                T_Pay_Order order = new T_Pay_Order();
                order.OrderID = Guid.NewGuid();
                order.BusinessNo = payRefundMode.BusinessFunNo;
                order.PrimaryID = payModel.BusnissID;
                order.PayRealAccountNo = payModel.PayAcountNo;
                order.PayRealAccountName = payModel.PayAcountName;
                order.PayRealBankName = payModel.PayAccountDbBank;

                order.MainOrderNo = payRefundMode.OrderNo;//主订单号

                order.OrderNo = payModel.OrderNo;
                order.ReceiptBankName = payModel.ReceiptAccountDbBank;
                order.ReceiptBankNo = payModel.ReceiptOpenBankNo;
                order.ReceiptAccountNo = payModel.ReceiptAcountNo;
                order.ReceiptAccountName = payModel.ReceiptAcountName;
                order.Amount = payModel.PayMoney;
                order.FeeAmount = payModel.PayFee;
                enter.T_Pay_Order.AddObject(order);//订单信息入库
                //业务关系表  通过 订单号+退款业务ID
                T_Pay_OrderBusiness business = enter.T_Pay_OrderBusiness.Where(p => p.OrderNo == order.OrderNo && p.Business_ID == payModel.BusnissID).FirstOrDefault();
                if (null == business)
                {
                    business = new T_Pay_OrderBusiness();
                    business.ID = Guid.NewGuid().ToString();
                    business.OrderNo = order.OrderNo;
                    business.MainOrderNo = payRefundMode.OrderNo;//主账号订单号
                    business.PayMent_Tm = DateTime.Now;
                    business.Business_ID = payModel.BusnissID;
                    business.Create_Tm = DateTime.Now;
                    //business.OrderStatus="1";
                    enter.T_Pay_OrderBusiness.AddObject(business);//订单信息入库
                }
                else
                {
                    business.Up_Tm = DateTime.Now;
                    //    business.OrderNo = refundPay.OrderNo;
                    enter.T_Pay_OrderBusiness.ApplyCurrentValues(business);//订单信息修改
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("异常" + ex.Message, "海盐退款异常");
                return false;
            }

        }
        /// <summary>
        /// 设置订单并返回业务相关信息
        /// </summary>
        /// <param name="refundRtn">响应对象</param>
        private PayRefundModel SetOrderAndBusniess(BOCRefundResponse refundRtn)
        {
            var payRefundMode = new PayRefundModel();//初始化用于返回
            payRefundMode.Descript = refundRtn.RspMsg;
            payRefundMode.PayRefundDtl = new List<PayStartModel>();//返回对象
            try
            {
                foreach (var bank in refundRtn.RefundResponseDtlLst)
                {
                    var payModel = new PayStartModel();
                    #region  返回后状态修改
                    if (string.IsNullOrEmpty(bank.InsId))
                    {
                        LogTxt.WriteEntry(string.Format("退款响应, 订单号 为空 信息为[{1}] ", bank.RspMsg), "海盐退款日志");
                        continue;
                    }
                    //订单信息
                    var orderInfo = enter.T_Pay_Order.Where(p => p.OrderNo == bank.InsId).FirstOrDefault();
                    if (null != orderInfo)
                    {
                        orderInfo.OrderSerialNumber = bank.ObssId;//银行流水号
                        orderInfo.OrderResult = bank.RspCod == "B001" ? ((int)OrderFlag.Sucess) : ((int)OrderFlag.Faile);
                        enter.T_Pay_Order.ApplyCurrentValues(orderInfo);//修改订单状态
                        //业务关系表   业务主键(可以不要)+订单号
                        //var businessID = orderInfo.PrimaryID;
                        var businessInfo = enter.T_Pay_OrderBusiness.Where(p => p.Business_ID == orderInfo.PrimaryID && p.OrderNo == orderInfo.OrderNo).FirstOrDefault();
                        if (null != businessInfo)
                        {
                            businessInfo.OrderStatus = orderInfo.OrderResult == ((int)OrderFlag.Sucess) ? ((int)PayFlag.PaySucess).ToString() : ((int)PayFlag.PayFail).ToString();
                            enter.T_Pay_OrderBusiness.ApplyCurrentValues(businessInfo);
                        }
                        else
                        {
                            LogTxt.WriteEntry(string.Format("退款响应,未找到对应业务信息（T_Pay_OrderBusiness）ID=[{0}]", businessInfo.ID), "海盐退款日志");
                        }
                    }
                    else
                    {
                        LogTxt.WriteEntry(string.Format("退款响应,未找到对应订单信息信息（T_Pay_Order）订单号=[{0}]  ", bank.InsId), "海盐退款日志");
                    }
                    #endregion
                    payModel.OrderNo = bank.InsId;
                    payModel.Remark = bank.RspMsg;
                    payModel.ReceiptSettingAccNo = bank.ObssId;
                    payModel.Result = bank.RspCod == "B001" ? true : false;
                    payRefundMode.PayRefundDtl.Add(payModel);
                }
                //提交数据

                enter.SaveChanges();
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("异常" + ex.Message, "海盐退款日志");
            }
            return payRefundMode;
        }



        #region sign
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
        #endregion
    }
}
