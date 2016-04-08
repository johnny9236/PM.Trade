using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.ALiPtlBiz.Ali
{
    public class AliConfig
    {
        #region 字段
        public string Partner { get; set; }
        //支付宝网关地址（新）
        public string GATEWAY_NEW { get; set; }
        //商户的私钥
        public string Key { get; set; }
        //编码格式
        public string Input_charset { get; set; }
        //签名方式
        public string Sign_type { get; set; }
        /// <summary>
        /// 支付宝返回验证地址
        /// </summary>
        public string VerifyUrl { get; set; }
        #endregion
    }
}
