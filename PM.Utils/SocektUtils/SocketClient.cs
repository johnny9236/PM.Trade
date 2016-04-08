using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PM.Utils.SocektUtils
{
    public class SocketClient
    {
        /// <summary>
        /// 缓冲区
        /// </summary>
        private static byte[] result = new byte[102400];
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="iP"></param>
        /// <param name="port"></param>
        /// <param name="sendMessage"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string SendToServ(string iP, int port, string sendMessage, Encoding encoding)
        {
            string rtnStr = string.Empty;
            //设定服务器IP地址  
            IPAddress ip = IPAddress.Parse(iP);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, port)); //配置服务器IP与端口  
                //Console.WriteLine("连接服务器成功");
            }
            catch (SocketException ex)
            {
                CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, "连接服务器失败", ex);
                throw ex;
            }
            //通过clientSocket接收数据  
            int receiveLength = 0;// clientSocket.Receive(result);
            //   Console.WriteLine("接收服务器消息：{0}", Encoding.ASCII.GetString(result, 0, receiveLength));
            try
            {
                Thread.Sleep(100);    //等待  
                //clientSocket.Send(Encoding.UTF8.GetBytes(sendMessage)); 

                //clientSocket.Send(Encoding.GetEncoding("GB2312").GetBytes(sendMessage));
                clientSocket.Send(encoding.GetBytes(sendMessage));
                //   Console.WriteLine("向服务器发送消息：{0}" + sendMessage);
                Thread.Sleep(1000);

                var temp_receStr = string.Empty;
                receiveLength = clientSocket.Receive(result);
                while (receiveLength > 0)
                {
                    temp_receStr = encoding.GetString(result, 0, receiveLength);
                    //rtnStr = Encoding.UTF8.GetString(result, 0, receiveLength);
                    //rtnStr = Encoding.GetEncoding("GB2312").GetString(result, 0, receiveLength);
                    rtnStr += temp_receStr;
                    receiveLength = clientSocket.Receive(result);
                }
                // rtnStr = encoding.GetString(result, 0, receiveLength);
            }
            catch (SocketException ex)
            {
                //  CLogMgr.G_Instance.WriteErrorLog(LogSeverity.error, "信息失败", ex);
                throw ex;
            }
            finally
            {
                if (null != clientSocket)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            return rtnStr;
        }
    }
}
