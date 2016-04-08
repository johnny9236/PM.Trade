using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using PM.Utils.WCF;
using PM.PaymentModel;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //
        //string username = "8f9964a615a471be636b7c5bc68cc4ce";
        //string password = "e10adc3949ba59abbe56e057f20f883e";
        //string url = "http://localhost:10573/api/testjy/";

        //string usernamePassword = username + ":" + password;
        //CredentialCache mycache = new CredentialCache();
        //WebRequest myReq = WebRequest.Create(url);
        //mycache.Add(new Uri(url), "Basic", new NetworkCredential(username, password));
        //myReq.Credentials = mycache;
        //myReq.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));

        //WebResponse wr = myReq.GetResponse();
        //Stream receiveStream = wr.GetResponseStream();
        //StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
        //string content = reader.ReadToEnd(); 

        var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>("http://127.0.0.1:9008/PaymentService");
        PayStartModel pay = new PayStartModel();
        pay.BusinessFunNo = "LPSBBCB2BPay";
        pay.BusnissID = "111";
        pay.OrderNo = DateTime.Now.ToString("yyMMddHHmmss");
        pay.PayMoney = (decimal)0.01;
        pay.InstitutionID = "001048";
        pay.PaySettingAccNo = "0001";
        pay.TradeInfo = "test";
        pay.PayBankID = "700";
        pay.PayBankAccountType = "11";
        pay.PayFee = 5;

        var rtn = info.DoPay(pay);


 


        show.InnerHtml = rtn;
    }
}