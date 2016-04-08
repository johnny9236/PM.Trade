using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel;
using PM.PaymentProtocolModel.BankCommModel.LPSBBC;
using PM.PaymentProtocolModel.PubModel;

using PM.Utils.Log;
using System.Security.Cryptography;
using PM.Utils;
using CCBSign;


namespace PM.LPSCCBPtlBiz
{
    /// <summary>
    /// 支付
    /// </summary>
    public partial class BBCProtocols
    {
        #region   支付请求
        /// <summary>
        /// B2C支付
        /// </summary>
        /// <param name="model">b2c对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private ResultInfo PayB2C(BBCB2CPay model, CfgInfo cfgInfo)
        {
            ResultInfo rst = new ResultInfo();
            var paramStr = string.Format("MERCHANTID={0}&POSID={1}&BRANCHID={2}&ORDERID={3}&PAYMENT={4}&CURCODE={5}&TXCODE={6}&REMARK1={7}&REMARK2={8}",
                model.MERCHANTID,
                model.POSID,
                model.BRANCHID,
                model.OrderNo,
                model.PAYMENT,
                model.CURCODE,
                model.TXCODE,
                model.Remark,
                model.Remark2);
            var toSign = paramStr + string.Format("&TYPE={0}&PUB={1}&GATEWAY={2}&CLIENTIP={3}&REGINFO={4}&PROINFO={5}&REFERER={6}",
                model.TYPE,
                model.PUB.Substring(model.PUB.Length - 30),
                model.GATEWAY,
                model.CLIENTIP,
                string.IsNullOrEmpty(model.REGINFO) == true ? string.Empty : System.Web.HttpUtility.UrlEncodeUnicode(model.REGINFO),
               string.IsNullOrEmpty(model.PROINFO) == true ? string.Empty : System.Web.HttpUtility.UrlEncodeUnicode(model.PROINFO),
                model.REFERER);

            var toPost = paramStr + string.Format("&TYPE={0}&GATEWAY={1}&CLIENTIP={2}&REGINFO={3}&PROINFO={4}&REFERER={5}",
                model.TYPE,
                model.GATEWAY,
                model.CLIENTIP,
                string.IsNullOrEmpty(model.REGINFO) == true ? string.Empty : System.Web.HttpUtility.UrlEncodeUnicode(model.REGINFO),
                string.IsNullOrEmpty(model.PROINFO) == true ? string.Empty : System.Web.HttpUtility.UrlEncodeUnicode(model.PROINFO),
                model.REFERER);
            var strMD5 = PM.Utils.StringHelper.MD5String(toSign);
            model.MAC = strMD5;
            var rtn = string.Format("{0}?{1}&MAC={2}", cfgInfo.RequestURL, toPost, strMD5);
            LogTxt.WriteEntry(string.Format("B2C发起url[{0}-{1}]",toSign, rtn), "六盘水b2c支付发起");
            if (!string.IsNullOrEmpty(strMD5))
            {
                rst.Result = ResultType.Success;
                rst.ActionURLToBank = rtn;
                #region post直接写url
                //rst.MessagePaket = string.Format(
                //"<input TYPE=\"hidden\" NAME=\"MERCHANTID\" VALUE=\"{0}\">" +
                //"<input TYPE=\"hidden\" NAME=\"POSID\" VALUE=\"{1}\">" +
                //"<input TYPE=\"hidden\" NAME=\"BRANCHID\" VALUE=\"{2}\">" +
                //"<input TYPE=\"hidden\" NAME=\"ORDERID\" VALUE=\"{3}\">" +
                //"<input TYPE=\"hidden\" NAME=\"PAYMENT\" VALUE=\"{4}\">" +
                //"<input TYPE=\"hidden\" NAME=\"CURCODE\" VALUE=\"{5}\">" +
                //"<input TYPE=\"hidden\" NAME=\"TXCODE\" VALUE=\"{6}\">" +
                //"<input TYPE=\"hidden\" NAME=\"REMARK1\" VALUE=\"{7}\">" +
                //"<input TYPE=\"hidden\" NAME=\"REMARK2\" VALUE=\"{8}\">" +
                //"<input TYPE=\"hidden\" NAME=\"TYPE\" VALUE=\"{9}\">" +
                //    //"<input TYPE=\"hidden\" NAME=\"PUB\" VALUE=\"{10}\">" +
                //"<input TYPE=\"hidden\" NAME=\"GATEWAY\" VALUE=\"{10}\">" +
                //"<input TYPE=\"hidden\" NAME=\"CLIENTIP\" VALUE=\"{11}\">" +
                //"<input TYPE=\"hidden\" NAME=\"REGINFO\" VALUE=\"{12}\">" +
                //"<input TYPE=\"hidden\" NAME=\"REFERER\" VALUE=\"{13}\">",
                //   model.MERCHANTID,
                //                model.POSID,
                //                model.BRANCHID,
                //                model.OrderNo,
                //                model.PAYMENT,
                //                model.CURCODE,
                //                model.TXCODE,
                //                model.Remark,
                //                model.Remark2,
                //                    model.TYPE,
                //    //      model.PUB.Substring(model.PUB.Length-30),

                //                "W1Z1",//model.GATEWAY,
                //                model.CLIENTIP,
                //                string.IsNullOrEmpty(model.REGINFO) == true ? string.Empty : System.Web.HttpUtility.UrlEncode(model.REGINFO),
                //                string.IsNullOrEmpty(model.PROINFO) == true ? string.Empty : System.Web.HttpUtility.UrlEncode(model.PROINFO),
                //                model.REFERER
                //                );
                #endregion
            }
            return rst;
        }

        /// <summary>
        /// B2B
        /// </summary>
        /// <param name="model">b2b对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private ResultInfo PayB2B(BBCB2BPay model, CfgInfo cfgInfo)
        {
            ResultInfo rst = new ResultInfo();
            var paramStr = string.Format("MERCHANTID={0}&POSID={1}&BRANCHID={2}&ORDERID={3}&PAYMENT={4}&CURCODE={5}&TXCODE={6}&REMARK1={7}&REMARK2={8}&PROJECTNO={9}&PAYACCNO={10}&ACCTYPE={11}&ENDTIME={12}&TYPE={13}&PUB={14}&REGINFO={15}&PROINFO={16}&REFERER={17}",
                model.MERCHANTID,
                model.POSID,
                model.BRANCHID,
                model.OrderNo,
                model.PAYMENT,
                model.CURCODE,
                model.TXCODE,
                model.Remark,
                model.Remark2,
                model.PROJECTNO,
                model.PAYACCNO,
                model.ACCTYPE,
                model.ENDTIME,
                model.TYPE,
                model.PUB.Trim().Substring(model.PUB.Trim().Length - 30),
                //string.IsNullOrEmpty(model.REGINFO) == true ? string.Empty : System.Web.HttpUtility.UrlEncodeUnicode(model.REGINFO),
                //string.IsNullOrEmpty(model.PROINFO) == true ? string.Empty : System.Web.HttpUtility.UrlEncodeUnicode(model.PROINFO),
                  string.IsNullOrEmpty(model.REGINFO) == true ? string.Empty : System.Web.HttpUtility.UrlEncodeUnicode(model.REGINFO),
                string.IsNullOrEmpty(model.PROINFO) == true ? string.Empty : System.Web.HttpUtility.UrlEncodeUnicode(model.PROINFO),
                model.REFERER);
            //var paramNoPubStr = string.Format("MERCHANTID={0}&POSID={1}&BRANCHID={2}&ORDERID={3}&PAYMENT={4}&CURCODE={5}&TXCODE={6}&REMARK1={7}&REMARK2={8}&PROJECTNO={9}&PAYACCNO={10}&ACCTYPE={11}&ENDTIME={12}&TYPE={13}&PUB={14}&REGINFO={15}&PROINFO={16}&REFERER={17}",
            var paramNoPubStr = string.Format("MERCHANTID={0}&POSID={1}&BRANCHID={2}&ORDERID={3}&PAYMENT={4}&CURCODE={5}&TXCODE={6}&REMARK1={7}&REMARK2={8}&PROJECTNO={9}&PAYACCNO={10}&ACCTYPE={11}&ENDTIME={12}&TYPE={13}&REGINFO={14}&PROINFO={15}&REFERER={16}",
                model.MERCHANTID,
                model.POSID,
                model.BRANCHID,
                model.OrderNo,
                model.PAYMENT,
                model.CURCODE,
                model.TXCODE,
                model.Remark,
                model.Remark2,
                model.PROJECTNO,
                model.PAYACCNO,
                model.ACCTYPE,
                model.ENDTIME,
                model.TYPE,
                // string.Empty, //model.PUB.Substring(model.PUB.Length - 30),
                string.IsNullOrEmpty(model.REGINFO) == true ? string.Empty : System.Web.HttpUtility.UrlEncodeUnicode(model.REGINFO),
                string.IsNullOrEmpty(model.PROINFO) == true ? string.Empty : System.Web.HttpUtility.UrlEncodeUnicode(model.PROINFO)
                ,                model.REFERER
                );

            //b2b非防钓鱼
            //var paramStr = string.Format("MERCHANTID={0}&POSID={1}&BRANCHID={2}&ORDERID={3}&PAYMENT={4}&CURCODE={5}&TXCODE={6}&REMARK1={7}&REMARK2={8}&PROJECTNO={9}&PAYACCNO={10}&ACCTYPE={11}&ENDTIME={12}&TYPE={13}",
            //  model.MERCHANTID,
            //  model.POSID,
            //  model.BRANCHID,
            //  model.OrderNo,
            //  model.PAYMENT,
            //  model.CURCODE,
            //  model.TXCODE,
            //  model.Remark,
            //  model.Remark2,
            //  model.PROJECTNO,
            //  model.PAYACCNO,
            //  model.ACCTYPE,
            //  model.ENDTIME,
            // "");

            var strMD5 = PM.Utils.StringHelper.MD5String(paramStr);
            model.MAC = strMD5;
           var rtn = string.Format("{0}?{1}&MAC={2}", cfgInfo.RequestURL, paramNoPubStr, strMD5);
           //var rtn = string.Format("{0}", cfgInfo.RequestURL);
            LogTxt.WriteEntry(string.Format("B2B发起url[{0}-{1}]",paramStr, rtn), "六盘水b2b支付发起");
            if (!string.IsNullOrEmpty(strMD5))
            {
                rst.Result = ResultType.Success;
                rst.ActionURLToBank = rtn;
                #region 参数暂时不用
//                rst.MessagePaket = string.Format(
//"<input TYPE=\"hidden\" NAME=\"MERCHANTID\" VALUE=\"{0}\">" +
//"<input TYPE=\"hidden\" NAME=\"POSID\" VALUE=\"{1}\">" +
//"<input TYPE=\"hidden\" NAME=\"BRANCHID\" VALUE=\"{2}\">" +
//"<input TYPE=\"hidden\" NAME=\"ORDERID\" VALUE=\"{3}\">" +
//"<input TYPE=\"hidden\" NAME=\"PAYMENT\" VALUE=\"{4}\">" +
//"<input TYPE=\"hidden\" NAME=\"CURCODE\" VALUE=\"{5}\">" +
//"<input TYPE=\"hidden\" NAME=\"TXCODE\" VALUE=\"{6}\">" +
//"<input TYPE=\"hidden\" NAME=\"REMARK1\" VALUE=\"{7}\">" +
//"<input TYPE=\"hidden\" NAME=\"REMARK2\" VALUE=\"{8}\">" +
//"<input TYPE=\"hidden\" NAME=\"PROJECTNO\" VALUE=\"{9}\">" +
//"<input TYPE=\"hidden\" NAME=\"PAYACCNO\" VALUE=\"{10}\">" +
//"<input TYPE=\"hidden\" NAME=\"ACCTYPE\" VALUE=\"{11}\">" +
//"<input TYPE=\"hidden\" NAME=\"ENDTIME\" VALUE=\"{12}\">" +
//"<input TYPE=\"hidden\" NAME=\"TYPE\" VALUE=\"{13}\">" +
//"<input TYPE=\"hidden\" NAME=\"REGINFO\" VALUE=\"{14}\">" +
//"<input TYPE=\"hidden\" NAME=\"PROINFO\" VALUE=\"{15}\">" +
//"<input TYPE=\"hidden\" NAME=\"REFERER\" VALUE=\"{16}\">" +
//"<input TYPE=\"hidden\" NAME=\"MAC\" VALUE=\"{17}\">",
//    model.MERCHANTID,
//                model.POSID,
//                model.BRANCHID,
//                model.OrderNo,
//                model.PAYMENT,
//                model.CURCODE,
//                model.TXCODE,
//                model.Remark,
//                model.Remark2,
//                model.PROJECTNO,
//                model.PAYACCNO,
//                model.ACCTYPE,
//                model.ENDTIME,
//                model.TYPE,
//                    //      model.PUB,
//                string.IsNullOrEmpty(model.REGINFO) == true ? string.Empty : System.Web.HttpUtility.UrlEncodeUnicode(model.REGINFO),
//                string.IsNullOrEmpty(model.PROINFO) == true ? string.Empty : System.Web.HttpUtility.UrlEncodeUnicode(model.PROINFO),
//                model.REFERER,
//                model.MAC
//                );
                #endregion
            }
            return rst;
        }
        #endregion
        #region   支付响应
        /// <summary>
        /// B2C支付响应
        /// </summary>
        /// <param name="model">响应对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        //private BBCB2CResult PayResponseB2C(CommPayReqestModel model, CfgInfo cfgInfo)
        private ResultInfo PayResponseB2C(CommPayReqestModel model, CfgInfo cfgInfo)
        {
            ResultInfo rst = null;
            LogTxt.WriteEntry(string.Format("获取验证参数[pubStr={0} sign={1} message={2}]", model.PubKey, model.Signature, model.MessagePaket), "六盘水b2c建行支付验证");
           
            RSASig sign = new RSASig();
            sign.setPublicKey(model.PubKey);
            bool result = sign.verifySigature(model.Signature, model.MessagePaket);
            if (true == result)//验证成功
            {
                double payment = 0;
                var inputPara = StringHelper.Para_filter(StringHelper.GetRequestParams(model.MessagePaket));//获取响应请求键值
                rst = new ResultInfo();
                rst.MSG = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "REMARK1") == null ? string.Empty : inputPara["REMARK1"];
                rst.NoticeMsg = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "REMARK1") == null ? string.Empty : inputPara["REMARK1"]; //备注 
                rst.Attach = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "REMARK2") == null ? string.Empty : inputPara["REMARK2"]; //备注 
                rst.Result = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "SUCCESS") == null ? ResultType.UnKnow : (inputPara["SUCCESS"] == "Y" ? ResultType.Success : ResultType.UnKnow);
                rst.OrderNo = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "ORDERID") == null ? string.Empty : inputPara["ORDERID"];
                if (rst.Result == ResultType.Success)
                {
                    rst.TradeAccount = new TradeAccountDetail();
                    rst.TradeAccount.AccType =
                        inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "ACC_TYPE") == null ? string.Empty : inputPara["ACC_TYPE"];  //账号类型
                    double.TryParse(
                        inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "PAYMENT") == null ? string.Empty : inputPara["PAYMENT"]
                         , out payment);
                    rst.TradeAccount.Amount = payment;//金额
                }
            }
            else//验证失败
            {
                LogTxt.WriteEntry("验证失败" + model.MessagePaket + model.Signature, "六盘水b2c建行支付");
            }
            return rst;
        }

        /// <summary>
        /// B2B支付响应
        /// </summary>
        /// <param name="model">响应对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private ResultInfo PayResponseB2B(CommPayReqestModel model, CfgInfo cfgInfo)
        {
            ResultInfo rst = null;
            LogTxt.WriteEntry(string.Format("获取验证参数[pubStr={0} sign={1} message={2}]", model.PubKey, model.Signature, model.MessagePaket), "六盘水b2B建行支付验证");
            RSASig sign = new RSASig();
              sign.setPublicKey(model.PubKey);
            bool result = sign.verifySigature(model.Signature, model.MessagePaket);
            if (true == result)//验证成功
            {
                var inputPara = StringHelper.Para_filter(StringHelper.GetRequestParams(model.ParmStr));//获取响应请求键值
                rst = new ResultInfo();
                rst.MSG = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "REMARK1") == null ? string.Empty : inputPara["REMARK1"];

                rst.NoticeMsg = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "REMARK1") == null ? string.Empty : inputPara["REMARK1"];//备注 
                rst.Attach = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "REMARK2") == null ? string.Empty : inputPara["REMARK2"];//备注2
              //  rst.Result = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "STATUS") == null ? ResultType.None : (inputPara["STATUS"] == "2" ? ResultType.Success : (  inputPara["STATUS"] == "5" ? ResultType.UnKnow:ResultType.Faile ));
                var resultInfo = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "STATUS") == null ? string.Empty : inputPara["STATUS"];//状态
              if (!string.IsNullOrEmpty(resultInfo ))
                {
                    if (resultInfo == "2")
                    {
                        rst.Result = ResultType.Success;
                    }
                    else if (resultInfo == "5")
                    {
                        rst.Result = ResultType.Faile;
                    }
                    else if (resultInfo == "6")
                    {
                        rst.Result = ResultType.UnKnow;
                    }
                }



              rst.OrderNo = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "ORDER_NUMBER") == null ? string.Empty : inputPara["ORDER_NUMBER"];
                rst.SuccessTime = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "TRAN_TIME") == null ? string.Empty : inputPara["TRAN_TIME"];

                if (rst.Result == ResultType.Success || rst.Result == ResultType.UnKnow)
                {
                    rst.TradeAccount = new TradeAccountDetail();
                    rst.TradeAccount.AccNo = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "ACC_NO") == null ? string.Empty : inputPara["ACC_NO"];
                    rst.TradeAccount.AccDbName = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "ACC_NAME") == null ? string.Empty : inputPara["ACC_NAME"];
                    rst.TradeAccount.AccDBBank = inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "BRANCH_NAME") == null ? string.Empty : inputPara["BRANCH_NAME"];
                    double payment = 0;
                    double.TryParse(
                      inputPara.Keys.FirstOrDefault(p => p.ToUpper() == "AMOUNT") == null ? string.Empty : inputPara["AMOUNT"]
                       , out payment);
                    rst.TradeAccount.Amount = payment;//金额
                }
            }
            else//验证失败
            {
                LogTxt.WriteEntry("验证失败" + model.MessagePaket + model.Signature, "六盘水建行b2b支付");
            }
            return rst;
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
                    var keys = p.Split('=');
                    sArray.Add(keys[0], keys[1]);
                });
            return sArray;
        }


        ///// <summary>
        ///// 生成签名结果
        ///// </summary>
        ///// <param name="sArray">要签名的数组</param>
        ///// <param name="key">安全校验码</param>
        ///// <param name="sign_type">签名类型</param>
        ///// <param name="_input_charset">编码格式</param>
        ///// <returns>签名结果字符串</returns>
        //private string Build_mysign(Dictionary<string, string> dicArray, string key, string sign_type, string _input_charset)
        //{
        //    string prestr = Create_linkstring(dicArray);  //把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        //    //去掉最後一個&字符
        //    int nLen = prestr.Length;
        //    prestr = prestr.Substring(0, nLen - 1);
        //    prestr = prestr + key;                      //把拼接后的字符串再与安全校验码直接连接起来
        //    string mysign = Sign(prestr, sign_type, _input_charset);	//把最终的字符串签名，获得签名结果
        //    return mysign;
        //}

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

        #endregion
    }
}
