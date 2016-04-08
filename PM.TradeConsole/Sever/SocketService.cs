using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PM.Utils.SocektUtils.AsySocket;
using PM.Utils;
using PM.Utils.Log;
using PM.Utils.SocektUtils;

namespace PM.TradeConsole.Sever
{
    /// <summary>
    /// socket监听
    /// </summary>
    public class SocketService
    {
        static CServerSocket cs = null;//服务端对象
        /// <summary>
        /// 启动socket服务
        /// </summary>
        public static void SocketServiceStart()
        {
            Console.WriteLine("--------------启动Socket------------------");
            //启动socket监听
            ////同步模式 需重写 CallBackBiz
            //SynSocketServer sv = new SynSocketServer(ConfigHelper.GetConfigInt("Port"), Encoding.UTF8, 50);
            //sv.Start();
            //异步模式
            cs = new CServerSocket(ConfigHelper.GetConfigInt("Port"));
            cs.OnConnect += new CServerSocket.ConnectionDelegate(cs_OnConnect);
            cs.OnDisconnect += new CServerSocket.ConnectionDelegate(cs_OnDisconnect);
            cs.OnError += new CServerSocket.ErrorDelegate(cs_OnError);
            cs.OnListen += new CServerSocket.ListenDelegate(cs_OnListen);
            cs.OnRead += new CServerSocket.ConnectionDelegate(cs_OnRead);
            cs.OnWrite += new CServerSocket.ConnectionDelegate(cs_OnWrite);
            if (!cs.Active())
            {
                Console.WriteLine("--------------激活Socket------------------");
                Console.WriteLine(string.Format("开始监听 <{0}>", cs.Port.ToString()));
            }
        }
        #region  socket事件
        static void cs_OnRead(System.Net.Sockets.Socket soc)
        {
            var receivedText = cs.ReceivedText;
            int socketIndex = cs.IndexOf(soc);
            PM.Utils.Log.LogTxt.WriteEntry("Read:" + soc.LocalEndPoint.AddressFamily.ToString() + " ::: " + receivedText, "socketRead");
            Console.WriteLine("Read:: " + receivedText);
            if (cs.Connected(socketIndex))
            {
                //  调用业务接口
                PM.PaymentContracts.IPaymentService sv = new PM.PaymentServices.PaymentService();
                Console.WriteLine("接收:" + receivedText);
                var responseModel = new PM.PaymentModel.PayResopnseModel();
                responseModel.Message = receivedText; 
                var sendStr = sv.PayCallback(responseModel);
                cs.SendText(sendStr, socketIndex);
                Console.WriteLine("send：" + sendStr);
                LogTxt.WriteEntry("send：" + soc.LocalEndPoint.AddressFamily.ToString() + receivedText, "socketSend");
            }
        }

        static void cs_OnWrite(System.Net.Sockets.Socket soc)
        {
            Console.WriteLine("Write:" + cs.IndexOf(soc).ToString() + " ::: " + cs.WriteText);
           // cs.SendText("ttttt", cs.IndexOf(soc));
        }
        static void cs_OnListen()
        {
            Console.WriteLine("Start  Listen:" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
        }
        static void cs_OnError(string ErroMessage, System.Net.Sockets.Socket soc, int ErroCode)
        {
            if (soc != null)
            {
                Console.WriteLine("err:" + "ErrorSocket: " + ErroMessage + " ::: " + ErroCode.ToString(), "socket");
                LogTxt.WriteEntry("err:" + "ErrorSocket: " + ErroMessage + " ::: " + ErroCode.ToString(), "socket");
            }
            else
            {
                Console.WriteLine("err:" + "ErrorSocket: " + ErroMessage + " ::: " + ErroCode.ToString(), "socket");
                LogTxt.WriteEntry("err:" + "ErrorSocket: " + ErroMessage + " ::: " + ErroCode.ToString(), "socket");
            }
        }
        static void cs_OnDisconnect(System.Net.Sockets.Socket soc)
        {
            string indice = cs.IndexOf(soc).ToString();
            Console.WriteLine("disconnect:" + indice, "socket");
        }
        static void cs_OnConnect(System.Net.Sockets.Socket soc)
        {
            string indice = cs.IndexOf(soc).ToString();
            Console.WriteLine("connect:" + indice + DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
        }
        #endregion
        /// <summary>
        /// 停止监听
        /// </summary>
        public static void SocketServiceStop()
        {
            if (null != cs)
            {
                try
                {
                    cs.Deactive();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }
}
