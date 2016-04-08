using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Quartz;
using PM.TaskBizInterface;

namespace PM.TaskBiz.HYSync.Expert
{
    /// <summary>
    /// 海盐专家获取job
    /// </summary>
    public class HYExpertTaskJob : QuartzJobBase
    { 
        protected override void InternalExecute(Quartz.IJobExecutionContext context)
        {
            ITimerTaskCallBiz biz = new  HYExpertCall();
            biz.TimerCall();
        }
    }
}
