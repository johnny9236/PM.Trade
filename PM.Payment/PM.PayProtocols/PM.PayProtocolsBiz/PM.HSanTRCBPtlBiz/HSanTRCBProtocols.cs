using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.PaymentProtocolModel;
using PM.Utils.Log;

namespace PM.HSanTRCBPtlBiz
{
    public partial class HSanTRCBProtocols : IPaymentProtocol
    {
        /// <summary>
        /// 保证金
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <param name="cfgInfo"></param>
        /// <returns></returns>
        public dynamic CallRemotePay(dynamic paymentModel, PaymentProtocolModel.CfgInfo cfgInfo)
        {
            try
            {
                BusinessType bt = BusinessType.None;
                Enum.TryParse(cfgInfo.BusinessKind, out bt);
                if (bt == BusinessType.Transfer)//人工退还
                {
                    return SendManualRefound(paymentModel, cfgInfo);
                }
                else if (bt == BusinessType.TransferNotice)//保证金退还
                {
                    return SendRefound(paymentModel, cfgInfo);
                }
            }
            catch (Exception ex)
            {
                #region 异常处理
                LogTxt.WriteEntry(string.Format("{0}-{1}", ex.Message, cfgInfo.BusinessKind), "农商行支付发起异常");
                //  rInfo.MSG = string.Format("{0}-{1}", ex.Message, paymentModel.BusinessKind.ToString());
                #endregion
            }
            return null;
        }
        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <param name="cfgInfo"></param>
        /// <returns></returns>
        public dynamic CallBackParse(dynamic paymentModel, PaymentProtocolModel.CfgInfo cfgInfo)
        {
            throw new NotImplementedException();
        }
    }
}
