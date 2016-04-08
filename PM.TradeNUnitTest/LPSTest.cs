using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PM.Utils.WCF;
using PM.PaymentModel;

namespace PM.TradeTest
{
    [TestFixture]
    public class LPSTest
    {
        //[Test]
        public void B2CPay()
        {
            var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>("http://127.0.0.1:9008/PaymentService");
            PayStartModel pay = new PayStartModel();
            pay.BusinessFunNo = "LPSBBCB2CPay";
            pay.BusnissID = "111";
            pay.OrderNo = DateTime.Now.ToString("yyMMddHHmmss");
            pay.PayMoney = 100;
            pay.InstitutionID = "001048";
            pay.PaySettingAccNo = "0001";
            pay.TradeInfo = "建行测试.)(@#!~$%^&*_+~！、、】";
            pay.PayBankID = "700";
            pay.PayBankAccountType = "11";
            pay.PayFee = 5;

            var rtn = info.DoPay(pay);
        }
        //[Test]
        public void B2BPay()
        {
            var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>("http://127.0.0.1:9008/PaymentService");
            PayStartModel pay = new PayStartModel();
            pay.BusinessFunNo = "LPSBBCB2BPay";
            pay.BusnissID = "111";
            pay.OrderNo = DateTime.Now.ToString("yyMMddHHmmss");
            pay.PayMoney = 100;
            pay.InstitutionID = "001048";
            pay.PaySettingAccNo = "0001";
            pay.TradeInfo = "建行测试.)(@#!~$%^&*_+~！、、】";
            pay.PayBankID = "700";
            pay.PayBankAccountType = "11";
            pay.PayFee = 5;

            var rtn = info.DoPay(pay);
            Assert.Contains("form", null);
        }
       // [Test]
        public void Query()
        {
            PM.TaskBiz.LPSBBCTask.LPSBBCTaskJob job = new TaskBiz.LPSBBCTask.LPSBBCTaskJob();
            job.Execute(null);             
        }
    }
}
