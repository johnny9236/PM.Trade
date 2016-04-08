using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFCA.Payment.Api;
using System.Collections;
using PM.NetBankPtlBiz.Model;
using PM.PaymentProtocolModel;
using PM.PaymentProtocolModel.BankCommModel.Netbank;
 


namespace PM.NetBankPtlBiz.Protocols
{
    /// <summary>
    /// 结算、代付相关
    /// </summary>
    public partial class NetBankProtocols
    {
        #region 结算请求
        /// <summary>
        /// 网银结算请求
        /// </summary>
        /// <param name="sendInfo"></param>
        /// <returns></returns>
        private ResultInfo GetNetBankSettlementRequest(NetBankSettlementRequestModel sendInfo, CfgInfo cfgInfo)
        {
            Pay1341Info pay1341 = new Pay1341Info();
            pay1341.InstitutionID =sendInfo.InstitutionID;//机构号
            pay1341.SerialNumber = sendInfo.SerialNumber;
            pay1341.OrderNo = sendInfo.OrderNo;//订单号
            pay1341.AccountType = Convert.ToInt32(sendInfo.AccType);
            pay1341.Amount = (long)(sendInfo.Amount * 100);
            pay1341.PaymentAccountName = sendInfo.PayAccName;
            pay1341.PaymentAccountNumber = sendInfo.PayAccNo;
            //收款方信息
            pay1341.BankID = sendInfo.RecBankID;
            pay1341.AccountName = sendInfo.RecAccName;
            pay1341.AccountNumber = sendInfo.RecAccNo;
            pay1341.BranchName = sendInfo.RecAccDBBank;
            pay1341.Province = sendInfo.RecAccPro;
            pay1341.City = sendInfo.RecAccCity;
            pay1341.Remark = sendInfo.Remarks;
            pay1341 = TransPay1341ResultNotice(pay1341);
            ResultInfo rInfo = new ResultInfo();
            rInfo.Result = pay1341.Result == true ? ResultType.Success : ResultType.Faile;
            rInfo.StatusDes = pay1341.Remark; 
            return rInfo;
        }
        /// <summary>
        /// 结算
        /// </summary>
        /// <param name="payInfo"></param>
        /// <returns></returns>
        public Pay1341Info TransPay1341ResultNotice(Pay1341Info payInfo)
        {
            Tx1341Request tx1341Request = new Tx1341Request();
            tx1341Request.setInstitutionID(payInfo.InstitutionID);
            tx1341Request.setSerialNumber(payInfo.SerialNumber);
            tx1341Request.setOrderNo(payInfo.OrderNo);
            tx1341Request.setAmount(payInfo.Amount);
            tx1341Request.setRemark(payInfo.Remark);
            tx1341Request.setAccountType(payInfo.AccountType);
            tx1341Request.setPaymentAccountName(payInfo.PaymentAccountName);
            tx1341Request.setPaymentAccountNumber(payInfo.PaymentAccountNumber);

            BankAccount bankAccount = new BankAccount();
            bankAccount.setBankID(payInfo.BankID);
            bankAccount.setAccountName(payInfo.AccountName);
            bankAccount.setAccountNumber(payInfo.AccountNumber);
            bankAccount.setBranchName(payInfo.BranchName);
            bankAccount.setProvince(payInfo.Province);
            bankAccount.setCity(payInfo.City);
            tx1341Request.setBankAccount(bankAccount);

            // 3.执行报文处理
            tx1341Request.process();

            //2个信息参数
            payInfo.txCode = BusinessType.TransferNotice;
            payInfo.txName = "市场订单结算（结算）";

            // 与支付平台进行通讯
            TxMessenger txMessenger = new TxMessenger();
            String[] respMsg = txMessenger.send(tx1341Request.getRequestMessage(), tx1341Request.getRequestSignature());// 0:message; 1:signature
            String plaintext = XmlUtil.formatXmlString(Encoding.UTF8.GetString(Convert.FromBase64String(respMsg[0])));

            //Console.WriteLine("[message] = [" + respMsg[0] + "]");
            //Console.WriteLine("[signature] = [" + respMsg[1] + "]");
            //Console.WriteLine("[plaintext] = [" + plaintext + "]");

            payInfo.Message = respMsg[0];
            payInfo.Signature = respMsg[1];
            //payInfo.PlainText = plaintext;
            Tx134xResponse tx134xResponse = new Tx134xResponse(respMsg[0], respMsg[1]);
            payInfo.PlainText = tx134xResponse.getResponsePlainText();
            if ("2000".Equals(tx134xResponse.getCode()))
            {
                //处理业务
                payInfo.Result = true;
            }
            else
            {
                payInfo.Remark = ((RtnStatus)(Enum.Parse(typeof(RtnStatus), tx134xResponse.getCode()))).ToString();
            }
            return payInfo;
        }
        #endregion

        #region 结算响应
        /// <summary>
        /// 网银结算响应通知
        /// </summary>
        /// <param name="sendInfo"></param>
        /// <returns></returns>
        private ResultInfo GetNetbankSettlementResponse(NetBankPayResponseModel sendInfo, CfgInfo cfgInfo)
        {
            return GetNetbankPayOrTransResponse(sendInfo, cfgInfo);
        }
        #endregion

        #region 代付
        /// <summary>
        /// 批量代付处理
        /// </summary>
        /// <param name="sendInfo"></param>
        /// <returns></returns>
        private ResultInfo GetNetbankBankBatchStayPays(NetbankBankBatchStayPaysRequestModel sendInfo, CfgInfo cfgInfo)
        {
            BatchStayPays info = new BatchStayPays();
            ResultInfo rInfo = new ResultInfo();
            if (null == sendInfo.InstitutionID)
            {
                rInfo.MSG = "无机构编码";
                return rInfo;
            }
            info.InstitutionID = sendInfo.InstitutionID;//付款方为中心
            info.BatchNo = sendInfo.OrderNo;//订单号
            info.TotalAmount = Convert.ToInt64(sendInfo.Amount * 100);
            if (null == sendInfo.AccTradeList || sendInfo.AccTradeList.Count == 0)
            {
                rInfo.MSG = "无批量代付明细信息";
                return rInfo;
            }
            info.TotalCount = sendInfo.AccTradeList.Count;
            info.Remark = sendInfo.Remarks;
            info.BatchList = new List<BatchInfo>();
            BatchInfo batch = null;
            foreach (var per in sendInfo.AccTradeList)
            {
                batch = new BatchInfo();
                batch.ItemNo = per.ItemNo;
                batch.Amount = Convert.ToInt64(per.Amount);
                batch.BankID = per.BankID;
                batch.AccType = per.AccType;
                batch.AccDbName = per.AccDbName;
                batch.AccNo = per.AccNo;
                batch.AccDBBank = per.AccDBBank;
                batch.AccPro = per.AccPro;
                batch.AccCity = per.AccCity;
                batch.Remark = per.Remarks;
                batch.PhoneNumber = per.PhoneNumber;
                batch.IdentificationNumber = per.IdentificationNumber;
                batch.IdentificationType = per.IdentificationType;
                batch.Email = per.Email;
                info.BatchList.Add(batch);
            }
            BatchStayPaysRequest(info);
            rInfo.MSG = info.Message;
            if (info.Result)
            {
                rInfo.Result = info.Result == true ? ResultType.Success : ResultType.Faile;
            }
            return rInfo;
        }
        /// <summary>
        /// 代付处理
        /// </summary>
        /// <param name="batchInfo"></param>
        /// <returns></returns>
        public BatchStayPays BatchStayPaysRequest(BatchStayPays batchInfo)
        {
            ArrayList itemList = null;
            Item item = null;
            Tx1510Request tx1510Request = new Tx1510Request();
            tx1510Request.setInstitutionID(batchInfo.InstitutionID);
            tx1510Request.setBatchNo(batchInfo.BatchNo);
            tx1510Request.setTotalAmount(batchInfo.TotalAmount);
            tx1510Request.setTotalCount(batchInfo.TotalCount);
            tx1510Request.setRemark(batchInfo.Remark);
            if (null != batchInfo.BatchList && batchInfo.BatchList.Count > 0)
            {
                itemList = new ArrayList();
                foreach (var batch in batchInfo.BatchList)
                {
                    item = new Item();
                    item.setItemNo(batch.ItemNo);
                    item.setAmount(batch.Amount);
                    item.setBankID(batch.BankID);
                    item.setAccountName(batch.AccDbName);
                    item.setAccountNumber(batch.AccNo);
                    item.setAccountType(Convert.ToInt32(batch.AccType));
                    item.setBranchName(batch.AccDBBank);
                    item.setProvince(batch.AccPro);
                    item.setCity(batch.AccCity);
                    item.setNote(batch.Remark);
                    item.setPhoneNumber(batch.PhoneNumber);
                    item.setEmail(batch.Email);
                    item.setIdentificationNumber(batch.IdentificationNumber);
                    item.setIdentificationType(batch.IdentificationType);
                    itemList.Add(item);
                }
                tx1510Request.setItemList(itemList);
            }
            // 执行报文处理
            tx1510Request.process();

            // 2个信息参数
            batchInfo.TxCode = "1510";
            batchInfo.TxName = "批量代付";
            // 与支付平台进行通讯
            TxMessenger txMessenger = new TxMessenger();
            String[] respMsg = txMessenger.send(tx1510Request.getRequestMessage(), tx1510Request.getRequestSignature());// 0:message; 1:signature
            // String plaintext = XmlUtil.formatXmlString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(respMsg[0])));
            batchInfo.Message = respMsg[0];
            batchInfo.Signature = respMsg[1];

            Tx1510Response tx1510Response = new Tx1510Response(respMsg[0], respMsg[1]);
            batchInfo.PlainText = tx1510Response.getResponsePlainText();
            if ("2000".Equals(tx1510Response.getCode()))
            {
                batchInfo.Result = true;
            }

            return batchInfo;
        }
        #endregion
    }
}
