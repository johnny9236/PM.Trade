using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.PaymentProtocolModel;

namespace PM.DDQABOC
{
    /// <summary>
    /// 查询相关协议
    /// </summary>
    public partial class DDQABOCCommonProtocols : IBankCommProtocol
    {
        public dynamic RemoteCall(dynamic objModel, PaymentProtocolModel.CfgInfo cfgInfo)
        {
            //BusinessType bt = BusinessType.None;
            //    Enum.TryParse(cfgInfo.BusinessKind, out bt);

            return GetQueryList(objModel, cfgInfo);
        }
    }
}
