using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.PaymentProtocolModel;
using PM.Utils.Log;


namespace PM.LPSCCBPtlBiz
{
    public partial class BBCProtocols : IPaymentProtocol
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
                if (bt == BusinessType.PayB2C)//b2c支付
                {
                    return PayB2C(paymentModel, cfgInfo);
                }
                else if (bt == BusinessType.Pay)
                {
                    return PayB2B(paymentModel, cfgInfo);
                }
            }
            catch (Exception ex)
            {
                #region 异常处理
                LogTxt.WriteEntry(string.Format("{0}-{1}", ex.Message, cfgInfo.BusinessNo), "六盘水支付发起异常");
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
                if (bt == BusinessType.PayB2CResponse)//b2c支付响应
                {
                    return PayResponseB2C(paymentModel, cfgInfo);
                }
                else if (bt == BusinessType.PayResponse)//b2b
                {
                    return PayResponseB2B(paymentModel, cfgInfo);
                }
            }
            catch (Exception ex)
            {
                #region 异常处理
                LogTxt.WriteEntry(string.Format("{0}-{1}", ex.Message, cfgInfo.BusinessNo), "六盘水支付响应发起异常");
                #endregion
            }
            return null;
        }
    }
}
