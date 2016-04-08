using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using PM.Utils.Log;
using PM.Utils.SocektUtils;
using PM.PaymentProtocolModel.BankCommModel.JHBOF;
using PM.PaymentProtocolModel;
using PM.Utils;


namespace PM.JHBOFPtlBiz
{
    /// <summary>
    /// 查询相关
    /// </summary>
    public partial class BOFCommProtocols
    {
        /// <summary>
        /// 金华交行 清单查询
        /// </summary>
        /// <param name="sendInfo"></param>
        /// <returns></returns>
        private List<JHBofQueryResult> GetJHBOFQuery(JHBOFQueryPayListModel sendInfo, CfgInfo cfgInfo)
        {
            bool result = false;
            BOFResponse bofResponse = null;
            BOFRequest queryInfo = new BOFRequest();
            queryInfo.AcountNo = sendInfo.AccNo;
            queryInfo.FilePath = cfgInfo.RootFilePath;//路径
            queryInfo.TradeKind = sendInfo.Use;//资金用途用来表示交易类型
            queryInfo.IP = cfgInfo.IP;
            int port = 0;
            int.TryParse(cfgInfo.Port, out  port);
            queryInfo.Port = port;
            queryInfo.TradeBegDate = sendInfo.StartDate;
            queryInfo.TradeEndDate = sendInfo.EndDate;
            queryInfo.TradeKind = "000001";//交易类型
            List<BOFModel> bofList = QueryInfo(queryInfo, bofResponse);
            var queryList = new List<JHBofQueryResult>();
            //if (null != bofResponse&&bofResponse.ResPonseCode == "000000")//成功处理
            //{

            JHBofQueryResult queryRst = null;
            if (null != bofList && bofList.Count > 0)
            {
                result = true;
                #region  返回结果
                foreach (var bof in bofList)
                {
                    queryRst = new JHBofQueryResult();
                    queryRst.AcountName = bof.PayAccountName;
                    queryRst.AcountNo = bof.PayAccountNO;
                    queryRst.Amount = Convert.ToInt64(bof.Amount);
                    queryRst.Remark = bof.Remark;//备注描述了 账号+标段编码+费用类型
                    queryRst.TradeDate = bof.TradeDate;
                    queryRst.TradeNo = bof.TradeNo;
                    queryRst.CustomTradeNo = bof.CustomTradeNo;//银行伪序列号
                    queryList.Add(queryRst);
                }
                #endregion
            }
            // }

            // LogTxt.WriteEntry(string.Format("查询账户明细返回信息内容{0}失败", result,bofResponse.ResPonseCodeDes), "Jh交行明细文件查询");
            return queryList;
        }

        #region 协议
        public List<BOFModel> QueryInfo(BOFRequest sendInfo, BOFResponse responseInfo)
        {
            List<BOFModel> bofModelList = null;
            #region   报文发生 并处理
            //返回信息获取
            responseInfo = QueryActiveInfo(sendInfo);
            //获取成功就取文件信息
            if (responseInfo != null && responseInfo.ResPonseCode == "000000")
            {
                if (!string.IsNullOrWhiteSpace(responseInfo.FileName))
                {
                    System.Threading.Thread.Sleep(1000);
                    bofModelList = ResultInfo(sendInfo, responseInfo);
                }
            }
            #endregion
            return bofModelList;
        }

        /// <summary>
        /// 返回响应结果
        /// </summary>
        /// <param name="sendInfo"></param>
        /// <returns></returns>
        private BOFResponse QueryActiveInfo(BOFRequest sendInfo)
        {
            BOFResponse rtnAp = null;
            string sendStr = string.Format("{0}{1}{2}{3}", sendInfo.TradeKind, sendInfo.AcountNo, sendInfo.TradeBegDate, sendInfo.TradeEndDate);
            LogTxt.WriteEntry(string.Format("查询账户明细发送信息内容{0}", sendStr), "Jh交行明细文件查询");
            string reciveStr = string.Empty;
            try
            {
                reciveStr = SocketClient.SendToServ(sendInfo.IP, sendInfo.Port, sendStr, Encoding.GetEncoding("GB2312"));
            }
            catch (Exception ex)
            { 
                #region 邮件发送
                var fromMail = ConfigHelper.GetCustomCfg("JH", "fromMail");
                var fromMailPwd = ConfigHelper.GetCustomCfg("JH", "fromMailPwd");
                var toMail = ConfigHelper.GetCustomCfg("JH", "toMail");
                var subject = ConfigHelper.GetCustomCfg("JH", "subject");
                var host = ConfigHelper.GetCustomCfg("JH", "host");
                var body = ConfigHelper.GetCustomCfg("JH", "body");

                System.Net.Mail.MailMessage myMail = new System.Net.Mail.MailMessage();
                myMail.From = new System.Net.Mail.MailAddress(fromMail);
                myMail.To.Add(toMail);

                myMail.Subject = subject;
                myMail.SubjectEncoding = Encoding.UTF8;
                myMail.Body = body; 
                myMail.BodyEncoding = Encoding.UTF8;
                myMail.IsBodyHtml = true;

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = host;// "smtp.qq.com";
                smtp.Credentials = new System.Net.NetworkCredential(fromMail, fromMailPwd);
                try
                { 
                    smtp.Send(myMail);
                }
                catch (Exception e)
                {
                    LogTxt.WriteEntry(string.Format("交行通讯失败-发送通知邮件失败", e.Message), "Jh交行明细文件查询");
                    throw e;
                }
                #endregion
            }
            LogTxt.WriteEntry(string.Format("查询账户明细返回信息内容{0}", reciveStr), "Jh交行明细文件查询");
            if (!string.IsNullOrWhiteSpace(reciveStr))
            {
                reciveStr = reciveStr.Trim();
                #region 赋值
                try
                {
                    if (!string.IsNullOrEmpty(reciveStr) && reciveStr.Length > 6)
                    {
                        rtnAp = new BOFResponse();
                        rtnAp.ResPonseCode = reciveStr.Substring(0, 6).Trim();
                        rtnAp.FileName = reciveStr.Substring(6).Trim();
                    }
                }
                catch (Exception ex)
                {
                    LogTxt.WriteEntry(string.Format("查询账户明细返回信息内容{0}失败", ex.Message), "Jh交行明细文件查询");
                    throw ex;
                }
                #endregion
            }
            return rtnAp;
        }

        /// <summary>
        /// 文件匹配文件处理
        /// </summary>
        /// <param name="sendInfo">请求信息</param>
        /// <param name="ap">响应返回信息</param>
        /// <returns></returns>
        private static List<BOFModel> ResultInfo(BOFRequest sendInfo, BOFResponse ap)
        {
            List<BOFModel> bofList = new List<BOFModel>();
            BOFModel bof = null;
            #region  文件操作
            using (StreamReader objReader = new StreamReader(string.Format("{0}/{1}", sendInfo.FilePath, ap.FileName), Encoding.Default))
            {
                string sLine = "";
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    //var lengthCh = SplitWorld.Length(sLine);
                    if (sLine != null && !sLine.Equals(""))
                    {
                        bof = new BOFModel();
                        bof.TradeDate = SplitWorld.SubString(sLine, 0, 8);
                        bof.TradeNo = SplitWorld.SubString(sLine, 8, 12).Trim();
                        bof.Amount = SplitWorld.SubString(sLine, 20, 15);
                        bof.Remark = SplitWorld.SubString(sLine, 35, 60).Trim();
                        bof.PayAccountNO = SplitWorld.SubString(sLine, 95, 32).Trim();
                        bof.PayAccountName = SplitWorld.SubString(sLine, 127, 60);
                        bof.CustomTradeNo = SplitWorld.SubString(sLine, 187, 10).Trim();//银行伪序列号
                        bofList.Add(bof);
                    }
                }
            }
            #endregion
            return bofList;
        }
        #endregion
    }
}
