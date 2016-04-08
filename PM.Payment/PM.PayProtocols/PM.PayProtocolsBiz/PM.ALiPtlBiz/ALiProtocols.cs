using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.PaymentProtocolModel;
using PM.Utils.Log;

namespace PM.ALiPtlBiz
{
    /// <summary>
    /// 支付宝支付协议
    /// </summary>
    public partial class ALiProtocols : IPaymentProtocol
    {
        /// <summary>
        /// 支付请求
        /// </summary>
        /// <param name="paymentModel">支付对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        public dynamic CallRemotePay(dynamic paymentModel, PM.PaymentProtocolModel.CfgInfo cfgInfo)
        {
            try
            {
                BusinessType bt = BusinessType.None;
                Enum.TryParse(cfgInfo.BusinessKind, out bt);
                if (bt == BusinessType.Pay)//转账
                {
                    return PayRequest(paymentModel, cfgInfo);
                }
            }
            catch (Exception ex)
            { 
                #region 异常处理
                LogTxt.WriteEntry(string.Format("{0}-{1}-{2}", ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name), "支付宝支付发起异常");
                #endregion
            }
            return null;
        }
        /// <summary>
        /// 支付响应
        /// </summary>
        /// <param name="paymentModel">响应对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        public dynamic CallBackParse(dynamic paymentModel, PM.PaymentProtocolModel.CfgInfo cfgInfo)
        {
            try
            {
                BusinessType bt = BusinessType.None;
                Enum.TryParse(cfgInfo.BusinessKind, out bt);
                if (bt == BusinessType.PayResponse)//退还保证金响应
                {
                    return PayResponse(paymentModel, cfgInfo);
                }
            }
            catch (Exception ex)
            {
                #region 异常处理
                LogTxt.WriteEntry(string.Format("{0}-{1}-{2}", ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name), "支付宝支付发起异常");
                #endregion
            }
            return null;
        }
    }
}
