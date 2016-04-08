using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PM.Utils.WCF
{
    /// <summary>
    /// 客户端动态调用wcf服务
    /// </summary>
    public class WCFInvoke
    {
        #region Wcf服务工厂
        /// <summary>
        /// 默认调用是wsHttpBinding
        /// </summary>
        /// <typeparam name="T">协议接口</typeparam>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        public static T CreateWCFServiceByURL<T>(string url)
        {
            return CreateWCFServiceByURL<T>(url, WCFBindingType.WSHttpBinding);
        }
        /// <summary>
        /// wcf调用
        /// </summary>
        /// <typeparam name="T">协议接口</typeparam>
        /// <param name="url">url地址</param>
        /// <param name="bing">绑定类型</param>
        /// <returns></returns>
        public static T CreateWCFServiceByURL<T>(string url, WCFBindingType bing)
        {
            if (string.IsNullOrEmpty(url)) throw new NotSupportedException("this url isn`t Null or Empty!");
            EndpointAddress address = new EndpointAddress(url);
            Binding binding = CreateBinding(bing);
            ChannelFactory<T> factory = new ChannelFactory<T>(binding, address);
            return factory.CreateChannel();
        }
        #endregion
        #region 创建传输协议
        /// <summary>
        /// 创建传输协议
        /// </summary>
        /// <param name="binding">绑定类型</param>
        /// <returns></returns>
        private static Binding CreateBinding(WCFBindingType binding)
        {
            Binding bindinginstance = null;
            DateTime dtStar = DateTime.Now;
            DateTime dtEnd = dtStar.AddMinutes(5);
            TimeSpan tm = dtEnd.Subtract(dtStar);
            
            switch (binding)
            {
                case WCFBindingType.BasicHttpBindin:
                    BasicHttpBinding ws = new BasicHttpBinding();
                    ws.MaxReceivedMessageSize = 65535000;
                    ws.SendTimeout = tm;
                    bindinginstance = ws;
                    break;
                case WCFBindingType.NetNamedPipeBinding:
                    NetNamedPipeBinding wsNetNamed = new NetNamedPipeBinding();
                    wsNetNamed.MaxReceivedMessageSize = 65535000;
                    wsNetNamed.SendTimeout = tm;
                    bindinginstance = wsNetNamed;
                    break;
                case WCFBindingType.NetPeerTcpBinding:
                    NetPeerTcpBinding wsNetPeer = new NetPeerTcpBinding();
                    wsNetPeer.MaxReceivedMessageSize = 65535000;
                    wsNetPeer.SendTimeout = tm;
                    bindinginstance = wsNetPeer;
                    break;
                case WCFBindingType.NetTcpBinding:
                    NetTcpBinding wsNetTcp = new NetTcpBinding();
                    wsNetTcp.MaxReceivedMessageSize = 65535000;
                    wsNetTcp.SendTimeout = tm;
                    wsNetTcp.Security.Mode = SecurityMode.None;
                    bindinginstance = wsNetTcp;
                    break;
                case WCFBindingType.WSDualHttpBinding:
                    WSDualHttpBinding wsDualHttp = new WSDualHttpBinding();
                    wsDualHttp.MaxReceivedMessageSize = 65535000;
                    wsDualHttp.SendTimeout = tm;
                    bindinginstance = wsDualHttp;
                    break;
                case WCFBindingType.WSFederationHttpBinding:
                    WSFederationHttpBinding wsFederation = new WSFederationHttpBinding();
                    wsFederation.MaxReceivedMessageSize = 65535000;
                    wsFederation.SendTimeout = tm;
                    bindinginstance = wsFederation;
                    break;
                case WCFBindingType.WSHttpBinding:
                    WSHttpBinding wsHttp = new WSHttpBinding(SecurityMode.None);
                    wsHttp.MaxReceivedMessageSize = 65535000; 
                    wsHttp.SendTimeout = tm;
                    wsHttp.Security.Message.ClientCredentialType = System.ServiceModel.MessageCredentialType.Windows;
                    wsHttp.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Windows; 
                    bindinginstance = wsHttp;
                    break;
           
                //else if (binding.ToLower() == "webhttpbinding")
                //{
                //    WebHttpBinding ws = new WebHttpBinding();
                //    ws.MaxReceivedMessageSize = 65535000;
                //    bindinginstance = ws;
                //} 
            }
            return bindinginstance;
        }
        #endregion
    }
    /// <summary>
    /// wcf绑定类型
    /// </summary>
    public enum WCFBindingType
    {
        /// <summary>
        /// 基础http绑定
        /// </summary>
        BasicHttpBindin,
        /// <summary>
        /// net命名管道绑定
        /// </summary>
        NetNamedPipeBinding,
        /// <summary>
        /// 对等网通绑定
        /// </summary>
        NetPeerTcpBinding,
        /// <summary>
        /// tcp绑定
        /// </summary>
        NetTcpBinding,
        /// <summary>
        /// 双工绑定
        /// </summary>
        WSDualHttpBinding,
        /// <summary>
        /// 联合安全绑定
        /// </summary>
        WSFederationHttpBinding,
        /// <summary>
        /// 分布式绑定
        /// </summary>
        WSHttpBinding
    }
}
