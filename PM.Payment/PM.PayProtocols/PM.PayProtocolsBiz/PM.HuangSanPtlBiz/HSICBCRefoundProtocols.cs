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
    ///  退款处理
    /// </summary>
    public partial class HSICBCProtocols
    {
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="refoundModel">退款对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private HSICBCRefundResponse SendRefound(HSICBCRefundRequset refoundModel, CfgInfo cfgInfo)
        {
            HSICBCRefundResponse refundResponse = new HSICBCRefundResponse();
            string returnStr = string.Empty;
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            returnStr = SocketClient.SendToServ(cfgInfo.IP, port, refoundModel.GetMessagePaket(), Encoding.GetEncoding("GB2312"));
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
        private HSICBCRefundResponse SendManualRefound(HSICBCRefundRequset refoundModel, CfgInfo cfgInfo)
        {
            HSICBCRefundResponse refundResponse = new HSICBCRefundResponse();
            string returnStr = string.Empty;
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            returnStr = SocketClient.SendToServ(cfgInfo.IP, port, refoundModel.GetMessagePaket(), Encoding.GetEncoding("GB2312"));
            if (!string.IsNullOrEmpty(returnStr))
                refundResponse.GetModel(returnStr);
            return refundResponse;
        }
    }
}
