using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PM.PaymentModel;
using PM.PaymentProtocolModel;
using PM.Utils;
using PM.PaymentProtocolModel.BankCommModel.JSABOC;
using PM.PlaymentPersistence.Payment.PersistenceBiz.JSABOC;
using PM.PlaymentPersistence.PaymentServiceFactory;


namespace PM.PlaymentPersistence
{
    /// <summary>
    /// 业务管理
    /// </summary>
    public class PlaymentPersistenceManager
    {
        /// <summary>
        /// 支付发起
        /// </summary>
        /// <param name="payModel">支付对象</param> 
        /// <returns></returns>
        public string DoPay(PayStartModel payModel)
        {
            return PayFactory.DoPay(payModel);
        }
        ///// <summary>
        ///// 退款(嘉善特殊)
        ///// </summary>
        ///// <param name="refundMode">退款对象</param> 
        ///// <returns></returns>
        //public string DoRefundPay(PayRefundModel refundMode)
        //{ 
        //    return PayFactory.DoRefundPay(refundMode);
        //}
        /// <summary>
        /// 退款(嘉善特殊)
        /// </summary>
        /// <param name="refundMode">退款对象</param> 
        /// <returns></returns>
        public dynamic DoRefundPay(PayRefundModel refundMode)
        {
            return PayFactory.DoRefundPay(refundMode);
        }
        /// <summary>
        /// 交易响应
        /// </summary>
        /// <param name="callBackModel">响应对象</param> 
        /// <returns></returns>
        public string PayCallBack(dynamic callBackModel)
        {
            return PayFactory.PayCallback(callBackModel);
        }
        /// <summary>
        /// 非支付调用  (通用对象，需要调用)
        /// </summary>
        /// <param name="objModel">通用对象</param>
        public string CommonRemoteCall(dynamic objModel)
        {
            return CommonFactory.CommonRemoteCall(objModel);
        }
    }
}
