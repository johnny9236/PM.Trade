using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PM.DDQABOC.ProtocolsModel
{
    /// <summary>
    /// 公共响应包
    /// </summary>
    public class PubResponsePackets
    {
        /// <summary>
        /// 内部交易代码
        /// </summary>
        public virtual string CCTransCode { get; set; }
        /// <summary>
        /// 请求方流水号
        /// </summary>
        public string ReqSeqNo { get; set; }
        /// <summary>
        /// 返回来源
        /// </summary>
        public string RespSource { get; set; }
        /// <summary>
        /// 应答流水号
        /// </summary>
        public string RespSeqNo { get; set; }
        /// <summary>
        /// 返回日期
        /// </summary>
        public string RespDate { get; set; }
        /// <summary>
        /// 返回时间
        /// </summary>
        public string RespTime { get; set; }
        /// <summary>
        /// 返回码
        /// </summary>
        public string RespCode { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string RespInfo { get; set; }
        /// <summary>
        /// 返回扩展信息
        /// </summary>
        public string RxtInfo { get; set; }
        /// <summary>
        /// 数据文件标识
        /// </summary>
        public string FileFlag { get; set; }
        /// <summary>
        /// 记录数
        /// </summary>	 
        public string RecordNum { get; set; }
        /// <summary>
        /// 字段数
        /// </summary>
        public string FieldNum { get; set; }
        /// <summary>
        /// 私有数据区
        /// </summary>
        public string RespPrvData { get; set; }
        /// <summary>
        /// 批量文件名
        /// </summary>
        public string BatchFileName { get; set; }
        /// <summary>
        /// 相应公共报文获取对应值
        /// </summary>
        /// <param name="xdoc"></param>
        public virtual void GetPubResponseInfo(XDocument xdoc)
        {
            var pub = from c in xdoc.Descendants("ap")
                      select new
                        {
                            CCTransCode = c.Element("CCTransCode") == null ? string.Empty : c.Element("CCTransCode").Value,
                            ReqSeqNo = c.Element("ReqSeqNo") == null ? string.Empty : c.Element("ReqSeqNo").Value,
                            RespSource = c.Element("RespSource") == null ? string.Empty : c.Element("RespSource").Value,
                            RespSeqNo = c.Element("RespSeqNo") == null ? string.Empty : c.Element("RespSeqNo").Value,
                            RespDate = c.Element("RespDate") == null ? string.Empty : c.Element("RespDate").Value,

                            RespTime = c.Element("RespTime") == null ? string.Empty : c.Element("RespTime").Value,
                            RespCode = c.Element("RespCode") == null ? string.Empty : c.Element("RespCode").Value,
                            RespInfo = c.Element("RespInfo") == null ? string.Empty : c.Element("RespInfo").Value,
                            RxtInfo = c.Element("RxtInfo") == null ? string.Empty : c.Element("RxtInfo").Value,
                            FileFlag = c.Element("FileFlag") == null ? string.Empty : c.Element("FileFlag").Value
                        };
            if (pub != null && pub.Count() > 0)
            {
                this.CCTransCode = pub.FirstOrDefault().CCTransCode;
                this.ReqSeqNo = pub.FirstOrDefault().ReqSeqNo;
                this.RespSource = pub.FirstOrDefault().RespSource;
                this.RespSeqNo = pub.FirstOrDefault().RespSeqNo;
                this.RespDate = pub.FirstOrDefault().RespDate;

                this.RespTime = pub.FirstOrDefault().RespTime;
                this.RespCode = pub.FirstOrDefault().RespCode;
                this.RespInfo = pub.FirstOrDefault().RespInfo;
                this.RxtInfo = pub.FirstOrDefault().RxtInfo;
                this.FileFlag = pub.FirstOrDefault().FileFlag;
            }
            #region  cme  + cmp
            var cme = from c in xdoc.Descendants("Cme")
                      select new
                        {
                            RecordNum = c.Element("RecordNum") == null ? string.Empty : c.Element("RecordNum").Value,
                            FieldNum = c.Element("FieldNum") == null ? string.Empty : c.Element("FieldNum").Value
                        };
            if (cme != null && cme.Count() > 0)
            {
                this.RecordNum = cme.FirstOrDefault().RecordNum;
                this.FieldNum = cme.FirstOrDefault().FieldNum;
            }
            var cmp = from c in xdoc.Descendants("Cmp")
                      select new
                        {
                            RespPrvData = c.Element("RespPrvData") == null ? string.Empty : c.Element("RespPrvData").Value,
                            BatchFileName = c.Element("BatchFileName") == null ? string.Empty : c.Element("BatchFileName").Value
                        };
            if (cmp != null && cmp.Count() > 0)
            {
                this.RespPrvData = cmp.FirstOrDefault().RespPrvData;
                this.BatchFileName = cmp.FirstOrDefault().BatchFileName;
            }
            #endregion
        }
        /// <summary>
        /// 判断是否是返回有错误信息
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public virtual bool RtnSucess(PubRequestPackets reqpak, out  string errMsg)
        {
            errMsg = string.Empty;
            bool rtnRes = false;

            if (this.RespCode == "0000")
                rtnRes = true;
            else
                errMsg = string.Format("{0}{1}", this.RespInfo, this.RxtInfo);
            #region 
            //判断来源
            //if (this.RespSource == "1")
            //{
            #region
            //判断错误号
            //if (this.RespCode == "999")
            //{
            //    errMsg = string.Format("{0}{1}", this.RespInfo, this.RxtInfo);
            //}
            //else
            //{

            //    //判断流水号是否一致
            //    //if (reqpak.ReqSeqNo == this.ReqSeqNo)
            //    //    rtnRes = true;
            //    //else
            //    //    errMsg = "流水号不匹配";
            //    rtnRes = true;
            //}
            #endregion
            //}
            //else
            //{
            //    errMsg = "非ICT自身返回";
            //}
            #endregion
            return rtnRes;
        }
    }
}
