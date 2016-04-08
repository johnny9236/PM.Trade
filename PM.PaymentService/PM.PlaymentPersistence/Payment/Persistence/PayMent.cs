using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Log;
using PM.PaymentProtocolModel;
using PM.PlaymentPersistence.ORM;
using PM.PaymentModel;

namespace PM.PlaymentPersistence.Payment.Persistence
{
    /// <summary>
    /// 支付基类
    /// </summary>
    public partial class Persistence
    {
        #region  支付
        #region  支付 发起
        /// <summary>
        /// 设置支付请求订单信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual bool SetRequestPayOrder(T_Pay_Order order)
        {
            return true;
        }
        /// <summary>
        /// 支付请求后续处理业务 （继承类处理）[1、订单保存 2、业务关系表 保存  3获取请求输出 ]  
        /// </summary>
        /// <param name="info"></param>
        /// <param name="order"></param>
        /// <param name="rtnStr"></param>
        /// <returns></returns>
        protected virtual bool OprationBKRequestPay(ResultInfo info, T_Pay_Order order, out string rtnStr)
        {
            rtnStr = string.Empty;
            return true;
        }

        /// <summary>
        /// 发起支付
        /// 注意：如果返回字符串中带"■"; 标示错误的 需要前台提示
        /// </summary>
        /// <returns></returns>
        public string PayRequest()
        {
            string rtnStr = string.Empty;
            try
            {
                T_Pay_Order order = new T_Pay_Order();
                order.OrderID = Guid.NewGuid();
                // order.BusinessNo = BusinessNo;
                //order.OrderNo = orderNo;
                if (!SetRequestPayOrder(order))//设置订单信息  需要重写
                {
                    LogTxt.WriteEntry("设置订单信息失败", "支付日志");
                    return rtnStr;
                }
                Entities.SaveChanges();
                //接口赋值
                var sendInfo = SetCallRemotePayCommunicationInfo(order);
                if (null != sendInfo)
                {
                    if (sendInfo.Result == ResultType.Success)
                    {
                        #region 记录 获取请求信息
                        var orderRequest = new T_Pay_OrderRequest();
                        orderRequest.ID = Guid.NewGuid();
                        orderRequest.OrderNo = order.OrderNo;
                        orderRequest.PacketMessage = sendInfo.MessagePaket ?? string.Empty;
                        orderRequest.RequestTime = DateTime.Now;
                        orderRequest.RequestUrl = sendInfo.ActionURLToBank ?? string.Empty;
                        orderRequest.Signature = sendInfo.Signature;
                        Entities.T_Pay_OrderRequest.AddObject(orderRequest);
                        if (!OprationBKRequestPay(sendInfo, order, out rtnStr))//后续处理订单信息
                        {
                            LogTxt.WriteEntry("设置订单后续信息失败", "银联支付日志");
                            return rtnStr;
                        }
                        #endregion
                        //if (!string.IsNullOrEmpty(orderRequest.RequestUrl))
                        //    rtnStr = Build_Form(sendInfo);
                    }
                    //else
                    //{
                    //    rtnStr = sendInfo.NoticeMsg + "■";//特殊符号标示错误的 需要前台提示
                    //}
                }
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(ex.Message, "银联支付日志");
            }
            return rtnStr;
        }
        /// <summary>
        /// 支付请求发起信息
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns></returns>
        //protected abstract ResultInfo SetCallRemotePayCommunicationInfo(T_Pay_Order order);
        protected abstract dynamic SetCallRemotePayCommunicationInfo(T_Pay_Order order);
        /// <summary>
        /// 支付响应通讯
        /// </summary> 
        /// <param name="resopnseModel">报文对象</param>
        /// <returns></returns>
        protected abstract dynamic SetCallBackRemotePayCommunicationInfo(PayResopnseModel resopnseModel);
        #endregion

        #region  支付 响应业务相关
        /// <summary>
        /// 设置支付返回订单信息
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns></returns>
        protected virtual bool SetResponsePayOrder(T_Pay_Order order)
        {
            return true;
        }
        /// <summary>
        /// 后续业务设置支付返回订单信息[1、订单信息修改2、订单返回请求信息保存3、业务表数据修改]
        /// </summary>
        /// <param name="info">获取返回对象</param> 
        /// <param name="bkInfo">是否后台显示</param>
        /// <param name="queryPayerBusinessNo">支付人业务功能号</param>
        /// <param name="rtnStr">订单返回字串信息</param>
        /// <returns></returns>
        protected virtual bool OprationBKResponsePay(ResultInfo info, bool bkShow, out string rtnStr)
        {
            rtnStr = string.Empty;
            return true;
        }
        /// <summary>
        /// 支付响应操作,内部需要在继承类中实现如下内容(响应总接口中使用)
        /// 1：获取响应信息解析
        /// 2：实现后续业务处理( 订单状态修改、业务关系表状态修改)
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="message"></param>
        /// <param name="bkShow"></param>
        /// <param name="showStr"></param>
        /// <returns></returns>
        private bool PayResopnseOpration(PayResopnseModel resopnseModel, out string showStr)
        {
            bool rtn = false;
            showStr = string.Empty;
           // string showInfo = string.Empty;
            var info = SetCallBackRemotePayCommunicationInfo(resopnseModel);//支付响应返回      
            if (null != info && (info.Result == ResultType.Success || info.Result == ResultType.UnKnow))
            {
                showStr = info.NoticeMsg;//后台显示返回 通知给银联  
                if (string.IsNullOrEmpty(info.OrderNo))
                {
                    showStr = "success";
                    LogTxt.WriteEntry(string.Format("返回signature:{0}\n------ message:{1}    对应订单号为空", resopnseModel.Signature, resopnseModel.Message), "支付日志");
                    return rtn;
                }
                else
                {
                    #region  获取请求返回对象信息
                    T_Pay_OrderResponse or = new T_Pay_OrderResponse();
                    or.ID = Guid.NewGuid();
                    or.PacketMessage = resopnseModel.Message;
                    or.Signature = resopnseModel.Signature;
                    or.RequestTime = DateTime.Now;
                    or.OrderNo = info.OrderNo;
                    or.SerialNumber = string.IsNullOrEmpty(info.SerialNumber) == true ? string.Empty : info.SerialNumber;//流水号 
                    Entities.T_Pay_OrderResponse.AddObject(or);
                    #endregion
                    rtn = OprationBKResponsePay(info, resopnseModel.IsShowBk, out showStr);//后续业务处理
                    //if (!resopnseModel.IsShowBk)//非后台则输出订单信息
                    //{
                    //showStr = showInfo;
                    //  }
                    //else
                    //{ 
                    //}
                }
            }
            else
            {
                showStr = "支付验证返回失败";
                LogTxt.WriteEntry("支付验证返回失败", "支付日志");
            }
            return rtn;
        }

        #endregion
        #endregion
    }
}
