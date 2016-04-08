using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PM.Utils;
using PM.Utils.Log;

namespace PM.PaymentProtocolModel.BankCommModel.AHQY
{
    /// <summary>
    /// 退款明细查询
    /// </summary>
    public class QYBBCQueryRtnResultModel : BBCQueryRtnResultModel
    {
        /// <summary>
        /// 交易代码
        /// </summary>
        public override string TransCode
        {
            get
            {
                return base.TransCode;
            }
            set
            {
                base.TransCode = value;
            }
        }
        /// <summary>
        /// 获取报文
        /// </summary>
        /// <param name="packetString"></param>
        /// <returns></returns>
        public override bool GetModel(string packetString)
        {
            return base.GetModel(packetString);
        }
    }
    ///// <summary>
    ///// 明细记录
    ///// </summary>
    //public class QYBBCRtnQueryInfo
    //{
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
    //    /// 是否退款
    //    /// </summary>
    //    public string Result { get; set; }
    //    /// <summary>
    //    /// 是否基本户 0否；1是， 默认1
    //    /// </summary>
    //    public string AddWord { get; set; }
    //}
}
