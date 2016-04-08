using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.LPSBBC
{
    /// <summary>
    /// b2c 支付
    /// </summary>
    public class BBCB2CPay : BBCBase
    {
        /// <summary>
        /// 网关类型
        /// </summary>
        public string GATEWAY { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string CLIENTIP { get; set; }
        /// <summary>
        /// 支付报文对象并提交
        /// </summary>
        /// <returns></returns>
        public string SendPost()
        {
            string rtnStr = string.Empty;

            return rtnStr;
        }
    }
    /// <summary>
    /// 网关类型 目前不使用
    /// </summary>
    public enum GateWayType
    {
        /// <summary>
        /// 仅显示帐号支付标签
        /// </summary>
        W0Z1,
        /// <summary>
        /// 仅显示帐号支付标签
        /// </summary>
        W0Z2,
        /// <summary>
        /// 仅显示网银客户支付标签
        /// </summary>
        W1Z0,
        /// <summary>
        /// 仅显示网银客户支付标签
        /// </summary>
        W2Z0,
        /// <summary>
        /// 仅显示网银客户支付页签
        /// </summary>
        W1,
        /// <summary>
        /// 仅显示帐号支付页签
        /// </summary>
        Z1,
        /// <summary>
        /// 仅显示手机银行客户支付页签
        /// </summary>
        S1,
        /// <summary>
        /// 显示网银客户支付和帐号支付，选中网银客户支付页签
        /// </summary>
        W2Z1,
        /// <summary>
        /// 显示网银客户支付和帐号支付，选中帐号支付页签
        /// </summary>
        W1Z2,
        /// <summary>
        /// 显示网银客户支付和帐号支付，选中客户上次使用的支付页签
        /// </summary>
        W1Z1,
        /// <summary>
        /// 显示网银客户支付和手机银行客户支付页签，选中网银客户支付页签
        /// </summary>
        W2S1,
        /// <summary>
        /// 显示网银客户支付和手机银行客户支付页签，选中手机银行客户支付页签
        /// </summary>
        W1S2,
        /// <summary>
        /// 示网银客户支付和手机银行客户支付页签，选中客户上次使用的支付页签
        /// </summary>
        W1S1,
        /// <summary>
        /// 显示帐号支付和手机银行客户支付页签，选中帐号支付页签
        /// </summary>
        Z2S1,
        /// <summary>
        /// 显示帐号支付和手机银行客户支付页签，选中手机银行客户支付页签
        /// </summary>
        Z1S2,
        /// <summary>
        /// 显示帐号支付和手机银行客户支付页签，选中客户上次使用的支付页签
        /// </summary>
        Z1S1,
        /// <summary>
        /// 三个页签均显示，选中网银客户支付页签
        /// </summary>
        W2Z1S1,
        /// <summary>
        /// 三个页签均显示，选中帐号支付页签
        /// </summary>
        W1Z2S1,
        /// <summary>
        /// 三个页签均显示，选中手机银行客户支付页签
        /// </summary>
        W1Z1S2,
        /// <summary>
        /// 三个页签均显示，选中客户上次使用的支付页签
        /// </summary>
        W1Z1S1,
        /// <summary>
        /// 显示网银客户支付和帐号支付，选中客户上次使用的支付页签
        /// </summary>
        Other
    }

}
