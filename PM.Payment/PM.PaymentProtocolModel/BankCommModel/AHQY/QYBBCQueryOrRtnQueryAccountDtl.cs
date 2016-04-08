using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils;

namespace PM.PaymentProtocolModel.BankCommModel.AHQY
{ 
    /// <summary>
    /// 青阳查询入账明细或退款明细
    /// </summary>
    public class QYBBCQueryOrRtnQueryAccountDtl : BBCQueryOrRtnQueryAccountDtl
    {
        /// <summary>
        /// 交易代码
        /// </summary>
        public override string TransCode
        {
            get
            {
                return base.TransCode;
            }
            set
            {
                base.TransCode = value;
            }
        }
        /// <summary>
        /// 获取报文
        /// </summary>
        /// <returns></returns>
        public override string GetMessagePaket()
        {
            return base.GetMessagePaket();
        }
    }
}
