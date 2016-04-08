using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.TRCB;

namespace PM.PaymentProtocolModel.BankCommModel.HSanTRCB
{
    /// <summary>
    /// 退款请求
    /// </summary>
    public class HSanTRCBRefundRequset : TRCBRefundRequset
    {
        /// <summary>
        /// 是否人工
        /// </summary>
        public bool ISManual { get; set; }
        /// <summary>
        /// 交易代码
        /// </summary>
        public override string TransCode
        {
            get
            {
                return "3031";
                //  return base.TransCode;
            }
            //set
            //{
            //    base.TransCode = value;
            //}
        }
        /// <summary>
        /// 人工退款明细
        /// </summary>
        public List<HSanTRCBRefundInfo> HSanTRCBRefundList { get; set; }

        /// <summary>
        /// 获取报文
        /// </summary>
        /// <returns></returns>
        public override string GetMessagePaket()
        {
            return base.GetMessagePaket();
        }
        /// <summary>
        /// 人工退款报文
        /// </summary>
        /// <returns></returns>
        public string GetManualMessagePaket()
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
            sb.Append("<BiaoDunNo>{3}</BiaoDunNo>");
            sb.Append("<SeqNo>{4}</SeqNo>");
            sb.Append("<AuthCode>{5}</AuthCode>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("<TotalNum>{6}</TotalNum>");
            foreach (var bank in HSanTRCBRefundList)
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
            , "3081"//this.TransCode
            , this.TransDate
            , this.TransTime
            , this.BiaoDunNo
            , this.SeqNo
                     , this.AuthCode
            , HSanTRCBRefundList.Count()
            );

            var strCount = PM.Utils.StringHelper.Text_Length(sendInfo) + 2;
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
    public class HSanTRCBRefundInfo : TRCBRefundInfo
    {
    }
}
