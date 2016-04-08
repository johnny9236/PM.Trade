using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using PM.Utils.Log;
using PM.Utils;
using PM.Utils.WCF;
using System.Collections.Specialized;
using System.IO;

public partial class CallBack_LPS_CallBackB2B : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        string message = string.Empty; ;
        string signStr = string.Empty;
       string  parmStr = string.Empty;
       GetRequestGetMessage(out message, out signStr, out parmStr);

        if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(signStr))
        { 
            LogTxt.WriteEntry(string.Format("返回报文为空，报文[{0}-----{1}-{2}]", message,signStr,parmStr), "b2B后台通知日志");
            return;
        }
        //  支付返回
        string showRtn = string.Empty;//报文信息用于打印
        string url = ConfigHelper.GetConfigString("WcfUrl");
        var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>(url);
        var responseModel = new PM.PaymentModel.PayResopnseModel();
        responseModel.BusinessFunNo = "LPSBBCB2BResponse";
        responseModel.IsShowBk = true;
        responseModel.Message = message;
        responseModel.Signature = signStr;
        responseModel.ReceiveText = parmStr;
        showRtn = info.PayCallback(responseModel);
        Response.Write(showRtn);

    }


    public void GetRequestGetMessage(out string message, out string signStr, out string parmStr)
    {
        message = string.Empty;
        signStr = string.Empty;
        parmStr = string.Empty;
        try
        {
            IServiceProvider provider = (IServiceProvider)HttpContext.Current;
            HttpWorkerRequest worker = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));
            byte[] bsr = worker.GetQueryStringRawBytes();
            String queryString = Encoding.GetEncoding("GBK").GetString(bsr);

            LogTxt.WriteEntry(string.Format("获取报文[{0}]", queryString), "b2B后台通知日志");
            NameValueCollection querys = HttpUtility.ParseQueryString(queryString);
            if (querys != null && querys.Count > 0)
            {
                foreach (String key in querys.AllKeys)
                {
                    if (string.IsNullOrEmpty(parmStr))
                    {
                        parmStr += string.Format("{0}={1}", key, querys[key]);
                    }
                    else
                    {
                        parmStr += string.Format("&{0}={1}", key, querys[key]);
                    }

                    if (key.Trim().ToLower() == "MPOSID".ToLower() ||
                          key.Trim().ToLower() == "ORDER_NUMBER".ToLower() ||
                          key.Trim().ToLower() == "CUST_ID".ToLower() ||
                          key.Trim().ToLower() == "ACC_NO".ToLower() ||
                          key.Trim().ToLower() == "ACC_NAME".ToLower() ||
                          key.Trim().ToLower() == "AMOUNT".ToLower() ||
                          key.Trim().ToLower() == "STATUS".ToLower() ||
                          key.Trim().ToLower() == "REMARK1".ToLower() ||
                           key.Trim().ToLower() == "REMARK2".ToLower() ||
                           key.Trim().ToLower() == "TRAN_FLAG".ToLower() ||
                           key.Trim().ToLower() == "TRAN_TIME".ToLower() ||
                              key.Trim().ToLower() == "REFERER".ToLower() ||
                           key.Trim().ToLower() == "BRANCH_NAME".ToLower())
                    {
                        if (!string.IsNullOrEmpty(querys[key]))
                        {
                            message += string.Format("{0}", querys[key]);
                        }

                    }
                    else if (key.Trim().ToLower() == "SIGNSTRING".ToLower())
                    {
                        signStr = querys[key];
                    }

                }
                LogTxt.WriteEntry(string.Format("返回报文，报文[{0}-----{1}---{2}]", message, signStr, parmStr), "b2B后台通知日志");

            }
        }
        catch (Exception ex)
        {
            LogTxt.WriteEntry(string.Format("获取参数异常{0}{1}", ex.StackTrace, ex.Message), "b2B后台通知日志");

        }
    }

}