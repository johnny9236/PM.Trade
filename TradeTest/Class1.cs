using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace TradeTest
{
    /// <summary>
    ///出入账明细(一次最多50条)
    /// </summary>
    [Table("T_BOC")]
    public class BOCQueryAccountDtlModel
    {

        public int ID { get; set; }
        /// <summary>
        /// 交易返回码  B001表示处理成功
        /// </summary> 
        public string RspCod { get; set; }
        /// <summary>
        ///  rspmsg表示解释信息
        /// </summary>
        public string RspMsg { get; set; }
        #region  付款
        /// <summary>
        /// 付款行号
        /// </summary>
        public string PayIbkNum { get; set; }
        /// <summary>
        /// 付款人开户行名
        /// </summary>
        public string PayIbkName { get; set; }
        /// <summary>
        /// 付款账号
        /// </summary>
        public string PayActacn { get; set; }
        /// <summary>
        /// 付款人
        /// </summary>
        public string PayAcntName { get; set; }
        #endregion
        #region  收款
        /// <summary>
        /// 收款行号
        /// </summary>
        public string ReciveToIbkn { get; set; }
        /// <summary>
        /// 收款账号
        /// </summary>
        public string ReciveActacn { get; set; }
        /// <summary>
        /// 收款人
        /// </summary>
        public string ReciveToName { get; set; }
        /// <summary>
        /// 收款人开户行名
        /// </summary>
        public string ReciveToBank { get; set; }
        #endregion
        /// <summary>
        /// 被代理行号
        /// </summary>
        public string MactIbkn { get; set; }
        /// <summary>
        /// 被代理账号
        /// </summary>
        public string Mactacn { get; set; }
        /// <summary>
        /// 被代理账户名
        /// </summary>
        public string Mactname { get; set; }
        /// <summary>
        /// 被代理账户开户行名
        /// </summary>
        public string MactBank { get; set; }
        /// <summary>
        /// 旧线是10位的凭证号或传票号，新线是9位流水号(不补0)+3位记录号
        /// </summary>
        public string Vchnum { get; set; }
        /// <summary>
        /// 记录标识号(9位JournalNumber+9位RecordNum(原)+9位RecordNum)
        /// </summary>
        public string TransId { get; set; }
        /// <summary>
        /// 交易日期 YYYYMMDD（非空）
        /// </summary>
        public string TxnDate { get; set; }
        /// <summary>
        /// 交易时间 HH24MISS
        /// </summary>
        public string TxnTime { get; set; }
        /// <summary>
        /// 	金额（非空）
        /// </summary>
        public string TxNamt { get; set; }
        /// <summary>
        /// 交易后余额
        /// </summary>
        public string Acctbal { get; set; }
        /// <summary>
        /// 可用余额
        /// </summary>
        public string Avlbal { get; set; }
        /// <summary>
        /// 冻结金额
        /// </summary>
        public string FrzAmt { get; set; }
        /// <summary>
        /// 	透支额度
        /// </summary>
        public string OverdrAmt { get; set; }
        /// <summary>
        /// 可用透支额度
        /// </summary>
        public string AvloverdrAmt { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        public string UseInfo { get; set; }
        /// <summary>
        /// 附言 (网银行内交易：OBSS+交易流水号后12位+GIRO+客户业务编号后12位
        /// 用途；网银跨行交易：OBSS+交易流水号后12位+GIRO+客户业务编号后12位用途//用途，)
        /// </summary>
        public string FurInfo { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string TransType { get; set; }
        /// <summary>
        /// 新业务类型
        /// </summary>
        public string BusType { get; set; }
        /// <summary>
        /// 货币名称（非空、如CNY或者001）
        /// </summary>
        public string TrnCur { get; set; }
        /// <summary>
        /// 	来往账标识（1-来账，2-往账）
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// 费用账户:收费交易通过一笔单独的交易来展示,所以该项返回空      
        /// </summary>
        public string FeeAct { get; set; }
        /// <summary>
        /// 费用金额 :收费交易通过一笔单独的交易来展示,所以该项返回空   
        /// </summary>
        public string FeeAmt { get; set; }
        /// <summary>
        /// 费用货币:收费交易通过一笔单独的交易来展示,所以该项返回空   
        /// </summary>
        public string FeeCur { get; set; }
        /// <summary>
        /// 起息日期YYYYMMDD
        /// </summary>
        public string ValDat { get; set; }
        /// <summary>
        /// 凭证类型，具体解释见附件5
        /// </summary>
        public string VouchTp { get; set; }
        /// <summary>
        /// 凭证号码(唯一值入库)
        /// </summary>
        public string VouchNum { get; set; }
        /// <summary>
        /// 汇率
        /// </summary>
        public string FxRate { get; set; }
        /// <summary>
        /// 整合信息，格式为：F:附言//A:摘要//U:用途//R:备注
        /// </summary>
        public string InterInfo { get; set; }
        /// <summary>
        /// 预留项
        /// </summary>
        public string Reserve1 { get; set; }
        /// <summary>
        /// 预留项
        /// </summary>
        public string Reserve2 { get; set; }
        /// <summary>
        /// 预留项
        /// </summary>
        public string Reserve3 { get; set; }
    }



}
