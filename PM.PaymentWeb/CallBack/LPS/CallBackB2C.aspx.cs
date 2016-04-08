using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PM.Utils.Log;
using PM.Utils;
using PM.Utils.WCF;
using System.Text;
using System.Collections.Specialized;

public partial class CallBack_LPS_CallBackB2C : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Request.ContentEncoding = Encoding.UTF8;
        Response.ContentEncoding = Encoding.UTF8;
        LogTxt.WriteEntry(string.Format(" ReqStr报文[{0}]", Request.QueryString), "b2c后台通知日志");

        String message = GetRequestGetMessage();
            //RequestHelp.GetRequestGet();
        LogTxt.WriteEntry(string.Format(" Get报文[{0}]", message), "b2c后台通知日志");


        if (string.IsNullOrEmpty(message))
        {
            //todo   建行返回判断
            //1、根据编码获取是b2b还是  b2c
            //2、处理获取参数  
            LogTxt.WriteEntry(string.Format("返回报文为空，报文[{0}]", message), "b2c后台通知日志");
            return;
        }
        //  支付返回
        string showRtn = string.Empty;//报文信息用于打印
        string url = ConfigHelper.GetConfigString("WcfUrl");
        var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>(url);
        var responseModel = new PM.PaymentModel.PayResopnseModel();
        responseModel.BusinessFunNo = "LPSBBCB2CResponse";
        responseModel.IsShowBk = true;
        responseModel.Message = message;
        responseModel.Signature = Request["SIGN"];
        showRtn = info.PayCallback(responseModel);
        Response.Write(showRtn);

    }
    public string GetRequestGetMessage()
    {
        string rtnStr = string.Empty;
        int i = 0;
        NameValueCollection coll;
        coll = Request.QueryString;
        String[] requestItem = coll.AllKeys;
        for (i = 0; i < requestItem.Length; i++)
        {
            //if (requestItem[i].ToLower() == "POSID".ToLower() ||
            //    requestItem[i].ToLower() == "BRANCHID".ToLower() ||
            //    requestItem[i].ToLower() == "ORDERID".ToLower() ||
            //    requestItem[i].ToLower() == "PAYMENT".ToLower() ||
            //    requestItem[i].ToLower() == "CURCODE".ToLower() ||
            //    requestItem[i].ToLower() == "REMARK1".ToLower() ||
            //    requestItem[i].ToLower() == "REMARK2".ToLower() ||
            //    requestItem[i].ToLower() == "SUCCESS".ToLower())
            if (requestItem[i].ToLower() != "SIGN".ToLower())
            {
                if (string.IsNullOrEmpty(rtnStr))
                    rtnStr += string.Format("{0}={1}", requestItem[i], Request.QueryString[requestItem[i]]);
                else
                    rtnStr += string.Format("&{0}={1}", requestItem[i], Request.QueryString[requestItem[i]]);
            }
        }
        LogTxt.WriteEntry(string.Format("返回报文，报文[{0}]", rtnStr), "b2c后台通知日志");
        return rtnStr;
    }
    


    /// <summary>
    ///  并以“参数名=参数值”的形式组成数组
    /// </summary>
    /// <returns>request回来的信息组成的数组</returns>
    public   string GetRequestGet()
    {
        string rtnStr = string.Empty;
        int i = 0;
        NameValueCollection coll;
        coll = Request.QueryString;
        String[] requestItem = coll.AllKeys;
        for (i = 0; i < requestItem.Length; i++)
        {
            if (i == 0)
                rtnStr += string.Format("{0}={1}", requestItem[i], Request.QueryString[requestItem[i]]);
            else
                rtnStr += string.Format("&{0}={1}", requestItem[i], Request.QueryString[requestItem[i]]);
        }
        return rtnStr;
    }

    /// <summary>
    ///  并以“参数名=参数值”的形式组成数组
    /// </summary>
    /// <returns>request回来的信息组成的数组</returns>
    public   string GetRequestPost()
    {
        string rtnStr = string.Empty;
        int i = 0;
        NameValueCollection coll;
        coll = Request.Form;
        String[] requestItem = coll.AllKeys;
        for (i = 0; i < requestItem.Length; i++)
        {
            if (i == 0)
                rtnStr += string.Format("{0}={1}", requestItem[i], Request.Form[requestItem[i]]);
            else
                rtnStr += string.Format("&{0}={1}", requestItem[i], Request.Form[requestItem[i]]);
        }
        return rtnStr;
    }



}