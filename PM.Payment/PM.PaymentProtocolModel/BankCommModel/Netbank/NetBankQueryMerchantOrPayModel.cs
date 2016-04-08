using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.Netbank
{
    /// <summary>
    /// 商户订单或者市场订单支付查询 状态查询
    /// </summary>
    public class NetBankQueryMerchantOrPayModel : CommunicationBase
    {
        /// <summary>
        /// 金额
        /// </summary>
        public long Amonut { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusDes { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
