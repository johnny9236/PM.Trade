using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.Web;
using System.Security.Cryptography;
using System.Web.UI;
using PM.Utils.ReflectionHelp;
using System.Globalization;

namespace PM.Utils
{
    public delegate bool StringIsBool(string str);
    public class StringHelper
    {
        #region
        /// <summary>
        /// 判断输入的字符是不是全为数字
        /// </summary>
        /// <param name="Str">要判断的字符串</param>
        /// <returns>是否为字符：True或False</returns>
        public static bool IsNum(string Str)
        {
            for (int i = 0; i < Str.Length; i++)
            {
                if (!Char.IsNumber(Str, i))
                {
                    return false; //输入的不是数字  
                }
            }
            return true; //否则是数字
        }
        /// <summary>
        /// 判断输入的字符串全是字符不包括数字，不是返回false，否则返回true
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static bool IsChar(string Str)
        {
            int count = 0;
            for (int i = 0; i < Str.Length; i++)
            {
                if (!Char.IsNumber(Str, i)) //不是数字
                {
                    count++;
                }
            }
            if (count == Str.Length)
            {
                return true; //输入全是字符，不包括数字
            }
            else
            {
                return false; //否则是数字
            }
        }
        /// <summary>
        /// 替换内容中特殊字符为全角
        /// </summary>
        /// <param name="str">要替换的字符</param>
        /// <returns>替换后的结果字符串</returns>
        public static string ReplaceStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            str = str.Replace("'", "‘");
            str = str.Replace(";", "；");
            str = str.Replace(",", "，");
            if (!false)
            {
                str = str.Replace("?", "？");
            }
            str = str.Replace("<", "＜");
            str = str.Replace(">", "＞");
            str = str.Replace("(", "(");
            str = str.Replace(")", ")");
            str = str.Replace("@", "＠");
            str = str.Replace("=", "＝");
            do
            {
                str = str.Replace("+", "＋");
                str = str.Replace("*", "＊");
            }
            while (false);
            str = str.Replace("&", "＆");
            str = str.Replace("#", "＃");
            if (true)
            {
                str = str.Replace("%", "％");
                str = str.Replace("$", "￥");
            }
            return str;
        }
        /// <summary>
        /// 格式化字节数字符串
        /// </summary>
        /// <param name="bytes">字节数</param>
        /// <returns>格式化的结果</returns>
        public static string FormatBytesStr(int bytes)
        {
            double num = 0; ;
            if (bytes > 1073741824)
            {
                num = (double)(bytes / 1073741824);
                return num.ToString("0") + "G";
            }
            double num2 = 0; ;
            if (bytes > 1048576)
            {
                num2 = (double)(bytes / 1048576);
                return num2.ToString("0") + "M";
            }
            while (bytes <= 1024)
            {
                bool flag = ((uint)num & 0u) == 0u;
                if (!flag)
                {
                    if ((uint)num2 + (uint)num > 4294967295u)
                    {
                        continue;
                    }
                }
                return bytes.ToString() + "Bytes";
            }
            return ((double)(bytes / 1024)).ToString("0") + "K";
        }
        /// <summary>
        /// 转换为简体中文
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns></returns>
        public static string ToSChinese(string str)
        {
            return Strings.StrConv(str, VbStrConv.SimplifiedChinese, 0);
        }
        /// <summary>
        /// 转换为繁体中文
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns></returns>
        public static string ToTChinese(string str)
        {
            return Strings.StrConv(str, VbStrConv.TraditionalChinese, 0);
        }
        /// <summary>
        /// 将字符串转换为Color
        /// </summary>
        /// <param name="colorName">字符串颜色：#000000</param>
        /// <returns></returns>
        public static Color ToColor(string colorName)
        {
            if (colorName.StartsWith("#"))
                colorName = colorName.Replace("#", string.Empty);
            int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
            return Color.FromArgb(Convert.ToByte((v >> 24) & 255), Convert.ToByte((v >> 16) & 255), Convert.ToByte((v >> 8) & 255),
                    Convert.ToByte((v >> 0) & 255));
        }
        /// <summary>
        /// 检查一个字符串是否可以转化为日期，一般用于验证用户输入日期的合法性。
        /// </summary>
        /// <param name="_value">需验证的字符串。</param>
        /// <returns>是否可以转化为日期的bool值。</returns>
        public static bool IsStringDate(string _value)
        {
            bool result = false;
            try
            {
                DateTime.Parse(_value);
            }
            catch (FormatException)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveUnsafeHtml(string content)
        {
            content = Regex.Replace(content, "(\\<|\\s+)o([a-z]+\\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "(script|frame|form|meta|behavior|style)([\\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }
        /// <summary>
        /// 分割字符串
        /// <param name="strContent">要分割的字符串</param>
        /// <param name="strSplit">分隔符</param>
        /// </summary>
        public static string[] Split(string strContent, string strSplit)
        {
            if (strContent.IndexOf(strSplit) < 0)
            {
                return new string[]
				{
					strContent
				};
            }
            return Regex.Split(strContent, strSplit.Replace(".", "\\."), RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        /// <param name="str">要删除逗号的字符串</param>
        /// <returns>删除逗号后的字符串</returns>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        /// <param name="str">未删除的字符串</param>
        /// <param name="strchar">要删除之后的字符串</param>
        /// <returns>删除后的字符串</returns>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }

        /// <summary>
        /// 将指定字符串复制到剪贴板'
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="ClipblardStr">要复制到剪贴板的字符串内容</param>
        public static void ClipboardData(Page page, string ClipblardStr)
        {
            if (!string.IsNullOrEmpty(ClipblardStr))
            {
                string text = "<script language='javascript'>";
                text = text + "window.clipboardData.setData('text', '" + ClipblardStr + "')";
                text += "</script>";
                page.ClientScript.RegisterStartupScript(page.GetType(), "message", text);
            }
        }
        /// <summary>
        /// 将Double类型的数据四舍五入
        /// </summary>
        /// <param name="Doubles">要四舍五入的Double类型数据</param>
        /// <param name="Point">保留小数点位数</param>
        /// <returns></returns>
        public static string DoubleToRound(double Doubles, int Point)
        {
            return Doubles.ToString("F" + Point);
        }
        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <param name="Str">字符串</param>
        /// <returns>字符长度</returns>
        public static int GetStringLength(string Str)
        {
            return Encoding.Default.GetBytes(Str).Length;
        }
        /// <summary>
        /// 将GB2312编码字符串转为UTF8
        /// </summary>
        /// <param name="Gb2312Str">GB2312编码字符串</param>
        /// <returns></returns>
        public static string GB2312ToUTF8(string Gb2312Str)
        {
            string result;
            try
            {
                Encoding uTF = Encoding.UTF8;
                Encoding encoding = Encoding.GetEncoding("gb2312");
                byte[] bytes = encoding.GetBytes(Gb2312Str);
                byte[] bytes2 = Encoding.Convert(encoding, uTF, bytes);
                string @string = uTF.GetString(bytes2);
                do
                {
                    result = @string;
                }
                while (false);
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        #endregion

        #region 静态方法
        /// <summary>
        /// 对字符串进行base64编码
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>base64编码串</returns>
        public static string Base64StringEncode(string input)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(encbuff);
        }

        /// <summary>
        /// 对字符串进行反编码
        /// </summary>
        /// <param name="input">base64编码串</param>
        /// <returns>字符串</returns>
        public static string Base64StringDecode(string input)
        {
            byte[] decbuff = Convert.FromBase64String(input);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }

        /// <summary>
        /// 替换字符串(忽略大小写)
        /// </summary>
        /// <param name="input">要进行替换的内容</param>
        /// <param name="oldValue">旧字符串</param>
        /// <param name="newValue">新字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string CaseInsensitiveReplace(string input, string oldValue, string newValue)
        {
            Regex regEx = new Regex(oldValue, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Replace(input, newValue);
        }

        /// <summary>
        /// 替换首次出现的字符串
        /// </summary>
        /// <param name="input">要进行替换的内容</param>
        /// <param name="oldValue">旧字符串</param>
        /// <param name="newValue">新字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceFirst(string input, string oldValue, string newValue)
        {
            Regex regEx = new Regex(oldValue, RegexOptions.Multiline);
            return regEx.Replace(input, newValue, 1);
        }

        /// <summary>
        /// 替换最后一次出现的字符串
        /// </summary>
        /// <param name="input">要进行替换的内容</param>
        /// <param name="oldValue">旧字符串</param>
        /// <param name="newValue">新字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceLast(string input, string oldValue, string newValue)
        {
            int index = input.LastIndexOf(oldValue);
            if (index < 0)
            {
                return input;
            }
            else
            {
                StringBuilder sb = new StringBuilder(input.Length - oldValue.Length + newValue.Length);
                sb.Append(input.Substring(0, index));
                sb.Append(newValue);
                sb.Append(input.Substring(index + oldValue.Length, input.Length - index - oldValue.Length));
                return sb.ToString();
            }
        }

        /// <summary>
        /// 根据词组过虑字符串(忽略大小写)
        /// </summary>
        /// <param name="input">要进行过虑的内容</param>
        /// <param name="filterWords">要过虑的词组</param>
        /// <returns>过虑后的字符串</returns>
        public static string FilterWords(string input, params string[] filterWords)
        {
            return StringHelper.FilterWords(input, char.MinValue, filterWords);
        }

        /// <summary>
        /// 根据词组过虑字符串(忽略大小写)
        /// </summary>
        /// <param name="input">要进行过虑的内容</param>
        /// <param name="mask">字符掩码</param>
        /// <param name="filterWords">要过虑的词组</param>
        /// <returns>过虑后的字符串</returns>
        public static string FilterWords(string input, char mask, params string[] filterWords)
        {
            string stringMask = mask == char.MinValue ? string.Empty : mask.ToString();
            string totalMask = stringMask;

            foreach (string s in filterWords)
            {
                Regex regEx = new Regex(s, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                if (stringMask.Length > 0)
                {
                    for (int i = 1; i < s.Length; i++)
                        totalMask += stringMask;
                }

                input = regEx.Replace(input, totalMask);

                totalMask = stringMask;
            }

            return input;
        }

        public static MatchCollection HasWords(string input, params string[] hasWords)
        {
            StringBuilder sb = new StringBuilder(hasWords.Length + 50);
            //sb.Append("[");

            foreach (string s in hasWords)
            {
                sb.AppendFormat("({0})|", StringHelper.HtmlSpecialEntitiesEncode(s.Trim()));
            }

            string pattern = sb.ToString();
            pattern = pattern.TrimEnd('|'); // +"]";

            Regex regEx = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Matches(input);
        }

        /// <summary>
        /// Html编码
        /// </summary>
        /// <param name="input">要进行编辑的字符串</param>
        /// <returns>Html编码后的字符串</returns>
        public static string HtmlSpecialEntitiesEncode(string input)
        {
            return HttpUtility.HtmlEncode(input);
        }

        /// <summary>
        /// Html解码
        /// </summary>
        /// <param name="input">要进行解码的字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string HtmlSpecialEntitiesDecode(string input)
        {
            return HttpUtility.HtmlDecode(input);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">要进行加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5String(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 对字符串进行MD5较验
        /// </summary>
        /// <param name="input">要进行较验的字符串</param>
        /// <param name="hash">散列串</param>
        /// <returns>是否匹配</returns>
        public static bool MD5VerifyString(string input, string hash)
        {
            string hashOfInput = StringHelper.MD5String(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string PadLeftHtmlSpaces(string input, int totalSpaces)
        {
            string space = "&nbsp;";
            return PadLeft(input, space, totalSpaces * space.Length);
        }

        public static string PadLeft(string input, string pad, int totalWidth)
        {
            return StringHelper.PadLeft(input, pad, totalWidth, false);
        }

        public static string PadLeft(string input, string pad, int totalWidth, bool cutOff)
        {
            if (input.Length >= totalWidth)
                return input;

            int padCount = pad.Length;
            string paddedString = input;

            while (paddedString.Length < totalWidth)
            {
                paddedString += pad;
            }

            if (cutOff)
                paddedString = paddedString.Substring(0, totalWidth);

            return paddedString;
        }

        public static string PadRightHtmlSpaces(string input, int totalSpaces)
        {
            string space = "&nbsp;";
            return PadRight(input, space, totalSpaces * space.Length);
        }

        public static string PadRight(string input, string pad, int totalWidth)
        {
            return StringHelper.PadRight(input, pad, totalWidth, false);
        }

        public static string PadRight(string input, string pad, int totalWidth, bool cutOff)
        {
            if (input.Length >= totalWidth)
                return input;

            string paddedString = string.Empty;

            while (paddedString.Length < totalWidth - input.Length)
            {
                paddedString += pad;
            }

            if (cutOff)
                paddedString = paddedString.Substring(0, totalWidth - input.Length);

            paddedString += input;

            return paddedString;
        }

        /// <summary>
        /// 去除新行
        /// </summary>
        /// <param name="input">要去除新行的字符串</param>
        /// <returns>已经去除新行的字符串</returns>
        public static string RemoveNewLines(string input)
        {
            return StringHelper.RemoveNewLines(input, false);
        }

        /// <summary>
        /// 去除新行
        /// </summary>
        /// <param name="input">要去除新行的字符串</param>
        /// <param name="addSpace">是否添加空格</param>
        /// <returns>已经去除新行的字符串</returns>
        public static string RemoveNewLines(string input, bool addSpace)
        {
            string replace = string.Empty;
            if (addSpace)
                replace = " ";

            string pattern = @"[\r|\n]";
            Regex regEx = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return regEx.Replace(input, replace);
        }

        /// <summary>
        /// 字符串反转
        /// </summary>
        /// <param name="input">要进行反转的字符串</param>
        /// <returns>反转后的字符串</returns>
        public static string Reverse(string input)
        {
            char[] reverse = new char[input.Length];
            for (int i = 0, k = input.Length - 1; i < input.Length; i++, k--)
            {
                if (char.IsSurrogate(input[k]))
                {
                    reverse[i + 1] = input[k--];
                    reverse[i++] = input[k];
                }
                else
                {
                    reverse[i] = input[k];
                }
            }
            return new System.String(reverse);
        }

        /// <summary>
        /// 转成首字母大字形式
        /// </summary>
        /// <param name="input">要进行转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string SentenceCase(string input)
        {
            if (input.Length < 1)
                return input;

            string sentence = input.ToLower();
            return sentence[0].ToString().ToUpper() + sentence.Substring(1);
        }

        /// <summary>
        /// 空格转换成&nbsp;
        /// </summary>
        /// <param name="input">要进行转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string SpaceToNbsp(string input)
        {
            string space = "&nbsp;";
            return input.Replace(" ", space);
        }

        /// <summary>
        /// 去除"<" 和 ">" 符号之间的内容
        /// </summary>
        /// <param name="input">要进行处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string StripTags(string input)
        {
            Regex stripTags = new Regex("<(.|\n)+?>");
            return stripTags.Replace(input, "");
        }

        public static string TitleCase(string input)
        {
            return TitleCase(input, true);
        }

        public static string TitleCase(string input, bool ignoreShortWords)
        {
            List<string> ignoreWords = null;
            if (ignoreShortWords)
            {
                //TODO: Add more ignore words?
                ignoreWords = new List<string>();
                ignoreWords.Add("a");
                ignoreWords.Add("is");
                ignoreWords.Add("was");
                ignoreWords.Add("the");
            }

            string[] tokens = input.Split(' ');
            StringBuilder sb = new StringBuilder(input.Length);
            foreach (string s in tokens)
            {
                if (ignoreShortWords == true
                    && s != tokens[0]
                    && ignoreWords.Contains(s.ToLower()))
                {
                    sb.Append(s + " ");
                }
                else
                {
                    sb.Append(s[0].ToString().ToUpper());
                    sb.Append(s.Substring(1).ToLower());
                    sb.Append(" ");
                }
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// 去除字符串内的空白字符
        /// </summary>
        /// <param name="input">要进行处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string TrimIntraWords(string input)
        {
            Regex regEx = new Regex(@"[\s]+");
            return regEx.Replace(input, " ");
        }

        /// <summary>
        /// 换行符转换成Html标签的换行符<br />
        /// </summary>
        /// <param name="input">要进行处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string NewLineToBreak(string input)
        {
            Regex regEx = new Regex(@"[\n|\r]+");
            return regEx.Replace(input, "<br />");
        }

        /// <summary>
        /// 插入换行符(不中断单词)
        /// </summary>
        /// <param name="input">要进行处理的字符串</param>
        /// <param name="charCount">每行字符数</param>
        /// <returns>处理后的字符串</returns>
        public static string WordWrap(string input, int charCount)
        {
            return StringHelper.WordWrap(input, charCount, false, Environment.NewLine);
        }

        /// <summary>
        /// 插入换行符
        /// </summary>
        /// <param name="input">要进行处理的字符串</param>
        /// <param name="charCount">每行字符数</param>
        /// <param name="cutOff">如果为真，将在单词的中部断开</param>
        /// <returns>处理后的字符串</returns>
        public static string WordWrap(string input, int charCount, bool cutOff)
        {
            return StringHelper.WordWrap(input, charCount, cutOff, Environment.NewLine);
        }

        /// <summary>
        /// 插入换行符
        /// </summary>
        /// <param name="input">要进行处理的字符串</param>
        /// <param name="charCount">每行字符数</param>
        /// <param name="cutOff">如果为真，将在单词的中部断开</param>
        /// <param name="breakText">插入的换行符号</param>
        /// <returns>处理后的字符串</returns>
        public static string WordWrap(string input, int charCount, bool cutOff, string breakText)
        {
            StringBuilder sb = new StringBuilder(input.Length + 100);
            int counter = 0;

            if (cutOff)
            {
                while (counter < input.Length)
                {
                    if (input.Length > counter + charCount)
                    {
                        sb.Append(input.Substring(counter, charCount));
                        sb.Append(breakText);
                    }
                    else
                    {
                        sb.Append(input.Substring(counter));
                    }
                    counter += charCount;
                }
            }
            else
            {
                string[] strings = input.Split(' ');
                for (int i = 0; i < strings.Length; i++)
                {
                    counter += strings[i].Length + 1; // the added one is to represent the inclusion of the space.
                    if (i != 0 && counter > charCount)
                    {
                        sb.Append(breakText);
                        counter = 0;
                    }

                    sb.Append(strings[i] + ' ');
                }
            }
            return sb.ToString().TrimEnd(); // to get rid of the extra space at the end.
        }
        #endregion

        #region
        /// <summary>   
        /// 出错时弹出提示对话框   
        /// </summary>   
        /// <param name="str_Control_Name">检验控件id值</param>   
        /// <param name="str_Form_Name">表单id值</param>   
        /// <param name="str_Prompt">提示信息</param>   
        /// <returns>string</returns>   
        public static string JsIsNull(string str_Control_Name, string str_Form_Name, string str_Prompt)
        {
            return "<script language=\"javascript\">alert('" + str_Prompt + "');document." + str_Form_Name + "." + str_Control_Name + ".focus(); document." + str_Form_Name + "." + str_Control_Name + ".select();</" + "script>";
        }


        /// <summary>   
        /// 出错时弹出提示对话框   
        /// </summary>   
        /// <param name="str_Prompt">提示信息</param>   
        /// <returns>string</returns>   
        public static string JsAlert(string str_Prompt)
        {
            return "<script language=\"javascript\">alert('" + str_Prompt + "');</" + "script>";
        }


        /// <summary>   
        /// 关闭对话框   
        /// </summary>   
        /// <param name="str_Prompt">提示信息</param>   
        /// <returns>string</returns>   
        public static string CloseParent(string str_Prompt)
        {
            return "<script language=\"javascript\">alert('" + str_Prompt + "');window.parent.close();</" + "script>";
        }

        /// <summary>   
        /// 出错时弹出提示对话框--关闭窗口   
        /// </summary>   
        /// <param name="str_Prompt">提示信息</param>   
        /// <param name="isReLoad">true为上个窗口自动刷新</param>   
        /// <returns>string</returns>   
        public static string JsIsNull(string str_Prompt, bool isReLoad)
        {
            if (isReLoad)
            {
                return "<script language=\"javascript\">alert('" + str_Prompt + "');opener.window.document.location.reload();window.close();</" + "script>";
            }
            else
            {
                return "<script language=\"javascript\">alert('" + str_Prompt + "');window.close();</" + "script>";
            }
        }

        /// <summary>   
        ///是否关闭窗口   
        /// </summary>   
        /// <param name="str_Prompt">提示信息</param>   
        /// <param name="isClose">true为关闭</param>   
        /// <returns>string</returns>   
        public static string JsIsClose(string str_Prompt, bool isClose)
        {
            if (!isClose)
            {
                return "<script language=\"javascript\">alert('" + str_Prompt + "');</" + "script>";
            }
            else
            {
                return "<script language=\"javascript\">alert('" + str_Prompt + "');window.close();opener.window.document.location.reload();</" + "script>";
            }
        }

        /// <summary>   
        /// 弹出信息并重装窗口   
        /// </summary>   
        /// <param name="str_Prompt">提示信息</param>   
        /// <param name="reLoadPath">重装路径</param>   
        /// <returns>string</returns>   
        public static string JsIsReLoad(string str_Prompt, string reLoadPath)
        {
            return "<script language=\"javascript\">alert('" + str_Prompt + "');this.window.document.location.reload('" + reLoadPath + "');</" + "script>";
        }

        /// <summary>   
        /// 重装窗口   
        /// </summary>   
        /// <param name="reLoadPath">提示信息</param>   
        /// <returns>string</returns>   
        public static string JsIsReLoad(string reLoadPath)
        {
            return "<script language=\"javascript\">this.window.document.location.reload('" + reLoadPath + "');</" + "script>";
        }

        /// <summary>   
        /// 获得一个16位时间随机数   
        /// </summary>   
        /// <returns>返回随机数</returns>   
        public static string GetDataRandom()
        {
            string strData = DateTime.Now.ToString();
            strData = strData.Replace(":", "");
            strData = strData.Replace("-", "");
            strData = strData.Replace(" ", "");
            Random r = new Random();
            strData = strData + r.Next(100000);
            return strData;
        }

        /// <summary>   
        ///  获得某个字符串在另个字符串中出现的次数   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strSymbol">符号</param>   
        /// <returns>返回值</returns>   
        public static int GetStrCount(string strOriginal, string strSymbol)
        {
            int count = 0;
            for (int i = 0; i < (strOriginal.Length - strSymbol.Length + 1); i++)
            {
                if (strOriginal.Substring(i, strSymbol.Length) == strSymbol)
                {
                    count = count + 1;
                }
            }
            return count;
        }

        /// <summary>   
        /// 获得某个字符串在另个字符串第一次出现时前面所有字符   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strSymbol">符号</param>   
        /// <returns>返回值</returns>   
        public static string GetFirstStr(string strOriginal, string strSymbol)
        {
            int strPlace = strOriginal.IndexOf(strSymbol);
            if (strPlace != -1)
                strOriginal = strOriginal.Substring(0, strPlace);
            return strOriginal;
        }

        /// <summary>   
        /// 获得某个字符串在另个字符串最后一次出现时后面所有字符   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strSymbol">符号</param>   
        /// <returns>返回值</returns>   
        public static string GetLastStr(string strOriginal, string strSymbol)
        {
            int strPlace = strOriginal.LastIndexOf(strSymbol) + strSymbol.Length;
            strOriginal = strOriginal.Substring(strPlace);
            return strOriginal;
        }

        /// <summary>   
        /// 获得两个字符之间第一次出现时前面所有字符   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strFirst">最前哪个字符</param>   
        /// <param name="strLast">最后哪个字符</param>   
        /// <returns>返回值</returns>   
        public static string GetTwoMiddleFirstStr(string strOriginal, string strFirst, string strLast)
        {
            strOriginal = GetFirstStr(strOriginal, strLast);
            strOriginal = GetLastStr(strOriginal, strFirst);
            return strOriginal;
        }

        /// <summary>   
        ///  获得两个字符之间最后一次出现时的所有字符   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strFirst">最前哪个字符</param>   
        /// <param name="strLast">最后哪个字符</param>   
        /// <returns>返回值</returns>   
        public static string GetTwoMiddleLastStr(string strOriginal, string strFirst, string strLast)
        {
            strOriginal = GetLastStr(strOriginal, strFirst);
            strOriginal = GetFirstStr(strOriginal, strLast);
            return strOriginal;
        }

        /// <summary>   
        /// 从数据库表读记录时,能正常显示   
        /// </summary>   
        /// <param name="strContent">要处理的字符</param>   
        /// <returns>返回正常值</returns>   
        public static string GetHtmlFormat(string strContent)
        {
            strContent = strContent.Trim();

            if (strContent == null)
            {
                return "";
            }
            strContent = strContent.Replace("<", "<");
            strContent = strContent.Replace(">", ">");
            strContent = strContent.Replace("\n", "<br />");
            return (strContent);
        }

        /// <summary>   
        /// 检查相等之后，获得字符串   
        /// </summary>   
        /// <param name="str">字符串1</param>   
        /// <param name="checkStr">字符串2</param>   
        /// <param name="reStr">相等之后要返回的字符串</param>   
        /// <returns>返回字符串</returns>   
        public static string GetCheckStr(string str, string checkStr, string reStr)
        {
            if (str == checkStr)
            {
                return reStr;
            }
            return "";
        }

        /// <summary>   
        /// 检查相等之后，获得字符串   
        /// </summary>   
        /// <param name="str">数值1</param>   
        /// <param name="checkStr">数值2</param>   
        /// <param name="reStr">相等之后要返回的字符串</param>   
        /// <returns>返回字符串</returns>   
        public static string GetCheckStr(int str, int checkStr, string reStr)
        {
            if (str == checkStr)
            {
                return reStr;
            }
            return "";
        }
        /// <summary>   
        /// 检查相等之后，获得字符串   
        /// </summary>   
        /// <param name="str"></param>   
        /// <param name="checkStr"></param>   
        /// <param name="reStr"></param>   
        /// <returns></returns>   
        public static string GetCheckStr(bool str, bool checkStr, string reStr)
        {
            if (str == checkStr)
            {
                return reStr;
            }
            return "";
        }
        /// <summary>   
        /// 检查相等之后，获得字符串   
        /// </summary>   
        /// <param name="str"></param>   
        /// <param name="checkStr"></param>   
        /// <param name="reStr"></param>   
        /// <returns></returns>   
        public static string GetCheckStr(object str, object checkStr, string reStr)
        {
            if (str == checkStr)
            {
                return reStr;
            }
            return "";
        }
        /// <summary>   
        /// 截取左边规定字数字符串,超过字数用endStr结束   
        /// </summary>   
        /// <param name="str">需截取字符串</param>   
        /// <param name="length">截取字数</param>   
        /// <param name="endStr">超过字数，结束字符串，如"..."</param>   
        /// <returns>返回截取字符串</returns>   
        public static string GetLeftStr(string str, int length, string endStr)
        {
            string reStr;
            if (length < GetStrLength(str))
            {
                reStr = str.Substring(0, length) + endStr;
            }
            else
            {
                reStr = str;
            }
            return reStr;
        }

        /// <summary>   
        /// 截取左边规定字数字符串,超过字数用...结束   
        /// </summary>   
        /// <param name="str">需截取字符串</param>   
        /// <param name="length">截取字数</param>   
        /// <returns>返回截取字符串</returns>   
        public static string GetLeftStr(string str, int length)
        {
            string reStr;
            if (length < str.Length)
            {
                reStr = str.Substring(0, length) + "...";
            }
            else
            {
                reStr = str;
            }
            return reStr;
        }

        /// <summary>   
        /// 截取左边规定字数字符串,超过字数用...结束   
        /// </summary>   
        /// <param name="str">需截取字符串</param>   
        /// <param name="length">截取字数</param>   
        /// <param name="subcount">若超过字数右边减少的字符长度</param>   
        /// <returns>返回截取字符串</returns>   
        public static string GetLeftStr(string str, int length, int subcount)
        {
            string reStr;
            if (length < str.Length)
            {
                reStr = str.Substring(0, length - subcount) + "...";
            }
            else
            {
                reStr = str;
            }
            return reStr;
        }

        /// <summary>   
        /// 获得双字节字符串的字节数   
        /// </summary>   
        /// <param name="str">要检测的字符串</param>   
        /// <returns>返回字节数</returns>   
        public static int GetStrLength(string str)
        {
            ASCIIEncoding n = new ASCIIEncoding();
            byte[] b = n.GetBytes(str);
            int l = 0;  // l 为字符串之实际长度   
            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] == 63)  //判断是否为汉字或全脚符号   
                {
                    l++;
                }
                l++;
            }
            return l;
        }

        /// <summary>   
        /// 剥去HTML标签   
        /// </summary>   
        /// <param name="text">带有HTML格式的字符串</param>   
        /// <returns>string</returns>   
        public static string RegStripHtml(string text)
        {
            string reStr;
            string RePattern = @"<\s*(\S+)(\s[^>]*)?>";
            reStr = Regex.Replace(text, RePattern, string.Empty, RegexOptions.Compiled);
            reStr = Regex.Replace(reStr, @"\s+", string.Empty, RegexOptions.Compiled);
            return reStr;
        }

        /// <summary>   
        /// 使Html失效,以文本显示   
        /// </summary>   
        /// <param name="str">原字符串</param>   
        /// <returns>失效后字符串</returns>   
        public static string ReplaceHtml(string str)
        {
            str = str.Replace("<", "<");
            return str;
        }


        /// <summary>   
        /// 获得随机数字   
        /// </summary>   
        /// <param name="Length">随机数字的长度</param>   
        /// <returns>返回长度为 Length 的　<see cref="System.Int32"/> 类型的随机数</returns>   
        /// <example>   
        /// Length 不能大于9,以下为示例演示了如何调用 GetRandomNext：<br />   
        /// <code>   
        ///  int le = GetRandomNext(8);   
        /// </code>   
        /// </example>   
        public static int GetRandomNext(int Length)
        {
            if (Length > 9)
                throw new System.IndexOutOfRangeException("Length的长度不能大于10");
            Guid gu = Guid.NewGuid();
            string str = "";
            for (int i = 0; i < gu.ToString().Length; i++)
            {
                if (isNumber(gu.ToString()[i]))
                {
                    str += ((gu.ToString()[i]));
                }
            }
            int guid = int.Parse(str.Replace("-", "").Substring(0, Length));
            if (!guid.ToString().Length.Equals(Length))
                guid = GetRandomNext(Length);
            return guid;
        }

        /// <summary>   
        /// 返回一个 bool 值，指明提供的值是不是整数   
        /// </summary>   
        /// <param name="obj">要判断的值</param>   
        /// <returns>true[是整数]false[不是整数]</returns>   
        /// <remarks>   
        ///  isNumber　只能判断正(负)整数，如果 obj 为小数则返回 false;   
        /// </remarks>   
        /// <example>   
        /// 下面的示例演示了判断 obj 是不是整数：<br />   
        /// <code>   
        ///  bool flag;   
        ///  flag = isNumber("200");   
        /// </code>   
        /// </example>   
        public static bool isNumber(object obj)
        {
            //为指定的正则表达式初始化并编译 Regex 类的实例   
            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^-?(\d*)$");
            //在指定的输入字符串中搜索 Regex 构造函数中指定的正则表达式匹配项   
            System.Text.RegularExpressions.Match mc = rg.Match(obj.ToString());
            //指示匹配是否成功   
            return (mc.Success);
        }

        /// <summary>   
        /// 高亮显示   
        /// </summary>   
        /// <param name="str">原字符串</param>   
        /// <param name="findstr">查找字符串</param>   
        /// <param name="cssclass">Style</param>   
        /// <returns>string</returns>   
        public static string OutHighlightText(string str, string findstr, string cssclass)
        {
            if (findstr != "")
            {
                string text1 = "<span class=\"" + cssclass + "\">%s</span>";
                str = str.Replace(findstr, text1.Replace("%s", findstr));
            }
            return str;
        }

        /// <summary>   
        /// 移除字符串首尾某些字符   
        /// </summary>   
        /// <param name="strOriginal">要操作的字符串</param>   
        /// <param name="startStr">要在字符串首部移除的字符串</param>   
        /// <param name="endStr">要在字符串尾部移除的字符串</param>   
        /// <returns>string</returns>   
        public static string RemoveStartOrEndStr(string strOriginal, string startStr, string endStr)
        {
            char[] start = startStr.ToCharArray();
            char[] end = endStr.ToCharArray();
            return strOriginal.TrimStart(start).TrimEnd(end);
        }

        /// <summary>   
        /// 删除指定位置指定长度字符串   
        /// </summary>   
        /// <param name="strOriginal">要操作的字符串</param>   
        /// <param name="startIndex">开始删除字符的位置</param>   
        /// <param name="count">要删除的字符数</param>   
        /// <returns>string</returns>   
        public static string RemoveStr(string strOriginal, int startIndex, int count)
        {
            return strOriginal.Remove(startIndex, count);
        }

        /// <summary>   
        /// 从左边填充字符串   
        /// </summary>   
        /// <param name="strOriginal">要操作的字符串</param>   
        /// <param name="totalWidth">结果字符串中的字符数</param>   
        /// <param name="paddingChar">填充的字符</param>   
        /// <returns>string</returns>   
        public static string LeftPadStr(string strOriginal, int totalWidth, char paddingChar)
        {
            if (strOriginal.Length < totalWidth)
                return strOriginal.PadLeft(totalWidth, paddingChar);
            return strOriginal;
        }

        /// <summary>   
        /// 从右边填充字符串   
        /// </summary>   
        /// <param name="strOriginal">要操作的字符串</param>   
        /// <param name="totalWidth">结果字符串中的字符数</param>   
        /// <param name="paddingChar">填充的字符</param>   
        /// <returns>string</returns>   
        public static string RightPadStr(string strOriginal, int totalWidth, char paddingChar)
        {
            if (strOriginal.Length < totalWidth)
                return strOriginal.PadRight(totalWidth, paddingChar);
            return strOriginal;
        }
        #endregion
        #region
        /// <summary>
        /// 获取字符长度 中文2个字节
        /// </summary>
        /// <param name="Text">输入字符</param>
        /// <returns>字符长度</returns>
        public static int Text_Length(string Text)
        {
            int len = 0;

            for (int i = 0; i < Text.Length; i++)
            {
                byte[] byte_len = Encoding.Default.GetBytes(Text.Substring(i, 1));
                if (byte_len.Length > 1)
                    len += 2;  //如果长度大于1，是中文，占两个字节，+2
                else
                    len += 1;  //如果长度等于1，是英文，占一个字节，+1
            }

            return len;
        }

        /// <summary>
        /// 获取子字符串(中文规则)
        /// </summary>
        /// <param name="text">字符串（可带中文）</param>
        /// <param name="start">起始位置</param>
        /// <param name="strLeng">长度</param>
        /// <returns></returns>
        public static string Get_SubstringChineseStr(string text, int start, int strLeng)
        {
            string rtnStr = string.Empty;
            string temp_text = string.Empty;
            int startCount = 0;//计数字节计数 
            for (int i = 0; i < text.Length; i++)
            {
                byte[] byte_len = Encoding.Default.GetBytes(text.Substring(i, 1));
                if (byte_len.Length > 1)
                    startCount += 2;  //如果长度大于1，是中文，占两个字节，+2
                else
                    startCount += 1;  //如果长度等于1，是英文，占一个字节，+1
                if (startCount > start)
                {
                    if (strLeng >= (startCount - start))
                    {
                        rtnStr += text.Substring(i, 1);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return rtnStr;
        }
        #endregion

        private static readonly char[] charArray_comma = new char[] { ',' };
        private static readonly char[] charArray_enter = new char[] { '\r', '\n' };
        private static readonly char[] charArray_semicolon = new char[] { ';' };
        private static StringIsBool s_StringIsBoolFunc = new StringIsBool(StringHelper.StringIsBool_MyFunc);

        /// <summary>
        /// base64 转string
        /// </summary>
        /// <param name="base64str">base64字符串</param>
        /// <returns>base64字符串转string</returns>
        public static string Base64ToString(string base64str)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64str));
        }
        /// <summary>
        /// 字符转 通用的类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object ConvertString(string str, Type conversionType)
        {
            if (conversionType == null)
            {
                throw new ArgumentNullException("conversionType");
            }
            if (conversionType == TypeList._string)
            {
                return str;
            }
            if (!string.IsNullOrEmpty(str))
            {
                if (conversionType.IsEnum)
                {
                    return int.Parse(str);
                }
                if (conversionType.IsGenericType)
                {
                    if (conversionType.GetGenericTypeDefinition() != TypeList._nullable)
                    {
                        throw new InvalidCastException();
                    }
                    conversionType = conversionType.GetGenericArguments()[0];
                }
                if (conversionType == TypeList._int)
                {
                    return int.Parse(str);
                }
                if (conversionType == TypeList._long)
                {
                    return long.Parse(str);
                }
                if (conversionType == TypeList._short)
                {
                    return short.Parse(str);
                }
                if (conversionType == TypeList._DateTime)
                {
                    return DateTime.Parse(str);
                }
                if (conversionType == TypeList._bool)
                {
                    return s_StringIsBoolFunc(str);
                }
                if (conversionType == TypeList._double)
                {
                    return double.Parse(str);
                }
                if (conversionType == TypeList._decimal)
                {
                    return decimal.Parse(str);
                }
                if (conversionType == TypeList._float)
                {
                    return float.Parse(str);
                }
                if (conversionType == TypeList._Guid)
                {
                    return new Guid(str);
                }
                if (conversionType == TypeList._ulong)
                {
                    return ulong.Parse(str);
                }
                if (conversionType == TypeList._uint)
                {
                    return uint.Parse(str);
                }
                if (conversionType == TypeList._ushort)
                {
                    return ushort.Parse(str);
                }
                if (conversionType == TypeList._char)
                {
                    return str[0];
                }
                if (conversionType == TypeList._byte)
                {
                    return byte.Parse(str);
                }
                if (conversionType != TypeList._sbyte)
                {
                    throw new InvalidCastException();
                }
                return sbyte.Parse(str);
            }
            if (conversionType == TypeList._DateTime)
            {
                return DateTime.MinValue;
            }
            if (conversionType == TypeList._Guid)
            {
                return Guid.Empty;
            }
            if (conversionType.IsValueType && !conversionType.IsGenericType)
            {
                try
                {
                    return Convert.ChangeType(0, conversionType);
                }
                catch
                {
                    throw new InvalidCastException();
                }
            }
            return null;
        }
        /// <summary>
        /// 字符转md5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5String(string input)
        {
            if (input == null)
            {
                input = string.Empty;
            }
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(input))).Replace("-", "");
        }
        /// <summary>
        /// 字符转sha1加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetSha1String(string input)
        {
            if (input == null)
            {
                input = string.Empty;
            }
            return BitConverter.ToString(new SHA1CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(input))).Replace("-", "");
        }
        /// <summary>
        /// 字符转时间格式yyyy-MM-dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetStdDateFormatString(string date)
        {
            DateTime time;
            if (DateTime.TryParse(date, out time))
            {
                return time.ToString("yyyy-MM-dd");
            }
            return null;
        }
        /// <summary>
        /// 字符转时间格式yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetStdDateTimeFormatString(string time)
        {
            DateTime time2;
            if (DateTime.TryParse(time, out time2))
            {
                return time2.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return null;
        }
        /// <summary>
        /// 16进制转 byte[]
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HexToBin(string hex)
        {
            if (hex == null)
            {
                throw new ArgumentNullException("hex");
            }
            if ((hex.Length % 2) != 0)
            {
                throw new InvalidOperationException(ErrorInfo.HexLenIsWrong);
            }
            byte[] buffer = new byte[hex.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return buffer;
        }

        /// <summary>
        /// 对象是否可以转成bool
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ObjectIsTrue(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Type type = obj.GetType();
            try
            {
                if (type == TypeList._bool)
                {
                    return (bool)obj;
                }
                if (type == TypeList._string)
                {
                    string str = obj.ToString();
                    if (string.IsNullOrEmpty(str))
                    {
                        return false;
                    }
                    return s_StringIsBoolFunc(str);
                }
                return Convert.ToBoolean(obj);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// base64转 string
        /// </summary>
        /// <param name="base64str"></param>
        /// <returns></returns>
        public static string SafeBase64ToString(string base64str)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(base64str));
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// string 转特定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T SafeConvertString<T>(string str)
        {
            if (str == null)
            {
                return default(T);
            }
            try
            {
                return (T)Convert.ChangeType(str, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// 以逗号分割
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static string[] SplitByComma(string line)
        {
            if (line == null)
            {
                throw new ArgumentNullException("line");
            }
            return line.Split(charArray_comma, StringSplitOptions.RemoveEmptyEntries);
        }
        /// <summary>
        /// 按回车换行分割
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string[] SplitByEnter(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            return text.Split(charArray_enter, StringSplitOptions.RemoveEmptyEntries);
        }
        /// <summary>
        /// 以逗号分割
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static string[] SplitBySemicolon(string line)
        {
            if (line == null)
            {
                throw new ArgumentNullException("line");
            }
            return line.Split(charArray_semicolon, StringSplitOptions.RemoveEmptyEntries);
        }
        /// <summary>
        /// 分割字符
        /// </summary>
        /// <param name="line"></param>
        /// <param name="separator1"></param>
        /// <param name="separator2"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> SplitString(string line, char separator1, char separator2)
        {
            if (string.IsNullOrEmpty(line))
            {
                return new List<KeyValuePair<string, string>>();
            }
            string[] strArray = line.Split(new char[] { separator1 }, StringSplitOptions.RemoveEmptyEntries);
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>(strArray.Length);
            char[] separator = new char[] { separator2 };
            foreach (string str in strArray)
            {
                string[] strArray2 = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                if (strArray2.Length != 2)
                {
                    throw new ArgumentException(ErrorInfo.StringFormatInvalid);
                }
                list.Add(new KeyValuePair<string, string>(strArray2[0], strArray2[1]));
            }
            return list;
        }
        /// <summary>
        /// string转 bool方法判断
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool StringIsBool_MyFunc(string str)
        {
            return (((string.Compare(str, "false", true) != 0) && !(str == "0")) && (str.Length > 0));
        }
        /// <summary>
        /// string 是否是guid格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool StringIsGuid(string str)
        {
            if ((str == null) || (str.Length != 0x24))
            {
                return false;
            }
            try
            {
                new Guid(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// string 转base64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToBase64(string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }
        /// <summary>
        /// 字符串转List<int>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="flag">分隔符</param>
        /// <returns></returns>
        public static List<int> StringToIntList(string str, char flag)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new List<int>();
            }
            string[] strArray = str.Split(new char[] { flag }, StringSplitOptions.RemoveEmptyEntries);
            List<int> list = new List<int>(strArray.Length);
            foreach (string str2 in strArray)
            {
                int num;
                if (int.TryParse(str2, out num))
                {
                    list.Add(num);
                }
            }
            return list;
        }
        /// <summary>
        /// 字符转时间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime TryToDateTime(string str)
        {
            return TryToDateTime(str, DateTime.MinValue);
        }
        /// <summary>
        ///  字符转时间
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static DateTime TryToDateTime(string str, DateTime defaultVal)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultVal;
            }
            DateTime result = defaultVal;
            DateTime.TryParse(str, out result);
            return result;
        }
        /// <summary>
        ///  字符转decimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal TryToDecimal(string str)
        {
            return TryToDecimal(str, 0M);
        }
        /// <summary>
        /// 字符转decimal
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static decimal TryToDecimal(string str, decimal defaultVal)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultVal;
            }
            decimal result = defaultVal;
            decimal.TryParse(str, NumberStyles.Currency, null, out result);
            return result;
        }
        /// <summary>
        /// 字符转int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int TryToInt(string str)
        {
            return TryToInt(str, 0);
        }
        /// <summary>
        /// 字符转int
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static int TryToInt(string str, int defaultVal)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultVal;
            }
            int result = defaultVal;
            int.TryParse(str, out result);
            return result;
        }

        public static StringIsBool StringIsBoolFunc
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                s_StringIsBoolFunc = value;
            }
        }

        #region
        #region 全角半角转换
        /// <summary>
        /// 转全角的函数(SBC case)
        /// 全角空格为12288，半角空格为32
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 转半角的函数(DBC case) 
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        #endregion
        #endregion

        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="str">字符串</param> 
        /// <returns>数字</returns>
        public static string GetNumberString(string str)
        {
            string result = string.Empty;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.）  
                str = Regex.Replace(str, @"[^\d.\d]", "");
                result = str;
                // 如果是数字，则转换为decimal类型 
                //if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                //{
                //    result = str;
                //}
            }
            return result;
        }


        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="havePoint">是否包含点号</param>
        /// <returns>数字</returns>
        public static string GetNumberString(string str, bool havePoint)
        {
            string result = string.Empty;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.） 
                if (havePoint)
                    str = Regex.Replace(str, @"[^\d.\d]", "");
                else
                    str = Regex.Replace(str, @"[^\d\d]", "");
                result = str;
                // 如果是数字，则转换为decimal类型 
                //if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                //{
                //    result = str;
                //}
            }
            return result;
        }



        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>数字</returns>
        public static decimal GetNumberDecimal(string str)
        {
            decimal result = 0;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.） 
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型 
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = decimal.Parse(str);
                }
            }
            return result;
        }
        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>数字</returns>
        public static int GetNumberInt(string str)
        {
            int result = 0;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.） 
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为int类型 
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = int.Parse(str);
                }
            }
            return result;
        }

        /// <summary>
        /// 响应获取键值对
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static SortedDictionary<string, string> GetRequestParams(string message)
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            String[] messageArray = message.Split('&');
            Array.ForEach(messageArray,
                p =>
                {
                    var keys = p.Split('=');
                    if (!string.IsNullOrEmpty(keys[0].Trim()))
                        sArray.Add(keys[0], keys[1]);
                });
            return sArray;
        }
        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string Create_linkstring(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }
            return prestr.ToString();
        }
        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param> 
        /// <param name="key">过滤的关键字</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> Para_filter(SortedDictionary<string, string> dicArrayPre, params string[] key)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (Array.TrueForAll(key, p => p.ToLower() == temp.Key.ToLower()) && temp.Value != "")
                {
                    dicArray.Add(temp.Key.ToUpper(), temp.Value);
                }
            }
            return dicArray;
        }
    }
}