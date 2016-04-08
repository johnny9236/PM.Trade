using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.BOC
{
    /// <summary>
    /// 中银基类（header部分）
    /// </summary>
    public abstract class BOCBase : CommunicationBase
    {
        /// <summary>
        ///代表一台企业前置机
        ///E开头+前置机IP地址（各段补零，无小数点12位）
        /// </summary>
        public string Termid { get; set; }
        /// <summary>
        /// 客户端产生的报文编号	字母数字串 0-12位
        /// </summary>
        public string Trnid { get; set; }
        /// <summary>
        /// 企业在中行网银系统的客户编码	数码1-10位
        /// </summary>
        public string CustId { get; set; }
        /// <summary>
        /// 企业操作员代码	字母数字标点串 1-20位
        /// </summary>
        public string CusOpr { get; set; }
        /// <summary>
        /// 交易代码	b2e开头加4位数字
        /// </summary>
        public string Trncod { get; set; }
        /// <summary>
        /// 交易验证标识，签到时生成、签退时注销	Base64字符串0-64位	检查令牌是否正确
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 报文头
        /// </summary>
        /// <returns></returns>
        protected string GetHeadMessagePaket()
        {
            string stringLenth = string.Empty;//字符长度
            string rtnString = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("<head>");
            sb.Append("<termid>{0}</termid>");
            sb.Append("<trnid>{1}</trnid>");
            sb.Append("<custid>{2}</custid>");
            sb.Append("<cusopr>{3}</cusopr>");
            sb.Append("<trncod>{4}</trncod>");
            sb.Append("<token>{5}</token>");
            sb.Append("</head>");
            var sendInfo = string.Format(sb.ToString()
            , this.Termid
            , this.Trnid
            , this.CustId
            , this.CusOpr
            , this.Trncod
            , this.Token
            );
            return sendInfo;
        }
        /// <summary>
        /// 报文体
        /// </summary>
        internal  abstract string GetTranMessagePaket();
        /// <summary>
        /// 发送报文
        /// </summary>
        /// <returns></returns>
        public string GetMessagePaket()
        {
            string stringLenth = string.Empty;//字符长度
            string rtnString = string.Empty;
            var tranMessage=GetTranMessagePaket();
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<bocb2e version=\"120\"  security=\" true\"  locale=\"zh_CN\" >");//请求
            sb.Append(GetHeadMessagePaket());
            sb.Append(tranMessage);
            sb.Append("</bocb2e>");
            return sb.ToString();
        }
    }
}
