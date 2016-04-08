using PM.TaskBizInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBiz.HYSync.Expert.Model;
using PM.TaskBiz.ORM;
using PM.Utils.Log;

namespace PM.TaskBiz.HYSync.Expert
{
    /// <summary>
    /// 海盐专家业务处理
    /// </summary>
    public class HYExpertCallBack : ITimerTaskCallBack
    {
        public void CallBack(dynamic dyObj)
        {
            //  增量入库操作
            var haveIn = IncrementToDB(dyObj);
        }

        #region
        /// <summary>
        /// 增量入库
        /// </summary>
        /// <param name="modelList">专家列表</param>
        /// <returns></returns>
        private bool IncrementToDB(List<HYExpert> modelList)
        {
            bool rtn = false;
            T_JSGC_ProjectExperts enter = null;
            var dbEnter = new PM.TaskBiz.ORM.JSGC();
            var dbList = dbEnter.T_JSGC_ProjectExperts;           //获取数据库内容
            string changeProject = string.Empty;//记录项目ID  判断 项目是否改变

            foreach (var lst in modelList)
            {
                var projGID = Guid.Parse(lst.ProjectID);
                var cqDate = DateTime.MinValue;
                var chk = dbList.FirstOrDefault(p => p.ProjectID == projGID && p.Captcha == lst.Captcha);
                if (chk == null)//增量添加
                {
                    enter = new T_JSGC_ProjectExperts();
                    enter.Id = Guid.NewGuid();
                    enter.ProjectID = projGID;

                    enter.Captcha = lst.Captcha;
                    DateTime.TryParse(lst.CQDate, out cqDate);
                    if (cqDate != DateTime.MinValue)
                        enter.CqDate = cqDate;

                    enter.CreateDate = DateTime.Now;
                    dbEnter.T_JSGC_ProjectExperts.AddObject(enter);

                    #region 更新专家同步表
                    if (changeProject.Trim().ToLower() != lst.ProjectID.Trim().ToLower())
                    {
                        var expertCondition = dbEnter.T_SyncExpertCondition.Where(p => p.ProjectID == lst.ProjectID).FirstOrDefault();
                        if (null != expertCondition)
                        {
                            expertCondition.GetDtlFlag = "1";
                            expertCondition.UpdateTM = DateTime.Now;
                            dbEnter.T_SyncExpertCondition.ApplyCurrentValues(expertCondition);
                            changeProject = lst.ProjectID;
                        }
                    }
                    #endregion
                    if (!rtn)
                    {
                        rtn = true;
                    }
                }
            }
            if (rtn)
            {
                try
                {
                    dbEnter.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry("异常" + ex.Message, "海盐获取专家信息");
                    rtn = false;
                }
            }
            return rtn;
        }
        #endregion
    }
}
