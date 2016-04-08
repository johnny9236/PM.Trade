using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PaymentProtocolModel.BankCommModel.JSABOC;
using PM.PaymentProtocolModel;
using PM.Utils.SocektUtils;
using PM.Utils.Log;
using PM.Utils;

namespace PM.JSABOC
{
    /// <summary>
    /// 查询交易明细
    /// </summary>
    public partial class JSABOCCommonProtocols
    {
        /// <summary>
        /// 按日期查交易明细
        /// </summary>
        /// <param name="query">交易查询对象</param>
        /// <param name="cfgInfo">配置对象</param>
        /// <returns></returns>
        private List<JSABOCRtnModel> GetQueryList(JSABOCQueryAccountDtl query, CfgInfo cfgInfo)
        {
            var rtnList = new List<JSABOCRtnModel>();//返回对象列表
            var sendptlModel = new JSABOCRtnModel();//发送报文对象
            JSABOCRtnModel temp_ptlModel = null;//临时报文协议
            sendptlModel.TradeCode = "ZTB1";
            //sendptlModel.TradeStructNum = "001";
            sendptlModel.TradeStructNum = query.TradeStructNum;//机构号
            sendptlModel.DetailDataTime = query.DetailDataTime;
            
            int port = 0;
            int.TryParse(cfgInfo.Port, out port);
            var receiveStr = SocketClient.SendToServ(cfgInfo.IP, port, sendptlModel.GetSendStr(), Encoding.GetEncoding("GB2312"));
            if (!string.IsNullOrEmpty(receiveStr))
            {
                //var fstLen = receiveStr.Substring(0, 7);//返回头长度
                //var fst = receiveStr.Substring(7, PM.Utils.StringHelper.TryToInt(fstLen));
                //return null;

                //获取返回头信息（不含明细）
                //var         fst = receiveStr.Length > 15 ? receiveStr.IndexOf("ZTB1|001|", 15) : 0; 
                //if (fst - 7 > 0)
                //{
                //    var summary = receiveStr.Substring(0, fst - 7);//取返回头字符串
                //    temp_ptlModel = new JSABOCRtnModel();
                //    if (temp_ptlModel.GetModel(summary))//获取返回头
                //    {
                //        if (null != temp_ptlModel && temp_ptlModel.ReturneCode == "0000" && temp_ptlModel.Count > 0)//有记录
                //        {
                //            LogTxt.WriteEntry("获取头信息,明细条数为" + temp_ptlModel.Count.ToString(), "嘉善农行查询");
                //            rtnList = GetQueryList(receiveStr.Substring(summary.Length));
                //        }
                //        else
                //        {
                //            LogTxt.WriteEntry(temp_ptlModel == null ? "转报文对象失败" : temp_ptlModel.ReturneMsg ?? "无返回信息", "嘉善农行查询");
                //        }
                //    }
                //}
                //else
                //{
                //    LogTxt.WriteEntry("服务端获取信息(提示-未处理成功):" + receiveStr, "嘉善农行查询");
                //}
                var fstLen = receiveStr.Substring(0, 7);//返回头长度
                var fst = StringHelper.Get_SubstringChineseStr(receiveStr, 7, PM.Utils.StringHelper.TryToInt(fstLen));
                if (!string.IsNullOrEmpty(fst))
                {
                    temp_ptlModel = new JSABOCRtnModel();
                    if (temp_ptlModel.GetModel(fst))//获取返回头
                    {
                        if (null != temp_ptlModel && temp_ptlModel.ReturneCode == "0000" && temp_ptlModel.Count > 0)//有记录
                        {
                            LogTxt.WriteEntry("获取头信息,明细条数为" + temp_ptlModel.Count.ToString(), "嘉善农行查询");
                            rtnList = GetQueryList(temp_ptlModel.Count, receiveStr.Substring(fst.Length + 7));
                        }
                        else
                        {
                            LogTxt.WriteEntry(temp_ptlModel == null ? "转报文对象失败" : temp_ptlModel.ReturneMsg ?? "无返回信息", "嘉善农行查询");
                        }
                    }
                }
            }
            else
            {
                LogTxt.WriteEntry(temp_ptlModel == null ? "服务端未获取信息" : temp_ptlModel.ReturneMsg ?? "无返回信息", "嘉善农行查询");
            }
            return rtnList;
        }

        #region  获取交易明细对象

        /// <summary>
        /// 获取报文对象
        /// </summary>
        /// <param name="protolStr">报文字符串（多条明细）</param>
        /// <param name="count">条数</param> 
        /// <returns></returns>
        private List<JSABOCRtnModel> GetQueryList(int count, string protolStr)
        {
            var rtnList = new List<JSABOCRtnModel>();//返回对象列表
            JSABOCRtnModel rtnModel = null;
            string protolString = string.Empty;
            try
            {

                #region  按长度获取
                int chineseCount = 0;
                int strCount = 0;
                for (int i = 0; i < count; i++)
                {
                    var Len = protolStr.Substring(strCount, 7);//返回头长度
                    var modelStr = StringHelper.Get_SubstringChineseStr(protolStr, chineseCount + 7, PM.Utils.StringHelper.TryToInt(Len));
                    chineseCount += StringHelper.Text_Length(modelStr) + 7;
                    strCount += modelStr.Length + 7;
                    if (!string.IsNullOrEmpty(modelStr))
                    {
                        rtnModel = GetModel(modelStr);
                        if (null != rtnModel)
                        {
                            rtnList.Add(rtnModel);
                        }
                    }
                }

                #endregion

                #region   substring方法
                //int postionCount = 0;
                //var postion = protolStr.IndexOf("ZTB1|001", 15);//第一个位置
                //protolString = protolStr.Substring(0, postion - 7);

                //while (!string.IsNullOrEmpty(protolString))
                //{
                //    postionCount += protolString.Length;
                //    postion = 0;
                //    rtnModel = GetModel(protolString);
                //    if (null != rtnModel)
                //    {
                //        rtnList.Add(rtnModel);
                //    }
                //    if (postionCount == protolStr.Length)
                //    {
                //        protolString = string.Empty;
                //        break;
                //    }
                //    postion = protolStr.IndexOf("ZTB1|001", postionCount + 15);
                //    if (postion == -1)
                //    {
                //        if (protolStr.Length > postionCount)//最后一部分
                //        {
                //            protolString = protolStr.Substring(postionCount);
                //        }
                //    }
                //    else
                //    {
                //        protolString = protolStr.Substring(postionCount, postion - postionCount - 7);
                //    }

                //}
                #endregion
                #region   split 方法
                //var protols = protolStr.Substring(0, protolStr.Length - 1).Split('|');
                //if (protols.Length % 21 == 0)
                //{
                //    var count = protols.Length / 21;
                //    for (int i = 0; i < count; i++)
                //    {
                //        var pString = string.Join("|", protols[0 + i * 21], protols[i * 21 + 1], protols[i * 21 + 2], protols[i * 21 + 3], protols[i * 21 + 4], protols[i * 21 + 5], protols[i * 21 + 6], protols[i * 21 + 7],
                //             protols[i * 21 + 8], protols[i * 21 + 9], protols[i * 21 + 10], protols[i * 21 + 11], protols[i * 21 + 12],
                //             protols[i * 21 + 13], protols[i * 21 + 14], protols[i * 21 + 15], protols[i * 21 + 16], protols[i * 21 + 17],
                //             protols[i * 21 + 18], protols[i * 21 + 19], protols[i * 21 + 20]);
                //        rtnModel = GetModel(pString);
                //        if (null != rtnModel)
                //        {
                //            rtnList.Add(rtnModel);
                //        }
                //    }
                //}
                //else
                //{
                //    LogTxt.WriteEntry("返回明细不符合规则", "嘉善农行查询");
                //}
                #endregion
            }
            catch (Exception ex)
            {
                LogTxt.WriteEntry("解析对象失败：" + ex.Message, "嘉善农行查询");
            }
            return rtnList;

        }
        /// <summary>
        /// 根据报文转报文对象
        /// </summary>
        /// <param name="modelStr">报文字符串</param>
        /// <returns></returns>
        private JSABOCRtnModel GetModel(string modelStr)
        {
            var model = new JSABOCRtnModel();
            if (model.GetModel(modelStr))//字符串转对象
            {
                //if (string.IsNullOrEmpty(model.PayAccNo))
                //    return null;
            }
            return model;
        }
        #endregion
    }
}
