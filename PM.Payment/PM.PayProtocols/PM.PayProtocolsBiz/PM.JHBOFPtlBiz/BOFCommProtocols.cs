using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.Utils.Log;
using PM.PaymentProtocolModel.BankCommModel.JHBOF;
using PM.PaymentProtocolModel;

namespace PM.JHBOFPtlBiz
{
    /// <summary>
    /// 金华交行入账明细协议
    /// </summary>
    public partial class BOFCommProtocols : IBankCommProtocol
    {
        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="objModel"></param>
        /// <param name="businessKind"></param>
        /// <returns></returns>
        public dynamic RemoteCall(dynamic objModel, CfgInfo cfgInfo)
        {
            List<JHBofQueryResult> rtn = null;
            try
            {
                rtn = GetJHBOFQuery((JHBOFQueryPayListModel)objModel, cfgInfo);
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(ex.Message, "交行查询");
            }
            return rtn;
        }

       
    }
}
