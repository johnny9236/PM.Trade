using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.HSICBC
{
    /// <summary>
    /// 退款操作
    /// </summary>
    public class HSICBCRefundRequset : ICBCRefundRequset
    {
        /// <summary>
        /// 交易代码
        /// </summary>
        public override string TransCode
        {
            get
            {
                return "6003"; 
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

    /// <summary>
    /// 退款明细
    /// </summary>
    public class HSICBCRefundInfo : ICBCRefundInfo
    {
         
    }
}
