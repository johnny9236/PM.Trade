using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using System.IO;
using PM.Utils;
using PM.PaymentProtocolModel;

/// <summary>
///ConfigHelp 的摘要说明
/// </summary>
public class ConfigHelp
{
    public ConfigHelp()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    public static string filePath = System.Web.HttpContext.Current.Server.MapPath("~") + @"\CFG.xml";
    /// <summary>
    /// 初始数据源
    /// </summary>
    public static void init()
    {
        SysConfigModel cfgModel = new SysConfigModel();
        try
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                //cfgModel = new CfgModel();
                cfgModel.CfgInfoList = new List<CfgInfo>();
                cfgModel.CfgName = "分发地址配置";
                cfgModel.xmlSeria(filePath);
            }
            else
            {
                if (cfgModel.CfgInfoList == null)
                {
                    cfgModel.CfgInfoList = new List<CfgInfo>();
                }
                cfgModel = cfgModel.xmlDeserialize(filePath);
            }
            if (null != cfgModel)
            {
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application["cfg"] = cfgModel;
                HttpContext.Current.Application.UnLock();
            }
            else
            {
                CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, "初始化失败", "异常");
            }
        }
        catch (Exception ex)
        {
            CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, ex.Message, ex);
        }
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="cfgModel"></param>
    /// <returns></returns>
    public static bool SaveCfg(SysConfigModel cfgModel)
    {
        bool result = false;
        try
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            if (null == cfgModel)
            {
                cfgModel = new SysConfigModel();
                cfgModel.CfgInfoList = new List<CfgInfo>();
                cfgModel.CfgName = "分发地址配置";
            }
            cfgModel.xmlSeria(filePath);
            if (null != cfgModel)
            {
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application["cfg"] = cfgModel;
                HttpContext.Current.Application.UnLock();
                result = true;
            }
            else
            {
                CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, "保存失败", "异常");
            }
        }
        catch (Exception ex)
        {
            CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, ex.Message, ex);
        }
        return result;
    }
}

 