using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel;

namespace PM.PaymentManger
{
    /// <summary>
    /// 获取支付配置信息
    /// </summary>
    public class PaymentConfig
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="payModel"></param>
        /// <param name="sysConfigModel"></param>
        /// <returns></returns>
        public static CfgInfo GetPaymentConfig(CommunicationBase payModel, SysConfigModel sysConfigModel)
        {
            var cfg = sysConfigModel.CfgInfoList.FirstOrDefault(p => p.BusinessNo.Trim().ToLower() == payModel.BusinessFunNo.Trim().ToLower());
            return cfg;
        }
    }
}
