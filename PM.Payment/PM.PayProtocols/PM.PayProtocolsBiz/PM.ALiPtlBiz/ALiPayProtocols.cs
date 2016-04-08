using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.ALiPay;
using PM.PaymentProtocolModel;
using System.Security.Cryptography;
using PM.PaymentProtocolModel.BankCommModel;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using PM.Utils;
using PM.Utils.Log;
using PM.ALiPtlBiz.Ali;

namespace PM.ALiPtlBiz
{
    public partial class ALiProtocols
    {
        #region  支付请求
        /// <summary>
        /// 支付请求
        /// </summary>
        /// <param name="payModel"></param>
        /// <param name="cfgInfo"></param>
        /// <returns></returns>
        private ResultInfo PayRequest(ALiPayModel payModel, CfgInfo cfgInfo)
        {
            payModel.Partner = ConfigHelper.GetCustomCfg("ALi", "ALiPartner");
            payModel.Key = ConfigHelper.GetCustomCfg("ALi", "ALiKey");
            payModel.InputCharset = ConfigHelper.GetCustomCfg("ALi", "ALiInputCharset");
            payModel.SignType = ConfigHelper.GetCustomCfg("ALi", "ALiSignType");
            payModel.Account = ConfigHelper.GetCustomCfg("ALi", "ALiAccount");
            ResultInfo rtn = new ResultInfo();
            var _key = payModel.Key.Trim();
            var _input_charset = payModel.InputCharset.Trim().ToLower();
            var _sign_type = payModel.SignType.ToUpper();  

            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            #region //构造签名参数数组
            #region old
            //sParaTemp.Add("service", "create_direct_pay_by_user");
            //sParaTemp.Add("payment_type", "1");
            //sParaTemp.Add("partner", payModel.Partner);
            //sParaTemp.Add("seller_email", payModel.Account);
            //sParaTemp.Add("return_url", cfgInfo.NotificationURL);
            //sParaTemp.Add("notify_url", cfgInfo.NotificationBgURL);
            //sParaTemp.Add("_input_charset", _input_charset);
            //sParaTemp.Add("show_url",cfgInfo.NotificationURL);// payModel.ShowUrl);
            //sParaTemp.Add("out_trade_no", payModel.OrderNo);
            //sParaTemp.Add("subject", payModel.Subject);
            //sParaTemp.Add("body", payModel.Body);
            //sParaTemp.Add("total_fee", payModel.Amount);
            //sParaTemp.Add("paymethod", payModel.Paymethod);
            //sParaTemp.Add("defaultbank", payModel.DefaultBank);
            //sParaTemp.Add("anti_phishing_key", payModel.AntiPhishingKey);
            //sParaTemp.Add("exter_invoke_ip", payModel.ExterInvokeIp);
            //sParaTemp.Add("extra_common_param", payModel.ExtraCommonParam ?? string.Empty);
            //sParaTemp.Add("buyer_email", payModel.DefaultBuyAccount);
            //sParaTemp.Add("royalty_type", payModel.RoyaltyType);
            //sParaTemp.Add("royalty_parameters", payModel.RoyaltyParameters);
            //var sPara = Para_filter(sParaTemp);
            ////获得签名结果
            //var mysign = Build_mysign(sPara, _key, _sign_type, _input_charset); 
            //rtn.NoticeMsg = Build_Form(sPara, payModel, cfgInfo, mysign);
            #endregion
            sParaTemp.Add("partner", payModel.Partner);
            sParaTemp.Add("_input_charset", _input_charset);
            sParaTemp.Add("service", "create_direct_pay_by_user");
            sParaTemp.Add("payment_type", "1");
            sParaTemp.Add("notify_url", cfgInfo.NotificationBgURL.Trim());
            sParaTemp.Add("return_url", cfgInfo.NotificationURL.Trim());
            sParaTemp.Add("seller_email", payModel.Account.Trim());
            sParaTemp.Add("out_trade_no", payModel.OrderNo.Trim());
            sParaTemp.Add("subject", payModel.Subject.Trim());
            sParaTemp.Add("total_fee", payModel.Amount.Trim());
            sParaTemp.Add("body", payModel.Body.Trim());
            sParaTemp.Add("show_url", cfgInfo.RootFilePath.Trim());
            sParaTemp.Add("anti_phishing_key", payModel.AntiPhishingKey.Trim());
            sParaTemp.Add("exter_invoke_ip", payModel.ExterInvokeIp.Trim());
            AliConfig cfg = new AliConfig();
            cfg.GATEWAY_NEW = cfgInfo.RequestURL;
            cfg.Input_charset = _input_charset;
            cfg.Key = _key;
            cfg.Partner = payModel.Partner;
            cfg.Sign_type = _sign_type;
            AlipaySubmit sbm = new AlipaySubmit(cfg);
            //建立请求
            string sHtmlText = sbm.BuildRequest(sParaTemp, "Post", "确认"); 
            #endregion 
            rtn.NoticeMsg = sHtmlText;
            rtn.Result = ResultType.Success;
            return rtn;
        }
        #endregion

        #region   支付响应
        /// <summary>
        /// 支付响应
        /// </summary>
        /// <param name="sendInfo">响应对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        public ResultInfo PayResponse(PubCallBackModel sendInfo, CfgInfo cfgInfo)
        {
            ResultInfo model = new ResultInfo();
            model.TradeAccount = new TradeAccountDetail();

            string rtnStr = string.Empty;
            #region 参数
            var _key = ConfigHelper.GetCustomCfg("ALi", "ALiKey");
            var _sign_type = ConfigHelper.GetCustomCfg("ALi", "ALiSignType");
            var _input_charset = ConfigHelper.GetCustomCfg("ALi", "ALiInputCharset");
            var _partner = ConfigHelper.GetCustomCfg("ALi", "ALiPartner"); 
            var account = ConfigHelper.GetCustomCfg("ALi", "ALiAccount");

            AliConfig cfg = new AliConfig(); 
            cfg.Input_charset = _input_charset;
            cfg.Key = _key;
            cfg.Partner = _partner;
            cfg.Sign_type = _sign_type;
            cfg.VerifyUrl = cfgInfo.RequestURL;
            #endregion
            var inputPara = GetRequest(sendInfo.MessagePaket);
            var sign = inputPara["sign"];         //获取支付宝反馈回来的sign结果
            var notify_id = inputPara["notify_id"];
            AlipayNotify aliNotify = new AlipayNotify(cfg);
            bool verifyResult = aliNotify.Verify(inputPara, notify_id, sign);

            if (verifyResult)//验证成功
            {
                model.SerialNumber = inputPara["trade_no"];         //支付宝交易号
                model.OrderNo = inputPara["out_trade_no"];     //获取订单号
                double amount = 0;
                double.TryParse(inputPara["total_fee"], out  amount);

                model.TradeAccount.Amount = amount;       //获取总金额
                //model.tr = inputPara["subject"];           //商品名称、订单名称
                //model.Body = inputPara["body"];                 //商品描述、订单备注、描述
                model.TradeAccount.AccNo = inputPara["buyer_email"];   //买家支付宝账号
                string trade_status = inputPara["trade_status"]; //交易状态
                if (inputPara["trade_status"] == "TRADE_FINISHED" || inputPara["trade_status"] == "TRADE_SUCCESS")
                {
                    model.Result = ResultType.Success;
                    model.StatusDes = "success";
                }
                else
                {
                    model.StatusDes = "fail";
                }
            }
            else
            {
                LogTxt.WriteEntry(string.Format("支付宝签名验证失败订单号{0}", inputPara["out_trade_no"]), "支付宝支付信息");
            }
            return model;
        }
        #endregion

        #region   辅助类
        /// <summary>
        /// 响应获取键值对
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private SortedDictionary<string, string> GetRequest(string message)
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            String[] messageArray = message.Split('&');
            Array.ForEach(messageArray,
                p =>
                {
                    if (!string.IsNullOrEmpty(p.Trim()))
                    {
                        var keys = p.Split('=');
                        sArray.Add(keys[0], keys[1]);
                    }
                });
            return sArray;
        }

        /// <summary>
        /// 获取远程服务器ATN结果
        /// </summary>
        /// <param name="strUrl">指定URL路径地址</param>
        /// <param name="timeout">超时时间设置</param>
        /// <returns>服务器ATN结果</returns>
        private string Get_Http(string strUrl, int timeout)
        {
            string strResult;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Timeout = timeout;
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.Default);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }

                strResult = strBuilder.ToString();
            }
            catch (Exception exp)
            {
                strResult = "错误：" + exp.Message;
            }
            return strResult;
        }

        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        private Dictionary<string, string> Para_filter(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key.ToLower() != "sign" && temp.Key.ToLower() != "sign_type" && temp.Value != "")
                {
                    dicArray.Add(temp.Key.ToLower(), temp.Value);
                }
            }
            return dicArray;
        }

        /// <summary>
        /// 生成签名结果
        /// </summary>
        /// <param name="sArray">要签名的数组</param>
        /// <param name="key">安全校验码</param>
        /// <param name="sign_type">签名类型</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>签名结果字符串</returns>
        private string Build_mysign(Dictionary<string, string> dicArray, string key, string sign_type, string _input_charset)
        {
            string prestr = Create_linkstring(dicArray);  //把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr = prestr.Substring(0, nLen - 1);
            prestr = prestr + key;                      //把拼接后的字符串再与安全校验码直接连接起来
            string mysign = Sign(prestr, sign_type, _input_charset);	//把最终的字符串签名，获得签名结果
            return mysign;
        }
        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        private string Create_linkstring(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }
            return prestr.ToString();
        }
        /// <summary>
        /// 签名字符串
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="sign_type">签名类型</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>签名结果</returns>
        private string Sign(string prestr, string sign_type, string _input_charset)
        {
            StringBuilder sb = new StringBuilder(32);
            if (sign_type.ToUpper() == "MD5")
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
                for (int i = 0; i < t.Length; i++)
                {
                    sb.Append(t[i].ToString("x").PadLeft(2, '0'));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 构造表单提交HTML
        /// </summary>
        /// <returns>输出 表单提交HTML文本</returns>
        public string Build_Form(Dictionary<string, string> sPara, ALiPayModel payModel, CfgInfo cfgInfo, string mysign)
        {
            var _key = payModel.Key.Trim();
            var _input_charset = payModel.InputCharset.ToLower();
            var _sign_type = payModel.SignType.ToUpper();
            StringBuilder sbHtml = new StringBuilder();
            //GET方式传递
            sbHtml.Append("<form id=\"alipaysubmit\" name=\"alipaysubmit\" action=\"" + cfgInfo.RequestURL + "_input_charset=" + _input_charset + "\" method=\"get\">");
            //POST方式传递（GET与POST二必选一）
            //sbHtml.Append("<form id=\"alipaysubmit\" name=\"alipaysubmit\" action=\"" + gateway + "_input_charset=" + _input_charset + "\" method=\"post\">");
            foreach (KeyValuePair<string, string> temp in sPara)
            {
                sbHtml.Append("<input type=\"hidden\" name=\"" + temp.Key + "\" value=\"" + temp.Value + "\"/>");
            }
            sbHtml.Append("<input type=\"hidden\" name=\"sign\" value=\"" + mysign + "\"/>");
            sbHtml.Append("<input type=\"hidden\" name=\"sign_type\" value=\"" + _sign_type + "\"/>");
            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type=\"submit\" value=\"支付宝确认付款\"></form>");
            sbHtml.Append("<script>document.forms['alipaysubmit'].submit();</script>");
            return sbHtml.ToString();
        }
        #endregion
    }
}
