using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.PubModel
{
    /// <summary>
    /// 支付、转账 结算 响应(通用)
    /// </summary>
    public class CommPayReqestModel:CommunicationBase
    {
        /// <summary>
        /// 签名信息
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 报文 供form提交
        /// </summary>
        public string MessagePaket { get; set; }
        /// <summary>
        /// 公匙
        /// </summary>
        public string PubKey { get; set; }
        /// <summary>
        /// 后台参数
        /// </summary>
        public string ParmStr { get; set; }
    }
}
