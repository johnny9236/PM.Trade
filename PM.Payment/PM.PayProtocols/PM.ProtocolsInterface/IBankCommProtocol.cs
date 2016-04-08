using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel;
 

namespace PM.ProtocolsInterface
{
    /// <summary>
    /// 通用协议 请求后能返回结果 （除掉类似支付）
    /// </summary>
    public interface IBankCommProtocol
    {
       /// <summary>
        /// 请求调用（非支付）
       /// </summary>
       /// <param name="objModel">请求对象</param>
       /// <param name="cfgInfo">配置</param>
       /// <returns>调用操作结果</returns>
        dynamic RemoteCall(dynamic objModel,  CfgInfo cfgInfo);
    }
}
