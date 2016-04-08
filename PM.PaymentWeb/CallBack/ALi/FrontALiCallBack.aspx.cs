using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using PM.Utils;
using PM.Utils.WCF;
using System.Collections.Specialized;
using PM.Utils.Log;

public partial class CallBack_ALi_FrontALiCallBack : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string showInfo = string.Empty;
        Request.ContentEncoding = Encoding.UTF8;
        Response.ContentEncoding = Encoding.UTF8;
        String message = GetRequestPost("");

        LogTxt.WriteEntry(string.Format("返回报文QueryString形式，message[{0}]-密匙[{1}]", Request.QueryString.ToString(), ""), "支付宝前台");
        String signature = string.Empty;
        if (string.IsNullOrEmpty(message))
        {
            message = GetRequestPost("form");
            LogTxt.WriteEntry(string.Format("form返回报文，message[{0}]-密匙[{1}]", message, ""), "支付宝前台");
        }
        if (string.IsNullOrEmpty(message))
        {
            resultInfo.Text = "获取内容为空!";
            return;
        }
        //  返回
        string url = ConfigHelper.GetConfigString("WcfUrl");
        var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>(url);
        //showInfo = info.PayCallback(message, signature);
        var responseModel = new PM.PaymentModel.PayResopnseModel();
        responseModel.Message = message;
        responseModel.Signature = signature;
        responseModel.BusinessFunNo = "AliPayRtn";
        showInfo = info.PayCallback(responseModel);
        if (!string.IsNullOrEmpty(showInfo))
        {
            this.resultInfo.Text = showInfo;
        }
        else
        {
            this.resultInfo.Text = "返回信息异常";
        }
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