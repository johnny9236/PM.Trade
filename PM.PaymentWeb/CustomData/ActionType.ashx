<%@ WebHandler Language="C#" Class="ActionType" %>

using System;
using System.Web;

public class ActionType : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
       // string businessTypeStr = "[ {\"actionTypeID\":\"Request\",\"actionTypeName\":\"请求\"}, {\"actionTypeID\":\"Response\",\"actionTypeName\":\"响应\"}]";

        string businessTypeStr = "[";
        int i = 0;
        try
        {
            foreach (PM.PaymentProtocolModel.ActionType m_type in Enum.GetValues(typeof(PM.PaymentProtocolModel.ActionType)))
            {
                //context.Response.Write(m_type.ToString() + "--" + PM.Utils.EnumUtil.EnumHelp.GetEnumDescription(m_type));
                if (i > 0)
                    businessTypeStr += ",";
                businessTypeStr += "{";
                businessTypeStr += string.Format("\"actionTypeID\":\"{0}\",\"actionTypeName\":\"{1}\"", m_type.ToString(), PM.Utils.EnumUtil.EnumHelp.GetEnumDescription(m_type));
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
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}