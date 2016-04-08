using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Quartz;
using PM.TaskBizInterface;

namespace PM.TaskBiz.HYBOCTASK
{
    /// <summary>
    /// 海盐 入账明细
    /// </summary>
    public class BOCTaskJob : QuartzJobBase
    {
        protected override void InternalExecute(Quartz.IJobExecutionContext context)
        {
            ITimerTaskCallBiz biz = new BOCCall();
            biz.TimerCall();
        }
    }
}
