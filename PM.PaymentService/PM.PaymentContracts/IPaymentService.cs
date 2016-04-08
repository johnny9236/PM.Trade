using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentModel;
using System.ServiceModel;

namespace PM.PaymentContracts
{
    /// <summary>
    /// 提供给业务系统使用的签约接口
    /// </summary>
    [ServiceContract]
    public interface IPaymentService
    {
        /// <summary>
        /// 支付发起接口
        /// </summary>
        /// <param name="payReceiveModel">支付请求对象</param>
        /// <returns></returns>
        [OperationContract] 
        string DoPay(PayStartModel payReceiveModel);
        ///// <summary>
        ///// 退款发起接口
        ///// </summary>
        ///// <param name="payRefundModel"></param>
        ///// <returns></returns>
        //[OperationContract]
        //string DoRefundPay(PayRefundModel payRefundModel);
        /// <summary>
        /// 退款发起接口
        /// </summary>
        /// <param name="payRefundModel"></param>
        /// <returns></returns>
        [OperationContract]
        PayRefundModel DoRefundPay(PayRefundModel payRefundModel);
        ///// <summary>
        ///// 支付回发响应（银行或者第三方）
        ///// </summary>
        ///// <param name="message">消息</param>
        ///// <param name="signature">签名</param>
        ///// <returns></returns>
        //[OperationContract]
        //string PayCallback(string message, string signature);
        /// <summary>
        /// 支付回发响应（银行或者第三方）
        /// </summary>
        /// <param name="resModel"></param>
        /// <returns></returns>
        [OperationContract]
        string PayCallback(PayResopnseModel resModel);

    }
}
