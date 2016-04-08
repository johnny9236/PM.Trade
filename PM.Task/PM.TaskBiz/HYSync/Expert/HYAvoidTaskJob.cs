using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Quartz;
using PM.TaskBizInterface;

namespace PM.TaskBiz.HYSync.Expert
{
    /// <summary>
    /// 项目专家回避条件
    /// </summary>
    public class HYAvoidTaskJob : QuartzJobBase
    {
        protected override void InternalExecute(Quartz.IJobExecutionContext context)
        {
            ITimerTaskCallBiz biz = new  HYAvoidCall();
            biz.TimerCall();
        }
    }
}
