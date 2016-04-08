<%@ Application Language="C#" %>
<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        Init();
    }

    void Application_End(object sender, EventArgs e)
    {
        //在应用程序关闭时运行的代码

    }

    void Application_Error(object sender, EventArgs e)
    {
        Exception currentError = Server.GetLastError();
        PM.Utils.CLogMgr.G_Instance.WriteErrorLog(PM.Utils.LogSeverity.error, string.Format("--发生异常: {0}\n{1}", Request.Url.ToString(), currentError.StackTrace), currentError);
    }

    void Session_Start(object sender, EventArgs e)
    {
        //在新会话启动时运行的代码

    }

    void Session_End(object sender, EventArgs e)
    {
        //在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式 
        //设置为 StateServer 或 SQLServer，则不会引发该事件。

    }
    private void Init()
    {
        ////////////////////////////
        ConfigHelp.init();//配置初始化        
        PM.Utils.CLogMgr.G_Instance.WriteAppLog(PM.Utils.LogSeverity.info, this.ToString(), "配置初始化");
    }
       
</script>
