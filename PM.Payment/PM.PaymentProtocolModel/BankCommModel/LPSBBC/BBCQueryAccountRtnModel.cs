using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.LPSBBC
{
    /// <summary>
    /// 账户入账明细
    /// </summary>
    public class BBCQueryAccountRtnModel
    {
        /// <summary>
        /// 商户代码
        /// </summary>
        public string MERCHANTID { get; set; }
        /// <summary>
        /// 商户所在分行
        /// </summary>
        public string BRANCHID { get; set; }
        /// <summary>
        /// 商户的POS号
        /// </summary>
        public string POSID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string ORDERID { get; set; }
        /// <summary>
        /// 支付/退款交易时间
        /// </summary>
        public string ORDERDATE { get; set; }
        /// <summary>
        /// 记账日期
        /// </summary>
        public string ACCDATE { get; set; }
        /// <summary>
        ///金额
        /// </summary>
        public string AMOUNT { get; set; }
        /// <summary>
        /// 支付/退款状态码
        /// </summary>
        public string STATUSCODE { get; set; }

        /// <summary>
        /// 支付/退款状态
        /// </summary>
        public string STATUS { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public string REFUND { get; set; }
        /// <summary>
        /// 串
        /// </summary>
        public string SIGN { get; set; }
    }
    /// <summary>
    /// 入账信息
    /// </summary>
    public class BBCQueryRtn
    {
        /// <summary>
        /// 交易返回码，成功时总为000000
        /// </summary>
        public string RETURN_CODE { get; set; }
        /// <summary>
        /// 交易返回提示信息，成功时为空
        /// </summary>
        public string RETURN_MSG { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int CURPAGE { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PAGECOUNT { get; set; }
        /// <summary>
        /// 明细记录
        /// </summary>
        public List<BBCQueryAccountRtnModel> BBCQueryAccountList { get; set; }

    }



}
