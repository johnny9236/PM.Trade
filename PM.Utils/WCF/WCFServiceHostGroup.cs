using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Configuration;

namespace PM.Utils.WCF
{
    /// <summary>
    /// 多服务统一管理
    /// </summary>
    public class WCFServiceHostGroup
    {
        static List<ServiceHost> _hosts = new List<ServiceHost>();
        /// <summary>
        /// 宿主 打开服务
        /// </summary>
        /// <param name="t"></param>
        private static void OpenHost(Type t)
        {
            try
            {
                WCFCustomSever host = new WCFCustomSever(t);
                Type svType = host.Description.ServiceType;//ty
                host.Open();
                _hosts.Add(host);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 启动所有服务
        /// </summary>
        public static void StartAllConfiguredServices()
        {
            //get custom config file name by our rule: config file name = ServiceType.Name
            // var myConfigFileName = this.Description.ServiceType.FullName;
            var myConfigFileName = @"config\WCFSetting";
            //get config file path
            string dir = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string myConfigFilePath = System.IO.Path.Combine(dir, myConfigFileName + ".config");
            var configFileMap = new System.Configuration.ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = myConfigFilePath;
            var conf = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            // ServiceModelSectionGroup svcmod =   (ServiceModelSectionGroup)conf.GetSectionGroup("system.serviceModel");
            var svcmod = System.ServiceModel.Configuration.ServiceModelSectionGroup.GetSectionGroup(conf);
            foreach (ServiceElement el in svcmod.Services.Services)
            {
                string Namespace = el.Name.Substring(0, el.Name.LastIndexOf("."));
                Type svcType = Type.GetType(el.Name + "," + Namespace);
                if (svcType == null) throw new Exception("Invalid Service Type " + el.Name + " in configuration file.");
                OpenHost(svcType);
            }
        }
        /// <summary>
        /// 关闭所有服务
        /// </summary>
        public static void CloseAllServices()
        {
            foreach (ServiceHost hst in _hosts)
            {
                hst.Close();
            }
        }
    }
}
