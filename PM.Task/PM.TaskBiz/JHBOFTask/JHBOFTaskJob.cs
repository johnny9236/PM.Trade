using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Quartz;
using Quartz;
using PM.TaskBizInterface;
using PM.TaskBusiness.JHBOFTask;

namespace PM.TaskBiz.JHBOFTask
{
    /// <summary>
    /// job执行
    /// </summary>
    public class JHBOFTaskJob : QuartzJobBase
    {
        protected override void InternalExecute(IJobExecutionContext context)
        {
            ITimerTaskCallBiz biz = new JHBOFCall();
            biz.TimerCall();
        }
    }
}
