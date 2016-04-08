using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Quartz;
using PM.TaskBizInterface;

namespace PM.TaskBiz.HuangShanICBC
{
    /// <summary>
    /// 黄山 退保证金 明细
    /// </summary>
    public class HuangShanICBCRtnTaskJob : QuartzJobBase
    {
        protected override void InternalExecute(Quartz.IJobExecutionContext context)
        {
            ITimerTaskCallBiz biz = new HuangShanICBCRtnCall();
            biz.TimerCall();
        }
    }
}
