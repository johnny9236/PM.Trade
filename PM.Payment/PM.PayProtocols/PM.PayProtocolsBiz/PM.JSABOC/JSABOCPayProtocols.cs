using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.JSABOC;
using PM.PaymentProtocolModel;
using PM.Utils.SocektUtils;
using PM.Utils.Log;
using PM.PaymentProtocolModel.BankCommModel;

namespace PM.JSABOC
{
    /// <summary>
    /// 支付协议
    /// </summary>
    public partial class JSABOCProtocols
    {
        /// <summary>
        /// 退款发起
        /// </summary>
        /// <param name="refoundModel">退款对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private bool SendRefound(JSABOCRefoundModel refoundModel, CfgInfo cfgInfo)
        {
            bool result = false;
            var model = new JSABOCRtnModel();
            var rtnModel = new JSABOCRtnModel();
            model.TradeCode = "ZTB2";
            model.TradeStructNum = "001";
            model.SectionNo = refoundModel.SectionNo;
            model.OrderNo = refoundModel.OrderNo;
            model.ReceiveAccDbName = refoundModel.ReceiveAccDbName;
            model.ReceiveAccNo = refoundModel.ReceiveAccNo;
            model.ReceiveAccDBBank = refoundModel.ReceiveAccDBBank;
            model.RealAmount = refoundModel.RealAmount;
            model.DetailDataTime = refoundModel.TradeDate;
            model.Amount = refoundModel.Amount;
            model.Summary = refoundModel.ABOCRemark;
            model.IsAboc = GetIsAboc(model.ReceiveAccDBBank);//是否农行
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            LogTxt.WriteEntry("发送报文" + model.GetSendStr(), "嘉善农行退保证金发起");
            var receiveStr = SocketClient.SendToServ(cfgInfo.IP, port, model.GetSendStr(), Encoding.GetEncoding("GB2312"));
            LogTxt.WriteEntry("接受报文" + model.GetSendStr(), "嘉善农行退保证金发起");
            if (!string.IsNullOrEmpty(receiveStr))
            {
                if (rtnModel.GetModel(receiveStr.Substring(7)))
                {
                    if (rtnModel.TradeCode.ToLower() == "ZTB2".ToLower() && rtnModel.ReturneCode == "0000")//成功
                    {
                        result = true;
                    }
                    else
                    {
                        LogTxt.WriteEntry(rtnModel == null ? "转报文对象失败" : rtnModel.ReturneMsg ?? "无返回信息", "嘉善农行退保证金发起");
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 保证金 响应
        /// </summary>
        /// <param name="refoundStr">报文内容</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private JSABOCBizModel GetRefound(PubCallBackModel callBackModel, CfgInfo cfgInfo)
        {
            bool success = false;//成功状态
            var abocModel = new JSABOCBizModel();
            abocModel.RtnModels = new List<JSABOCRtnModel>();
            var rtnModel = new JSABOCRtnModel();
            var temp_Model = new JSABOCRtnModel();
            temp_Model.TradeCode = "ZTB3";
            temp_Model.TradeStructNum = "001";
            temp_Model.ReturneCode = "9999";//失败
            temp_Model.ReturneMsg = "失败";
            LogTxt.WriteEntry("接受信息" + callBackModel.MessagePaket, "嘉善农行保证金响应");
            if (rtnModel.GetModel(callBackModel.MessagePaket.Substring(7)))//转交易对象
            {
                if (rtnModel.TradeCode.ToLower() == "ZTB3".ToLower())//成功获取
                {
                    if (rtnModel.ReturneCode == "0000")
                    {
                        temp_Model.ReturneCode = "0000";
                        temp_Model.ReturneMsg = "成功";                       
                    }
                    else
                    {
                        temp_Model.ReturneCode = "0000";
                        temp_Model.ReturneMsg = "处理失败记录成功";
                    }
                    success = true;
                }
                else
                {
                    LogTxt.WriteEntry(rtnModel == null ? "转报文对象失败" : rtnModel.ReturneMsg ?? "无返回信息", "嘉善农行保证金响应");
                }
            }
            if (success)
                abocModel.RtnModels.Add(rtnModel);//退款响应列表
            abocModel.RtnToProtol = temp_Model;//返回报文对象
            return abocModel;
        }

        #region private
        /// <summary>
        /// 是否农行判断
        /// </summary>
        /// <param name="bankName">开户行</param>
        /// <returns></returns>
        private string GetIsAboc(string bankName)
        {
            if (bankName.IndexOf("农行") > -1)
            {
                return "1";
            }
            else if (bankName.IndexOf("农业银行") > -1)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        #endregion
    }
}
