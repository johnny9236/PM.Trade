using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.PaymentProtocolModel;
using PM.Utils.Log;

namespace PM.HuangSanPtlBiz
{
    /// <summary>
    /// 支付相关协议类
    /// </summary>
    public partial class HSICBCProtocols : IPaymentProtocol
    {
        /// <summary>
        /// 退保证金
        /// </summary>
        /// <param name="paymentModel">保证金对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        public dynamic CallRemotePay(dynamic paymentModel, PaymentProtocolModel.CfgInfo cfgInfo)
        {
            try
            {
                BusinessType bt = BusinessType.None;
                Enum.TryParse(cfgInfo.BusinessKind, out bt);
                //if (bt == BusinessType.Transfer)//人工退还
                //{
                //    return SendManualRefound(paymentModel, cfgInfo);
                //}
                //else
                    if (bt == BusinessType.TransferNotice)//保证金退还
                {
                    return SendRefound(paymentModel, cfgInfo);
                }
            }
            catch (Exception ex)
            {
                #region 异常处理
                LogTxt.WriteEntry(string.Format("{0}-{1}", ex.Message, cfgInfo.BusinessKind), "黄山工行支付发起异常");
                //  rInfo.MSG = string.Format("{0}-{1}", ex.Message, paymentModel.BusinessKind.ToString());
                #endregion
            }
            return null;
        }

        public dynamic CallBackParse(dynamic paymentModel, PaymentProtocolModel.CfgInfo cfgInfo)
        {
            throw new NotImplementedException();
        }
    }
}
