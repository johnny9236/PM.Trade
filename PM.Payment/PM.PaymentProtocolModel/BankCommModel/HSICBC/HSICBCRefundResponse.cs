using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PM.PaymentProtocolModel.BankCommModel.HSICBC
{
    public class HSICBCRefundResponse : ICBCRefundResponse
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
    public class HSICBCReturnRefundDtl : ICBCReturnRefundDtl
    {
    }
}
