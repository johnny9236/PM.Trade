using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace PM.Utils
{
    /// <summary>
    /// 获取AppSetting信息
    /// </summary>
    public static class AppSettingReader
    {
        internal static int CheckValueRange(int num, int? min, int? max, int defaultVal)
        {
            if (min.HasValue && (num < min.Value))
            {
                return defaultVal;
            }
            if (max.HasValue && (num > max.Value))
            {
                return defaultVal;
            }
            return num;
        }

        public static int GetInt(string keyName)
        {
            return GetInt(keyName, false, 0, null, null);
        }

        public static int GetInt(string keyName, int defaultVal)
        {
            return GetInt(keyName, false, defaultVal, null, null);
        }

        public static int GetInt(string keyName, bool mustExist, int defaultVal, int? min, int? max)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException("keyName");
            }
            string str = ConfigurationManager.AppSettings[keyName];
            if ((str == null) && mustExist)
            {
                throw new ConfigurationErrorsException(string.Format(ErrorInfo.ConfigItemNotSet, keyName));
            }
            return CheckValueRange(StringHelper.TryToInt(str, defaultVal), min, max, defaultVal);
        }

        public static string GetString(string keyName)
        {
            return GetString(keyName, false, string.Empty);
        }

        public static string GetString(string keyName, bool mustExist, string defaultVal)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException("keyName");
            }
            string str = ConfigurationManager.AppSettings[keyName];
            if (str != null)
            {
                return str;
            }
            if (mustExist)
            {
                throw new ConfigurationErrorsException(string.Format(ErrorInfo.ConfigItemNotSet, keyName));
            }
            return defaultVal;
        }
    }
}
