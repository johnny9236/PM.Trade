using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.Netbank
{
    /// <summary>
    /// 结算请求
    /// </summary>
    public class NetBankSettlementRequestModel : CommunicationBase
    {
        /// <summary>
        /// 金额
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// 付款账户名
        /// </summary>
        public string PayAccName { get; set; }
        /// <summary>
        /// 付款账号
        /// </summary>
        public string PayAccNo { get; set; }
        /// <summary>
        /// 收款账号      （必要信息）
        /// </summary>
        public string RecAccNo { get; set; }
        /// <summary>
        ///收款 账户名     
        /// </summary>
        public string RecAccName { get; set; }
        /// <summary>
        ///收款 账户开户行（必要信息）
        /// </summary>
        public string RecAccDBBank { get; set; }
        /// <summary>
        /// 收款账户开户行行号（必要信息）
        /// </summary>
        public string RecAccDBBankNo { get; set; }
        /// <summary>
        /// 收款银行ID（代码）
        /// </summary>
        public string RecBankID { get; set; }
        /// <summary>
        /// 收款省
        /// </summary>
        public string RecAccPro { get; set; }
        /// <summary>
        ///收款 市
        /// </summary>
        public string RecAccCity { get; set; }
        ///// <summary>
        ///// 币种
        ///// </summary>
        //public string AccCur { get; set; }
        /// <summary>
        /// 账户类型 个人11、企业12
        /// </summary>
        public string AccType { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remarks { get; set; }
    }
     
}
