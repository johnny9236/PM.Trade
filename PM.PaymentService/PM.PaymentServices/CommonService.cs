using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentContracts;
using PM.PlaymentPersistence;
using PM.PaymentModel;

namespace PM.PaymentServices
{
    public class CommonService : ICommService
    {
        PlaymentPersistenceManager manager = new PlaymentPersistenceManager();
        /// <summary>
        /// 非支付调用
        /// </summary>
        /// <param name="objModel">调用对象</param>
        /// <returns></returns>
        public string CommonRemoteCall(CommServiceProtocolModel objModel)
        {
            return manager.CommonRemoteCall(objModel); ;
        }
    }
}
