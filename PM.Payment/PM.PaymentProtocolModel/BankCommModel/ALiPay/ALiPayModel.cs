using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.ALiPay
{
    /// <summary>
    /// 阿里支付请求对象
    /// </summary>
    public class ALiPayModel : CommunicationBase
    {
        /// <summary>
        /// 合作身份者ID
        /// </summary>
        public string Partner { get; set; }
        /// <summary>
        /// 签约支付宝账号或卖家支付宝帐户
        /// </summary>
        public string Account { get; set; }
        ///// <summary>
        ///// 付完款后跳转的页面 要用 以http开头格式的完整路径，不允许加?id=123这类自定义参数
        ///// </summary>
        //public string ReturnUrl { get; set; }
        ///// <summary>
        ///// 交易过程中服务器通知的页面
        ///// </summary>
        //public string NotifyUrl { get; set; }
        /// <summary>
        /// 网站商品的展示地址
        /// </summary>
        public string ShowUrl { get; set; }
        /// <summary>
        /// 订单名称，显示在支付宝收银台里的“商品名称”里，显示在支付宝的交易管理的“商品名称”的列表里
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 订单描述、订单详细、订单备注，显示在支付宝收银台里的“商品描述”里
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 订单总金额，显示在支付宝收银台里的“应付总额”里
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 默认支付方式，四个值可选：bankPay(网银); cartoon(卡通); directPay(余额); CASH(网点支付)
        /// 默认directPay(余额)
        /// </summary>
        public string Paymethod { get; set; }

        /// <summary>
        /// 默认网银代号
        /// </summary>
        public string DefaultBank { get; set; }
        /// <summary>
        /// 防钓鱼时间戳
        /// </summary>
        public string AntiPhishingKey { get; set; }
        /// <summary>
        /// 买家本地电脑的IP地址
        /// </summary>
        public string ExterInvokeIp { get; set; }
        /// <summary>
        /// 自定义参数，可存放任何内容（除等特殊字符外），不会显示在页面上
        /// </summary>
        public string ExtraCommonParam { get; set; }
        /// <summary>
        /// 默认买家支付宝账号
        /// </summary>
        public string DefaultBuyAccount { get; set; }
        /// <summary>
        /// 提成类型，该值为固定值：10，不需要修改
        /// </summary>
        public string RoyaltyType { get; set; }

        /// <summary>
        /// 提成信息集，与需要结合商户网站自身情况动态获取每笔交易的各分润收款账号、各分润金额、各分润说明
        /// </summary>
        public string RoyaltyParameters { get; set; }
        /// <summary>
        ///安全检验码
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 字符编码格式 目前支持 gbk 或 utf-8
        /// </summary>
        public string InputCharset { get; set; }
        /// <summary>
        /// 签名方式 不需修改
        /// </summary>
        public string SignType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 结果值(用于输出)
        /// </summary>
        public string  Result { get; set; }
    }
}
