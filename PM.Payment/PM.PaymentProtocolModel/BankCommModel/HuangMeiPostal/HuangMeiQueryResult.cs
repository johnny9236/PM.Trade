using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.HuangMeiPostal
{
    /// <summary>
    /// 黄梅查询返回信息
    /// </summary>
    public class HuangMeiQueryResult
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string  AccountNo { get; set; }
        /// <summary>
        /// 交易日期(yyyyMMdd)
        /// </summary>
        public string TradeDate { get; set; }
        /// <summary>
        /// 前台交易流水
        /// </summary>
        public string TradeSerialNumber { get; set; }
        /// <summary>
        /// 凭证种类
        /// </summary>
        public string ProofKind { get; set; }
        /// <summary>
        /// 凭证号码
        /// </summary>
        public string ProofNum { get; set; }
        /// <summary>
        /// 发生额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 借贷标志
        /// </summary>
        public string LoanMark { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal RemainAmount { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string Oprationer { get; set; }
        /// <summary>
        /// 复核员
        /// </summary>
        public string Checker { get; set; }
        /// <summary>
        /// 授权员
        /// </summary>
        public string Authorizer { get; set; }
        /// <summary>
        /// 对方账号
        /// </summary>
        public string CounterpartAccountNo { get; set; }
        /// <summary>
        /// 对方户名
        /// </summary>
        public string CounterpartAccountName { get; set; }
        /// <summary>
        /// 附言
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 交易渠道
        /// </summary>
        public string TradeWay { get; set; }
        /// <summary>
        /// 交易码
        /// </summary>
        public string TradeNo { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }
        /// <summary>
        /// 开户机构开户名称
        /// </summary>
        public string DepartmentAccName { get; set; }
    }
}
