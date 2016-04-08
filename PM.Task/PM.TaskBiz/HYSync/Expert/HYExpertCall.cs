using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.Utils;
using PM.Utils.WebUtils;
using PM.TaskBiz.HYSync.Expert.Model;
using PM.Utils.Log;
using PM.TaskBiz.ORM;

namespace PM.TaskBiz.HYSync.Expert
{
    /// <summary>
    /// 海盐专家获取
    /// </summary>
    public class HYExpertCall : ITimerTaskCallBiz
    {
        public void TimerCall()
        {
            List<HYExpert> allExpertList = new List<HYExpert>();
            var dbEnter = new PM.TaskBiz.ORM.JSGC();
            //未获取的项目处理
            var expertsProjectList = dbEnter.T_SyncExpertCondition.Where(p => p.GetDtlFlag != "1" || p.GetDtlFlag == null);
            if (null != expertsProjectList && expertsProjectList.Count() > 0)
            {
                foreach (var p in expertsProjectList)
                {
                    var hyExperts = Getexperts(p.ProjectID);
                    if (null != hyExperts && hyExperts.Count > 0)
                        allExpertList.AddRange(hyExperts);
                }
            }
            if (allExpertList.Count > 0)
            {
                //入库操作
                GetCallbackInterface().CallBack(allExpertList);
            }
        }

        /// <summary>
        /// 回调处理业务
        /// </summary>
        /// <returns></returns>
        public ITimerTaskCallBack GetCallbackInterface()
        {
            return new HYExpertCallBack();
        }

        #region  private
        /// <summary>
        ///发送 解析报文
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <param name="depID">部门</param>
        /// <param name="projectID">项目id</param>
        /// <returns></returns>
        private List<HYExpert> Getexperts(string projectID)
        {
            string url = ConfigHelper.GetCustomCfg("HY", "Url");
            string pwd = ConfigHelper.GetCustomCfg("HY", "Pwd");
            string depID = ConfigHelper.GetCustomCfg("HY", "DeptID");
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(depID))
            {
                LogTxt.WriteEntry("获取参数失败", "海盐获取专家信息");
                return null;
            }

            List<HYExpert> expertList = null;
            object[] parm = new object[] { pwd, depID, projectID };
            var rtnObj = (string)WebServiceHelper.InvokeWebService(url, "GetProjExpertList", parm);
            if (!string.IsNullOrEmpty(rtnObj))
            {
                expertList = new List<HYExpert>();
                var expertArrs = rtnObj.Split('|');
                Array.ForEach(expertArrs, p =>
                     {
                         var expts = p.Split(';');
                         if (null != expts && expts.Length == 2)
                         {
                             if (!string.IsNullOrEmpty(expts[0]))
                             {
                                 var hyExp = new HYExpert();
                                 hyExp.Captcha = expts[0];
                                 hyExp.CQDate = expts[1];
                                 hyExp.ProjectID = projectID;
                                 expertList.Add(hyExp);
                             }
                         }
                     }
             );
                return expertList;
            }
            return null;
        }

        #endregion
    }

}
