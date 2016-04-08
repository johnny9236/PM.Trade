using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel
{
    #region 账号信息
    /// <summary>
    /// 交易账号信息
    /// </summary>
    public class TradeAccount
    {
        /// <summary>
        /// 账号      （必要信息）
        /// </summary>
        public string AccNo { get; set; }
        /// <summary>
        /// 账户名     
        /// </summary>
        public string AccDbName { get; set; }
        /// <summary>
        /// 账户开户行（必要信息）
        /// </summary>
        public string AccDBBank { get; set; }
        /// <summary>
        /// 账户开户行行号（必要信息）
        /// </summary>
        public string AccDBBankNo { get; set; }
        /// <summary>
        /// 银行ID（代码）
        /// </summary>
        public string BankID { get; set; }
        ///// <summary>
        ///// 支付银行类型(银行标识)
        ///// </summary>
        //public string BankName { get; set; }
        ///// <summary>
        ///// 结算标示(结算到那个账户)
        ///// </summary>
        //public string SettingAccNo { get; set; }
        ///// <summary>
        ///// 机构代码(备用)
        ///// </summary>
        //public string StructCode { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string AccPro { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string AccCity { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public string AccCur { get; set; }
        /// <summary>
        /// 账户类型 个人11、企业12
        /// </summary>
        public string AccType { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 人员ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        public string UserName { get; set; }

    }
    /// <summary>
    /// 对应交易账户明细
    /// </summary>
    public class TradeAccountDetail : TradeAccount
    {
        /// <summary>
        /// 金额(单笔等 目前用于结算等)
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// 手续费(单笔等 目前用于结算等)
        /// </summary>
        public double Free { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public string ItemNo { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 开户证件类型
        /// </summary>
        public string IdentificationType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdentificationNumber { get; set; }

        ///// <summary>
        ///// 交易日期    （YYYY/MM/DD）    
        ///// </summary>
        //public string TradeDate { get; set; }
        ///// <summary>
        ///// 交易时间    (HH:MM:SS)  
        ///// </summary>
        //public string TradeTime { get; set; }
        /// <summary>
        /// 交易明细时间
        /// </summary>
        public DateTime TradeTm { get; set; }
    }
    #endregion

    /// <summary>
    /// 返回结果信息
    /// </summary>
    public class ResultInfo : CommunicationBase
    {
        /// <summary>
        /// 第三方需要通知业务系统URL
        /// </summary>
        public string NotificationURL { get; set; }
        /// <summary>
        /// 提交地址(银行接口地址)
        /// </summary>
        public string ActionURLToBank { get; set; }
        /// <summary>
        /// 签名信息  供form提交
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 报文  供form提交
        /// </summary>
        public string MessagePaket { get; set; }
        /// <summary>
        /// 报文(辅助)  供form提交
        /// </summary>
        public string PlanText { get; set; }
        ///// <summary>
        ///// 返回url
        ///// </summary>
        //public string UrlStr { get; set; }
        /// <summary>
        /// 返回信息(成功或错误信息)
        /// </summary>
        public string MSG { get; set; } 
        /// <summary>
        /// 成功状态
        /// </summary>
        public ResultType Result { get; set; }
        /// <summary>
        /// 成功时间
        /// </summary> 
        public string SuccessTime { get; set; }
        /// <summary>
        /// 状态值
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusDes { get; set; }
        /// <summary>
        /// 供网银等第三方通知结果
        /// </summary>
        public string NoticeMsg { get; set; }

        /// <summary>
        /// 附加信息
        /// </summary>
        public string Attach { get; set; }

        #region 业务信息
        /// <summary>
        /// 业务编码
        /// </summary>
        public string TxCode { get; set; }
        /// <summary>
        /// 业务描述
        /// </summary>
        public string TxName { get; set; }
        #endregion
        #region  付款信息
        /// <summary>
        /// 付款信息
        /// </summary>
        public TradeAccountDetail TradeAccount { get; set; }
        #endregion
    }

}
