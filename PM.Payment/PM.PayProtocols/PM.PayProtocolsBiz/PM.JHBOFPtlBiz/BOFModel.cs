using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.JHBOFPtlBiz
{
    /// <summary>
    /// 交易查询请求对象
    /// </summary>
    public class BOFRequest
    {
        /// <summary>
        /// 交易类型
        /// </summary>
        public string TradeKind { get; set; }
        /// <summary>
        /// 交易账号
        /// </summary>
        public string AcountNo { get; set; }
        /// <summary>
        /// 开始日期(YYYYMMdd) 时间跨度不能大于30
        /// </summary>
        public string TradeBegDate { get; set; }
        /// <summary>
        /// 截止日期(YYYYMMdd)
        /// </summary>
        public string TradeEndDate { get; set; }
        /// <summary>
        /// ip
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
    }

    /// <summary>
    /// 响应返回
    /// </summary>
    public class BOFResponse
    {
        /// <summary>
        /// 响应码
        /// </summary>
        public string ResPonseCode { get; set; }
        /// <summary>
        /// 返回码描述
        /// </summary>
        public string ResPonseCodeDes
        {
            get
            {
                string rtn = string.Empty;
                switch (ResPonseCode)
                {
                    case "000000":
                        rtn = "成功";
                        break;
                    case "000001":
                        rtn = "账号错误";
                        break;
                    case "000002":
                        rtn = "超过规定查询日期";
                        break;
                    case "999999":
                        rtn = "系统错误";
                        break;
                    default:
                        rtn = "无返回信息";
                        break;
                }
                return rtn;
            }
        }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
    }

    /// <summary>
    /// 交易明细信息
    /// </summary>
    public class BOFModel
    {
        /// <summary>
        /// 交易日期
        /// </summary> 
        public string TradeDate { get; set; }
        /// <summary>
        /// 交易流水
        /// </summary> 
        public string TradeNo { get; set; }
        /// <summary>
        /// 金额    15位	 没有小数点"."，精确到分，最后两位为小数位，不足前补0。
        /// </summary> 
        public string Amount { get; set; }
        /// <summary>
        /// 摘要  60位	长度按需要是否够用(账号+标段编码+费用类型[可能需要])
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 对方账号
        /// </summary>
        public string PayAccountNO { get; set; }
        /// <summary>
        /// 对方账号户名
        /// </summary>
        public string PayAccountName { get; set; }

        /// <summary>
        /// 银行伪序列号
        /// </summary>
        public string CustomTradeNo { get; set; }
    }
}
