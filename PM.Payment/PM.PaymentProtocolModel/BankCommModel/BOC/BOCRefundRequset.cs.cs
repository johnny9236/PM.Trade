using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.BOC
{
    /// <summary>
    /// 退款转账请求
    /// </summary>
    public class BOCRefundRequset : BOCBase
    {
        /// <summary>
        /// 数字签名，该标签由前置机自动添加，企业无须上送
        /// </summary>
        public string CeitInfo { get; set; }

        /// <summary>
        /// 交易类型 
        /// 1、	委托待授权 
        /// 2、	授权退回修改该项可空，表示普通转账汇划 交易；
        /// 非空时只能为1或2
        /// </summary>
        public string TransType { get; set; }
        /// <summary>
        /// 交易明细
        /// </summary>
        public List<BOCRefundRQDtl> BOCRefundRQDtlLst { get; set; }

        /// <summary>
        /// 设置报文体
        /// </summary>
        /// <returns></returns>
        internal override string GetTranMessagePaket()
        {
            string stringLenth = string.Empty;//字符长度
            string rtnString = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("<trans>");
            sb.Append("<trn-b2e0009-rq>");
            //sb.Append("<ceitinfo>");
            //sb.Append(this.CeitInfo);
            //sb.Append("</ceitinfo>");
            sb.Append("<transtype>");
            sb.Append(this.TransType);
            sb.Append("</transtype>");
            #region  明细
            foreach (var dtl in BOCRefundRQDtlLst)
            {
                sb.Append("<b2e0009-rq>");

                sb.Append("<insid>");
                sb.Append(dtl.InsId);
                sb.Append("</insid>");
                sb.Append("<obssid>");
                sb.Append(dtl.ObssId);
                sb.Append("</obssid>");
                #region  付款方
                sb.Append("<fractn>");
                sb.Append("<fribkn>");
                sb.Append(dtl.PayFribkn);
                sb.Append("</fribkn>");
                sb.Append("<actacn>");
                sb.Append(dtl.PayActaCn);
                sb.Append("</actacn>");
                sb.Append("<actnam>");
                sb.Append(dtl.PayActNam);
                sb.Append("</actnam>");
                sb.Append("</fractn>");
                #endregion
                #region   收款方
                sb.Append("<toactn>");
                sb.Append("<toibkn>");
                sb.Append(dtl.ReceiveToibkn);
                sb.Append("</toibkn>");
                sb.Append("<actacn>");
                sb.Append(dtl.Receiveactacn);
                sb.Append("</actacn>");
                sb.Append("<toname>");
                sb.Append(dtl.ReceiveToName);
                sb.Append("</toname>");
                sb.Append("<toaddr>");
                sb.Append(dtl.ReceiveToibknoAddr);
                sb.Append("</toaddr>");
                sb.Append("<tobknm>");
                sb.Append(dtl.ReceiveTobknm);
                sb.Append("</tobknm>");                
                sb.Append("</toactn>");
                #endregion
                sb.Append("<trnamt>");
                sb.Append(dtl.TrnAmt);
                sb.Append("</trnamt>");

                sb.Append("<trncur>");
                sb.Append(dtl.TrnCur);
                sb.Append("</trncur>");
                sb.Append("<priolv>");
                sb.Append(dtl.Priolv);
                sb.Append("</priolv>");

                sb.Append("<furinfo>");
                sb.Append(dtl.FurInfo);
                sb.Append("</furinfo>");
                sb.Append("<trfdate>");
                sb.Append(dtl.TrfDate);
                sb.Append("</trfdate>");
                sb.Append("<comacn>");
                sb.Append(dtl.Comacn);
                sb.Append("</comacn>");

                sb.Append("</b2e0009-rq>");
            }

            #endregion
            sb.Append("</trn-b2e0009-rq>");
            sb.Append("</trans>");
            var sendInfo = string.Format(sb.ToString());
            this.Trncod = "b2e0009";
            return sendInfo;
        }

    }
    /// <summary>
    /// 转账退款明细
    /// </summary>
    public class BOCRefundRQDtl
    {
        /// <summary>
        /// 退款订单号 不超过12位
        /// </summary>
        public string InsId { get; set; }
        /// <summary>
        /// 网银交易流水号  
        /// 交易类型为2时上送有效
        /// </summary>
        public string ObssId { get; set; }
        /// <summary>
        /// 联行号   有效
        /// </summary>
        public string PayFribkn { get; set; }
        /// <summary>
        /// 付款账号
        /// </summary>
        public string PayActaCn { get; set; }
        /// <summary>
        /// 付款人名称
        /// </summary>
        public string PayActNam { get; set; }
        /// <summary>
        /// 收款行联行号 可空
        /// </summary>
        public string ReceiveToibkn { get; set; }
        /// <summary>
        /// 收款账号
        /// </summary>
        public string Receiveactacn { get; set; }
        /// <summary>
        /// 收款人名称
        /// </summary>
        public string ReceiveToName { get; set; }
        /// <summary>
        /// 收款人地址
        /// </summary>
        public string ReceiveToibknoAddr { get; set; }
        /// <summary>
        /// 收款人开户行名称  该项可以为空
        /// </summary>
        public string ReceiveTobknm { get; set; }
        /// <summary>
        /// 转账金额
        /// </summary>
        public string TrnAmt { get; set; }
        /// <summary>
        /// 转账货币
        /// </summary>
        public string TrnCur { get; set; }
        /// <summary>
        /// 报文发送优先级(0-普通；1-加急)
        /// </summary>
        public string Priolv { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        public string FurInfo { get; set; }
        /// <summary>
        /// 要求的转账日期 YYYYMMDD
        /// </summary>
        public string TrfDate { get; set; }
        /// <summary>
        /// 手续费账号 如果为空则使用付款账户代替
        /// </summary>
        public string Comacn { get; set; }

    }
}
