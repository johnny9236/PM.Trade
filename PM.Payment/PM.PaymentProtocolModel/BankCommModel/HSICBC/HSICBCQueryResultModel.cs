using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PM.Utils;
using PM.Utils.Log;

namespace PM.PaymentProtocolModel.BankCommModel.HSICBC
{
    /// <summary>
    /// 入账明细查询返回
    /// </summary>
    public class HSICBCQueyResultModel : ICBCQueyResultModel
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
    public class HSICBCQueryInfo : ICBCQueryInfo
    { 
    }
}
