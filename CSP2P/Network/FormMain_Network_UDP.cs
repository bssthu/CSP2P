using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// 本文件用于检索局域网内在线好友
// 使用UDP广播来实现，可以不手动设置对方IP

// UDP报文自定协议如下：
// 1) 广播自己的用户名，要求收到回复
// type = update, name = 发送方用户名, tcpport, udpport, ping = 随机序号
// 
// 2) 回复，告知自己的用户名
// type = ackaddr, name = 发送方用户名, tcpport, udpport, ping
// 
// 3) 
// type = logoff, name = 发送方用户名

namespace CSP2P
{
    public partial class FormMain
    {
        #region MemberVariables
        /// <summary>
        /// UDP监听的端口, 8004
        /// </summary>
        public int PortUDPListener = 8004;

        /// <summary>
        /// UDP发送的目标端口
        /// </summary>
        public int PortUDPSend = 8004;

        /// <summary>
        /// UDP广播监听PortUDPListener端口，检查局域网内在线用户
        /// </summary>
        private UdpClient udpListener;

        /// <summary>
        /// UDP广播发送，表明自己上线
        /// </summary>
        private Socket udpBroadcaster;

        /// <summary>
        /// UDP监听线程
        /// </summary>
        private Thread udpListenerThread;

        /// <summary>
        /// 最近的ping字符串
        /// </summary>
        private string pingString;

        /// <summary>
        /// 发出ping字符串的毫秒数
        /// </summary>
        private long pingMillis;

        /// <summary>
        /// UDP收到数据的处理程序的委托
        /// </summary>
        /// <param name="udpString">收到的数据</param>
        /// <param name="sourceIPEndPoint">发送方地址</param
        private delegate void udpReceivedDelegate(
            string udpString, IPEndPoint sourceIPEndPoint);
        #endregion

        #region Methood
        /// <summary>
        /// 发送UDP广播给局域网内客户端表明自己刚上线或刷新在线好友
        /// </summary>
        /// <param name="ipAddress">要发送到的地址</param>
        protected void StartLoginUDPBroadcast(IPAddress ipAddress = null)
        {
            string protocalText = protocalHandler.Pack("update");
            protocalText = protocalHandler.Append(protocalText,
                "tcpport", PortTCPListener.ToString());
            protocalText = protocalHandler.Append(protocalText,
                "udpport", PortUDPListener.ToString());
            Random rnd = new Random((int)DateTime.Now.Ticks);
            pingString = rnd.Next(0x7FFF).ToString();
            protocalText = protocalHandler.Append(protocalText,
                "ping", pingString);
            pingMillis = DateTime.Now.Millisecond;
            if (ipAddress != null)
            {
                startUDPSend(protocalText, ipAddress);
            }
            else
            {
                startUDPSend(protocalText);
            }
        }

        /// <summary>
        /// 发送UDP广播给局域网内某一客户端表明自己在线
        /// </summary>
        /// <param name="ipAddress">要发送到的地址</param>
        /// <param name="ping">之前收到的ping字符串</param>
        protected void StartAckUDPBroadcast(IPAddress ipAddress, string ping)
        {
            string protocalText = protocalHandler.Pack("ackaddr");
            protocalText = protocalHandler.Append(protocalText,
                "tcpport", PortTCPListener.ToString());
            protocalText = protocalHandler.Append(protocalText,
                "udpport", PortUDPListener.ToString());
            protocalText = protocalHandler.Append(protocalText,
                "ping", ping);
            startUDPSend(protocalText, ipAddress);
        }

        /// <summary>
        /// 发送UDP广播给局域网内客户端表明已经下线
        /// </summary>
        private void StartLogoffUDPBroadcast()
        {
            startUDPSend(protocalHandler.Pack("logoff"));
        }

        /// <summary>
        /// 开始UDP广播
        /// </summary>
        /// <param name="stringToSend">要发送的数据</param>
        private void startUDPSend(string stringToSend)
        {
            startUDPSend(stringToSend, IPAddress.Broadcast);
        }

        /// <summary>
        /// 开始UDP发送，到指定地址
        /// </summary>
        /// <param name="stringToSend">要发送的数据</param>
        /// <param name="ipAddress">要发送到的地址</param>
        private void startUDPSend(string stringToSend, IPAddress ipAddress)
        {
            if (udpBroadcaster == null)
            {
                udpBroadcaster = new Socket(AddressFamily.InterNetwork,
                    SocketType.Dgram, ProtocolType.Udp);
                udpBroadcaster.SetSocketOption(
                    SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            }
            byte[] sendbuf = Encoding.ASCII.GetBytes(stringToSend);
            SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
            saEA.Completed +=
                new EventHandler<SocketAsyncEventArgs>(udpBroadcastEventHandler);
            saEA.SetBuffer(sendbuf, 0, sendbuf.Length);
            saEA.RemoteEndPoint =
                new IPEndPoint(ipAddress, PortUDPSend);
            udpBroadcaster.SendToAsync(saEA);
        }

        /// <summary>
        /// UDP广播发送成功的事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void udpBroadcastEventHandler(object sender, EventArgs ea)
        {
            SocketAsyncEventArgs socketAsyncEA = (SocketAsyncEventArgs)ea;
        }

        /// <summary>
        /// 开始UDP监听，监听PortUDPListener端口
        /// </summary>
        protected void StartUDPListen()
        {
            udpListener = new UdpClient(PortUDPListener);
            try
            {
                if (udpListenerThread == null ||
                    udpListenerThread.ThreadState != ThreadState.Running)
                {

                    udpListenerThread = new Thread(new ThreadStart(udpListen));
                }
                else
                {
                    udpListenerThread.Abort();
                }
            }
            catch { }
            udpListenerThread.Start();
        }

        /// <summary>
        /// UDP监听线程，
        /// 监听时会阻塞线程
        /// </summary>
        private void udpListen()
        {
            while (true)
            {
                try
                {
                    IPEndPoint remoteEndPoint =
                        new IPEndPoint(IPAddress.Any, 0);
                    byte[] rcvbuf = udpListener.Receive(ref remoteEndPoint);
                    this.BeginInvoke(new udpReceivedDelegate(udpReceived),
                        new object[]
                        {
                            Encoding.ASCII.GetString(rcvbuf),
                            remoteEndPoint
                        });
                }
                catch { }
                finally
                {
                    Thread.Sleep(1);
                }
            }
        }

        /// <summary>
        /// 收到UDP数据的处理程序
        /// </summary>
        /// <param name="udpString">收到的数据</param>
        /// <param name="sourceIPEndPoint">发送方地址</param>
        private void udpReceived(string udpString, IPEndPoint sourceIPEndPoint)
        {
            try
            {
                if (!protocalHandler.SetXmlText(udpString))
                {
                    throw new MyProtocalException(
                        "UDP收到的不是有效的协议文本，找不到CSP2P标记");
                }
                // 数据包类型
                string type = protocalHandler.GetElementTextByTag("type");
                if (type == null)
                {
                    throw new MyProtocalException(
                        "UDP收到的不是有效的协议文本，找不到type标记");
                }
                // 对方用户名
                string targetName =
                    protocalHandler.GetElementTextByTag("name");  // base64
                if (targetName == null)
                {
                    throw new MyProtocalException(
                        "UDP收到的不是有效的协议文本，找不到name标记");
                }
                // 解码
                targetName =
                    protocalHandler.Base64stringToString(targetName);
                int tcpport;
                int udpport;
                string port;
                string ping;
                switch (type)
                {
                    case "update":
                        // TCP监听端口
                        port = protocalHandler.GetElementTextByTag("tcpport");
                        if (port == null)
                        {
                            throw new MyProtocalException(
                                "UDP收到的不是有效的协议文本，找不到tcpport标记");
                        }
                        tcpport = Convert.ToInt16(port);
                        // UDP监听端口
                        port = protocalHandler.GetElementTextByTag("udpport");
                        if (port == null)
                        {
                            throw new MyProtocalException(
                                "UDP收到的不是有效的协议文本，找不到udpport标记");
                        }
                        udpport = Convert.ToInt16(port);
                        // Ping字符串
                        ping = protocalHandler.GetElementTextByTag("ping");
                        if (port == null)
                        {
                            throw new MyProtocalException(
                                "UDP收到的不是有效的协议文本，找不到ping标记");
                        }
                        // 刷新
                        OnLoginRefresh(targetName);
                        if (remoteIPaddress.ContainsKey(targetName))
                        {
                            remoteIPaddress[targetName].Update(
                                sourceIPEndPoint.Address, tcpport, udpport);
                        }
                        else
                        {
                            remoteIPaddress.Add(targetName,
                                new IPInfo(
                                    sourceIPEndPoint.Address, tcpport, udpport));
                        }
                        StartAckUDPBroadcast(sourceIPEndPoint.Address, ping);
                        break;
                    case "ackaddr":
                        // TCP监听端口
                        port = protocalHandler.GetElementTextByTag("tcpport");
                        if (port == null)
                        {
                            throw new MyProtocalException(
                                "UDP收到的不是有效的协议文本，找不到tcpport标记");
                        }
                        tcpport = Convert.ToInt16(port);
                        // UDP监听端口
                        port = protocalHandler.GetElementTextByTag("udpport");
                        if (port == null)
                        {
                            throw new MyProtocalException(
                                "UDP收到的不是有效的协议文本，找不到udpport标记");
                        }
                        udpport = Convert.ToInt16(port);
                        // Ping字符串
                        ping = protocalHandler.GetElementTextByTag("ping");
                        if (port == null)
                        {
                            throw new MyProtocalException(
                                "UDP收到的不是有效的协议文本，找不到ping标记");
                        }
                        // 刷新
                        if (ping == pingString)
                        {
                            loginRefreshWithPingDelegate(
                                targetName, (DateTime.Now.Millisecond - pingMillis).ToString());
                        }
                        else
                        {
                            loginRefreshDelegate(targetName);
                        }
                        if (remoteIPaddress.ContainsKey(targetName))
                        {
                            remoteIPaddress[targetName].Update(
                                sourceIPEndPoint.Address, tcpport, udpport);
                        }
                        else
                        {
                            remoteIPaddress.Add(targetName,
                                new IPInfo(
                                    sourceIPEndPoint.Address, tcpport, udpport));
                        }
                        break;
                    case "logoff":
                        OnLogoffRefresh(targetName);
                        remoteIPaddress.Remove(targetName);
                        break;
                    default:
                        throw new MyProtocalException(
                            "UDP收到的不是有效的协议文本，type的含义未知");
                }
            }
            catch { }
        }
        #endregion
    }
}
