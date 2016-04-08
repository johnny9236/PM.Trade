using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.JSABOC
{
    /// <summary>
    /// 返回结果对象供业务及通知返回
    /// </summary>
    public class JSABOCBizModel
    {
        /// <summary>
        /// 返回给服务端输出(退还保证金响应给银行等)
        /// </summary>
        public JSABOCRtnModel RtnToProtol { get; set; }
        ///// <summary>
        ///// 返回摘要信息(查询明细时使用)
        ///// </summary>
        //public JSABOCRtnModel SummModel { get; set; }
        /// <summary>
        /// 交易明细或退还保证金明细()
        /// </summary>
        public List<JSABOCRtnModel> RtnModels { get; set; }

    }
}
