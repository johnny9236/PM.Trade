using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.Utils.Log;

namespace PM.TaskBiz.HYSync.Expert
{
    public class HYAvoidCallBack : ITimerTaskCallBack
    {
        public void CallBack(dynamic dyObj)
        {
            //  增量入库操作
            var haveIn = IncrementToDB(dyObj);
        }
        #region
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="modelList">专家列表</param>
        /// <returns></returns>
        private bool IncrementToDB(List<string> modelList)
        {
            bool rtn = false;
            var dbEnter = new PM.TaskBiz.ORM.JSGC();
            try
            {
                var changeModel = from p in dbEnter.T_SyncExpertCondition
                                  join s in modelList
                                  on p.ProjectID.ToLower() equals s.ToLower()
                                  select p;
                if (null != changeModel && changeModel.Count() > 0)
                {
                    foreach (var project in changeModel)
                    {
                        project.AvoidFlag = "1";
                        project.UpdateTM = DateTime.Now;
                        dbEnter.T_SyncExpertCondition.ApplyCurrentValues(project);
                    } 
                    dbEnter.SaveChanges(); 
                }
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("异常" + ex.Message, "海盐专家回避信息");
                rtn = false;
            }
            return rtn;
        }
        #endregion
    }
}
