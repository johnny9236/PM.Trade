using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PM.PaymentModel
{
    /// <summary>
    /// 退款请求对象
    /// </summary>
    public class PayRefundModel
    {
        /// <summary>
        /// 业务号（用于导向到具体的协议及功能）
        /// </summary>
        [DataMember]
        public string BusinessFunNo { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        [DataMember]
        public string OrderNo { get; set; }
        /// <summary>
        /// 主账号
        /// </summary>
        [DataMember]
        public string MainAccount { get; set; }
        /// <summary>
        /// 交易信息用途(可用来做标段或项目)
        /// </summary>
        [DataMember]
        public string TradeInfo { get; set; }
        /// <summary>
        /// 总金额 
        /// </summary>
        [DataMember]
        public decimal PayMoney { get; set; }
        /// <summary>
        /// 总手续费
        /// </summary>
        [DataMember]
        public decimal PayFee { get; set; }
        /// <summary>
        /// 交易日期  YYYYMMDD
        /// </summary>
        [DataMember]
        public string TransDate { get; set; }
        /// <summary>
        /// 交易时间  HHMMSS
        /// </summary>
        [DataMember]
        public string TransTime { get; set; }
        /// <summary>
        /// 授权码
        /// </summary>
        [DataMember]
        public string AuthCode { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Descript { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Result { get; set; }
        /// <summary>
        ///退款 明细
        /// </summary>
        [DataMember]
        public List<PayStartModel> PayRefundDtl { get; set; }
    }
}
