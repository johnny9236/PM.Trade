using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentModel;
using PM.PlaymentPersistence.Payment.PersistenceBiz.AHQY;

namespace PM.PlaymentPersistence.Payment.PersistenceBiz.HuangSan
{
    /// <summary>
    ///  建行行退款（包括人工）(调用青阳建行)
    /// </summary>
    public class HuangSanCCBPersistence
    {
        public PayRefundModel DoRefundPay(PayRefundModel payRefundMode)
        {
            //QYPersistence qy = new QYPersistence();
            //return qy.DoRefundPay(payRefundMode);
            return null;
        }
    }
}
