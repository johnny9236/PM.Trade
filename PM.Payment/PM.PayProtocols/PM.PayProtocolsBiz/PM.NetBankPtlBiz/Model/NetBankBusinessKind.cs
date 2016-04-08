using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.NetBankPtlBiz.Model
{
    /// <summary>
    /// 业务类型对应业务功能号
    /// </summary>
    public enum NetBankBusinessKind
    {
        /// <summary>
        /// 网银支付请求（商户订单）
        /// </summary>
        NetBankPay,
        /// <summary>
        /// 网银转账请求(市场订单)
        /// </summary>
        NetBankTransPay,
        /// <summary>
        /// 网银通知支付
        /// </summary>
        NetbankPayCallBack,
        /// <summary>
        /// 网银转账通知
        /// </summary>
        NetbankTransCallBack,
        /// <summary>
        /// 网银转账结算请求
        /// </summary>
        NetbankTransClear,
        /// <summary>
        /// 网银转账结算通知
        /// </summary>
        NetbankTransClearCallBack,
        /// <summary>
        /// 网银批量代付
        /// </summary>
        NetbankBatchStayPays,
        /// <summary>
        /// 网银操作类型(用于查询)
        /// </summary>
        NetbankOpkind,
        /// <summary>
        /// 查询支付人信息
        /// </summary>
        NetbankSearchPayer,
        /// <summary>
        /// 商户订单支付查询(1120)
        /// </summary>
        NetbankMerchantQuery,
        /// <summary>
        /// 市场订单支付查询（1320）
        /// </summary>
        NetbankMarketPayQuery,
        /// <summary>
        /// 市场订单结算查询(1350)
        /// </summary>
        NetbankMarketTransClearQuery,
        /// <summary>
        /// 对账单(1810) 时间格式YYYY-MM-DD
        /// </summary>
        NetbankBankStatement,
        /// <summary>
        /// 查询列表
        /// </summary>
        QueryList
    }
}
