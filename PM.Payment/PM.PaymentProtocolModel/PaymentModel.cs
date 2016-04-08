//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace PM.PayModel
//{  
//    /// <summary>
//    /// 支付对象
//    /// </summary>
//    public class PaymentModel : CommunicationBase
//    {
//          /// <summary>
//        /// 商品信息
//        /// </summary>
//        public TradeProduct TradeProduct { get; set; }
//        /// <summary>
//        /// 交易明细
//        /// </summary>
//        public TradeDetail TradeDetail { get; set; }
//        ///// <summary>
//        ///// 辅助信息
//        ///// </summary>
//        //public Perpare Perpare { get; set; }
//        ///// <summary>
//        ///// 返回结果
//        ///// </summary>
//        //public ResultInfo Result { get; set; }
//    }


///// <summary>
///// 商品信息
///// </summary>
//public class TradeProduct
//{
//    /// <summary>
//    /// 商品类型
//    /// </summary>
//    public string ProductType { get; set; }
//    /// <summary>
//    /// 交易信息名称
//    /// </summary>
//    public string TradeName { get; set; }

//    /// <summary>
//    /// 主键id(用于标示主键)
//    /// </summary>
//    public string PrimaryID { get; set; }
//    /// <summary>
//    /// 主键编码
//    /// </summary>
//    public string PrimaryCode { get; set; }
//    /// <summary>
//    /// 主键名称
//    /// </summary>
//    public string PrimaryName { get; set; }
//    /// <summary>
//    /// 从键id
//    /// </summary>
//    public string SlaveID { get; set; }
//    /// <summary>
//    /// 从键编码
//    /// </summary>
//    public string SlaveCode { get; set; }
//    /// <summary>
//    /// 从键名称
//    /// </summary>
//    public string SlaveName { get; set; }
//    /// <summary>
//    /// 用途
//    /// </summary>
//    public string Usage { get; set; }
//    /// <summary>
//    /// 备注信息
//    /// </summary>
//    public string Remarks { get; set; }

//    /// <summary>
//    /// 通知给第三方的URL
//    /// </summary>
//    public string NotificationURL { get; set; }
//    /// <summary>
//    /// 信息 
//    /// </summary>
//    public string MSG { get; set; }
//    /// <summary>
//    /// 后台通知地址
//    /// </summary>
//    public string NotificationBgURL { get; set; }
//    /// <summary>
//    /// 签名信息
//    /// </summary>
//    public string Signature { get; set; }
//    /// <summary>
//    /// 报文
//    /// </summary>
//    public string MessagePaket { get; set; }
//    /// <summary>
//    /// 报文(辅助)
//    /// </summary>
//    public string PlanText { get; set; }
//}
///// <summary>
///// 辅助对象(用于需要特殊传递的信息)
///// </summary>
//public class Perpare
//{
//    /// <summary>
//    /// 通知给第三方的URL
//    /// </summary>
//    public string NotificationURL { get; set; }
//    /// <summary>
//    /// 返回信息(成功或错误信息)
//    /// </summary>
//    public string MSG { get; set; }
//    /// <summary>
//    /// 后台通知地址
//    /// </summary>
//    public string NotificationBgURL { get; set; }
//    /// <summary>
//    /// 签名信息
//    /// </summary>
//    public string Signature { get; set; }
//    /// <summary>
//    /// 报文
//    /// </summary>
//    public string MessagePaket { get; set; }
//    /// <summary>
//    /// 报文(辅助)
//    /// </summary>
//    public string PlanText { get; set; }



//    #region  支付能通过文件等获取信息
//    /// <summary>
//    /// ip
//    /// </summary>
//    public string IP { get; set; }
//    /// <summary>
//    /// 端口
//    /// </summary>
//    public int Port { get; set; }
//    /// <summary>
//    /// 文件根目录(或者ftp地址)
//    /// </summary>
//    public string RootFilePath { get; set; }
//    /// <summary>
//    /// ftp用户名
//    /// </summary>
//    public string FtpUserName { get; set; }
//    /// <summary>
//    /// ftp密码
//    /// </summary>
//    public string FtpUserPwd { get; set; }
//    /// <summary>
//    /// 文件类型
//    /// </summary>
//    public FileTp FileType { get; set; }
//    #endregion
//}

///// <summary>
///// 交易明细
///// </summary>
//public class TradeDetail
//{
//    /// <summary>
//    /// 支付人账号信息
//    /// 备注：要求有付款人账号信息，实际应用时在代付或者转账结算时，会实际应用到银行协议中
//    /// </summary>
//    public TradeAccountDetail PayAccTrade { get; set; }
//    /// <summary>
//    /// 多个收款信息（代付结算等支付到多个账号使用） 一个情况说明是支付
//    /// </summary>
//    public List<TradeAccountDetail> AccTradeList { get; set; }

//    /// <summary>
//    /// 设定交易金额(必要信息)  总金额
//    /// </summary>
//    public double Amount { get; set; }
//    /// <summary>
//    /// 手续费  
//    /// </summary>
//    public double Free { get; set; }
//    /// <summary>
//    /// 交易明细时间
//    /// </summary>
//    public DateTime TradeTm { get; set; }
//    /// <summary>
//    /// 备注信息
//    /// </summary>
//    public string Remarks { get; set; }
//}
//}
