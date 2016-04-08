using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.TaskBizInterface;
using PM.TaskBiz.HYSync.QYK.Model;
using PM.Utils;
using PM.TaskBiz.ORM;
using PM.Utils.Log;

namespace PM.TaskBiz.HYSync.QYK
{
    /// <summary>
    /// 同步回调
    /// </summary>
    public class QykSyncCallBack : ITimerTaskCallBack
    {
        /// <summary>
        /// 企业库
        /// </summary>
        PM_QYKEntities qyk = new PM_QYKEntities();
        /// <summary>
        /// 请求地址
        /// </summary>
        string url = ConfigHelper.GetCustomCfg("HY", "QykUrl");
        /// <summary>
        /// 用户标示
        /// </summary>
        string userToken = ConfigHelper.GetCustomCfg("HY", "QykuserToken");

        public void CallBack(dynamic dyObj)
        {
            throw new NotImplementedException();
        }

        #region      处理
        /// <summary>
        /// 资质编码处理
        /// </summary>
        /// <param name="queueLst">资质编码更新队列</param>
        /// <returns></returns>
        private void GetAptitudeCodes(List<QueueUpdateResult> queueLst)
        {
            bool haveDate = false;//入库标记
            AptitudeCodeRequest apt = null;
            foreach (var q in queueLst)
            {
                apt = new AptitudeCodeRequest();
                apt.UserToken = userToken;
                apt.Url = url;

                if (apt.GetAptitudeCode())//获取返回对象
                {
                    var aptitudes = apt.AptitudeCodeList;
                    if (null != aptitudes)
                    {
                        #region  处理
                        if (aptitudes.Count > 0)
                        {
                            var aptCodes = qyk.T_HY_AptitudeCode.ToList();
                            foreach (var a in aptitudes)
                            {
                                //入库
                                var aptcode = new T_HY_AptitudeCode();
                                aptcode.CodeID = a.ID;
                                aptcode.CodeName = a.CodeName;
                                aptcode.CodeType = a.CodeType;
                                aptcode.InCode = a.InCode;
                                aptcode.IndexOf = a.IndexOf;
                                aptcode.CreateTm = DateTime.Now;
                                aptcode.ID = Guid.NewGuid().ToString();
                                var chkApt = aptCodes.Find(p => p.CodeID == a.ID);
                                if (null != chkApt)
                                {
                                    aptcode.UpdateTm = DateTime.Now;
                                }
                                qyk.T_HY_AptitudeCode.AddObject(aptcode);
                                if (!haveDate)
                                    haveDate = true;
                            }
                        }
                        #endregion
                    }
                }
            }
            if (haveDate)
            {
                try
                {
                    qyk.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry(string.Format("资质编码表入库失败"+ex.Message), "海盐企业库同步");
                }
            }

        }

        /// <summary>
        /// 企业处理
        /// </summary>
        /// <param name="queueLst">企业更新队列</param>
        /// <returns></returns>
        private void GetEntrpriseInfos(List<QueueUpdateResult> queueLst)
        {
            bool haveDate = false;//入库标记
            EntrpriseInfoRequset ent = null;
            foreach (var e in queueLst)
            {
                ent = new EntrpriseInfoRequset();
                ent.EnterpriseID = e.DataID;
                ent.Url = url;
                ent.UserToken = userToken;
                #region   逻辑
                if (ent.GetEntrpriseInfo())
                {
                    var entrInfo = ent.EntrpriseInfo;
                    if (null != entrInfo)
                    {


                    }
                }
                #endregion
            }
            if (haveDate)
            {
                try
                {
                    qyk.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry(string.Format("企业信息表入库失败"), "海盐企业库同步");
                }
            }

        }
        /// <summary>
        /// 企业人员处理
        /// </summary>
        /// <param name="queueLst">企业更新队列</param>
        /// <returns></returns>
        private void GetPersonnelInfos(List<QueueUpdateResult> queueLst)
        {
            bool haveDate = false;//入库标记
            PersonnelInfoRequset person = null;
            foreach (var p in queueLst)
            {
                person = new PersonnelInfoRequset();
                person.UnionPersonID = p.DataID;
                person.Url = url;
                person.UserToken = userToken;

                if (person.GetPersonnelInfo())
                {
                    //person.Personnel;
                }
            }
            if (haveDate)
            {
                try
                {
                    qyk.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry(string.Format("企业人员信息表入库失败"), "海盐企业库同步");
                }
            }
        }
        #endregion
    }
}
