using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using PM.Utils.WCF;
using PM.Utils;

public partial class CallBack_FrontCallBack : System.Web.UI.Page
{
    /// <summary>
    /// 前台返回显示
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string showInfo = string.Empty;
        Request.ContentEncoding = Encoding.UTF8;
        Response.ContentEncoding = Encoding.UTF8;
        String message = Request["message"];
        String signature = Request["signature"];
        if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(signature))
        {
            //todo   建行返回判断
            //1、根据编码获取是b2b还是  b2c
            //2、处理获取参数
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
        responseModel.IsShowBk = false;
        responseModel.BusinessFunNo = "1118";
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
}