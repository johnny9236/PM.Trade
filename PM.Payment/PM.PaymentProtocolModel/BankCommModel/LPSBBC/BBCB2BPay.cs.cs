using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.LPSBBC
{
    /// <summary>
    /// b2b
    /// </summary>
    public class BBCB2BPay : BBCBase
    {
        /// <summary>
        /// 项目号
        /// </summary>
        public string PROJECTNO { get; set; }
        /// <summary>
        /// 指定支付账号
        /// </summary>
        public string PAYACCNO { get; set; }
        /// <summary>
        /// 验证基本户
        /// </summary>
        public string ACCTYPE { get; set; }
        /// <summary>
        /// 订单截止时间(yyyyMMddHHMISS)
        /// </summary>
        public string ENDTIME { get; set; }
       
    }
}
