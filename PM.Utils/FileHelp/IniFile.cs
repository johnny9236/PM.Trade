using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PM.Utils.FileHelp
{
    /// <summary>
    /// ini文件操作
    /// </summary>
    public sealed class IniFile
    {
        private string m_iniPath;
        private static readonly string s_0 = "0";
        private static readonly string s_1 = "1";
        private static readonly char[] s_splitCharArray = new char[] { '\n' };

        public IniFile(string iniPath)
        {
            this.m_iniPath = iniPath;
        }

        public static void DeleteSection(string sectionName, string iniFilePath)
        {
            WritePrivateProfileString(sectionName, IntPtr.Zero, IntPtr.Zero, iniFilePath);
        }

        public static void DeleteValue(string sectionName, string keyName, string iniFilePath)
        {
            WritePrivateProfileString(sectionName, keyName, IntPtr.Zero, iniFilePath);
        }

        public int GetInt(string lpAppName, string lpKeyName, int nDefault)
        {
            return GetPrivateProfileInt(lpAppName, lpKeyName, nDefault, this.m_iniPath);
        }

        public static int GetInt(string lpAppName, string lpKeyName, int nDefault, string fileName)
        {
            return GetPrivateProfileInt(lpAppName, lpKeyName, nDefault, fileName);
        }

        public int GetInt(string lpAppName, string lpKeyName, int nDefault, int min, int max)
        {
            int num = GetPrivateProfileInt(lpAppName, lpKeyName, nDefault, this.m_iniPath);
            if ((num >= min) && (num <= max))
            {
                return num;
            }
            return nDefault;
        }

        public static int GetInt(string lpAppName, string lpKeyName, int nDefault, string fileName, int min, int max)
        {
            int num = GetPrivateProfileInt(lpAppName, lpKeyName, nDefault, fileName);
            if ((num >= min) && (num <= max))
            {
                return num;
            }
            return nDefault;
        }

        public static string[] GetKeyNames(string sectionName, string iniFilePath)
        {
            return GetKeyNames(sectionName, iniFilePath, 0x400);
        }

        public static string[] GetKeyNames(string sectionName, string iniFilePath, int bufferSize)
        {
            char[] buffer = new char[bufferSize];
            int len = GetPrivateProfileString(sectionName, IntPtr.Zero, string.Empty, buffer, bufferSize, iniFilePath);
            return GetStringArrayFromCharArray(buffer, len);
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileString(IntPtr nullString, IntPtr nullString2, string lpDefault, char[] buffer, int nSize, string lpFileName);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileString(string lpAppName, IntPtr nullString, string lpDefault, char[] buffer, int nSize, string lpFileName);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        public static string[] GetSections(string iniFilePath)
        {
            return GetSections(iniFilePath, 0x400);
        }

        public static string[] GetSections(string iniFilePath, int bufferSize)
        {
            char[] buffer = new char[bufferSize];
            int len = GetPrivateProfileString(IntPtr.Zero, IntPtr.Zero, string.Empty, buffer, bufferSize, iniFilePath);
            return GetStringArrayFromCharArray(buffer, len);
        }

        public string GetString(string lpAppName, string lpKeyName, string lpDefault)
        {
            return this.GetString(lpAppName, lpKeyName, lpDefault, 0x200);
        }

        public string GetString(string lpAppName, string lpKeyName, string lpDefault, int nSize)
        {
            StringBuilder lpReturnedString = new StringBuilder(nSize);
            GetPrivateProfileString(lpAppName, lpKeyName, lpDefault, lpReturnedString, nSize, this.m_iniPath);
            return lpReturnedString.ToString();
        }

        public static string GetString(string lpAppName, string lpKeyName, string lpDefault, string fileName)
        {
            return GetString(lpAppName, lpKeyName, lpDefault, 0x200, fileName);
        }

        public static string GetString(string lpAppName, string lpKeyName, string lpDefault, int nSize, string fileName)
        {
            StringBuilder lpReturnedString = new StringBuilder(nSize);
            GetPrivateProfileString(lpAppName, lpKeyName, lpDefault, lpReturnedString, nSize, fileName);
            return lpReturnedString.ToString();
        }

        private static string[] GetStringArrayFromCharArray(char[] buffer, int len)
        {
            if (len <= 0)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                if (buffer[i] == '\0')
                {
                    builder.Append('\n');
                }
                else
                {
                    builder.Append(buffer[i]);
                }
            }
            return builder.ToString().Split(s_splitCharArray, StringSplitOptions.RemoveEmptyEntries);
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool WritePrivateProfileString(string lpAppName, IntPtr nullString, IntPtr nullString2, string lpFileName);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, IntPtr nullString, string lpFileName);
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);
        public bool WriteString(string lpAppName, string lpKeyName, bool value)
        {
            return WritePrivateProfileString(lpAppName, lpKeyName, value ? s_1 : s_0, this.m_iniPath);
        }

        public bool WriteString(string lpAppName, string lpKeyName, int value)
        {
            return WritePrivateProfileString(lpAppName, lpKeyName, value.ToString(), this.m_iniPath);
        }

        public bool WriteString(string lpAppName, string lpKeyName, string lpString)
        {
            return WritePrivateProfileString(lpAppName, lpKeyName, lpString, this.m_iniPath);
        }

        public static bool WriteString(string lpAppName, string lpKeyName, bool value, string fileName)
        {
            return WritePrivateProfileString(lpAppName, lpKeyName, value ? s_1 : s_0, fileName);
        }

        public static bool WriteString(string lpAppName, string lpKeyName, int value, string fileName)
        {
            return WritePrivateProfileString(lpAppName, lpKeyName, value.ToString(), fileName);
        }

        public static bool WriteString(string lpAppName, string lpKeyName, string lpString, string fileName)
        {
            return WritePrivateProfileString(lpAppName, lpKeyName, lpString, fileName);
        }
    }
}
