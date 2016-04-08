using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.TRCB;

namespace PM.PaymentProtocolModel.BankCommModel.HSanTRCB
{
    /// <summary>
    /// 黄山入账明细结果
    /// </summary>
    public class HSanTRCBQueyResultModel : TRCBQueyResultModel
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
    /// <summary>
    /// 明细记录
    /// </summary>
    public class HSanQueryInfo : TRCBQueryInfo
    {

    }
}
