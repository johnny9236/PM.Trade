using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.BOC;
using PM.PaymentProtocolModel;
using PM.Utils.WebUtils;
using PM.Utils.Log;

namespace PM.BOCPtlBiz
{
    /// <summary>
    /// 签到签退
    /// </summary>
    public partial class BOCCommonProtocols
    {
        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="refoundModel"></param>
        /// <param name="cfgInfo"></param>
        /// <returns></returns>
        private BOCSignInResponse SignIn(BOCSignInRequest refoundModel, CfgInfo cfgInfo)
        {
            BOCSignInResponse refundResponse = null;
            var sendXml = refoundModel.GetMessagePaket();
            LogTxt.WriteEntry(string.Format("发送报文--{0}", sendXml), "中国银行签到协议报文");
            var returnStr = HttpTransfer.RequestPost(cfgInfo.RequestURL, "application/xmlstream", sendXml, Encoding.UTF8);

            LogTxt.WriteEntry(string.Format("接受报文--{0}", returnStr), "中国银行签到协议报文");
            if (!string.IsNullOrEmpty(returnStr))
            {
                refundResponse = new  BOCSignInResponse();
                refundResponse.GetModel(returnStr);
            }
            return refundResponse;
        }
        /// <summary>
        /// 签退
        /// </summary>
        /// <param name="refoundModel"></param>
        /// <param name="cfgInfo"></param>
        /// <returns></returns>
        private BOCSignOUtResponse SignOUt(BOCSignOUtRequset refoundModel, CfgInfo cfgInfo)
        {
            BOCSignOUtResponse refundResponse = null;
            var sendXml = refoundModel.GetMessagePaket();
            LogTxt.WriteEntry(string.Format("发送报文--{0}", sendXml), "中国银行签退协议报文");
            var returnStr = HttpTransfer.RequestPost(cfgInfo.RequestURL, "application/xmlstream", sendXml, Encoding.UTF8);
            LogTxt.WriteEntry(string.Format("接受报文--{0}", returnStr), "中国银行签退协议报文");
            if (!string.IsNullOrEmpty(returnStr))
            {
                refundResponse = new BOCSignOUtResponse();
                refundResponse.GetModel(returnStr);
            }
            return refundResponse;
        }
    }
}
