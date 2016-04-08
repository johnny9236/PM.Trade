using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using PM.Utils.WCF;
using PM.PaymentProtocolModel;
using PM.Utils.EnumUtil;
using System.ComponentModel;
using System.Xml.Linq;
using PM.PaymentModel.BizModel.AHQY;
using PM.PaymentModel;
using PM.PaymentProtocolModel.BankCommModel;
using System.Text;


public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        var str = "%EF%BF%BD%EF%BF%BD%EF%BF%BD%EF%BF%BD%CB%AE%EF%BF%BD%D0%B4%EF%BF%BD%EF%BF%BD%EF%BF%BD%E5%B9%A4%EF%BF%BD%CC%BD%EF%BF%BD%EF%BF%BD%EF%BF%BD%EF%BF%BD%EF%BF%BD%D1%AF%EF%BF%BD%EF%BF%BD%EF%BF%BD%DE%B9%EF%BF%BD%CB%BE";
        var tt = Encoding.Default.GetBytes(str);
        var restusss = Encoding.GetEncoding("GBK").GetString(tt);

        Response.Write(restusss);

        return;



        //var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>("http://127.0.0.1:9008/PaymentService");
        //PayStartModel pay = new PayStartModel();
        //pay.BusinessFunNo = "1112";
        //pay.BusnissID = "111";
        //pay.OrderNo = DateTime.Now.ToString("yyMMddHHmmss");
        //pay.PayMoney = 100;
        //pay.InstitutionID = "001094";
        //pay.PaySettingAccNo = "0001";
        //pay.TradeInfo = "银联测试1112";
        //pay.PayBankID = "700";
        //pay.PayBankAccountType = "11";
        //pay.PayFee = 5;

        //var rtn = info.DoPay(pay);
        //show.InnerHtml = rtn;
         

        //var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>("http://192.168.0.10:9008/PaymentService");
        //PayStartModel pay = new PayStartModel();
        //pay.BusinessFunNo = "AliPay";
        //pay.BusnissID = "111";
        //pay.OrderNo = DateTime.Now.ToString("yyMMddHHmmss");
        //pay.PayMoney = 100;
        //pay.TradeInfo = "测试ali"; 
        //var rtn = info.DoPay(pay);
        //show.InnerHtml = rtn;

        //string url = "http://127.0.0.1:800/";
        //var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>(url);
        //var ttt = info.DoPay(null);
        #region  lps
        //string url = "http://192.168.0.10:9008/PaymentService";
        //var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>(url);
        //var model = new PM.PaymentModel.PayStartModel();
        //model.BusinessFunNo = "1111";// "LPSBBCB2CPay";//
        //model.InstitutionID = "000280";
        //model.BusnissID = "BusnissID";
        //model.OrderNo = "00000000062";
        //model.PayAccountDbBank = "";
        //model.PayAcountName = "";
        //model.PayAcountNo = "";
        //model.PayBankAccountType = "12";
        //model.PayBankID = "700";
        //model.PayCity = "";
        //model.PayCur = "Cur";
        //model.PayerID = "b58e5414-66fb-4831-b5c9-d87b5334e5c2";
        //model.PayerName = "xxx公司";
        //model.PayMoney = 100;
        //model.PayOpenBankNo = "1234567890";
        //model.PayProvince = "";
        //model.PaySettingAccNo = "0001";
        //model.RateMoney = 0;
        //model.ReceiptAccountDbBank = "交通银行金华分行";
        //model.ReceiptAcountName = "代理公共资源投标席位费清算户";
        //model.ReceiptAcountNo = "337001012620196003899";
        //model.ReceiptBankAccountType = "12";
        //model.ReceiptBankID = "700";
        //model.ReceiptCity = "";
        //model.ReceiptCur = "";
        //model.ReceiptOpenBankNo = "301338000023";
        //model.ReceiptProvince = "";
        //model.ReceiptSettingAccNo = "0001";
        //model.Remark = "demo";
        //model.TradeInfo = "测试费用";
        //model.PayFee = 0;

        //var pay = info.DoPay(model);
        //this.show.InnerHtml = pay;
        #endregion


        ////通用 接口实现方式
        //CommServiceProtocolModel comm = null;
        //string url = "http://192.168.0.10:9008/CommonService";
       #region   青阳
        QYVirtualAccountRequset virtualAcc = new QYVirtualAccountRequset();
        //virtualAcc.AcctNo = "";
        //virtualAcc.BiaoDuanNo = "";
        //virtualAcc.IsRetire = "1";
        //virtualAcc.AcctNo="34001767308053001098"; 
        //virtualAcc.OpenDate = DateTime.Now.ToString("yyyyMMdd");
        //virtualAcc.OpenTime = DateTime.Now.ToString("HHmmSS");
        //virtualAcc.ProjectName = "pm测试";
        //virtualAcc.ProjectNo = "HSQ-TB-HS-2013112503";
        //virtualAcc.BiaoDuanNo = "A";
        //virtualAcc.TransDate = DateTime.Now.ToString("yyyyMMdd");
        //virtualAcc.TransTime = DateTime.Now.ToString("HHmmSS");
        //virtualAcc.SeqNo = DateTime.Now.ToString("yyyyMMddHHmmSS");
         
        //comm = new CommServiceProtocolModel();
        //comm.BusinessFunNo = "QYVirtualAccount";
        //comm.Content = virtualAcc.GetMessagePaket();
        //var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.ICommService>(url);
        //var rstStr = info.CommonRemoteCall(comm);
        //if (!string.IsNullOrEmpty(rstStr))
        //{
        //    QYVirtualAccountResponse rst = new QYVirtualAccountResponse();
        //    rst.GetModel(rstStr);
        //    //rst 对象
        //}
        ////////////////////////
        //QYBidRequest bid = new QYBidRequest();
        ////bid.TransCode = "3041";
        //bid.TransDate = DateTime.Now.ToString("yyyyMMdd");
        //bid.TransTime = DateTime.Now.ToString("HHmmSS");
        //bid.OpenDate = DateTime.Now.ToString("yyyyMMdd");
        //bid.OpenTime = DateTime.Now.ToString("HHmmSS");
        //bid.SeqNo = DateTime.Now.ToString("yyyyMMddHHmmss");
        //bid.BiaoDuanNo = "A";
        //bid.AuthCode = "";

        //comm = new CommServiceProtocolModel();
        //comm.BusinessFunNo = "QYBidTm";
        //comm.Content = bid.GetMessagePaket();
        ////info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.ICommService>(url);
        //var bidRstStr = info.CommonRemoteCall(comm);
        //if (!string.IsNullOrEmpty(rstStr))
        //{
        //    QYBidOrBZJResponse rst = new QYBidOrBZJResponse();
        //    rst.GetModel(bidRstStr);
        //    //rst 对象
        //}
        /////////////////////
        //QYBZJRequest bzj = new QYBZJRequest();
        //bzj.TransDate = DateTime.Now.ToString("yyyyMMdd");
        //bzj.TransTime = DateTime.Now.ToString("HHmmSS");
        //bzj.BZJEndDate = DateTime.Now.ToString("yyyyMMdd");
        //bzj.BZJEndTime = DateTime.Now.ToString("HHmmSS");
        //bzj.SeqNo = DateTime.Now.ToString("yyyyMMddHHmmss");
        //bzj.BiaoDuanNo = "A";
        //bzj.AuthCode = "";





        //comm = new CommServiceProtocolModel();
        //comm.BusinessFunNo = "QYBzjEndTm";
        //comm.Content = bid.GetMessagePaket();
        ////info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.ICommService>(url);
        //var bzjRstStr = info.CommonRemoteCall(comm);
        //if (!string.IsNullOrEmpty(rstStr))
        //{
        //    QYBidOrBZJResponse rst = new QYBidOrBZJResponse();
        //    rst.GetModel(bidRstStr);
        //    //rst 对象
        //}
         

     //   PayRefundModel payRefundMode = new PayRefundModel();
     //   payRefundMode.AuthCode = "";//授权码
     //   payRefundMode.MainAccount = "";//虚拟账号(特殊注意)
       
     //   payRefundMode.TradeInfo = "";//标段编号
     //   payRefundMode.BusinessFunNo = "QYBzjReFund";
     //   payRefundMode.TransDate = DateTime.Now.ToString("yyyyMMdd");
     //   payRefundMode.TransTime = DateTime.Now.ToString("HHmmSS");


     //   payRefundMode.PayRefundDtl = new List<PayStartModel>();
     //   PayStartModel psModel = new PayStartModel();
     //   psModel.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmss");

     //   psModel.ReceiptAccountDbBank = "";
     //   psModel.ReceiptOpenBankNo = "";

     //   psModel.ReceiptAcountNo = "";
     //   psModel.ReceiptAcountName = "";

     //   psModel.InDate = DateTime.Now.ToString("yyyyMMdd");
     //   psModel.InTime = DateTime.Now.ToString("HHmmSS");
     //   psModel.PayMoney = 1;
     //   psModel.BusnissID = ""; //业务功能号
     //   payRefundMode.PayRefundDtl.Add(psModel);
     //var  info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>("");
     //info.DoRefundPay(payRefundMode);
 
        #endregion

    }
}

