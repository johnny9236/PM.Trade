using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;

namespace PM.TaskBiz.HYSync.Expert
{
    /// <summary>
    /// 定时发送回避条件
    /// </summary>
    public class HYAvoidCall : ITimerTaskCallBiz
    {
        public void TimerCall()
        {
            ProjectSync sync = null;//同步对象
            List<string> avoidSyncProjectList = null;
            var dbEnter = new PM.TaskBiz.ORM.JSGC();
            //未获取的项目处理
            var expertsProjectList = dbEnter.T_SyncExpertCondition.Where(p => p.AvoidFlag != "1" || p.AvoidFlag == null);
            if (null != expertsProjectList && expertsProjectList.Count() > 0)
            {
                sync = new ProjectSync();
                avoidSyncProjectList = new List<string>();
                foreach (var p in expertsProjectList)
                {
                    var pID = Guid.Parse(p.ProjectID);
                    var project = dbEnter.T_JSGC_ProjectInfo.FirstOrDefault(h =>  h.ProjectId == pID);
                    if (null != project)
                    {
                        if (project.BidOpeningStart.Value.Subtract(DateTime.Now).Days == 1)
                        {
                            var rst = sync.SnycAvoid(p.ProjectID);
                            if (rst)
                            {
                                avoidSyncProjectList.Add(p.ProjectID);
                            }
                        }
                    }
                }
            }
            if (null != avoidSyncProjectList && avoidSyncProjectList.Count > 0)
            {
                //入库操作
                GetCallbackInterface().CallBack(avoidSyncProjectList);
            }
        }

        public ITimerTaskCallBack GetCallbackInterface()
        {
            return new HYAvoidCallBack();
        }
    }

}
