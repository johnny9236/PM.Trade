<%@ WebHandler Language="C#" Class="OprationType" %>

using System;
using System.Web;

public class OprationType : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        //string OprationTypeStr = "[{\"oprationTypeID\":\"Pay\",\"oprationTypeName\":\"支付\"},{\"oprationTypeID\":\"Search\",\"oprationTypeName\":\"查询\"},{\"oprationTypeID\":\"Other\",\"oprationTypeName\":\"其他\"}]";
        string OprationTypeStr = "[";
        int i = 0;
        try
        {
            foreach (PM.PaymentProtocolModel.OprationType m_type in Enum.GetValues(typeof(PM.PaymentProtocolModel.OprationType)))
            {
                      if (i > 0)
                          OprationTypeStr += ",";
                      OprationTypeStr += "{";
                      OprationTypeStr += string.Format("\"oprationTypeID\":\"{0}\",\"oprationTypeName\":\"{1}\"", m_type.ToString(), PM.Utils.EnumUtil.EnumHelp.GetEnumDescription(m_type));
                      OprationTypeStr += "}";
                i++;
            }
        }
        catch (Exception ex)
        {
            PM.Utils.CLogMgr.G_Instance.WriteErrorLog(PM.Utils.LogSeverity.error, ex.Source + ex.Message, "异常");
        }
        OprationTypeStr += "]";

        context.Response.Write(OprationTypeStr);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}