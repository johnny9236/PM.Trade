using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.PaymentProtocolModel;
using PM.Utils.Log;

namespace PM.AHQYPtlBiz
{
    /// <summary>
    /// 通用协议实现(非支付)
    /// </summary>
    public partial class QYBBCCommonProtocols : IBankCommProtocol
    {
        /// <summary>
        /// 通用协议
        /// </summary>
        /// <param name="objModel">请求对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        public dynamic RemoteCall(dynamic objModel, PaymentProtocolModel.CfgInfo cfgInfo)
        {
            try
            {
                BusinessType bt = BusinessType.None;
                Enum.TryParse(cfgInfo.BusinessKind, out bt);
                switch (bt)
                {
                    case BusinessType.Create://创建虚拟账号
                        return CreateVirtualAccount(objModel, cfgInfo);
                    case BusinessType.Update://更新开标时间
                        return UpdateBidTm(objModel, cfgInfo);
                    case BusinessType.Other://更新保证金截止时间
                        return UpdateBZJTm(objModel, cfgInfo);
                    case BusinessType.BankStatement://保证金入账明细
                        return QueryAccountDtl(objModel, cfgInfo);
                    case BusinessType.MarketPayQuery: //保证金退还明细
                        return QueryRtnAccountDtl(objModel, cfgInfo);
                    case BusinessType.Finish: //保证金退还明细
                        return FinishPro(objModel, cfgInfo);
                }
            }
            catch (Exception ex)
            {
                #region 异常处理
                LogTxt.WriteEntry(string.Format("{0}-{1}", ex.Message, cfgInfo.BusinessKind), "青阳建行通用协议发起异常");
                //  rInfo.MSG = string.Format("{0}-{1}", ex.Message, paymentModel.BusinessKind.ToString());
                #endregion
            }
            return null;
        }
    }
}
