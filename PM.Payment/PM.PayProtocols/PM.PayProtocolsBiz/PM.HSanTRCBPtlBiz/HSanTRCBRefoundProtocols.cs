using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.HSanTRCB;
using PM.PaymentProtocolModel;
using PM.Utils.Log;
using PM.Utils.SocektUtils;

namespace PM.HSanTRCBPtlBiz
{
    /// <summary>
    /// 退款
    /// </summary>
    public partial class HSanTRCBProtocols
    {
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="refoundModel">退款对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private HSanTRCBRefundResponse SendRefound(HSanTRCBRefundRequset refoundModel, CfgInfo cfgInfo)
        {
            HSanTRCBRefundResponse refundResponse = new HSanTRCBRefundResponse();
            string returnStr = string.Empty;
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            var sendMessage = refoundModel.GetMessagePaket();
            LogTxt.WriteEntry(string.Format("发送报文--{0}", sendMessage), "农商行退款协议报文");
            returnStr = SocketClient.SendToServ(cfgInfo.IP, port, sendMessage, Encoding.GetEncoding("GB2312"));
            LogTxt.WriteEntry(string.Format("接受报文--{0}", returnStr), "农商行退款协议报文");
            if (!string.IsNullOrEmpty(returnStr))
                refundResponse.GetModel(returnStr);
            return refundResponse;
        }
        /// <summary>
        /// 人工退款
        /// </summary>
        /// <param name="refoundModel">退款对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private HSanTRCBRefundResponse SendManualRefound(HSanTRCBRefundRequset refoundModel, CfgInfo cfgInfo)
        {
            refoundModel.ISManual = true;//设置成人工
            HSanTRCBRefundResponse refundResponse = new HSanTRCBRefundResponse();
            string returnStr = string.Empty;
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            var sendMessage = refoundModel.GetManualMessagePaket();
            LogTxt.WriteEntry(string.Format("发送报文--{0}", sendMessage), "农商行人工退款协议报文");
            returnStr = SocketClient.SendToServ(cfgInfo.IP, port, sendMessage, Encoding.GetEncoding("GB2312"));
            LogTxt.WriteEntry(string.Format("接受报文--{0}", returnStr), "农商行人工退款协议报文");
            if (!string.IsNullOrEmpty(returnStr))
                refundResponse.GetModel(returnStr);
            return refundResponse;
        }
  
    }
}
