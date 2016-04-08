using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PM.PaymentProtocolModel.BankCommModel.DDQABOC
{
    
    /// <summary>
    /// 返回查询记录对象
    /// </summary>
    public class DDQAccountDtl
    {
        /// <summary>
        /// 省市代码
        /// </summary>
        public string Prov { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string AccNo { get; set; }
        /// <summary>
        /// 货币码
        /// </summary>
        public string Cur { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        public string TrDate { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public string TimeStab { get; set; }
        /// <summary>
        /// 日志号
        /// </summary>
        public string TrJrn { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public string TrType { get; set; }
        /// <summary>
        /// 交易行号
        /// </summary>
        public string TrBankNo { get; set; }
        /// <summary>
        /// 户名
        /// </summary>
        public string AccName { get; set; }
        /// <summary>
        /// 发生额标志
        /// </summary>
        public string AmtIndex { get; set; }
        /// <summary>
        /// 对方账号省市代码
        /// </summary>
        public string OppProv { get; set; }
        /// <summary>
        /// 对方账号
        /// </summary>
        public string OppAccNo { get; set; }
        /// <summary>
        /// 对方账号货币码
        /// </summary>
        public string OppCur { get; set; }
        /// <summary>
        /// 对方账号户名
        /// </summary>
        public string OppName { get; set; }
        /// <summary>
        /// 对方账号开户行
        /// </summary>
        public string OppBkName { get; set; }
        /// <summary>
        /// 现转标志
        /// </summary>
        public string CshIndex { get; set; }
        /// <summary>
        /// 错账日期
        /// </summary>
        public string ErrDate { get; set; }
        /// <summary>
        /// 错账传票号
        /// </summary>
        public string ErrVchNo { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string Amt { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public string Bal { get; set; }
        /// <summary>
        /// 上笔余额
        /// </summary>
        public string PreAmt { get; set; }
        /// <summary>
        /// 手续费总额
        /// </summary>
        public string TotChg { get; set; }
        /// <summary>
        /// 凭证种类
        /// </summary>
        public string VoucherType { get; set; }
        /// <summary>
        /// 凭证省市代号
        /// </summary>
        public string VoucherProv { get; set; }
        /// <summary>
        /// 凭证批次号
        /// </summary>
        public string VoucherBat { get; set; }
        /// <summary>
        /// 凭证号
        /// </summary>
        public string VoucherNo { get; set; }
        /// <summary>
        /// 客户参考号
        /// </summary>
        public string CustRef { get; set; }
        /// <summary>
        /// 交易码
        /// </summary>
        public string TransCode { get; set; }
        /// <summary>
        /// 柜员号
        /// </summary>
        public string Teller { get; set; }
        /// <summary>
        /// 传票号
        /// </summary>
        public string VchNo { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Abs { get; set; }
        /// <summary>
        /// 附言
        /// </summary>
        public string PostScript { get; set; }
        /// <summary>
        /// 交易来源
        /// </summary>
        public string TrFrom { get; set; }
    }
}
