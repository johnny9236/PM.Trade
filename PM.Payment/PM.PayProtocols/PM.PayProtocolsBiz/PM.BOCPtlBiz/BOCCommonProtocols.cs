using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.PaymentProtocolModel;
using PM.Utils.Log;

namespace PM.BOCPtlBiz
{
    /// <summary>
    /// 中国银行通用接口
    /// </summary>
    public partial class BOCCommonProtocols : IBankCommProtocol
    {
        public dynamic RemoteCall(dynamic objModel, PaymentProtocolModel.CfgInfo cfgInfo)
        {
            try
            {
                BusinessType bt = BusinessType.None;
                Enum.TryParse(cfgInfo.BusinessKind, out bt);
                switch (bt)
                {
                    case BusinessType.Create://签到
                        return SignIn(objModel, cfgInfo);
                    case BusinessType.Finish://签退
                        return SignOUt(objModel, cfgInfo);                
                    case BusinessType.BankStatement://保证金入账明细
                        return QueryAccountDtl(objModel, cfgInfo);                  
                }
            }
            catch (Exception ex)
            {
                #region 异常处理
                LogTxt.WriteEntry(string.Format("{0}-{1}-{2}", ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name), "中国银行通用协议发起异常");
                //  rInfo.MSG = string.Format("{0}-{1}", ex.Message, paymentModel.BusinessKind.ToString());
                #endregion
            }
            return null;
        }
    }
}
