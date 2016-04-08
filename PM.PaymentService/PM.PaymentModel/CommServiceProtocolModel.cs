using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PM.PaymentModel
{
    /// <summary>
    /// 通用协议对象  (非支付都可以使用)
    /// </summary>
    [DataContract]
    [KnownType(typeof(CommServiceProtocolModel))]
    public class CommServiceProtocolModel
    {
        /// <summary>
        /// 业务功能号(用于导向到具体的协议及功能）
        /// </summary>
        [DataMember]
        public string BusinessFunNo { get; set; }
        /// <summary>
        ///通讯内容（在外面一层定义对象  ）
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }
    }
}
