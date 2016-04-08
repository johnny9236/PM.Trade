/*----------------------------------------------------
 * 说明：黄石保证金交易查询接口 报文实体文件 不含配置中已有字段
 *       包含请求报文实体、返回响应报文实体
 * 作者：梁亮
 * 时间：2013-8-7
 * 
 -----------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.HuangShi
{
    /// <summary>
    /// 黄石保证金交易查询接口
    /// </summary>
    public class HuangShiDepositQueryModel:CommunicationBase
    {
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
        /// 对方来账户账号
        /// </summary>
        public string OTHER_ACCOUNT { get; set; }
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
        /// 金额
        /// </summary>
        public decimal MONEY { get; set; }
        /// <summary>
        /// 定位串 从返回报文里获取，如果不分页，可传空。第一次查询为空。
        /// </summary>
        public string INDEX_STRING { get; set; }
    }

    #region 黄石保证金交易查询接口 返回响应报文实体

    /// <summary>
    /// 黄石保证金交易查询接口 返回响应报文实体
    /// </summary>
    public class HuangShiDepositResponseModel
    {
        //header
        /// <summary>
        /// 请求序列号 同请求报文中的序列号	
        /// </summary>
        public string REQUEST_SN { get; set; }
        /// <summary>
        /// 商户号 同请求报文中的商户号
        /// </summary>
        public string CUST_ID { get; set; }
        /// <summary>
        /// 交易码 同请求报文中的交易码
        /// </summary>
        public string TX_CODE { get; set; }
        /// <summary>
        /// 响应码 交易响应码
        /// </summary>
        public string RETURN_CODE { get; set; }
        /// <summary>
        /// 响应信息 交易响应信息
        /// </summary>
        public string RETURN_MSG { get; set; }
        /// <summary>
        /// 语言 CN，同请求报文
        /// </summary>
        public string LANGUAGE { get; set; }

        //------------------body-------------
        /// <summary>
        /// 总页次
        /// </summary>
        public int TPAGE { get; set; }
        /// <summary>
        /// 当前页次
        /// </summary>
        public int CUR_PAG { get; set; }
        /// <summary>
        /// 定位串
        /// </summary>
        public string INDEX_STRING { get; set; }
        /// <summary>
        /// 提示
        /// </summary>
        public string NOTICE { get; set; }

        //list
        /// <summary>
        /// 交易记录明细实体集合
        /// </summary>
        public List<HuangShiDepositResponseListModel> ResponseListModel;
    }

    #endregion

    #region 返回响应报文实体的记录列表实体

    /// <summary>
    /// 黄石保证金交易查询接口 返回响应报文实体 交易记录明细实体
    /// </summary>
    public class HuangShiDepositResponseListModel
    {
        /// <summary>
        /// 记账日期 YYYY/MM/DD	
        /// </summary>
        public string TRAN_DATE { get; set; }
        /// <summary>
        /// 记账时间 HH:MM:SS
        /// </summary>
        public string TRAN_TIME { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string TRAN_SQ { get; set; }
        /// <summary>
        /// 商户保证金结算账户号
        /// </summary>
        public string ACCOUNT { get; set; }
        /// <summary>
        /// 商户保证金结算户名
        /// </summary>
        public string ACCOUNT_NAME { get; set; }
        /// <summary>
        /// 转账金额
        /// </summary>
        public decimal MONEY { get; set; }
        /// <summary>
        /// 对方账户账号
        /// </summary>
        public string OTHER_ACCOUNT { get; set; }
        /// <summary>
        /// 对方账户名称
        /// </summary>
        public string OTHER_ACCOUNT_NAME { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string ORDER { get; set; }

    }

    #endregion
}
