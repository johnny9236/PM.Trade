using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Quartz;
using PM.TaskBizInterface;

namespace PM.TaskBiz.HuangMeiPostlTask
{
    /// <summary>
    /// 黄梅定时任务
    /// </summary>
    public class HuangMeiPostlTaskJob: QuartzJobBase
    {
        /// <summary>
        /// 任务入口
        /// </summary>
        /// <param name="context"></param>
        protected override void InternalExecute(Quartz.IJobExecutionContext context)
        {
            ITimerTaskCallBiz biz = new  HuangMeiPostlCall();
            biz.TimerCall();
        }
    }
}
