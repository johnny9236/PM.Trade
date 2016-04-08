using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.TRCB;

namespace PM.PaymentProtocolModel.BankCommModel.HSanTRCB
{
    /// <summary>
    /// 黄石入账或退款明细
    /// </summary>
    public class HSanTRCBQueryOrRtnQueryAccountDtl : TRCBQueryOrRtnQueryAccountDtl
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
