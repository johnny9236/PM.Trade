using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.HuangMeiPostal;
using PM.PaymentProtocolModel;
using PSBCMerchant;
using PM.Utils.WebUtils;
using PM.Utils.Log;
using System.Xml.Linq;

namespace PM.HuangMeiPostalPtlBiz
{
    /// <summary>
    /// 黄梅查询入账明细
    /// </summary>
    public partial class HuangMeiPostlCommProtocols
    {
        /// <summary>
        /// 按日期查交易明细
        /// </summary>
        /// <param name="query">交易查询对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private List<HuangMeiQueryResult> GetQueryList(HuangMeiQuery query, CfgInfo cfgInfo)
        {
            string transName = "EDFR";
            string rtnString = string.Empty;
            try
            {
                string Plain = string.Format("MercCode={0}|BeginDate={1}|EndDate={2}|AcctNo={3}", query.MercCode, query.BeginDate, query.EndDate, query.AcctNo);
                string Signature = SignatureService.sign(Plain);
                string xmlString = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?><packet><transName>{0}</transName><Plain>{1}</Plain><Signature>{2}</Signature></packet>", transName, Plain, Signature);
                string contentStr = "text/xml";
                rtnString = HttpTransfer.RequestPost(cfgInfo.RequestURL, contentStr, xmlString, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(string.Format("查询账户明细返回信息内容{0}失败", ex.Message), "黄梅查询入账信息");
                throw ex;
            }
            return ParseXml(rtnString);
        }
        /// <summary>
        /// 解析报文对象
        /// </summary>
        /// <param name="xmlStr">报文</param>
        /// <returns></returns>
        private List<HuangMeiQueryResult> ParseXml(string xmlStr)
        {
            List<HuangMeiQueryResult> resultList = new List<HuangMeiQueryResult>();
            HuangMeiQueryResult result = null;
            //t解析xml前面  截取字段 
            XDocument doc = XDocument.Parse(xmlStr);
            var content = (from Plain in doc.Descendants("Plain")
                           select Plain.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(content))
                LogTxt.WriteEntry(string.Format("解析返回报文信息失败返回为空"), "黄梅查询入账信息");
            var protolStr = content.Substring(9);
            var protolDtlCount = protolStr.Split('|');
            try
            {
                int protolCount = protolDtlCount.Length / 20;
                for (int i = 0; i < protolCount; i++)
                {
                    var index = i * 20;
                    result = new HuangMeiQueryResult();
                    result.AccountNo = protolDtlCount[index + 0].Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(" ", "");
                    result.TradeDate = protolDtlCount[index + 1];
                    result.TradeSerialNumber = protolDtlCount[index + 2];
                    result.ProofKind = protolDtlCount[index + 3];
                    result.ProofNum = protolDtlCount[index + 4];
                    decimal amount = 0;
                    decimal.TryParse(protolDtlCount[index + 5], out amount);
                    result.Amount = amount;
                    result.LoanMark = protolDtlCount[index + 6];
                    decimal remainAmount = 0;
                    decimal.TryParse(protolDtlCount[index + 7], out remainAmount);
                    result.RemainAmount = remainAmount;
                    result.Oprationer = protolDtlCount[index + 8];
                    result.Checker = protolDtlCount[index + 9];
                    result.Authorizer = protolDtlCount[index + 10];
                    result.CounterpartAccountNo = protolDtlCount[index + 11];
                    result.CounterpartAccountName = protolDtlCount[index + 12];
                    result.Remark = protolDtlCount[index + 13];
                    result.TradeWay = protolDtlCount[index + 14];
                    result.TradeNo = protolDtlCount[index + 15];
                    result.TimeStamp = protolDtlCount[index + 16];
                    //result.DepartmentAccName = protolDtlCount[index + 17];
                    //result.DepartmentAccName = protolDtlCount[index + 18];
                    result.DepartmentAccName = protolDtlCount[index + 19];

                    resultList.Add(result);
                }
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(string.Format("解析返回报文信息{0}失败", ex.Message), "黄梅查询入账信息");
                throw ex;
            }
            return resultList;
        }
    }
}
