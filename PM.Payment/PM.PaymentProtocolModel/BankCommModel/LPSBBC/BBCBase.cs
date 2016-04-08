using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.LPSBBC
{
    /// <summary>
    /// bbc 基类
    /// </summary>
    public class BBCBase : CommunicationBase
    {
        /// <summary>
        /// 商户代码
        /// </summary>
        public string MERCHANTID { get; set; }
        /// <summary>
        /// 商户柜台代码
        /// </summary>
        public string POSID { get; set; }
        /// <summary>
        /// 分行代码
        /// </summary>
        public string BRANCHID { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public string PAYMENT { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string CURCODE { get; set; }
        /// <summary>
        /// 交易码
        /// </summary>
        public string TXCODE { get; set; }
        /// <summary>
        /// MAC校验域
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// 接口类型
        /// </summary>
        public string TYPE { get; set; }
        /// <summary>
        /// 公钥后30位
        /// </summary>
        public string PUB { get; set; }
        ///// <summary>
        ///// MAC校验域
        ///// </summary>
        //public string MAC { get; set; }
        /// <summary>
        /// 客户注册信息
        /// </summary>
        public string REGINFO { get; set; }
        /// <summary>
        /// 商品信息
        /// </summary>
        public string PROINFO { get; set; }
        /// <summary>
        /// 商户URL 为空
        /// </summary>
        public string REFERER { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 备注2
        /// </summary>
        public string Remark2 { get; set; }
    }
}
