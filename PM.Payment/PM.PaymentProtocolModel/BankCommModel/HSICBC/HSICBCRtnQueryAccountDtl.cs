using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils;

namespace PM.PaymentProtocolModel.BankCommModel.HSICBC
{
    /// <summary>
    /// 退款明细查询
    /// </summary>
    public class HSICBCRtnQueryAccountDtl : ICBCQueryOrRtnQueryAccountDtl
    {
        /// <summary>
        /// 交易代码        退保证金查询 6003
        /// </summary>
        public override string TransCode
        {
            get
            {
                return base.TransCode;
            }
            set
            {
                base.TransCode = value;
            }
        }
        #region      保证金退回明细
        /// <summary>
        /// 母账号
        /// </summary>
        public string AcctNo { get; set; }

        /// <summary>
        /// 开始日期时间
        /// </summary>
        public string StartDateTime { get; set; }
        /// <summary>
        /// 结束日期时间
        /// </summary>
        public string EndDateTime { get; set; }
        /// <summary>
        /// 起始笔数
        /// </summary>
        public string StartNum { get; set; }
        /// <summary>
        /// 查询笔数 默认50笔
        /// </summary>
        public string EndNum { get; set; }


        #endregion
        /// <summary>
        /// 产生报文
        /// </summary>
        /// <returns></returns>
        public override string GetMessagePaket()
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
            sb.Append("<AcctNo>{7}</AcctNo>");
            sb.Append("<StartDateTime >{8}</StartDateTime >");
            sb.Append("<EndDateTime >{9}</EndDateTime >");
            sb.Append("<StartNum>{10}</StartNum>");
            sb.Append("<EndNum>{11}</EndNum>");
            sb.Append("</body>");
            sb.Append("</root>");
            var sendInfo = string.Format(sb.ToString()
            , this.TransCode
            , this.TransDate
            , this.TransTime
            , this.SeqNo
            , this.ItemNo
            , this.ItemNoX
            , this.AuthCode
            ,this.AcctNo
            ,this.StartDateTime
            ,this.EndDateTime
            ,this.StartNum
            ,this.EndNum
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
