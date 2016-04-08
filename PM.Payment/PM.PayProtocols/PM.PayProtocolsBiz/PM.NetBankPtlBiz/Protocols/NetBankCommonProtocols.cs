using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.ProtocolsInterface;
using PM.Utils.Log;
using PM.NetBankPtlBiz.Model;
using PM.PaymentProtocolModel;
using PM.PaymentProtocolModel.BankCommModel;
using PM.PaymentProtocolModel.BankCommModel.Netbank;
 

namespace PM.NetBankPtlBiz.Protocols
{
    /// <summary>
    /// 银联非支付实现
    /// </summary>
    public partial class NetBankCommonProtocols : IBankCommProtocol
    {
        /// <summary>
        /// 调用 返回
        /// </summary>
        /// <param name="objModel"></param>
        /// <param name="cfgInfo"></param>
        /// <returns></returns>
        public dynamic RemoteCall(dynamic objModel,  CfgInfo cfgInfo)
        {
            try
            {
                BusinessType bt = BusinessType.None;
                Enum.TryParse(cfgInfo.BusinessKind, out  bt);
                switch (bt)
                {
                    case BusinessType.PayerInfoQuery://支付人
                        GetNetbankSearchPayer((QueryPayerDetailModel)objModel, cfgInfo);
                        break;
                    case BusinessType.None://查询列表返回

                        break;
                    case BusinessType.MerchantQuery://商户订单支付查询(1120)
                        GetNetbankMerchantOrPayQuery((NetBankQueryMerchantOrPayModel)objModel, cfgInfo);
                                                                            
                        break;
                    case BusinessType.MarketPayQuery://市场订单支付查询（1320）
                        GetNetbankMerchantOrPayQuery((NetBankQueryMerchantOrPayModel)objModel, cfgInfo);//,BusinessType.MarketPayQuery
                        break;
                    case BusinessType.MarketTransClearQuery://市场订单结算查询(1350)
                        GetNetbankMarketSettlementQuery((NetBankQueryMarketSettlementModel)objModel, cfgInfo);
                        break;
                    case BusinessType.BankStatement://对账（目前银联对应1810）
                        GetNetbankBankStatementList((NetBankQueryStatementListModel)objModel,cfgInfo);
                        break;
                    case BusinessType.OPKind://查询操作类型
                        GetNetbankOpKind((QueryOpKindModel)objModel,cfgInfo);
                        break;
                    default:
                        break;
                }
                return objModel;
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(ex.Message, cfgInfo.BusinessKind + "银联调用");
                throw ex;
            }
        }
    }
}
