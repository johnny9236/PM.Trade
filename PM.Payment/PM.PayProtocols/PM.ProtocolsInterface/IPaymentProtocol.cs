using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel;

namespace PM.ProtocolsInterface
{
    /// <summary>
    /// 支付类型接口
    /// </summary>
    public interface IPaymentProtocol
    {
        /// <summary>
        /// 支付调用
        /// </summary>
        /// <param name="paymentModel">请求对象</param>
        /// <param name="cfgInfo">对应配置对象</param>
        /// <returns>请求操作结果</returns>
        dynamic CallRemotePay(dynamic paymentModel, CfgInfo cfgInfo);
        /// <summary>
        /// 响应解析
        /// </summary>
        /// <param name="paymentModel">响应对象</param>
        /// <param name="cfgInfo">对应配置对象</param>
        /// <returns>响应操作结果</returns>
        dynamic CallBackParse(dynamic paymentModel, CfgInfo cfgInfo);
    }
}
