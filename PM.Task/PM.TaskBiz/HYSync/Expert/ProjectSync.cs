/*
 * 说明： 专家项目基本信息同步 
 * 创建人： 朱雷松
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using PM.TaskBiz.ORM;
using PM.Utils.Log;
using PM.Utils;
using PM.JSGC.Biz.HYSync.Expert.Model;

namespace PM.TaskBiz.HYSync.Expert
{
    /// <summary>
    /// 专家项目同步
    /// 配置节点说明    
    /// 父  HYSync
    ///    HYAreaNum  HYPwd  HYWorkmanID   HYWSUrl  HYDeptID
    /// </summary>
    public class ProjectSync
    {
        /// <summary>
        /// 建设工程
        /// </summary>
        PM.TaskBiz.ORM.JSGC YWOC = new PM.TaskBiz.ORM.JSGC();
        /// <summary>
        /// 企业库
        /// </summary>
        PM.TaskBiz.ORM.PM_QYKEntities QYK = new PM_QYKEntities();

        #region 回避条件
        /// <summary>
        /// 回避条件根据项目ID
        /// </summary>
        /// <param name="projectID">项目ID</param>
        /// <returns></returns>
        public bool SnycAvoid(string projectID)
        {
            bool result = false;
            var projectGID = Guid.Parse(projectID);

            //专家条件主表
            var projectExpertFlow = YWOC.T_JSGC_ProjectExpertFlow.Where(p => p.CaseId == projectGID).FirstOrDefault();
            var project = YWOC.T_JSGC_ProjectInfo.Where(p => p.ProjectId == projectGID).FirstOrDefault();
            if (null != projectExpertFlow && null != projectExpertFlow)
            {
                result = SnycAvoid(project, projectExpertFlow);
            }
            else
            {
                LogTxt.WriteEntry(string.Format("专家抽取信息为空或项目信息获取为空对应projectID=[{0}]", projectID), "海盐专家回避条件");
            }
            return result;
        }
        /// <summary>
        /// 根据专家抽取信息同步回避条件
        /// </summary>
        /// <param name="project">项目信息</param>
        /// <param name="projectExpertFlow">专家抽取信息</param>
        /// <returns></returns>
        public bool SnycAvoid(T_JSGC_ProjectInfo project, T_JSGC_ProjectExpertFlow projectExpertFlow)
        {
            bool result = false;
            string hyPassword = ConfigHelper.GetCustomCfg("HY", "HYPwd");//密码 
            string hyUrl = ConfigHelper.GetCustomCfg("HY", "HYWSUrl");//专家库webservice 地址

            //系统参数 
            if (string.IsNullOrEmpty(hyPassword) || string.IsNullOrEmpty(hyUrl))
            {
                LogTxt.WriteEntry(string.Format("专家抽取配置参数信息为空对应hyPassword=[{0}]hyUrl=[{1}]", hyPassword, hyUrl), "海盐专家回避条件");
                return result;
            }
            #region   回避条件
            var syncAvoid = new HYSyncAvoidModel();
            syncAvoid.SPassword = hyPassword;
            syncAvoid.Url = hyUrl;
            syncAvoid.SProjectID = project.ProjectId.ToString();

            if (!string.IsNullOrEmpty(projectExpertFlow.AvoidUnit))
            {
                syncAvoid.AvoidList = new List<Avoid>();
                if (projectExpertFlow.AvoidUnit.IndexOf("1") > -1)
                {
                    //投标单位
                    var bidAvoidlst = GetBidAvoidUnits(project.ProjectId);
                    if (null != bidAvoidlst && bidAvoidlst.Count > 0)
                        syncAvoid.AvoidList.AddRange(bidAvoidlst);
                }
                if (projectExpertFlow.AvoidUnit.IndexOf("2") > -1)
                {
                    //业主
                    var ownerAvoid = GetOwnerAvoidUnit(projectExpertFlow);
                    if (null != ownerAvoid)
                        syncAvoid.AvoidList.Add(ownerAvoid);
                }
                if (projectExpertFlow.AvoidUnit.IndexOf("3") > -1)
                {
                    //代理
                    var agentAvoid = GetAgentAvoidUnit(project.UnitId.ToString());
                    if (null != agentAvoid)
                        syncAvoid.AvoidList.Add(agentAvoid);
                }

                //其他原因
                var otherAvoidLst = GetOtherAvoidUnit(projectExpertFlow.Id);
                if (null != otherAvoidLst && otherAvoidLst.Count > 0)
                    syncAvoid.AvoidList.AddRange(otherAvoidLst);
            }
            #endregion
            //发起
            result = syncAvoid.Sync();
            return result;
        } 
        #endregion 

        #region   private
        /// <summary>
        /// 获取投标单位的回避条件
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        private List<Avoid> GetBidAvoidUnits(Guid projectID)
        {
            List<Avoid> avoids = null;
            var tenderBM = (from bm in YWOC.T_JSGC_TenderBM
                            where (bm.ProjectId == projectID)
                            select new
                            {
                                UnitId = bm.UnitId
                            }).ToList();
            var bidAvoids = from bm in tenderBM
                            join crop in QYK.Corp_EnterpriseInfo
                            on bm.UnitId equals crop.EnterpriseId
                            select new
                            {
                                CorpOrgCode = crop.OrganizationalCode,
                                CorpName = crop.CorpName
                            };

            if (null != bidAvoids && bidAvoids.Count() > 0)
            {
                avoids = new List<Avoid>();
                foreach (var a in bidAvoids)
                {
                    var avoid = new Avoid();
                    avoid.EludeType = "1";
                    avoid.UnitID = a.CorpOrgCode;//组织机构
                    avoid.UnitName = a.CorpName;
                    avoids.Add(avoid);
                }
            }
            return avoids;
        }
        /// <summary>
        /// 获取代理机构的回避条件
        /// </summary>
        /// <param name="agentUnitId">代理单位ID</param>
        /// <returns></returns>
        private Avoid GetAgentAvoidUnit(string agentUnitId)
        {
            if (!string.IsNullOrEmpty(agentUnitId))
            {
                var enterpriseId = Guid.Parse(agentUnitId); 
                var agentInfo = QYK.Corp_EnterpriseInfo.Where(p => p.EnterpriseId == enterpriseId).FirstOrDefault();
                if (null != agentInfo && !string.IsNullOrEmpty(agentInfo.OrganizationalCode))
                {
                    var avoid = new Avoid();
                    avoid.EludeType = "3";
                    avoid.UnitID = agentInfo.OrganizationalCode;//组织机构
                    avoid.UnitName = agentInfo.CorpName;
                    return avoid;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取业主的回避条件
        /// </summary>
        /// <param name="projectExpertFlow">专家条件</param>
        /// <returns></returns>
        private Avoid GetOwnerAvoidUnit(T_JSGC_ProjectExpertFlow projectExpertFlow)
        {
            if (!string.IsNullOrEmpty(projectExpertFlow.OwnerCode))
            {
                var avoid = new Avoid();
                avoid.EludeType = "2";
                avoid.UnitID = projectExpertFlow.OwnerCode;//组织机构
                avoid.UnitName = projectExpertFlow.ProxyUnit;
                return avoid;
            }
            return null;
        }


        /// <summary>
        /// 获取其他原因的回避条件
        /// </summary>
        /// <param name="expertFlowID">专家条件ID</param>
        /// <returns></returns>
        private List<Avoid> GetOtherAvoidUnit(Guid expertFlowID)
        {
            List<Avoid> avoids = null;
            var otherAvoids = YWOC.T_JSGC_OtherAvoid.Where(p => p.pid == expertFlowID);
            if (null != otherAvoids && otherAvoids.Count() > 0)
            {
                avoids = new List<Avoid>();
                foreach (var a in otherAvoids)
                {
                    var avoid = new Avoid();
                    avoid.EludeType = "7";
                    avoid.UnitID = a.OrganizationCode;//组织机构
                    avoid.UnitName = a.AvoidUnit;
                    avoids.Add(avoid);
                }
            }
            return avoids;
        }

        #endregion
    }
}
