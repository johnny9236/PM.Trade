using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace PM.Utils.SocektUtils
{
    /// <summary>
    /// 同步socket 
    /// 如果接受后有业务处理  继承并重写CallBackBiz
    /// </summary>
    public class SynSocketServer
    {
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        ///   挂起连接队列的最大长度(一般50就可以)
        /// </summary>
        public int BlockCount { get; private set; }
        /// <summary>
        /// 编码
        /// </summary>
        public Encoding CustomEncoding { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="port">监听端口</param>
        /// <param name="encoding">编码</param>
        /// <param name="blockCount"> 挂起连接队列的最大长度 一般初始化值给50</param>
        public SynSocketServer(int port, Encoding encoding, int blockCount)
        {
            Port = port;
        }
        /// <summary>
        /// 获取网络标示
        /// </summary>
        public EndPoint LocalEndPoint
        {
            get
            {
                if (null != TCPListen)
                    return TCPListen.LocalEndPoint;
                else
                    return null;
            }
        }
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="port"></param>
        public void Start()
        {
            if (Port == 0)
            {
                throw new Exception("端口为0!");
            }
            TCPListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint LocalPort = new IPEndPoint(IPAddress.Any, Port);
            TCPListen.Bind(LocalPort);
            TCPListen.Listen(BlockCount == 0 ? 50 : BlockCount);//默认50
            Thread AcceptThread = new Thread(new ThreadStart(AcceptWorkThread));
            AcceptThread.Start();
        }
        static Socket TCPListen = null;
        // 接收连接线程
        [STAThread]
        void AcceptWorkThread()
        {

            // 指示为后台线程
            Thread.CurrentThread.IsBackground = true;
            while (true)
            {
                // 为新建连接创建新的Socket实例
                Socket s_Accept = TCPListen.Accept();
                IPEndPoint remoteAddress = (IPEndPoint)s_Accept.RemoteEndPoint;
                Console.WriteLine(string.Format("接收到来自 {0} 的连接", remoteAddress));

                // 接收数据专用线程 
                Thread ReceiveThread = new Thread(new ParameterizedThreadStart(ReceiveWorkThread));
                ReceiveThread.Start(s_Accept);
                Thread.Sleep(100);
            }
        }
        /// <summary>
        /// 接受数据
        /// </summary>
        /// <param name="obj"></param>
        void ReceiveWorkThread(object obj)
        {
            if (null == CustomEncoding)
                CustomEncoding = Encoding.UTF8;
            string receiveStr = string.Empty;//全部对象
            Thread.CurrentThread.IsBackground = true;   // 标识后台线程
            Socket s_Receive = (Socket)obj;
            byte[] buffer = new byte[1024];     // 创建接收缓冲
            IPEndPoint remoteAddress = (IPEndPoint)s_Receive.RemoteEndPoint;
            try
            {
                while (s_Receive.Connected)
                {
                    if (s_Receive.Connected)
                    {
                        while (true)
                        {
                            int ReceiveCount = s_Receive.Receive(buffer);
                            if (ReceiveCount <= 0)
                                break;
                            string receive = CustomEncoding.GetString(buffer);
                            receiveStr += receive;
                            if (ReceiveCount < buffer.Length)
                            {
                                break;
                            }
                            // 返回接收成功数据  
                        }
                        Console.WriteLine(string.Format("{0}:{1}", remoteAddress, receiveStr.TrimEnd('\0')));
                        Thread.Sleep(100);
                        //返回信息 
                        var sendStr = CallBackBiz(receiveStr);
                        if (!string.IsNullOrWhiteSpace(sendStr))
                            s_Receive.Send(CustomEncoding.GetBytes(sendStr));//处理业务
                        s_Receive.Close();
                        break;
                    }
                    else
                    {
                        s_Receive.Close();
                        break;
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(string.Format("{0} 断开连接", remoteAddress));
                throw ex;
            }
        }

        #region
        /// <summary>
        /// 接受到数据业务数据处理后返回给客户端（自定义业务时需要重写）
        /// </summary>
        /// <param name="bizString"></param>
        /// <returns></returns>
        protected virtual string CallBackBiz(string bizString)
        {
            return string.Empty;
        }
        #endregion
    }
}
