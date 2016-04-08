using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PM.PaymentModel.BizModel.HSTRCB
{
    /// <summary>
    /// 黄山农商行开标时间
    /// </summary>
    public class HSTRCBBid : HSTRCBCommBase
    {
        /// <summary>
        /// 交易代码
        /// </summary>
        public string TransCode { get { return "3041"; } }
        /// <summary>
        /// 项目标段号
        /// </summary>
        public string BiaoDuanNo { get; set; }
        /// <summary>
        /// 开标日期
        /// </summary>
        public string OpenDate { get; set; }
        /// <summary>
        /// 开标时间
        /// </summary>
        public string OpenTime { get; set; }
        /// <summary>
        /// 中心授权码
        /// </summary>
        public string AuthCode { get; set; }
        /// <summary>
        /// 发送报文
        /// </summary>
        /// <returns></returns>
        public string GetMessagePaket()
        {
            string stringLenth = string.Empty;//字符长度
            string rtnString = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version='1.0' encoding='UTF-8'?>");
            sb.Append("<root>");
            sb.Append("<head>");
            sb.Append("<TransCode>{0}</TransCode>");
            sb.Append("<TransDate>{1}</TransDate>");
            sb.Append("<TransTime>{2}</TransTime>");
            sb.Append("<SeqNo>{3}</SeqNo>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("<BiaoDuanNo>{4}</BiaoDuanNo>");
            sb.Append("<OpenDate>{5}</OpenDate>");
            sb.Append("<OpenTime>{6}</OpenTime>");
            sb.Append("<AuthCode>{7}</AuthCode>");
            sb.Append("</body>");
            sb.Append("</root>");
            var sendInfo = string.Format(sb.ToString()
                , this.TransCode
                , this.TransDate
                , this.TransTime
                , this.SeqNo
                , this.BiaoDuanNo
                , this.OpenDate
                , this.OpenTime
                , this.AuthCode
                );

            var strCount = StringUtil.Text_Length(sendInfo);
            stringLenth = strCount.ToString();//长度为10
            for (int i = 0; i < 10 - strCount.ToString().Length; i++)
            {
                stringLenth = "0" + stringLenth;
            }
            //长度10位后加2个0
            rtnString = string.Format("{0}00{1}", stringLenth, sendInfo);
            return rtnString;
        }
    }

    /// <summary>
    /// 开标时间或保证金响应
    /// </summary>
    public class HSTRCBBidOrBZJResponse
    {
        /// <summary>
        /// 交易代码
        /// </summary>
        public string TransCode { get; set; }
        /// <summary>
        /// 应答代码
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 应答描述
        /// </summary>
        public string AddWord { get; set; }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="packetString">报文对象</param>
        /// <returns></returns>
        public bool GetModel(string packetString)
        {
            bool rst = false;
            try
            {
                var xdoc = XDocument.Parse(packetString);
                var cmp = from c in xdoc.Descendants("body")
                          select new
                            {
                                TransCode = c.Element("TransCode") == null ? string.Empty : c.Element("TransCode").Value,
                                Result = c.Element("Result") == null ? string.Empty : c.Element("Result").Value,
                                AddWord = c.Element("AddWord") == null ? string.Empty : c.Element("AddWord").Value
                            };
                if (cmp != null && cmp.Count() > 0)
                {

                    this.TransCode = cmp.FirstOrDefault().TransCode;
                    this.Result = cmp.FirstOrDefault().Result;
                    if (this.Result == "1")
                    {
                        rst = true;
                    }
                    this.AddWord = cmp.FirstOrDefault().AddWord;
                }
            }
            catch (Exception e)
            {
                rst = false;
                this.AddWord = e.Message;
            }
            return rst;
        }
    }
}
