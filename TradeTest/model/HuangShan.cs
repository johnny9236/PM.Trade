using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeTest.model
{
    /// <summary>
    /// 明细记录
    /// </summary>
    //[Table("T_HuangShan_AcctDtl")]
    //public class ICBCQueryInfo
    //{
    //    /// <summary>
    //    /// 编码标识
    //    /// </summary>
    //    public int ID { get; set; }
    //    /// <summary>
    //    /// 标段编号
    //    /// </summary>
    //    public string SectionCode { get; set; }
    //    /// <summary>
    //    /// 授权码
    //    /// </summary>
    //    public string AuthCode { get; set; }

    //    /// <summary>
    //    /// 类型 业务类型 0保证金入账明细  1退款明细
    //    /// </summary>
    //    public string QYType { get; set; }
    //    /// <summary>
    //    /// 到账日期
    //    /// </summary>
    //    public string InDate { get; set; }
    //    /// <summary>
    //    /// 到账时间
    //    /// </summary>
    //    public string InTime { get; set; }
    //    /// <summary>
    //    /// 到账金额
    //    /// </summary>
    //    public string InAmount { get; set; }
    //    /// <summary>
    //    /// 付款人户名
    //    /// </summary>
    //    public string InName { get; set; }
    //    /// <summary>
    //    /// 付款人账号
    //    /// </summary>
    //    public string InAcct { get; set; }
    //    /// <summary>
    //    /// 收款账号
    //    /// </summary>
    //    public string InMemo { get; set; }
    //    /// <summary>
    //    /// 交易流水号
    //    /// </summary>
    //    public string HstSeqNum { get; set; }
    //    /// <summary>
    //    /// 当前利息
    //    /// </summary>
    //    public string PunInst { get; set; }
    //    /// <summary>
    //    /// 是否基本户 0否；1是， 默认1
    //    /// </summary>
    //    public string Gernal { get; set; }
    //    /// <summary>
    //    /// 是否退款
    //    /// </summary>
    //    public string Result { get; set; }
    //    /// <summary>
    //    /// 是否基本户 0否；1是， 默认1
    //    /// </summary>
    //    public string AddWord { get; set; }


    //    /// <summary>
    //    /// 是否匹配  1匹配
    //    /// </summary>
    //    public int Match { get; set; }
    //    /// <summary>
    //    /// 是否处理  1 处理
    //    /// </summary>
    //    public int Flag { get; set; }
    //    /// <summary>
    //    /// 备注
    //    /// </summary>
    //    public string Remark { get; set; }
    //    /// <summary>
    //    /// 创建时间
    //    /// </summary>
    //    public DateTime? CreateTm { get; set; }
    //    /// <summary>
    //    /// 更新时间
    //    /// </summary>
    //    public DateTime? UpdateTm { get; set; }
    //}


    /// <summary>
    /// 明细记录
    /// </summary>
    [Table("T_ICBCRtn")]
    public class ICBCRtnQueryInfo
    {
        /// <summary>
        /// 编码标识
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 标段编号
        /// </summary>
        public string SectionCode { get; set; }
        /// <summary>
        /// 银行
        /// </summary>
        public string BankType { get; set; }
        /// <summary>
        /// 授权码
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// 类型 业务类型 0保证金入账明细  1退款明细
        /// </summary>
        public string BusniessType { get; set; }
        /// <summary>
        /// 退还日期
        /// </summary>
        public string RetDate { get; set; }
        /// <summary>
        /// 退还时间
        /// </summary>
        public string RetTime { get; set; }
        /// <summary>
        /// 退还本金
        /// </summary>
        public string RetAmount { get; set; }
        /// <summary>
        /// 退还利息
        /// </summary>
        public string RetPunInst { get; set; }
        /// <summary>
        /// 退还本利和
        /// </summary>
        public string RetTotal { get; set; }
        /// <summary>
        /// 收款人户名
        /// </summary>
        public string RetName { get; set; }
        /// <summary>
        /// 收款人账号
        /// </summary>
        public string RetAcct { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string HstSeqNum { get; set; }
        /// <summary>
        /// 母账号
        /// </summary>
        public string AcctNo { get; set; }
        /// <summary>
        /// 现金管理平台流水
        /// </summary>
        public string Serial_No { get; set; }

        /// <summary>
        /// 是否匹配  1匹配
        /// </summary>
        public int Match { get; set; }
        /// <summary>
        /// 是否处理  1 处理
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTm { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTm { get; set; }
    }
}
