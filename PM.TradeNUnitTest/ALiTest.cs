using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PM.PaymentModel;
using PM.Utils.WCF;

namespace PM.TradeNUnitTest
{
    [TestFixture]
    public class ALiTest
    {
        private static string url = "http://192.168.0.10:9008/PaymentService";
        //[Test]
        public void Test()
        {

            var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>(url);
            PayStartModel pay = new PayStartModel();
            pay.BusinessFunNo = "AliPay";
            pay.BusnissID = "111";
            pay.OrderNo = DateTime.Now.ToString("yyMMddHHmmss");
            pay.PayMoney = 100;
            pay.TradeInfo = "测试ali";
            var rtn = info.DoPay(pay);
           
        }
    }
}
