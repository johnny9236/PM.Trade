using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.LPSBBC;
using PM.PaymentProtocolModel;
using PM.Utils.WebUtils;
using System.Xml.Linq;

namespace PM.LPSCCBPtlBiz
{
    /// <summary>
    /// 查询入账
    /// </summary>
    public partial class LPSBBCCommProtocols
    {
        /// <summary>
        /// 查询入账情况
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cfgInfo"></param>
        /// <returns></returns>
        private BBCQueryRtn GetQueryList(BBCQuery query, CfgInfo cfgInfo)
        {
            return GetQuery(query, cfgInfo);
        }

        #region
        /// <summary>
        /// http请求
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="cfgInfo"></param>
        private BBCQueryRtn GetQuery(BBCQuery query, CfgInfo cfgInfo)
        {
            var paramStr = string.Format("MERCHANTID={0}&BRANCHID={1}&POSID={2}&ORDERDATE={3}&BEGORDERTIME={4}&ENDORDERTIME={5}&ORDERID={6}&QUPWD=&TXCODE={7}&TYPE={8}&KIND={9}&STATUS={10}&SEL_TYPE={11}&PAGE={12}&OPERATOR={13}&CHANNEL={14}",
                query.MERCHANTID,
                query.BRANCHID,
                query.POSID,
                query.ORDERDATE,
                query.BEGORDERTIME,
                query.ENDORDERTIME,
                query.ORDERID,
              query.QUPWD,
              query.TXCODE,
              query.TYPE,
query.KIND,
query.STATUS,
query.SEL_TYPE,//3
query.PAGE,
query.OPERATOR,
query.CHANNEL);
            var toSign = string.Format("MERCHANTID={0}&BRANCHID={1}&POSID={2}&ORDERDATE={3}&BEGORDERTIME={4}&ENDORDERTIME={5}&ORDERID={6}&QUPWD=&TXCODE={7}&TYPE={8}&KIND={9}&STATUS={10}&SEL_TYPE={11}&PAGE={12}&OPERATOR={13}&CHANNEL={14}",
                query.MERCHANTID,
                query.BRANCHID,
                query.POSID,
                query.ORDERDATE,
                query.BEGORDERTIME,
                query.ENDORDERTIME,
                query.ORDERID,
              query.QUPWD,//签名是否需要设置空
              query.TXCODE,
              query.TYPE,
query.KIND,
query.STATUS,
query.SEL_TYPE,
query.PAGE,
query.OPERATOR,
query.CHANNEL);
            var strMD5 = PM.Utils.StringHelper.MD5String(toSign);
            //获取请求后的报文
            var responseStr = HttpTransfer.HttpRequest(cfgInfo.RequestURL, "post", "", string.Format("{0}&&MAC={1}", paramStr, strMD5), Encoding.GetEncoding("gb2312"));
            return ParseXml(responseStr);
        }

        /// <summary>
        /// 解析报文
        /// </summary>
        /// <param name="xmlStr">报文原文</param>
        /// <returns></returns>
        private BBCQueryRtn ParseXml(string xmlStr)
        {
            var queryRtn = new BBCQueryRtn();
            queryRtn.BBCQueryAccountList = new List<BBCQueryAccountRtnModel>();
            BBCQueryAccountRtnModel rtnModel = null;
            var queryXDoc = XDocument.Parse(xmlStr);
            var returnCode = (from code in queryXDoc.Descendants("RETURN_CODE")
                              select code.Value).FirstOrDefault();
            var returnMsg = (from code in queryXDoc.Descendants("RETURN_MSG")
                             select code.Value).FirstOrDefault();
            var curPage = (from code in queryXDoc.Descendants("CURPAGE")
                           select code.Value).FirstOrDefault();
            var pageCount = (from code in queryXDoc.Descendants("PAGECOUNT")
                             select code.Value).FirstOrDefault();
            if (null != returnCode)
            {
                queryRtn.RETURN_CODE = returnCode;//交易返回码
                queryRtn.RETURN_MSG = returnMsg;
                int temp_Page = 0;
                int.TryParse(curPage ?? "0", out temp_Page);
                queryRtn.CURPAGE = temp_Page;
                temp_Page = 0;
                int.TryParse(pageCount ?? "0", out temp_Page);
                queryRtn.PAGECOUNT = temp_Page;
                var queryOrders = from code in queryXDoc.Descendants("QUERYORDER")
                                  select code;
                foreach (var order in queryOrders)
                {
                    #region    赋值明细
                    rtnModel = new BBCQueryAccountRtnModel();
                    rtnModel.MERCHANTID = (from code in queryXDoc.Descendants("MERCHANTID")
                                           select code.Value).FirstOrDefault();
                    rtnModel.BRANCHID = (from code in queryXDoc.Descendants("BRANCHID")
                                         select code.Value).FirstOrDefault();
                    rtnModel.POSID = (from code in queryXDoc.Descendants("POSID")
                                      select code.Value).FirstOrDefault();
                    rtnModel.ORDERID = (from code in queryXDoc.Descendants("ORDERID")
                                        select code.Value).FirstOrDefault();
                    rtnModel.ORDERDATE = (from code in queryXDoc.Descendants("ORDERDATE")
                                          select code.Value).FirstOrDefault();

                    rtnModel.ACCDATE = (from code in queryXDoc.Descendants("ACCDATE")
                                        select code.Value).FirstOrDefault();
                    rtnModel.AMOUNT = (from code in queryXDoc.Descendants("AMOUNT")
                                       select code.Value).FirstOrDefault();
                    rtnModel.STATUSCODE = (from code in queryXDoc.Descendants("STATUSCODE")
                                           select code.Value).FirstOrDefault();
                    rtnModel.STATUS = (from code in queryXDoc.Descendants("STATUS")
                                       select code.Value).FirstOrDefault();
                    rtnModel.REFUND = (from code in queryXDoc.Descendants("REFUND")
                                       select code.Value).FirstOrDefault();

                    rtnModel.SIGN = (from code in queryXDoc.Descendants("SIGN")
                                     select code.Value).FirstOrDefault();

                    queryRtn.BBCQueryAccountList.Add(rtnModel);//添加记录
                    #endregion
                }
            }
            return queryRtn;
        }
        #endregion
    }
}
