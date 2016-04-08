using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFCA.Payment.Api;
using PM.NetBankPtlBiz.Model;
using PM.PaymentProtocolModel;
using PM.PaymentProtocolModel.BankCommModel.Netbank;
 

namespace PM.NetBankPtlBiz.Protocols
{
    /// <summary>
    /// 支付相关
    /// </summary>
    public partial class NetBankProtocols
    {
        #region 支付请求
        /// <summary>
        /// 网银支付请求
        /// </summary>
        /// <param name="payInfomation"></param>
        /// <returns></returns>
        private ResultInfo GetNetBankPayRequest(NetBankPayRequestModel sendInfo, CfgInfo cfgInfo)
        {
            //  var sendInfo = payInfomation as NetBankPayRequestModel;
            PayInfo payInfo = new PayInfo();
            payInfo.AccountType = int.Parse(sendInfo.AccType);
            payInfo.Amount = Convert.ToInt64(sendInfo.Amount * 100);
            payInfo.BankID = sendInfo.BankID;
            payInfo.Fee = Convert.ToInt64(sendInfo.Free * 100);//Convert.ToInt64(sendInfo.TradeDetail.Free * 100);
            payInfo.InstitutionID = sendInfo.InstitutionID;
            payInfo.NotificationURL = cfgInfo.NotificationURL;//sendInfo.TradeProduct.NotificationURL;
            //payInfo.PayerID = sendInfo.TradeDetail.PayAccTrade.UserID;//操作人
            //payInfo.PayerName = sendInfo.TradeDetail.PayAccTrade.UserName;//操作人姓名
            payInfo.PayerID = sendInfo.OprationerID;//操作人 
            payInfo.PayerName = sendInfo.OprationerName;//操作人 
            payInfo.PaymentNo = sendInfo.OrderNo;
            payInfo.Usage = sendInfo.Usage;//用途
            payInfo.SettlementFlag = sendInfo.SettingAccNo;//收款方结算标示
            payInfo.Remark = sendInfo.Remark;
            PayRequest(payInfo);

            ResultInfo rInfo = new ResultInfo();
            rInfo.ActionURLToBank = payInfo.ActionURL;
            if (!string.IsNullOrEmpty(payInfo.ActionURL))
                rInfo.Result = ResultType.Success;
            rInfo.MessagePaket = payInfo.Message;
            rInfo.PlanText = payInfo.PlainText;
            rInfo.Signature = payInfo.Signature;
            rInfo.TxCode = ((int)payInfo.txCode).ToString();
            rInfo.TxName = payInfo.txName;
            return rInfo;
        }
        /// <summary>
        /// 支付请求
        /// </summary>
        /// <param name="payInfo">请求信息</param>
        /// <returns></returns>
        public PayInfo PayRequest(PayInfo payInfo)
        {
            //1.创建交易请求对象
            Tx1111Request tx1111Request = new Tx1111Request();
            tx1111Request.setInstitutionID(payInfo.InstitutionID);
            tx1111Request.setPaymentNo(payInfo.PaymentNo);
            tx1111Request.setAmount(payInfo.Amount);
            tx1111Request.setFee(payInfo.Fee);
            tx1111Request.setPayerID(payInfo.PayerID);
            tx1111Request.setPayerName(payInfo.PayerName);
            tx1111Request.setSettlementFlag(payInfo.SettlementFlag);
            tx1111Request.setUsage(payInfo.Usage);
            tx1111Request.setRemark(payInfo.Remark);
            tx1111Request.setNotificationURL(payInfo.NotificationURL);
            tx1111Request.setBankID(payInfo.BankID);
            tx1111Request.setAccountType(payInfo.AccountType);

            // 2.执行报文处理
            tx1111Request.process();
            payInfo.PlainText = tx1111Request.getRequestPlainText();
            payInfo.Message = tx1111Request.getRequestMessage();
            payInfo.Signature = tx1111Request.getRequestSignature();
            payInfo.ActionURL = PaymentEnvironment.PaymentURL;
            payInfo.txCode = BusinessType.Pay;
            payInfo.txName = "商户订单支付";
            return payInfo;
        }
        #endregion

        #region   1112不确认支付请求
        /// <summary>
        /// 网银支付请求
        /// </summary>
        /// <param name="payInfomation"></param>
        /// <returns></returns>
        private ResultInfo GetNetBankUndeterminedPayRequest(NetBankPayRequestModel sendInfo, CfgInfo cfgInfo)
        {
            //  var sendInfo = payInfomation as NetBankPayRequestModel;
            PayInfo payInfo = new PayInfo();
            payInfo.AccountType = int.Parse(sendInfo.AccType);
            payInfo.Amount = Convert.ToInt64(sendInfo.Amount * 100);
            payInfo.BankID = sendInfo.BankID;
            payInfo.Fee = Convert.ToInt64(sendInfo.Free * 100);//Convert.ToInt64(sendInfo.TradeDetail.Free * 100);
            payInfo.InstitutionID = sendInfo.InstitutionID;
            payInfo.NotificationURL = cfgInfo.NotificationURL;//sendInfo.TradeProduct.NotificationURL;
            //payInfo.PayerID = sendInfo.TradeDetail.PayAccTrade.UserID;//操作人
            //payInfo.PayerName = sendInfo.TradeDetail.PayAccTrade.UserName;//操作人姓名
            payInfo.PayerID = sendInfo.OprationerID;//操作人 
            payInfo.PayerName = sendInfo.OprationerName;//操作人 
            payInfo.PaymentNo = sendInfo.OrderNo;
            payInfo.Usage = sendInfo.Usage;//用途
            payInfo.SettlementFlag = sendInfo.SettingAccNo;//收款方结算标示
            payInfo.Remark = sendInfo.Remark;
            payInfo.Note = sendInfo.Usage;
            PayUndeterminedRequest(payInfo);

            ResultInfo rInfo = new ResultInfo();
            rInfo.ActionURLToBank = payInfo.ActionURL;
            if (!string.IsNullOrEmpty(payInfo.ActionURL))
                rInfo.Result = ResultType.Success;
            rInfo.MessagePaket = payInfo.Message;
            rInfo.PlanText = payInfo.PlainText;
            rInfo.Signature = payInfo.Signature;
            rInfo.TxCode = ((int)payInfo.txCode).ToString();
            rInfo.TxName = payInfo.txName;
            return rInfo;
        }
        /// <summary>
        /// 支付请求
        /// </summary>
        /// <param name="payInfo">请求信息</param>
        /// <returns></returns>
        public PayInfo PayUndeterminedRequest(PayInfo payInfo)
        {
            //1.创建交易请求对象
            Tx1112Request tx1112Request = new Tx1112Request();
            tx1112Request.setInstitutionID(payInfo.InstitutionID);
            tx1112Request.setPaymentNo(payInfo.PaymentNo);
            tx1112Request.setAmount(payInfo.Amount); 

            tx1112Request.setPayerID(payInfo.PayerID);
            tx1112Request.setPayerName(payInfo.PayerName);
            tx1112Request.setSettlementFlag(payInfo.SettlementFlag);
            tx1112Request.setUsage(payInfo.Usage);
            tx1112Request.setRemark(payInfo.Remark);
            tx1112Request.setNotificationURL(payInfo.NotificationURL);
            tx1112Request.setNote(payInfo.Note);  
            // 2.执行报文处理
            tx1112Request.process();


            payInfo.PlainText = tx1112Request.getRequestPlainText();
            payInfo.Message = tx1112Request.getRequestMessage();
            payInfo.Signature = tx1112Request.getRequestSignature();
            payInfo.ActionURL = PaymentEnvironment.PaymentURL;
            payInfo.txCode = BusinessType.UndeterminedPay;
            payInfo.txName = "商户订单支付（不要确认）";
            return payInfo;
        }
        #endregion






        #region 支付响应
        /// <summary>
        /// 网银通知返回(不包含结算)
        /// </summary>
        /// <param name="sendInfo"></param>
        /// <param name="cfgInfo"></param>
        /// <returns></returns>
        private ResultInfo GetNetbankPayOrTransResponse(NetBankPayResponseModel sendInfo, CfgInfo cfgInfo)
        {
            // var sendInfo = sendInfomation as NetBankPayResponseModel;
            //报文及密匙
            Notice1118Or1318Or1348ResponseInfo info = PayResponse(sendInfo.MessagePaket, sendInfo.Signature);
            ResultInfo rInfo = new ResultInfo();
            if (info.Result)
            {
              //  rInfo.Status = info.Status;
                rInfo.MSG = info.InstitutionID;//机构号// cfgInfo.StructCode;//
                rInfo.Result = info.Result == true ? ResultType.Success : ResultType.Faile;
                rInfo.OrderNo = info.PaymentNo;
                if (!string.IsNullOrEmpty(info.SerialNumber))
                    rInfo.SerialNumber = info.SerialNumber;

                //if (sendInfo.TradeDetail == null)
                //    sendInfo.TradeDetail = new TradeDetail();
                //sendInfo.TradeDetail.Amount = (Convert.ToDouble(info.Amount)) / 100;
                //sendInfo.TradeDetail.TradeTm = DateTime.Now;
                if (!string.IsNullOrEmpty(info.Status.ToString()))
                    rInfo.Status = info.Status;
            }
            else
            {
                rInfo.Result = info.Result == true ? ResultType.Success : ResultType.Faile;
            }
            //通知给网银
            rInfo.NoticeMsg = info.MessageResponse;
            return rInfo;
        }
        /// <summary>
        /// 支付响应(目前公用)
        /// </summary>
        /// <param name="message">报文</param>
        /// <param name="signature">签名</param>
        /// <returns></returns>
        public Notice1118Or1318Or1348ResponseInfo PayResponse(string message, string signature)
        {
            Notice1118Or1318Or1348ResponseInfo responseInfo = new Notice1118Or1318Or1348ResponseInfo();

            string txName = string.Empty;
            bool result = false;
            //1生成交易结果对象
            NoticeRequest noticeRequest = new NoticeRequest(message, signature);

            //2业务处理
            if (((int)BusinessType.PayResponse).ToString().Equals(noticeRequest.getTxCode()))
            {
                Notice1118Request nr = new Notice1118Request(noticeRequest.getDocument());
                //！！！ 在这里添加商户处理逻辑！！！
                //以下为演示代码
                txName = "商户订单支付状态变更通知";
                result = true;
                responseInfo.InstitutionID = nr.getInstitutionID();
                responseInfo.PaymentNo = nr.getPaymentNo();
                responseInfo.Amount = nr.getAmount();
                responseInfo.Status = nr.getStatus();
            }
            //else if ("1138".Equals(noticeRequest.getTxCode()))
            //{
            //    Notice1138Request nr = new Notice1138Request(noticeRequest.getDocument());
            //    //！！！ 在这里添加商户处理逻辑！！！
            //    //以下为演示代码
            //    txName = "商户订单退款结算状态变更通知";

            //    Console.WriteLine("[OrderNo]      = [" + nr.getInstitutionID() + "]");
            //    Console.WriteLine("[SerialNumber] = [" + nr.getSerialNumber() + "]");
            //    Console.WriteLine("[OrderNo]      = [" + nr.getPaymentNo() + "]");
            //    Console.WriteLine("[Amount]       = [" + nr.getAmount() + "]");
            //    Console.WriteLine("[Status]       = [" + nr.getStatus() + "]");
            //    Console.WriteLine("[Status]       = [" + nr.getRefundTime() + "]");

            //}
            else if (((int)BusinessType.TransferResponse).ToString().Equals(noticeRequest.getTxCode()))
            {
                Notice1318Request nr = new Notice1318Request(noticeRequest.getDocument());
                //！！！ 在这里添加商户处理逻辑！！！

                txName = "市场订单支付状态变更通知";
                result = true;
                responseInfo.InstitutionID = nr.getInstitutionID();
                responseInfo.PaymentNo = nr.getPaymentNo();
                responseInfo.Amount = nr.getAmount();
                responseInfo.Status = nr.getStatus();
            }
            else if (((int)BusinessType.TransferClearNotice).ToString().Equals(noticeRequest.getTxCode()))
            {
                Notice1348Request nr = new Notice1348Request(noticeRequest.getDocument());
                //！！！ 在这里添加商户处理逻辑！！！
                txName = "市场订单结算状态变更通知";
                result = true;
                responseInfo.InstitutionID = nr.getInstitutionID();
                responseInfo.PaymentNo = nr.getOrderNo();
                responseInfo.Amount = nr.getAmount();
                responseInfo.Status = nr.getStatus();
                responseInfo.SerialNumber = nr.getSerialNumber();
            }
            //else if ("1438".Equals(noticeRequest.getTxCode()))
            //{
            //    Notice1438Request nr = new Notice1438Request(noticeRequest.getDocument());
            //    //！！！ 在这里添加商户处理逻辑！！！
            //}
            else
            {
                txName = "未知通知类型";
            }
            responseInfo.Result = result;
            //响应支付平台
            responseInfo.MessageResponse = Convert.ToBase64String(Encoding.UTF8.GetBytes(new NoticeResponse().getMessage()));
            responseInfo.TxCode = noticeRequest.getTxCode();
            responseInfo.PlainText = noticeRequest.getPlainText();
            responseInfo.TxName = txName;
            return responseInfo;
        }
        #endregion

        #region 转账请求
        /// <summary>
        /// 网银转账发起
        /// </summary>
        /// <param name=" cfgInfo"></param>
        /// <returns></returns>
        private ResultInfo GetNetBankTransPayRequest(NetBankPayRequestModel sendInfo, CfgInfo cfgInfo)
        {
            Pay1311Info payInfo = new Pay1311Info();
            //待定
            payInfo.InstitutionID = sendInfo.InstitutionID;//sendInfo.StructCode;//机构号
            payInfo.OrderNo = sendInfo.OrderNo;
            payInfo.PaymentNo = sendInfo.OrderNo;
            payInfo.Amount = Convert.ToInt64(sendInfo.Amount * 100);
            payInfo.Fee = Convert.ToInt64(sendInfo.Free * 100);//Convert.ToInt64(cfgInfo.Free * 100);//
            // payInfo.PayerID = sendInfo.Oprationer;//操作人
            //payInfo.PayerName = sendInfo.TradeDetail.PayAccTrade.UserName;//操作人姓名
            payInfo.PayerID = sendInfo.OprationerID;//操作人
            payInfo.PayerName = sendInfo.OprationerName;//操作人
            payInfo.Usage = sendInfo.Usage;//用途
            payInfo.Remark = sendInfo.Remark;
            payInfo.NotificationURL = cfgInfo.NotificationURL;
            payInfo.BankID = sendInfo.BankID;
            payInfo.AccountType = int.Parse(sendInfo.AccType);
            //payInfo.Payees = sendInfo.TradeDetail.PayAccTraderBank.UserName;//操作人姓名(为空)

            TransPayRequest(payInfo);
            ResultInfo rInfo = new ResultInfo();

            if (!string.IsNullOrEmpty(payInfo.ActionURL))
                rInfo.Result = ResultType.Success;
            rInfo.PlanText = payInfo.PlainText;
            rInfo.MessagePaket = payInfo.Message;
            rInfo.Signature = payInfo.Signature;
            rInfo.TxCode = ((int)payInfo.txCode).ToString();
            rInfo.TxName = payInfo.txName;
            rInfo.ActionURLToBank = payInfo.ActionURL;

            return rInfo;
        }
        /// <summary>
        /// 转账请求
        /// </summary>
        /// <param name="payInfo"></param>
        /// <returns></returns>
        public Pay1311Info TransPayRequest(Pay1311Info payInfo)
        {
            //1.创建交易请求对象
            Tx1311Request tx1311Request = new Tx1311Request();
            tx1311Request.setInstitutionID(payInfo.InstitutionID);
            tx1311Request.setOrderNo(payInfo.OrderNo);
            tx1311Request.setPaymentNo(payInfo.PaymentNo);
            tx1311Request.setAmount(payInfo.Amount);
            tx1311Request.setFee(payInfo.Fee);
            tx1311Request.setPayerID(payInfo.PayerID);
            tx1311Request.setPayerName(payInfo.PayerName);
            tx1311Request.setUsage(payInfo.Usage);
            tx1311Request.setRemark(payInfo.Remark);
            tx1311Request.setNotificationURL(payInfo.NotificationURL);
            tx1311Request.setBankID(payInfo.BankID);
            tx1311Request.setAccountType(payInfo.AccountType);
            if (null != payInfo.Payees && payInfo.Payees.Length > 0)
            {
                String[] payeeList = payInfo.Payees.Split(';');
                for (int i = 0; i < payeeList.Length; i++)
                {
                    tx1311Request.addPayee(payeeList[i]);
                }
            }

            // 3.执行报文处理
            tx1311Request.process();

            payInfo.PlainText = tx1311Request.getRequestPlainText();
            payInfo.Message = tx1311Request.getRequestMessage();
            payInfo.Signature = tx1311Request.getRequestSignature();
            // //2个信息参数

            payInfo.txCode = BusinessType.Transfer;
            payInfo.txName = "市场订单支付直通车";
            // 1个action(支付平台地址)参数
            payInfo.ActionURL = PaymentEnvironment.PaymentURL;
            return payInfo;
        }

        #endregion
    }
}
