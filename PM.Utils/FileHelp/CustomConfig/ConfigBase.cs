using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Reflection;


//调用方法 
//  public class ClientConfig : ConfigBase<NameValueCollection>
//        {
//            public ClientConfig(WeiboType weiboType)
//                : this(weiboType, null)
//            { } 
//            public ClientConfig(WeiboType weiboType, string configFile)
//                : base(string.Format("WeiboClientSectionGroup/{0}Section", weiboType), configFile)
//           {          } 
//           public string AccessToken
//           {
//               get { return Section["AccessToken"]; }
//           } 
//           public string AccessTokenSecret
//           {
//               get { return Section["AccessTokenSecret"]; }
//           } 
//           public ResultFormat ResultFormat
//           {
//               get { return (ResultFormat)Enum.Parse(typeof(ResultFormat), Section["ResultFormat"]); }
//           }
//       }
namespace PM.Utils.FileHelp.CustomConfig
{
    /// <summary>
    /// 自定义配置基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ConfigBase<T> where T : class
    {
        protected ConfigBase(string sectionName, string configFile)
        {
            if (string.IsNullOrEmpty(configFile))
            {
                Section = (T)ConfigurationManager.GetSection(sectionName);
            }
            else
            {
                var configMap = new ExeConfigurationFileMap { ExeConfigFilename = configFile };
                var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                var configurationSection = config.GetSection(sectionName);
                if (configurationSection != null)
                {
                    Section = LoadSection(configurationSection.SectionInformation);
                }
            }
        }
        /// <summary>
        /// 加载配置节
        /// </summary>
        /// <typeparam name="TReturn">返回值</typeparam>
        /// <param name="information">配置节信息</param>
        /// <returns></returns>
        private static TReturn LoadSection<TReturn>(SectionInformation information) where TReturn : class
        {
            string[] strs = information.Type.Split(",".ToCharArray(), 2);
            var handler = (IConfigurationSectionHandler)Assembly.Load(strs[1]).CreateInstance(strs[0]);
            var doc = new XmlDocument();
            doc.LoadXml(information.GetRawXml());
            if (handler != null)
                return (TReturn)handler.Create(null, null, doc.ChildNodes[0]);
            return null;
        }

        protected T LoadSection(SectionInformation information)
        {
            return LoadSection<T>(information);
        }

        protected T Section { get; private set; }
    }
}
