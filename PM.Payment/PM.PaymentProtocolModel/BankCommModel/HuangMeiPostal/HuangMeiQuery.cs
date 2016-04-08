using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.HuangMeiPostal
{
    /// <summary>
    /// 黄梅入账明细查询对象
    /// </summary>
    public class HuangMeiQuery : CommunicationBase
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public string MercCode { get; set; }
        /// <summary>
        /// 开始时间(yyyyMMdd)
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        /// 结束时间(yyyyMMdd)
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string AcctNo { get; set; }
     
    }

    
}
