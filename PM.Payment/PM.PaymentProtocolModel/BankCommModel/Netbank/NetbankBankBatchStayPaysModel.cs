using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.Netbank
{
    /// <summary>
    /// 代付请求
    /// </summary>
    public class NetbankBankBatchStayPaysRequestModel : CommunicationBase
    { 
        /// <summary>
        /// 设定交易金额(必要信息)  总金额
        /// </summary>
        public double Amount { get; set; }
        ///// <summary>
        ///// 手续费  
        ///// </summary>
        //public double Free { get; set; }
        ///// <summary>
        ///// 交易明细时间
        ///// </summary>
        //public DateTime TradeTm { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 多个收款信息（代付结算等支付到多个账号使用） 一个情况说明是支付
        /// </summary>
        public List<TradeAccountDetail> AccTradeList { get; set; }

    }
}
