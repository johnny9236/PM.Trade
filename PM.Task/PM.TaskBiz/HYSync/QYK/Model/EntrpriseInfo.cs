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
    /// 获取企业信息
    /// </summary>
    public class EntrpriseInfoRequset
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
        /// 企业标示
        /// </summary>
        public string EnterpriseID
        {
            get;
            set;
        }
        /// <summary>
        /// 企业信息
        /// </summary>
        public EntrpriseInfo EntrpriseInfo { get; set; }
        /// <summary>
        ///获取企业信息
        /// </summary>
        public bool GetEntrpriseInfo()
        {
            var url = Url??ConfigHelper.GetCustomCfg("HY", "QykUrl");
            var userToken = this.UserToken ?? ConfigHelper.GetCustomCfg("HY", "QykuserToken");
            bool result = false;
            object[] parm = new object[] { userToken, this.EnterpriseID };
            var rtnObj = WebServiceHelper.InvokeWebService(url, "GetEntrpriseInfo", parm);
            if (null != rtnObj)//只获取有返回的结果
            {
                var rtnStr = rtnObj.ToString();
                LogTxt.WriteEntry("获取企业信息，返回报文:" + rtnStr, "海盐企业库同步");
                try
                {
                    var xdoc = XDocument.Parse(rtnStr);// 
                    var cmp = (from c in xdoc.Descendants("Enterprise")
                               select new
                                 {
                                     #region
                                     id = c.Element("id") == null ? string.Empty : c.Element("id").Value,
                                     fprnumber = c.Element("fprnumber") == null ? string.Empty : c.Element("fprnumber").Value,
                                     trade_license_no = c.Element("trade_license_no") == null ? string.Empty : c.Element("trade_license_no").Value,
                                     enter_type = c.Element("enter_type") == null ? string.Empty : c.Element("enter_type").Value,
                                     enter_name = c.Element("enter_name") == null ? string.Empty : c.Element("enter_name").Value,
                                     AREA = c.Element("AREA") == null ? string.Empty : c.Element("AREA").Value,
                                     legal_rept = c.Element("legal_rept") == null ? string.Empty : c.Element("legal_rept").Value,
                                     legal_repttime = c.Element("legal_repttime") == null ? string.Empty : c.Element("legal_repttime").Value,
                                     tech_principal = c.Element("tech_principal") == null ? string.Empty : c.Element("tech_principal").Value,
                                     tech_principaltime = c.Element("tech_principaltime") == null ? string.Empty : c.Element("tech_principaltime").Value,
                                     enter_kind = c.Element("enter_kind") == null ? string.Empty : c.Element("enter_kind").Value,
                                     busiLicense_no = c.Element("busiLicense_no") == null ? string.Empty : c.Element("busiLicense_no").Value,
                                     contact_people = c.Element("contact_people") == null ? string.Empty : c.Element("contact_people").Value,
                                     contact_tel = c.Element("contact_tel") == null ? string.Empty : c.Element("contact_tel").Value,
                                     contact_fax = c.Element("contact_fax") == null ? string.Empty : c.Element("contact_fax").Value,
                                     contact_address = c.Element("contact_address") == null ? string.Empty : c.Element("contact_address").Value,
                                     Email = c.Element("Email") == null ? string.Empty : c.Element("Email").Value,
                                     postcode = c.Element("postcode") == null ? string.Empty : c.Element("postcode").Value,
                                     trade_license_validtime = c.Element("trade_license_validtime") == null ? string.Empty : c.Element("trade_license_validtime").Value,
                                     record_validtime = c.Element("record_validtime") == null ? string.Empty : c.Element("record_validtime").Value,
                                     zczj = c.Element("zczj") == null ? string.Empty : c.Element("zczj").Value,
                                     permanent_assets = c.Element("permanent_assets") == null ? string.Empty : c.Element("permanent_assets").Value,
                                     engineer_staff = c.Element("engineer_staff") == null ? string.Empty : c.Element("engineer_staff").Value,
                                     update_date = c.Element("update_date") == null ? string.Empty : c.Element("update_date").Value,
                                     Is_safetyconstruction = c.Element("Is_safetyconstruction") == null ? string.Empty : c.Element("Is_safetyconstruction").Value,
                                     Is_safetyconstructiontime = c.Element("Is_safetyconstructiontime") == null ? string.Empty : c.Element("Is_safetyconstructiontime").Value,
                                     ryzsbh1 = c.Element("ryzsbh1") == null ? string.Empty : c.Element("ryzsbh1").Value,
                                     ryzsbh2 = c.Element("ryzsbh2") == null ? string.Empty : c.Element("ryzsbh2").Value,
                                     zdddz = c.Element("zdddz") == null ? string.Empty : c.Element("zdddz").Value,
                                     zddfzr = c.Element("zddfzr") == null ? string.Empty : c.Element("zddfzr").Value,
                                     zddlxr = c.Element("zddlxr") == null ? string.Empty : c.Element("zddlxr").Value,
                                     zdd_fax = c.Element("zdd_fax") == null ? string.Empty : c.Element("zdd_fax").Value,
                                     zddlxdh = c.Element("zddlxdh") == null ? string.Empty : c.Element("zddlxdh").Value,
                                     shtg = c.Element("shtg") == null ? string.Empty : c.Element("shtg").Value,
                                     lxdz = c.Element("lxdz") == null ? string.Empty : c.Element("lxdz").Value,
                                     legal_M1 = c.Element("legal_M1") == null ? string.Empty : c.Element("legal_M1").Value,
                                     legal_M2 = c.Element("legal_M2") == null ? string.Empty : c.Element("legal_M2").Value,
                                     ryzsbhM1 = c.Element("ryzsbhM1") == null ? string.Empty : c.Element("ryzsbhM1").Value,
                                     ryzsbhM2 = c.Element("ryzsbhM2") == null ? string.Empty : c.Element("ryzsbhM2").Value,
                                     legal_M1time = c.Element("legal_M1time") == null ? string.Empty : c.Element("legal_M1time").Value,
                                     legal_M2time = c.Element("legal_M2time") == null ? string.Empty : c.Element("legal_M2time").Value
                                     #endregion
                                 }).FirstOrDefault();
                    if (cmp != null)
                    {
                        #region  赋值
                        this.EntrpriseInfo = new EntrpriseInfo();

                        this.EntrpriseInfo.AREA = cmp.AREA;
                        this.EntrpriseInfo.BusiLicense_no = cmp.busiLicense_no;
                        this.EntrpriseInfo.Contact_address = cmp.contact_address;
                        this.EntrpriseInfo.Contact_fax = cmp.contact_fax;
                        this.EntrpriseInfo.Contact_people = cmp.contact_people;

                        this.EntrpriseInfo.Contact_tel = cmp.contact_tel;
                        this.EntrpriseInfo.Email = cmp.Email;
                        this.EntrpriseInfo.Engineer_staff = cmp.engineer_staff;
                        this.EntrpriseInfo.Enter_kind = cmp.enter_kind;
                        this.EntrpriseInfo.Enter_name = cmp.enter_name;

                        this.EntrpriseInfo.Enter_type = cmp.enter_type;
                        this.EntrpriseInfo.Fprnumber = cmp.fprnumber;
                        this.EntrpriseInfo.Id = cmp.id;
                        this.EntrpriseInfo.Is_safetyconstruction = cmp.Is_safetyconstruction;
                        this.EntrpriseInfo.Is_safetyconstructiontime = cmp.Is_safetyconstructiontime;

                        this.EntrpriseInfo.Legal_M1 = cmp.legal_M1;
                        this.EntrpriseInfo.Legal_M1time = cmp.legal_M1time;
                        this.EntrpriseInfo.Legal_M2 = cmp.legal_M2;
                        this.EntrpriseInfo.Legal_M2time = cmp.legal_M2time;
                        this.EntrpriseInfo.Legal_rept = cmp.legal_rept;

                        this.EntrpriseInfo.Legal_repttime = cmp.legal_repttime;
                        this.EntrpriseInfo.Lxdz = cmp.lxdz;
                        this.EntrpriseInfo.Permanent_assets = cmp.permanent_assets;
                        this.EntrpriseInfo.Postcode = cmp.postcode;
                        this.EntrpriseInfo.Record_validtime = cmp.record_validtime;

                        this.EntrpriseInfo.Ryzsbh1 = cmp.ryzsbh1;
                        this.EntrpriseInfo.Ryzsbh2 = cmp.ryzsbh2;
                        this.EntrpriseInfo.RyzsbhM1 = cmp.ryzsbhM1;
                        this.EntrpriseInfo.RyzsbhM2 = cmp.ryzsbhM2;
                        this.EntrpriseInfo.Shtg = cmp.shtg;

                        this.EntrpriseInfo.Tech_principal = cmp.tech_principal;
                        this.EntrpriseInfo.Tech_principaltime = cmp.tech_principaltime;
                        this.EntrpriseInfo.Trade_license_no = cmp.trade_license_no;
                        this.EntrpriseInfo.Trade_license_validtime = cmp.trade_license_validtime;
                        this.EntrpriseInfo.Update_date = cmp.update_date;

                        this.EntrpriseInfo.Zczj = cmp.zczj;
                        this.EntrpriseInfo.Zdd_fax = cmp.zdd_fax;
                        this.EntrpriseInfo.Zdddz = cmp.zdddz;
                        this.EntrpriseInfo.Zddfzr = cmp.zddfzr;
                        this.EntrpriseInfo.Zddlxdh = cmp.zddlxdh;

                        this.EntrpriseInfo.Zddlxr = cmp.zddlxr;
                        #region  资质
                        var specialitys = from c in xdoc.Descendants("EnterpriseSpeciality")
                                          select new
                                            {
                                                fid = c.Element("fid") == null ? string.Empty : c.Element("fid").Value,
                                                Trade_no = c.Element("Trade_no") == null ? string.Empty : c.Element("Trade_no").Value,
                                                specialty = c.Element("specialty") == null ? string.Empty : c.Element("specialty").Value,
                                                grading = c.Element("grading") == null ? string.Empty : c.Element("grading").Value,
                                                bz = c.Element("bz") == null ? string.Empty : c.Element("bz").Value,
                                                zzzsbh = c.Element("zzzsbh") == null ? string.Empty : c.Element("zzzsbh").Value,
                                                yxsj = c.Element("yxsj") == null ? string.Empty : c.Element("yxsj").Value
                                            };
                        #endregion
                        if (specialitys != null && specialitys.Count() > 0)
                        {
                            this.EntrpriseInfo.EnterpriseSpecialitys = new List<EnterpriseSpeciality>();
                            foreach (var sp in specialitys)
                            {
                                var specialityInfo = new EnterpriseSpeciality();
                                specialityInfo.Bz = sp.bz;
                                specialityInfo.Fid = sp.fid;
                                specialityInfo.Grading = sp.grading;
                                specialityInfo.Specialty = sp.specialty;
                                specialityInfo.Trade_no = sp.Trade_no;
                                specialityInfo.Yxsj = sp.yxsj;
                                specialityInfo.Zzzsbh = sp.zzzsbh;

                                this.EntrpriseInfo.EnterpriseSpecialitys.Add(specialityInfo);
                            }
                        }
                        // this.EntrpriseInfo.PM_Enter_type = cmp.postcode;//需要通过
                        #endregion
                        result = true;
                    }
                    else
                    {
                        LogTxt.WriteEntry("获取企业信息，解析报文为空", "海盐企业库同步");
                    }
                }
                catch (Exception e)
                {
                    result = false;
                    LogTxt.WriteEntry("获取企业信息，异常:" + e.Message + e.StackTrace, "海盐企业库同步");
                }
            }
            else
            {
                LogTxt.WriteEntry("获取企业信息，报文为空", "海盐企业库同步");
            }
            return result;
        }
    }

    /// <summary>
    /// 企业信息
    /// </summary>
    public class EntrpriseInfo
    {
        /// <summary>
        /// 企业标识
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string Fprnumber { get; set; }
        /// <summary>
        /// 交易证编号
        /// </summary>
        public string Trade_license_no { get; set; }
        /// <summary>
        /// 企业类型(建议不要使用)
        /// </summary>
        public string Enter_type { get; set; }
        /////////////////////////////////////////////// 信息
        /// <summary>
        /// PM企业类型(处理完 供品茗入库)
        /// </summary>
        public string PM_Enter_type { get; set; }
        ///////////////////////////////////////////////////

        /// <summary>
        /// 企业名称
        /// </summary>
        public string Enter_name { get; set; }
        /// <summary>
        /// 所属地区
        /// </summary>
        public string AREA { get; set; }
        /// <summary>
        /// 法定代表人
        /// </summary>
        public string Legal_rept { get; set; }
        /// <summary>
        /// 法人A证有效期
        /// </summary>
        public string Legal_repttime { get; set; }
        /// <summary>
        /// 技术负责人
        /// </summary>
        public string Tech_principal { get; set; }
        /// <summary>
        /// 技术负责人A证有效期
        /// </summary>
        public string Tech_principaltime { get; set; }
        /// <summary>
        /// 企业性质
        /// </summary>
        public string Enter_kind { get; set; }
        /// <summary>
        /// 营业执照
        /// </summary>
        public string BusiLicense_no { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact_people { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Contact_tel { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Contact_fax { get; set; }
        /// <summary>
        /// 注册地址
        /// </summary>
        public string Contact_address { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string Postcode { get; set; }
        /// <summary>
        /// 交易证有效期
        /// </summary>
        public string Trade_license_validtime { get; set; }
        /// <summary>
        /// 备案有效期(用于控制整个企业    是否可用)
        /// </summary>
        public string Record_validtime { get; set; }
        /// <summary>
        /// 注册资金
        /// </summary>
        public string Zczj { get; set; }
        /// <summary>
        /// 固定资产
        /// </summary>
        public string Permanent_assets { get; set; }
        /// <summary>
        /// 驻当地技术负责人
        /// </summary>
        public string Engineer_staff { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public string Update_date { get; set; }
        /// <summary>
        /// 安全生产许可证号
        /// </summary>
        public string Is_safetyconstruction { get; set; }
        /// <summary>
        /// 安全生产许可证有效期
        /// </summary>
        public string Is_safetyconstructiontime { get; set; }
        /// <summary>
        /// 法人A证编号
        /// </summary>
        public string Ryzsbh1 { get; set; }
        /// <summary>
        /// 技术负责人A证编号
        /// </summary>
        public string Ryzsbh2 { get; set; }
        /// <summary>
        /// 驻当地地址
        /// </summary>
        public string Zdddz { get; set; }
        /// <summary>
        /// 驻当地负责人
        /// </summary>
        public string Zddfzr { get; set; }
        /// <summary>
        /// 驻当地联系人
        /// </summary>
        public string Zddlxr { get; set; }
        /// <summary>
        /// 驻当地传真
        /// </summary>
        public string Zdd_fax { get; set; }
        /// <summary>
        /// 驻当地联系电话
        /// </summary>
        public string Zddlxdh { get; set; }
        /// <summary>
        /// 是否审核通过（1是0否）
        /// </summary>
        public string Shtg { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string Lxdz { get; set; }
        /// <summary>
        /// 经理
        /// </summary>
        public string Legal_M1 { get; set; }
        /// <summary>
        /// 分管安全副经理
        /// </summary>
        public string Legal_M2 { get; set; }
        /// <summary>
        /// 经理A证编号
        /// </summary>
        public string RyzsbhM1 { get; set; }
        /// <summary>
        /// 分管安全副经理A证编号
        /// </summary>
        public string RyzsbhM2 { get; set; }
        /// <summary>
        /// 经理A证有效期
        /// </summary>
        public string Legal_M1time { get; set; }
        /// <summary>
        /// 分管安全副经理A证有效期
        /// </summary>
        public string Legal_M2time { get; set; }
        /// <summary>
        /// 资质
        /// </summary>
        public List<EnterpriseSpeciality> EnterpriseSpecialitys { get; set; }
    }
    /// <summary>
    /// 企业资质
    /// </summary>
    public class EnterpriseSpeciality
    {
        /// <summary>
        /// 企业资质标识
        /// </summary>
        public string Fid { get; set; }
        /// <summary>
        /// 企业标识
        /// </summary>
        public string Trade_no { get; set; }
        /// <summary>
        /// 资质名称
        /// </summary>
        public string Specialty { get; set; }
        /// <summary>
        /// 资质等级
        /// </summary>
        public string Grading { get; set; }
        /// <summary>
        /// 备注，是否主项(数据不全
        /// </summary>
        public string Bz { get; set; }
        /// <summary>
        /// 资质证书编号(数据不全)
        /// </summary>
        public string Zzzsbh { get; set; }
        /// <summary>
        /// 资质有效期
        /// </summary>
        public string Yxsj { get; set; }
    }
}
