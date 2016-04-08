/*
 * 说明： 投标人及其它需要回避的单位同步对象（省专家库-建设网）
 * 创建人： 朱雷松
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PM.Utils.Log;
using PM.Utils.WebUtils;

namespace PM.JSGC.Biz.HYSync.Expert.Model
{
    /// <summary>
    /// 投标人及其它需要回避的单位同步
    /// </summary>
    public class HYSyncAvoidModel
    {
        #region  配置参数
        /// <summary>
        /// 请求url
        /// </summary>
        public string Url { get; set; }
        #endregion
        #region   接口系统参数
        /// <summary>
        /// 通讯密码
        /// </summary>
        public string SPassword { get; set; }
        /// <summary>
        /// 各市对应系统对应活动的ID
        /// </summary>
        public string SProjectID { get; set; }
        #endregion

        #region 回避信息
        /// <summary>
        /// 回避列表
        /// </summary>
        public List<Avoid> AvoidList { get; set; }
        #endregion
        /// <summary>
        /// 获取回避报文
        /// </summary>
        /// <returns></returns>
        private string GetAvoidXml()
        {
            string rtnXml = string.Empty;
            string avoidXml = string.Empty;
            foreach (var avo in AvoidList)
            {
                //var avoUnit = new XElement("unit",
                //new XElement("unitID", avo.UnitID?? string.Empty),
                //  new XElement("unitName", avo.UnitName ?? string.Empty),
                //    new XElement("eludeType", avo.EludeType ?? string.Empty)
                //        );
                var avoUnitStr = string.Format("<unit  unitID='{0}'  unitName='{1}'  eludeType='{2}'   ></unit>"
                    , avo.UnitID
                    , avo.UnitName
                    , avo.EludeType
                    );
                avoidXml += avoUnitStr;
            }
            rtnXml = string.Format("<?xml version='1.0'  encoding='gb2312' ?><data>{0}</data>", avoidXml);
            return rtnXml;
        }

        /// <summary>
        ///同步处理
        /// </summary>
        /// <returns></returns>
        public bool Sync()
        {
            bool result = false;

            string avoidXml = string.Empty;
            string sPwd = this.SPassword;
            string sProjectID = this.SProjectID;//活动ID 
            try
            {
                avoidXml = GetAvoidXml();
                LogTxt.WriteEntry(string.Format("项目id=[{0}]专家回避信息同步xml=[{1}]", this.SProjectID, avoidXml), "海盐回避信息");
            }
            catch (Exception e)
            {
                LogTxt.WriteEntry(string.Format(e.Message + e.StackTrace + "专家回避信息同步报文设置失败（" + System.Reflection.MethodBase.GetCurrentMethod().Name + "）"), "海盐回避信息");

                return false;
            }
            if (string.IsNullOrEmpty(this.Url) || string.IsNullOrEmpty(sPwd) || string.IsNullOrEmpty(sProjectID))
            {
                LogTxt.WriteEntry("专家回避信息同步系统参数设置为空" + System.Reflection.MethodBase.GetCurrentMethod().Name, "海盐回避信息");
                return false;
            }

            try
            {
                object[] parm = new object[] { sPwd, avoidXml, sProjectID };
                var rtnObj = WebServiceHelper.InvokeWebService(this.Url, "inputUnit", parm);
                if (null != rtnObj && (int)rtnObj == 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry(ex.Message + System.Reflection.MethodBase.GetCurrentMethod().Name, "海盐回避信息");
            }
            return result;
        }
    }
    /// <summary>
    /// 回避对象
    /// </summary>
    public class Avoid
    {
        /// <summary>
        /// 组织机构代码，要求不大于50个字符
        /// </summary>
        public string UnitID { get; set; }
        /// <summary>
        /// 回避单位名称，要求不大于50个字符
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 回避类别，要求返回1-7（除5）的数字
        /// 1　投标单位2　项目业主3　代理机构4　主管单位6　设计单位7　其他原因
        /// </summary>
        public string EludeType { get; set; }
    }
}
