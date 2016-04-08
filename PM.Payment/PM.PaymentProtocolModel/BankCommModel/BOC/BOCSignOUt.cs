using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PM.Utils.Log;

namespace PM.PaymentProtocolModel.BankCommModel.BOC
{
    /// <summary>
    /// 中行签退请求
    /// </summary>
    public class BOCSignOUtRequset : BOCBase
    {
        /// <summary>
        /// 客户端日期时间	日期格式YYMMDDhhmmss
        /// </summary>
        public string CustDt { get; set; }
        /// <summary>
        /// 报文体消息
        /// </summary>
        /// <returns></returns>
        internal override string GetTranMessagePaket()
        {
            string stringLenth = string.Empty;//字符长度
            string rtnString = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("<trans>");
            sb.Append("<trn-b2e0002-rq>");//请求
            sb.Append("<b2e0002-rq>");
            sb.Append("<custdt>{0}</custdt>");
            sb.Append("</b2e0002-rq>");

            sb.Append("</trn-b2e0002-rq>");
            sb.Append("</trans>");
            var sendInfo = string.Format(sb.ToString()
            , this.CustDt
            );
            this.Trncod = "b2e0002";//交易类型
            return sendInfo;
        }
    }
    /// <summary>
    /// 中行签退响应
    /// </summary>
    public class BOCSignOUtResponse
    {
        /// <summary>
        /// 交易返回码  B001表示处理成功
        /// </summary>
        public string RspCod { get; set; }
        /// <summary>
        ///  rspmsg表示解释信息
        /// </summary>
        public string RspMsg { get; set; }
        /// <summary>
        /// 服务端日期时间，YYYYMMDDHH24MISS
        /// </summary>
        public string ServerDt { get; set; }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="packetString">报文</param>
        /// <returns></returns>
        public bool GetModel(string packetString)
        {
            bool rst = false;
            try
            {
                var xdoc = XDocument.Parse(packetString);// 
                var bodyInfo = from c in xdoc.Descendants("status")
                               select new
                                 {
                                     rspcod = c.Element("rspcod") == null ? string.Empty : c.Element("rspcod").Value,
                                     rspmsg = c.Element("rspmsg") == null ? string.Empty : c.Element("rspmsg").Value
                                 };
                //返回结果
                if (bodyInfo != null && bodyInfo.Count() > 0)
                {
                    this.RspCod = bodyInfo.FirstOrDefault().rspcod;
                    this.RspMsg = bodyInfo.FirstOrDefault().rspmsg;
                    if (this.RspCod.ToLower() == "b001")
                        rst = true;
                }
                //签到串
                var tokenInfo = from c in xdoc.Descendants("trn-b2e0001-rs")
                                select new
                                  {
                                      serverdt = c.Element("serverdt") == null ? string.Empty : c.Element("serverdt").Value
                                  };
                if (tokenInfo != null && tokenInfo.Count() > 0)
                {
                    this.ServerDt = tokenInfo.FirstOrDefault().serverdt;
                }
            }
            catch (Exception ex)
            {
                rst = false;
                LogTxt.WriteEntry("异常信息:" + ex.Message, "中行签到信息");
                throw ex;
            }
            return rst;
        }
    }

}
