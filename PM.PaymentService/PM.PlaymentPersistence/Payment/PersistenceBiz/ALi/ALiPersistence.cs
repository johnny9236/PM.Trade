using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentModel;
using PM.PlaymentPersistence.ORM;
using PM.PaymentProtocolModel;
using PM.Utils.Log;
using PM.PaymentProtocolModel.BankCommModel.ALiPay;
using PM.PaymentManger;
using PM.Utils;
using PM.PaymentProtocolModel.BankCommModel;

namespace PM.PlaymentPersistence.Payment.PersistenceBiz.ALi
{
    /// <summary>
    /// 支付宝
    /// </summary>
    public class ALiPersistence : PM.PlaymentPersistence.Payment.Persistence.Persistence
    {
        /// <summary>
        /// 业务系统发送  支付信息对象
        /// </summary>
        public PayStartModel PayReceiveModel
        {
            get;
            set;
        }
        public ALiPersistence(PayStartModel pOrder, publicEntities ef, CfgInfo cfg)
            : base(ef)
        {
            PayReceiveModel = pOrder;
            Cfg = cfg;
        }
        #region   重写
        /// <summary>
        /// 支付请求发起前 获取订单信息  （业务数据）设置订单号  订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected override bool SetRequestPayOrder(T_Pay_Order order)
        {
            if (null == order)
            {
                LogTxt.WriteEntry("业务对象T_ZTB_MoneyPayment为空", "支付宝支付日志");
                return false;
            }
            #region  订单赋值
            order.PrimaryID = PayReceiveModel.BusnissID;//关联键
            order.OrderName = PayReceiveModel.TradeInfo;
            order.OrderNo = PayReceiveModel.OrderNo;
            order.PayerID = PayReceiveModel.PayerID;
            order.PayerName = PayReceiveModel.PayerName;
            order.CostType = PayReceiveModel.Remark;

            order.Amount = PayReceiveModel.PayMoney;
            order.FeeAmount = order.FeeAmount;// PayReceiveModel.PayFee;
            order.Remark = PayReceiveModel.Remark;
            //order.InvoiceTitle = PayReceiveModel.InvoiceTitle;
            order.BusinessNo = PayReceiveModel.BusinessFunNo;//功能号

            #region 账号相关
            order.ReceiptAccountNo = PayReceiveModel.ReceiptAcountNo; //ConfigHelper.GetConfigString("ALiAccount");
            order.ReceiptAccountName = PayReceiveModel.ReceiptAcountName;
            order.ReceiptSettingAccNo = PayReceiveModel.ReceiptSettingAccNo;
            //order.ReceiptAccountNo = PayReceiveModel.ReceiptAcountNo;
            //order.ReceiptAccountName = PayReceiveModel.ReceiptAcountName;
            //order.ReceiptBankName = PayReceiveModel.ReceiptAccountDbBank;
            //order.ReceiptBankAccountType = PayReceiveModel.ReceiptBankAccountType;
            //order.ReceiptBankNo = PayReceiveModel.ReceiptOpenBankNo;//开户行行号
            //order.PayRealSettingAccNo = PayReceiveModel.ReceiptSettingAccNo;//结算账户
            //order.ReceiptBankID = PayReceiveModel.ReceiptBankID;//收款行id 
            //order.ReceiptProvince = PayReceiveModel.ReceiptProvince;//收款行id
            //order.ReceiptCity = PayReceiveModel.ReceiptCity;//收款省
            //order.ReceiptCur = PayReceiveModel.ReceiptCur;//收款行市 
            //order.PayBankID = PayReceiveModel.PayBankID;//银行id

            #endregion
            order.PayBankAccountType = PayReceiveModel.PayBankAccountType;// ((int)(BankName.个人支付银联在线)).ToString() == order.PayBankID ? ((int)(BankPayEnterTp.Person)).ToString() : ((int)(BankPayEnterTp.Enter)).ToString();
            order.OrderTime = DateTime.Now;
            //先添加并保存
            Entities.T_Pay_Order.AddObject(order);
            #endregion
            #region 业务关联
            var orderBusiness = Entities.T_Pay_OrderBusiness.Where(p => p.Business_ID == PayReceiveModel.BusnissID && p.OrderNo == PayReceiveModel.OrderNo).FirstOrDefault();
            if (null == orderBusiness)
            {
                orderBusiness = new T_Pay_OrderBusiness();
                orderBusiness.ID = Guid.NewGuid().ToString();
                orderBusiness.Business_ID = PayReceiveModel.BusnissID;
                //orderBusiness.InvoiceTitle = PayReceiveModel.InvoiceTitle;
                orderBusiness.ReMark = PayReceiveModel.Remark;
                orderBusiness.OrderNo = order.OrderNo;
                orderBusiness.Create_Tm = DateTime.Now;
                order.ReceiptInstitutionID = PayReceiveModel.InstitutionID;//机构编码
                Entities.T_Pay_OrderBusiness.AddObject(orderBusiness);
            }
            else
            {
                //原来支付就成功  提示
                if (orderBusiness.OrderStatus == PayFlag.PaySucess.ToString() || orderBusiness.OrderStatus == PayFlag.IncomeSucess.ToString())
                {
                    LogTxt.WriteEntry("支付对应订单已经处理成功，需要人工确认" + PayReceiveModel.BusnissID + "---" + PayReceiveModel.OrderNo, "支付宝支付信息");
                    return false;
                }
                else
                {
                    //orderBusiness.InvoiceTitle = PayReceiveModel.InvoiceTitle;
                    orderBusiness.ReMark = PayReceiveModel.Remark;
                    //orderBusiness.OrderNo = order.OrderNo;
                    orderBusiness.Up_Tm = DateTime.Now;
                    Entities.T_Pay_OrderBusiness.ApplyCurrentValues(orderBusiness);
                }
            }
            #endregion
            //Entities.SaveChanges();
            return true;
        }
        /// <summary>
        /// 支付业务发起后  业务处理
        /// </summary>
        /// <param name="sendInfo"></param>
        /// <param name="order"></param>
        /// <param name="rtnStr"></param>
        /// <returns></returns>
        protected override bool OprationBKRequestPay(ResultInfo sendInfo, T_Pay_Order order, out string rtnStr)
        {
            bool rtn = false;
            rtnStr = string.Empty;
            if (null == order)
            {
                LogTxt.WriteEntry("订单对象为空", "支付宝支付日志");
                return false;
            }
            #region 业务数据处理
            if (null != sendInfo && sendInfo.Result == ResultType.Success)
            {
                Entities.SaveChanges();//先保存下 
                var orderBusines = Entities.T_Pay_OrderBusiness.Where(p => p.Business_ID == order.PrimaryID && p.OrderNo == order.OrderNo).FirstOrDefault();//通过主键关联
                if (null == orderBusines)
                {
                    LogTxt.WriteEntry(string.Format("业务对象为空Business_ID[{0}] OrderNo[{1}]", order.PrimaryID, order.OrderNo), "支付宝支付日志");
                    return false;
                }
                //orderBusines.PayOrderNo = order.OrderNo;
                orderBusines.OrderStatus = ((int)PayFlag.Submit).ToString();
                orderBusines.Up_Tm = DateTime.Now;
                Entities.T_Pay_OrderBusiness.ApplyCurrentValues(orderBusines);
                try
                {
                    Entities.SaveChanges();
                    if (!string.IsNullOrEmpty(sendInfo.NoticeMsg))
                        rtnStr = sendInfo.NoticeMsg;//form提交信息
                    rtn = true;
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry(ex.Message, "支付宝支付日志");
                }
            }
            //else
            //{
            //    rtnStr = sendInfo.NoticeMsg + "■";//特殊符号标示错误的 需要前台提示
            //}
            #endregion
            return rtn;
        }

        /// <summary>
        /// 支付请求
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns></returns>
        protected override dynamic SetCallRemotePayCommunicationInfo(ORM.T_Pay_Order order)
        {
            string businessNO = "AliPay";
            var sendInfo = new ALiPayModel();

            sendInfo.OrderNo = order.OrderNo;
            sendInfo.BusinessFunNo = businessNO;
            #region  发送信息
            sendInfo.Body = order.OrderName;// order.OrderNo + "-" + businessNO;//订单号+费用类型
            sendInfo.Subject = sendInfo.Body;
            sendInfo.Partner = order.ReceiptAccountNo;//ConfigHelper.GetConfigString("ALiPartner");//支付宝合作者ID
            sendInfo.Account = order.ReceiptAccountName; ;
            sendInfo.Key = order.ReceiptSettingAccNo;
            sendInfo.Amount = order.Amount == null ? "0" : order.Amount.Value.ToString();

            sendInfo.Paymethod = "directPay";//支付宝支付 默认
            sendInfo.DefaultBank = "";
            sendInfo.DefaultBank = "";
            sendInfo.AntiPhishingKey = "";
            sendInfo.ExterInvokeIp = "";
            sendInfo.ExtraCommonParam = "";
            sendInfo.DefaultBuyAccount = "";
            sendInfo.RoyaltyType = "";
            sendInfo.RoyaltyParameters = "";

            sendInfo.InputCharset = ConfigHelper.GetCustomCfg("ALi", "ALiInputCharset");
            sendInfo.SignType = ConfigHelper.GetCustomCfg("ALi", "ALiSignType");

            sendInfo.OprationerID = order.PayerID;
            sendInfo.OprationerName = order.PayerName;
            sendInfo.Remark = order.Remark;
            #endregion
            return (ResultInfo)(Manager.PaymentManager(sendInfo, Cfg));//此地可以获取到配置信息就不需要在内部再次获取
        }
        #endregion


        #region   响应
        /// <summary>
        /// 支付响应
        /// </summary>
        /// <param name="resopnseModel">报文对象</param> 
        /// <returns></returns>
        protected override dynamic SetCallBackRemotePayCommunicationInfo(PayResopnseModel resopnseModel)
        {
          //  string businessNO = "AliPayRtn";
            var sendInfo = new PubCallBackModel();
            sendInfo.BusinessFunNo = resopnseModel.BusinessFunNo;
            #region  发送信息
            sendInfo.Signature = resopnseModel.Signature;
            sendInfo.MessagePaket = resopnseModel.Message;
            #endregion
            return (ResultInfo)(Manager.PaymentManager(sendInfo, Cfg));
        }
        /// <summary>
        /// 后续业务设置支付返回订单信息[ 1、 获取付款信息  2、订单信息修改  3、订单返回请求信息保存   4、业务表数据修改  5回调业务系统]
        /// </summary>
        /// <param name="info"></param>
        /// <param name="bkShow"></param>
        /// <param name="rtnStr"></param>
        /// <returns></returns>
        protected override bool OprationBKResponsePay(ResultInfo info, bool bkShow, out string rtnStr)
        {
            bool result = false;
            rtnStr = string.Empty;
            T_Pay_Order order = Entities.T_Pay_Order.Where(p => p.OrderNo == info.OrderNo).FirstOrDefault();
            if (null == order)
            {
                rtnStr = "返回获取业务对象OrderNo获取失败";
                LogTxt.WriteEntry("返回获取业务对象OrderNo获取失败", "支付宝支付日志");
                return result;
            }
            T_Pay_OrderBusiness orderBusines = Entities.T_Pay_OrderBusiness.Where(p => p.OrderNo == info.OrderNo).FirstOrDefault();
            if (null == orderBusines)
            {
                rtnStr = string.Format("返回订单号{0}未找到对应业务信息", info.OrderNo);
                LogTxt.WriteEntry(string.Format("返回订单号{0}未找到对应业务信息", info.OrderNo), "支付宝支付日志");
                return result;
            }
            // 如果是支付或结算返回直接修改支付情况
            #region   订单信息修改 保存逻辑
            order.Remark += string.Format("({0})", info.Status);//附加状态
            order.OrderResult = info.Result == ResultType.Success ? (int)OrderFlag.Sucess : (int)OrderFlag.Faile;//成功失败标记
            orderBusines.PayMent_Tm = DateTime.Now.AddDays(1);
            //看是否需 要匹配 付款银行
            orderBusines.OrderStatus = info.Result == ResultType.Success ? ((int)PayFlag.PaySucess).ToString() : ((int)PayFlag.PayFail).ToString();//入账成功 (目前使用该模式)
            #region  结算 预留
            #endregion
            Entities.T_Pay_OrderBusiness.ApplyCurrentValues(orderBusines);//修改    
            Entities.T_Pay_Order.ApplyCurrentValues(order);

            #endregion
            #region 支付返回的情况下获取支付账号信息
            #region  未有支付账号就取企业库里的用户信息(金华不需要)
            //UnitInfo unit = sc.GetUnitInfo(pr.UnitId.ToString());
            //if (null != unit)
            //{
            //    order.PayAccountName = unit.BankUserName;
            //    order.PayAccountNo = unit.BankCard;
            //    order.PayBankName = unit.OpenBank;
            //    //order.PayCity = info.TradeDetail.PayAccTraderBank.AccCity;
            //    //order.PayProvince = info.TradeDetail.PayAccTraderBank.AccPro;
            //}
            #endregion
            //设置支付账号
            if (null != info.TradeAccount)
            {
                order.PayRealAccountNo = info.TradeAccount.AccNo;
                order.PayAccountNo = info.TradeAccount.AccNo;
                Entities.T_Pay_Order.ApplyCurrentValues(order);
            }
            try
            {
                Entities.SaveChanges();
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(ex.Message + "继续发送给业务系统,订单号：" + order.OrderNo, "支付宝支付日志");
                //return result;
            }
            #region 回调给业务系统
            if (info.Result == ResultType.Success && !string.IsNullOrWhiteSpace(order.PayRealAccountNo))//回调给业务系统(失败就发送3次)
            { 
                PostBackToBusinesss(order, ConfigHelper.GetConfigString("BusinessUrl"), ConfigHelper.GetConfigString("enCoding"), ConfigHelper.GetConfigString("chkStr"));
            }
            #endregion
            #endregion

            #region   返回信息获取
            if (!bkShow)
            {
                rtnStr = string.Format(@"支付成功信息 <br>
                        订单号:{0}<br>
                        订单信息:{1}<br>
                        订单金额:{2}  ",
                                  order.OrderNo,
                                  order.OrderName,
                                  order.Amount + order.FeeAmount
                                  );
            }
            #endregion
            result = true;
            return result;
        }
        #endregion
    }
}
