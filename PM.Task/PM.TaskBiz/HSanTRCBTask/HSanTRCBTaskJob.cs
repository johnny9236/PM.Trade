using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Quartz;
using PM.TaskBizInterface;

namespace PM.TaskBiz.HSanTRCBTask
{
    /// <summary>
    /// 青阳建行入账明细
    /// </summary>
    public class HSanTRCBTaskJob : QuartzJobBase
    {
        protected override void InternalExecute(Quartz.IJobExecutionContext context)
        {
            ITimerTaskCallBiz biz = new HSanTRCBCall();
            biz.TimerCall();
        }
    }
}
