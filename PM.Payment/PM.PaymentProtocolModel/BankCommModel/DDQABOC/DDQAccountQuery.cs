using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.DDQABOC
{
    /// <summary>
    /// 账户查询
    /// </summary>
    public class DDQAccountQuery : CommunicationBase
    {
        /// <summary>
        /// 认证码
        /// </summary>
        public string AuthNo { get; set; } 
        /// <summary>
        /// 数字签名
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 起始日期 yyyyMMDD
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 终止日期yyyyMMDD
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 借方账号
        /// </summary>
        public string DbAccNo { get; set; }
        /// <summary>
        /// 借方省市代码
        /// </summary>
        public string DbProv { get; set; }
        /// <summary>
        /// 借方货币号
        /// </summary>
        public string DbCur { get { return "CNY"; } }
        /// <summary>
        /// 末笔时间戳  HHmmss
        /// </summary>
        public string StartTime { get; set; }
    }
}
