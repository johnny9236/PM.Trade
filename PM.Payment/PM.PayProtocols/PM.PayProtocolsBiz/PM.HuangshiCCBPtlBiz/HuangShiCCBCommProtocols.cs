using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.Utils.Log;
using PM.PaymentProtocolModel.BankCommModel.HuangShi;

namespace PM.HuangshiCCBPtlBiz
{
    /// <summary>
    /// 黄石入账明细查询协议
    /// </summary>
    public partial class HuangShiCCBCommProtocols : IBankCommProtocol
    {
        /// <summary>
        /// 调用查询入账报文信息
        /// </summary>
        /// <param name="objModel">查询对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        public dynamic RemoteCall(dynamic objModel, PaymentProtocolModel.CfgInfo cfgInfo)
        {
            HuangShiDepositResponseModel response = null;
            try
            {
                response = GetHuangshiDepositQuery(objModel, cfgInfo);
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(string.Format("查询黄石保证金交易明细,{0}", ex.Message), "HuangShiCCB保证金明细查询");
            }
            return response;
        }
    }
}
