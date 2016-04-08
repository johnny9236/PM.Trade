using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentModel.BizModel.HSICBC
{
    /// <summary>
    /// 黄山通用协议基类
    /// </summary>
    public class HSICBCCommBase
    {
        /// <summary>
        /// 交易日期  YYYYMMDD
        /// </summary>
        public string TransDate { get; set; }
        /// <summary>
        /// 交易时间  HHMMSS
        /// </summary>
        public string TransTime { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string SeqNo { get; set; }
    }
}
