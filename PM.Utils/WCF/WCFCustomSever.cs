using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Configuration;

namespace PM.Utils.WCF
{
    /// <summary>
    /// 自定义WCF服务
    /// </summary>
    public class WCFCustomSever : ServiceHost
    {
        public WCFCustomSever(Type t)
            : base(t)
        {
        }
        /// <summary>
        /// 从自定义配置文件加载服务说明信息
        /// </summary>
        protected override void ApplyConfiguration()
        {
            ////get custom config file name by our rule: config file name = ServiceType.Name
            //var myConfigFileName = this.Description.ServiceType.FullName; 
            string physicalPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string configFileName = ConfigHelper.GetConfigString("WcfServiceConfigFile");//System.Configuration.ConfigurationManager.AppSettings["ServiceConfigFile"];
            if (string.IsNullOrEmpty(configFileName))
            {
                configFileName = "Config\\Service.config";
            }
            string filePath = System.IO.Path.Combine(physicalPath, configFileName);
            if (!System.IO.File.Exists(filePath))
            {
                base.ApplyConfiguration();
                return;
            }
            var configFileMap = new System.Configuration.ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = filePath;
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            var serviceModel = System.ServiceModel.Configuration.ServiceModelSectionGroup.GetSectionGroup(config);
            bool loaded = false;
            foreach (System.ServiceModel.Configuration.ServiceElement se in serviceModel.Services.Services)
            {
                if (!loaded)
                {
                    if (se.Name == this.Description.ConfigurationName)
                    {
                        base.LoadConfigurationSection(se);
                        loaded = true;
                    }
                }
            }
            if (!loaded)
                throw new ArgumentException("ServiceElement doesn't exist");
        }
    }
}
