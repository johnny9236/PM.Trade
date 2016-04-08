using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

/// <summary>
///RequestHelp 的摘要说明
/// </summary>
public class RequestHelp
{
    /// <summary>
    ///  并以“参数名=参数值”的形式组成数组
    /// </summary>
    /// <returns>request回来的信息组成的数组</returns>
    public  static string GetRequestGet()
    {
        string rtnStr = string.Empty;
        int i = 0;
        NameValueCollection coll;
        coll =HttpContext.Current.Request.QueryString;
        String[] requestItem = coll.AllKeys;
        for (i = 0; i < requestItem.Length; i++)
        {
            if (i == 0)
                rtnStr += string.Format("{0}={1}", requestItem[i], HttpContext.Current.Request.QueryString[requestItem[i]]);
            else
                rtnStr += string.Format("&{0}={1}", requestItem[i], HttpContext.Current.Request.QueryString[requestItem[i]]);
        }
        return rtnStr;
    }

    /// <summary>
    ///  并以“参数名=参数值”的形式组成数组
    /// </summary>
    /// <returns>request回来的信息组成的数组</returns>
    public static string GetRequestPost()
    {
        string rtnStr = string.Empty;
        int i = 0;
        NameValueCollection coll;
        coll = HttpContext.Current.Request.Form;
        String[] requestItem = coll.AllKeys;
        for (i = 0; i < requestItem.Length; i++)
        {
            if (i == 0)
                rtnStr += string.Format("{0}={1}", requestItem[i], HttpContext.Current.Request.Form[requestItem[i]]);
            else
                rtnStr += string.Format("&{0}={1}", requestItem[i], HttpContext.Current.Request.Form[requestItem[i]]);
        }
        return rtnStr;
    }





    /// <summary>
    ///  并以“参数名=参数值”的形式组成数组
    /// </summary>
    /// <returns>request回来的信息组成的数组</returns>
    public static SortedDictionary<string, string> GetRequestPostDic()
    {
        int i = 0;
        SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
        NameValueCollection coll;
        //Load Form variables into NameValueCollection variable.
        coll = HttpContext.Current.Request.Form;

        // Get names of all forms into a string array.
        String[] requestItem = coll.AllKeys;

        for (i = 0; i < requestItem.Length; i++)
        {
            sArray.Add(requestItem[i], HttpContext.Current.Request.Form[requestItem[i]]);
        }

        return sArray;
    }
}