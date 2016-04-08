using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel
{
    /// <summary>
    /// 查询对应操作类型
    /// </summary>
    public class QueryOpKindModel : CommunicationBase
    {
        /// <summary>
        /// 报文
        /// </summary>
        public string MessagePaket { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 通知信息
        /// </summary>
        public string NoticeMsg { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrInfo { get; set; }
    }
}
