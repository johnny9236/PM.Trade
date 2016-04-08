using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Quartz;
using PM.TaskBizInterface;

namespace PM.TaskBiz.HuangShiCCBTask
{
    /// <summary>
    /// 执行黄石入账明细匹配job
    /// </summary>
    public class HuangShiCCBTaskJob : QuartzJobBase
    {
        protected override void InternalExecute(Quartz.IJobExecutionContext context)
        {
            ITimerTaskCallBiz biz = new HuangShiCCBCall();
            biz.TimerCall();
        }
    }
}
