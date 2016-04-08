using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel
{
    //银行相关配置

    /// <summary>
    /// 支付银行类型  企业个人
    /// </summary>
    public enum BankPayEnterTp
    {
        /// <summary>
        ///企业
        /// </summary>
        Person = 11,
        /// <summary>
        /// 个人
        /// </summary>
        Enter = 12
    }

    /// <summary>
    /// 开户证件类型
    /// </summary>
    public class IdentificationType
    {
        public static string GetIdType(string IDdes)
        {
            string rtnStr = "x";//其他证件
            #region  代号
            switch (IDdes)
            {
                case "身份证":
                    rtnStr = "0";
                    break;
                case "户口簿":
                    rtnStr = "1";
                    break;
                case "护照":
                    rtnStr = "2";
                    break;
                case "军官证":
                    rtnStr = "3";
                    break;

                case "士兵证":
                    rtnStr = "4";
                    break;

                case "港澳居民来往内地通行证":
                    rtnStr = "5";
                    break;

                case "台湾同胞来往内地通行证":
                    rtnStr = "6";
                    break;

                case "临时身份证":
                    rtnStr = "7";
                    break;
                case "外国人居留证":
                    rtnStr = "8";
                    break;
                case "警官证":
                    rtnStr = "9";
                    break;
                default://其他证件
                    rtnStr = "x";
                    break;
            }
            #endregion
            return rtnStr;
        }
        public static string GetDesTypeByID(string ID)
        {
            string rtnStr = "其他证件";//其他证件
            #region  描述
            switch (ID)
            {
                case "0":
                    rtnStr = "身份证";
                    break;
                case "1":
                    rtnStr = "户口簿";
                    break;
                case "2":
                    rtnStr = "护照";
                    break;
                case "3":
                    rtnStr = "军官证";
                    break;

                case "4":
                    rtnStr = "士兵证";
                    break;

                case "5":
                    rtnStr = "港澳居民来往内地通行证";
                    break;

                case "6":
                    rtnStr = "台湾同胞来往内地通行证";
                    break;

                case "7":
                    rtnStr = "临时身份证";
                    break;
                case "8":
                    rtnStr = "外国人居留证";
                    break;
                case "9":
                    rtnStr = "警官证";
                    break;
                default://其他证件
                    rtnStr = "其他证件";
                    break;
            }
            #endregion
            return rtnStr;
        }
    }

    /// <summary>
    /// 银行类型 通用类型 
    /// </summary>
    public enum BankName
    {
        嘉善农行直连 = 999,
        #region
        中国工商银行 = 102,
        中国农业银行 = 103,
        中国银行 = 104,
        中国建设银行 = 105,
        交通银行 = 301,
        华夏银行 = 304,
        中国光大银行 = 303,
        中国民生银行 = 305,
        深圳发展银行 = 307,
        中国招商银行 = 308,
        上海浦东发展银行 = 310,
        徽商银行 = 440,
        //贵阳银行 = 443,
        河北银行 = 422,
        东亚银行 = 3001,
        中信 = 302,
        兴业 = 309,

        CFCA模拟银行 = 700,
        个人支付银联在线 = 889

        #endregion
    }

}
