using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PM.DDQABOC.ProtocolsModel
{
    /// <summary>
    /// 转账返回对象
    /// </summary>
    public class PayTransferRtn : PubResponsePackets
    {
        /// <summary>
        /// 借方账号
        /// </summary>
        public string DbAccNo { get; set; }
        /// <summary>
        /// 借方省市代码
        /// </summary>
        public string DbProv { get; set; }
        /// <summary>
        /// 借方货币码
        /// </summary>
        public string DbCur { get; set; }
        /// <summary>
        /// 贷方账号
        /// </summary>
        public string CrAccNo { get; set; }
        /// <summary>
        /// 贷方省市代码
        /// </summary>
        public string CrProv { get; set; }
        /// <summary>
        /// 贷方货币号
        /// </summary>
        public string CrCur { get; set; }
        /// <summary>
        /// 落地处理标志
        /// </summary>
        public string WaitFlag { get; set; }
        /// <summary>
        /// 借方户名
        /// </summary>
        public string DbAccName { get; set; }
        /// <summary>
        ///  获取返回报文
        /// </summary>
        /// <param name="xdoc"></param>
        public override void GetPubResponseInfo(XDocument xdoc)
        {
            base.GetPubResponseInfo(xdoc);
            var cmp = from c in xdoc.Descendants("Cmp")
                      select new
                        {
                            DbAccNo = c.Element("DbAccNo") == null ? string.Empty : c.Element("DbAccNo").Value,
                            DbProv = c.Element("DbProv") == null ? string.Empty : c.Element("DbProv").Value,
                            DbCur = c.Element("DbCur") == null ? string.Empty : c.Element("DbCur").Value,
                            CrAccNo = c.Element("CrAccNo") == null ? string.Empty : c.Element("CrAccNo").Value,
                            CrProv = c.Element("CrProv") == null ? string.Empty : c.Element("CrProv").Value,
                            CrCur = c.Element("CrCur") == null ? string.Empty : c.Element("CrCur").Value
                        };
            if (cmp != null && cmp.Count() > 0)
            {
                this.DbAccNo = cmp.FirstOrDefault().DbAccNo;
                this.DbProv = cmp.FirstOrDefault().DbProv;
                this.DbCur = cmp.FirstOrDefault().DbCur;
                this.CrAccNo = cmp.FirstOrDefault().CrAccNo;
                this.CrProv = cmp.FirstOrDefault().CrProv;
                this.CrCur = cmp.FirstOrDefault().CrCur;
            }
            var corp = from c in xdoc.Descendants("Corp")
                       select new
                         {
                             WaitFlag = c.Element("WaitFlag") == null ? string.Empty : c.Element("WaitFlag").Value,
                             DbAccName = c.Element("DbAccName") == null ? string.Empty : c.Element("DbAccName").Value,
                         };
            if (corp != null && corp.Count() > 0)
            {
                this.WaitFlag = corp.FirstOrDefault().WaitFlag;
                this.DbAccName = corp.FirstOrDefault().DbAccName;
            }
        }
    }
}
