<%@ WebHandler Language="C#" Class="BusinessType" %>

using System;
using System.Web;

public class BusinessType : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string businessTypeStr = "[ ";//{\"businessTypeID\":\"Pay\",\"businessTypeName\":\"支付请求(B2B)\"},{\"businessTypeID\":\"PayB2C\",\"businessTypeName\":\"支付请求(B2C)\"}, {\"businessTypeID\":\"PayResponse\",\"businessTypeName\":\"支付响应(B2B)\"},{\"businessTypeID\":\"PayResponseB2C\",\"businessTypeName\":\"支付响应(B2C)\"},{\"businessTypeID\":\"Transfer\",\"businessTypeName\":\"转账请求\"},  {\"businessTypeID\":\"TransferResponse\",\"businessTypeName\":\"转账响应\"},  {\"businessTypeID\":\"TransferNotice\",\"businessTypeName\":\"转账结算请求\"},	  {\"businessTypeID\":\"TransferClearNotice\",\"businessTypeName\":\"转账结算响应\"},	  {\"businessTypeID\":\"PayerInfoQuery\",\"businessTypeName\":\"付款信息查询\"}, 	  {\"businessTypeID\":\"MerchantQuery\",\"businessTypeName\":\"商户订单支付查询\"},	  {\"businessTypeID\":\"MarketPayQuery\",\"businessTypeName\":\"市场订单支付查询\"},	  {\"businessTypeID\":\"MarketTransClearQuery\",\"businessTypeName\":\"市场订单结算查询\"},	  {\"businessTypeID\":\"BankStatement\",\"businessTypeName\":\"下载查询\"},	  {\"businessTypeID\":\"BankBatchStayPays\",\"businessTypeName\":\"批量代付\"},	  {\"businessTypeID\":\"OPKind\",\"businessTypeName\":\"获取操作类型\"}]";
        int i = 0;
        try
        {
            foreach (PM.PaymentProtocolModel.BusinessType m_type in Enum.GetValues(typeof(PM.PaymentProtocolModel.BusinessType)))
            {
                if (i > 0)
                    businessTypeStr += ",";
                businessTypeStr += "{";
                businessTypeStr += string.Format("\"businessTypeID\":\"{0}\",\"businessTypeName\":\"{1}\"", m_type.ToString(), PM.Utils.EnumUtil.EnumHelp.GetEnumDescription(m_type));
                businessTypeStr += "}";
                i++;
            }
        }
        catch (Exception ex)
        {
            PM.Utils.CLogMgr.G_Instance.WriteErrorLog(PM.Utils.LogSeverity.error, ex.Source + ex.Message, "异常");
        }
        businessTypeStr += "]";
        context.Response.Write(businessTypeStr);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}