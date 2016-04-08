using System;
using System.Collections.Generic;
using System.Text;

namespace PM.TaskBizInterface
{
    /// <summary>
    /// 业务发起接口
    /// </summary>
    public interface ITimerTaskCallBiz
    {
        /// <summary>
        /// 定时任务调用
        /// </summary>
        void TimerCall();
        /// <summary>
        /// 获取回调接口
        /// </summary>
        ITimerTaskCallBack GetCallbackInterface();
    }
}
