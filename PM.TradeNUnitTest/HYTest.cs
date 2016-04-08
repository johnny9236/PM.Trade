using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PM.Utils.WebUtils;
using PM.Utils.WCF;

namespace PM.TradeTest
{
    [TestFixture]
    public class HYTest
    {
        //[Test]
        public void GetDtl()
        {
            string url = @"http://218.108.28.44:8080/zjkservice/service.asmx";
            string pwd = "hyxcqzd201";
            string projectID = "E68A0E97-A9BB-43C6-9F4C-0F2B4F52F108";//"7400C186-A082-4F2E-B14A-EA44A2813D44";
            int wdID = 90452;
            string dptID = "178";
            object[] rstParm = new object[] { pwd, dptID, projectID };
            var rstObj = WebServiceHelper.InvokeWebService(url, "GetProjExpertListStr", rstParm);
            var tt = rstObj.ToString();
            List<HYExpert> expertList = new List<HYExpert>();
            var expertArrs = tt.Split('|');
            Array.ForEach(expertArrs, p =>
                 {
                     var expts = p.Split(';');
                     if (null != expts && expts.Length == 2)
                     {
                         if (!string.IsNullOrEmpty(expts[0]))
                         {
                             var hyExp = new HYExpert();
                             hyExp.Captcha = expts[0];
                             hyExp.CQDate = expts[1];
                             hyExp.ProjectID = projectID;
                             expertList.Add(hyExp);
                         }
                     }
                 }
         );
            Assert.AreEqual(1, 1);
        }
        [Test]
        public void DoRefound()
        {
            var info = WCFInvoke.CreateWCFServiceByURL<PM.PaymentContracts.IPaymentService>("http://127.0.0.1:9008/PaymentService");
            var refoundModel = new PaymentModel.PayRefundModel();
            refoundModel.BusinessFunNo = "HaiYanBOCRefund";
            refoundModel.OrderNo = DateTime.Now.AddMinutes(-5).ToString("yyyyMMddHHmmss");//主订单号
            refoundModel.PayRefundDtl = new List<PaymentModel.PayStartModel>();
            var payModel = new PM.PaymentModel.PayStartModel();
            payModel.BusnissID = "mx001";//业务号
            payModel.TradeInfo = "测试001";
            payModel.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmss");



            payModel.PayAcountNo = "374058654971";
            payModel.PayAcountName = "支付测试客户一";
            //收款账号  收款人名称  收款人地址  收款人开户行名称

            payModel.ReceiptAcountNo = "377958654978";
            payModel.ReceiptAccountDbBank = "中国银行股份有限公司浙江省分行理财中心";
            payModel.ReceiptOpenBankNo = "";
            payModel.ReceiptProvince = "";
            payModel.ReceiptAcountName = "支付测试客户一";
            payModel.InDate = DateTime.Now.ToString("yyyyMMdd");
            payModel.PayMoney = 1; 

            refoundModel.PayRefundDtl.Add(payModel);



            var payModel2 = new PM.PaymentModel.PayStartModel();
            payModel2.BusnissID = "mx002";//业务号
            payModel2.TradeInfo = "测试002";
            payModel2.OrderNo = DateTime.Now.AddDays(1).ToString("yyyyMMddHHmmss");



            payModel2.PayAcountNo = "374058654971";
            payModel2.PayAcountName = "支付测试客户一";
            //收款账号  收款人名称  收款人地址  收款人开户行名称

            payModel2.ReceiptAcountNo = "123456789102";// "379258654978";
            payModel2.ReceiptAccountDbBank = "中国银行股份有限公司浙江省分行理财中心";
            payModel2.ReceiptOpenBankNo = "";
            payModel2.ReceiptProvince = "";
            payModel2.ReceiptAcountName = "支付测试客户一";
            payModel2.InDate = DateTime.Now.ToString("yyyyMMdd");
            payModel2.PayMoney = 100;

            refoundModel.PayRefundDtl.Add(payModel2);


            var rst = info.DoRefundPay(refoundModel);

            Assert.AreEqual(rst.PayRefundDtl[0].Result, true);



        }
    }

    /// <summary>
    /// 获取专家列表
    /// </summary>
    public class HYExpert
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectID { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Captcha { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 抽取时间
        /// </summary>
        public string CQDate { get; set; }
    }
}
