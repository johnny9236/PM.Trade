using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;

namespace PM.LPSCCBPtlBiz
{
    public  partial class LPSBBCCommProtocols : IBankCommProtocol
    {
        /// <summary>
        /// 查询入账
        /// </summary>
        /// <param name="objModel">查询对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        public dynamic RemoteCall(dynamic objModel, PaymentProtocolModel.CfgInfo cfgInfo)
        {
            return GetQueryList(objModel, cfgInfo);//建行查询入账信息
        }  
    }
}
