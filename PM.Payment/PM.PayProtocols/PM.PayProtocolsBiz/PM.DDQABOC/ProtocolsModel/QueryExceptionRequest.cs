using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PM.DDQABOC.ProtocolsModel
{
    /// <summary>
    /// 异常时  发起查询
    /// </summary>
    public class QueryExceptionRequest : PubRequestPackets
    {
        /// <summary>
        /// 业务代码(默认CQRA10)
        /// </summary>
        public override string CCTransCode { get { return "CQRT04"; } }
        /// <summary>
        /// 借方账号
        /// </summary>
        public string DbAccNo { get; set; }
        /// <summary>
        /// 借方省市名称
        /// </summary>
        public string DbProv { get; set; }
        /// <summary>
        /// 借方货币号(默认是写的人民币)
        /// </summary>
        public string DbCur { get; set; }
        /// <summary>
        /// 客户端流水号
        /// </summary>
        public string CmeSeqNo { get; set; }
        public override XDocument SetRequsetPak()
        {
            XDocument myXDoc = base.SetRequsetPak();
            myXDoc.Element("ap").Add(
                          new XElement("Cmp",
                              new XElement("DbProv", Help.GetProv(this.DbProv)),
                              new XElement("DbAccNo", this.DbAccNo),
                              new XElement("DbCur", Help.GetCur(this.DbCur)),
                                new XElement("CmeSeqNo", this.CmeSeqNo)));
            return myXDoc;
        }
    }
}
