using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PM.Utils.Log;

namespace PM.PaymentProtocolModel.BankCommModel
{
    public class ICBCRefundResponse
    {
        /// <summary>
        /// 交易代码
        /// </summary>
        public virtual  string TransCode { get; set; }
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
        /// 应答描述
        /// </summary>
        public string AddWord { get; set; }
        /// <summary>
        /// 保证金退回返回列表
        /// </summary>
        public List<ICBCReturnRefundDtl> ICBCReturnRefundDtlList { get; set; }
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
                                     AddWord = c.Element("AddWord") == null ? string.Empty : c.Element("AddWord").Value
                                 };
                //返回结果
                if (head != null && head.Count() > 0)
                {
                    this.AddWord = bodyInfo.FirstOrDefault().AddWord;
                }
                //明细列表
                var bankList = from c in xdoc.Descendants("bank")
                               select new
                                 {
                                     HstSeqNum = c.Element("HstSeqNum") == null ? string.Empty : c.Element("HstSeqNum").Value,
                                     InAcctNo = c.Element("InAcctNo") == null ? string.Empty : c.Element("InAcctNo").Value,
                                     InName = c.Element("InName") == null ? string.Empty : c.Element("InName").Value,
                                     Result = c.Element("Result") == null ? string.Empty : c.Element("Result").Value
                                 };
                if (bankList != null && bankList.Count() > 0)
                    this.ICBCReturnRefundDtlList = new List<ICBCReturnRefundDtl>();
                foreach (var bank in bankList)
                {
                    var dtl = new ICBCReturnRefundDtl();
                    dtl.HstSeqNum = bank.HstSeqNum;
                    dtl.InAcctNo = bank.InAcctNo;
                    dtl.InName = bank.InName;
                    dtl.Result = bank.Result;

                    this.ICBCReturnRefundDtlList.Add(dtl);
                }
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("异常信息:" + ex.Message, "工行退款明细");
                rst = false;
                throw ex;
            }
            return rst;
        }

    }
    /// <summary>
    /// 保证金退回返回
    /// </summary>
    public class ICBCReturnRefundDtl
    {
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
        /// 应答状态 1成功 0失败
        /// </summary>
        public string Result { get; set; }

    }
}
