using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentModel;
using PM.PaymentManger;
using PM.PaymentProtocolModel.BankCommModel.JSABOC;
using PM.Utils.Log;
using PM.PaymentProtocolModel.BankCommModel;

namespace PM.PlaymentPersistence.Payment.PersistenceBiz.JSABOC
{
    /// <summary>
    /// 退款
    /// </summary>
    public class JSABOCPersistence
    {
        /// <summary>
        /// 数据库对象
        /// </summary>
        JSABOC dbEnter = new JSABOC();
        /// <summary>
        /// 退保证金请求
        /// </summary>
        /// <param name="payRefundMode">业务退款对象</param>
        /// <returns>返回  成功标段业务主键ID</returns>
        public string DoRefundPay(PayRefundModel payRefundMode)
        {
            string rtnStr = string.Empty;
            JSABOCRefoundModel refoundModel = null;//退款对象
            T_JSABOC aboc = null;//退款明细入库
            foreach (var dtl in payRefundMode.PayRefundDtl)
            {
                refoundModel = new JSABOCRefoundModel();
                refoundModel.BusinessFunNo = "ZTB2";
                refoundModel.TradeDate = DateTime.Now.ToString("yyyyMMdd");
                refoundModel.SectionNo = dtl.Remark;//业务对象   备注标段编号
                refoundModel.ABOCRemark = dtl.BusnissID;// 备注使用主键id
                refoundModel.OrderNo = dtl.OrderNo;
                refoundModel.ReceiveAccNo = dtl.ReceiptAcountNo;
                refoundModel.ReceiveAccDBBank = dtl.ReceiptAccountDbBank;
                refoundModel.ReceiveAccDbName = dtl.ReceiptAcountName;
                refoundModel.RealAmount = dtl.PayMoney;
                refoundModel.Amount = dtl.PayMoney - dtl.RateMoney;
                try
                {
                    aboc = RefoundToDB(refoundModel);// 交易信息入库
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry(ex.Message + ex.Source, "嘉善保证金退回发起");
                    continue;//保证先写  记录表
                }
                if (Manager.PaymentManager(refoundModel))//协议发送成功
                {
                    try
                    {
                        aboc.Flag = 1;//发起成功
                        dbEnter.T_JSABOC.ApplyCurrentValues(aboc);
                        rtnStr += refoundModel.ABOCRemark + ",";//备注存放 业务主键ID 
                    }
                    catch (Exception ex)
                    {
                        LogTxt.WriteEntry(ex.Message + ex.Source, "嘉善保证金退回发起");
                    }
                }
            }
            try
            {
                dbEnter.SaveChanges();//保存数据库
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(ex.Message + ex.Source, "嘉善保证金退回发起");
            }
            return rtnStr;
        }

        /// <summary>
        /// 响应保证退回通知
        /// </summary>
        /// <param name="message">报文字符串</param>
        /// <returns>返回给客户端报文</returns>
        public string HttpPayCallback(string message)
        {
            bool rtn = false;//是否需要更新标记
            PubCallBackModel callBackProtol = new PubCallBackModel();//响应协议对象
            string rtnString = string.Empty;
            if (!string.IsNullOrEmpty(message))
            {
                callBackProtol.BusinessFunNo = "ZTB3";
                callBackProtol.MessagePaket = message;
                var rtnModel = (JSABOCBizModel)Manager.PaymentManager(callBackProtol);//解析报文
                //  rtnString = rtnModel.RtnStr;//返回报文
                rtnString = rtnModel.RtnToProtol.GetSendStr();
                if (null == rtnModel && null == rtnModel.RtnToProtol)
                {
                    LogTxt.WriteEntry("保证金业务ZTB3响应:报文" + message + "解析报文失败", "嘉善保证金退回响应");
                    var rtnErrModel = new JSABOCRtnModel();//返回错误信息
                    rtnErrModel.TradeCode = "ZTB3";
                    rtnErrModel.TradeStructNum = "001";
                    rtnErrModel.ReturneCode = "0002";//默认错误
                    rtnErrModel.ReturneMsg = "报文解析失败";//默认错误
                    return rtnErrModel.GetSendStr();//返回错误信息
                }
                foreach (var pay in rtnModel.RtnModels)//退回保证金响应
                {
                    try
                    {
                        if (RefoundCallbackToDB(pay))//入库响应信息
                        {
                            //  LogTxt.WriteEntry("保证金业务ZTB3响应:订单号" + pay.OrderNo + "插入报文记录表", "嘉善保证金退回响应");
                            rtn = true;
                            if (UpdatePaymentStatus(pay, ref rtnModel))
                            {
                                LogTxt.WriteEntry("保证金业务ZTB3响应:订单号" + pay.OrderNo + "更新入账表", "嘉善保证金退回响应");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogTxt.WriteEntry("保证金业务ZTB3响应:" + ex.Message + ex.Source, "嘉善保证金退回响应");
                    }
                }
                if (rtn)//有更新
                {
                    try
                    {
                        dbEnter.SaveChanges();
                        rtnString = rtnModel.RtnToProtol.GetSendStr();
                    }
                    catch (Exception ex)
                    {
                        LogTxt.WriteEntry("保证金业务ZTB3响应:" + ex.Message + ex.Source, "嘉善保证金退回响应");
                    }
                }
            }
            LogTxt.WriteEntry("保证金业务ZTB3响应 接受报文:" + message, "嘉善保证金退回响应银行报文");
            LogTxt.WriteEntry("保证金业务ZTB3响应 发送报文:"+ rtnString, "嘉善保证金退回响应银行报文");
            return rtnString;
        }

        #region  退保证金请求记录入库
        /// <summary> 
        /// 退保证金请求记录入库
        /// </summary>
        /// <param name="refoundModel">退款对象</param>
        /// <returns>交易记录对象入库</returns>
        private T_JSABOC RefoundToDB(JSABOCRefoundModel refoundModel)
        {
            T_JSABOC aboc = new T_JSABOC();
            aboc.ID = Guid.NewGuid().ToString();
            aboc.TradeCode = "ZTB2";
            aboc.TradeStructNum = "001";
            aboc.DetailDataTime = refoundModel.TradeDate;
            aboc.SectionNo = refoundModel.SectionNo; //标段编号
            aboc.OrderNo = refoundModel.OrderNo;
            aboc.ReceiveAccNo = refoundModel.ReceiveAccNo;
            aboc.ReceiveAccDBBank = refoundModel.ReceiveAccDBBank;
            aboc.ReceiveAccDbName = refoundModel.ReceiveAccDbName;
            aboc.RealAmount = refoundModel.RealAmount;
            aboc.CreateTm = DateTime.Now;
            aboc.Amount = refoundModel.Amount;
            dbEnter.T_JSABOC.AddObject(aboc);
            dbEnter.SaveChanges();
            return aboc;
        }

        #endregion
        #region  退保证金响应记录入库
        /// <summary> 
        /// 退保证金响应记录入库
        /// </summary>
        /// <param name="refoundModel">退款对象</param>
        /// <returns>交易记录对象入库</returns>
        private bool RefoundCallbackToDB(JSABOCRtnModel refoundModel)
        {
            bool rtn = false;
            T_JSABOC aboc = new T_JSABOC();
            aboc.ID = Guid.NewGuid().ToString();
            aboc.TradeCode = refoundModel.TradeCode;
            aboc.TradeStructNum = refoundModel.TradeStructNum;
            //aboc.DetailDataTime = refoundModel.TradeDate;
            aboc.SectionNo = refoundModel.SectionNo; //标段编号
            aboc.OrderNo = refoundModel.OrderNo;
            aboc.ReceiveAccNo = refoundModel.ReceiveAccNo;
            aboc.ReceiveAccDBBank = refoundModel.ReceiveAccDBBank;
            aboc.ReceiveAccDbName = refoundModel.ReceiveAccDbName;
            aboc.RealAmount = refoundModel.RealAmount;
            aboc.Amount = refoundModel.Amount;
            aboc.Summary = refoundModel.Summary;
            aboc.SerialNumber = refoundModel.SerialNumber;
            aboc.ReturneCode = refoundModel.ReturneCode;
            aboc.ReturneMsg = refoundModel.ReturneMsg;
            aboc.CreateTm = DateTime.Now;
            dbEnter.T_JSABOC.AddObject(aboc);
            dbEnter.SaveChanges();//先保存
            rtn = true;
            return rtn;
        }

        /// <summary>
        /// 更新入账表退保证金状态
        /// </summary> 
        /// <param name="orderModel">协议对象</param>
        /// <param name="rtnModel">业务返回对象</param>
        /// <returns></returns>
        private bool UpdatePaymentStatus(JSABOCRtnModel orderModel, ref JSABOCBizModel rtnModel)
        {
            bool rtn = false;
            var payMent = dbEnter.T_ZTB_BidMoneyPayReturn.Where(p => p.ABOCReturnOrderNo == orderModel.OrderNo).FirstOrDefault();
            if (null != payMent)
            {

                bool abocMatchFlag = false;//农行账号匹配是否成功
                if (((payMent.PayAccDBBank.IndexOf("农行") > -1) || (payMent.PayAccDBBank.IndexOf("农业银行") > -1)))
                {
                    var payBankAcc = payMent.PayAccNo.Trim().ToLower().Length > 15 ? payMent.PayAccNo.Trim().ToLower().Substring(payMent.PayAccNo.Trim().Length - 15) : payMent.PayAccNo.Trim().ToLower();
                    var orderBankAcc = orderModel.ReceiveAccNo.Trim().ToLower().Length > 15 ? orderModel.ReceiveAccNo.Trim().ToLower().Substring(orderModel.ReceiveAccNo.Trim().Length - 15) : orderModel.ReceiveAccNo.Trim().ToLower();
                    abocMatchFlag = payBankAcc == orderBankAcc;
                }
                else
                {
                    abocMatchFlag = payMent.PayAccNo.Trim().ToLower() == orderModel.ReceiveAccNo.Trim().ToLower();
                }
                if ((payMent.RtnTotalMoney == orderModel.RealAmount && abocMatchFlag))//金额及账号相同则更新
                {
                    if (orderModel.ReturneCode == "0000")
                    {
                        if (payMent.IsReturn != 1)
                        {
                            payMent.IsReturn = 1;
                            dbEnter.T_ZTB_BidMoneyPayReturn.ApplyCurrentValues(payMent);
                            rtn = true;
                        }
                        else
                        {
                            LogTxt.WriteEntry("保证金业务ZTB3响应:订单号" + orderModel.OrderNo + "已经入账处理", "嘉善保证金退回响应");
                        }
                    }
                    else
                    {
                        payMent.IsReturn = 0;
                        dbEnter.T_ZTB_BidMoneyPayReturn.ApplyCurrentValues(payMent);
                        LogTxt.WriteEntry("保证金业务ZTB3响应:订单号" + orderModel.OrderNo + "入账状态修改为失败", "嘉善保证金退回响应");
                        rtnModel.RtnToProtol.ReturneCode = "0000";
                        rtnModel.RtnToProtol.ReturneMsg = "处理订单号：" + orderModel.OrderNo + "状态为未入账";
                        rtn = true;
                    }
                }
                else
                {
                    if ((payMent.RtnTotalMoney != orderModel.RealAmount))
                    {
                        rtnModel.RtnToProtol.ReturneCode = "9998";
                        rtnModel.RtnToProtol.ReturneMsg = "金额不匹配";
                        LogTxt.WriteEntry("保证金业务ZTB3响应:订单号" + orderModel.OrderNo + "金额不匹配", "嘉善保证金退回响应");
                    }
                    else
                    {
                        rtnModel.RtnToProtol.ReturneCode = "9997";
                        rtnModel.RtnToProtol.ReturneMsg = "账号不匹配";
                        LogTxt.WriteEntry("保证金业务ZTB3响应:订单号" + orderModel.OrderNo + "账号不匹配", "嘉善保证金退回响应");
                    }
                }
            }
            else
            {
                rtnModel.RtnToProtol.ReturneCode = "9996";
                rtnModel.RtnToProtol.ReturneMsg = "未找到对应业务数据";
                LogTxt.WriteEntry("保证金业务ZTB3响应:订单号" + orderModel.OrderNo + "未找到对应业务数据", "嘉善保证金退回响应未找到业务数据");
            }
            return rtn;
        }

        #endregion
    }
}
