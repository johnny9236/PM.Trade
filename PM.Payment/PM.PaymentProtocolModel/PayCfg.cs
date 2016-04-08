using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PM.PaymentModel;

namespace PM.PaymentProtocolModel
{     
    /// <summary>
    /// 功能号
    /// </summary>
    public class FunctionCode
    {
        /// <summary>
        /// 协议类型
        /// </summary>
        public ProtocolsWay ProtocolsWay { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public OprationType OprationType { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessKind { get; set; }

    }
}
