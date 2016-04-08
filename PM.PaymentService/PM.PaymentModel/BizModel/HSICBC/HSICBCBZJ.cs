using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentModel.BizModel.HSICBC
{
    /// <summary>
    ///黄山 保证金截止时间修改
    /// </summary>
    public class HSICBCZJRequest : HSICBCCommBase
    {
        /// <summary>
        /// 交易代码
        /// </summary>
        public string TransCode { get { return "3061"; } }
        /// <summary>
        /// 项目标段号
        /// </summary>
        public string BiaoDuanNo { get; set; }
        /// <summary>
        /// 保证金截止日期
        /// </summary>
        public string BZJEndDate { get; set; }
        /// <summary>
        /// 保证金截止时间
        /// </summary>
        public string BZJEndTime { get; set; }
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
            sb.Append("<?xml version='1.0' encoding='gb2312'?>");
            sb.Append("<root>");
            sb.Append("<head>");
            sb.Append("<TransCode>{0}</TransCode>");
            sb.Append("<TransDate>{1}</TransDate>");
            sb.Append("<TransTime>{2}</TransTime>");
            sb.Append("<SeqNo>{3}</SeqNo>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("<BiaoDuanNo>{4}</BiaoDuanNo>");
            sb.Append("<BZJEndDate>{5}</BZJEndDate>");
            sb.Append("<BZJEndTime>{6}</BZJEndTime>");
            sb.Append("<AuthCode>{7}</AuthCode>");
            sb.Append("</body>");
            sb.Append("</root>");
            var sendInfo = string.Format(sb.ToString()
                , this.TransCode
                , this.TransDate
                , this.TransTime
                , this.SeqNo
                , this.BiaoDuanNo
                , this.BZJEndDate
                , this.BZJEndTime
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
}
