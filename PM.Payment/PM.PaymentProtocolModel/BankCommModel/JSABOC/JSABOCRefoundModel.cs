using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.JSABOC
{
    /// <summary>
    /// 退款对象
    /// </summary>
    public class JSABOCRefoundModel : CommunicationBase
    {
        /// <summary>
        /// 标段编号
        /// </summary>
        public string SectionNo { get; set; }
        ///收款 账号  
        /// </summary>
        public string ReceiveAccNo { get; set; }
        /// <summary>
        ///收款  账户名     
        /// </summary>
        public string ReceiveAccDbName { get; set; }
        /// <summary>
        ///收款  账户开户行 
        /// </summary>
        public string ReceiveAccDBBank { get; set; }
        /// <summary>
        /// 缴款金额
        /// </summary>
        public decimal  Amount { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal  RealAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string ABOCRemark { get; set; }
    }
}
