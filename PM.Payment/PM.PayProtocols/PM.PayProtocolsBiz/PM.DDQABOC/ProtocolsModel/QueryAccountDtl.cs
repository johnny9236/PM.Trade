using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PM.DDQABOC.ProtocolsModel
{
    /// <summary>
    /// 查询账户明细请求
    /// </summary>
    public class QueryAccountDtl : PubRequestPackets
    {
        /// <summary>
        /// 业务代码(默认CQRA10)
        /// </summary>
        public override string CCTransCode { get { return "CQRA10"; } }
        /// <summary>
        /// 起始日期
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 终止日期
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        ///末笔日志号 默认为0
        /// </summary>
        public string LastJrnNo { get; set; }

        /// <summary>
        /// 借方账号
        /// </summary>
        public string AccNo { get; set; }
        /// <summary>
        /// 借方省市名称
        /// </summary>
        public string Prov { get; set; }
        /// <summary>
        /// 借方货币号(默认是写的人民币)
        /// </summary>
        public string Cur { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string StartTime { get; set; }

        //#region
        ///// <summary>
        ///// 文件根目录(或者ftp地址)
        ///// </summary>
        //public string RootFilePath { get; set; }
        ///// <summary>
        ///// ftp用户名
        ///// </summary>
        //public string FtpUserName { get; set; }
        ///// <summary>
        ///// ftp密码
        ///// </summary>
        //public string FtpUserPwd { get; set; }
        ///// <summary>
        ///// 文件类型
        ///// </summary>
        //public FileTp FileType { get; set; }
        //#endregion


        ///// <summary>
        ///// 支付行账号
        ///// </summary>
        //public string FromAccNo { get; set; }
        ///// <summary>
        ///// 支付方账户名    
        ///// </summary>
        //public string FromAccount { get; set; }
        ///// <summary>
        ///// 支付行银行
        ///// </summary>
        //public string FromAccBank { get; set; }
        ///// <summary>
        ///// 金额
        ///// </summary>
        //public double Amount { get; set; }
        ///// <summary>
        ///// 核对时间（凭证录入）
        ///// </summary>
        //public string CheckTm { get; set; }

        ///// <summary>
        ///// 摘要
        ///// </summary>
        //public string Abs { get; set; }
        ///// <summary>
        ///// 断言
        ///// </summary>
        //public string PostScript { get; set; }
        ///// <summary>
        ///// 来源
        ///// </summary>
        //public string TrFrom { get; set; }
        /// <summary>
        /// 设置发送报文信息
        /// </summary>
        /// <returns></returns>
        public override XDocument SetRequsetPak()
        {
            XDocument myXDoc = base.SetRequsetPak();
            myXDoc.Element("ap").Add(
                new XElement("Corp",
                    new XElement("StartDate", this.StartDate),
                    new XElement("EndDate", this.EndDate)),
                         new XElement("Channel", new XElement("LastJrnNo", this.LastJrnNo)),
                          new XElement("Cmp",
                              new XElement("DbAccNo", this.AccNo),
                              new XElement("DbProv", Help.GetProv(this.Prov)),
                              new XElement("DbCur", Help.GetCur(this.Cur)),
                                new XElement("StartTime", this.StartTime)));
            return myXDoc;
        }
    }
}
