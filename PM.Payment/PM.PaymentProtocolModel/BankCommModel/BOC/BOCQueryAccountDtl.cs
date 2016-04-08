using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.BOC
{
    /// <summary>
    /// 出入账明细请求
    /// </summary>
    public class BOCQueryAccountDtl : BOCBase
    {
        /// <summary>
        /// 查询账户的联行号1、非空数码5位  2、联行号有对应的省行联行号
        /// </summary>
        public string IbkNum { get; set; }

        /// <summary>
        /// 查询账户的账号
        ///1、	非空数码字符串1-20位
        ///2、操作员有权限 
        /// </summary>
        public string Actacn { get; set; }
        /// <summary>
        /// 查询类型	非空枚举：2001（当日查询）、2002（历史查询）、2005 （T+1查询T日夜间批量交易）
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 开始日期（含） 	非空YYYYMMDD	
        /// 查询类型为2002，日期为当前日期之前一年内，
        /// 跨度为一个自然月；2001,系统默认取当前日期；
        /// 如果查询类型是2005，系统默认取当前日期前一天
        /// </summary>
        public string DatescopeFrom { get; set; }
        /// <summary>
        /// 截止日期（含）	非空YYYYMMDD
        /// </summary>
        public string DatescopeTo { get; set; }
        /// <summary>
        /// 下限(含)	可空正数数字, 长度（22，2）	1、可空正数数字, 长度（18，2）  2、为空时，系统默认为0.00
        /// </summary>
        public string AmountscopeFrom { get; set; }
        /// <summary>
        /// 上限（含）	可空正数数字, 长度（22，2）	1、可空正数数字, 长度（18，2）  2、大于金额下限，为空时，系统默认为 999999999999999999.99。
        /// </summary>
        public string AmountscopeTo { get; set; }
        /// <summary>
        /// 本次查询的交易起始位置	可空数码最长8位	第一次查询以1开始；如果该项为空则默认为1
        /// </summary>
        public string BegNum { get; set; }
        /// <summary>
        /// 查询记录数	可空数码2位，不足2位前面补0	最大50，如果该项为空则默认为1。不足2位，前补0
        /// </summary>
        public string RecNum { get; set; }
        /// <summary>
        /// 来往账标识	非空字母数字1位	
        /// 非空枚举：0-全部，1-来账，2-往账,3-内部往来，4-外部交易，5 部分内部往来 其中3、4、5只有现金3.0客户可以上送
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// 设置报文体
        /// </summary>
        /// <returns></returns>
        internal override string GetTranMessagePaket()
        {
            string stringLenth = string.Empty;//字符长度
            string rtnString = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("<trans>");
            sb.Append("<trn-b2e0035-rq>");
            sb.Append("<b2e0035-rq>");
            // 
            sb.Append("<ibknum>{0}</ibknum>");
            sb.Append("<actacn>{1}</actacn>");
            sb.Append("<type>{2}</type>");
            #region  区间
            sb.Append("<datescope>");
            sb.Append("<from>{3}</from>");
            sb.Append("<to>{4}</to>");
            sb.Append("</datescope>");
            /////
            sb.Append("<amountscope>");
            sb.Append("<from>{5}</from>");
            sb.Append("<to>{6}</to>");
            sb.Append("</amountscope>");
            #endregion
            sb.Append("<begnum>{7}</begnum>");
            sb.Append("<recnum>{8}</recnum>");
            sb.Append("<direction>{9}</direction>");
            // 
            sb.Append("</b2e0035-rq>");
            sb.Append("</trn-b2e0035-rq>");
            sb.Append("</trans>");
            var sendInfo = string.Format(sb.ToString()
                , this.IbkNum
                , this.Actacn
                , this.Type
                , this.DatescopeFrom
                , this.DatescopeTo
                , this.AmountscopeFrom
                , this.AmountscopeTo
                , this.BegNum
                , this.RecNum
                , this.Direction
            );
            this.Trncod = "b2e0035";//交易类型
            return sendInfo;
        }

    }
}
