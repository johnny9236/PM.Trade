using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils;
using PM.Utils.WebUtils;
using PM.Utils.Log;
using System.Xml.Linq;

namespace PM.TaskBiz.HYSync.QYK.Model
{
    /// <summary>
    /// 获取资质编码
    /// </summary>
    public class AptitudeCodeRequest
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
        ///  资质编码列表
        /// </summary>
        public List<AptitudeCode> AptitudeCodeList { get; set; }

        /// <summary>
        /// 更新资质编码对象信息
        /// </summary>
        public bool GetAptitudeCode()
        {
            var url = Url ?? ConfigHelper.GetCustomCfg("HY", "QykUrl");
            var userToken = this.UserToken ?? ConfigHelper.GetCustomCfg("HY", "QykuserToken");
            bool result = false;
            object[] parm = new object[] { userToken };
            var rtnObj = WebServiceHelper.InvokeWebService(url, "GetQueueToUpdate", parm);
            if (null != rtnObj)//只获取有返回的结果
            {
                var rtnStr = rtnObj.ToString();
                LogTxt.WriteEntry("获取资质编码，返回报文:" + rtnStr, "海盐企业库同步");
                try
                {
                    var xdoc = XDocument.Parse(rtnStr);// 
                    var cmp = from c in xdoc.Descendants("AptitudeCode")
                              select new
                                {
                                    ID = c.Element("ID") == null ? string.Empty : c.Element("ID").Value,
                                    CodeType = c.Element("CodeType") == null ? string.Empty : c.Element("CodeType").Value,
                                    CodeName = c.Element("CodeName") == null ? string.Empty : c.Element("CodeName").Value,
                                    InCode = c.Element("InCode") == null ? string.Empty : c.Element("InCode").Value,
                                    IndexOf = c.Element("IndexOf") == null ? string.Empty : c.Element("IndexOf").Value
                                };
                    if (cmp != null && cmp.Count() > 0)
                    {
                        this.AptitudeCodeList = new List<AptitudeCode>();
                        foreach (var c in cmp)
                        {
                            var aptitudeCode = new AptitudeCode();
                            aptitudeCode.CodeName = c.CodeName;
                            aptitudeCode.CodeType = c.CodeType;
                            aptitudeCode.ID = c.ID;
                            aptitudeCode.InCode = c.InCode;
                            aptitudeCode.IndexOf = c.IndexOf;
                            this.AptitudeCodeList.Add(aptitudeCode);
                        }
                        result = true;
                    }
                    else
                    {
                        LogTxt.WriteEntry("获取资质编码，解析报文为空", "海盐企业库同步");
                    }
                }
                catch (Exception e)
                {
                    result = false;
                    LogTxt.WriteEntry("获取资质编码，异常:" + e.Message + e.StackTrace, "海盐企业库同步");
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
    /// 资质编码
    /// </summary>
    public class AptitudeCode
    {
        /// <summary>
        /// 编码标识
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 编码类型
        /// </summary>
        public string CodeType { get; set; }
        /// <summary>
        /// 编码内容
        /// </summary>
        public string CodeName { get; set; }
        /// <summary>
        /// 所属父编码标识
        /// </summary>
        public string InCode { get; set; }
        /// <summary>
        /// 编码排序（部分）
        /// </summary>
        public string IndexOf { get; set; }
    }
}
