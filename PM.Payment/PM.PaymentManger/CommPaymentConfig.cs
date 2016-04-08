using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Log;
using PM.PaymentProtocolModel;

namespace PM.PaymentManger
{
    /// <summary>
    /// 非支付通用配置获取
    /// </summary>
    public class CommPaymentConfig
    {
        /// <summary>
        /// 获取某个配置对象
        /// </summary>
        /// <param name="objModel"></param>
        /// <param name="sysConfigModel"></param>
        /// <returns></returns>
        public static CfgInfo GetCommConfig(CommunicationBase objModel, SysConfigModel sysConfigModel)
        {
            CfgInfo cfg = null;
            try
            {
                cfg = sysConfigModel.CfgInfoList.FirstOrDefault(p => p.BusinessNo.Trim().ToLower() == objModel.BusinessFunNo.Trim().ToLower());
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(ex.Message, "支付相关信息");
            }
            return cfg;
        }
        /// <summary>
        /// 获取某个配置对象
        /// </summary>
        /// <param name="businessNo">功能号</param>
        /// <param name="sysConfigModel">配置对象</param>
        /// <returns></returns>
        public static CfgInfo GetCommConfig(string businessNo, SysConfigModel sysConfigModel)
        {
            CfgInfo cfg = null;
            try
            {
                cfg = sysConfigModel.CfgInfoList.FirstOrDefault(p => p.BusinessNo.Trim().ToLower() == businessNo.Trim().ToLower());
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(ex.Message, "支付相关信息");
            }
            return cfg;
        }

        /// <summary>
        ///获取全局配置对象
        /// </summary>   
        /// <returns></returns>
        public static SysConfigModel GetSysConfig()
        {
            var cfgPath = string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, @"config\CFG.xml");
            var sysConfigModel = new SysConfigModel();
            sysConfigModel = sysConfigModel.xmlDeserialize(cfgPath);
            return sysConfigModel;
        }

        /// <summary>
        ///获取某个配置对象
        /// </summary>
        /// <param name="objModel"></param>
        /// <returns></returns>
        public static CfgInfo GetCommConfig(CommunicationBase objModel)
        {
            return GetCommConfig(objModel, GetSysConfig());
        }
        /// <summary>
        ///获取某个配置对象
        /// </summary>
        /// <param name="businessNo">功能号</param>
        /// <returns></returns>
        public static CfgInfo GetCommConfig(string businessNo)
        {
            return GetCommConfig(businessNo, GetSysConfig());
        }
    }
}
