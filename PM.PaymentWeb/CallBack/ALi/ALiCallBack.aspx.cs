using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PM.Utils;
using PM.Utils.WCF;
using PM.Utils.Log;
using System.Text;
using System.Collections.Specialized;

public partial class CallBack_ALi_ALiCallBack : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Request.ContentEncoding = Encoding.UTF8;
        Response.ContentEncoding = Encoding.UTF8;
        String message = GetRequestPost("");  
        LogTxt.WriteEntry(string.Format("返回报文query形式，message[{0}]-密匙[{1}]", message, ""), "支付宝");
        String signature = string.Empty;
        if (string.IsNullOrEmpty(message))
        {
            message = GetRequestPost("form");
            LogTxt.WriteEntry(string.Format("返回报文form形式，message[{0}]-密匙[{1}]", message, ""), "支付宝");
        } 
        if (string.IsNullOrEmpty(message))
        {
            LogTxt.WriteEntry(string.Format("form返回报文为空，报文[{0}]-密匙[{1}]", message, signature), "支付宝");
            return;
        }
        string showRtn = string.Empty;//报文信息用于打印      
        //  支付返回
        string url = ConfigHelper.GetConfigString("WcfUrl");
        var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>(url);
        //showRtn = info.PayCallback(message, signature);
        var responseModel = new PM.PaymentModel.PayResopnseModel();
        responseModel.Message = message;
        responseModel.Signature = signature;
        responseModel.BusinessFunNo = "AliPayRtn";
        showRtn = info.PayCallback(responseModel);
        Response.Write(showRtn);
    }
    /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
    /// </summary>
    /// <returns>request回来的信息组成的字符串</returns>
    public string GetRequestPost(string postMoth)
    {
        string message = string.Empty;
        int i = 0;
        SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
        NameValueCollection coll;
        if (postMoth == "form")
        {
            coll = Request.Form; ;
        }
        else
        { 
            coll = Request.QueryString;
        }
        String[] requestItem = coll.AllKeys;

        for (i = 0; i < requestItem.Length; i++)
        {
            if (postMoth == "form")
            {
                message += string.Format("&{0}={1}", requestItem[i], Request.Form[requestItem[i]]);
            }
            else
            {
                message += string.Format("&{0}={1}", requestItem[i], Request.QueryString[requestItem[i]]);
            }
        }

        return message;
    }
}