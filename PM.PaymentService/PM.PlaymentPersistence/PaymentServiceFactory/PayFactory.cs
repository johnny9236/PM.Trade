using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.PlaymentPersistence.ORM;
using PM.PlaymentPersistence.Payment.PersistenceBiz;
using PM.PaymentModel;
using PM.PaymentProtocolModel;
using PM.Utils;
using PM.PlaymentPersistence.Payment.PersistenceBiz.JSABOC;
using PM.PlaymentPersistence.Payment.PersistenceBiz.LPSBBC;
using PM.PlaymentPersistence.Payment.PersistenceBiz.AHQY;
using PM.PlaymentPersistence.Payment.PersistenceBiz.ALi;
using PM.PlaymentPersistence.Payment.PersistenceBiz.HuangSan;
using PM.PlaymentPersistence.Payment.PersistenceBiz.HYCOB;

namespace PM.PlaymentPersistence.PaymentServiceFactory
{
    /// <summary>
    /// 业务实现工厂
    /// </summary>
    public class PayFactory
    {
        /// <summary>
        /// 支付请求
        /// </summary>
        /// <param name="payModel">请求对象</param>
        /// <returns></returns>
        public static dynamic DoPay(dynamic payModel)
        {
            dynamic rtn = null;
            var area = ConfigHelper.GetConfigString("Area");//有的地方项目可以不传这个功能号 
            var model = (PayStartModel)payModel;
            var cfg = GetConfig(((PayStartModel)payModel).BusinessFunNo);
            var enter = new publicEntities();
            switch (area)
            {
                #region
                case "LPS"://六盘水
                    #region   //六盘水
                    if (null != cfg)
                    {
                        if (cfg.ProtocolsWay.ToLower() == ProtocolsWay.NetBank.ToString().ToLower())
                        {
                            NetBankPersistence nBPay = new NetBankPersistence(payModel, enter, cfg);
                            rtn = nBPay.PayRequest();
                        }
                        else if (cfg.ProtocolsWay.ToLower() == ProtocolsWay.LPSBBC.ToString().ToLower())//建行
                        {
                            //var businessKind = BusinessType.None;
                            //Enum.TryParse(cfg.BusinessKind, out businessKind);
                            //switch (businessKind)
                            //{
                            //case BusinessType.PayB2C:
                            LPSBBCPersistence lpsBBC = new LPSBBCPersistence(payModel, enter, cfg);
                            rtn = lpsBBC.PayRequest();
                            //    break;
                            //case BusinessType.Pay://b2b
                            //    break;
                            // }
                        }
                    }
                    break;
                    #endregion
                case "JSABOC"://嘉善直接执行
                    //todo
                    break;
                case "AHQY":
                    #region  安徽青阳
                    if (null != cfg)
                    {
                        if (cfg.ProtocolsWay.ToLower() == ProtocolsWay.NetBank.ToString().ToLower())
                        {
                            NetBankPersistence nBPay = new NetBankPersistence(payModel, enter, cfg);
                            rtn = nBPay.PayRequest();
                        }
                        else if (cfg.ProtocolsWay.ToLower() == ProtocolsWay.QYBBC.ToString().ToLower())//青阳建行
                        {
                            QYPersistence qy = new QYPersistence();
                            rtn = qy.DoRefundPay(payModel);
                        }
                    }
                    break;
                    #endregion
                case "HuangSan":
                    #region  黄山
                    if (null != cfg)
                    {
                        if (cfg.ProtocolsWay.ToLower() == ProtocolsWay.HSICBC.ToString().ToLower())//黄山工行
                        {
                            HuangSanTRCBPersistence huangSan = new HuangSanTRCBPersistence();
                            rtn = huangSan.DoRefundPay(payModel);
                        }
                        else if (cfg.ProtocolsWay.ToLower() == ProtocolsWay.QYBBC.ToString().ToLower())//黄山建行与青阳建行相同 直接采用青阳的配置
                        {
                            QYPersistence qy = new QYPersistence();
                            rtn = qy.DoRefundPay(payModel);
                        }
                    }
                    break;
                    #endregion
                case "ZMD"://驻马店
                    #region  驻马店
                    ALiPersistence ali = new ALiPersistence(payModel, enter, cfg);
                    rtn = ali.PayRequest();
                    break;
                    #endregion

                case "HaiYan":
                    #region  海盐
                    if (null != cfg)
                    {
                         if (cfg.ProtocolsWay.ToLower() == ProtocolsWay.BOC.ToString().ToLower())//海盐中行
                        {
                            HYBOCPersistence qy = new HYBOCPersistence();
                            rtn = qy.DoRefundPay(payModel);
                        }
                    }
                    break;
                    #endregion
                #endregion
            }
            return rtn;
        }

        /// <summary>
        /// 退款处理
        /// </summary>
        /// <param name="refundMode">退款对象</param>
        /// <param name="cfg">配置对象</param>
        /// <returns></returns>
        public static dynamic DoRefundPay(dynamic refundMode)
        {
            //dynamic rtn = null;
            var area = ConfigHelper.GetConfigString("Area");
            PayRefundModel rtn = null;
            switch (area)
            {
                #region
                case "JSABOC"://嘉善农行
                    #region
                    JSABOCPersistence biz = new JSABOCPersistence();
                    var rtnstring = biz.DoRefundPay(refundMode);
                    if (!string.IsNullOrEmpty(rtnstring))
                    {
                        rtn = new PayRefundModel();
                        rtn.Result = rtnstring;
                    }
                    break;
                    #endregion
                case "AHQY"://安徽青阳
                    #region
                    QYPersistence qyBiz = new QYPersistence();
                    rtn = qyBiz.DoRefundPay(refundMode);
                    break;
                    #endregion
                case "HaiYan"://海盐
                    #region
                    HYBOCPersistence hyBiz = new HYBOCPersistence();
                    rtn = hyBiz.DoRefundPay(refundMode);
                    break;
                    #endregion
                #endregion
            }
            return rtn;
        }
        /// <summary>
        /// 支付响应
        /// </summary>
        /// <param name="callBackModel">响应对象</param>
        /// <returns></returns>
        public static dynamic PayCallback(dynamic callBackModel)
        {
            dynamic rtn = null;
            var area = ConfigHelper.GetConfigString("Area");
            var model = (PayResopnseModel)callBackModel;
            var cfg = GetConfig(model.BusinessFunNo);
            var enter = new publicEntities();
            switch (area)
            {
                #region
                case "JSABOC"://嘉善农行
                    #region
                    JSABOCPersistence biz = new JSABOCPersistence();
                    rtn = biz.HttpPayCallback(((PayResopnseModel)callBackModel).Message);
                    break;
                    #endregion
                case "LPS"://六盘水  
                    #region
                    if (null != cfg)
                    {
                        if (cfg.ProtocolsWay.ToLower() == ProtocolsWay.NetBank.ToString().ToLower())
                        {
                            NetBankPersistence nBPay = new NetBankPersistence(null, enter, cfg);
                            rtn = nBPay.Response(model);
                        }
                        else if (cfg.ProtocolsWay.ToLower() == ProtocolsWay.LPSBBC.ToString().ToLower())//建行
                        {
                            LPSBBCPersistence lpsBBC = new LPSBBCPersistence(null, enter, cfg);
                            rtn = lpsBBC.Response(model);
                        }
                    }
                    break;
                    #endregion
                case "AHQY":
                    #region
                    if (null != cfg)
                    {
                        if (cfg.ProtocolsWay.ToLower() == ProtocolsWay.NetBank.ToString().ToLower())
                        {
                            NetBankPersistence nBPay = new NetBankPersistence(null, enter, cfg);
                            rtn = nBPay.Response(model);
                        }
                    }
                    break;
                    #endregion
                case "ZMD"://驻马店
                    #region
                    ALiPersistence ali = new ALiPersistence(null, enter, cfg);
                    rtn = ali.Response(model);
                    break;
                    #endregion
                #endregion
            }
            //string showInfo = string.Empty;
            //var PEOC = new publicEntities();
            //NetBankBiz nBPay = new NetBankBiz(null, PEOC, GetConfig());
            //nBPay.Response(signature, message, false, out showInfo);
            // JSABOCPersistence biz = new JSABOCPersistence();
            //var  showInfo = biz.HttpPayCallback(message);
            return rtn;
        }

        #region 私有方法
        /// <summary>
        /// 获取配置对象
        /// </summary>
        /// <returns></returns>
        private static SysConfigModel GetConfig()
        {
            SysConfigModel sysConfigModel = null;
            try
            {
                var cfgPath = string.Format(@"{0}\config\{1}", AppDomain.CurrentDomain.BaseDirectory, "CFG.xml");
                sysConfigModel = new SysConfigModel();
                sysConfigModel = sysConfigModel.xmlDeserialize(cfgPath);
            }
            catch (Exception ex)
            {
                CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, ex.Message + ex.Source, ex);
            }
            return sysConfigModel;
        }

        /// <summary>
        /// 获取具体配置对象
        /// </summary>
        /// <param name="bussNo">业务号</param>
        /// <returns></returns>
        private static CfgInfo GetConfig(string bussNo)
        {
            CfgInfo cfgInfo = null;
            try
            {
                var sysConfigModel = GetConfig();
                cfgInfo = sysConfigModel.CfgInfoList.Find(p => p.BusinessNo.Trim().ToLower() == bussNo.Trim().ToLower());
            }
            catch (Exception ex)
            {
                CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, ex.Message + ex.Source, ex);
            }
            return cfgInfo;
        }
        #endregion
    }
}
