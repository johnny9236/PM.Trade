using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils;

namespace PM.PaymentProtocolModel.BankCommModel.HSICBC
{ 
    /// <summary>
    /// 黄山工行查询入账明细
    /// </summary>
    public class HSICBCQueryAccountDtl : ICBCQueryOrRtnQueryAccountDtl
    {
        /// <summary>
        /// 交易代码     入账明细为 3011   
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
