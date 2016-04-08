using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentModel;
using PM.PaymentProtocolModel.BankCommModel.AHQY;
using PM.PaymentManger;
using PM.PlaymentPersistence.ORM;
using PM.Utils.Log;
using PM.PaymentProtocolModel.BankCommModel;
using PM.PaymentProtocolModel;

namespace PM.PlaymentPersistence.Payment.PersistenceBiz.AHQY
{
    /// <summary>
    ///  退款处理（包括人工）[按标段退]
    /// 通过入账明细中流水号来匹配
    /// payRefundMode.TradeInfo;//标段号[注意]
    /// </summary>
    public class QYPersistence
    {
        publicEntities enter = new publicEntities();//数据库对象
        /// <summary>
        /// 退保证金操作 
        /// 退款明细列表中 需要序列号
        /// </summary>
        /// <param name="payRefundMode"></param>
        /// <returns></returns>
        public PayRefundModel DoRefundPay(PayRefundModel payRefundMode)
        {
            string businessFunNo = string.Empty;//功能类型
            string sectionNo = string.Empty;//标段号或ID
            string authCode = string.Empty;//授权码
            List<string> orderNoLst = new List<string>();//本次发起所有订单号

            PayStartModel payModel = null;//保证金返回对象
            QYBBCRefundRequset refundModel = null;
            BBCRefundInfo qyRefund = null;
            if (null != payRefundMode && null != payRefundMode.PayRefundDtl && payRefundMode.PayRefundDtl.Count > 0)
            {
                authCode = payRefundMode.AuthCode;//授权号
                sectionNo = payRefundMode.TradeInfo;//标段号
                businessFunNo = payRefundMode.BusinessFunNo;//功能号
                //退款处理查询    入账明细  QYType == "0"（0入账明细 1，退款明细)   + 授权号 ==标段号
                //var rtnList = enter.T_AHQY.Where(p => p.QYType == "0" && p.SectionCode == sectionNo && p.AuthCode == authCode).ToList();//获取所有的标段的信息（防止多次读取数据库先取出）
                //if (rtnList == null || (rtnList != null && rtnList.Count == 0))
                //{
                //    LogTxt.WriteEntry(string.Format("无对应标段入账数据授权号{0}标段号{1}", authCode, sectionNo), "青阳退款异常");
                //    return new PayRefundModel();//退回
                //}

                #region  发送准备
                refundModel = new QYBBCRefundRequset();
                refundModel.AuthCode = authCode;//授权码
                refundModel.BiaoDunNo = sectionNo;//标段编号
                refundModel.BusinessFunNo = businessFunNo;
                refundModel.TransDate = payRefundMode.TransDate;
                refundModel.TransTime = payRefundMode.TransTime;
                refundModel.SeqNo = payRefundMode.OrderNo;
                refundModel.IAcctNo = payRefundMode.MainAccount;//虚拟账号(特殊注意)
                refundModel.BBCRefundList = new List<BBCRefundInfo>();
                if (null != payRefundMode && null != payRefundMode.PayRefundDtl && payRefundMode.PayRefundDtl.Count > 0)
                {
                    foreach (var refundPay in payRefundMode.PayRefundDtl)
                    {
                        qyRefund = new BBCRefundInfo();
                        qyRefund.BankName = refundPay.ReceiptAccountDbBank;
                        qyRefund.BankNo = refundPay.ReceiptOpenBankNo;
                          //无账号+无对应入账明细就跳过
                        if (string.IsNullOrEmpty(refundPay.ReceiptAcountNo))
                        {
                            LogTxt.WriteEntry("无入账账号,对应主订单号:" + payRefundMode.OrderNo + "子订单号:" + refundPay.OrderNo, "青阳退款");
                            continue;
                        }
                        var seqNum = refundPay.PaySettingAccNo;//GetRtnSeqNum(rtnList, refundPay.ReceiptAcountNo);
                        if (string.IsNullOrEmpty(seqNum))
                        {
                            LogTxt.WriteEntry("无原入账流水号,对应主订单号:" + payRefundMode.OrderNo + "子订单号:" + refundPay.OrderNo, "青阳退款");
                            continue;
                        }
                        orderNoLst.Add(refundPay.OrderNo);//保存订单号  后续返回时匹配（银行返回无订单号）
                        qyRefund.HstSeqNum = seqNum;//原入账银行流水
                        qyRefund.InAcctNo = refundPay.ReceiptAcountNo;
                        qyRefund.InName = refundPay.ReceiptAcountName;
                        qyRefund.InDate = refundPay.InDate;
                        qyRefund.InTime = refundPay.InTime;
                        qyRefund.InTranAmt = refundPay.PayMoney.ToString();

                        // 订单信息
                        if (CreateOrderAndBusniess(seqNum, payRefundMode, refundPay))//订单相关信息创建成功后添加记录
                            refundModel.BBCRefundList.Add(qyRefund);
                    }
                #endregion
                    //提交数据
                    try
                    {
                        enter.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        LogTxt.WriteEntry("异常" + ex.Message, "青阳退款异常");
                        return new PayRefundModel();//退回
                    }

                    //接口通讯
                    QYBBCRefundResponse refundRtn = Manager.PaymentManager(refundModel);//发送协议
                    if (null != refundRtn && null != refundRtn.BBCReturnRefundDtlList && refundRtn.BBCReturnRefundDtlList.Count > 0)
                    {
                        // 返回处理
                        payRefundMode = SetOrderAndBusniess(payRefundMode.OrderNo, refundRtn);
                    }
                }
            }
            return payRefundMode;
        }

        #region  private

        /// <summary>
        /// 设置业务相关信息
        /// </summary>
        /// <param name="seqNum">流水号</param>
        /// <param name="payRefundMode">支付对象</param>
        /// <param name="payModel">一个支付明细</param>
        private bool CreateOrderAndBusniess(string seqNum, PayRefundModel payRefundMode, PayStartModel payModel)
        {
            try
            {
                #region  订单信息
                T_Pay_Order order = new T_Pay_Order();
                order.OrderID = Guid.NewGuid();
                order.BusinessNo = payRefundMode.BusinessFunNo;
                order.PrimaryID = payModel.BusnissID;
                order.OrderNo = payModel.OrderNo;
                order.PayRealAccountNo = payModel.PayAcountNo;
                order.PayRealAccountName = payModel.PayAcountName;
                order.PayRealBankName = payModel.PayAccountDbBank;

                order.MainOrderNo = payRefundMode.OrderNo;//主订单号
                order.OrderSerialNumber = seqNum;//银行流水号（入账明细中的流水号）

                order.ReceiptBankName = payModel.ReceiptAccountDbBank;
                order.ReceiptBankNo = payModel.ReceiptOpenBankNo;
                order.ReceiptAccountNo = payModel.ReceiptAcountNo;
                order.ReceiptAccountName = payModel.ReceiptAcountName;
                order.Amount = payModel.PayMoney;
                order.FeeAmount = payModel.PayFee;
                order.OrderTime = DateTime.Now;
                enter.T_Pay_Order.AddObject(order);//订单信息入库
                //业务关系表  通过 订单号+退款业务ID+银行流水号
                T_Pay_OrderBusiness business = enter.T_Pay_OrderBusiness.Where(p => p.OrderNo == order.OrderNo && p.Business_ID == payModel.BusnissID && p.MainOrderNo == seqNum).FirstOrDefault();
                if (null == business)
                {
                    business = new T_Pay_OrderBusiness();
                    business.ID = Guid.NewGuid().ToString();
                    business.OrderNo = order.OrderNo;
                    business.MainOrderNo = payRefundMode.OrderNo;//订单号
                    business.PayMent_Tm = DateTime.Now;
                    business.Business_ID = payModel.BusnissID;
                    business.ReMark = payRefundMode.TradeInfo;//标段号[注意] 
                    business.Create_Tm = DateTime.Now;
                    enter.T_Pay_OrderBusiness.AddObject(business);//订单信息入库
                }
                else
                {
                    business.Up_Tm = DateTime.Now;
                    enter.T_Pay_OrderBusiness.ApplyCurrentValues(business);//订单信息修改
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("异常" + ex.Message, "青阳退款异常");
                return false;
            }
        }


        /// <summary>
        /// 设置订单并返回业务相关信息
        /// </summary>
        /// <param name="mainOrderNo">主订单号</param>
        /// <param name="refundRtn">响应对象</param>
        /// <returns></returns>
        private PayRefundModel SetOrderAndBusniess(string mainOrderNo, QYBBCRefundResponse refundRtn)
        {
            var payRefundMode = new PayRefundModel();//初始化用于返回
            payRefundMode.Descript = refundRtn.AddWord;
            payRefundMode.PayRefundDtl = new List<PayStartModel>();//返回对象
            string orderNoRtn = string.Empty;//返回订单号
            if (null != refundRtn && null != refundRtn.BBCReturnRefundDtlList && refundRtn.BBCReturnRefundDtlList.Count > 0)
            {
                try
                {
                    foreach (var bank in refundRtn.BBCReturnRefundDtlList)
                    {
                        var bussinessID = string.Empty;//业务功能号
                        orderNoRtn = string.Empty;
                        var payModel = new PayStartModel();
                        #region  返回后状态修改
                        if (string.IsNullOrEmpty(bank.InAcctNo) || string.IsNullOrEmpty(bank.HstSeqNum))
                        {
                            LogTxt.WriteEntry(string.Format("退款响应, 订单号 为空 信息为[{0}]银行流水[{1}] ", bank.InAcctNo, bank.HstSeqNum), "青阳退款记录");
                            continue;
                        }

                        // 通过流水号+订单号
                        var orderInfo = enter.T_Pay_Order.Where(p => p.OrderSerialNumber == bank.HstSeqNum && p.MainOrderNo == mainOrderNo).FirstOrDefault();
                        if (null != orderInfo)
                        {
                            orderNoRtn = orderInfo.OrderNo;
                            orderInfo.OrderResult = bank.Result == "1" ? ((int)OrderFlag.Sucess) : ((int)OrderFlag.Faile);
                            enter.T_Pay_Order.ApplyCurrentValues(orderInfo);//修改订单状态

                            //成功情况修改 入账明细中标记 (待测试)
                            if (bank.Result == "1")
                            {
                                var ahqy = enter.T_AHQY.Where(p => p.HstSeqNum == bank.HstSeqNum).FirstOrDefault();
                                if (null != ahqy && ahqy.Flag != 1)
                                {
                                    ahqy.Flag = 1;
                                    enter.T_AHQY.ApplyCurrentValues(ahqy);
                                }
                                else
                                {
                                    LogTxt.WriteEntry(string.Format("退款响应,未找到对应入账信息（T_AHQY）银行流水=[{0}] 主订单号=[{1}] ", bank.HstSeqNum, mainOrderNo), "青阳退款记录");
                                 //   continue;
                                }
                            }
                            var businessInfo = enter.T_Pay_OrderBusiness.Where(p => p.Business_ID == orderInfo.PrimaryID && p.OrderNo == orderInfo.OrderNo).FirstOrDefault();

                            if (null != businessInfo)
                            {
                                bussinessID = businessInfo.Business_ID;// 业务功能号
                                businessInfo.OrderStatus = orderInfo.OrderResult == ((int)OrderFlag.Sucess) ? ((int)PayFlag.PaySucess).ToString() : ((int)PayFlag.PayFail).ToString();
                                enter.T_Pay_OrderBusiness.ApplyCurrentValues(businessInfo);
                            }
                            else
                            {
                                LogTxt.WriteEntry(string.Format("退款响应,未找到对应业务信息（T_Pay_OrderBusiness）订单号=[{0}] 主订单号=[{1}]", orderInfo.OrderNo, mainOrderNo), "青阳退款记录");
                                continue;
                            }
                        }
                        else
                        {
                            LogTxt.WriteEntry(string.Format("退款响应,未找到对应订单信息（T_Pay_Order）银行流水=[{0}] 主订单号=[{1}] ", bank.HstSeqNum, mainOrderNo), "青阳退款记录");
                            continue;
                        }

                        #endregion
                        payModel.BusnissID = bussinessID;
                        payModel.OrderNo = orderNoRtn;
                        payModel.ReceiptSettingAccNo = bank.HstSeqNum;//结算代码 返回（银行流水）
                        payModel.Result = bank.Result == "1" ? true : false;
                        payModel.ReceiptAcountNo = bank.InAcctNo;
                        payModel.ReceiptAcountName = bank.InName;

                        payRefundMode.PayRefundDtl.Add(payModel);
                    }

                    //提交数据

                    enter.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry("异常" + ex.Message, "青阳退款异常");
                }
            }
            return payRefundMode;
        }



        /// <summary>
        /// 获取退款对应（查询入账时的银行流水号）
        /// </summary>
        /// <param name="models">当前标段入账记录列表</param>
        /// <param name="acountNo">付款账号</param>
        /// <returns></returns>
        private string GetRtnSeqNum(List<T_AHQY> models, string accountNo)
        {
            string seqNum = string.Empty;//银行流水号

            var IncomeAccount = models.FirstOrDefault(p => p.InAcct == accountNo);
            if (null != IncomeAccount)
            {
                //一个标段（项目）付款方不能重复
                if (IncomeAccount.Flag == 1)//原来已经处理成功
                {
                    LogTxt.WriteEntry(string.Format("账号{0} 标段号{1}原来处理成功ID{2}", accountNo, IncomeAccount.SectionCode, IncomeAccount.ID), "青阳退款未获取银行流水记录");
                }
                else
                {
                    seqNum = IncomeAccount.HstSeqNum;
                    // IncomeAccount.Flag = 1;//修改标记
                    //enter.T_AHQY.ApplyCurrentValues(IncomeAccount);
                }
            }
            else
            {
                LogTxt.WriteEntry(string.Format("账号{0}未找到", accountNo), "青阳退款未获取银行流水记录");
            }
            return seqNum;
        }
        #endregion
    }
}
