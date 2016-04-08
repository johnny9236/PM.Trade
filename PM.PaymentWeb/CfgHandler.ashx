<%@ WebHandler Language="C#" Class="CfgHandler" %>

using System;
using System.Web;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using PM.Utils;
using PM.PaymentProtocolModel;


public class CfgHandler : IHttpHandler
{
    SysConfigModel cfgModel = HttpContext.Current.Application["cfg"] as SysConfigModel;
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.ContentType = "text/plain";
        string rtnStr = string.Empty;
        var act = HttpContext.Current.Request["action"];
        if (!string.IsNullOrWhiteSpace(act))
        {
            if (act.ToLower() == "list")
            {
                rtnStr = GetList();
            }
            else if (act.ToLower() == "save")
            {
                rtnStr = Save().ToString();
            }
            context.Response.Write(rtnStr);

        }
    }
    #region
    /// <summary>
    /// 获取列表
    /// </summary>
    /// <returns></returns>
    private string GetList()
    {
        string rtn = string.Empty;
        try
        {
            var rtnJson = new JsonCfgInfo() { total = cfgModel.CfgInfoList.Count, rows = cfgModel.CfgInfoList };

            rtn = new JavaScriptSerializer().Serialize(rtnJson);
        }
        catch (Exception ex)
        {
            CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, ex.Message, ex);
        }
        return rtn;
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <returns></returns>
    private bool Save()
    {
        try
        {
            var insertInfo = HttpContext.Current.Request["inserted"];
            var delInfo = HttpContext.Current.Request["deleted"];
            var upInfo = HttpContext.Current.Request["updated"];

            if (!string.IsNullOrWhiteSpace(delInfo))
            {
                var delLst = new JavaScriptSerializer().Deserialize<List<CfgInfo>>(delInfo);
                delLst.ForEach(p => cfgModel.CfgInfoList.RemoveAt(cfgModel.CfgInfoList.FindIndex(h => h.ID == p.ID)));
            }
            if (!string.IsNullOrWhiteSpace(upInfo))
            {
                var upLst = new JavaScriptSerializer().Deserialize<List<CfgInfo>>(upInfo);
                upLst.ForEach(p => cfgModel.CfgInfoList.RemoveAt(cfgModel.CfgInfoList.FindIndex(h => h.ID == p.ID)));

                cfgModel.CfgInfoList.AddRange(upLst);
            }
            if (!string.IsNullOrWhiteSpace(insertInfo))
            {
                var insertLst = new JavaScriptSerializer().Deserialize<List<CfgInfo>>(insertInfo);
                cfgModel.CfgInfoList.AddRange(insertLst);
            }
            if (!string.IsNullOrWhiteSpace(insertInfo) || !string.IsNullOrWhiteSpace(upInfo) || !string.IsNullOrWhiteSpace(delInfo))
            {
                return ConfigHelp.SaveCfg(cfgModel);
            }
        }
        catch (Exception ex)
        {
            CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, ex.Message, ex);
            return false;
        }
        return true;
    }
    #endregion

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}