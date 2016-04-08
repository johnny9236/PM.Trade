using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils;

namespace PM.PaymentProtocolModel.BankCommModel
{

    /// <summary>
    /// 查询入账明细或退款明细
    /// </summary>
    public class ICBCQueryOrRtnQueryAccountDtl : CommunicationBase
    {
        /// <summary>
        /// 交易代码  入账明细为 3071  退保证金查询 3051
        /// </summary>
        public virtual string TransCode { get; set; }        
        /// <summary>
        /// 交易日期  YYYYMMDD
        /// </summary>
        public string TransDate { get; set; }
        /// <summary>
        /// 交易时间  HHMMSS
        /// </summary>
        public string TransTime { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string SeqNo { get; set; }
        /// <summary>
        /// 项目编号 项目编号
        /// </summary>
        public string ItemNo { get; set; }
        /// <summary>
        /// 项目标段号
        /// </summary>
        public string ItemNoX { get; set; }
        /// <summary>
        /// 中心授权码 
        /// </summary>
        public string AuthCode { get; set; }
        /// <summary>
        /// 获取报文数据
        /// </summary>
        /// <returns></returns>
        public  virtual string GetMessagePaket()
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
            sb.Append("<ItemNo>{4}</ItemNo>");
            sb.Append("<ItemNoX>{5}</ItemNoX>");
            sb.Append("<AuthCode>{6}</AuthCode>"); 
            sb.Append("</body>");
            sb.Append("</root>");
            var sendInfo = string.Format(sb.ToString()
            , this.TransCode 
            , this.TransDate
            , this.TransTime
            , this.SeqNo
            , this.ItemNo
            ,this.ItemNoX
            ,this.AuthCode           
            );

            var strCount = StringHelper.Text_Length(sendInfo);
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
