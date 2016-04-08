using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PM.DDQABOC.ProtocolsModel
{
    /// <summary>
    /// 发送公共报文头
    /// </summary>
    public class PubRequestPackets
    { 
        /// <summary>
        /// 内部交易代码
        /// </summary>
        public virtual string CCTransCode { get; set; }
        public string ProductID { get { return "ICC"; } }
        public string ChannelType { get { return "ERP"; } }
        /// <summary>
        /// 企业技监局号码/客户号
        /// </summary>
        public string CorpNo { get; set; }
        /// <summary>
        /// 企业操作员编号
        /// </summary>
        public string OpNo { get; set; }
        /// <summary>
        /// 认证码
        /// </summary>
        public string AuthNo { get; set; }
        /// <summary>
        /// 请求方流水号
        /// </summary>
        public string ReqSeqNo { get; set; }
        /// <summary>
        /// 请求日期
        /// </summary>
        public string ReqDate { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public string ReqTime { get; set; }
        /// <summary>
        /// 数字签名
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 设置请求报文头信息
        /// </summary>
        /// <returns></returns>
        public virtual XDocument SetRequsetPak()
        {
            XDocument myXDoc = new XDocument(
             new XElement("ap",
                    new XElement("CCTransCode", this.CCTransCode),
                    new XElement("ProductID", this.ProductID),
                      new XElement("ChannelType", this.ChannelType),
                        new XElement("CorpNo", this.CorpNo),
                          new XElement("OpNo", this.OpNo),
                            new XElement("AuthNo", this.AuthNo),
                              new XElement("ReqSeqNo", this.ReqSeqNo),
                                new XElement("ReqDate", this.ReqDate),
                                new XElement("ReqTime", this.ReqTime),
                                   new XElement("Sign", this.Sign)));
            return myXDoc;
        }
    }
}
