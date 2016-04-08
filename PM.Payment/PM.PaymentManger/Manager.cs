using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Log;
using PM.ProtocolsInterface;
using PM.PaymentProtocolModel;
using PM.PaymentManger.Factory;
using PM.PaymentModel;

namespace PM.PaymentManger
{
    /// <summary>
    /// 管理类
    /// </summary>
    public class Manager
    {
        /// <summary>
        ///    支付相关管理
        /// </summary>
        /// <param name="objModel">实体对象</param>
        /// <returns></returns>
        public static dynamic PaymentManager(dynamic objModel)
        {
            var model = objModel as CommunicationBase;
                      var cfg = CommPaymentConfig.GetCommConfig(model);//获取配置
            if (null != cfg)
            {
                return PaymentManager(objModel as object, cfg);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 支付相关管理
        /// </summary>
        /// <param name="objModel"></param>
        /// <param name="sysConfigModel"></param>
        /// <returns></returns>
        public static dynamic PaymentManager(dynamic objModel, SysConfigModel sysConfigModel)
        {
            //ResultInfo rInfo = null;//返回结果 
            var model = objModel as CommunicationBase;
            var cfg = CommPaymentConfig.GetCommConfig(model, sysConfigModel);//获取配置
            if (null != cfg)
            {
                return PaymentManager(objModel as object, cfg);
            }
            return null;
        }

        /// <summary>
        /// 支付相关管理
        /// </summary>
        /// <param name="objModel"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static dynamic PaymentManager(dynamic objModel, CfgInfo cfg)
        {
            ResultInfo rInfo = null;//返回结果
            ProtocolsWay protocolsWay = ProtocolsWay.NULL;//协议 
            OprationType oprationType = OprationType.NULL;//操作类型 
            ActionType actionType = ActionType.NULL;//动作类型 
            var model = objModel as CommunicationBase;
            //var cfg = CommPaymentConfig.GetCommConfig(model, sysConfigModel);//获取配置
            if (null != cfg)
            {
                try
                {
                    Enum.TryParse(cfg.ProtocolsWay, out  protocolsWay);
                    Enum.TryParse(cfg.OprationType, out  oprationType);
                    Enum.TryParse(cfg.ActionType, out  actionType);
                    #region  判断
                    if (protocolsWay == ProtocolsWay.NULL)
                    {
                        LogTxt.WriteEntry("无协议类型" + model.BusinessFunNo, "支付相关信息");
                        return null;
                    }
                    if (oprationType == OprationType.NULL)
                    {
                        LogTxt.WriteEntry("无操作类型" + model.BusinessFunNo, "支付相关信息");
                        return null;
                    }
                    if (actionType == ActionType.NULL)
                    {
                        LogTxt.WriteEntry("无动作类型" + model.BusinessFunNo, "支付相关信息");
                        return null;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry(ex + model.BusinessFunNo, "支付相关信息");
                }
                #region 实例化对象
                if (oprationType == OprationType.Pay)//支付
                {
                   // var payModel = objModel as PaymentModel;
                    IPaymentProtocol paymentProtocol = PaymentProtocolsFactory.CreatePay(protocolsWay);
                    if (actionType == ActionType.Request)//请求
                        //rInfo = paymentProtocol.CallRemotePay(objModel as object, cfg);
                        return  paymentProtocol.CallRemotePay(objModel as object, cfg);
                    else//响应
                        //rInfo = paymentProtocol.CallBackParse(objModel as object, cfg);
                        return  paymentProtocol.CallBackParse(objModel as object, cfg);
                }
                else//其他都是只有请求操作的 
                {
                    IBankCommProtocol commProtocol = CommProtocolsFactory.CreateComm(protocolsWay);
                    return commProtocol.RemoteCall(objModel as object, cfg);
                }
                #endregion

            }
            return rInfo;
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="cfgFilePath"></param>
        /// <returns></returns>
        public static bool Init(string cfgFilePath)
        {
            return InitCfCa(cfgFilePath);
        }

        #region
        /// <summary>
        /// 银联初始化(银联的)
        /// </summary>
        /// <param name="cfgFilePath"></param>
        /// <returns></returns>
        public static bool InitCfCa(string cfgFilePath)
        {
            bool result = false;
            try
            {
                CFCA.Payment.Api.PaymentEnvironment.Initialize(cfgFilePath);
                result = true;
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("银联" + ex.Message, "配置信息");
            }
            return result;
        }
        #endregion
    }
}
