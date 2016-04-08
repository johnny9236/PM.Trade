using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.Netbank
{
    /// <summary>
    /// 1810对账单（查询）
    /// </summary>
    public class NetBankQueryStatementListModel : CommunicationBase
    {
        /// <summary>
        /// 机构号
        /// </summary>
        public string StructCode { get; set; }
        /// <summary>
        /// 查询时间
        /// </summary>
        public string QueryDate { get; set; }
        /// <summary>
        /// 明细
        /// </summary>
        public List<Statement> QueryResult { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }
    }
    /// <summary>
    /// 查询明细
    /// </summary>
    public class Statement
    {
        /// <summary>
        /// 交易类型
        /// </summary>
        public string TxType { get; set; }
        /// <summary>
        /// 交易编号
        /// </summary>     
        public string TxSn { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public long TxAmount { get; set; }
        /// <summary>
        /// 机构应收的金额
        /// </summary>
        public long InstitutionAmount { get; set; }
        /// <summary>
        /// 支付平台应收的金额
        /// </summary>
        public long PaymentAmoun { get; set; }
        /// <summary>
        /// 付款人手续费
        /// </summary>
        public long PayerFee { get; set; }
        /// <summary>
        /// 机构手续
        /// </summary>
        public long InstitutionFee { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 支付平台收到银行通知时间，格式：YYYYMMDDhhmmss
        /// </summary>
        public string BankNotificationTime { get; set; }
        /// <summary>
        /// 结算标示
        /// </summary>
        public string SettlementFlag { get; set; }
    }
}
