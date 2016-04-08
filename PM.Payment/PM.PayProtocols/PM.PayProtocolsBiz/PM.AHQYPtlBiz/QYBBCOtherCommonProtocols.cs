using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel;
using PM.Utils.SocektUtils;
using PM.Utils.Log;

namespace PM.AHQYPtlBiz
{
    /// <summary>
    /// 创建虚拟账号等通用协议实现
    /// </summary>
    public partial class QYBBCCommonProtocols
    {
        /// <summary>
        /// 创建虚拟账号
        /// </summary>
        /// <param name="sendMessage">发送报文</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private string CreateVirtualAccount(string sendMessage, CfgInfo cfgInfo)
        {
            LogTxt.WriteEntry(string.Format("发送报文--{0}", sendMessage), "建行创建虚拟账号协议报文");
            var rtnStr = Send(sendMessage, cfgInfo);
            LogTxt.WriteEntry(string.Format("接受报文--{0}", rtnStr), "建行创建虚拟账号协议报文");
            return rtnStr;
        }
        /// <summary>
        /// 更新开标时间
        /// </summary>
        /// <param name="sendMessage">发送报文</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private string UpdateBidTm(string sendMessage, CfgInfo cfgInfo)
        {
            return Send(sendMessage, cfgInfo);
        }
        /// <summary>
        /// 更新保证金时间
        /// </summary>
        /// <param name="sendMessage">发送报文</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private string UpdateBZJTm(string sendMessage, CfgInfo cfgInfo)
        {
            return Send(sendMessage,cfgInfo);
        }
        /// <summary>
        /// 项目结束
        /// </summary>
        /// <param name="sendMessage">发送报文</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private string FinishPro(string sendMessage, CfgInfo cfgInfo)
        {
            return Send(sendMessage, cfgInfo);
        }
        /// <summary>
        /// 提交报文
        /// </summary>
        /// <param name="sendMessage"></param>
        /// <param name="cfgInfo"></param>
        /// <returns></returns>
        private string Send(string sendMessage, CfgInfo cfgInfo)
        {
            LogTxt.WriteEntry(string.Format("发送报文--{0}", sendMessage), "建行通用协议报文");
            string returnStr = string.Empty;
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            returnStr = SocketClient.SendToServ(cfgInfo.IP, port, sendMessage, Encoding.GetEncoding("GB2312"));
            LogTxt.WriteEntry(string.Format("接受报文--{0}", returnStr), "建行通用协议报文");
             return returnStr;
        }

    }
}
