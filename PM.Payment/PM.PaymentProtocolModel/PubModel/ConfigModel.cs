using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using PM.Utils;

namespace PM.PaymentProtocolModel
{
    /// <summary>
    /// 公共配置对象(配置文件获取)
    /// </summary>
    [Serializable]
    public class SysConfigModel
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        public string CfgName { get; set; }
        /// <summary>
        /// 配置明细
        /// </summary>
        public List<CfgInfo> CfgInfoList { get; set; }
        #region
        /// <summary>
        /// 序列
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool xmlSeria(string path)
        {
            bool res = false;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SysConfigModel));
                using (StreamWriter streamWriter = File.CreateText(path))
                {
                    serializer.Serialize(streamWriter, this);
                    res = true;
                }
            }
            catch (Exception ex)
            {
                CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, ex.Message, ex);
                throw ex;
            }
            return res;
        }
        /// <summary>
        /// 反序列
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public SysConfigModel xmlDeserialize(string path)
        {
            SysConfigModel deserializedInstance = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SysConfigModel));
                using (StreamReader streamReader = File.OpenText(path))
                {
                    deserializedInstance = serializer.Deserialize(streamReader) as SysConfigModel;
                }
            }
            catch (Exception ex)
            {
                CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, ex.Message, ex);
                throw ex;
            }
            return deserializedInstance;
        }
        #endregion
    }
    /// <summary>
    /// 配置文件对象
    /// </summary>
    [Serializable]
    public class CfgInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 业务号
        /// </summary>
        public string BusinessNo { get; set; }
        /// <summary>
        /// 协议类型
        /// </summary>
        public string ProtocolsWay { get; set; }
        /// <summary>
        /// 协议类型描述
        /// </summary>
        public string ProtocolsWayDes { get; set; }
        /// <summary>
        /// 操作类型  支付还是查询
        /// </summary>
        public string OprationType { get; set; }
        /// <summary>
        /// 操作类型描述  支付还是查询
        /// </summary>
        public string OprationTypeDes { get; set; }

        /// <summary>
        /// 动作类型  请求、响应
        /// </summary>
        public string ActionType { get; set; }
        /// <summary>
        /// 动作类型  请求、响应
        /// </summary>
        public string ActionTypeDes { get; set; } 


        /// <summary>
        /// 业务功能类型
        /// </summary>
        public string BusinessKind { get; set; }
        /// <summary>
        /// 业务功能类型描述
        /// </summary>
        public string BusinessKindDes { get; set; }
        /// <summary>
        /// 系统编号
        /// </summary>
        public string SysCode { get; set; }
        #region 必填

        ///// <summary>
        ///// 机构代码
        ///// </summary>
        //public string StructCode { get; set; }
        ///// <summary>
        ///// 手续费  
        ///// </summary>
        //public double Free { get; set; }

        #endregion
        /// <summary>
        /// 证书路径
        /// </summary>
        public string CfgFilePath { get; set; }

        /// <summary>
        /// 请求URL
        /// </summary>
        public string  RequestURL { get; set; } 
        /// <summary>
        /// 通知给第三方的URL
        /// </summary>
        public string NotificationURL { get; set; } 
        /// <summary>
        /// 后台通知地址
        /// </summary>
        public string NotificationBgURL { get; set; }
        
        #region  支付能通过文件等获取信息
        /// <summary>
        /// ip
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// 文件根目录(或者ftp地址)
        /// </summary>
        public string RootFilePath { get; set; }
        /// <summary>
        /// ftp用户名
        /// </summary>
        public string FtpUserName { get; set; }
        /// <summary>
        /// ftp密码
        /// </summary>
        public string FtpUserPwd { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string  FileType { get; set; }
        /// <summary>
        /// 文件类型描述
        /// </summary>
        public string FileTypeDes { get; set; }
        #endregion
        
    }

    /// <summary>
    /// 页面传递对象(需要小写)
    /// </summary>
    [Serializable]
    public class JsonCfgInfo
    {
        /// <summary>
        /// 记录总数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 有用的行
        /// </summary>
        public List<CfgInfo> rows { get; set; }
    }

}
