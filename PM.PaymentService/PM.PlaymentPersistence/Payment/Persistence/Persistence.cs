using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.WebUtils;
using System.Web;
using PM.Utils.Log;
using PM.PaymentProtocolModel;
using PM.PlaymentPersistence.ORM;
using PM.PaymentModel;

namespace PM.PlaymentPersistence.Payment.Persistence
{
    /// <summary>
    ///支付核心
    /// </summary>
    public abstract partial class Persistence
    {
        public Persistence()
        { }
        public Persistence(publicEntities entities)
        {
            Entities = entities;
        }
        /// <summary>
        /// 设置全局变量
        /// </summary>
        public publicEntities Entities
        {
            get;
            set;
        }
        /// <summary>
        /// 配置信息
        /// </summary>
        //public SysConfigModel Sys_ConfigModel { get; set; }
        public CfgInfo Cfg { get; set; }

        #region   统一响应
        ///// <summary>
        ///// 响应返回
        ///// </summary>
        ///// <param name="signature">签名</param>
        ///// <param name="message">西门新</param>
        ///// <param name="bkShow">是否输出显示</param>
        ///// <param name="showStr">输出显示内容</param>
        ///// <returns></returns>
        //public bool Response(string signature, string message, bool bkShow, out string showStr)
        //{
        /// <summary>
        ///    /// 响应返回
        /// </summary>
        /// <param name="resopnseModel">返回对象</param>
        /// <returns></returns>
        public string Response(PayResopnseModel resopnseModel)
        {
            bool rtn = false;
            var showStr = string.Empty;
            string showInfo = string.Empty;
            string orderNo = string.Empty;//订单号
            //ResultInfo info = null;//响应返回信息
            #region  获取请求返回对象信息
            T_Pay_OrderResponse or = new T_Pay_OrderResponse();
            or.ID = Guid.NewGuid();
            or.PacketMessage = resopnseModel.Message;
            or.Signature = resopnseModel.Signature;
            or.RequestTime = DateTime.Now;
            #endregion
            switch (GetOpKind(resopnseModel))
            {
                case "Pay":
                    //rtn = PayResopnseOpration(resopnseModel.Signature, resopnseModel.Message, resopnseModel.IsShowBk, out  showStr);
                    rtn = PayResopnseOpration(resopnseModel, out  showStr);
                    break;
                default:
                    break;
            }
            return showStr;
        }
        #endregion

        #region 辅助类
        /// <summary>
        /// 获取当前是那种操作类型
        /// </summary> 
        /// <param name="resopnseModel">报文对象</param> 
        /// <returns></returns>
        protected virtual string GetOpKind(PayResopnseModel resopnseModel)
        {
            return "Pay";
        }
        #endregion

        #region private方法
        /// <summary>
        /// 回调给业务系统
        /// </summary>
        /// <param name="order">订单</param>
        /// <param name="urlStr">回调地址</param>
        /// <param name="enCoding">编码</param>
        /// <param name="rtnCheckStr">回调页面返回（用于比对是否处理成功）</param>
        protected void PostBackToBusinesss(T_Pay_Order order, string urlStr, string enCoding, string rtnCheckStr)
        {
            string postBack = string.Empty;
            int i = 0;
            try
            {
                //var urlStr = ConfigHelper.GetConfigString("BusinessUrl");
                //var enCoding = ConfigHelper.GetConfigString("enCoding");
                var payRealAccountName = string.IsNullOrEmpty(order.PayRealAccountName) == true ? string.Empty : HttpUtility.UrlEncode(order.PayRealAccountName, System.Text.Encoding.GetEncoding(enCoding));
                var payRealAccountNo = string.IsNullOrEmpty(order.PayRealAccountNo) == true ? string.Empty : HttpUtility.UrlEncode(order.PayRealAccountNo, System.Text.Encoding.GetEncoding(enCoding));
                var payRealBankName = string.IsNullOrEmpty(order.PayRealBankName) == true ? string.Empty : HttpUtility.UrlEncode(order.PayRealBankName, System.Text.Encoding.GetEncoding(enCoding));
                var amount = order.Amount;
                var feeAmount = order.FeeAmount;
                var primaryID = string.IsNullOrEmpty(order.PrimaryID) == true ? string.Empty : HttpUtility.UrlEncode(order.PrimaryID, System.Text.Encoding.GetEncoding(enCoding));
                var slaveID = string.IsNullOrEmpty(order.SlaveID) == true ? string.Empty : HttpUtility.UrlEncode(order.SlaveID, System.Text.Encoding.GetEncoding(enCoding));
                var orderNo = string.IsNullOrEmpty(order.OrderNo) == true ? string.Empty : HttpUtility.UrlEncode(order.OrderNo, System.Text.Encoding.GetEncoding(enCoding));
                var orderSerialNumber = string.IsNullOrEmpty(order.OrderSerialNumber) == true ? string.Empty : HttpUtility.UrlEncode(order.OrderSerialNumber, System.Text.Encoding.GetEncoding(enCoding));
                var loanMark = 0;
                var contentStr = string.Format(@"PayRealAccountName={0}&PayRealAccountNo={1}&PayRealBankName={2}&Amount={3}&FeeAmount={4}&PrimaryID={5}&SlaveID={6}&TradeNo={7}&SerialNumber={8}&LoanMark={9}&CostType={10}"
                  , payRealAccountName
                  , payRealAccountNo
                  , payRealBankName
                  , amount
                  , feeAmount
                  , primaryID
                  , slaveID
                  , orderNo
                  , orderSerialNumber
                  , loanMark
                  , "QT"
                    );
                postBack = HttpTransfer.RequestPost(urlStr, contentStr, System.Text.Encoding.GetEncoding(enCoding));
                LogTxt.WriteEntry(string.Format("回调给业务系统:{0}{1}", urlStr, contentStr), "支付回调日志");
                while (postBack.ToLower() != rtnCheckStr.ToLower() && i < 3)
                {
                    i++;
                    postBack = HttpTransfer.RequestPost(urlStr, contentStr, System.Text.Encoding.GetEncoding(enCoding));
                    LogTxt.WriteEntry(string.Format("回调给业务系统:{0}{1}", urlStr, contentStr), "支付回调日志");
                }
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("回调给业务系统:" + ex.Message, "支付回调日志");
            }
        }
        #endregion

    }
}
