using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.DDQABOC.ProtocolsModel
{
    #region  通用
    /// <summary>
    /// 省市
    /// </summary>
    public enum Province
    {
        总行 = 01,
        天津市 = 02,
        上海市分行 = 03,
        山西省 = 04,
        内蒙古 = 05,
        辽宁省 = 06,
        吉林省 = 07,
        黑龙江 = 08,
        上海市农行信用卡中心 = 09,
        江苏省 = 10,
        北京市 = 11,
        安徽省 = 12,
        福建省 = 13,
        江西省 = 14,
        山东省 = 15,
        河南省 = 16,
        湖北省 = 17,
        湖南省 = 18,
        浙江省 = 19,
        广西区 = 20,
        海南省 = 21,
        四川省 = 22,
        贵州省 = 23,
        云南省 = 24,
        西藏区 = 25,
        陕西省 = 26,
        甘肃省 = 27,
        青海省 = 28,
        宁夏区 = 29,
        新疆区 = 30,
        重庆市 = 31,
        大连市 = 34,
        青岛市 = 38,
        宁波市 = 39,
        厦门市 = 40,
        深圳市 = 41,
        广东省 = 44,
        河北省 = 50,
        台湾省 = 71,
        营业部 = 81,
        香港 = 97,
        澳门 = 99
        // 总行 = 99
    }
    /// <summary>
    /// 币种
    /// </summary>
    public enum CURRENCY
    {
        复合币种 = 00,
        CNY = 01,
        GBP = 12,
        HKD = 13,
        USD = 14,
        CHF = 15,
        SGD = 18,
        SEK = 21,
        DKK = 22,
        NOK = 23,
        JPY = 27,
        CAD = 28,
        AUD = 29,
        EUR = 38,
        MOP = 81
    }
    #endregion
    /// <summary>
    /// 省市特殊标记处理
    /// </summary>
    public class Help
    {
        /// <summary>
        /// 省市
        /// </summary>
        /// <param name="prov"></param>
        /// <returns></returns>
        public static string GetProv(string prov)
        {
            int prv = (int)((Province)Enum.Parse(typeof(Province), prov));

            if (prv < 10)
            {
                return "0" + prv.ToString();
            }
            else
            {
                return prv.ToString();
            }
        }

        /// <summary>
        /// 币种
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public static string GetCur(string current)
        {
            int cur = (int)((CURRENCY)Enum.Parse(typeof(CURRENCY), current));

            if (cur < 10)
            {
                return "0" + cur.ToString();
            }
            else
            {
                return cur.ToString();
            }
        }
    }
}
