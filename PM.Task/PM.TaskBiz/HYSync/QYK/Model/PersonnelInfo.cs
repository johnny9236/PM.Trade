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
    /// 获取企业人员信息
    /// </summary>
    public class PersonnelInfoRequset
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
        /// 人员联合标识
        /// </summary>
        public string UnionPersonID { get; set; }
        /// <summary>
        /// 人员信息
        /// </summary>
        public Personnel Personnel { get; set; }
        /// <summary>
        /// 人员信息
        /// </summary>
        public bool GetPersonnelInfo()
        {
            var url =Url?? ConfigHelper.GetCustomCfg("HY", "QykUrl");
            var userToken = this.UserToken ?? ConfigHelper.GetCustomCfg("HY", "QykuserToken");
            bool result = false;
            object[] parm = new object[] { userToken, this.UnionPersonID };
            var rtnObj = WebServiceHelper.InvokeWebService(url, "GetPersonnelInfo", parm);
            if (null != rtnObj)//只获取有返回的结果
            {
                var rtnStr = rtnObj.ToString();
                LogTxt.WriteEntry("获取人员信息，返回报文:" + rtnStr, "海盐企业库同步");
                try
                {
                    var xdoc = XDocument.Parse(rtnStr);
                    var cmp = (from c in xdoc.Descendants("Personnel")
                               select new
                                 {
                                     #region
                                     UnionPersonID = c.Element("UnionPersonID") == null ? string.Empty : c.Element("UnionPersonID").Value,
                                     fid = c.Element("fid") == null ? string.Empty : c.Element("fid").Value,
                                     MemberType = c.Element("MemberType") == null ? string.Empty : c.Element("MemberType").Value,
                                     EnterpriseID = c.Element("EnterpriseID") == null ? string.Empty : c.Element("EnterpriseID").Value,
                                     xm = c.Element("xm") == null ? string.Empty : c.Element("xm").Value,

                                     sfz = c.Element("sfz") == null ? string.Empty : c.Element("sfz").Value,
                                     SafeCert = c.Element("SafeCert") == null ? string.Empty : c.Element("SafeCert").Value,
                                     SafeCertValidTime = c.Element("SafeCertValidTime") == null ? string.Empty : c.Element("SafeCertValidTime").Value,
                                     xmjlzsbh = c.Element("xmjlzsbh") == null ? string.Empty : c.Element("xmjlzsbh").Value,

                                     sex = c.Element("sex") == null ? string.Empty : c.Element("sex").Value,
                                     insert_date = c.Element("insert_date") == null ? string.Empty : c.Element("insert_date").Value,
                                     shyx = c.Element("shyx") == null ? string.Empty : c.Element("shyx").Value,
                                     ChiefEngineer = c.Element("ChiefEngineer") == null ? string.Empty : c.Element("ChiefEngineer").Value,
                                     ChiefEngineer_sz = c.Element("ChiefEngineer_sz") == null ? string.Empty : c.Element("ChiefEngineer_sz").Value,

                                     ChiefEngiValidTime = c.Element("ChiefEngiValidTime") == null ? string.Empty : c.Element("ChiefEngiValidTime").Value,
                                     aqjlsgz = c.Element("aqjlsgz") == null ? string.Empty : c.Element("aqjlsgz").Value,
                                     sgzyxq = c.Element("sgzyxq") == null ? string.Empty : c.Element("sgzyxq").Value
                                     #endregion
                                 }).FirstOrDefault();
                    if (cmp != null)
                    {
                        #region  赋值
                        this.Personnel = new Personnel();

                        this.Personnel.Aqjlsgz = cmp.aqjlsgz;
                        this.Personnel.ChiefEngineer = cmp.ChiefEngineer;
                        this.Personnel.ChiefEngineer_sz = cmp.ChiefEngineer_sz;
                        this.Personnel.ChiefEngiValidTime = cmp.ChiefEngiValidTime;
                        this.Personnel.EnterpriseID = cmp.EnterpriseID;

                        this.Personnel.Fid = cmp.fid;
                        this.Personnel.Insert_date = cmp.insert_date;
                        this.Personnel.MemberType = cmp.MemberType;
                        this.Personnel.SafeCert = cmp.SafeCert;
                        this.Personnel.SafeCertValidTime = cmp.SafeCertValidTime;

                        this.Personnel.Sex = cmp.sex;
                        this.Personnel.Sfz = cmp.sfz;
                        this.Personnel.Sgzyxq = cmp.sgzyxq;
                        this.Personnel.Shyx = cmp.shyx;
                        this.Personnel.UnionPersonID = cmp.UnionPersonID;

                        this.Personnel.Xm = cmp.xm;
                        this.Personnel.Xmjlzsbh = cmp.xmjlzsbh;

                        #region  资质
                        var specialitys = from c in xdoc.Descendants("PersonnelSpeciality")
                                          select new
                                            {
                                                fid = c.Element("fid") == null ? string.Empty : c.Element("fid").Value,
                                                UnionPersonID = c.Element("UnionPersonID") == null ? string.Empty : c.Element("UnionPersonID").Value,
                                                memberid = c.Element("memberid") == null ? string.Empty : c.Element("memberid").Value,
                                                zczs = c.Element("zczs") == null ? string.Empty : c.Element("zczs").Value,
                                                zczy = c.Element("zczy") == null ? string.Empty : c.Element("zczy").Value,

                                                zcdj = c.Element("zcdj") == null ? string.Empty : c.Element("zcdj").Value,
                                                bz = c.Element("bz") == null ? string.Empty : c.Element("bz").Value
                                            };
                        #endregion
                        if (specialitys != null && specialitys.Count() > 0)
                        {
                            this.Personnel.PersonnelSpecialitylst = new List<PersonnelSpeciality>();
                            foreach (var sp in specialitys)
                            {
                                var specialityInfo = new PersonnelSpeciality();
                                specialityInfo.Bz = sp.bz;
                                specialityInfo.Fid = sp.fid;
                                specialityInfo.Memberid = sp.memberid;
                                specialityInfo.UnionPersonID = sp.UnionPersonID;
                                specialityInfo.Zcdj = sp.zcdj;
                                specialityInfo.Zczs = sp.zczs;
                                specialityInfo.Zczy = sp.zczy;
                                this.Personnel.PersonnelSpecialitylst.Add(specialityInfo);
                            }
                        }
                        #endregion
                        result = true;
                    }
                    else
                    {
                        LogTxt.WriteEntry("获取人员信息，解析报文为空，人员id:" + UnionPersonID, "海盐企业库同步");
                    }
                }
                catch (Exception e)
                {
                    result = false;
                    LogTxt.WriteEntry("获取人员信息，异常:" + e.Message + e.StackTrace, "海盐企业库同步");
                }
            }
            else
            {
                LogTxt.WriteEntry("获取人员信息，返回报文为空，人员id:" + UnionPersonID, "海盐企业库同步");
            }
            return result;
        }
    }

    /// <summary>
    /// 人员信息
    /// </summary>
    public class Personnel
    {
        /// <summary>
        /// 人员联合标识
        /// </summary>
        public string UnionPersonID { get; set; }
        /// <summary>
        /// 人员标识（不同类型会重复）
        /// </summary>
        public string Fid { get; set; }
        /// <summary>
        /// 人员类型：建造师、安全员、监理工程师、其他类人员
        /// </summary>
        public string MemberType { get; set; }
        /// <summary>
        /// 对应的企业标识
        /// </summary>
        public string EnterpriseID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Xm { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string Sfz { get; set; }
        /// <summary>
        /// 安全证书（B、C）编号
        /// </summary>
        public string SafeCert { get; set; }
        /// <summary>
        /// 安全证书有效期
        /// </summary>
        public string SafeCertValidTime { get; set; }
        /// <summary>
        /// 资质证书编号（建造师）注册证书编号（监理）
        /// </summary>
        public string Xmjlzsbh { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string Insert_date { get; set; }
        /// <summary>
        /// 是否审核通过（1是0否）
        /// </summary>
        public string Shyx { get; set; }
        /// <summary>
        /// 是否首席工程师（施工房建、监理、其他类）
        /// </summary>
        public string ChiefEngineer { get; set; }
        /// <summary>
        /// 是否首席工程师（施工市政）
        /// </summary>
        public string ChiefEngineer_sz { get; set; }
        /// <summary>
        /// 首席工程师有效期
        /// </summary>
        public string ChiefEngiValidTime { get; set; }
        /// <summary>
        /// 安全监理上岗证
        /// </summary>
        public string Aqjlsgz { get; set; }
        /// <summary>
        /// 安全监理上岗证有效期
        /// </summary>
        public string Sgzyxq { get; set; }
        /// <summary>
        /// 人员资质列表
        /// </summary>
        public List<PersonnelSpeciality> PersonnelSpecialitylst { get; set; }
    }
    /// <summary>
    /// 人员资质
    /// </summary>
    public class PersonnelSpeciality
    {
        /// <summary>
        /// 人员资质标识
        /// </summary>
        public string Fid { get; set; }
        /// <summary>
        /// 人员联合标识
        /// </summary>
        public string UnionPersonID { get; set; }
        /// <summary>
        /// 人员标识（不同类型会重复）
        /// </summary>
        public string Memberid { get; set; }
        /// <summary>
        /// 资质证书编号
        /// </summary>
        public string Zczs { get; set; }
        /// <summary>
        /// 注册资质类型
        /// </summary>
        public string Zczy { get; set; }
        /// <summary>
        /// 注册等级
        /// </summary>
        public string Zcdj { get; set; }
        /// <summary>
        /// 备注（是否项目经理）
        /// </summary>
        public string Bz { get; set; }

    }
}
