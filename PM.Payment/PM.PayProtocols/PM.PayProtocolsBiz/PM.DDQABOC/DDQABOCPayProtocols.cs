using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;

namespace PM.DDQABOC
{
    /// <summary>
    /// 支付相关协议
    /// </summary>
    public partial class DDQABOCPayProtocols : IPaymentProtocol
    {
        public dynamic CallRemotePay(dynamic paymentModel, PaymentProtocolModel.CfgInfo cfgInfo)
        {
            throw new NotImplementedException();
        }

        public dynamic CallBackParse(dynamic paymentModel, PaymentProtocolModel.CfgInfo cfgInfo)
        {
            throw new NotImplementedException();
        }
    }
}
