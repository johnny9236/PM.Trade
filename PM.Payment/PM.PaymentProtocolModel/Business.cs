using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel
{
    //业务订单相关配置
    /// <summary>
    /// 支付结果 
    /// </summary>
    public enum PayFlag
    {
        /// <summary>
        /// 默认
        /// </summary>
        None = 0,
        /// <summary>
        /// 已提交
        /// </summary>
        Submit = 1,
        /// <summary>
        /// 支付成功
        /// </summary>
        PaySucess = 2,
        /// <summary>
        /// 支付失败
        /// </summary>
        PayFail = 3,
        /// <summary>
        /// 入账成功
        /// </summary>
        IncomeSucess = 4,
        /// <summary>
        /// 入账失败
        /// </summary>
        IncomeFail = 5,
        /// <summary>
        /// 未知
        /// </summary>
        UnKnow=9
    }
    /// <summary>
    /// 退还保证金标记 
    /// </summary>
    public enum PayReturnFlag
    {
        /// <summary>
        /// 默认
        /// </summary>
        None = 0,
        /// <summary>
        /// 已提交
        /// </summary>
        Submit = 1,
        /// <summary>
        /// 操作成功
        /// </summary>
        OprationSucess = 2,
        /// <summary>
        /// 操作失败
        /// </summary>
        OprationFaile = 3,
        /// <summary>
        /// 退还保证金成功
        /// </summary>
        Sucess = 4,
        /// <summary>
        /// 退还保证金失败
        /// </summary>
        Fail = 5,
        /// <summary>
        /// 未知
        /// </summary>
        UnKnow=9
    }
    /// <summary>
    /// 订单状态 
    /// </summary>
    public enum OrderFlag
    {
        /// <summary>
        /// 默认
        /// </summary>
        None = 0,
        /// <summary>
        /// 已提交
        /// </summary>
        Submit = 1,
        /// <summary>
        /// 成功
        /// </summary>
        Sucess = 2,
        /// <summary>
        /// 失败
        /// </summary>
        Faile = 3,
        /// <summary>
        /// 未知
        /// </summary>
        UnKnow=9

    }
    /// <summary>
    /// 处理结果返回 （用于银行服务器端返回）
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 失败
        /// </summary>
        Faile,
        /// <summary>
        /// 未知
        /// </summary>
        UnKnow

    }

    
}
