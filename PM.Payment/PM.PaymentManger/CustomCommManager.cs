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
    /// 非支付相关通用管理
    /// </summary>
    public class CustomCommManager
    {
        /// <summary>
        /// 根据配置实例化对应对象
        /// </summary>
        /// <param name="objModel">通用对象（传递的主要是报文字符串）</param>
        /// <returns></returns>
        public static dynamic CallProtocol(dynamic objModel)
        {
            var commModel = (CommServiceProtocolModel)objModel;
            //var cfg = CommPaymentConfig.GetCommConfig(objModel as CommunicationBase, sysConfigModel);
            var cfg = CommPaymentConfig.GetCommConfig(commModel.BusinessFunNo);
            return CallProtocol(commModel.Content, cfg);
        }

        /// <summary>
        /// 根据配置实例化对应对象
        /// </summary>
        /// <param name="objModel">通用对象</param>
        /// <param name="sysConfigModel">配置对象</param>
        /// <returns></returns>
        public static dynamic CallProtocol(object objModel, SysConfigModel sysConfigModel)
        {
            var cfg = CommPaymentConfig.GetCommConfig(objModel as CommunicationBase, sysConfigModel);
            return CallProtocol(objModel, cfg);
        }

        /// <summary>
        /// 根据配置实例化对应对象
        /// </summary>
        /// <param name="objModel"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static dynamic CallProtocol(object objModel, CfgInfo cfg)
        {
            try
            {
                //  var cfg = CommPaymentConfig.GetCommProtocol(objModel as CommunicationBase, sysConfigModel);
                if (null != cfg)
                {
                    ProtocolsWay protocolsWay = ProtocolsWay.NULL;
                    Enum.TryParse(cfg.ProtocolsWay, out  protocolsWay);
                    if (protocolsWay == ProtocolsWay.NULL)
                    {
                        LogTxt.WriteEntry("无协议类型", "支付相关");
                        return null;
                    }
                    IBankCommProtocol commProtocol = CommProtocolsFactory.CreateComm(protocolsWay);
                    return commProtocol.RemoteCall(objModel, cfg);
                }
                else
                {
                    LogTxt.WriteEntry("未找到配置信息", "支付相关");
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(ex.Message, "支付相关");
                return null;
            }
        }
    }
}
