using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.TRCB;

namespace PM.PaymentProtocolModel.BankCommModel.HSanTRCB
{
    /// <summary>
    /// 黄山退款明细结果
    /// </summary>
    public class HSanTRCBQueryRtnResultModel : TRCBQueryRtnResultModel
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
        /// <param name="packetString"></param>
        /// <returns></returns>
        public override bool GetModel(string packetString)
        {
            return base.GetModel(packetString);
        }
         
    }
   
}
