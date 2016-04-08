using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PM.DDQABOC.ProtocolsModel
{
    /// <summary>
    /// 异常时  查询返回
    /// </summary>
    public class QueryExceptionResult : PubResponsePackets
    {
        /// <summary>
        /// 币种
        /// </summary>
        public string DbCur { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string DbAccNo { get; set; }
        /// <summary>
        /// 省市代码
        /// </summary>
        public string DbProv { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string CmeSeqNo { get; set; }
        /// <summary>
        /// 流水详细信息("%4s%2hd%12s%4s%4s""1970",1, szRespSeqNo,szRespCode,szAbisRespCode)
        /// </summary>
        public string RespPrvData { get; set; }
        /// <summary>
        /// 流水的状态信息
        /// </summary>
        public string Postscript { get; set; }

        /// <summary>
        /// 返回报文
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
                          CmeSeqNo = c.Element("CmeSeqNo") == null ? string.Empty : c.Element("CmeSeqNo").Value,
                          RespPrvData = c.Element("RespPrvData") == null ? string.Empty : c.Element("RespPrvData").Value
                      };
            if (cmp != null && cmp.Count() > 0)
            {
                this.DbAccNo = cmp.FirstOrDefault().DbAccNo;
                this.DbProv = cmp.FirstOrDefault().DbProv;
                this.DbCur = cmp.FirstOrDefault().DbCur;
                this.CmeSeqNo = cmp.FirstOrDefault().CmeSeqNo;
                this.RespPrvData = cmp.FirstOrDefault().RespPrvData;
            }
            var corp = from c in xdoc.Descendants("Corp")
                       select new
                       {
                           Postscript = c.Element("Postscript") == null ? string.Empty : c.Element("Postscript").Value
                       };
            if (corp != null && corp.Count() > 0)
            {
                this.Postscript = corp.FirstOrDefault().Postscript;
            }
        }
    }
}
