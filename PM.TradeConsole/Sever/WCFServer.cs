using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.WCF;

namespace PM.TradeConsole.Sever
{
    /// <summary>
    /// wcf服务
    /// </summary>
    public class WCFServer
    {
        /// <summary>
        /// Wcf服务开启
        /// </summary>
        public static void WCFServerStart()
        {
            try
            {
                WCFServiceHostGroup.StartAllConfiguredServices();
                Console.WriteLine("-------------------------Wcf服务开启-----------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wcf服务异常：" + ex.Message);
            }
        }
    }
}
