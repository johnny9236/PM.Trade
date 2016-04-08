using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.Utils
{
    /// <summary>
    /// 字符串获取
    /// </summary>
    public static class SplitWorld
    {
        /// <summary>
        /// 获取中英文混排字符串的实际长度(字节数)
        /// </summary>
        /// <param name="str">要获取长度的字符串</param>
        /// <returns>字符串的实际长度值（字节数）</returns>
        public static int Length(string str)
        {
            if (str.Equals(string.Empty))
                return 0;
            int strlen = 0;
            ASCIIEncoding strData = new ASCIIEncoding();
            //将字符串转换为ASCII编码的字节数字
            byte[] strBytes = strData.GetBytes(str);
            for (int i = 0; i <= strBytes.Length - 1; i++)
            {
                if (strBytes[i] == 63)  //中文都将编码为ASCII编码63,即"?"号
                    strlen++;
                strlen++;
            }
            return strlen;
        }

        /// <summary>截取指定字节长度的字符串</summary> 
        /// <param name="str">原字符串</param>
        ///<param name="len">截取字节长度</param> 
        /// <returns>string</returns>
        public static string SubString(string str, int len)
        {
            string result = string.Empty;// 最终返回的结果
            if (string.IsNullOrEmpty(str))
            {
                return result;
            }
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);
            // 单字节字符长度
            int charLen = str.Length;
            // 把字符平等对待时的字符串长度
            int byteCount = 0;
            // 记录读取进度 
            int pos = 0;
            // 记录截取位置 
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)
                    // 按中文字符计算加 2 
                    {
                        byteCount += 2;
                    }
                    else
                    // 按英文字符计算加 1 
                    {
                        byteCount += 1;
                    }
                    if (byteCount > len)
                    // 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == len)// 记下当前位置
                    {
                        pos = i + 1; break;
                    }
                } if (pos >= 0)
                {
                    result = str.Substring(0, pos);
                }
            }
            else { result = str; } return result;
        }
        /// <summary>截取指定字节长度的字符串</summary> 
        /// <param name="str">原字符串</param>
        /// <param name="start">开始位置</param>
        ///<param name="len">截取字节长度</param> 
        /// <returns>string</returns>
        public static string SubString(string str, int start, int len)
        {
            string result = string.Empty;// 最终返回的结果
            if (string.IsNullOrEmpty(str))
            {
                return result;
            }
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);
            // 单字节字符长度
            int charLen = str.Length;
            // 把字符平等对待时的字符串长度
            int byteCount = 0;
            // 记录读取进度 
            int pos = 0;
            int begin = 0;
            bool isChk = false;//是否有中文
            // 记录截取位置 
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    isChk = false;
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)
                    // 按中文字符计算加 2 
                    {
                        byteCount += 2;
                        isChk = true;
                    }
                    else
                    // 按英文字符计算加 1 
                    {
                        byteCount += 1;
                    }
                    if ((isChk && (byteCount == start + 2)) || (byteCount == start + 1))
                    {
                        //开始位置 
                        begin = i;
                    }
                    if (byteCount > start + len + 1)
                    // 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == start + len)// 记下当前位置
                    {
                        pos = i + 1; break;
                    }
                }

                if (pos >= begin)
                {
                    result = str.Substring(begin, pos - begin);
                }
            }
            else { result = str; } return result;
        }

    }
}
