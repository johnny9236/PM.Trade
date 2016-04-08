using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.Utils.Log;
using PM.PaymentProtocolModel;
using PM.PaymentManger.Factory;
using PM.PaymentModel;
 
namespace PM.PaymentManger
{
    /// <summary>
    /// 支付相关管理类
    /// </summary>
    public class PaymentManager
    {
        /// <summary>
        /// 支付请求
        /// </summary>
        /// <param name="payModel"></param>
        /// <param name="sysConfigModel"></param>
        /// <returns></returns>
        public ResultInfo DoPay(dynamic payModel, SysConfigModel sysConfigModel)
        {
            var cfg = PaymentConfig.GetPaymentConfig(payModel as CommunicationBase, sysConfigModel);
            return DoPay(payModel as object, cfg);
        }

        /// <summary>
        /// 支付请求
        /// </summary>
        /// <param name="payModel"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public ResultInfo DoPay(object payModel, CfgInfo cfg)
        {
            ResultInfo rInfo = null;
            IPaymentProtocol paymentProtocol = null;
            ProtocolsWay pWay = ProtocolsWay.NULL;
            if (null != cfg)
            {
                try
                {
                    Enum.TryParse(cfg.ProtocolsWay, out pWay);
                    var payMent = payModel as CommunicationBase;
                    paymentProtocol = PaymentProtocolsFactory.CreatePay(pWay);
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry(ex.Message, "支付信息");
                    return null;
                }
                rInfo = paymentProtocol.CallRemotePay(payModel, cfg);
            }
            else
            {
                LogTxt.WriteEntry("未找到配置信息", "支付信息");
            }
            return rInfo;
        }

        /// <summary>
        /// 支付响应
        /// </summary>
        /// <param name="payModel"></param>
        /// <param name="sysConfigModel"></param>
        /// <returns></returns>
        public ResultInfo PayCallBack(dynamic payModel, SysConfigModel sysConfigModel)
        {
            ResultInfo rInfo = null;
            if (null != sysConfigModel)
            {
                var cfg = PaymentConfig.GetPaymentConfig(payModel as CommunicationBase, sysConfigModel);
                rInfo = PayCallBack(payModel as object, cfg);
            }
            else
            {
                LogTxt.WriteEntry("未找到配置信息", "支付信息");
            }
            return rInfo;
        }
        /// <summary>
        /// 支付响应
        /// </summary>
        /// <param name="payModel"></param>
        /// <returns></returns>
        public ResultInfo PayCallBack(dynamic payModel, CfgInfo cfg)
        {
            ResultInfo rInfo = null;
            IPaymentProtocol paymentProtocol = null;
            ProtocolsWay pWay = ProtocolsWay.NULL;
            if (null != cfg)
            {
                try
                {
                    Enum.TryParse(cfg.ProtocolsWay, out pWay);
                    paymentProtocol = PaymentProtocolsFactory.CreatePay(pWay);
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry(ex.Message, "支付信息");
                    return null;
                }
                rInfo = paymentProtocol.CallBackParse(payModel as object, cfg);
            }
            else
            {
                LogTxt.WriteEntry("未找到配置信息", "支付信息");
            }

            return rInfo;
        }


        ///// <summary>
        ///// 初始化
        ///// </summary>
        ///// <param name="sendInfo"></param>
        ///// <returns></returns>
        //public bool Init(string cfgPath)
        //{
        //    bool result = false;
        //    try
        //    {
        //        PaymentEnvironment.Initialize(cfgPath);
        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogTxt.WriteEntry(ex.Message, "初始化失败");
        //    }
        //    return result;
        //}
    }
}
