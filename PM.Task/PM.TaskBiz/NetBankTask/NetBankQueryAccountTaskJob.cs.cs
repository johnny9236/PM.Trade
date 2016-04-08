using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Quartz;
using PM.TaskBizInterface;

namespace PM.TaskBiz.NetBankTask
{
    public class NetBankQueryAccountTaskJob : QuartzJobBase
    {
        protected override void InternalExecute(Quartz.IJobExecutionContext context)
        {
            ITimerTaskCallBiz biz = new  NetBankQueryAccountCall();
            biz.TimerCall();
        }
    }
}
