using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PM.PaymentModel.BizModel.HSTRCB
{
    /// <summary>
    /// 农商行创建虚拟账号
    /// </summary>
    public class HSTRCBVirtualAccount : HSTRCBCommBase
    {
        /// <summary>
        /// 交易代码
        /// </summary>
        public string TransCode
        {
            get
            {
                return "3001";
            }
        }

        /// <summary>
        /// 母账户账号  (中心账号)
        /// </summary>
        public string AcctNo { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 项目标段号
        /// </summary>
        public string BiaoDuanNo { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 开标日期 YYYYMMDD
        /// </summary>
        public string OpenDate { get; set; }
        /// <summary>
        /// 开标时间  HHMMSS
        /// </summary>
        public string OpenTime { get; set; }
        /// <summary>
        /// 是否退息0- 是,1- 否
        /// </summary>
        public string IsRetire { get; set; }
        /// <summary>
        /// 到期日
        /// </summary>
        public string MatuDay { get; set; }
        /// <summary>
        /// 获取报文数据
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
            sb.Append("<AcctNo>{4}</AcctNo>");
            sb.Append("<ProjectNo>{5}</ProjectNo>");
            sb.Append("<BiaoDuanNo>{6}</BiaoDuanNo>");
            sb.Append("<ProjectName>{7}</ProjectName>");
            sb.Append("<OpenDate>{8}</OpenDate>");
            sb.Append("<OpenTime>{9}</OpenTime>");
            sb.Append("<IsRetire>{10}</IsRetire>");
            sb.Append("<MatuDay>{11}</MatuDay>");
            sb.Append("</body>");
            sb.Append("</root>");
            var sendInfo = string.Format(sb.ToString()
            , this.TransCode
             , this.TransDate
            , this.TransTime
            , this.SeqNo
            , this.AcctNo
            , this.ProjectNo
            , this.BiaoDuanNo
            , this.ProjectName
            , this.OpenDate
            , this.OpenTime
            , this.IsRetire
            , this.MatuDay
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
    /// 虚拟账号创建返回
    /// </summary>
    public class HSTRCBVirtualAccountResponse
    {
        /// <summary>
        /// 交易代码
        /// </summary>
        public string TransCode { get; set; }
        /// <summary>
        /// 交易结果信息
        /// </summary>
        public string TransRltMsg { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string SeqNo { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        public string TransDate { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public string TransTime { get; set; }
        /// <summary>
        /// 虚拟子账号
        /// </summary>
        public string IAcctNo { get; set; }
        /// <summary>
        /// 虚拟子账户授权码
        /// </summary>
        public string AuthCode { get; set; }
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
                var xdoc = XDocument.Parse(packetString.Substring(12));//是否需要12位去掉
                var cmp = from c in xdoc.Descendants("body")
                          select new
                            {
                                TransCode = c.Element("TransCode") == null ? string.Empty : c.Element("TransCode").Value,
                                TransRltMsg = c.Element("TransRltMsg") == null ? string.Empty : c.Element("TransRltMsg").Value,
                                SeqNo = c.Element("SeqNo") == null ? string.Empty : c.Element("SeqNo").Value,
                                TransDate = c.Element("TransDate") == null ? string.Empty : c.Element("TransDate").Value,
                                TransTime = c.Element("TransTime") == null ? string.Empty : c.Element("TransTime").Value,
                                IAcctNo = c.Element("IAcctNo") == null ? string.Empty : c.Element("IAcctNo").Value,
                                AuthCode = c.Element("AuthCode") == null ? string.Empty : c.Element("AuthCode").Value
                            };
                if (cmp != null && cmp.Count() > 0)
                {
                    rst = true;
                    this.TransCode = cmp.FirstOrDefault().TransCode;
                    this.TransRltMsg = cmp.FirstOrDefault().TransRltMsg;
                    this.SeqNo = cmp.FirstOrDefault().SeqNo;
                    this.TransDate = cmp.FirstOrDefault().TransDate;
                    this.TransTime = cmp.FirstOrDefault().TransTime;

                    this.IAcctNo = cmp.FirstOrDefault().IAcctNo;
                    this.AuthCode = cmp.FirstOrDefault().AuthCode;
                }
            }
            catch (Exception e)
            {
                rst = false;
                this.TransRltMsg = e.Message;
            }
            return rst;
        }
    }
}
