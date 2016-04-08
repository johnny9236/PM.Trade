using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using PM.PaymentModel;

namespace PM.PaymentContracts
{
    /// <summary>
    ///  提供给业务系统使用的签约接口
    /// </summary>
    [ServiceContract]
    public interface ICommService
    {
        /// <summary>
        /// 其他通用操作
        /// 1、对象先按各自规则封装
        /// 2、通过CommServiceProtocolModel传递
        /// 3、获取后在通过各自对象实例化成对象
        /// </summary>
        /// <param name="objModel">操作对象</param>
        /// <returns></returns>
        [OperationContract]
        string CommonRemoteCall(CommServiceProtocolModel objModel);
    }
}
