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
    /// 黄山工行退款明细查询
    /// </summary>
    public class HSICBCQueryRtnResultModel : ICBCQueryRtnResultModel
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


        #region  退款
        /// <summary>
        /// 查询总笔数
        /// </summary>
        public string QueryTotalNum { get; set; }
        /// <summary>
        /// 当前页起始笔数
        /// </summary>
        public string CurStartNum { get; set; }
        /// <summary>
        /// 当前页查询笔数
        /// </summary>
        public string CurQueryNum { get; set; } 
        #endregion


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
