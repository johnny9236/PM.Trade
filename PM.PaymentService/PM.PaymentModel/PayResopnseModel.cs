using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
 
namespace PM.PaymentModel
{
    /// <summary>
    /// 支付响应接口对象
    /// </summary>
    public class PayResopnseModel
    {
        /// <summary>
        /// 业务号（用于导向到具体的协议及功能）
        /// </summary>
        [DataMember]
        public string BusinessFunNo { get; set; }
        /// <summary>
        /// 报文
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [DataMember]
        public string Signature { get; set; }
        /// <summary>
        /// 报文原文
        /// </summary>
        [DataMember]
        public string ReceiveText { get; set; }
        /// <summary>
        /// 是否后台显示
        /// </summary>
           [DataMember]
        public bool IsShowBk { get; set; }  
    }
}
