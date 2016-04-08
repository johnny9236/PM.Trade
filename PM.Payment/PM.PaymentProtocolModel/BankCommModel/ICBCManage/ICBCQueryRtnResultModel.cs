using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PM.Utils;
using PM.Utils.Log;

namespace PM.PaymentProtocolModel.BankCommModel
{
    /// <summary>
    /// 退款明细查询
    /// </summary>
    public class ICBCQueryRtnResultModel
    {
        /// <summary>
        /// 交易代码
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
        /// 应答代码 1成功 0失败
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 应答描述 应答描述
        /// </summary>
        public string AddWord { get; set; }
        /// <summary>
        /// 明细列表
        /// </summary>
        public List<ICBCRtnQueryInfo> ICBCRtnQueryList { get; set; }
        /// <summary>
        /// 获取明细对象
        /// </summary>
        /// <param name="packetString"></param>
        /// <returns></returns>
        public virtual  bool GetModel(string packetString)
        {
            bool rst = false;
            try
            {
                var xdoc = XDocument.Parse(packetString.Substring(12));//是否需要12位去掉
                var head = from c in xdoc.Descendants("head")
                           select new
                             {
                                 TransCode = c.Element("TransCode") == null ? string.Empty : c.Element("TransCode").Value,
                                 SeqNo = c.Element("SeqNo") == null ? string.Empty : c.Element("SeqNo").Value,
                                 TransDate = c.Element("TransDate") == null ? string.Empty : c.Element("TransDate").Value,
                                 TransTime = c.Element("TransTime") == null ? string.Empty : c.Element("TransTime").Value
                             };
                if (head != null && head.Count() > 0)
                {
                    this.TransCode = head.FirstOrDefault().TransCode;
                    this.SeqNo = head.FirstOrDefault().SeqNo;
                    this.TransDate = head.FirstOrDefault().TransDate;
                    this.TransTime = head.FirstOrDefault().TransTime;
                }

                var bodyInfo = from c in xdoc.Descendants("body")
                               select new
                                 {
                                     Result = c.Element("Result ") == null ? string.Empty : c.Element("Result ").Value,
                                     AddWord = c.Element("AddWord ") == null ? string.Empty : c.Element("AddWord ").Value
                                 };
                //返回结果
                if (head != null && head.Count() > 0)
                {
                    this.Result = bodyInfo.FirstOrDefault().Result;
                    this.AddWord = bodyInfo.FirstOrDefault().AddWord;
                    if (this.Result == "1")
                        rst = true;
                }
                //明细列表
          
                var bankList = from c in xdoc.Descendants("bank")
                               select new
                                 {
                                     RetDate = c.Element("RetDate") == null ? string.Empty : c.Element("RetDate").Value,
                                     RetTime = c.Element("RetTime") == null ? string.Empty : c.Element("RetTime").Value,
                                     RetAmount = c.Element("RetAmount") == null ? string.Empty : c.Element("RetAmount").Value,
                                     RetPunInst = c.Element("RetPunInst") == null ? string.Empty : c.Element("RetPunInst").Value,
                                     RetTotal = c.Element("RetTotal") == null ? string.Empty : c.Element("RetTotal").Value,

                                     RetName = c.Element("RetName") == null ? string.Empty : c.Element("RetName").Value,
                                     RetAcct = c.Element("RetAcct") == null ? string.Empty : c.Element("RetAcct").Value,
                                     HstSeqNum = c.Element("HstSeqNum") == null ? string.Empty : c.Element("HstSeqNum").Value,
                                     AcctNo = c.Element("AcctNo") == null ? string.Empty : c.Element("AcctNo").Value,
                                     Serial_No = c.Element("Serial_No") == null ? string.Empty : c.Element("Serial_No").Value
                                     
                                 };
                if (bankList != null && bankList.Count() > 0)
                    this.ICBCRtnQueryList = new List<ICBCRtnQueryInfo>();
                foreach (var bank in bankList)
                {
                    var dtl = new ICBCRtnQueryInfo();
                    dtl.RetDate = bank.RetDate;
                    dtl.RetTime = bank.RetTime;
                    dtl.RetAmount = bank.RetAmount;
                    dtl.RetPunInst = bank.RetPunInst;
                    dtl.RetTotal = bank.RetTotal;
                    dtl.RetName = bank.RetName;
                    dtl.HstSeqNum = bank.HstSeqNum;
                    dtl.RetAcct = bank.RetAcct;
                    dtl.HstSeqNum = bank.HstSeqNum;
                    dtl.AcctNo = bank.AcctNo;
                    dtl.Serial_No = bank.Serial_No;
                    this.ICBCRtnQueryList.Add(dtl);
                }
            }
            catch (Exception ex)
            {
                rst = false;
                LogTxt.WriteEntry("异常信息:" + ex.Message, "工行退款明细");
                throw ex;
            }
            return rst;
        }
    }
    /// <summary>
    /// 明细记录
    /// </summary>
    public class ICBCRtnQueryInfo
    {
        /// <summary>
        /// 标段编号
        /// </summary>
        public string SectionCode { get; set; }
        /// <summary>
        /// 银行
        /// </summary>
        public string BankType { get; set; }
        /// <summary>
        /// 授权码
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// 类型 业务类型 0保证金入账明细  1退款明细
        /// </summary>
        public string BusniessType { get; set; }
        /// <summary>
        /// 退还日期
        /// </summary>
        public string RetDate { get; set; }
        /// <summary>
        /// 退还时间
        /// </summary>
        public string RetTime { get; set; }
        /// <summary>
        /// 退还本金
        /// </summary>
        public string RetAmount { get; set; }
        /// <summary>
        /// 退还利息
        /// </summary>
        public string RetPunInst { get; set; }
        /// <summary>
        /// 退还本利和
        /// </summary>
        public string RetTotal { get; set; }
        /// <summary>
        /// 收款人户名
        /// </summary>
        public string RetName { get; set; }
        /// <summary>
        /// 收款人账号
        /// </summary>
        public string RetAcct { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string HstSeqNum { get; set; }
        /// <summary>
        /// 母账号
        /// </summary>
        public string AcctNo { get; set; }
        /// <summary>
        /// 现金管理平台流水
        /// </summary>
        public string Serial_No { get; set; }
    }
}
