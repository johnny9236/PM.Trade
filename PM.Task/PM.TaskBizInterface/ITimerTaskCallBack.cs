using System;
using System.Collections.Generic;
using System.Text;

namespace PM.TaskBizInterface
{
    /// <summary>
    /// 回调处理 业务信息
    /// </summary>
    public interface ITimerTaskCallBack
    {
        /// <summary>
        /// 回调后业务处理
        /// </summary>
        /// <param name="dyObj"></param>
        void CallBack(dynamic dyObj);
    }
}
