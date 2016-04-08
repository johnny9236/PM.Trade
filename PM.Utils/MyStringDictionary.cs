using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace PM.Utils
{
    /// <summary>
    ///    一个字符串字典类，KEY和Value全要求字符串。KEY不区分大小写。主要用于保存“查询字符串”参数
    /// </summary>
    public sealed class MyStringDictionary
    {
        private Dictionary<string, string> _dict;
        private static readonly char[] separator1 = new char[] { '&' };
        private static readonly char[] separator2 = new char[] { '=' };

        public MyStringDictionary()
            : this(20)
        {
        }

        public MyStringDictionary(NameValueCollection collection)
            : this((int)(collection.Count + 5))
        {
            foreach (string str in collection.AllKeys)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    this._dict[str] = collection[str];
                }
            }
        }

        public MyStringDictionary(int capacity)
        {
            this._dict = new Dictionary<string, string>(capacity, StringComparer.CurrentCultureIgnoreCase);
        }

        public MyStringDictionary(string queryStringLine)
            : this()
        {
            this.LoadValuesFromUrl(queryStringLine);
        }

        public MyStringDictionary(NameValueCollection collection1, NameValueCollection collection2)
            : this((int)((collection1.Count + collection2.Count) + 5))
        {
            foreach (string str in collection1.AllKeys)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    this._dict[str] = collection1[str];
                }
            }
            foreach (string str2 in collection2.AllKeys)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    this._dict[str2] = collection2[str2];
                }
            }
        }

        public bool ContainsKey(string key)
        {
            return this._dict.ContainsKey(key);
        }

        public int LoadValuesFromUrl(string line)
        {
            this._dict.Clear();
            if (string.IsNullOrEmpty(line))
            {
                return 0;
            }
            int index = line.IndexOf('?');
            if (index == (line.Length - 1))
            {
                return 0;
            }
            try
            {
                line = line.Substring(index + 1);
                foreach (string str in line.Split(separator1, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] strArray2 = str.Split(separator2, StringSplitOptions.RemoveEmptyEntries);
                    if (strArray2.Length == 2)
                    {
                        this._dict.Add(strArray2[0], strArray2[1]);
                    }
                }
            }
            catch
            {
            }
            return this._dict.Count;
        }

        public bool Remove(string key)
        {
            return this._dict.Remove(key);
        }

        public string ToUrlString(string urlFilePath)
        {
            StringBuilder builder = new StringBuilder(500);
            if (!string.IsNullOrEmpty(urlFilePath))
            {
                builder.Append(urlFilePath).Append('?');
            }
            if (this._dict.Count > 0)
            {
                foreach (KeyValuePair<string, string> pair in this._dict)
                {
                    builder.Append(pair.Key).Append('=').Append(HttpUtility.UrlEncode(pair.Value)).Append('&');
                }
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        public int Count
        {
            get
            {
                return this._dict.Count;
            }
        }

        public string this[string key]
        {
            get
            {
                string str;
                if (this._dict.TryGetValue(key, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                this._dict[key] = value;
            }
        }
    }
}
