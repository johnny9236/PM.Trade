using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Quartz;
using PM.TaskBizInterface;

namespace PM.TaskBiz.DDQABOCTask
{
    /// <summary>
    /// 入账明细任务
    /// </summary>
    public class DDQABOCTaskJob : QuartzJobBase
    {
        /// <summary>
        /// 执行入账处理
        /// </summary>
        /// <param name="context"></param>
        protected override void InternalExecute(Quartz.IJobExecutionContext context)
        {
            ITimerTaskCallBiz biz = new DDQABOCCall();
            biz.TimerCall();
        }
    }
}
