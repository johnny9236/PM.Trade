using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Quartz;
using PM.TaskBizInterface;

namespace PM.TaskBiz.JSABOCTask
{
    /// <summary>
    /// 嘉善入账明细任务
    /// </summary>
    public class JSABOCTaskJob : QuartzJobBase
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        protected override void InternalExecute(Quartz.IJobExecutionContext context)
        {
            ITimerTaskCallBiz biz = new JSABOCCall();
            biz.TimerCall();
        }
    }
}
