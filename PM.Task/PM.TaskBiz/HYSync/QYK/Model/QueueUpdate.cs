using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.WebUtils;
using PM.Utils;
using PM.Utils.Log;
using System.Xml.Linq;

namespace PM.TaskBiz.HYSync.QYK.Model
{
    #region   获取队列
    /// <summary>
    /// 获取更新队列请求对象
    /// </summary>
    public class QueueUpdateRequset
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserToken
        {
            get;
            set;
        }
        /// <summary>
        /// 请求url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 响应对象
        /// </summary>
        public List<QueueUpdateResult> QueueToUpdateList { get; set; }
        /// <summary>
        /// 更新对象信息
        /// </summary>
        public bool GetQueueToUpdate()
        {
            var url = Url ?? ConfigHelper.GetCustomCfg("HY", "QykUrl");
            var userToken = this.UserToken ?? ConfigHelper.GetCustomCfg("HY", "QykuserToken");
            bool result = false;
            object[] parm = new object[] { userToken };
            var rtnObj = WebServiceHelper.InvokeWebService(url, "GetQueueToUpdate", parm);
            if (null != rtnObj)//只获取有返回的结果
            {
                var rtnStr = rtnObj.ToString();
                LogTxt.WriteEntry("获取更新队列，返回报文:" + rtnStr, "海盐企业库同步");
                try
                {
                    var xdoc = XDocument.Parse(rtnStr);// 
                    var cmp = from c in xdoc.Descendants("QueueToUpdate")
                              select new
                                {
                                    ID = c.Element("ID") == null ? string.Empty : c.Element("ID").Value,
                                    DataType = c.Element("DataType") == null ? string.Empty : c.Element("DataType").Value,
                                    DataID = c.Element("DataID") == null ? string.Empty : c.Element("DataID").Value,
                                    CreateTime = c.Element("CreateTime") == null ? string.Empty : c.Element("CreateTime").Value
                                };
                    if (cmp != null && cmp.Count() > 0)
                    {
                        this.QueueToUpdateList = new List<QueueUpdateResult>();
                        foreach (var c in cmp)
                        {
                            var queueInfo = new QueueUpdateResult();
                            queueInfo.CreateTime = c.CreateTime;
                            queueInfo.DataID = c.DataID;
                            queueInfo.DataType = c.DataType;
                            queueInfo.ID = c.ID;
                            this.QueueToUpdateList.Add(queueInfo);
                        }
                        result = true;
                    }
                    else
                    {
                        LogTxt.WriteEntry("获取更新队列，解析报文为空", "海盐企业库同步");
                    }
                }
                catch (Exception e)
                {
                    result = false;
                    LogTxt.WriteEntry("获取更新队列，异常:" + e.Message + e.StackTrace, "海盐企业库同步");
                }
            }
            else
            {
                LogTxt.WriteEntry("获取更新队列，报文为空", "海盐企业库同步");
            }
            return result;
        }
    }
    /// <summary>
    /// 获取更新队列返回对象
    /// </summary>
    public class QueueUpdateResult
    {
        /// <summary>
        /// 记录标识，主键
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 数据类型：企业、人员等
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 数据标识，用于获取数据
        /// </summary>
        public string DataID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }
    #endregion


    #region   清除队列
    /// <summary>
    /// 清除队列请求
    /// </summary>
    public class ClearDealtDataInQueueRequest
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserToken { get; set; }
        /// <summary>
        /// 请求url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 数据类型：企业、人员 等
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 数据标识
        /// </summary>
        public string DataID { get; set; }
        /// <summary>
        /// 返回结果
        /// </summary>
        public ClearQueueResult ClearQueueResult { get; set; }

        public bool GetClearQueueResult()
        {
            var url = Url ?? ConfigHelper.GetCustomCfg("HY", "QykUrl");
            var userToken = this.UserToken ?? ConfigHelper.GetCustomCfg("HY", "QykuserToken");
            bool result = false;
            object[] parm = new object[] { userToken, this.DataType, this.DataID };
            var rtnObj = WebServiceHelper.InvokeWebService(url, "ClearDealtDataInQueue", parm);
            if (null != rtnObj)//只获取有返回的结果
            {
                var rtnStr = rtnObj.ToString();
                LogTxt.WriteEntry("获取清除请求队列，返回报文:" + rtnStr, "海盐企业库同步");
                try
                {
                    var xdoc = XDocument.Parse(rtnStr);// 
                    var cmp = (from c in xdoc.Descendants("ExecResult")
                               select new
                                 {
                                     ExecCuccessfully = c.Element("ExecCuccessfully") == null ? string.Empty : c.Element("ExecCuccessfully").Value,
                                     Description = c.Element("Description") == null ? string.Empty : c.Element("Description").Value
                                 }).FirstOrDefault();
                    if (cmp != null)
                    {
                        this.ClearQueueResult = new ClearQueueResult();
                        this.ClearQueueResult.Description = cmp.Description;
                        this.ClearQueueResult.ExecCuccessfully = cmp.ExecCuccessfully == "1" ? true : false;
                        result = true;
                    }
                }
                catch (Exception e)
                {
                    result = false;
                    LogTxt.WriteEntry("获取清除请求队列，异常:" + e.Message + e.StackTrace, "海盐企业库同步");
                }
            }
            return result;
        }
    }
    /// <summary>
    /// 清除队列结果
    /// </summary>
    public class ClearQueueResult
    {
        /// <summary>
        ///  结果
        /// </summary>
        public bool ExecCuccessfully { get; set; }
        /// <summary>
        /// 成功时返回空；失败时存放异常信息
        /// </summary>
        public string Description { get; set; }
    }

    #endregion


}
