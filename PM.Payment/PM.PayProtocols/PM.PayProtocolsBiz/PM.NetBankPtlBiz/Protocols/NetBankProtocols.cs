using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.Utils.Log;
using CFCA.Payment.Api;
using PM.NetBankPtlBiz.Model;
using PM.PaymentProtocolModel;


namespace PM.NetBankPtlBiz.Protocols
{
    /// <summary>
    ///银联 支付实现
    /// </summary>
    public partial class NetBankProtocols : IPaymentProtocol
    {
        /// <summary>
        /// 支付请求
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <param name="cfgInfo"></param>
        /// <returns></returns>
        public dynamic CallRemotePay(dynamic paymentModel, CfgInfo cfgInfo)
        {
            ResultInfo rInfo = new ResultInfo();
            try
            {
                BusinessType bt = BusinessType.None;
                Enum.TryParse(cfgInfo.BusinessKind, out bt);
                switch (bt)//业务类型
                {
                    case BusinessType.Pay://直通车 1111
                        rInfo = GetNetBankPayRequest(paymentModel, cfgInfo);
                        break;
                    case BusinessType.UndeterminedPay://(支付不确认)  1112
                        rInfo = GetNetBankUndeterminedPayRequest(paymentModel, cfgInfo);
                        break;

                    case BusinessType.Transfer://转账(市场订单)
                        rInfo = GetNetBankTransPayRequest(paymentModel, cfgInfo);
                        break;
                    case BusinessType.TransferNotice://结算
                        rInfo = GetNetBankSettlementRequest(paymentModel, cfgInfo);
                        break;
                    case BusinessType.BankBatchStayPays://代付无响应直接返回
                        rInfo = GetNetbankBankBatchStayPays(paymentModel, cfgInfo);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                #region 异常处理
                LogTxt.WriteEntry(string.Format("{0}-{1}-{2}", ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name), "支付发起异常");
                // rInfo.MSG = string.Format("{0}-{1}", ex.Message, ex.Source);
                #endregion
            }
            return rInfo;

        }
        /// <summary>
        /// 响应解析
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <param name="cfgInfo"></param>
        /// <returns></returns>
        public dynamic CallBackParse(dynamic paymentModel, CfgInfo cfgInfo)
        {
            ResultInfo rInfo = new ResultInfo();
            try
            {
                BusinessType bt = BusinessType.None;
                Enum.TryParse(cfgInfo.BusinessKind, out bt);
                switch (bt)
                {
                    case BusinessType.PayResponse://支付
                        rInfo = GetNetbankPayOrTransResponse(paymentModel, cfgInfo);
                        break;
                    case BusinessType.TransferResponse://转账
                        rInfo = GetNetbankPayOrTransResponse(paymentModel, cfgInfo);
                        break;
                    case BusinessType.TransferClearNotice://结算
                        rInfo = GetNetbankSettlementResponse(paymentModel, cfgInfo);
                        break;
                    //case RequestOrResponseType.BatchPays://代付无响应直接返回

                    //    break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                #region 异常处理
                LogTxt.WriteEntry(string.Format("{0}-{1}-{2}", ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name), "支付发起异常");
                //rInfo.MSG = string.Format("{0}-{1}", ex.Message, paymentModel.BusinessKind.ToString());
                #endregion
            }
            return rInfo;

        }
    }
}
