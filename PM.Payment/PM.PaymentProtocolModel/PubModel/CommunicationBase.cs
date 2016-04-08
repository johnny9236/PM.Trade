using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel
{
    /// <summary>
    /// 通信对象基类
    /// </summary>
    public class CommunicationBase
    {
        /// <summary>
        /// 业务号，系统用于指定实例化协议用
        /// </summary>
        public string BusinessFunNo { get; set; }

        /// <summary>
        /// 设定订单号(必要信息)
        /// 规则：
        /// 1、不重复
        /// 2、用于第三方或银行等通讯的标识，本系统中用于查询交易具体信息 
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 设定商户交易流水号
        /// 规则：
        /// 1、流水号系统中不能重复
        /// 2、在发生退款业务时，需要初始化本字段，用于表示当前退款业务的唯一编号(前面订单号用于发生退款的那笔业务的编号)
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public string OprationerID { get; set; }
        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string OprationerName { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public string TradeDate { get; set; }
        ///// <summary>
        ///// 备注
        ///// </summary>
        //public string Remark { get; set; }
        ///// <summary>
        ///// 协议类型(预留)
        ///// </summary>
        //public ProtocolsWay ProtocolsWay { get; set; }
        ///// <summary>
        ///// 操作类型  支付还是查询(预留)
        ///// </summary>
        //public OprationType OprationType { get; set; }
        ///// <summary>
        ///// 业务功能类型(预留)
        ///// </summary>
        //public string BusinessKind { get; set; }
        ///// <summary>
        ///// 系统编号
        ///// </summary>
        //public string SysCode { get; set; }
        /// <summary>
        /// 机构代码
        /// </summary>
        public string InstitutionID { get; set; }


    }
}
