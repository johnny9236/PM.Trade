//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using PM.ProtocolsInterface;
//using PM.NetBankPtlBiz.Protocols;
//using PM.JHBOFPtlBiz;
//using PM.PaymentProtocolModel;
//using PM.JSABOC;
//using PM.HuangMeiPostalPtlBiz;
//using PM.HuangshiCCBPtlBiz;

//namespace PM.PaymentManger
//{
//    /// <summary>
//    /// 协议工厂类
//    /// </summary>
//    public static class ProtocolsFactory
//    {
//        ///// <summary>
//        ///// 支付  工厂方法
//        ///// </summary>
//        ///// <returns></returns>
//        //public static IPaymentProtocol CreatePay(ProtocolsWay protocolsWay)
//        //{
//        //    IPaymentProtocol protocols = null;
//        //    if (null == protocols)
//        //    {
//        //        switch (protocolsWay)
//        //        {
//        //            case ProtocolsWay.NetBank:
//        //                protocols = new NetBankProtocols();
//        //                break;
//        //            case ProtocolsWay.JSABOC://嘉善农行
//        //                protocols = new JSABOCProtocols();
//        //                break;
//        //            default:
//        //                break;
//        //        }
//        //    }
//        //    return protocols;
//        //}
//        ///// <summary>
//        ///// 非支付  工厂方法
//        ///// </summary>
//        ///// <returns></returns>
//        //public static IBankCommProtocol CreateComm(ProtocolsWay protocolsWay)
//        //{
//        //    IBankCommProtocol protocols = null;
//        //    if (null == protocols)
//        //    {
//        //        switch (protocolsWay)
//        //        {
//        //            case ProtocolsWay.NetBank://银联
//        //                protocols = new NetBankCommonProtocols();
//        //                break;
//        //            case ProtocolsWay.JHBOF://金华交行
//        //                protocols = new BOFCommProtocols();
//        //                break;
//        //            case ProtocolsWay.JSABOC://嘉善农行
//        //                protocols = new JSABOCCommonProtocols();
//        //                break;
//        //            case ProtocolsWay.HuangMei://黄梅
//        //                protocols = new HuangMeiPostlCommProtocols();
//        //                break;
//        //            case ProtocolsWay.HuangShi://黄石
//        //                protocols = new HuangShiCCBCommProtocols();
//        //                break;
//        //            default:
//        //                break;
//        //        }
//        //    }
//        //    return protocols;
//        //}
//    }
//}
