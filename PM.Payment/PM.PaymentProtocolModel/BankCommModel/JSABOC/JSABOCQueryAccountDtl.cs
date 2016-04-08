using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.JSABOC
{
    /// <summary>
    /// 查询入账明细
    /// </summary>
    public class JSABOCQueryAccountDtl : CommunicationBase
    {
        /// <summary>
        /// 明细日期
        /// </summary>
        public string DetailDataTime { get; set; }
        /// <summary>
        /// 机构号
        /// </summary>
        public string TradeStructNum { get; set; }
    }
}
