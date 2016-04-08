using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.PaymentProtocolModel;
using PM.NetBankPtlBiz.Protocols;
using PM.JSABOC;
using PM.ALiPtlBiz;
using PM.PaymentModel;
using PM.LPSCCBPtlBiz;

namespace PM.PaymentManger.Factory
{
    /// <summary>
    /// 支付  工厂方法
    /// </summary>
   public  class PaymentProtocolsFactory
    {
        /// <summary>
        /// 支付  工厂方法
        /// </summary>
        /// <returns></returns>
        public static IPaymentProtocol CreatePay(ProtocolsWay protocolsWay)
        {
            IPaymentProtocol protocols = null;
            if (null == protocols)
            {
                switch (protocolsWay)
                {
                    case ProtocolsWay.NetBank:
                        protocols = new NetBankProtocols();
                        break;
                    case ProtocolsWay.JSABOC://嘉善农行
                        protocols = new JSABOCProtocols();
                        break;
                    case ProtocolsWay.HuangMei://黄梅
                    case ProtocolsWay.ALI://阿里
                        protocols = new ALiProtocols();
                        break;
                    case ProtocolsWay.LPSBBC://六盘水
                        protocols = new BBCProtocols();
                        break;
                    case ProtocolsWay.QYBBC://安徽青阳
                        protocols = new AHQYPtlBiz.QYBBCProtocols();
                        break;
                    case ProtocolsWay.BOC://中国银行
                        protocols = new BOCPtlBiz.BOCPayProtocols();
                        break;
                    default:
                        break;
                }
            }
            return protocols;
        }
    }
}
