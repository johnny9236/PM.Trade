using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.AHQY;
using PM.PaymentProtocolModel;
using PM.Utils.SocektUtils;
using PM.Utils.Log;

namespace PM.AHQYPtlBiz
{
    /// <summary>
    /// 查询明细相关
    /// </summary>
    public partial class QYBBCCommonProtocols
    {
        /// <summary>
        /// 保证金入账明细
        /// </summary>
        /// <param name="queryModel">查询对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private QYBBCQueyResultModel QueryAccountDtl(QYBBCQueryOrRtnQueryAccountDtl queryModel, CfgInfo cfgInfo)
        {
            QYBBCQueyResultModel queryRestult = null;
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            var sendMessage = queryModel.GetMessagePaket();
            LogTxt.WriteEntry(string.Format("发送报文--{0}", sendMessage), "建行保证金入账明细协议报文");
            var returnStr = SocketClient.SendToServ(cfgInfo.IP, port, sendMessage, Encoding.GetEncoding("GB2312"));
            LogTxt.WriteEntry(string.Format("接受报文--{0}", returnStr), "建行保证金入账明细协议报文");
            if (!string.IsNullOrEmpty(returnStr))
            {
                queryRestult = new QYBBCQueyResultModel();
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
        private QYBBCQueryRtnResultModel QueryRtnAccountDtl(QYBBCQueryOrRtnQueryAccountDtl queryModel, CfgInfo cfgInfo)
        {
            QYBBCQueryRtnResultModel queryRestult = null;
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            var sendMessage = queryModel.GetMessagePaket();
            LogTxt.WriteEntry(string.Format("发送报文--{0}", sendMessage), "建行保证金退还明细协议报文");
            var returnStr = SocketClient.SendToServ(cfgInfo.IP, port, sendMessage, Encoding.GetEncoding("GB2312"));
            LogTxt.WriteEntry(string.Format("接受报文--{0}", returnStr), "建行保证金退还明细协议报文");
            if (!string.IsNullOrEmpty(returnStr))
            {
                queryRestult = new QYBBCQueryRtnResultModel();
                queryRestult.GetModel(returnStr);
            }
            return queryRestult;
        }

    }
}
