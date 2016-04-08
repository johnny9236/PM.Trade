using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils;

namespace PM.PaymentProtocolModel.BankCommModel.JSABOC
{
    /// <summary>
    /// 返回或者明细对象等结果对象(具体的报文格式)
    /// </summary>
    public class JSABOCRtnModel
    {
        /// <summary>
        /// 交易码
        /// </summary>
        public string TradeCode { get; set; }
        /// <summary>
        /// 招投标中心代码
        /// </summary>
        public string TradeStructNum { get; set; }
        /// <summary>
        /// 返回码
        /// </summary>
        public string ReturneCode { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string ReturneMsg { get; set; }
        /// <summary>
        /// 明细日期
        /// </summary>
        public string DetailDataTime { get; set; }
        /// <summary>
        /// 笔数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 标段编号
        /// </summary>
        public string SectionNo { get; set; }

        /// <summary>
        /// 收款 账号  
        /// </summary>
        public string ReceiveAccNo { get; set; }
        /// <summary>
        ///收款  账户名     
        /// </summary>
        public string ReceiveAccDbName { get; set; }
        /// <summary>
        ///收款  账户开户行 
        /// </summary>
        public string ReceiveAccDBBank { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        public string Use { get; set; }
        /// <summary>
        /// 付款 账号  
        /// </summary>
        public string PayAccNo { get; set; }
        /// <summary>
        ///付款 账户名     
        /// </summary>
        public string PayAccDbName { get; set; }
        /// <summary>
        ///付款 账户开户行 
        /// </summary>
        public string PayAccDBBank { get; set; }
        /// <summary>
        /// 缴纳金额(支付金额)
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 退款金额（退保证金用）
        /// </summary>
        public decimal RealAmount { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string ABOCRemark { get; set; }
        /// <summary>
        ///银行流水号
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 是否农行（1是  0 否)
        /// </summary>
        public string IsAboc { get; set; }
        /// <summary>
        /// 支付金额类型
        /// </summary>
        public string AccountType { get; set; }
        /// <summary>
        /// 获取报文格式
        /// </summary>
        /// <returns>返回处理后的报文字符串</returns>
        public string GetSendStr()
        {
            string stringLenth = string.Empty;//字符长度
            string rtnString = string.Empty;

            var sendInfo = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|",
                this.TradeCode,
                this.TradeStructNum,
                this.ReturneCode,
                this.ReturneMsg,
                this.DetailDataTime,
                this.Count,
                this.SectionNo,
                this.OrderNo,
                this.ReceiveAccDbName,//收款人户名
                this.ReceiveAccNo,//账号
                this.ReceiveAccDBBank,//开户行
                this.IsAboc,
                this.Use,
                this.PayAccDbName,//付款方户名
                this.PayAccNo,
                this.PayAccDBBank,
                this.Amount,
                this.RealAmount,
                this.Summary,
                this.SerialNumber,
                this.ABOCRemark
                );
            var strCount = StringHelper.Text_Length(sendInfo);
            stringLenth = strCount.ToString();//长度为7
            for (int i = 0; i < 7 - strCount.ToString().Length; i++)
            {
                stringLenth = "0" + stringLenth;
            }
            //rtnString = string.Format("{0}{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}",
            //    stringLenth,
            //  this.TradeCode,
            //  this.TradeStructNum,
            //  this.ReturneCode,
            //  this.ReturneMsg,
            //  this.DetailDataTime,
            //  this.Count,
            //  this.SectionNo,
            //  this.OrderNo,
            //  this.ReceiveAccDbName,//收款人户名
            //  this.ReceiveAccNo,//账号
            //  this.ReceiveAccDBBank,//开户行
            //  this.Use,
            //  this.PayAccDbName,//付款方户名
            //  this.PayAccNo,
            //  this.PayAccDBBank,
            //  this.Amount,
            //  this.RealAmount,
            //  this.Summary,
            //  this.SerialNumber,
            //  this.ABOCRemark
            //  );
            rtnString = string.Format("{0}{1}", stringLenth, sendInfo);
            return rtnString;
        }
        /// <summary>
        /// 根据返回报文获取对象
        /// </summary>
        /// <param name="packetString">报文</param>
        /// <returns>报文对象</returns>
        public bool GetModel(string packetString)
        {
            bool result = false;
            try
            {
                var packetStr = packetString;//.Substring(7);
                var infos = packetStr.Split('|');
                //if (infos.Length != 22)
                //{
                //    log
                //    return result;
                //}
                this.TradeCode = infos[0];//.Substring(7);
                this.TradeStructNum = infos[1];
                this.ReturneCode = infos[2];
                this.ReturneMsg = infos[3];
                this.DetailDataTime = infos[4];
                int count = 0;
                int.TryParse(infos[5], out count);
                this.Count = count;
                this.SectionNo = infos[6];
                this.OrderNo = infos[7];
                this.ReceiveAccDbName = infos[8];//收款人户名
                this.ReceiveAccNo = infos[9];//账号
                this.ReceiveAccDBBank = infos[10];//开户行
                this.IsAboc = infos[11];//开户行
                this.Use = infos[12];
                this.PayAccDbName = infos[13];//付款方户名
                this.PayAccNo = infos[14];
                this.PayAccDBBank = infos[15];
                decimal amount = 0;
                decimal.TryParse(infos[16], out amount);
                this.Amount = amount;
                decimal realAmount = 0;
                decimal.TryParse(infos[17], out realAmount);
                this.RealAmount = realAmount;
                this.Summary = infos[18];
                this.SerialNumber = infos[19];
                this.ABOCRemark = infos[20];
                result = true;
            }
            catch (Exception ex)
            {
                CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, ex.Source, ex);
                throw ex;
            }
            return result;
        }
    }

}
