using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PM.Utils.Log;

namespace PM.PaymentProtocolModel.BankCommModel.BOC
{
    /// <summary>
    /// 签到请求
    /// </summary>
    public class BOCSignInRequest : BOCBase
    {
        /// <summary>
        /// 用户签名信息，该标签由前置机自动添加，企业无须上送
        /// </summary>
        public string Ceitinfo { get; set; }
        /// <summary>
        /// 客户端日期时间，YYYYMMDDHH24MISS
        /// </summary>
        public string CustDt { get; set; }
        /// <summary>
        /// 登录密码 检查登录密码,当日输错5次锁定；累计输错15次锁定。
        /// </summary>
        public string OprPwd { get; set; }
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
            sb.Append("<trn-b2e0001-rq>");//请求
            //sb.Append("<ceitinfo>{0}</ceitinfo>");

            sb.Append("<b2e0001-rq>");
            sb.Append("<custdt>{0}</custdt>");
            sb.Append("<oprpwd>{1}</oprpwd>");
            sb.Append("</b2e0001-rq>");

            sb.Append("</trn-b2e0001-rq>");
            sb.Append("</trans>");
            var sendInfo = string.Format(sb.ToString()
             //, this.Ceitinfo
            , this.CustDt ?? DateTime.Now.ToString("yyyyMMddHHmmss")
            , this.OprPwd
            );
            this.Trncod = "b2e0001";
            return sendInfo;
        }     
    }

    /// <summary>
    /// 签到响应
    /// </summary>
    public class BOCSignInResponse
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
        /// 交易验证标识，签到时返回
        /// </summary>
        public string Token { get; set; }

        public bool GetModel(string packetString)
        {
            bool rst = false;
            try
            {
                var xdoc = XDocument.Parse(packetString);//
                var head = from c in xdoc.Descendants("head")
                           select new
                             { 
                                 termid = c.Element("termid") == null ? string.Empty : c.Element("termid").Value,
                                 trnid = c.Element("trnid") == null ? string.Empty : c.Element("trnid").Value,
                                 custid = c.Element("custid") == null ? string.Empty : c.Element("custid").Value,
                                 cusopr = c.Element("cusopr") == null ? string.Empty : c.Element("cusopr").Value,
                                 trncod = c.Element("trncod") == null ? string.Empty : c.Element("trncod").Value,
                                 token = c.Element("token") == null ? string.Empty : c.Element("token").Value

                             };
                if (head != null && head.Count() > 0)
                {
                    this.Token = head.FirstOrDefault().token; 
                }

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
                                     serverdt = c.Element("serverdt") == null ? string.Empty : c.Element("serverdt").Value,
                                     token = c.Element("token") == null ? string.Empty : c.Element("token").Value
                                 };
                if (tokenInfo != null && tokenInfo.Count() > 0)
                {
                    this.ServerDt = tokenInfo.FirstOrDefault().serverdt;
                    this.Token = tokenInfo.FirstOrDefault().token;
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
