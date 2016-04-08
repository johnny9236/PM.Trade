using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.BOC;
using PM.PaymentProtocolModel;
using PM.Utils.Log;
using PM.Utils.WebUtils;

namespace PM.BOCPtlBiz
{
    public partial class BOCPayProtocols
    {
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="refoundModel">退款请求对象</param>
        /// <param name="cfgInfo"></param>
        /// <returns></returns>
        private BOCRefundResponse SendRefound(BOCRefundRequset refoundModel, CfgInfo cfgInfo)
        {
            BOCRefundResponse refundResponse = null;

            var sendXml = refoundModel.GetMessagePaket();
            LogTxt.WriteEntry(string.Format("发送报文--{0}", sendXml), "中国银行退款协议报文");
            var returnStr = HttpTransfer.RequestPost(cfgInfo.RequestURL, "application/xmlstream", sendXml, Encoding.UTF8);
            LogTxt.WriteEntry(string.Format("接受报文--{0}", returnStr), "中国银行退款协议报文");
            if (!string.IsNullOrEmpty(returnStr))
            {
                refundResponse = new BOCRefundResponse();
                refundResponse.GetModel(returnStr);
            }
            return refundResponse;
        }
    }
}
