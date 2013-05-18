using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// 主窗体的Socket连接部分

namespace CSP2P
{
    public partial class FormMain
    {
        /// <summary>
        /// TCP监听的端口，8006
        /// </summary>
        public int PortTCPListener = 8006;

        /// <summary>
        /// TCP发送到的端口
        /// </summary>
        public int PortTCPSend = 8006;

        /// <summary>
        /// 记录好友的IP地址
        /// </summary>
        public Dictionary<string, IPInfo> remoteIPaddress =
            new Dictionary<string, IPInfo>();

        /// <summary>
        /// 自定义的通信报文处理类
        /// </summary>
        public ProtocalHandler protocalHandler = new ProtocalHandler();

        /// <summary>
        /// TCP监听，监听好友的连接，监听PortTCPListener端口
        /// </summary>
        private TcpListener listenerForFriends;

        /// <summary>
        /// TCP监听线程
        /// </summary>
        private Thread tcpListenerThread;

        /// <summary>
        /// 收到TCP连接的处理程序
        /// </summary>
        /// <param name="rcvSocket">本机作为服务端新建的Socket</param>
        private delegate void newTcpAcceptedDelegate(Socket rcvSocket);


        /// <summary>
        /// 监听好友的Socket连接的Socket，监听端口PortTCPListener
        /// </summary>
        protected void StartTCPListening()
        {
            try
            {
                listenerForFriends = new TcpListener(
                    IPAddress.Any, PortTCPListener);
                listenerForFriends.Start();
                if (tcpListenerThread == null ||
                    tcpListenerThread.ThreadState !=
                        System.Threading.ThreadState.Running)
                {

                    tcpListenerThread = new Thread(new ThreadStart(tcpListen));
                }
                else
                {
                    tcpListenerThread.Abort();
                    tcpListenerThread = new Thread(new ThreadStart(tcpListen));
                }
                tcpListenerThread.Start();
            }
            catch { }
        }

        /// <summary>
        /// TCP监听线程，
        /// 监听时会阻塞线程
        /// </summary>
        private void tcpListen()
        {
            while (true)
            {
                try
                {
                    Socket rcvSocket = listenerForFriends.AcceptSocket();
                    this.BeginInvoke(new newTcpAcceptedDelegate(newTcpCreated),
                        new object[] { rcvSocket });
                }
                catch { }
                finally
                {
                    Thread.Sleep(1);
                }
            }
        }

        /// <summary>
        /// 接受方
        /// 收到要求而新建了TCP连接，准备接收1次确认（对方（发起方）用户名）
        /// 收到1次确认后再判断类型
        /// owner is null
        /// </summary>
        /// <param name="rcvSocket">本机作为服务端新建的Socket</param>
        private void newTcpCreated(Socket rcvSocket)
        {
            try
            {
                SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
                // 接收1次确认
                saEA.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(
                        firstAckReceivedEventHandler);
                // 接收缓冲区
                byte[] rcvbuf =
                    new byte[P2PChatClient.BufferSize > P2PGroupClient.BufferSize ?
                        P2PChatClient.BufferSize : P2PGroupClient.BufferSize];
                saEA.SetBuffer(rcvbuf, 0, rcvbuf.Length);
                // 开始异步接收
                rcvSocket.ReceiveAsync(saEA);
            }
            catch
            {
                try
                {
                    rcvSocket.Close();
                }
                catch { }
            }
            finally { }
        }

        /// <summary>
        /// 接受方
        /// 收到1次确认，判断连接的类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void firstAckReceivedEventHandler(
            object sender, EventArgs ea)
        {
            Socket rcvSocket = sender as Socket;
            try
            {
                // 接收到的异步Socket事件参数
                SocketAsyncEventArgs socketAsyncEA = (SocketAsyncEventArgs)ea;
                // 提取接收到的文本
                string rcvString =
                    Encoding.ASCII.GetString(socketAsyncEA.Buffer);
                ProtocalHandler protocalHandler =
                    new ProtocalHandler(MyName);
                // 是否是协议
                if (!protocalHandler.SetXmlText(rcvString))
                {
                    throw new MyProtocalException(
                        "不是有效的协议文本，找不到CSP2P标签");
                }
                // 数据包类型
                string type = protocalHandler.GetElementTextByTag("type");
                if (type == null)
                {
                    throw new MyProtocalException(
                        "不是有效的1次确认，找不到type标签");
                }
                if (type.Equals("closesocket"))     // 对方要求关闭Socket
                {
                    rcvSocket.Close();
                }
                // 对方用户名
                string targetNameBase64 =
                    protocalHandler.GetElementTextByTag("name");
                string targetName =
                    protocalHandler.Base64stringToString(targetNameBase64);
                if (targetName == null)
                {
                    throw new MyProtocalException(
                        "不是有效的1次确认，" +
                        "找不到name标签");
                }
                if (type.Equals("init_chat_request"))   // 私聊
                {
                    P2PChatClient socketHandler =
                        new P2PChatClient(this, rcvSocket);
                    socketHandler.newSocketChatNameReceived(protocalHandler);
                }
                else if (type.Equals("init_gp_request"))     // 群聊
                {
                    P2PGroupClient socketHandler =
                        new P2PGroupClient(this, rcvSocket);
                    socketHandler.newSocketGroupNamesReceived(protocalHandler);
                }
                else
                {
                    throw new MyProtocalException(
                        "不是有效的1次确认，" +
                        "type不为init_chat_request或init_gp_request");
                }
                // 记录对方地址
                if (remoteIPaddress.ContainsKey(targetName))
                {
                    remoteIPaddress[targetName] =
                        new IPInfo((IPEndPoint)rcvSocket.RemoteEndPoint);
                }
                else
                {
                    remoteIPaddress.Add(targetName,
                        new IPInfo((IPEndPoint)rcvSocket.RemoteEndPoint));
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：" +
                    "firstAckReceivedEventHandler");
                Trace.WriteLine(ex.Message);
                // 直接关闭socket
                try
                {
                    rcvSocket.Close();
                }
                catch (Exception ex2)
                {
                    Trace.WriteLine("异常位置：" +
                        "firstAckReceivedEventHandler出错关闭rcvSocket时");
                    Trace.WriteLine(ex2.Message);
                }
            }
        }

        // 以下内容参见P2PChatClient_Create.cs FormChat_Network.cs
    }
}
