using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PlaymentPersistence.ORM;

namespace PM.PlaymentPersistence.Payment.Persistence
{
    /// <summary>
    /// 转账 结算
    /// </summary>
    public partial class Persistence
    {
        #region   转账业务相关
        /// <summary>
        /// 设置转账支付返回订单信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual bool SetResponseTransferPayOrder(T_Pay_Order order)
        {
            return true;
        }
        /// <summary>
        /// 设置转账支付请求订单信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual bool SetRequestTransferPayOrder(T_Pay_Order orderList)
        {
            return true;
        }
        #endregion
        #region  结算业务相关
        /// <summary>
        /// 设置结算支付返回订单信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual bool SetResponseClearPayOrder(T_Pay_Order order)
        {
            return true;
        }
        /// <summary>
        /// 设置结算支付请求订单信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual bool SetClearPayRequestOrder(List<T_Pay_Order> orderList)
        {
            return true;
        }
        #endregion

    }
}
