using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Log;
using System.Xml.Linq;

namespace PM.PaymentProtocolModel.BankCommModel.BOC
{
    /// <summary>
    /// 退款转账响应
    /// </summary>
    public class BOCRefundResponse
    {
        /// <summary>
        /// 交易返回码  B001,成功； 
        /// </summary>
        public string RspCod { get; set; }
        /// <summary>
        ///  rspmsg表示解释信息
        /// </summary>
        public string RspMsg { get; set; }
        /// <summary>
        /// 退款返回情况明细列表
        /// </summary>
        public List<BOCRefundResponseDtl> RefundResponseDtlLst { get; set; }

        public bool GetModel(string packetString)
        {
            bool rst = false;
            try
            {
                var xdoc = XDocument.Parse(packetString);

                #region 返回状态
                var result = from c in xdoc.Descendants("status")
                             where c.Parent.Name == "trn-b2e0009-rs"
                             select new
                               {
                                   rspcod = c.Element("rspcod") == null ? string.Empty : c.Element("rspcod").Value,
                                   rspmsg = c.Element("rspmsg") == null ? string.Empty : c.Element("rspmsg").Value
                               };
                #endregion
                //返回结果
                if (result != null && result.Count() > 0)
                {
                    this.RspCod = result.FirstOrDefault().rspcod;
                    this.RspMsg = result.FirstOrDefault().rspmsg;
                    if (this.RspCod.ToLower() == "b001")
                        rst = true;
                }
                if (rst)//操作成功后解析明细
                {

                    BOCRefundResponseDtl dtl = null;//明细对象
                    var detailInfo = from c in xdoc.Descendants("b2e0009-rs")
                                     select c;
                    if (detailInfo != null && detailInfo.Count() > 0)
                        this.RefundResponseDtlLst = new List<BOCRefundResponseDtl>();
                    foreach (var info in detailInfo)
                    {
                        dtl = new BOCRefundResponseDtl();
                        var dtlStatus = from s in info.Descendants("status")
                                        select new
                                        {
                                            rspcod = s.Element("rspcod") == null ? string.Empty : s.Element("rspcod").Value,
                                            rspmsg = s.Element("rspmsg") == null ? string.Empty : s.Element("rspmsg").Value
                                        };
                        if (dtlStatus != null && dtlStatus.Count() > 0)
                        {
                            dtl.RspCod = dtlStatus.FirstOrDefault().rspcod;
                            dtl.RspMsg = dtlStatus.FirstOrDefault().rspmsg;
                        }
                        var insid = info.Element("insid") == null ? string.Empty : info.Element("insid").Value;//指令ID，请求时给出的ID
                        var obssid = info.Element("obssid") == null ? string.Empty : info.Element("obssid").Value;//每条划账指令的网银划账流水号
                        dtl.InsId = insid;
                        dtl.ObssId = obssid;
                        this.RefundResponseDtlLst.Add(dtl);
                    }
                }

            }
            catch (Exception ex)
            {
                rst = false;
                LogTxt.WriteEntry("退款解析异常:" + ex.Message, "中行转账退款明细信息");
                throw ex;
            }
            return rst;
        }



        /// <summary>
        /// 退款返回情况明细
        /// </summary>
        public class BOCRefundResponseDtl
        {
            /// <summary>
            /// 交易返回码  B001,成功； 
            /// </summary>
            public string RspCod { get; set; }
            /// <summary>
            ///  rspmsg表示解释信息
            /// </summary>
            public string RspMsg { get; set; }
            /// <summary>
            /// 指令ID，请求时给出的ID
            /// </summary>
            public string InsId { get; set; }
            /// <summary>
            /// 每条划账指令的网银划账流水号
            /// </summary>
            public string ObssId { get; set; }
        }
    }
}
