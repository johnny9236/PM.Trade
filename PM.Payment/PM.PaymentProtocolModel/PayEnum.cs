using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PM.PaymentProtocolModel
{
    /// <summary>
    /// 协议实现类型
    /// </summary>
    public enum ProtocolsWay
    {
        /// <summary>
        /// 无
        /// </summary> 
        [Description("无")]
        NULL,
        /// <summary>
        /// 嘉善农行
        /// </summary>
        [Description("嘉善农行")]
        JSABOC,
        /// <summary>
        /// 金华交行
        /// </summary>
        [Description("金华交行")]
        JHBOF,
        /// <summary>
        /// 银联
        /// </summary>
        [Description("银联")]
        NetBank,

        /// <summary>
        /// 阿里
        /// </summary>
        [Description("阿里")]
        ALI,
        /// <summary>
        /// 黄梅
        /// </summary>
        [Description("黄梅")]
        HuangMei,
        /// <summary>
        /// 黄石
        /// </summary>
        [Description("黄石")]
        HuangShi,
        /// <summary>
        /// 六盘水
        /// </summary>
        [Description("六盘水建行")]
        LPSBBC,
        /// <summary>
        /// 安徽青阳建行
        /// </summary>
        [Description("安徽青阳建行")]
        QYBBC,

        /// <summary>
        /// 黄山工行
        /// </summary>
        [Description("黄山工行")]
        HSICBC,
        /// <summary>
        /// 掇刀区农行
        /// </summary>
        [Description("掇刀区农行")]
        DDQABOC,
        /// <summary>
        /// 中国银行
        /// </summary>
        [Description("中国银行")]
        BOC
    }
    /// <summary>
    /// 业务类型
    /// </summary>
    public enum BusinessType
    {
        [Description("创建")]
        Create = 11,
        [Description("更新")]
        Update = 13,
        [Description("结束")]
        Finish = 999,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 15,
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,

        /// <summary>
        /// 支付(一般包含全部的)
        /// </summary>
        [Description("支付")]
        Pay = 1111,
        /// <summary>
        /// 支付不确认
        /// </summary>
        [Description("支付不确认")]
        UndeterminedPay = 1112,
        /// <summary>
        /// b2c支付  需要区分的情况下 
        /// </summary>
        [Description(" b2c支付")]
        PayB2C = 0001,
        /// <summary>
        /// 支付响应(默认包含全部)
        /// </summary>
        [Description("支付响应")]
        PayResponse = 1118,



        /// <summary>
        /// b2c支付响应
        /// </summary>
        [Description(" b2c支付响应")]
        PayB2CResponse = 0002,
        /// <summary>
        ///  退还保证金
        /// </summary>
        [Description(" 退还保证金")]
        Transfer = 1311,
        /// <summary>
        /// 退还保证金响应
        /// </summary>
        [Description("退还保证金响应")]
        TransferResponse = 1318,
        /// <summary>
        /// 转账(退还保证金结算）
        /// </summary>
        [Description("转账(退还保证金结算）")]
        TransferNotice = 1341,
        /// <summary>
        /// 转账结算通知(退还保证金结算）
        /// </summary>
        [Description("转账结算通知")]
        TransferClearNotice = 1348,
        /// <summary>
        /// 付款信息查询
        /// </summary>
        [Description("付款信息查询")]
        PayerInfoQuery = 1121,
        /// <summary>
        /// 商户订单支付查询
        /// </summary>
        [Description("商户订单支付查询")]
        MerchantQuery = 1120,
        /// <summary>
        /// 市场订单支付查询（1320）
        /// </summary>
        [Description(" 市场订单支付查询")]
        MarketPayQuery = 1320,
        /// <summary>
        /// 市场订单结算查询(1350)
        /// </summary>
        [Description(" 市场订单结算查询")]
        MarketTransClearQuery = 1350,
        /// <summary>
        /// 下载查询
        /// </summary>
        [Description("下载对账查询")]
        BankStatement = 1810,
        /// <summary>
        /// 批量代付
        /// </summary>
        [Description("批量代付")]
        BankBatchStayPays = 1510,
        /// <summary>
        /// 获取操作类型
        /// </summary>
        [Description("获取操作类型")]
        OPKind = 400
    }
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OprationType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        NULL = -1,
        /// <summary>
        /// 支付
        /// </summary>
        [Description("支付")]
        Pay = 0,
        ///// <summary>
        ///// 转账（特殊的支付类型）
        ///// </summary>
        //Trans = 1,
        /// <summary>
        /// 查询
        /// </summary>
        [Description("查询")]
        Search = 2,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 3
    }
    /// <summary>
    /// 动作类型  请求  、响应
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        NULL,
        /// <summary>
        /// 请求
        /// </summary>
        [Description("请求")]
        Request,
        /// <summary>
        /// 响应
        /// </summary>
        [Description("响应")]
        Response
    }
}
