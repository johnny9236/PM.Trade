using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;

namespace PM.HuangMeiPostalPtlBiz
{
    /// <summary>
    /// 黄梅查询协议
    /// </summary>
    public partial class HuangMeiPostlCommProtocols : IBankCommProtocol
    {
        /// <summary>
        /// 调用查询入账
        /// </summary>
        /// <param name="objModel">查询入账对象</param>
        /// <param name="cfgInfo">配置信息</param>
        /// <returns></returns>
        public dynamic RemoteCall(dynamic objModel, PaymentProtocolModel.CfgInfo cfgInfo)
        {
            return GetQueryList(objModel, cfgInfo);
        }
    }
}
