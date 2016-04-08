using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentContracts;
using PM.PaymentModel;
using PM.PlaymentPersistence;


namespace PM.PaymentServices
{
    /// <summary>
    /// 支付相关服务实现
    /// </summary>
    public class PaymentService : IPaymentService
    {
        PlaymentPersistenceManager manager = new PlaymentPersistenceManager();
        /// <summary>
        /// 支付发起接口
        /// </summary>
        /// <param name="payReceiveModel">支付请求对象</param>
        /// <returns></returns>
        public string DoPay(PayStartModel payReceiveModel)
        {
            return manager.DoPay(payReceiveModel);
        }
        ///// <summary>
        ///// 退款(嘉善)
        ///// </summary>
        ///// <param name="payRefundModel">退款对象</param>
        ///// <returns></returns>
        //public string DoRefundPay(PayRefundModel payRefundModel)
        //{
        //    return manager.DoRefundPay(payRefundModel);
        //}
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="payRefundModel">退款对象</param>
        /// <returns></returns>
        public PayRefundModel DoRefundPay(PayRefundModel payRefundModel)
        {
            return manager.DoRefundPay(payRefundModel);
        }
        /// <summary>
        /// 回调响应
        /// </summary>
        /// <param name="resModel">回调对象</param>
        /// <returns></returns>
        public string PayCallback(PayResopnseModel resModel)
        {
            return manager.PayCallBack(resModel);
        } 
    }
}
