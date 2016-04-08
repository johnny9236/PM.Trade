using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.ServiceModel.Activation;
using System.ServiceModel;
using System.Reflection;
using System.ServiceModel.Description;

namespace PM.Utils.WCF
{
    /// <summary>
    /// wcf动态生成服务(目前只支持basichttp  其他还未实现)
    /// //使用  在web  golab Application_Start启动
    /// HostingEnvironment.RegisterVirtualPathProvider(new ServicePathProvider());
    /// 配置文件中 加对应服务dll
    /// 设置WcfDLL
    /// 命名空间WcfDLLNameSpace
    /// </summary>
    public class ServicePathProvider : VirtualPathProvider
    {
        public override VirtualFile GetFile(string virtualPath)
        {
            if (!this.IsServiceCall(virtualPath))
                return this.Previous.GetFile(virtualPath);
            return new ServiceFile(virtualPath);
        }

        private bool IsServiceCall(string virtualPath)
        {
            // Check if it is a wcf service call
            virtualPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return (virtualPath.ToLower().StartsWith("~/srv_"));
        }

        public override bool FileExists(string virtualPath)
        {
            if (!this.IsServiceCall(virtualPath))
                return this.Previous.FileExists(virtualPath);
            return true;
        }
    }
    /// <summary>
    /// 文件
    /// </summary>
    public class ServiceFile : VirtualFile
    {
        public ServiceFile(string virtualPath)
            : base(virtualPath)
        { }
        public string GetCallingServiceName
        {
            get
            {
                var pathStr = base.VirtualPath.Split('/'); 
                var className = pathStr.Last().Trim();
                if (string.IsNullOrEmpty(className))
                    throw new ArgumentNullException("请求路径不对");
                className = className.Substring(4, className.LastIndexOf(".") - 4);
                return className.Replace("srv_", string.Empty)
                 .Replace(".svc", string.Empty).ToLower();
            }
        }

        public string GetService()
        {
            string srv = this.GetCallingServiceName;
            // hello => Hello
            return srv[0].ToString().ToUpper() + srv.Substring(1);
        }

        public override Stream Open()
        {
            var serviceDef = new MemoryStream();
            var defWriter = new StreamWriter(serviceDef);
            //动态输出svc文件
            //defWriter.Write(string.Format("<%@ ServiceHost Language=\"C#\" Debug=\"true\" Service=\"{0}.{1}\"   Factory=\"PM.Utils.WCF.DynamicHostFactory, PM.Utils\" %>", ConfigHelper.GetConfigString("WcfDllNameSpace"), this.GetService()));
            defWriter.Write(string.Format("<%@ ServiceHost Language=\"C#\" Service=\"{0}\"   Factory=\"PM.Utils.WCF.DynamicHostFactory, PM.Utils\" %>", this.GetService()));
            defWriter.Flush();
            serviceDef.Position = 0;
            return serviceDef;
        }
    }
    /// <summary>
    /// 动态产生
    /// </summary>
    public class DynamicHostFactory : ServiceHostFactory
    {
        public DynamicHostFactory() { }
        public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        {
            ServiceHost host = null;
            try
            {
                var ssemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                var contractAssemb = ssemblies.FirstOrDefault(p => p.GetTypes().ToList().FindIndex(h => h.Name.ToLower() == constructorString.ToLower()) > -1);
                if (null == contractAssemb)
                    throw new NullReferenceException("未找到对应契约程序集");               
                var serviceType = contractAssemb.GetTypes().FirstOrDefault(p => p.Name.ToLower() == constructorString.ToLower());
                if(null==serviceType)
                    throw new NullReferenceException("未找到对应契约类");    
                host = new ServiceHost(serviceType, baseAddresses);
                // Add endpoints
                foreach (Type contract in serviceType.GetInterfaces())
                {
                    var attribute = (ServiceContractAttribute)
                        Attribute.GetCustomAttribute(contract, typeof(ServiceContractAttribute));
                    if (attribute != null)
                        host.AddServiceEndpoint(contract, new BasicHttpBinding(), "");
                }
                // Add metdata behavior for generating wsdl
                var metadata = new ServiceMetadataBehavior();
                metadata.HttpGetEnabled = true;
                host.Description.Behaviors.Add(metadata);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return host;
        }
    }
}
