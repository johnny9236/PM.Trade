using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TradeTest
{

    /// <summary>
    /// 资质编码
    /// </summary>
    [Table("T_HY_AptitudeCode")]
    public class AptitudeCode
    {
        /// <summary>
        /// 编码标识
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 编码标识
        /// </summary>
        public string CodeID { get; set; }
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

        /// <summary>
        /// 是否匹配  1匹配
        /// </summary>
        public int Match { get; set; }
        /// <summary>
        /// 是否处理  1 处理
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTm { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTm { get; set; }
    }



    /// <summary>
    /// 企业信息
    /// </summary>
    [Table("T_HY_EntrpriseInfo")]
    public class EntrpriseInfo
    {
        public int ID { get; set; }
        /// <summary>
        /// 企业标识
        /// </summary>
        public string CodeId { get; set; }
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
        /// 是否匹配  1匹配
        /// </summary>
        public int Match { get; set; }
        /// <summary>
        /// 是否处理  1 处理
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTm { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTm { get; set; }
    }



    /// <summary>
    /// 企业资质
    /// </summary>
    [Table("T_HY_EnterpriseSpeciality")]
    public class EnterpriseSpeciality
    {
        public int ID { get; set; }
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

        /// <summary>
        /// 是否匹配  1匹配
        /// </summary>
        public int Match { get; set; }
        /// <summary>
        /// 是否处理  1 处理
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTm { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTm { get; set; }
    }

    ///
    /// <summary>
    /// 人员信息
    /// </summary>
    [Table("T_HY_Personnel")]
    public class Personnel
    {
        public int ID { get; set; }
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
        /// 是否匹配  1匹配
        /// </summary>
        public int Match { get; set; }
        /// <summary>
        /// 是否处理  1 处理
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTm { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTm { get; set; }

    }
    /// <summary>
    /// 人员资质
    /// </summary>
    [Table("T_HY_PersonnelSpeciality")]
    public class PersonnelSpeciality
    {
        public int ID { get; set; }
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

        /// <summary>
        /// 是否匹配  1匹配
        /// </summary>
        public int Match { get; set; }
        /// <summary>
        /// 是否处理  1 处理
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTm { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTm { get; set; }

    }
}
