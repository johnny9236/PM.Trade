using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.HSICBC;
using PM.PaymentProtocolModel;
using PM.Utils.SocektUtils;

namespace PM.HuangSanPtlBiz
{
    /// <summary>
    /// 查询明细相关
    /// </summary>
    public partial class HSICBCCommonProtocols
    {
        /// <summary>
        /// 保证金入账明细
        /// </summary>
        /// <param name="queryModel">查询对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private HSICBCQueyResultModel QueryAccountDtl(HSICBCQueryAccountDtl queryModel, CfgInfo cfgInfo)
        {
            HSICBCQueyResultModel queryRestult = null;
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            var returnStr = SocketClient.SendToServ(cfgInfo.IP, port, queryModel.GetMessagePaket(), Encoding.GetEncoding("GB2312"));
            if (!string.IsNullOrEmpty(returnStr))
            {
                queryRestult = new HSICBCQueyResultModel();
                queryRestult.GetModel(returnStr);
            }
            return queryRestult;
        }
        /// <summary>
        /// 保证金退还明细
        /// </summary>
        /// <param name="queryModel">查询对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private HSICBCQueryRtnResultModel QueryRtnAccountDtl(HSICBCQueryAccountDtl queryModel, CfgInfo cfgInfo)
        {
            HSICBCQueryRtnResultModel queryRestult = null;
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            var returnStr = SocketClient.SendToServ(cfgInfo.IP, port, queryModel.GetMessagePaket(), Encoding.GetEncoding("GB2312"));
            if (!string.IsNullOrEmpty(returnStr))
            {
                queryRestult = new HSICBCQueryRtnResultModel();
                queryRestult.GetModel(returnStr);
            }
            return queryRestult;
        }
    }
}
