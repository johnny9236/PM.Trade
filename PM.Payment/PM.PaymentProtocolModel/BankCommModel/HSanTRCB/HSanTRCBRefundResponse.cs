using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.TRCB;

namespace PM.PaymentProtocolModel.BankCommModel.HSanTRCB
{
    /// <summary>
    /// 保证金退回响应
    /// </summary>
    public class HSanTRCBRefundResponse : TRCBRefundResponse
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
        /// 获取返回退款对象
        /// </summary>
        /// <param name="packetString"></param>
        /// <returns></returns>
        public override bool GetModel(string packetString)
        {
            return base.GetModel(packetString);
        }

    }
    /// <summary>
    /// 保证金退回返回
    /// </summary>
    public class HSanTRCBReturnRefundDtl : TRCBReturnRefundDtl
    {
    }
}
