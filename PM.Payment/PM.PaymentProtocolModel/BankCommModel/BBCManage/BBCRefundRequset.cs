using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel
{
    /// <summary>
    /// 退款操作
    /// </summary>
    public class   BBCRefundRequset : CommunicationBase
    {
        
        /// <summary>
        /// 交易代码
        /// </summary>
        public virtual string TransCode { get { return "3031"; }  
      
        }
        /// <summary>
        /// 交易日期  YYYYMMDD
        /// </summary>
        public string TransDate { get; set; }
        /// <summary>
        /// 交易时间  HHMMSS
        /// </summary>
        public string TransTime { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string SeqNo { get; set; }
        /// <summary>
        /// 中心授权码
        /// </summary>
        public string AuthCode { get; set; }
        /// <summary>
        /// 标段编号
        /// </summary>
        public string BiaoDunNo { get; set; }
        /// <summary>
        /// 虚拟账户
        /// </summary>
        public string IAcctNo { get; set; }
        public List<BBCRefundInfo>  BBCRefundList { get; set; }

        /// <summary>
        /// 获取报文数据
        /// </summary>
        /// <returns></returns>
        public virtual string GetMessagePaket()
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
            sb.Append("<BiaoDunNo>{3}</BiaoDunNo>");
            sb.Append("<SeqNo>{4}</SeqNo>");
          
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("<IAcctNo>{5}</IAcctNo>");
            sb.Append("<AuthCode>{6}</AuthCode>");          
            sb.Append("<TotalNum>{7}</TotalNum>");
            foreach (var bank in  BBCRefundList)
            {
                #region 明细
                sb.Append("<BanK>");
                sb.Append("<BankNo>");
                sb.Append(bank.BankNo);
                sb.Append("</BankNo>");
                sb.Append("<BankName>");
                sb.Append(bank.BankName);
                sb.Append("</BankName>");
                sb.Append("<HstSeqNum>");
                sb.Append(bank.HstSeqNum);
                sb.Append("</HstSeqNum>");
                sb.Append("<InAcctNo>");
                sb.Append(bank.InAcctNo);
                sb.Append("</InAcctNo>");
                sb.Append("<InName>");
                sb.Append(bank.InName);
                sb.Append("</InName>");
                sb.Append("<InDate>");
                sb.Append(bank.InDate);
                sb.Append("</InDate>");
                sb.Append("<InTime>");
                sb.Append(bank.InTime);
                sb.Append("</InTime>");
                sb.Append("<InTranAmt>");
                sb.Append(bank.InTranAmt);
                sb.Append("</InTranAmt>");
                sb.Append("</BanK>");
                #endregion
            }

            sb.Append("</body>");
            sb.Append("</root>");
            var sendInfo = string.Format(sb.ToString()
            , this.TransCode
            , this.TransDate
            , this.TransTime
            , this.BiaoDunNo
            , this.SeqNo
            ,this.IAcctNo
            , this.AuthCode
            , BBCRefundList.Count()
            );

            var strCount = PM.Utils.StringHelper.Text_Length(sendInfo)+2;
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
    /// 退款明细
    /// </summary>
    public class BBCRefundInfo
    {
        /// <summary>
        /// 开户行行号
        /// </summary>
        public string BankNo { get; set; }
        /// <summary>
        /// 开户行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 转账流水号
        /// </summary>
        public string HstSeqNum { get; set; }
        /// <summary>
        /// 转入账号
        /// </summary>
        public string InAcctNo { get; set; }
        /// <summary>
        /// 转入户名
        /// </summary>
        public string InName { get; set; }
        /// <summary>
        /// 到账日期
        /// </summary>
        public string InDate { get; set; }
        /// <summary>
        /// 到账时间
        /// </summary>
        public string InTime { get; set; }
        /// <summary>
        /// 转入本金金额
        /// </summary>
        public string InTranAmt { get; set; }
    }
}
