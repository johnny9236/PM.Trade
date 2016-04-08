using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.TradeConsole
{
    class Program
    {
        /// <summary>
        /// 控制台启动
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            InitServer.Init();//初始化 
            #region 测试
            ////以下为测试
            //PM.TaskBiz.HYBOCTASK.BOCTaskJob job = new TaskBiz.HYBOCTASK.BOCTaskJob();
            //job.Execute(null);
            //PM.TaskBiz.HYSync.Expert.HYAvoidTaskJob job = new TaskBiz.HYSync.Expert.HYAvoidTaskJob();
            //job.Execute(null);
            //////黄梅
            //PM.TaskBiz.HuangMeiPostlTask.HuangMeiPostlTaskJob job = new TaskBiz.HuangMeiPostlTask.HuangMeiPostlTaskJob();
            //job.Execute(null);
            //金华
            //PM.TaskBiz.JHBOFTask.JHBOFTaskJob job = new TaskBiz.JHBOFTask.JHBOFTaskJob();
            //job.Execute(null);
            //银联
            //PM.TaskBiz.NetBankTask.NetBankQueryAccountTaskJob job = new TaskBiz.NetBankTask.NetBankQueryAccountTaskJob();
            //job.Execute(null);


            //嘉善
            //ZTB1
            //PM.TaskBiz.JSABOCTask.JSABOCTaskJob job = new TaskBiz.JSABOCTask.JSABOCTaskJob();
            //job.Execute(null);

            ////ZTB2
            //PM.PaymentServices.PaymentService ps = new PaymentServices.PaymentService();
            //var payFound = new PM.PaymentModel.PayRefundModel();

            //payFound.PayRefundDtl = new List<PaymentModel.PayReceiveModel>();
            //var payMoedl = new PaymentModel.PayReceiveModel();
            //payMoedl.BusinessFunNo = "ZTB2";
            //payMoedl.BusnissID = "23f50840-4582-4c92-b89c-0c7d591a9833";
            //payMoedl.OrderNo = "12127";

            //payMoedl.ReceiptAccountDbBank = "中国工商银行广丰县支行";
            //payMoedl.ReceiptAcountName = "云林建设集团有限公司";
            //payMoedl.ReceiptAcountNo = "1512214009022127388";
            //payMoedl.Remark = "嘉善测试";
            //payMoedl.PayMoney = 100000;
            //payFound.PayRefundDtl.Add(payMoedl);
            //ps.DoRefundPay(payFound);

            ////ZTB3
            //string receivedText = "0000032ZTB3|001|0000|succ|20130628|3|20130725ZJ00477-1|12121|嘉兴市温州商会|acc1|中国农业银行嘉兴市分行|12|13|14|15|16|17|100|summ|lsh|21|";
            //PM.PaymentContracts.IPaymentService sv = new PM.PaymentServices.PaymentService();
            //var sendStr = sv.PayCallback(receivedText, "");
            #endregion
            Console.WriteLine("初始化完成");
            Console.ReadLine();
            InitServer.InitDispose();//停止服务
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
