using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;

namespace PM.JSABOC
{
    /// <summary>
    /// 查询明细协议
    /// </summary>
    public partial class JSABOCCommonProtocols : IBankCommProtocol
    {
        /// <summary>
        /// 获取查询明细
        /// </summary>
        /// <param name="objModel">查询请求对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        public dynamic RemoteCall(dynamic objModel, PaymentProtocolModel.CfgInfo cfgInfo)
        {
            return GetQueryList(objModel, cfgInfo);
        }
    }
}
