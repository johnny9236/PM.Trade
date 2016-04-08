using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.PaymentProtocolModel;
using PM.Utils.Log;

namespace PM.BOCPtlBiz
{
    /// <summary>
    /// 中国银行支付相关
    /// </summary>
    public partial class BOCPayProtocols : IPaymentProtocol
    {
        public dynamic CallRemotePay(dynamic paymentModel, PaymentProtocolModel.CfgInfo cfgInfo)
        {
            try
            {
                BusinessType bt = BusinessType.None;
                Enum.TryParse(cfgInfo.BusinessKind, out bt);
                if (bt == BusinessType.Transfer)//保证金退还
                {
                    return SendRefound(paymentModel, cfgInfo);
                }
            }
            catch (Exception ex)
            {
                #region 异常处理
                LogTxt.WriteEntry(string.Format("{0}-{1}-{2}", ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name), "中国银行退款（汇款）异常");
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
