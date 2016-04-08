using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.Utils.Log;
using PM.PaymentProtocolModel;

namespace PM.JSABOC
{
    /// <summary>
    /// 嘉善农行支付相关协议
    /// </summary>
    public partial class JSABOCProtocols : IPaymentProtocol
    {
        /// <summary>
        /// 支付相关请求发起
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
                if (bt == BusinessType.Transfer)//转账
                {
                    return SendRefound(paymentModel, cfgInfo);
                }
            }
            catch (Exception ex)
            {
                #region 异常处理
                LogTxt.WriteEntry(string.Format("{0}-{1}", ex.Message, cfgInfo.BusinessKind), "农行支付发起异常");
                //  rInfo.MSG = string.Format("{0}-{1}", ex.Message, paymentModel.BusinessKind.ToString());
                #endregion
            }
            return null;
        }
        /// <summary>
        /// 支付响应
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <param name="cfgInfo"></param>
        /// <returns></returns>
        public dynamic CallBackParse(dynamic paymentModel, PM.PaymentProtocolModel.CfgInfo cfgInfo)
        {
            try
            {
                BusinessType bt = BusinessType.None;
                Enum.TryParse(cfgInfo.BusinessKind, out bt);
                if (bt == BusinessType.TransferResponse)//退还保证金响应
                {
                    return GetRefound(paymentModel, cfgInfo);
                }
            }
            catch (Exception ex)
            {
                #region 异常处理
                LogTxt.WriteEntry(string.Format("{0}-{1}", ex.Message, cfgInfo.BusinessKind), "农行支付发起异常");
                  #endregion
            }
            return null;

        }
    }
}
