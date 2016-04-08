using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentManger;
using PM.Utils;

namespace PM.TradeConsole
{
    /// <summary>
    /// 业务初始化(目前是支付证书初始化)
    /// </summary>
    public class BizInit
    {
        /// <summary>
        ///  初始化
        /// </summary>
        public static void Init()
        {
            if (NetbankInit())//网银支付初始化
                CLogMgr.G_Instance.WriteAppLog(PM.Utils.LogSeverity.info, "支付初始化", "成功");
            else
                CLogMgr.G_Instance.WriteAppLog(PM.Utils.LogSeverity.info, "支付初始化", "失败");
        }
        /// <summary>
        /// 网银支付初始化
        /// </summary>
        private static bool NetbankInit()
        {
            String configPath = ConfigHelper.GetCustomCfg("NetBank", "payment.config.path");
            return (Manager.Init(configPath));
        }






    }
}
