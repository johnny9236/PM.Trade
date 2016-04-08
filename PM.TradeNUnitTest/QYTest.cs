using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PM.PaymentModel.BizModel.AHQY;
using PM.PaymentModel;
using PM.Utils.WCF;


namespace PM.TradeTest
{
    [TestFixture]
    public class QYTest
    {
        CommServiceProtocolModel comm = null;
        string url = "http://127.0.0.1:9008/CommonService";
        string Payurl = "http://127.0.0.1:9008/PayMent";
        //[Test]
        public void CreateVirtTest()
        {
            QYVirtualAccountRequset virtualAcc = new QYVirtualAccountRequset();
            virtualAcc.AcctNo = "";
            virtualAcc.BiaoDuanNo = "";
            virtualAcc.IsRetire = "1";
            virtualAcc.AcctNo = "34001767308053001098";
            virtualAcc.OpenDate = DateTime.Now.ToString("yyyyMMdd");
            virtualAcc.OpenTime = DateTime.Now.ToString("HHmmSS");
            virtualAcc.ProjectName = "pm测试";
            virtualAcc.ProjectNo = "HSQ-TB-HS-2013112503";
            virtualAcc.BiaoDuanNo = "A";
            virtualAcc.TransDate = DateTime.Now.ToString("yyyyMMdd");
            virtualAcc.TransTime = DateTime.Now.ToString("HHmmSS");
            virtualAcc.SeqNo = DateTime.Now.ToString("yyyyMMddHHmmSS");

            comm = new CommServiceProtocolModel();
            comm.BusinessFunNo = "QYVirtualAccount";
            comm.Content = virtualAcc.GetMessagePaket();
            var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.ICommService>(url);
            var rstStr = info.CommonRemoteCall(comm);
            if (!string.IsNullOrEmpty(rstStr))
            {
                QYVirtualAccountResponse rst = new QYVirtualAccountResponse();
                rst.GetModel(rstStr);
                Assert.AreEqual(rst.TransCode, "B001");
                //rst 对象
            }

        }
        //[Test]
        public void BidTime()
        {
            QYBidRequest bid = new QYBidRequest();
            //bid.TransCode = "3041";
            bid.TransDate = DateTime.Now.ToString("yyyyMMdd");
            bid.TransTime = DateTime.Now.ToString("HHmmSS");
            bid.OpenDate = DateTime.Now.ToString("yyyyMMdd");
            bid.OpenTime = DateTime.Now.ToString("HHmmSS");
            bid.SeqNo = DateTime.Now.ToString("yyyyMMddHHmmss");
            bid.BiaoDuanNo = "A";
            bid.AuthCode = "";

            comm = new CommServiceProtocolModel();
            comm.BusinessFunNo = "QYBidTm";
            comm.Content = bid.GetMessagePaket();
            var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.ICommService>(url);
            var bidRstStr = info.CommonRemoteCall(comm);
            if (!string.IsNullOrEmpty(bidRstStr))
            {
                QYBidOrBZJOrFinishResponse rst = new QYBidOrBZJOrFinishResponse();
                rst.GetModel(bidRstStr);
                //rst 对象
            }
        }
        //[Test]
        public void BzjTime()
        {
            QYBZJRequest bzj = new QYBZJRequest();
            bzj.TransDate = DateTime.Now.ToString("yyyyMMdd");
            bzj.TransTime = DateTime.Now.ToString("HHmmSS");
            bzj.BZJEndDate = DateTime.Now.ToString("yyyyMMdd");
            bzj.BZJEndTime = DateTime.Now.ToString("HHmmSS");
            bzj.SeqNo = DateTime.Now.ToString("yyyyMMddHHmmss");
            bzj.BiaoDuanNo = "A";
            bzj.AuthCode = "";

            comm = new CommServiceProtocolModel();
            comm.BusinessFunNo = "QYBzjEndTm";
            comm.Content = bzj.GetMessagePaket();
            var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.ICommService>(url);
            var bzjRstStr = info.CommonRemoteCall(comm);
            if (!string.IsNullOrEmpty(bzjRstStr))
            {
                QYBidOrBZJOrFinishResponse rst = new QYBidOrBZJOrFinishResponse();
                rst.GetModel(bzjRstStr);
                //rst 对象
            }

        }

         //[Test]
        public void RefundBzj()
        {
            PayRefundModel payRefundMode = new PayRefundModel();
            payRefundMode.AuthCode = "";//授权码
            payRefundMode.MainAccount = "";//虚拟账号(特殊注意)

            payRefundMode.TradeInfo = "";//标段编号
            payRefundMode.BusinessFunNo = "QYBzjReFund";
            payRefundMode.TransDate = DateTime.Now.ToString("yyyyMMdd");
            payRefundMode.TransTime = DateTime.Now.ToString("HHmmSS");


            payRefundMode.PayRefundDtl = new List<PayStartModel>();
            PayStartModel psModel = new PayStartModel();
            psModel.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmss");

            psModel.ReceiptAccountDbBank = "";
            psModel.ReceiptOpenBankNo = "";

            psModel.ReceiptAcountNo = "";
            psModel.ReceiptAcountName = "";

            psModel.InDate = DateTime.Now.ToString("yyyyMMdd");
            psModel.InTime = DateTime.Now.ToString("HHmmSS");
            psModel.PayMoney = 1;
            psModel.BusnissID = ""; //业务功能号
            payRefundMode.PayRefundDtl.Add(psModel);
            var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>(Payurl);
            info.DoRefundPay(payRefundMode);
        }



        //[Test]
        public void FinishPro()
        {
            QYFinishPro fp = new QYFinishPro();
            fp.AuthCode = "";//中心授权码
            fp.BiaoDuanNo = "A";
            fp.IAcctNo = "";//虚拟账号
            fp.SeqNo = DateTime.Now.ToString("yyyyMMddHHmmss");
            fp.TransDate = DateTime.Now.ToString("yyyyMMdd");
            fp.TransTime = DateTime.Now.ToString("HHmmss");


            comm = new CommServiceProtocolModel();
            comm.BusinessFunNo = "QYFinish";
            comm.Content = fp.GetMessagePaket();
            var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.ICommService>(url);
            var rstStr = info.CommonRemoteCall(comm);
            if (!string.IsNullOrEmpty(rstStr))
            {
                QYBidOrBZJOrFinishResponse rst = new QYBidOrBZJOrFinishResponse();
                rst.GetModel(rstStr);
                //rst 对象
                Assert.AreEqual(rst.Result, "1");//成功
            }
        }
    }
}
