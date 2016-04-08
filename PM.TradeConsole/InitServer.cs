using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils;
using PM.TradeConsole.Sever;

namespace PM.TradeConsole
{
    /// <summary>
    /// 初始化各种服务
    /// </summary>
    public class InitServer
    {
        /// <summary>
        /// 定时、监听等 系统服务初始化
        /// </summary>
        public static void Init()
        {
            #region 是否启动定时任务
            if (ConfigHelper.GetConfigBool("HaveJob"))
            {
                TimerService.TimerServiceStart();
            }
            #endregion

            #region  是否启动socket监听
            if (ConfigHelper.GetConfigBool("HaveListen"))
            {
                SocketService.SocketServiceStart();
            }
            #endregion
            if (ConfigHelper.GetConfigBool("HaveWcf"))
            {
                //wcf服务启动
                WCFServer.WCFServerStart();
            }
            BizInit.Init();//初始化证书相关

        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public static void InitDispose()
        {
            TimerService.TimerServiceStop();
            SocketService.SocketServiceStop();
        }
    }
}
