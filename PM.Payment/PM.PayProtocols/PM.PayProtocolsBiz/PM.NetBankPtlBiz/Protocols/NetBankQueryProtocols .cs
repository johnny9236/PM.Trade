using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFCA.Payment.Api;
using System.Collections;
using PM.NetBankPtlBiz.Model;
using PM.PaymentProtocolModel.BankCommModel.Netbank;
using PM.PaymentProtocolModel;
using PM.PaymentProtocolModel.BankCommModel;


namespace PM.NetBankPtlBiz.Protocols
{
    /// <summary>
    /// 查询相关
    /// </summary>
    public partial class NetBankCommonProtocols
    {
        #region 查询网银相关操作的类型
        /// <summary>
        /// 查询网银相关操作的类型(响应时候用)
        /// </summary>
        /// <param name="sendInfo"></param>
        /// <returns></returns>
        private QueryOpKindModel GetNetbankOpKind(QueryOpKindModel sendInfo, CfgInfo cfgInfo)
        {
             
            //报文及密匙
            Notice1118Or1318Or1348ResponseInfo info = NetBankOpKind(sendInfo.MessagePaket, sendInfo.Signature);
            string bankOpKind = string.Empty;
            //switch ((BusinessType)Enum.Parse(typeof(BusinessType), info.Message))
            //{
            //    case BusinessType.PayResponse:
            //        bankOpKind = BusinessKind.NetbankPayCallBack.ToString();
            //        break;
            //    case BusinessType.TransferResponse:
            //        bankOpKind = BusinessKind.NetbankTransCallBack.ToString();
            //        break;
            //    case BusinessType.TransferClearNotice:
            //        bankOpKind = BusinessKind.NetbankTransClearCallBack.ToString();
            //        break;
            //    default:
            //        bankOpKind = BusinessKind.NetbankOpkind.ToString();//默认查询
            //        break;
            //}
            sendInfo.Message = info.Message;//获取业务编号  
            sendInfo.NoticeMsg = info.MessageResponse;//返回编码
            return sendInfo;
        }
        /// <summary>
        /// 获取网银操作类型
        /// </summary>
        /// <param name="message"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public Notice1118Or1318Or1348ResponseInfo NetBankOpKind(string message, string signature)
        {
            Notice1118Or1318Or1348ResponseInfo responseInfo = new Notice1118Or1318Or1348ResponseInfo();

            string txName = string.Empty;
            NoticeRequest noticeRequest = new NoticeRequest(message, signature);
            //业务操作编号
            responseInfo.Message = noticeRequest.getTxCode();
            responseInfo.MessageResponse = Convert.ToBase64String(Encoding.UTF8.GetBytes(new NoticeResponse().getMessage()));
            responseInfo.TxCode = noticeRequest.getTxCode();
            responseInfo.PlainText = noticeRequest.getPlainText();
            return responseInfo;
        }
        #endregion

        #region 支付人信息获取
        /// <summary>
        /// 支付人信息获取(查询)
        /// </summary>
        /// <param name="sendInfo"></param>
        /// <returns></returns>
        private QueryPayerDetailModel GetNetbankSearchPayer(QueryPayerDetailModel sendInfo, CfgInfo cfgInfo)
        {
            //报文及密匙
            Notice1121ResponseInfo info = NetBankSearchPayer(sendInfo.InstitutionID, sendInfo.PaymentNo);
            if (info.Result)
            {
                if (null == sendInfo.TradeAccount)
                    sendInfo.TradeAccount = new TradeAccount();

                //sendInfo.InstitutionID = info.InstitutionID;//机构号
                sendInfo.Result = info.Result;
                //sendInfo.PaymentNo = info.PaymentNo;
                //支付方信息

                sendInfo.TradeAccount.AccCity = info.PayerCity;
                sendInfo.TradeAccount.AccPro = info.PayerProvince;
                sendInfo.TradeAccount.AccNo = info.PayerAccountNumber;
                sendInfo.TradeAccount.AccDbName = info.PayerAccountName;
                sendInfo.TradeAccount.AccDBBank = info.PayerBranchName;
            }
            else
            {
                sendInfo.ErrInfo = "获取支付人信息失败";
            }

            //通知给网银
            return sendInfo;
        }
        /// <summary>
        /// 网银支付查询获取支付人信息
        /// </summary>
        /// <param name="institutionID"></param>
        /// <param name="paymentNo"></param>
        /// <returns></returns>
        public Notice1121ResponseInfo NetBankSearchPayer(string institutionID, string paymentNo)
        {
            System.Net.ServicePointManager.Expect100Continue = false;//支持协议为1.1
            Notice1121ResponseInfo noticeInfo = new Notice1121ResponseInfo();

            Tx1121Request tx1121Request = new Tx1121Request();
            tx1121Request.setInstitutionID(institutionID);
            tx1121Request.setPaymentNo(paymentNo);

            // 3.执行报文处理
            tx1121Request.process();

            //2个信息参数
            noticeInfo.TxCode = "1121";
            noticeInfo.TxName = "付款账户信息查询";

            // 与支付平台进行通讯
            TxMessenger txMessenger = new TxMessenger();
            String[] respMsg = txMessenger.send(tx1121Request.getRequestMessage(), tx1121Request.getRequestSignature());// 0:message; 1:signature

            String plaintext = XmlUtil.formatXmlString(Encoding.UTF8.GetString(Convert.FromBase64String(respMsg[0])));
            noticeInfo.Message = respMsg[0];
            noticeInfo.Signature = respMsg[1];
            noticeInfo.PlainText = plaintext;

            Tx1121Response tx1121Response = new Tx1121Response(respMsg[0], respMsg[1]);
            if ("2000".Equals(tx1121Response.getCode()))
            {
                //处理业务
                noticeInfo.InstitutionID = tx1121Response.getInstitutionID();
                noticeInfo.PaymentNo = tx1121Response.getPaymentNo();

                noticeInfo.PayerAccountName = tx1121Response.getPayerAccountName();
                noticeInfo.PayerAccountNumber = tx1121Response.getPayerAccountNumber();
                noticeInfo.PayerBranchName = tx1121Response.getPayerBranchName();
                noticeInfo.PayerProvince = tx1121Response.getPayerProvince();
                noticeInfo.PayerCity = tx1121Response.getPayerCity();
                noticeInfo.Result = true;
            }
            return noticeInfo;
        }
        #endregion

        #region 商户订单支付查询
        /// <summary>
        /// 商户订单支付查询(1120)
        /// </summary>
        /// <param name="sendInfo"></param>
        /// <param name="businessType">动作类型</param>
        /// <returns></returns>
        private NetBankQueryMerchantOrPayModel GetNetbankMerchantOrPayQuery(NetBankQueryMerchantOrPayModel sendInfo, CfgInfo cfgInfo)
        {
            //报文及密匙
            Notice1120Or1320ResponseInfo info = NetBankMerchantOrPayQuery(sendInfo.InstitutionID, sendInfo.OrderNo, cfgInfo.BusinessKind);
            if (info.Result)
            {
                //sendInfo.Remark = info.InstitutionID;//机构号
                sendInfo.Result = info.Result;
                //sendInfo.TradeProduct.TradeNo = info.PaymentNo;
                //信息
                sendInfo.Amonut = info.Amonut;
                sendInfo.Remark = info.Remark;
                sendInfo.Status = info.Status;
                sendInfo.StatusDes = info.StatusDes;
            }
            else
            {
                sendInfo.ErrMsg = "商户订单支付查询失败";
            }
            //通知给网银
            return sendInfo;
        }

        /// <summary>
        /// 商户订单支付查询或市场支付查询(1120)
        /// </summary>
        /// <param name="institutionID"></param>
        /// <param name="paymentNo"></param>
        /// <returns></returns>
        public Notice1120Or1320ResponseInfo NetBankMerchantOrPayQuery(string institutionID, string paymentNo, string businessType)
        {
            Notice1120Or1320ResponseInfo noticeInfo = new Notice1120Or1320ResponseInfo();
            dynamic txRequest = null;

            // 创建交易请求对象
            if (businessType == ((int)BusinessType.MerchantQuery).ToString())
                txRequest = new Tx1120Request();
            else if (businessType == ((int)BusinessType.MarketPayQuery).ToString())
                txRequest = new Tx1320Request();
            else
                txRequest = null;
            if (txRequest == null)
            {
                noticeInfo.Remark = "创建请求对象失败";
                return noticeInfo;
            }

            txRequest.setInstitutionID(institutionID);
            txRequest.setPaymentNo(paymentNo);

            // 执行报文处理
            txRequest.process();


            noticeInfo.TxCode = "1120";
            noticeInfo.TxName = "商户订单支付交易查询";

            // 与支付平台进行通讯
            TxMessenger txMessenger = new TxMessenger();
            String[] respMsg = txMessenger.send(txRequest.getRequestMessage(), txRequest.getRequestSignature());// 0:message; 1:signature

            String plaintext = XmlUtil.formatXmlString(Encoding.UTF8.GetString(Convert.FromBase64String(respMsg[0])));
            //Console.WriteLine("[message] = [" + respMsg[0] + "]");
            //Console.WriteLine("[signature] = [" + respMsg[1] + "]");
            //Console.WriteLine("[plaintext] = [" + plaintext + "]");


            dynamic txResponse = null;
            if (businessType == ((int)BusinessType.MerchantQuery).ToString())
                txResponse = new Tx1120Response(respMsg[0], respMsg[1]);
            else if (businessType == ((int)BusinessType.MarketPayQuery).ToString())
                txResponse = new Tx1120Response(respMsg[0], respMsg[1]);
            else
                txResponse = null;
            if (txResponse == null)
            {
                noticeInfo.Remark = "创建返回对象失败";
                return noticeInfo;
            }

            noticeInfo.PlainText = txResponse.getResponsePlainText();
            if ("2000".Equals(txResponse.getCode()))
            {
                //处理业务
                noticeInfo.InstitutionID = txResponse.getInstitutionID();
                noticeInfo.PaymentNo = txResponse.getPaymentNo();
                noticeInfo.Amonut = txResponse.getAmount();
                noticeInfo.Remark = txResponse.getRemark();
                noticeInfo.Status = txResponse.getStatus();
                noticeInfo.BankNotificationTime = txResponse.getBankNotificationTime();
                noticeInfo.Result = true;
            }
            return noticeInfo;
        }
        #endregion

        #region 市场订单结算查询(1350)
        /// <summary>
        /// 市场订单结算查询(1350)
        /// </summary>
        /// <param name="sendInfo"></param>
        /// <returns></returns>
        private NetBankQueryMarketSettlementModel GetNetbankMarketSettlementQuery(NetBankQueryMarketSettlementModel sendInfo, CfgInfo cfgInfo)
        {
            //报文及密匙
            Notice1350ResponseInfo info = NetBankMarketSettlementQuery(sendInfo.InstitutionID, sendInfo.SerialNumber);

            if (info.Result)
            {
                if (null == sendInfo.TradeAccount)
                    sendInfo.TradeAccount = new TradeAccount();
                //sendInfo.MSG = info.InstitutionID;//机构号
                sendInfo.Result = info.Result;
                //sendInfo.TradeNo = info.PaymentNo;
                //信息
                sendInfo.Amonut = info.Amount;
                sendInfo.Remark = info.Remark;
                sendInfo.Status = info.Status;
                sendInfo.StatusDes = info.StatusDes;
                sendInfo.TradeAccount.AccDbName = info.AccountName;
                sendInfo.TradeAccount.AccNo = info.AccountNumber;
                sendInfo.TradeAccount.AccPro = info.Province;
                sendInfo.TradeAccount.AccCity = info.City;
                sendInfo.TradeAccount.BankID = info.BankID;
                sendInfo.TradeAccount.AccDbName = info.BranchName;
            }
            //通知给网银
            return sendInfo;
        }
        /// <summary>
        /// 市场订单结算1350
        /// </summary>
        /// <param name="institutionID"></param>
        /// <param name="serialNumber">原结算流水号</param>
        /// <returns></returns>
        public Notice1350ResponseInfo NetBankMarketSettlementQuery(string institutionID, string serialNumber)
        {
            Notice1350ResponseInfo responseInfo = new Notice1350ResponseInfo();
            // 2.创建交易请求对象
            Tx1350Request tx1350Request = new Tx1350Request();
            tx1350Request.setInstitutionID(institutionID);
            tx1350Request.setSerialNumber(serialNumber);

            // 3.执行报文处理
            tx1350Request.process();

            // 2个信息参数
            responseInfo.TxCode = "1350";
            responseInfo.TxName = "市场订单结算交易查询";

            // 与支付平台进行通讯
            TxMessenger txMessenger = new TxMessenger();
            String[] respMsg = txMessenger.send(tx1350Request.getRequestMessage(), tx1350Request.getRequestSignature());// 0:message; 1:signature
            String plaintext = XmlUtil.formatXmlString(Encoding.UTF8.GetString(Convert.FromBase64String(respMsg[0])));

            //Console.WriteLine("[message] = [" + respMsg[0] + "]");
            //Console.WriteLine("[signature] = [" + respMsg[1] + "]");
            //Console.WriteLine("[plaintext] = [" + plaintext + "]");

            Tx1350Response tx1350Response = new Tx1350Response(respMsg[0], respMsg[1]);
            responseInfo.PlainText = tx1350Response.getResponsePlainText();
            if ("2000".Equals(tx1350Response.getCode()))
            {
                //信息
                responseInfo.Message = tx1350Response.getMessage();
                responseInfo.InstitutionID = tx1350Response.getInstitutionID();
                responseInfo.SerialNumber = tx1350Response.getSerialNumber();
                responseInfo.PaymentNo = tx1350Response.getOrderNo();
                responseInfo.Amount = tx1350Response.getAmount();
                responseInfo.Remark = tx1350Response.getRemark();
                responseInfo.AccountType = tx1350Response.getAccountType();
                responseInfo.PayerAccountName = tx1350Response.getPaymentAccountName();
                responseInfo.PayerAccountNumber = tx1350Response.getPaymentAccountNumber();
                responseInfo.BankID = tx1350Response.getBankAccount().getBankID();
                responseInfo.AccountName = tx1350Response.getBankAccount().getAccountName();
                responseInfo.AccountNumber = tx1350Response.getBankAccount().getAccountNumber();
                responseInfo.BranchName = tx1350Response.getBankAccount().getBranchName();
                responseInfo.Province = tx1350Response.getBankAccount().getProvince();
                responseInfo.City = tx1350Response.getBankAccount().getCity();
                responseInfo.Status = tx1350Response.getStatus();
                responseInfo.Result = true;
            }
            return responseInfo;
        }
        #endregion

        #region 对账单 
        /// <summary>
        /// 对账单(未编写  目前用不到)
        /// </summary>
        /// <param name="sendInfo"></param>
        /// <returns></returns>
        private NetBankQueryStatementListModel GetNetbankBankStatementList(NetBankQueryStatementListModel sendInfo, CfgInfo cfgInfo)
        {
            //报文及密匙
            Notice1810ResponseInfo info = NetBankBankStatement(sendInfo.StructCode, sendInfo.QueryDate);
            if (info.Result)
            {
                if (null == sendInfo.QueryResult)
                    sendInfo.QueryResult = new List<Statement>();
                Statement qResult = null;
                //sendInfo.RtnResultInfo.MSG = info.InstitutionID;//机构号
                sendInfo.Result = info.Result;
                foreach (var rInfo in info.TX)
                {
                    qResult = new Statement();
                    qResult.TxAmount = rInfo.TxAmount;
                    qResult.TxType = rInfo.TxType;
                    qResult.TxSn = rInfo.TxSn;
                    qResult.InstitutionAmount = rInfo.InstitutionAmount;
                    qResult.PaymentAmoun = rInfo.PaymentAmoun;
                    qResult.PayerFee = rInfo.PayerFee;
                    qResult.InstitutionFee = rInfo.InstitutionFee;
                    qResult.Remark = rInfo.Remark;
                    qResult.BankNotificationTime = rInfo.BankNotificationTime;
                    sendInfo.QueryResult.Add(qResult);
                }
            }
            else
            {
                sendInfo.Msg = info.PlainText;
            }
            //通知给网银
            return sendInfo;
        }
        /// <summary>
        /// 对账
        /// </summary>
        /// <param name="institutionID"></param>
        /// <param name="searchData"></param>
        /// <returns></returns>
        public Notice1810ResponseInfo NetBankBankStatement(string institutionID, string searchData)
        {
            System.Net.ServicePointManager.Expect100Continue = false;//支持协议为1.1
            Notice1810ResponseInfo info = new Notice1810ResponseInfo();
            Tx1810Request tx1810Request = new Tx1810Request();
            tx1810Request.setInstitutionID(institutionID);
            tx1810Request.setDate(searchData);

            // 3.执行报文处理
            tx1810Request.process();

            // 2个信息参数
            info.TxCode = "1810";
            info.TxName = "下载交易对账单";
            // 与支付平台进行通讯
            TxMessenger txMessenger = new TxMessenger();
            String[] respMsg = txMessenger.send(tx1810Request.getRequestMessage(), tx1810Request.getRequestSignature());// 0:message; 1:signature
            String plaintext = XmlUtil.formatXmlString(Encoding.UTF8.GetString(Convert.FromBase64String(respMsg[0])));

            //Console.WriteLine("[message] = [" + respMsg[0] + "]");
            //Console.WriteLine("[signature] = [" + respMsg[1] + "]");
            //Console.WriteLine("[plaintext] = [" + plaintext + "]");

            Tx1810Response tx1810Response = new Tx1810Response(respMsg[0], respMsg[1]);
            info.PlainText = tx1810Response.getResponsePlainText();
            if ("2000".Equals(tx1810Response.getCode()))
            {
                info.TX = new List<Tx1810>();
                //处理业务
                ArrayList txList = tx1810Response.getTxList();
                int size = txList.Count;
                for (int i = 0; i < size; i++)
                {
                    Tx1810 infoDetal = new Tx1810();
                    Tx tx = (Tx)txList[i];

                    infoDetal.TxType = tx.getTxType();
                    infoDetal.TxSn = tx.getTxSn();
                    infoDetal.TxAmount = tx.getTxAmount();
                    infoDetal.InstitutionAmount = tx.getInstitutionAmount();
                    infoDetal.PaymentAmoun = tx.getPaymentAmount();
                    infoDetal.PayerFee = tx.getPayerFee();
                    infoDetal.InstitutionFee = tx.getInstitutionFee();
                    infoDetal.Remark = tx.getRemark();
                    infoDetal.BankNotificationTime = tx.getBankNotificationTime();
                    //infoDetal.SettlementFlag=tx.gets
                    info.TX.Add(infoDetal);
                }
                info.Result = true;
            }
            return info;

        }
        #endregion


    }
}
