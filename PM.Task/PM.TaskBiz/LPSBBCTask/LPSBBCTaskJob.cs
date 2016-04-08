using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Quartz;
using PM.TaskBizInterface;

namespace PM.TaskBiz.LPSBBCTask
{
    /// <summary>
    /// 六盘水入账
    /// </summary>
    public class LPSBBCTaskJob : QuartzJobBase
    {
        protected override void InternalExecute(Quartz.IJobExecutionContext context)
        {
            ITimerTaskCallBiz biz = new LPSBBCCall();
            biz.TimerCall();
        }
    }
}
