using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.HSanTRCB;
using PM.Utils.Log;
using PM.Utils.SocektUtils;
using PM.PaymentProtocolModel;

namespace PM.HSanTRCBPtlBiz
{
    public partial class HSanTRCBCommonProtocols
    {
        /// <summary>
        /// 保证金入账明细
        /// </summary>
        /// <param name="queryModel">查询对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private HSanTRCBQueyResultModel QueryAccountDtl(HSanTRCBQueryOrRtnQueryAccountDtl queryModel, CfgInfo cfgInfo)
        {
            HSanTRCBQueyResultModel queryRestult = null;
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            var sendMessage = queryModel.GetMessagePaket();
            LogTxt.WriteEntry(string.Format("发送报文--{0}", sendMessage), "农商行保证金入账明细协议报文");
            var returnStr = SocketClient.SendToServ(cfgInfo.IP, port, sendMessage, Encoding.GetEncoding("GB2312"));
            LogTxt.WriteEntry(string.Format("接受报文--{0}", returnStr), "农商行保证金入账明细协议报文");
            if (!string.IsNullOrEmpty(returnStr))
            {
                queryRestult = new HSanTRCBQueyResultModel();
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
        private HSanTRCBQueryRtnResultModel QueryRtnAccountDtl(HSanTRCBQueryOrRtnQueryAccountDtl queryModel, CfgInfo cfgInfo)
        {
            HSanTRCBQueryRtnResultModel queryRestult = null;
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            var sendMessage = queryModel.GetMessagePaket();
            LogTxt.WriteEntry(string.Format("发送报文--{0}", sendMessage), "农商行保证金退还明细协议报文");
            var returnStr = SocketClient.SendToServ(cfgInfo.IP, port, sendMessage, Encoding.GetEncoding("GB2312"));
            LogTxt.WriteEntry(string.Format("接受报文--{0}", returnStr), "农商行保证金退还明细协议报文");
            if (!string.IsNullOrEmpty(returnStr))
            {
                queryRestult = new HSanTRCBQueryRtnResultModel();
                queryRestult.GetModel(returnStr);
            }
            return queryRestult;
        }
    }
}
