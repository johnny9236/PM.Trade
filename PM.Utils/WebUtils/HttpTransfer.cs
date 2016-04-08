using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using PM.Utils.Log;

namespace PM.Utils.WebUtils
{
    /// <summary>
    /// 请求与发送
    /// </summary>
    public class HttpTransfer
    {
        /// <summary>
        /// 回调信息
        /// </summary>
        /// <param name="contentStr">发送内容
        /// 其中中文处理：
        /// HttpUtility.UrlEncode(order.PayRealBankName, System.Text.Encoding.GetEncoding(enCoding))
        /// </param>
        /// <param name="urlStr">请求地址</param>
        /// <param name="enCoding">编码</param>
        /// <param name="rtnCheckStr">返回对比输出（被请求页面去掉没用的html标记）</param>
        /// <param name="roundCount">失败回调次数</param>
        public static bool PostBackToBusinesss(string contentStr, string urlStr, Encoding enCoding, string rtnCheckStr, int roundCount)
        {
            bool result = false;
            string postBack = string.Empty;
            int i = 0;
            try
            {
                //var urlStr = ConfigHelper.GetConfigString("BusinessUrl");
                //var enCoding = ConfigHelper.GetConfigString("enCoding");            
                postBack = HttpTransfer.RequestPost(urlStr, contentStr, enCoding);
                while (postBack.ToLower() != rtnCheckStr.ToLower() && i < roundCount)
                {
                    i++;
                    postBack = HttpTransfer.RequestPost(urlStr, contentStr, enCoding);
                    System.Threading.Thread.Sleep(1000);//暂停1秒
                }
                if (postBack.ToLower() == rtnCheckStr.ToLower())
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("回调给业务系统:" + ex.Message, "回调日志");
            }
            return result;
        }

        /// <summary>
        /// post 数据
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="Context"></param>
        /// <param name="eCode"></param>
        /// <returns></returns>
        public static string RequestPost(string Url, string Context, Encoding eCode)//两个参数分别是Url地址和Post过去的数据
        {
            string PageStr = string.Empty;
            Uri url = new Uri(Url);
            byte[] reqbytes = Encoding.ASCII.GetBytes(Context);
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "post";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = reqbytes.Length;
                Stream stm = req.GetRequestStream();
                stm.Write(reqbytes, 0, reqbytes.Length);

                stm.Close();
                HttpWebResponse wr = (HttpWebResponse)req.GetResponse();
                Stream stream = wr.GetResponseStream();
                StreamReader srd = new StreamReader(stream, eCode);
                PageStr += srd.ReadToEnd();
                stream.Close();
                srd.Close();
            }
            catch (Exception e)
            {
                PageStr += e.Message;
            }
            return PageStr;
        }

        /// <summary>
        /// post 数据
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="ContentType">默认"application/x-www-form-urlencoded"</param>
        /// <param name="Context"></param>
        /// <param name="eCode"></param>
        /// <returns></returns>
        public static string RequestPost(string Url, string ContentType, string Context, Encoding eCode)//两个参数分别是Url地址和Post过去的数据
        {
            string PageStr = string.Empty;
            Uri url = new Uri(Url);
            byte[] reqbytes = Encoding.UTF8.GetBytes(Context);
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "post";
                if (!string.IsNullOrEmpty(ContentType))
                    req.ContentType = ContentType;// "application/x-www-form-urlencoded";
                else
                    req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = reqbytes.Length;
                Stream stm = req.GetRequestStream();
                stm.Write(reqbytes, 0, reqbytes.Length);

                stm.Close();
                HttpWebResponse wr = (HttpWebResponse)req.GetResponse();
                Stream stream = wr.GetResponseStream();
                StreamReader srd = new StreamReader(stream, eCode);
                PageStr += srd.ReadToEnd();
                stream.Close();
                srd.Close();
            }
            catch (Exception e)
            {
                PageStr += e.Message;
            }
            return PageStr;
        }
        /// <summary>
        /// http提交 数据
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="httpMethod">get  or  post</param>
        /// <param name="ContentType">默认"application/x-www-form-urlencoded"</param>
        /// <param name="Context"></param>
        /// <param name="eCode"></param>
        /// <returns></returns>
        public static string HttpRequest(string Url, string httpMethod, string ContentType, string Context, Encoding eCode)//两个参数分别是Url地址和Post过去的数据
        {
            string PageStr = string.Empty;
            Uri url = new Uri(Url);
            byte[] reqbytes = Encoding.ASCII.GetBytes(Context);
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = httpMethod;
                if (!string.IsNullOrEmpty(ContentType))
                    req.ContentType = ContentType;// "application/x-www-form-urlencoded";
                else
                    req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = reqbytes.Length;
                Stream stm = req.GetRequestStream();
                stm.Write(reqbytes, 0, reqbytes.Length);

                stm.Close();
                HttpWebResponse wr = (HttpWebResponse)req.GetResponse();
                Stream stream = wr.GetResponseStream();
                StreamReader srd = new StreamReader(stream, eCode);
                PageStr += srd.ReadToEnd();
                stream.Close();
                srd.Close();
            }
            catch (Exception e)
            {
                PageStr += e.Message;
            }
            return PageStr;
        }
    }
}
