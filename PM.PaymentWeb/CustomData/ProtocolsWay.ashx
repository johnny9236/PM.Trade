<%@ WebHandler Language="C#" Class="ProtocolsWay" %>

using System;
using System.Web;

public class ProtocolsWay : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string ProtocolsWay = "[";
        int i = 0;
        try
        {
            foreach (PM.PaymentProtocolModel.ProtocolsWay m_type in Enum.GetValues(typeof(PM.PaymentProtocolModel.ProtocolsWay)))
            {
                //context.Response.Write(m_type.ToString() + "--" + PM.Utils.EnumUtil.EnumHelp.GetEnumDescription(m_type));
                if (i > 0)
                    ProtocolsWay += ",";
                ProtocolsWay += "{";
                ProtocolsWay += string.Format("\"protocolsWayID\":\"{0}\",\"protocolsWayName\":\"{1}\"", m_type.ToString(), PM.Utils.EnumUtil.EnumHelp.GetEnumDescription(m_type));
                ProtocolsWay += "}";
                i++;
            }
        }
        catch (Exception ex)
        {
            PM.Utils.CLogMgr.G_Instance.WriteErrorLog(PM.Utils.LogSeverity.error, ex.Source + ex.Message, "异常");
        }
        ProtocolsWay += "]";
        //ProtocolsWay = "[{\"protocolsWayID\":\"NetBank\",\"protocolsWayName\":\"银联\"},{\"protocolsWayID\":\"JHBOF\",\"protocolsWayName\":\"金华交行\"},{\"protocolsWayID\":\"JSABOC\",\"protocolsWayName\":\"嘉善农行\"}]";
        context.Response.Write(ProtocolsWay);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}