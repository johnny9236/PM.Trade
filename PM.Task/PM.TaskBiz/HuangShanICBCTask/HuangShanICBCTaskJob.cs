using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Quartz;
using PM.TaskBizInterface;

namespace PM.TaskBiz.HuangShanICBC
{
    public class HuangShanICBCTaskJob : QuartzJobBase
    {
        /// <summary>
        /// 黄山任务(保证金入账明细)
        /// </summary>
        /// <param name="context"></param>
        protected override void InternalExecute(Quartz.IJobExecutionContext context)
        {
            ITimerTaskCallBiz biz = new HuangShanICBCCall();
            biz.TimerCall();
        }
    }
}
