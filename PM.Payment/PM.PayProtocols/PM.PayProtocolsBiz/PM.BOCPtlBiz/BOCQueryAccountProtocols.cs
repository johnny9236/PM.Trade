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
    /// <summary>
    /// 查询
    /// </summary>
    public partial class BOCCommonProtocols
    {
        /// <summary>
        /// 保证金入账明细
        /// </summary>
        /// <param name="refoundModel"></param>
        /// <param name="cfgInfo"></param>
        /// <returns></returns>
        private BOCQueryAccountDtlResult QueryAccountDtl(BOCQueryAccountDtl refoundModel, CfgInfo cfgInfo)
        {
            BOCQueryAccountDtlResult refundResponse = null;
            var sendXml = refoundModel.GetMessagePaket();
            LogTxt.WriteEntry(string.Format("发送报文--{0}", sendXml), "中国银行入账明细协议报文");
            var returnStr = HttpTransfer.RequestPost(cfgInfo.RequestURL, "application/xmlstream", sendXml, Encoding.UTF8);
            LogTxt.WriteEntry(string.Format("接受报文--{0}", returnStr), "中国银行入账明细协议报文");
            if (!string.IsNullOrEmpty(returnStr))
            {
                refundResponse = new BOCQueryAccountDtlResult();
                refundResponse.GetModel(returnStr);
            }
            return refundResponse;
        }
    }
}
