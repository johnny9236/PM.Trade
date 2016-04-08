using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PM.Utils.Log;
using System.Text;
using PM.Utils;
using PM.Utils.WCF;

public partial class CallBack_CallBack : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Request.ContentEncoding = Encoding.UTF8;
        Response.ContentEncoding = Encoding.UTF8;
        String message = Request["message"];
        String signature = Request["signature"];
        LogTxt.WriteEntry(string.Format(" ReqStr报文message[{0}]signature[{1}]", message, signature), "银联支付后台日志");
            string showRtn = string.Empty;//报文信息用于打印
        if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(signature))
        { 
            //todo   建行返回判断
            //1、根据编码获取是b2b还是  b2c
            //2、处理获取参数  
            LogTxt.WriteEntry(string.Format("返回报文为空，报文[{0}]-密匙[{1}]", message, signature), "银联支付后台日志");
            return;
        }
        //  支付返回
        string url = ConfigHelper.GetConfigString("WcfUrl");
        var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>(url);
        var responseModel = new PM.PaymentModel.PayResopnseModel();
        responseModel.BusinessFunNo = "1118";
        responseModel.IsShowBk = true;
        responseModel.Message = message;
        responseModel.Signature = signature;
        showRtn = info.PayCallback(responseModel); 
        Response.Write(showRtn);

    }
}