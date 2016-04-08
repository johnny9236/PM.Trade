using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PM.Utils.Log;

namespace PM.PaymentProtocolModel.BankCommModel.TRCB
{
    /// <summary>
    /// 退款明细查询
    /// </summary>
    public class TRCBQueryRtnResultModel
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
        public List<TRCBQueryInfo> TRCBRtnQueryList { get; set; }
        /// <summary>
        /// 获取明细对象
        /// </summary>
        /// <param name="packetString"></param>
        /// <returns></returns>
        public virtual bool GetModel(string packetString)
        {
            bool rst = false;
            try
            {
                var xdoc = XDocument.Parse(packetString);//
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
                                     Result = c.Element("Result") == null ? string.Empty : c.Element("Result").Value,
                                     AddWord = c.Element("AddWord") == null ? string.Empty : c.Element("AddWord").Value
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
                                     InDate = c.Element("InDate") == null ? string.Empty : c.Element("InDate").Value,
                                     InTime = c.Element("InTime") == null ? string.Empty : c.Element("InTime").Value,
                                     InAmount = c.Element("InAmount") == null ? string.Empty : c.Element("InAmount").Value,
                                     InName = c.Element("InName") == null ? string.Empty : c.Element("InName").Value,
                                     InAcct = c.Element("InAcct") == null ? string.Empty : c.Element("InAcct").Value,

                                     InMemo = c.Element("InMemo") == null ? string.Empty : c.Element("InMemo").Value,
                                     HstSeqNum = c.Element("HstSeqNum") == null ? string.Empty : c.Element("HstSeqNum").Value,
                                     BackResult = c.Element("Result") == null ? string.Empty : c.Element("Result").Value,
                                     AddWord = c.Element("AddWord") == null ? string.Empty : c.Element("AddWord").Value
                                 };
                if (bankList != null && bankList.Count() > 0)
                    this.TRCBRtnQueryList = new List<TRCBQueryInfo>();
                foreach (var bank in bankList)
                {
                    var dtl = new TRCBQueryInfo();
                    dtl.InDate = bank.InDate;
                    dtl.InTime = bank.InTime;
                    dtl.InAmount = bank.InAmount;
                    dtl.InName = bank.InName;
                    dtl.InAcct = bank.InAcct;
                    dtl.InMemo = bank.InMemo;
                    dtl.HstSeqNum = bank.HstSeqNum;
                    dtl.BackResult = bank.BackResult;
                    dtl.AddWord = bank.AddWord;
                    this.TRCBRtnQueryList.Add(dtl);
                }
            }
            catch (Exception ex)
            {
                rst = false;
                LogTxt.WriteEntry("异常信息:" + ex.Message, "青阳退款明细");
                throw ex;
            }
            return rst;
        }
    }
}
