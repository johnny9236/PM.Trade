﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Log;
using PM.Utils;
using PM.PaymentManger;
using PM.PaymentProtocolModel;
using PM.PaymentProtocolModel.BankCommModel.Netbank;
using PM.PaymentProtocolModel.BankCommModel;
using PM.PaymentModel;
using PM.PlaymentPersistence.ORM;

namespace PM.PlaymentPersistence.Payment.PersistenceBiz
{
    /// <summary>
    /// 银联支付业务
    /// </summary>
    public partial class NetBankPersistence : PM.PlaymentPersistence.Payment.Persistence.Persistence
    {
        /// <summary>
        /// 业务系统发送  支付信息对象
        /// </summary>
        public PayStartModel PayReceiveModel
        {
            get;
            set;
        }
        public NetBankPersistence(PayStartModel pOrder, publicEntities ef, CfgInfo cfg)
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
                LogTxt.WriteEntry("业务对象T_ZTB_MoneyPayment为空", "银联支付日志");
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
            order.FeeAmount = PayReceiveModel.PayFee;
            order.Remark = PayReceiveModel.Remark;
            //order.InvoiceTitle = PayReceiveModel.InvoiceTitle;
            order.BusinessNo = PayReceiveModel.BusinessFunNo;//功能号

            #region 账号相关
            order.ReceiptAccountNo = PayReceiveModel.ReceiptAcountNo;
            order.ReceiptAccountName = PayReceiveModel.ReceiptAcountName;
            order.ReceiptBankName = PayReceiveModel.ReceiptAccountDbBank;
            order.ReceiptBankAccountType = PayReceiveModel.ReceiptBankAccountType;
            order.ReceiptBankNo = PayReceiveModel.ReceiptOpenBankNo;//开户行行号
            order.PayRealSettingAccNo = PayReceiveModel.ReceiptSettingAccNo;//结算账户
            order.ReceiptSettingAccNo = PayReceiveModel.ReceiptSettingAccNo;//结算账户
            order.PayRealSettingAccNo = PayReceiveModel.ReceiptSettingAccNo;//结算账户
            order.ReceiptBankID = PayReceiveModel.ReceiptBankID;//收款行id

            order.ReceiptProvince = PayReceiveModel.ReceiptProvince;//收款行id
            order.ReceiptCity = PayReceiveModel.ReceiptCity;//收款省
            order.ReceiptCur = PayReceiveModel.ReceiptCur;//收款行市
            //机构号
            order.ReceiptInstitutionID = PayReceiveModel.InstitutionID;
            order.OrderName = PayReceiveModel.TradeInfo;
            #region    企业库用户信息  金华不需要
            //////获取支付行信息（通过登录用户 获取企业库）
            //////UnitInfo payInfo = sc.GetUnitInfo(biderID.ToString());
            ////order.PayBankName = userInfo..OpenBank;
            ////order.PayAccountNo = payInfo.BankCard;
            ////order.PayAccountName = payInfo.BankUserName;
            ////order.PayProvince = payInfo.PayBankProvince;
            ////order.PayCity = payInfo.PayBankCity;

            ////order.PayBankNo = payInfo.OpenBankNo;
            #endregion
            order.PayBankID = PayReceiveModel.PayBankID;//银行id
            //设置真正的支付银行ID
            //order.PayRealBankID = PayOrder.PayBankID;//银行代码
            //order.PayBankNo = "";//开户行行号
            #endregion

            order.PayBankAccountType = ((int)(BankName.个人支付银联在线)).ToString() == order.PayBankID ? ((int)(BankPayEnterTp.Person)).ToString() : ((int)(BankPayEnterTp.Enter)).ToString();
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
                Entities.T_Pay_OrderBusiness.AddObject(orderBusiness);
            }
            else
            {
                //原来支付就成功  提示
                if (orderBusiness.OrderStatus == PayFlag.PaySucess.ToString() || orderBusiness.OrderStatus == PayFlag.IncomeSucess.ToString())
                {
                    LogTxt.WriteEntry("支付对应订单已经处理成功，需要人工确认,订单号:" + PayReceiveModel.OrderNo + "-业务号" + PayReceiveModel.BusnissID, "银联支付日志");
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
                LogTxt.WriteEntry("订单对象为空", "银联支付日志");
                return false;
            }
            #region 业务数据处理
            if (null != sendInfo && sendInfo.Result == ResultType.Success)
            {
                try
                {
                    Entities.SaveChanges();//先保存下 
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry(ex.Message, "银联支付日志");
                }
                var orderBusines = Entities.T_Pay_OrderBusiness.Where(p => p.Business_ID == order.PrimaryID && p.OrderNo == order.OrderNo).FirstOrDefault();//通过主键关联
                if (null == orderBusines)
                {
                    LogTxt.WriteEntry(string.Format("业务对象为空Business_ID[{0}]OrderNo[{1}]", order.PrimaryID, order.OrderNo), "银联支付日志");
                    return false;
                }
                //orderBusines.PayOrderNo = order.OrderNo;
                orderBusines.OrderStatus = ((int)PayFlag.Submit).ToString();
                orderBusines.Up_Tm = DateTime.Now;
                Entities.T_Pay_OrderBusiness.ApplyCurrentValues(orderBusines);
                try
                {
                    Entities.SaveChanges();
                    if (!string.IsNullOrEmpty(sendInfo.ActionURLToBank))
                        rtnStr = Build_Form(sendInfo);
                    rtn = true;
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry(ex.Message, "银联支付日志");
                }
            }
            else
            {
                rtnStr = sendInfo.NoticeMsg + "■";//特殊符号标示错误的 需要前台提示
            }
            #endregion
            return rtn;
        }
        /// <summary>
        /// 支付请求发起
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected override dynamic SetCallRemotePayCommunicationInfo(T_Pay_Order order)
        {
            //string businessNO = "1111";
            var sendInfo = new NetBankPayRequestModel();

            sendInfo.OrderNo = order.OrderNo;
            sendInfo.BusinessFunNo = order.BusinessNo;
            #region  发送信息
            sendInfo.Usage = order.OrderNo + "-" + order.OrderName;//订单号+费用类型
            // sendInfo.NotificationURL=order.NotificationURL
            sendInfo.AccType = order.PayBankAccountType;//付款银行类型   企业还是个人

            sendInfo.Amount = (double)order.Amount;
            sendInfo.BankID = order.PayBankID;
            sendInfo.Free = (double)order.FeeAmount;
            sendInfo.InstitutionID = order.ReceiptInstitutionID;
            sendInfo.OprationerID = order.PayerID;
            sendInfo.OprationerName = order.PayerName;
            sendInfo.Remark = order.Remark;
            sendInfo.SettingAccNo = order.ReceiptSettingAccNo;//结算账号 或标识

            #endregion
            return (ResultInfo)(Manager.PaymentManager(sendInfo, Cfg));//此地可以获取到配置信息就不需要在内部再次获取
        }
        /// <summary>
        /// 支付响应通讯
        /// </summary>
        /// <param name="resopnseModel">返回报文</param> 
        /// <returns></returns>
        protected override dynamic SetCallBackRemotePayCommunicationInfo(PayResopnseModel resopnseModel)
        {
            //string businessNO = "1118";
            var sendInfo = new NetBankPayResponseModel();
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
            string queryPayerBusinessNo = "1121";//特殊查询
            T_Pay_Order order = Entities.T_Pay_Order.Where(p => p.OrderNo == info.OrderNo).FirstOrDefault();
            if (null == order)
            {
                rtnStr = "返回获取业务对象OrderNo获取失败";
                LogTxt.WriteEntry("返回获取业务对象OrderNo获取失败", "银联支付日志");
                return result;
            }
            T_Pay_OrderBusiness orderBusines = Entities.T_Pay_OrderBusiness.Where(p => p.OrderNo == info.OrderNo).FirstOrDefault();
            if (null == orderBusines)
            {
                rtnStr = string.Format("返回订单号{0}未找到对应业务信息", info.OrderNo);
                LogTxt.WriteEntry(string.Format("返回订单号{0}未找到对应业务信息", info.OrderNo), "银联支付日志");
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
            var traderBankInfo = GetTrader(order, queryPayerBusinessNo);//业务功能号
            if (null != traderBankInfo && null != traderBankInfo.TradeAccount)
            {
                order.PayRealAccountName = traderBankInfo.TradeAccount.AccDbName;
                order.PayRealAccountNo = traderBankInfo.TradeAccount.AccNo;
                order.PayRealCity = traderBankInfo.TradeAccount.AccCity;
                order.PayRealProvince = traderBankInfo.TradeAccount.AccPro;
                order.PayRealBankName = traderBankInfo.TradeAccount.AccDBBank;
                Entities.T_Pay_Order.ApplyCurrentValues(order);
            }
            try
            {
                Entities.SaveChanges();
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(ex.Message + "继续发送给业务系统,订单号：" + order.OrderNo, "银联支付日志");
                //return result;
            }
            #region 回调给业务系统
            //if (info.Result == ResultType.Success && !string.IsNullOrWhiteSpace(order.PayRealAccountNo))//回调给业务系统(失败就发送3次)
            if (info.Result == ResultType.Success)//回调给业务系统(失败就发送3次)  
            {
                var businessPost = new Action<T_Pay_Order, string, string, string>(PostBackToBusinesss);
                businessPost.BeginInvoke(order, ConfigHelper.GetConfigString("BusinessUrl"), ConfigHelper.GetConfigString("enCoding"), ConfigHelper.GetConfigString("chkStr"), null, null);
                //  PostBackToBusinesss(order, ConfigHelper.GetConfigString("BusinessUrl"), ConfigHelper.GetConfigString("enCoding"), ConfigHelper.GetConfigString("chkStr"));
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
                                  order.Amount ?? 0 + order.FeeAmount ?? 0
                                  );
            }
            else
            {
                rtnStr = info.NoticeMsg;//输出
            }
            #endregion
            result = true;
            return result;
        }
        /// <summary>
        /// 操作类型获取(用于响应时 对应的操作)
        /// </summary> 
        /// <param name="resopnseModel">报文对象</param>
        /// <returns></returns>
        protected override string GetOpKind(PayResopnseModel resopnseModel)
        {
            return base.GetOpKind(resopnseModel);
        }
        #endregion
        #region  辅助方法

        /// <summary>
        /// 获取支付人信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected QueryPayerDetailModel GetTrader(T_Pay_Order order, string businessNo)
        {
            var queryTrader = new QueryPayerDetailModel();
            #region 支付返回的情况下获取支付账号信息
            try
            {
                queryTrader.BusinessFunNo = businessNo;
                queryTrader.PaymentNo = order.OrderNo;
                queryTrader.InstitutionID = order.ReceiptInstitutionID;
                //var manager = new CustomCommManager();
                //var info = (QueryPayerDetail)(manager.CallProtocol(queryTrader, SysConfigModel));
                var info = (QueryPayerDetailModel)(Manager.PaymentManager(queryTrader));
                if (info != null && info.Result && info.TradeAccount != null)
                {
                }
                else
                {
                    queryTrader = null;
                }
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("获取支付人信息:" + ex.Message, "银联支付日志");
            }
            #endregion
            return queryTrader;
        }


        /// <summary>
        /// 获取支付提交html
        /// </summary>
        /// <param name="sendInfo">支付消息</param>
        /// <returns></returns>
        private string Build_Form(ResultInfo sendInfo)
        {
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<form id=\"paySubmit\"  target=\"_blank\"    name=\"paySubmit\" action=\"" + sendInfo.ActionURLToBank + "\"    accept-charset=\"utf-8\"    method=\"post\">");
            sbHtml.Append("<input type=\"hidden\" name=\"message\" value=\"" + sendInfo.MessagePaket + "\"/>");
            sbHtml.Append("<input type=\"hidden\" name=\"signature\" value=\"" + sendInfo.Signature + "\"/>");
            sbHtml.Append(" </form>");
            sbHtml.Append("<script>document.forms['paySubmit'].submit();</script>");
            return sbHtml.ToString();
        }
        #endregion
    }
}