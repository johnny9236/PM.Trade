using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.PaymentProtocolModel;
using PM.NetBankPtlBiz.Protocols;
using PM.JHBOFPtlBiz;
using PM.JSABOC;
using PM.HuangMeiPostalPtlBiz;
using PM.HuangshiCCBPtlBiz;
using PM.PaymentModel;

namespace PM.PaymentManger.Factory
{
    /// <summary>
    /// 通用  工厂方法(非支付)
    /// </summary>
    public class CommProtocolsFactory
    {
        /// <summary>
        /// 非支付通用  工厂方法
        /// </summary>
        /// <returns></returns>
        public static IBankCommProtocol CreateComm(ProtocolsWay protocolsWay)
        {
            IBankCommProtocol protocols = null;
            if (null == protocols)
            {
                switch (protocolsWay)
                {
                    case ProtocolsWay.NetBank://银联
                        protocols = new NetBankCommonProtocols();
                        break;
                    case ProtocolsWay.JHBOF://金华交行
                        protocols = new BOFCommProtocols();
                        break;
                    case ProtocolsWay.JSABOC://嘉善农行
                        protocols = new JSABOCCommonProtocols();
                        break;
                    case ProtocolsWay.HuangMei://黄梅
                        protocols = new HuangMeiPostlCommProtocols();
                        break;
                    case ProtocolsWay.HuangShi://黄石
                        protocols = new HuangShiCCBCommProtocols();
                        break;
                    case ProtocolsWay.LPSBBC://六盘水
                        protocols = new LPSCCBPtlBiz.LPSBBCCommProtocols();
                        break;
                    case ProtocolsWay.QYBBC://安徽青阳
                        protocols = new AHQYPtlBiz.QYBBCCommonProtocols();
                        break;
                    case ProtocolsWay.BOC://中国银行
                        protocols = new BOCPtlBiz.BOCCommonProtocols();
                        break;
                    default:
                        break;
                }
            }
            return protocols;
        }
    }
}
