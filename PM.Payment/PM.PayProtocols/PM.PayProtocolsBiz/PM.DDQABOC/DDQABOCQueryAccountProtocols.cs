using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel;
using PM.PaymentProtocolModel.BankCommModel.DDQABOC;
using PM.DDQABOC.ProtocolsModel;
using PM.Utils;
using PM.Utils.Log;
using PM.Utils.SocektUtils;
using System.Xml.Linq;
using System.IO;

namespace PM.DDQABOC
{
    /// <summary>
    /// 查询账户明细
    /// </summary>
    public partial class DDQABOCCommonProtocols
    {
        /// <summary>
        /// 查询获取对账明细
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private List<DDQAccountDtl> GetQueryList(DDQAccountQuery query, CfgInfo cfgInfo)
        {
            QueryAccountDtl queryDtl = new QueryAccountDtl();
            queryDtl.AccNo = query.DbAccNo;
            queryDtl.AuthNo = query.AuthNo;
            queryDtl.Cur = "CNY";//人民币
            queryDtl.LastJrnNo = "0";
            queryDtl.Prov = query.DbProv;
            queryDtl.ReqDate = DateTime.Now.ToString("yyyyMMdd");
            queryDtl.ReqTime = DateTime.Now.ToString("HHmmss");
            queryDtl.ReqSeqNo = query.OrderNo;
            queryDtl.Sign = query.Sign;
            queryDtl.StartTime = query.StartTime;
            queryDtl.StartDate = query.StartDate;
            queryDtl.EndDate = query.EndDate;
            queryDtl.CCTransCode = "CQRA10";
            var result = QueryInfo(queryDtl, cfgInfo);
            if (null != result)
            {
                if (!string.IsNullOrWhiteSpace(result.BatchFileName))
                {
                    return ResultInfo(result, cfgInfo);
                }
                else
                {
                    LogTxt.WriteEntry(string.Format("查询账户明细发送信息内容{0}", string.Format("{0}-{1}获取文件名失败", query.StartDate, query.EndDate)), "DDQ明细文件查询");
                }
            }
            return null;
        }
        /// <summary>
        /// 报文返回结果
        /// </summary>
        /// <param name="sendInfo">请求对象</param>
        /// <param name="sendInfo">配置对象</param>
        /// <returns></returns>
        private static QueryAccountResult QueryInfo(QueryAccountDtl sendInfo, CfgInfo cfgInfo)
        {
            QueryAccountResult rtnAp = null;
            try
            {
                string sendStr = sendInfo.SetRequsetPak().ToString();
                int strLen = StringHelper.Text_Length(sendStr);//报文长度7为位
                string sendAllStr = string.Format("0{0}", strLen);
                for (int i = 0; i < 6 - strLen.ToString().Length; i++)
                {
                    sendAllStr += " ";
                }
                sendAllStr += sendStr;
                LogTxt.WriteEntry(string.Format("查询账户明细发送信息内容{0}", sendAllStr), "DDQ明细文件查询");
                int port = 0;
                int.TryParse(cfgInfo.Port, out port);
                string reciveStr = SocketClient.SendToServ(cfgInfo.IP, port, sendAllStr, Encoding.GetEncoding(ConfigHelper.GetCustomCfg("DDQ", "socketCode")));
                LogTxt.WriteEntry(string.Format("查询账户明细返回信息内容{0}", reciveStr), "DDQ明细文件查询");
                if (!string.IsNullOrWhiteSpace(reciveStr))
                {
                    #region 赋值
                    rtnAp = new QueryAccountResult();
                    if (!string.IsNullOrEmpty(reciveStr.Substring(7).Trim()))
                        rtnAp.GetPubResponseInfo(XDocument.Parse(reciveStr.Substring(7)));
                    #endregion
                }
            }
            catch (Exception ex)
            {
                CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, "查询账户明细异常", ex);
            }
            return rtnAp;

        }
        /// <summary>
        /// 获取交易记录
        /// </summary>
        /// <param name="ap"></param>
        /// <returns></returns>
        private static List<DDQAccountDtl> ResultInfo(QueryAccountResult ap, CfgInfo cfgInfo)
        {
            List<DDQAccountDtl> dtlList = new List<DDQAccountDtl>();
            DDQAccountDtl dtl = null;
            var rootFilePath = cfgInfo.RootFilePath;
            #region  文件操作
            StreamReader objReader = null;
            //using (StreamReader objReader = new StreamReader(string.Format("{0}/{1}", sendInfo.RootFilePath, ap.BatchFileName), Encoding.Default))
            //if (sendInfo.FileType == FileTp.Ftp)
            //    objReader = new StreamReader(GetFtpFile(sendInfo.RootFilePath, sendInfo.FtpUserName, sendInfo.FtpUserPwd, ap.BatchFileName), Encoding.Default);
            //else
            objReader = new StreamReader(string.Format("{0}/{1}", rootFilePath, ap.BatchFileName), Encoding.Default);
            using (objReader)
            {
                string sLine = "";
                string[] linInfo = null;
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null && !sLine.Equals(""))
                    {
                        linInfo = sLine.Split('|');
                        if (linInfo.Length - 1 == int.Parse(ap.FieldNum))
                        {
                            #region  明细
                            dtl = new DDQAccountDtl();
                            dtl.Prov = linInfo[0];
                            dtl.AccNo = linInfo[1];
                            dtl.Cur = linInfo[2];
                            dtl.TrDate = linInfo[3];
                            dtl.TimeStab = linInfo[4];
                            dtl.TrJrn = linInfo[5];
                            dtl.TrType = linInfo[6];
                            dtl.TrBankNo = linInfo[7];
                            dtl.AccName = linInfo[8];
                            dtl.AmtIndex = linInfo[9];
                            dtl.OppProv = linInfo[10];
                            dtl.OppAccNo = linInfo[11];
                            dtl.OppCur = linInfo[12];
                            dtl.OppName = linInfo[13];
                            dtl.OppBkName = linInfo[14];
                            dtl.CshIndex = linInfo[15];
                            dtl.ErrDate = linInfo[16];
                            dtl.ErrVchNo = linInfo[17];
                            dtl.Amt = linInfo[18];
                            dtl.Bal = linInfo[19];
                            dtl.PreAmt = linInfo[20];
                            dtl.TotChg = linInfo[21];
                            dtl.VoucherType = linInfo[22];
                            dtl.VoucherProv = linInfo[23];
                            dtl.VoucherBat = linInfo[24];
                            dtl.VoucherNo = linInfo[25];
                            dtl.CustRef = linInfo[26];
                            dtl.TransCode = linInfo[27];
                            dtl.Teller = linInfo[28];
                            dtl.VchNo = linInfo[29];
                            dtl.Abs = linInfo[30];
                            dtl.PostScript = linInfo[31];
                            dtl.TrFrom = linInfo[32];
                            #endregion
                            dtlList.Add(dtl);
                        }
                    }
                }
            }
            #endregion
            return dtlList;
        }
    }
}
