using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel
{
    /// <summary>
    /// 响应对象公用
    /// </summary>
    public class PubCallBackModel : CommunicationBase
    {
        /// <summary>
        /// 签名信息
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string MessagePaket { get; set; }
    }
}
