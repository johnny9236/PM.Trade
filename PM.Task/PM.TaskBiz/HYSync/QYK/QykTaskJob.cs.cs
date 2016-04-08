using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.Quartz;
using PM.TaskBizInterface;

namespace PM.TaskBiz.HYSync.QYK
{
    /// <summary>
    /// 企业库企业信息同步任务
    /// </summary>
    public class QykTaskJob : QuartzJobBase
    {
        protected override void InternalExecute(Quartz.IJobExecutionContext context)
        {
            ITimerTaskCallBiz biz = new QykSyncCall();
            biz.TimerCall();
        }
    }
}
