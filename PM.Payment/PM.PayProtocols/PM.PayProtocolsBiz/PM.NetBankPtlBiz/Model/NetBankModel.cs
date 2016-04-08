using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel;


namespace PM.NetBankPtlBiz.Model
{
    #region  请求
    /// <summary>
    /// 1111网银支付信息(作为基类)
    /// </summary>
    public class PayInfo
    {
        /// <summary>
        /// 机构号码(嘉善为 000226)
        /// </summary>
        public string InstitutionID { get; set; }
        /// <summary>
        /// 支付流水号
        /// </summary>
        public string PaymentNo { get; set; }
        /// <summary>
        /// 订单金额(需要*100)
        /// </summary>
        public long Amount { get; set; }
        /// <summary>
        /// 支付服务手续费  默认为10元(需要*100)
        /// </summary>
        public long Fee { get; set; }
        /// <summary>
        /// 付款者ID
        /// </summary>
        public string PayerID { get; set; }
        /// <summary>
        /// 付款者名称
        /// </summary>
        public string PayerName { get; set; }
        /// <summary>
        /// 结算标识
        /// </summary>
        public string SettlementFlag { get; set; }
        /// <summary>
        /// 资金用途
        /// </summary>
        public string Usage { get; set; }
        /// <summary>
        /// 订单描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 通知URL
        /// </summary>
        public string NotificationURL { get; set; }
        /// <summary>
        /// 银行ID
        /// </summary>
        public string BankID { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        public int AccountType { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessType txCode { get; set; }
        /// <summary>
        /// 业务描述
        /// </summary>
        public string txName { get; set; }
        /// <summary>
        /// 报文
        /// </summary>
        public string PlainText { get; set; }
        /// <summary>
        /// 用户自定义信息，用于在网关页面显示 
        /// </summary>
        public string Note { get; set; }
        #region  签名等相关
        /// <summary>
        /// 提交地址(银行接口地址)
        /// </summary>
        public string ActionURL { get; set; }
        /// <summary>
        /// 签名信息
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 报文
        /// </summary>
        public string Message { get; set; }
        #endregion

    }

    /// <summary>
    /// 1311退还保证金
    /// </summary>
    public class Pay1311Info : PayInfo
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 收款人
        /// </summary>
        public string Payees { get; set; }
    }
    /// <summary>
    /// 保证金结算处理
    /// </summary>
    public class Pay1341Info : Pay1311Info
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 支付账号名称
        /// </summary>
        public string PaymentAccountName { get; set; }
        /// <summary>
        /// 支付账号号码
        /// </summary>
        public string PaymentAccountNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        /// <summary>
        /// 分支行名称
        /// </summary>
        public string BranchName { get; set; }
        /// <summary>
        /// 分支行省市
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 分支行城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 结果集
        /// </summary>
        public bool Result { get; set; }
    }

    #region  代付
    /// <summary>
    /// 批量代付
    /// </summary>
    public class BatchStayPays
    {
        /// <summary>
        /// 机构号码
        /// </summary>
        public string InstitutionID { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 订单金额(需要*100)
        /// </summary>
        public long TotalAmount { get; set; }
        /// <summary>
        /// 支付服务手续费  默认为10元(需要*100)
        /// </summary>
        public long Fee { get; set; }
        /// <summary>
        /// 总比数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 明细
        /// </summary>
        public List<BatchInfo> BatchList { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 报文
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 成功时间
        /// </summary> 
        public string SuccessTime { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string TxCode { get; set; }
        /// <summary>
        /// 业务描述
        /// </summary>
        public string TxName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 响应报文
        /// </summary>
        public string PlainText { get; set; }
        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Result { get; set; }
    }
    /// <summary>
    /// 批次明细
    /// </summary>
    public class BatchInfo
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public string ItemNo { get; set; }
        /// <summary>
        /// 订单金额(需要*100)
        /// </summary>
        public long Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 账号      （必要信息）
        /// </summary>
        public string AccNo { get; set; }
        /// <summary>
        /// 账户名     
        /// </summary>
        public string AccDbName { get; set; }
        /// <summary>
        /// 账户开户行（必要信息）
        /// </summary>
        public string AccDBBank { get; set; }
        /// <summary>
        /// 账户开户行行号（必要信息）
        /// </summary>
        public string AccDBBankNo { get; set; }
        /// <summary>
        /// 银行ID（代码）
        /// </summary>
        public string BankID { get; set; }

        #region 预留
        /// <summary>
        /// 结算标示(结算到那个账户)
        /// </summary>
        public string SettingAccNo { get; set; }
        /// <summary>
        /// 机构代码
        /// </summary>
        public string StructCode { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public string AccCur { get; set; }
        #endregion

        /// <summary>
        /// 省
        /// </summary>
        public string AccPro { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string AccCity { get; set; }

        /// <summary>
        /// 账户类型 个人11、企业12
        /// </summary>
        public string AccType { get; set; }


        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 开户证件类型
        /// </summary>
        public string IdentificationType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdentificationNumber { get; set; }
    }
    #endregion


    #endregion

    #region   响应
    /// <summary>
    /// 1111    1311 报文响应
    /// </summary>
    public class Notice1118Or1318Or1348ResponseInfo
    {
        /// <summary>
        /// 金额
        /// </summary>
        public long Amount { get; set; }
        /// <summary>
        /// 通知时间
        /// </summary>
        public string BankNotificationTime { get; set; }
        /// <summary>
        /// 机构号码
        /// </summary>
        public string InstitutionID { get; set; }
        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PaymentNo { get; set; }
        /// <summary>
        /// 支付流水号
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 状态值
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusDes
        {
            get
            {
                return Enum.Parse(typeof(RtnStatus), Status.ToString()).ToString();
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 签名信息
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 报文
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 成功时间
        /// </summary> 
        public string SuccessTime { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string TxCode { get; set; }
        /// <summary>
        /// 业务描述
        /// </summary>
        public string TxName { get; set; }
        /// <summary>
        /// 响应报文
        /// </summary>
        public string PlainText { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 设置返回信息 供 网银网关程序判断是否推送成功
        /// </summary>
        public string MessageResponse { get; set; }
    }

    #region  查询响应

    /// <summary>
    /// 查询下载对应 响应（公用部分相对较少故未抽象部分对象）
    /// </summary>
    public class Notice1810ResponseInfo
    {
        /// <summary>
        /// 机构号
        /// </summary>
        public string InstitutionID { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 报文
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 成功时间
        /// </summary> 
        public string SuccessTime { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string TxCode { get; set; }
        /// <summary>
        /// 业务描述
        /// </summary>
        public string TxName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 响应报文
        /// </summary>
        public string PlainText { get; set; }
        /// <summary>
        /// 查询明细1810专用
        /// </summary>
        public List<Tx1810> TX { get; set; }
        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Result { get; set; }
        ///// <summary>
        ///// 订单号（流水号）
        ///// </summary>
        //public string PaymentNo { get; set; }

    }
    /// <summary>
    /// 查询支付人信息
    /// </summary>
    public class Notice1121ResponseInfo
    {
        /// <summary>
        /// 机构号
        /// </summary>
        public string InstitutionID { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 报文
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 成功时间
        /// </summary> 
        public string SuccessTime { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string TxCode { get; set; }
        /// <summary>
        /// 业务描述
        /// </summary>
        public string TxName { get; set; }
        /// <summary>
        /// 响应报文
        /// </summary>
        public string PlainText { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 订单号（流水号）
        /// </summary>
        public string PaymentNo { get; set; }

        #region  获取支付人信息  供1121 使用
        /// <summary>
        /// 付款人账户名称
        /// </summary>
        public string PayerAccountName { get; set; }
        /// <summary>
        /// 付款人账号
        /// </summary>
        public string PayerAccountNumber { get; set; }
        /// <summary>
        /// 分支行
        /// </summary>
        public string PayerBranchName { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string PayerProvince { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string PayerCity { get; set; }
        #endregion

    }

    /// <summary>
    /// 商户订单支付或市场订单支付查询信息(1120 、1320)
    /// </summary>
    public class Notice1120Or1320ResponseInfo
    {
        /// <summary>
        /// 机构号
        /// </summary>
        public string InstitutionID { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 报文
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 成功时间
        /// </summary> 
        public string SuccessTime { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string TxCode { get; set; }
        /// <summary>
        /// 业务描述
        /// </summary>
        public string TxName { get; set; }
        /// <summary>
        /// 响应报文
        /// </summary>
        public string PlainText { get; set; }
        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 订单号（流水号）
        /// </summary>
        public string PaymentNo { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public long Amonut { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 支付平台收到银行通知时间
        /// </summary>
        public string BankNotificationTime { get; set; }
        /// <summary>
        /// 状态值
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusDes
        {
            get
            {

                return Status == 20 ? "已支付" : "未支付";
            }
        }
    }
    /// <summary>
    /// 市场订单结算查询(1350)
    /// </summary>
    public class Notice1350ResponseInfo
    {
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 报文
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 机构号
        /// </summary>
        public string InstitutionID { get; set; }

        /// <summary>
        /// 原结算交易流水号
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 订单号（流水号）
        /// </summary>
        public string PaymentNo { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public long Amount { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string TxCode { get; set; }
        /// <summary>
        /// 业务描述
        /// </summary>
        public string TxName { get; set; }
        /// <summary>
        /// 响应报文
        /// </summary>
        public string PlainText { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        public int AccountType { get; set; }
        /// <summary>
        /// 账户类型描述
        /// </summary>
        public string AccountTypeDes
        {
            get
            {
                string des = string.Empty;
                if (AccountType == 11)
                {
                    des = "个人账户";
                }
                else if (AccountType == 12)
                {
                    des = "企业账户";
                }
                else if (AccountType == 20)
                {
                    des = "支付平台内部账户";
                }
                return des;
            }
        }

        /// <summary>
        /// 成功时间
        /// </summary> 
        public string SuccessTime { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Result { get; set; }


        #region  获取支付人信息  供1350 使用
        /// <summary>
        /// 账户名称（支付平台账户）
        /// </summary>
        public string PayerAccountName { get; set; }
        /// <summary>
        /// 账户号码（支付平台账户）
        /// </summary>
        public string PayerAccountNumber { get; set; }


        /// <summary>
        /// 收款方银行编号
        /// </summary>
        public string BankID { get; set; }
        /// <summary>
        /// 收款方账户名称
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 收款方账户号码
        /// </summary>
        public string AccountNumber { get; set; }
        /// <summary>
        /// 收款方分支行名称
        /// </summary>
        public string BranchName { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        #endregion

        /// <summary>
        /// 状态值
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusDes
        {
            get
            {
                string des = string.Empty;
                if (AccountType == 10)
                {
                    des = "已经受理";
                }
                else if (AccountType == 30)
                {
                    des = "正在结算";
                }
                else if (AccountType == 40)
                {
                    des = "已经执行(已发送转账指令)";
                }
                return des;
            }
        }


    }









    /// <summary>
    /// 查询明细
    /// </summary>
    public class Tx1810
    {
        /// <summary>
        /// 交易类型
        /// </summary>
        public string TxType { get; set; }
        /// <summary>
        /// 交易编号
        /// </summary>     
        public string TxSn { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public long TxAmount { get; set; }
        /// <summary>
        /// 机构应收的金额
        /// </summary>
        public long InstitutionAmount { get; set; }
        /// <summary>
        /// 支付平台应收的金额
        /// </summary>
        public long PaymentAmoun { get; set; }
        /// <summary>
        /// 付款人手续费
        /// </summary>
        public long PayerFee { get; set; }
        /// <summary>
        /// 机构手续
        /// </summary>
        public long InstitutionFee { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 支付平台收到银行通知时间，格式：YYYYMMDDhhmmss
        /// </summary>
        public string BankNotificationTime { get; set; }
        /// <summary>
        /// 结算标示
        /// </summary>
        public string SettlementFlag { get; set; }
    }

    #endregion
    #endregion

    #region  public
    /// <summary>
    /// 返回状态
    /// </summary>
    public enum RtnStatus
    {
        成功受理了请求 = 2000,
        系统内部错误 = 2001,
        验证签名失败 = 2002,
        解析报文错误 = 2003,
        报文格式错误 = 2004,
        不支持的版本 = 2005,
        无效交易类型 = 2006,
        无此操作的权限 = 2007,
        重复交易请求 = 2008,
        无效日期 = 2009,
        无效日期范围 = 2010,
        数据库存取异常 = 2011,
        会话不存在 = 2012,
        系统不支持GET方法 = 2013,
        参数不正确 = 2016,
        交易证书过期 = 2017,
        请求中不存在message参数或者signature参数 = 240001,
        不存在此交易代码请查看参数TxCode = 240002,
        不存此机构请查看参数InstitutionID = 240003,
        不存在此订单请查看参数OrderNo = 240004,
        不存在此支付交易请查看参数PaymentNo = 240005,
        不存在此结算交易请查看参数SerialNumber = 240006,
        订单号长度不正确请查看参数OrderNo = 240007,
        支付交易流水号长度不正确请查看参数PaymentNo = 240008,
        交易流水号长度不正确请查看参数SerialNumber = 240009,
        订单号重复请查看参数OrderNo = 240010,
        支付交易流水号重复请查看参数PaymentNo = 240011,
        交易流水号重复请查看参数SerialNumber = 240012,
        账户类型错误请查看参数AccountType = 240013,
        金额格式不对 = 240014,
        结算金额大于可结算金额 = 240015,
        备注信息太多 = 240016,
        该笔订单没有支付不能退款 = 240017,
        退款累计金额大于订单金额 = 240018,
        账户名称与账户号码不匹配 = 240019,
        不存在此退款交易请查看参数SerialNumber = 240020,
        金额必须大于0 = 240021,
        银行账户信息不完整 = 240022,
        该笔订单已经退款只能退款一次 = 240023,
        支付平台账户不存在 = 240024,
        无结算对账记录 = 240025,
        金额长度太大 = 240026,
        日期格式错误 = 240027,
        保证金退息标志不得为空并且只能为0或1 = 240028,
        该笔订单的退款正在处理请等待后续通知 = 240029,
        开始时间大于结束时间 = 240030,
        监管银行为空请联系支付平台工作人员配置监管银行 = 240031,
        每日15点之后不可以做保证金退款 = 240032,
        未找到对应批次号的代付 = 250001,
        未找到对应批次号的代付明细 = 250002,
        批次号重复 = 250003,
        批备注过长 = 250008,
        请查看参数BatchNo = 250009,
        总笔数格式不对 = 250015,
        总笔数必须大于0 = 250016,
        总笔数不一致 = 250017,
        总金额不一致 = 250018,
        批量付款明细记录为空 = 250020,
        明细号重复 = 250021,
        第X行明细号为空或长度不正确 = 250022,
        第X行明细金额为空 = 250023,
        第X行明细金额格式不对 = 250024,
        第X行银行ID为空 = 250025,
        代付不支持该ID银行 = 250026,
        批量付款明细超过1000笔 = 250029,
        第X行账户类型为空或值错误 = 250030,
        第X行账户名称为空或长度不正确 = 250031,
        第X行账户号码为空或长度不正确 = 250032,
        第X行分支行为空或长度不正确 = 250033,
        第X行分支行省份为空或长度不正确 = 250034,
        第X行分支行城市为空或长度不正确 = 250035,
        协议用户编号为空或长度不正确 = 250036,
        协议号为空或长度不正确 = 250037,
        批量付款明细记录格式不正确 = 250039,
        未找到对应日期的对账信息 = 250050
    }

    #endregion
}
