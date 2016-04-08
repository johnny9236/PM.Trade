using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel;
using PM.PaymentProtocolModel.BankCommModel.AHQY;
using PM.Utils.SocektUtils;
using PM.Utils.Log;

namespace PM.AHQYPtlBiz
{
    /// <summary>
    ///  退款处理
    /// </summary>
    public partial class QYBBCProtocols
    {
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="refoundModel">退款对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private QYBBCRefundResponse SendRefound(QYBBCRefundRequset refoundModel, CfgInfo cfgInfo)
        {
            QYBBCRefundResponse refundResponse = new QYBBCRefundResponse();
            string returnStr = string.Empty;
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            var sendMessage = refoundModel.GetMessagePaket();
            LogTxt.WriteEntry(string.Format("发送报文--{0}", sendMessage), "建行退款协议报文");
            returnStr = SocketClient.SendToServ(cfgInfo.IP, port, sendMessage, Encoding.GetEncoding("GB2312"));
            LogTxt.WriteEntry(string.Format("接受报文--{0}", returnStr), "建行退款协议报文");
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
        private QYBBCRefundResponse SendManualRefound(QYBBCRefundRequset refoundModel, CfgInfo cfgInfo)
        {
            refoundModel.ISManual = true;//设置成人工
            QYBBCRefundResponse refundResponse = new QYBBCRefundResponse();
            string returnStr = string.Empty;
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            var sendMessage = refoundModel.GetManualMessagePaket();
            LogTxt.WriteEntry(string.Format("发送报文--{0}", sendMessage), "建行人工退款协议报文");
            returnStr = SocketClient.SendToServ(cfgInfo.IP, port, sendMessage, Encoding.GetEncoding("GB2312"));
            LogTxt.WriteEntry(string.Format("接受报文--{0}", returnStr), "建行人工退款协议报文");
            if (!string.IsNullOrEmpty(returnStr))
                refundResponse.GetModel(returnStr);
            return refundResponse;
        }
    }
}
