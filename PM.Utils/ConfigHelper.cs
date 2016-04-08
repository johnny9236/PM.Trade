using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;

namespace PM.Utils
{
    /// <summary>
    /// <para>　</para>
    /// 类名：常用工具类——Web.Config操作类 
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key">AppSetting中关键字KEY</param>
        /// <returns>AppSettings中的配置字符串信息</returns>
        public static string GetConfigString(string key)
        {
            return (string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]) == true ? string.Empty : ConfigurationManager.AppSettings[key]);
        }
        /// <summary>
        /// 得到Connection中配置字符串信息
        /// </summary>
        /// <param name="key">Connection中name的值</param>
        /// <returns>Connection中name的值</returns>
        public static string GetConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ToString();
        }
        /// <summary>
        /// 得到AppSettings中的配置Bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetConfigBool(string key)
        {
            bool result = false;
            string configString = ConfigHelper.GetConfigString(key);
            while (configString != null && string.Empty != configString)
            {
                try
                {
                    result = bool.Parse(configString);
                    break;
                }
                catch (FormatException)
                {
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 得到AppSettings中的配置Decimal信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal GetConfigDecimal(string key)
        {
            decimal result = 0m;
            string configString = ConfigHelper.GetConfigString(key);
            if (true)
            {
                if (configString == null)
                {
                    return result;
                }
            }
            if (string.Empty != configString)
            {
                try
                {
                    result = decimal.Parse(configString);
                }
                catch (FormatException)
                {
                }
            }
            return result;
        }
        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetConfigInt(string key)
        {
            int result = 0;
            string configString = ConfigHelper.GetConfigString(key);
            if (configString != null)
            {
                if (string.Empty != configString)
                {
                    try
                    {
                        result = int.Parse(configString);
                    }
                    catch (FormatException)
                    {
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取自定义配置内容
        /// 路径通过config文件   <add key="CustomCfgPath" value=""/>
        /// </summary>
        /// <param name="setctionKey">配置节名</param>
        /// <param name="conditionAttributeValue">key名</param>
        /// <returns></returns>
        public static string GetCustomCfg(string setctionKey, string conditionAttributeValue)
        {
            string physicalPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string configFileName = ConfigHelper.GetConfigString("CustomCfgPath");
            if (string.IsNullOrEmpty(configFileName))
                throw new KeyNotFoundException();
            string filePath = System.IO.Path.Combine(physicalPath, configFileName);
            return GetCustomCfg(filePath, setctionKey, "add", "key", conditionAttributeValue, "value");
        }

        /// <summary>
        /// 获取自定义配置文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="setctionKey"></param>
        /// <param name="conditionAttributeValue"></param>
        /// <returns></returns>
        public static string GetCustomCfg(string filePath, string setctionKey, string conditionAttributeValue)
        {
            return GetCustomCfg(filePath, setctionKey, "add", "key", conditionAttributeValue, "value");
        }
        /// <summary>
        /// <summary>
        /// 自定义配置信息（自定义配置文件）
        /// </summary>
        /// <param name="filePath">自定义配置文件路径</param>
        /// <param name="setctionKey">节点key</param>
        /// <param name="elemnet">元素</param>
        /// <param name="conditionAttributeKey">条件属性key</param>
        /// <param name="conditionAttributeValue">条件属性值</param>
        /// <param name="attributeKey">待获取属性key</param>
        /// <returns>待获取属性值</returns>
        public static string GetCustomCfg(string filePath, string setctionKey, string elemnet, string conditionAttributeKey, string conditionAttributeValue, string attributeKey)
        {
            string cfgValue = string.Empty;
            try
            {
                XDocument adList = XDocument.Load(filePath); 

                var ad = (from a in adList.Descendants(setctionKey).Elements(elemnet)
                          where a.Attribute(conditionAttributeKey).Value == conditionAttributeValue
                          select new
                          {
                              value = a.Attribute(attributeKey).Value,
                          }).FirstOrDefault();
                if (null != ad)
                    cfgValue = ad.value;
            }
            catch (Exception ex)
            {
                CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, ex.Message, ex);
            }
            return cfgValue;
        }
    }
}
