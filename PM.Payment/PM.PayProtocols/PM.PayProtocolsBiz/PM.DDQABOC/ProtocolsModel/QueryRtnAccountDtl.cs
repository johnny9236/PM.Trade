using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PM.DDQABOC.ProtocolsModel
{
    /// <summary>
    /// 查询账户明细结果
    /// </summary>
    [Serializable]
    public class QueryAccountResult : PubResponsePackets
    {
        /// <summary>
        /// 业务代码(默认CQRA10)
        /// </summary>
        public override string CCTransCode
        {
            get { return "CQRA10"; } //set;
        }
        /// <summary>
        /// 借方账号
        /// </summary>
        public string DbAccNo { get; set; }
        /// <summary>
        /// 借方省市代码
        /// </summary>
        public string DbProv { get; set; }
        /// <summary>
        /// 借方货币号
        /// </summary>
        public string DbCur { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string BatchFileName { get; set; }
        /// <summary>
        ///  获取返回报文
        /// </summary>
        /// <param name="xdoc"></param>
        public void GetPubResponseInfo(XDocument xdoc)
        {
            base.GetPubResponseInfo(xdoc);
            var cmp = from c in xdoc.Descendants("Cmp")
                      select new
                        {
                            DbAccNo = c.Element("DbAccNo") == null ? string.Empty : c.Element("DbAccNo").Value,
                            DbProv = c.Element("DbProv") == null ? string.Empty : c.Element("DbProv").Value,
                            DbCur = c.Element("DbCur") == null ? string.Empty : c.Element("DbCur").Value,
                            BatchFileName = c.Element("BatchFileName") == null ? string.Empty : c.Element("BatchFileName").Value,
                        };
            if (cmp != null && cmp.Count() > 0)
            {
                this.DbAccNo = cmp.FirstOrDefault().DbAccNo;
                this.DbProv = cmp.FirstOrDefault().DbProv;
                this.DbCur = cmp.FirstOrDefault().DbCur;
                this.BatchFileName = cmp.FirstOrDefault().BatchFileName;
            }
        }
    } 
}
