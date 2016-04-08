/*----------------------------------------------------
 * 说明：黄石保证金交易查询 查询协议实现
 *       
 * 作者：梁亮
 * 时间：2013-8-8
 * 
 -----------------------------------------------------*/

#region 引用命名空间
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PM.PaymentProtocolModel.BankCommModel.HuangShi;
using PM.PaymentProtocolModel;
using PM.Utils.Log;
using PM.Utils.SocektUtils;
using System.Xml;
using System.Xml.Linq;

#endregion

namespace PM.HuangshiCCBPtlBiz
{
    /// <summary>
    /// 查询协议实现 部分类
    /// </summary>
    public partial class HuangShiCCBCommProtocols
    {
        #region 获取黄石保证金交易明细

        /// <summary>
        /// 查询黄石保证金交易明细
        /// 发送请求报文 并获取返回结果
        /// </summary>
        /// <param name="modDepositQuery">请求实体部分字段实体</param>
        /// <param name="cfgInfo">配置文件信息</param>
        /// <returns>返回交易明细</returns>
        private HuangShiDepositResponseModel GetHuangshiDepositQuery(HuangShiDepositQueryModel modDepositQuery, CfgInfo cfgInfo)
        {
            HuangShiDepositResponseModel modResponse = null;
            
            if (modDepositQuery==null||cfgInfo==null)
            {
                LogTxt.WriteEntry(string.Format("查询黄石保证金交易明细,{0}", "传入参数为null"), "HuangShiCCB保证金明细查询");
                return null;
            }
            //封装请求实体
            HuangShiDepositRequestModel modRequest = GetHuangShiDepositRequestModel(modDepositQuery);
            //将请求实体转成xml串
            string sendStr = ConvertRequestModelToXml(modRequest);
            LogTxt.WriteEntry(string.Format("查询账户明细发送信息内容{0}", sendStr), "HuangShiCCB保证金明细查询");

            //发送请求报文并接收返报文
            int sendPort;
            int.TryParse(cfgInfo.Port, out sendPort);
            string reciveStr = SocketClient.SendToServ(cfgInfo.IP, sendPort, sendStr, Encoding.GetEncoding("GB2312"));
            LogTxt.WriteEntry(string.Format("查询账户明细发送信息内容{0}", reciveStr), "HuangShiCCB保证金明细查询");
                
            //将返回报文转成实体
            modResponse = new HuangShiDepositResponseModel();
            string strNoticeMsg = string.Empty;
            modResponse = ConvertRepositResponseXmlToModel(reciveStr, out strNoticeMsg);

            return modResponse;
        }
        #endregion

        #region 封装请求报文实体

        /// <summary>
        /// 封装请求报文实体
        /// </summary>
        /// <param name="modDepositQuery">请求实体部分字段实体</param>
        /// <returns>请求报文实体</returns>
        private HuangShiDepositRequestModel GetHuangShiDepositRequestModel(HuangShiDepositQueryModel modDepositQuery)
        {
            HuangShiDepositRequestModel modRequest = new HuangShiDepositRequestModel();
            modRequest.REQUEST_SN = modDepositQuery.OrderNo;
            modRequest.CUST_ID = modDepositQuery.CUST_ON;
            modRequest.USER_ID = modDepositQuery.OprationerID;
            modRequest.PASSWORD = modDepositQuery.PASSWORD;
            modRequest.TX_CODE = modDepositQuery.TX_CODE;
            modRequest.LANGUAGE = modDepositQuery.LANGUAGE;

            modRequest.CUST_ON = modDepositQuery.CUST_ON;
            modRequest.ACCOUNT = modDepositQuery.ACCOUNT;
            modRequest.START = modDepositQuery.START;
            modRequest.END = modDepositQuery.END;
            modRequest.PAGE = modDepositQuery.PAGE;
            modRequest.OTHER_ACCOUNT = modDepositQuery.OTHER_ACCOUNT;
            modRequest.MONEY = modDepositQuery.MONEY;
            modRequest.ORDER = modDepositQuery.OrderNo;
            modRequest.INDEX_STRING = modDepositQuery.INDEX_STRING;

            return modRequest;
        }

        #endregion

        #region 将请求实体转成xml串
        /// <summary>
        /// 将请求报文实体转成xml串
        /// </summary>
        /// <param name="modRequest">请求报文实体</param>
        /// <returns>请求报文xml串</returns>
        private string ConvertRequestModelToXml(HuangShiDepositRequestModel modRequest)
        {
            string strRequestXml = string.Empty;
            try
            {
                #region 示例文件
                //<?xml version="1.0" encoding="GB2312" standalone="yes" ?>
                //<TX>
                //  <REQUEST_SN>请求序列号</REQUEST_SN>
                //  <CUST_ID>商户代码</CUST_ID>
                //  <USER_ID>操作员号</USER_ID>
                //  <PASSWORD>操作员号密码</PASSWORD>
                //  <TX_CODE>5W1011</TX_CODE>
                //  <LANGUAGE>CN</LANGUAGE>
                //  <TX_INFO>
                //    <CUST_ON>企业网银签约客户号</CUST_ON>
                //    <ACCOUNT>商户保证金结算账户号</ACCOUNT>
                //    <OTHER_ACCOUNT>对方账户账号</OTHER_ACCOUNT>
                //    <START>起始日期</START>
                //    <END>截止日期</END>
                //    <MONEY>金额</MONEY>
                //    <PAGE>当前页次</PAGE>
                //    <INDEX_STRING>定位串</INDEX_STRING>
                //    <ORDER>摘要</ORDER>
                //  </TX_INFO>
                //</TX>
                #endregion
                
                StringBuilder sbRequestXml = new StringBuilder();
                sbRequestXml.Append("<?xml version=\"1.0\" encoding=\"GB2312\" standalone=\"yes\" ?>");
                sbRequestXml.Append("<TX>");
                sbRequestXml.Append(string.Format("<REQUEST_SN>{0}</REQUEST_SN>",modRequest.REQUEST_SN));
                sbRequestXml.Append(string.Format("<CUST_ID>{0}</CUST_ID>", modRequest.CUST_ID));
                sbRequestXml.Append(string.Format("<USER_ID>{0}</USER_ID>", modRequest.USER_ID));
                sbRequestXml.Append(string.Format("<PASSWORD>{0}</PASSWORD>", modRequest.PASSWORD));
                sbRequestXml.Append(string.Format("<TX_CODE>{0}</TX_CODE>", modRequest.TX_CODE));
                sbRequestXml.Append(string.Format("<LANGUAGE>{0}</LANGUAGE>", modRequest.LANGUAGE));
                sbRequestXml.Append("<TX_INFO>");
                sbRequestXml.Append(string.Format("<CUST_ON>{0}</CUST_ON>", modRequest.CUST_ON));
                sbRequestXml.Append(string.Format("<ACCOUNT>{0}</ACCOUNT>", modRequest.ACCOUNT));
                sbRequestXml.Append(string.Format("<OTHER_ACCOUNT>{0}</OTHER_ACCOUNT>", modRequest.OTHER_ACCOUNT));
                sbRequestXml.Append(string.Format("<START>{0}</START>", modRequest.START));
                sbRequestXml.Append(string.Format("<END>{0}</END>", modRequest.END));
                sbRequestXml.Append(string.Format("<MONEY>{0}</MONEY>", modRequest.MONEY));
                sbRequestXml.Append(string.Format("<PAGE>{0}</PAGE>", modRequest.PAGE));
                sbRequestXml.Append(string.Format("<INDEX_STRING>{0}</INDEX_STRING>", modRequest.INDEX_STRING));
                sbRequestXml.Append(string.Format("<ORDER>{0}</ORDER>", modRequest.ORDER));
                sbRequestXml.Append("</TX_INFO>");
                sbRequestXml.Append("</TX>");

                strRequestXml = sbRequestXml.ToString();
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(string.Format("将请求报文实体转成xml串{0}失败", ex.Message), "HuangShiCCB保证金明细查询");
                throw ex;
            }
            return strRequestXml;
        }
        #endregion

        #region 将返回报文xml串转成实体
        /// <summary>
        /// 将返回报文xml串转成实体
        /// </summary>
        /// <param name="strResponseXml">返回报文xml串</param>
        /// <returns>交易明细实体</returns>
        public HuangShiDepositResponseModel ConvertRepositResponseXmlToModel(string strResponseXml,out string strNoticeMsg)
        {
            HuangShiDepositResponseModel modResponse = null;
            strNoticeMsg = "";
            try
            {
                #region 示例文件
                //<?xml version="1.0" encoding="GB2312" standalone="yes" ?> 
                //  <TX>
                //   <REQUEST_SN>请求序列码</REQUEST_SN> 
                //   <CUST_ID>商户号</CUST_ID> 
                //   <TX_CODE>5W1011</TX_CODE> 
                //   <RETURN_CODE>响应码</RETURN_CODE> 
                //   <RETURN_MSG>响应信息</RETURN_MSG>
                //   <LANGUAGE>CN</LANGUAGE>
                //  <TX_INFO>
                //   <CUR_PAGE>当前页次</CUR_PAGE> 
                //   <PAGE_COUNT>总页次</PAGE_COUNT> 
                //    <INDEX_STRING>定位串</INDEX_STRING>
                //   <LIST>
                //    <TRAN_DATE>记账日期</TRAN_DATE> 
                //    <TRAN_TIME>记账时间</TRAN_TIME> 
                //    <TRAN_SQ>交易流水号</TRAN_SQ> 
                //    <ORDER>订单号</ORDER> 
                //    <ACCOUNT>商户保证金结算账户号</ACCOUNT> 
                //    <MONEY>金额</MONEY> 
                //    <OTHER_ACCOUNT>对方账户账号</OTHER_ACCOUNT>
                //    <OTHER_ACCOUNT_NAME>对方账户名称</OTHER_ACCOUNT_NAME>
                //   </LIST>
                //   <NOTICE>提示信息</NOTICE>
                //  </TX_INFO>
                //  </TX>
                StringBuilder sbResponse = new StringBuilder();
                sbResponse.Append("<?xml version=\"1.0\" encoding=\"GB2312\" standalone=\"yes\" ?>");
                sbResponse.Append("<TX>");
                sbResponse.Append(" <REQUEST_SN>请求序列码</REQUEST_SN>");
                sbResponse.Append(" <CUST_ID>商户号</CUST_ID>");
                sbResponse.Append(" <TX_CODE>5W1011</TX_CODE>");
                sbResponse.Append(" <RETURN_CODE>响应码</RETURN_CODE>");
                sbResponse.Append(" <RETURN_MSG>响应信息</RETURN_MSG>");
                sbResponse.Append(" <LANGUAGE>CN</LANGUAGE>");
                sbResponse.Append(" <TX_INFO>");
                sbResponse.Append("     <CUR_PAGE>当前页次</CUR_PAGE>");
                sbResponse.Append("     <TPAGE>总页次</TPAGE>");
                sbResponse.Append("     <INDEX_STRING>定位串</INDEX_STRING>");
                sbResponse.Append("     <LIST>");
                sbResponse.Append("         <TRAN_DATE>记账日期</TRAN_DATE>");
                sbResponse.Append("         <TRAN_TIME>记账时间</TRAN_TIME>");
                sbResponse.Append("         <TRAN_SQ>交易流水号</TRAN_SQ>");
                sbResponse.Append("         <ORDER>订单号</ORDER>");
                sbResponse.Append("         <ACCOUNT>商户保证金结算账户号</ACCOUNT>");
                sbResponse.Append("         <MONEY>金额</MONEY>");
                sbResponse.Append("         <OTHER_ACCOUNT>对方账户账号</OTHER_ACCOUNT>");
                sbResponse.Append("         <OTHER_ACCOUNT_NAME>对方账户名称</OTHER_ACCOUNT_NAME>");
                sbResponse.Append("     </LIST>");
                sbResponse.Append("     <NOTICE>提示信息</NOTICE>");
                sbResponse.Append(" </TX_INFO>");
                sbResponse.Append("</TX>");
                strResponseXml = sbResponse.ToString();
                #endregion
                
                if (string.IsNullOrEmpty(strResponseXml))
                {
                    strNoticeMsg = "返回结果为空字符串";
                    LogTxt.WriteEntry(string.Format("将返回报文xml串转成实体{0}失败", "返回结果为空字符串"), "HuangShiCCB保证金明细查询");
                }
                else
                {
                    XElement xel = XElement.Parse(strResponseXml);
                    string strValues = string.Empty;
                    modResponse.REQUEST_SN = xel.Element("REQUEST_SN").Value == null ? "" : xel.Element("REQUEST_SN").Value.ToString();
                    modResponse.CUST_ID = xel.Element("CUST_ID").Value == null ? "" : xel.Element("CUST_ID").Value.ToString();
                    modResponse.TX_CODE = xel.Element("TX_CODE").Value == null ? "" : xel.Element("TX_CODE").Value.ToString();
                    modResponse.RETURN_CODE = xel.Element("RETURN_CODE").Value == null ? "" : xel.Element("RETURN_CODE").Value.ToString();
                    modResponse.RETURN_MSG = xel.Element("RETURN_MSG").Value == null ? "" : xel.Element("RETURN_MSG").Value.ToString();
                    modResponse.LANGUAGE = xel.Element("LANGUAGE").Value == null ? "" : xel.Element("LANGUAGE").Value.ToString();

                    var responseXmlInfo = from response in xel.Descendants("TX_INFO")
                                          select response;
                    if (responseXmlInfo != null)
                    {
                        foreach (var xe in responseXmlInfo)
                        {
                            int cur_page;
                            int.TryParse(xe.Element("CUR_PAGE").Value == null ? "" : xe.Element("CUR_PAGE").Value.ToString(), out cur_page);
                            modResponse.CUR_PAG = cur_page;

                            int pagecount;
                            int.TryParse(xe.Element("TPAGE").Value == null ? "" : xe.Element("TPAGE").Value.ToString(), out pagecount);
                            modResponse.TPAGE = pagecount;
                            modResponse.INDEX_STRING = xe.Element("INDEX_STRING").Value == null ? "" : xe.Element("INDEX_STRING").Value.ToString();

                            var responseXmlList = from responselist in xe.Descendants("LIST")
                                                  select responselist;
                            if (responseXmlList != null)
                            {
                                List<HuangShiDepositResponseListModel> listResponse = new List<HuangShiDepositResponseListModel>();
                                HuangShiDepositResponseListModel modResponseList = null;
                                foreach (var xelist in responseXmlList)
                                {
                                    modResponseList = new HuangShiDepositResponseListModel();
                                    modResponseList.TRAN_DATE = xelist.Element("TRAN_DATE").Value == null ? "" : xelist.Element("TRAN_DATE").Value.ToString();
                                    modResponseList.TRAN_TIME = xelist.Element("TRAN_TIME").Value == null ? "" : xelist.Element("TRAN_TIME").Value.ToString();
                                    modResponseList.TRAN_SQ = xelist.Element("TRAN_SQ").Value == null ? "" : xelist.Element("TRAN_SQ").Value.ToString();
                                    modResponseList.ORDER = xelist.Element("ORDER").Value == null ? "" : xelist.Element("ORDER").Value.ToString();
                                    modResponseList.ACCOUNT = xelist.Element("ACCOUNT").Value == null ? "" : xelist.Element("ACCOUNT").Value.ToString();
                                    decimal money;
                                    decimal.TryParse(xelist.Element("MONEY").Value == null ? "" : xelist.Element("MONEY").Value.ToString(),out money);
                                    modResponseList.MONEY = money;
                                    modResponseList.OTHER_ACCOUNT = xelist.Element("OTHER_ACCOUNT").Value == null ? "" : xelist.Element("OTHER_ACCOUNT").Value.ToString();
                                    modResponseList.OTHER_ACCOUNT_NAME = xelist.Element("OTHER_ACCOUNT_NAME").Value == null ? "" : xelist.Element("OTHER_ACCOUNT_NAME").Value.ToString();
                                    listResponse.Add(modResponseList);
                                }
                                modResponse.ResponseListModel = listResponse;
                            }

                            modResponse.NOTICE = xe.Element("NOTICE").Value == null ? "" : xe.Element("NOTICE").Value.ToString();
                            strNoticeMsg += modResponse.NOTICE.Trim();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(string.Format("将返回报文xml串转成实体{0}失败", ex.Message), "HuangShiCCB保证金明细查询");
                throw ex;
            }
            return modResponse;
        }
        #endregion
    }
}
