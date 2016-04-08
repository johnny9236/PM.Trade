using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace PM.Utils
{
    /// <summary>
    /// 序列化对象类
    /// </summary>
    public static class SerializerHelper
    {
        /// <summary>
        /// 二进制反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T BinDeserialize<T>(byte[] b)
        {
            if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            using (MemoryStream stream = new MemoryStream(b))
            {
                stream.Position = 0L;
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
            }
        }
        /// <summary>
        /// 文件中反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T BinDeserializeFromFile<T>(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            return BinDeserialize<T>(File.ReadAllBytes(path));
        }
        /// <summary>
        /// object 转二进制对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] BinSerialize(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, obj);
                stream.Position = 0L;
                return stream.ToArray();
            }
        }
        /// <summary>
        /// object保存到文件
        /// </summary>
        /// <param name="o"></param>
        /// <param name="path"></param>
        public static void BinSerializeToFile(object o, string path)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            byte[] bytes = BinSerialize(o);
            File.WriteAllBytes(path, bytes);
        }
        /// <summary>
        /// xml转DataTable
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromXmlString(string xmlString)
        {
            if (xmlString == null)
            {
                throw new ArgumentNullException("xmlString");
            }
            DataTable table = new DataTable();
            using (StringReader reader = new StringReader(xmlString))
            {
                table.ReadXml(reader);
                return table;
            }
        }
        /// <summary>
        /// DataTable转xml
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string GetXmlStringFromDataTable(DataTable table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            using (StringWriter writer = new StringWriter())
            {
                table.WriteXml(writer, XmlWriteMode.WriteSchema);
                return writer.ToString();
            }
        }
        /// <summary>
        /// 二进制反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(data))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
        /// <summary>
        /// 字符反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string s, Encoding encoding)
        {
            T local;
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("s");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(encoding.GetBytes(s)))
            {
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    local = (T)serializer.Deserialize(reader);
                }
            }
            return local;
        }
        /// <summary>
        /// xml文件反序列化长对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T XmlDeserializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            return XmlDeserialize<T>(File.ReadAllText(path, encoding), encoding);
        }
        /// <summary>
        /// 对象序列化长二进制
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] XmlSerialize(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize((Stream)stream, obj);
                return stream.ToArray();
            }
        }
        /// <summary>
        /// 对象序列化长二进制
        /// </summary>
        /// <param name="o"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string XmlSerialize(object o, Encoding encoding)
        {
            byte[] bytes = XmlSerializeInternal(o, encoding);
            return encoding.GetString(bytes);
        }
        /// <summary>
        /// xml序列化成二进制
        /// </summary>
        /// <param name="o"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static byte[] XmlSerializeInternal(object o, Encoding encoding)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlTextWriter writer = new XmlTextWriter(stream, encoding))
                {
                    writer.Formatting = Formatting.Indented;
                    serializer.Serialize((XmlWriter)writer, o);
                    writer.Close();
                }
                return stream.ToArray();
            }
        }
        /// <summary>
        /// object对象序列化字符
        /// </summary>
        /// <param name="o"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string XmlSerializeObject(object o, Encoding encoding)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    NewLineChars = "\r\n",
                    Encoding = encoding,
                    OmitXmlDeclaration = true,
                    IndentChars = "\t"
                };
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                XmlWriter xmlWriter = XmlWriter.Create(stream, settings);
                serializer.Serialize(xmlWriter, o, namespaces);
                xmlWriter.Close();
                return encoding.GetString(stream.ToArray());
            }
        }
        /// <summary>
        /// 对象写入文件
        /// </summary>
        /// <param name="o"></param>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        public static void XmlSerializeToFile(object o, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            byte[] bytes = XmlSerializeInternal(o, encoding);
            File.WriteAllBytes(path, bytes);
        }



        /// <summary>
        /// 对象序列化成 XML String(uft8)
        /// </summary>
        public static string XmlSerialize<T>(T obj)
        {
            string xmlString = string.Empty;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, obj);
                xmlString = Encoding.UTF8.GetString(ms.ToArray());
            }
            return xmlString;
        }

        /// <summary>
        /// XML String 反序列化成对象(uft8)
        /// </summary>
        public static T XmlDeserialize<T>(string xmlString)
        {
            T t = default(T);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                {
                    Object obj = xmlSerializer.Deserialize(xmlReader);
                    t = (T)obj;
                }
            }
            return t;
        }

        #region  josin
        /// <summary>
        /// 序列化对象成Json字节        
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Sender"></param>
        /// <returns></returns>
        public static Byte[] SerializeToBytes<T>(T Sender)
        {
            using (MemoryStream TargetBytes = new MemoryStream())
            {
                TargetBytes.WriteByte(255);
                TargetBytes.WriteByte(254);
                DataContractJsonSerializer Serializer = new DataContractJsonSerializer(Sender.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    Serializer.WriteObject(ms, Sender);
                    string jsonText = Encoding.UTF8.GetString(ms.ToArray());
                    byte[] jsonBytes = Encoding.Unicode.GetBytes(jsonText);
                    TargetBytes.Write(jsonBytes, 0, jsonBytes.Length);
                }
                return TargetBytes.ToArray();
            }
        }

        /// <summary>
        /// 字节数组压缩转base64后字符
        /// </summary>
        /// <param name="sourceStr">原字符串</param>
        /// <returns></returns>
        public static string JsonBytesToBase64(byte[] jsonBytes)
        {
            if (jsonBytes.Length == 0)
                return string.Empty;
            string base64Text = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                using (zlib.ZOutputStream outputStream = new zlib.ZOutputStream(ms, zlib.zlibConst.Z_DEFAULT_COMPRESSION))
                {
                    outputStream.Write(jsonBytes, 0, jsonBytes.Length);
                }
                base64Text = Convert.ToBase64String(ms.ToArray());
            }
            return base64Text;
        }

        //<summary>
        //字符串压缩转base64后字符
        //</summary>
        //<param name="sourceStr">原字符串</param>
        //<returns></returns>
        public static string JsonTextToBase64(string sourceStr)
        {
            if (string.IsNullOrWhiteSpace(sourceStr))
                return string.Empty;
            string rtnStr = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                using (zlib.ZOutputStream outputStream = new zlib.ZOutputStream(ms, zlib.zlibConst.Z_DEFAULT_COMPRESSION))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(sourceStr);
                    outputStream.Write(buffer, 0, buffer.Length);
                }
                rtnStr = Convert.ToBase64String(ms.ToArray());
            }
            return rtnStr;
        }

        /// <summary>
        /// base64解压转字符串
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static string base64tojsontext(string base64str)
        {
            if (string.IsNullOrWhiteSpace(base64str))
                return string.Empty;
            string rtnstr = string.Empty;
            byte[] bytes = Convert.FromBase64String(base64str);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Position = 0;
                using (zlib.ZOutputStream outputstream = new zlib.ZOutputStream(ms))
                {
                    outputstream.Position = 0;
                    outputstream.Write(bytes, 0, bytes.Length);
                }
                rtnstr = Encoding.UTF8.GetString(ms.ToArray());
            }
            return rtnstr;
        }

        /// <summary>
        /// base64解压转字符串
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static string Base64ToText(string base64Str)
        {
            if (string.IsNullOrWhiteSpace(base64Str))
                return string.Empty;
            string rtnStr = string.Empty;
            byte[] bytes = Convert.FromBase64String(base64Str);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Position = 0;
                using (zlib.ZOutputStream outputStream = new zlib.ZOutputStream(ms))
                {
                    outputStream.Position = 0;
                    outputStream.Write(bytes, 0, bytes.Length);
                }
                byte[] FixHeader = ms.ToArray();
                if ((FixHeader[0] == 255) && (FixHeader[0] == 255))
                {
                    int offset = 2;
                    int len = ms.ToArray().Length - offset;
                    rtnStr = Encoding.Unicode.GetString(ms.ToArray(), offset, len);

                }
                else
                {
                    int offset = 0;// 2;
                    int len = ms.ToArray().Length;// -offset;
                    rtnStr = Encoding.UTF8.GetString(ms.ToArray(), offset, len);
                }
            }
            return rtnStr;
        }

        /// <summary>
        /// json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string jsonText)
        {
            if (jsonText == "")
                return default(T);
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonText)))
            {
                DataContractJsonSerializer Serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)Serializer.ReadObject(ms);
            }
        }
        #endregion
    }
}
