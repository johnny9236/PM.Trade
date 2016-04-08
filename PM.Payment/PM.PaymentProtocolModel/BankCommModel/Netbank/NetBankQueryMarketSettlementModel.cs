using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.Netbank
{
    /// <summary>
    /// 市场订单结算查询(1350)[理解为市场支付的扩展]
    /// </summary>
    public class NetBankQueryMarketSettlementModel : NetBankQueryMerchantOrPayModel
    {
        /// <summary>
        /// 收款账户信息
        /// </summary>
        public TradeAccount TradeAccount { get; set; }
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
