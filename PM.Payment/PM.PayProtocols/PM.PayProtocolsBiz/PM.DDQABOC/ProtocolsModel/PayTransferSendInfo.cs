using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PM.DDQABOC.ProtocolsModel
{
    /// <summary>
    /// 转账信息请求信息
    /// </summary>
    public class PayTransferSendInfo : PubRequestPackets
    {
        /// <summary>
        /// 业务代码(默认CFRT02)
        /// </summary>
        public override string CCTransCode { get { return "CFRT02"; } }
        /// <summary>
        /// 金额
        /// </summary>
        public double Amt { get; set; }
        /// <summary>
        /// 借方省市代码
        /// </summary>
        public string DbProv { get; set; }
        /// <summary>
        /// 借方账号(收款)
        /// </summary>
        public string DbAccNo { get; set; }
        /// <summary>
        /// 借方逻辑账号
        /// </summary>
        public string DbLogAccNo { get; set; }
        /// <summary>
        /// 借方货币码
        /// </summary>
        public string DbCur { get; set; }
        /// <summary>
        /// 贷方省市代码
        /// </summary>
        public string CrProv { get; set; }
        /// <summary>
        /// 贷方账号（支付）
        /// </summary>
        public string CrAccNo { get; set; }
        /// <summary>
        /// 贷方逻辑账号
        /// </summary>
        public string CrLogAccNo { get; set; }
        /// <summary>
        /// 贷方货币码
        /// </summary>
        public string CrCur { get; set; }
        /// <summary>
        /// 校验贷方户名标志   0 不校验  1校验
        /// </summary>
        public string ConFlag { get; set; }
        /// <summary>
        /// 大额支付标识节点PsFlag暂时无用，送空即可
        /// </summary>
        public string PsFlag
        {
            get { return string.Empty; }
            //set;
        }
        /// <summary>
        /// 预约标志   0 不预约 1预约
        /// </summary>
        public string BookingFlag { get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        public string BookingDate { get; set; }
        /// <summary>
        /// 预约时点
        /// </summary>
        public string BookingTime { get; set; }
        /// <summary>
        /// 加急标志 0 不加急 1 加急
        /// </summary>
        public string UrgencyFlag { get; set; }
        /// <summary>
        /// 它行标志  0 本行 1他行
        /// </summary>
        public string OthBankFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 它行行别
        /// </summary>
        public string CrBankType { get; set; }
        /// <summary>
        /// 收款方户名
        /// </summary>
        public string CrAccName { get; set; }
        /// <summary>
        /// 收款方开户行名
        /// </summary>
        public string CrBankName { get; set; }
        /// <summary>
        /// 收款方开户行号
        /// </summary>
        public string CrBankNo { get; set; }
        /// <summary>
        /// 付款方户名
        /// </summary>
        public string DbAccName { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        public string WhyUse { get; set; }
        /// <summary>
        /// 附言
        /// </summary>
        public string Postscript { get; set; }
        /// <summary>
        /// 设置发送报文信息
        /// </summary>
        /// <returns></returns>
        public override XDocument SetRequsetPak()
        {
            XDocument myXDoc = base.SetRequsetPak();
            myXDoc.Element("ap").Add(
                new XElement("Amt", this.Amt),
                          new XElement("Cmp",
                              new XElement("DbProv", Help.GetProv(this.DbProv)),
                              new XElement("DbAccNo", this.DbAccNo),
                                new XElement("DbLogAccNo", this.DbLogAccNo),
                                     new XElement("DbCur", Help.GetCur(this.DbCur)),

                                           new XElement("CrProv", Help.GetProv(this.CrProv)),
                                              new XElement("CrAccNo", this.CrAccNo),
                                                 new XElement("CrLogAccNo", this.CrLogAccNo),
                                                 new XElement("CrCur", Help.GetCur(this.CrCur)),
                                                     new XElement("ConFlag", this.ConFlag)),

                new XElement("Corp",
                    new XElement("PsFlag", this.PsFlag),
                    new XElement("BookingFlag", this.BookingFlag),
                         new XElement("BookingDate", this.BookingDate),
                              new XElement("BookingTime", this.BookingTime),
                                new XElement("UrgencyFlag", this.UrgencyFlag),
                                  new XElement("OthBankFlag", this.OthBankFlag),
                                    new XElement("CrBankType", this.CrBankType),
                                      new XElement("CrAccName", this.CrAccName),
                                       new XElement("CrBankName", this.CrBankName),
                                        new XElement("CrBankNo", this.CrBankNo),
                                           new XElement("DbAccName", this.DbAccName),
                                              new XElement("WhyUse", this.WhyUse),
                                                 new XElement("Postscript", this.WhyUse)));
            return myXDoc;
        }

    }
     
}
