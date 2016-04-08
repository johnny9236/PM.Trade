using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PM.Utils.Log;

namespace PM.PaymentProtocolModel.BankCommModel.BOC
{
    /// <summary>
    /// 出入账对象
    /// </summary>
    public class BOCQueryAccountDtlResult
    {
        /// <summary>
        /// 交易返回码  B001,成功；B002,成功，未完；B003，交易数为0
        /// </summary>
        public string RspCod { get; set; }
        /// <summary>
        ///  rspmsg表示解释信息
        /// </summary>
        public string RspMsg { get; set; }
        /// <summary>
        /// 本次返回笔数
        /// </summary>
        public int TotalNum { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int NoteNum { get; set; }
        /// <summary>
        /// 交易明细
        /// </summary>
        public List<BOCQueryAccountDtlModel> AccountDtlList { get; set; }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="packetString">报文</param>
        /// <returns></returns>
        public bool GetModel(string packetString)
        {
            bool rst = false;
            try
            {
                var xdoc = XDocument.Parse(packetString);

                #region 返回状态
                var result = from c in xdoc.Descendants("status")
                             where c.Parent.Name == "trn-b2e0035-rs"
                             select new
                               {
                                   rspcod = c.Element("rspcod") == null ? string.Empty : c.Element("rspcod").Value,
                                   rspmsg = c.Element("rspmsg") == null ? string.Empty : c.Element("rspmsg").Value
                               };
                var recordCountInfo = from c in xdoc.Descendants("trn-b2e0035-rs")
                                      select new
                                        {
                                            totalnum = c.Element("totalnum") == null ? string.Empty : c.Element("totalnum").Value,
                                            notenum = c.Element("notenum") == null ? string.Empty : c.Element("notenum").Value
                                        };
                //返回结果
                if (result != null && result.Count() > 0)
                {
                    this.RspCod = result.FirstOrDefault().rspcod;
                    this.RspMsg = result.FirstOrDefault().rspmsg;
                    if (this.RspCod.ToLower() == "b001" || this.RspCod.ToLower() == "b002" || this.RspCod.ToLower() == "b003")
                        rst = true;
                }
                int count = 0;
                if (recordCountInfo != null && recordCountInfo.Count() > 0)
                {
                    int.TryParse(recordCountInfo.FirstOrDefault().notenum, out count);
                    this.NoteNum = count;
                    count = 0;
                    int.TryParse(recordCountInfo.FirstOrDefault().totalnum, out count);
                    this.TotalNum = count;
                }
                #endregion

                if (rst)//操作成功后解析明细
                {

                    BOCQueryAccountDtlModel accountModel = null;//明细对象
                    var detailInfo = from c in xdoc.Descendants("b2e0035-rs")
                                     select c;
                    if (detailInfo != null && detailInfo.Count() > 0)
                        this.AccountDtlList = new List<BOCQueryAccountDtlModel>();
                    foreach (var dtl in detailInfo)
                    {
                        accountModel = new BOCQueryAccountDtlModel();//明细对象
                        #region 解析
                        #region  状态
                        var dtlStatus = from info in dtl.Descendants("status")
                                        select new
                                        {
                                            rspcod = info.Element("rspcod") == null ? string.Empty : info.Element("rspcod").Value,
                                            rspmsg = info.Element("rspmsg") == null ? string.Empty : info.Element("rspmsg").Value
                                        };
                        if (dtlStatus != null && dtlStatus.Count() > 0)
                        {
                            accountModel.RspCod = dtlStatus.FirstOrDefault().rspcod;
                            accountModel.RspMsg = dtlStatus.FirstOrDefault().rspmsg;
                        }
                        #endregion

                        #region 付款方
                        var dtlFractn = from info in dtl.Descendants("fractn")
                                        select new
                                        {
                                            ibknum = info.Element("ibknum") == null ? string.Empty : info.Element("ibknum").Value,
                                            acntname = info.Element("acntname") == null ? string.Empty : info.Element("acntname").Value,
                                            ibkname = info.Element("ibkname") == null ? string.Empty : info.Element("ibkname").Value,
                                            actacn = info.Element("actacn") == null ? string.Empty : info.Element("actacn").Value
                                        };
                        if (dtlFractn != null && dtlFractn.Count() > 0)
                        {
                            accountModel.PayIbkNum = dtlFractn.FirstOrDefault().ibknum;
                            accountModel.PayAcntName = dtlFractn.FirstOrDefault().acntname;
                            accountModel.PayActacn = dtlFractn.FirstOrDefault().actacn;
                            accountModel.PayIbkName = dtlFractn.FirstOrDefault().ibkname;
                        }
                        #endregion

                        #region 收款方
                        var dtlToactn = from info in dtl.Descendants("toactn")
                                        select new
                                        {
                                            toibkn = info.Element("toibkn") == null ? string.Empty : info.Element("toibkn").Value,
                                            actacn = info.Element("actacn") == null ? string.Empty : info.Element("actacn").Value,
                                            toname = info.Element("toname") == null ? string.Empty : info.Element("toname").Value,
                                            tobank = info.Element("tobank") == null ? string.Empty : info.Element("tobank").Value
                                        };
                        if (dtlToactn != null && dtlToactn.Count() > 0)
                        {
                            accountModel.ReciveActacn = dtlToactn.FirstOrDefault().actacn;
                            accountModel.ReciveToBank = dtlToactn.FirstOrDefault().tobank;
                            accountModel.ReciveToIbkn = dtlToactn.FirstOrDefault().toibkn;
                            accountModel.ReciveToName = dtlToactn.FirstOrDefault().toname;
                        }
                        #endregion

                        #region  其他
                       var mactibkn = dtl.Element("mactibkn") == null ? string.Empty : dtl.Element("mactibkn").Value;//被代理行号
                        var mactacn = dtl.Element("mactacn") == null ? string.Empty : dtl.Element("mactacn").Value;//被代理账号
                        var mactname = dtl.Element("mactname") == null ? string.Empty : dtl.Element("mactname").Value;//被代理账户名
                        var mactbank = dtl.Element("mactbank") == null ? string.Empty : dtl.Element("mactbank").Value;//被代理账户开户行名
                        var vchnum = dtl.Element("vchnum") == null ? string.Empty : dtl.Element("vchnum").Value;//旧线是10位的凭证号或传票号，新线是9位流水号(不补0)+3位记录号

                        var transid = dtl.Element("transid") == null ? string.Empty : dtl.Element("transid").Value;//记录标识号(9位JournalNumber+9位RecordNum(原)+9位RecordNum)
                        var txndate = dtl.Element("txndate") == null ? string.Empty : dtl.Element("txndate").Value;//交易日期 YYYYMMDD（非空）
                        var txntime = dtl.Element("txntime") == null ? string.Empty : dtl.Element("txntime").Value;//交易时间 HH24MISS
                        var txnamt = dtl.Element("txnamt") == null ? string.Empty : dtl.Element("txnamt").Value;//金额（非空）
                        var acctbal = dtl.Element("acctbal") == null ? string.Empty : dtl.Element("acctbal").Value;//交易后余额

                        var avlbal = dtl.Element("avlbal") == null ? string.Empty : dtl.Element("avlbal").Value;//可用余额
                        var frzamt = dtl.Element("frzamt") == null ? string.Empty : dtl.Element("frzamt").Value;//冻结金额
                        var overdramt = dtl.Element("overdramt") == null ? string.Empty : dtl.Element("overdramt").Value;//透支额度
                        var avloverdramt = dtl.Element("avloverdramt") == null ? string.Empty : dtl.Element("avloverdramt").Value;//可用透支额度
                        var useinfo = dtl.Element("useinfo") == null ? string.Empty : dtl.Element("useinfo").Value;//用途

                        var furinfo = dtl.Element("furinfo") == null ? string.Empty : dtl.Element("furinfo").Value;//附言
                        var transtype = dtl.Element("transtype") == null ? string.Empty : dtl.Element("transtype").Value;//业务类型
                        var bustype = dtl.Element("bustype") == null ? string.Empty : dtl.Element("bustype").Value;//新业务类型,见附三
                        var trncur = dtl.Element("trncur") == null ? string.Empty : dtl.Element("trncur").Value;//货币名称（非空、如CNY或者001）
                        var direction = dtl.Element("direction") == null ? string.Empty : dtl.Element("direction").Value;//来往账标识（1-来账，2-往账）

                        var feeact = dtl.Element("feeact") == null ? string.Empty : dtl.Element("feeact").Value;//费用账户:收费交易通过一笔单独的交易来展示,所以该项返回空      
                        var feeamt = dtl.Element("feeamt") == null ? string.Empty : dtl.Element("feeamt").Value;//费用金额 :收费交易通过一笔单独的交易来展示,所以该项返回空 
                        var feecur = dtl.Element("feecur") == null ? string.Empty : dtl.Element("feecur").Value;//费用货币:收费交易通过一笔单独的交易来展示,所以该项返回空   
                        var valdat = dtl.Element("valdat") == null ? string.Empty : dtl.Element("valdat").Value;//日期YYYYMMDD
                        var vouchtp = dtl.Element("vouchtp") == null ? string.Empty : dtl.Element("vouchtp").Value;//凭证类型

                        var vouchnum = dtl.Element("vouchnum") == null ? string.Empty : dtl.Element("vouchnum").Value;//凭证号码唯一
                        var fxrate = dtl.Element("fxrate") == null ? string.Empty : dtl.Element("fxrate").Value;//汇率
                        var interinfo = dtl.Element("interinfo") == null ? string.Empty : dtl.Element("interinfo").Value;//整合信息，格式为：F:附言//A:摘要//U:用途//R:备注

                        var reserve1 = dtl.Element("reserve1") == null ? string.Empty : dtl.Element("reserve1").Value;//预留项
                        var reserve2 = dtl.Element("reserve2") == null ? string.Empty : dtl.Element("reserve2").Value;//预留项
                        var reserve3 = dtl.Element("reserve3") == null ? string.Empty : dtl.Element("reserve3").Value;//预留项3

                        accountModel.MactIbkn = mactibkn;
                        accountModel.Mactacn = mactacn;
                        accountModel.Mactname = mactname;
                        accountModel.MactBank = mactbank;
                        accountModel.Vchnum = vchnum;

                        accountModel.TransId = transid;

                        accountModel.TxnDate = txndate;
                        accountModel.TxnTime = txntime;
                        accountModel.TxNamt = txnamt;
                        accountModel.Acctbal = acctbal;
                        accountModel.Avlbal = avlbal;

                        accountModel.FrzAmt = frzamt;
                        accountModel.OverdrAmt = overdramt;
                        accountModel.AvloverdrAmt = avloverdramt;
                        accountModel.UseInfo = useinfo;
                        accountModel.FurInfo = furinfo;

                        accountModel.TransType = transtype;
                        accountModel.BusType = bustype;
                        accountModel.TrnCur = trncur;
                        accountModel.Direction = direction;
                        accountModel.FeeAct = feeact;

                        accountModel.FeeAmt = feeamt;
                        accountModel.FeeCur = feecur;
                        accountModel.ValDat = valdat;
                        accountModel.VouchTp = vouchtp;
                        accountModel.VouchNum = vouchnum;

                        accountModel.FxRate = fxrate;
                        accountModel.InterInfo = interinfo;
                        accountModel.Reserve1 = reserve1;
                        accountModel.Reserve2 = reserve2;
                        accountModel.Reserve3 = reserve3;

                     
                        #endregion
                        #endregion
                        this.AccountDtlList.Add(accountModel);
                    }
                }
            }
            catch (Exception ex)
            {
                rst = false;
                LogTxt.WriteEntry("异常信息:" + ex.Message, "中行入账明细信息");
                throw ex;
            }
            return rst;
        }

    }
    /// <summary>
    ///出入账明细(一次最多50条)
    /// </summary>
    public class BOCQueryAccountDtlModel
    {
        /// <summary>
        /// 交易返回码  B001表示处理成功
        /// </summary>
        public string RspCod { get; set; }
        /// <summary>
        ///  rspmsg表示解释信息
        /// </summary>
        public string RspMsg { get; set; }
        #region  付款
        /// <summary>
        /// 付款行号
        /// </summary>
        public string PayIbkNum { get; set; }
        /// <summary>
        /// 付款人开户行名
        /// </summary>
        public string PayIbkName { get; set; }
        /// <summary>
        /// 付款账号
        /// </summary>
        public string PayActacn { get; set; }
        /// <summary>
        /// 付款人
        /// </summary>
        public string PayAcntName { get; set; }
        #endregion
        #region  收款
        /// <summary>
        /// 收款行号
        /// </summary>
        public string ReciveToIbkn { get; set; }
        /// <summary>
        /// 收款账号
        /// </summary>
        public string ReciveActacn { get; set; }
        /// <summary>
        /// 收款人
        /// </summary>
        public string ReciveToName { get; set; }
        /// <summary>
        /// 收款人开户行名
        /// </summary>
        public string ReciveToBank { get; set; }
        #endregion
        /// <summary>
        /// 被代理行号
        /// </summary>
        public string MactIbkn { get; set; }
        /// <summary>
        /// 被代理账号
        /// </summary>
        public string Mactacn { get; set; }
        /// <summary>
        /// 被代理账户名
        /// </summary>
        public string Mactname { get; set; }
        /// <summary>
        /// 被代理账户开户行名
        /// </summary>
        public string MactBank { get; set; }
        /// <summary>
        /// 旧线是10位的凭证号或传票号，新线是9位流水号(不补0)+3位记录号
        /// </summary>
        public string Vchnum { get; set; }
        /// <summary>
        /// 记录标识号(9位JournalNumber+9位RecordNum(原)+9位RecordNum)
        /// </summary>
        public string TransId { get; set; }
        /// <summary>
        /// 交易日期 YYYYMMDD（非空）
        /// </summary>
        public string TxnDate { get; set; }
        /// <summary>
        /// 交易时间 HH24MISS
        /// </summary>
        public string TxnTime { get; set; }
        /// <summary>
        /// 	金额（非空）
        /// </summary>
        public string TxNamt { get; set; }
        /// <summary>
        /// 交易后余额
        /// </summary>
        public string Acctbal { get; set; }
        /// <summary>
        /// 可用余额
        /// </summary>
        public string Avlbal { get; set; }
        /// <summary>
        /// 冻结金额
        /// </summary>
        public string FrzAmt { get; set; }
        /// <summary>
        /// 	透支额度
        /// </summary>
        public string OverdrAmt { get; set; }
        /// <summary>
        /// 可用透支额度
        /// </summary>
        public string AvloverdrAmt { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        public string UseInfo { get; set; }
        /// <summary>
        /// 附言 (网银行内交易：OBSS+交易流水号后12位+GIRO+客户业务编号后12位
        /// 用途；网银跨行交易：OBSS+交易流水号后12位+GIRO+客户业务编号后12位用途//用途，)
        /// </summary>
        public string FurInfo { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string TransType { get; set; }
        /// <summary>
        /// 新业务类型
        /// </summary>
        public string BusType { get; set; }
        /// <summary>
        /// 货币名称（非空、如CNY或者001）
        /// </summary>
        public string TrnCur { get; set; }
        /// <summary>
        /// 	来往账标识（1-来账，2-往账）
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// 费用账户:收费交易通过一笔单独的交易来展示,所以该项返回空      
        /// </summary>
        public string FeeAct { get; set; }
        /// <summary>
        /// 费用金额 :收费交易通过一笔单独的交易来展示,所以该项返回空   
        /// </summary>
        public string FeeAmt { get; set; }
        /// <summary>
        /// 费用货币:收费交易通过一笔单独的交易来展示,所以该项返回空   
        /// </summary>
        public string FeeCur { get; set; }
        /// <summary>
        /// 起息日期YYYYMMDD
        /// </summary>
        public string ValDat { get; set; }
        /// <summary>
        /// 凭证类型，具体解释见附件5
        /// </summary>
        public string VouchTp { get; set; }
        /// <summary>
        /// 凭证号码(唯一值入库)
        /// </summary>
        public string VouchNum { get; set; }
        /// <summary>
        /// 汇率
        /// </summary>
        public string FxRate { get; set; }
        /// <summary>
        /// 整合信息，格式为：F:附言//A:摘要//U:用途//R:备注
        /// </summary>
        public string InterInfo { get; set; }
        /// <summary>
        /// 预留项
        /// </summary>
        public string Reserve1 { get; set; }
        /// <summary>
        /// 预留项
        /// </summary>
        public string Reserve2 { get; set; }
        /// <summary>
        /// 预留项
        /// </summary>
        public string Reserve3 { get; set; }
    }
}
