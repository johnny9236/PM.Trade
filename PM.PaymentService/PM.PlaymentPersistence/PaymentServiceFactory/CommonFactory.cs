using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils;
using PM.PaymentManger;

namespace PM.PlaymentPersistence.PaymentServiceFactory
{
    /// <summary>
    /// 非支付工厂
    /// </summary>
    public class CommonFactory
    {
        /// <summary>
        /// 非支付调用  
        /// </summary>
        /// <param name="objModel">通用对象</param>
        public static dynamic CommonRemoteCall(dynamic objModel)
        {
            dynamic rtn = null;
            var area = ConfigHelper.GetConfigString("Area");
            switch (area)
            {
                case "JSABOC"://六盘水
                    //new PM.TaskBiz.JSABOCTask.JSABOCCall().TimerCall();
                    break;
                case "AHQY"://安徽青阳
                case "HuangSan"://黄山
                    rtn =CustomCommManager.CallProtocol(objModel);//发送协议
                    break;
                case "HaiYan"://海盐
                    rtn = CustomCommManager.CallProtocol(objModel);//发送协议
                    break;
            }
            return rtn;
        }
    }
}
