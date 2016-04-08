using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.JHBOF
{
    /// <summary>
    /// 金华交易清单
    /// </summary>
    public class JHBOFQueryPayListModel : CommunicationBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string AccNo { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        public string Use { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }
        ///// <summary>
        ///// 返回结果
        ///// </summary>
        //public bool Result { get; set; }
        ///// <summary>
        ///// 信息
        ///// </summary>
        //public string Msg { get; set; }
        ///// <summary>
        ///// 错误信息
        ///// </summary>
        //public string ErrInfo { get; set; }
        ///// <summary>
        ///// 结果集
        ///// </summary>
        //public List<QueryResult> QueryResultList { get; set; }
    }
    /// <summary>
    /// 交易明细信息
    /// </summary>
    public class JHBofQueryResult
    {
        /// <summary>
        /// 交易日期
        /// </summary> 
        public string TradeDate { get; set; }
        /// <summary>
        /// 交易流水
        /// </summary> 
        public string TradeNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 金额    15位	 没有小数点"."，精确到分，最后两位为小数位，不足前补0。
        /// </summary> 
        public long Amount { get; set; }
        /// <summary>
        /// 对方账号
        /// </summary>
        public string AcountNo { get; set; }
        /// <summary>
        /// 对方户名
        /// </summary>
        public string AcountName { get; set; }

        /// <summary>
        /// 银行伪序列号
        /// </summary>
        public string CustomTradeNo { get; set; }

    }


}
