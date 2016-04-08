using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PM.PaymentModel
{
    /// <summary>
    /// 业务系统发起支付请求信息
    /// </summary> 
    [DataContract]
    [KnownType(typeof(PayStartModel))]
    public class PayStartModel
    {
        /// <summary>
        ///机构编码
        /// </summary>
        [DataMember]
        public string InstitutionID { get; set; }
        /// <summary>
        /// 业务关联ID 
        /// </summary>
        [DataMember]
        public string BusnissID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        [DataMember]
        public string OrderNo { get; set; }
        /// <summary>
        /// 交易信息用途
        /// </summary>
        [DataMember]
        public string TradeInfo { get; set; }
        /// <summary>
        /// 支付金额(含税)
        /// </summary>
        [DataMember]
        public decimal PayMoney { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
            [DataMember]
        public decimal PayFee { get; set; }
        /// <summary>
        /// 税额
        /// </summary>
            [DataMember]
        public decimal RateMoney { get; set; }
        /// <summary>
        /// 支付人ID
        /// </summary>
        [DataMember]
        public string PayerID { get; set; }
        /// <summary>
        /// 支付人名称
        /// </summary>
        [DataMember]
        public string PayerName { get; set; }
        #region   付款信息
        /// <summary>
        /// 付款银行id
        /// </summary>
        [DataMember]
        public string PayBankID { get; set; }
        /// <summary>
        /// 付款账号名称
        /// </summary>
        [DataMember]
        public string PayAcountName { get; set; }
        /// <summary>
        ///付款 账号
        /// </summary>
        [DataMember]
        public string PayAcountNo { get; set; }
        /// <summary>
        /// 付款开户行
        /// </summary>
        [DataMember]
        public string PayAccountDbBank { get; set; }
        /// <summary>
        /// 付款行账号类型   11 为个人 12为企业
        /// </summary>
        [DataMember]
        public string PayBankAccountType { get; set; }
        /// <summary>
        /// 付款结算代码
        /// </summary>
        [DataMember]
        public string PaySettingAccNo { get; set; }
        /// <summary>
        /// 付款行号
        /// </summary>
        [DataMember]
        public string PayOpenBankNo { get; set; }

        /// <summary>
        /// 付款省
        /// </summary>
        [DataMember]
        public string PayProvince { get; set; }
        /// <summary>
        /// 付款 市
        /// </summary>
        [DataMember]
        public string PayCity { get; set; }
        /// <summary>
        /// 付款币种
        /// </summary>
        [DataMember]
        public string PayCur { get; set; }

        #endregion

        #region  收款信息
        /// <summary>
        ///收款银行id
        /// </summary>
        [DataMember]
        public string ReceiptBankID { get; set; }
        /// <summary>
        /// 收款账号名称
        /// </summary>
        [DataMember]
        public string ReceiptAcountName { get; set; }
        /// <summary>
        ///收款 账号
        /// </summary>
        [DataMember]
        public string ReceiptAcountNo { get; set; }
        /// <summary>
        /// 收款开户行
        /// </summary>
        [DataMember]
        public string ReceiptAccountDbBank { get; set; }
        /// <summary>
        /// 收款行账号类型   1 为个人 2为企业
        /// </summary>
        [DataMember]
        public string ReceiptBankAccountType { get; set; }
        /// <summary>
        /// 收款结算代码
        /// </summary>
        [DataMember]
        public string ReceiptSettingAccNo { get; set; }
        /// <summary>
        /// 收款行号
        /// </summary>
        [DataMember]
        public string ReceiptOpenBankNo { get; set; }

        /// <summary>
        /// 收款省
        /// </summary>
        [DataMember]
        public string ReceiptProvince { get; set; }
        /// <summary>
        /// 收款 市
        /// </summary>
        [DataMember]
        public string ReceiptCity { get; set; }
        /// <summary>
        /// 收款币种
        /// </summary>
        [DataMember]
        public string ReceiptCur { get; set; }
        #endregion
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }
        /// <summary>
        /// 业务功能号(用于导向到具体的协议及功能）
        /// </summary>
        [DataMember]
        public string BusinessFunNo { get; set; }
        #region
        /// <summary>
        /// 到账日期
        /// </summary>
           [DataMember]
        public string InDate { get; set; }
        /// <summary>
        /// 到账时间
        /// </summary>
           [DataMember]
        public string InTime { get; set; }

        /// <summary>
        /// 结果  
        /// </summary>
           [DataMember]
        public bool Result { get; set; }
        #endregion
    }
}
