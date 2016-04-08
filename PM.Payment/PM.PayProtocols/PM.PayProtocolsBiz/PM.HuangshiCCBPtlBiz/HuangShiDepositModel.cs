/*----------------------------------------------------
 * 说明：黄石保证金交易查询接口 报文实体文件
 *       包含请求报文实体、返回响应报文实体
 * 作者：梁亮
 * 时间：2013-8-7
 * 
 -----------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.HuangshiCCBPtlBiz
{
    #region 黄石保证金交易查询接口 请求报文实体
    
    /// <summary>
    /// 黄石保证金交易查询接口 请求报文实体
    /// </summary>
    public class HuangShiDepositRequestModel
    {
        //-----------header----------------
        /// <summary>
        /// 请求序列号
        /// </summary>
        public string REQUEST_SN { get; set; }
        /// <summary>
        /// 商户代码
        /// </summary>
        public string CUST_ID { get; set; }
        /// <summary>
        /// 操作员号
        /// </summary>
        public string USER_ID { get; set; }
        /// <summary>
        /// 操作员密码
        /// </summary>
        public string PASSWORD { get; set; }
        /// <summary>
        /// 交易请求码
        /// </summary>
        public string TX_CODE { get; set; }
        /// <summary>
        /// 语言 CN
        /// </summary>
        public string LANGUAGE { get; set; }

        //-------------------body---------------
        /// <summary>
        /// 商户的企业网银签约客户号
        /// </summary>
        public string CUST_ON { get; set; }
        /// <summary>
        /// 商户保证金结算账户号
        /// </summary>
        public string ACCOUNT { get; set; }
        /// <summary>
        /// 起始日期 YYYYMMDD
        /// </summary>
        public string START { get; set; }
        /// <summary>
        /// 截止日期 YYYYMMDD
        /// </summary>
        public string END { get; set; }
        /// <summary>
        /// 当前页次
        /// </summary>
        public int PAGE { get; set; }
        /// <summary>
        /// 对方来账户账号
        /// </summary>
        public string OTHER_ACCOUNT { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal MONEY { get; set; }
        /// <summary>
        /// 摘要 此处填写订单号
        /// </summary>
        public string ORDER { get; set; }
        /// <summary>
        /// 定位串 从返回报文里获取，如果不分页，可传空。第一次查询为空。
        /// </summary>
        public string INDEX_STRING { get; set; }
      
    }

    #endregion

    
}
