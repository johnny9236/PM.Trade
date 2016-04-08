using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.Netbank
{
    /// <summary>
    /// 支付请求
    /// </summary>
    public class NetBankPayRequestModel : CommunicationBase
    {
        /// <summary>
        /// 账户类型 个人11、企业12
        /// </summary>
        public string AccType { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
        public double Free { get; set; }
        /// <summary>
        /// 付款银行ID（代码）
        /// </summary>
        public string BankID { get; set; }
        ///// <summary>
        ///// 机构代码(默认是通过配置获取 如果配置中没有则此地获取)
        ///// </summary>
        //public string InstitutionID { get; set; }
        ///// <summary>
        ///// 通知地址(目前给转账使用)
        ///// </summary>
        //public string NotificationURL { get; set; }       
        /// <summary>
        /// 用途
        /// </summary>
        public string Usage { get; set; }
        /// <summary>
        /// 结算标示
        /// </summary>
        public string SettingAccNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }

    /// <summary>
    /// 支付、转账 结算 响应
    /// </summary>
    public class NetBankPayResponseModel:CommunicationBase
    {
        /// <summary>
        /// 签名信息
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 报文 供form提交
        /// </summary>
        public string MessagePaket { get; set; }
    }
}
