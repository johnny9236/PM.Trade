using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel
{
    /// <summary>
    /// 查询支付人信息(Notice1121QueryInfo)
    /// </summary>
    public class QueryPayerDetailModel : CommunicationBase
    {         
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 报文
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 成功时间
        /// </summary> 
        public string SuccessTime { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string TxCode { get; set; }
        /// <summary>
        /// 业务描述
        /// </summary>
        public string TxName { get; set; }
        /// <summary>
        /// 响应报文
        /// </summary>
        public string PlainText { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 订单号（流水号）
        /// </summary>
        public string PaymentNo { get; set; }
        /// <summary>
        /// 账户信息
        /// </summary>
        public TradeAccount TradeAccount { get; set; }
      
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrInfo { get; set; }
    }
}
