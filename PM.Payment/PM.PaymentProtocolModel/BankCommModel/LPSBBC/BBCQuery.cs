using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PM.PaymentProtocolModel.BankCommModel.LPSBBC
{
    /// <summary>
    /// 查询条件对象
    /// </summary>
    public class BBCQuery : CommunicationBase
    { 
        /// <summary>
        /// 商户代码
        /// </summary>
        public string MERCHANTID { get; set; }
        /// <summary>
        /// 商户柜台代码
        /// </summary>
        public string POSID { get; set; }
        /// <summary>
        /// 分行代码
        /// </summary>
        public string BRANCHID { get; set; }
        /// <summary>
        /// 定单日期 YYYYMMDD
        /// </summary>
        public string ORDERDATE { get; set; }
        /// <summary>
        /// 定单开始时间
        /// </summary>
        public string BEGORDERTIME
        {
            get
            {
                return "00:00:00";
            }
        }
        /// <summary>
        /// 定单结束时间
        /// </summary>
        public string ENDORDERTIME
        {
            get
            {
                return "23:59:59";
            }
        }
        /// <summary>
        /// 9.	流水类型TYPE
        /// 0支付流水
        /// 1退款流水       
        /// </summary>
        public string TYPE { get; set; }
        /// <summary>
        /// 10.	交易码TXCODE=410408
        /// </summary>
        public string TXCODE { get; set; }
        /// <summary>
        /// 11.	流水状态KIND
        /// 必输项（当日只有未结算流水可供查询）
        /// 0 未结算流水  1 已结算流水
        /// </summary>
        public string KIND { get; set; }
        /// <summary>
        /// 交易状态STATUS必输项
        /// 0失败
        /// 1成功
        /// 2不确定
        /// 3全部（已结算流水查询不支持全部）
        /// </summary>
        public string STATUS { get; set; }
        

        /// <summary>
        /// 订单id
        /// </summary>
        public string ORDERID { get; set; }
        /// <summary>
        /// 8.	查询密码
        /// </summary>
        public string QUPWD { get; set; }
      /// <summary>
      /// 13.	查询方式SEL_TYPE 必输项
      /// 1页面形式   
      /// 2文件返回形式 (提供TXT和XML格式文件的下载) 
      /// 3 XML页面形式
      /// </summary>
        public string SEL_TYPE { get; set; }
        /// <summary>
        /// 14.	页码PAGE 
        /// 输入将要查询的页码
        /// </summary>
        public string PAGE { get; set; }
        /// <summary>
        /// 15.	操作员 
        /// </summary>
        public string OPERATOR { get; set; }
        /// <summary>
        /// 16.	预留字段
        /// </summary>
        public string CHANNEL { get; set; }


    }
}
