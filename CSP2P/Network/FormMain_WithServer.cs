using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

// 主窗体的Socket连接部分
// 与服务器的连接都为停等方式，
// 使用异步收发是防止卡死

namespace CSP2P
{
    public partial class FormMain
    {
        /// <summary>
        /// 连接到中央服务器166.111.180.60:8000的Socket
        /// </summary>
        private Socket clientToServer;

        /// <summary>
        /// 已登陆到中央服务器
        /// </summary>
        public bool lol = false;

        /// <summary>
        /// 缓冲区字节数
        /// </summary>
        public const uint BufferSize = 4096;

        /// <summary>
        /// 正在检查好友的在线状态，
        /// 禁止其他与服务器的交互
        /// （下线例外）
        /// </summary>
        private bool checkingStatus = false;

        /// <summary>
        /// 需要检查的好友的用户名
        /// </summary>
        private string[] checkingUsers;

        /// <summary>
        /// 建立到中央服务器的Socket连接
        /// </summary>
        protected void ConnectToServer()
        {
            Socket client = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
            saEA.Completed +=
                new EventHandler<SocketAsyncEventArgs>(
                    connectToServerCompletedEventHandler);
            saEA.RemoteEndPoint = new IPEndPoint(new IPAddress(
                new byte[] { 166, 111, 180, 60 }), 8000);
            saEA.UserToken = client;
            client.ConnectAsync(saEA);
        }

        /// <summary>
        /// 建立到中央服务器的Socket连接完成的事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void connectToServerCompletedEventHandler(
            object sender, EventArgs ea)
        {
            SocketAsyncEventArgs socketAsyncEA = (SocketAsyncEventArgs)ea;
            clientToServer = socketAsyncEA.ConnectSocket;
            try
            {
                if (clientToServer == null)     // 没有连接成功
                {
                    string msg = String.Format("无法连接到中央服务器:\n{0}\n{1}",
                        socketAsyncEA.RemoteEndPoint.ToString(),
                        socketAsyncEA.SocketError.ToString());
                    MessageBox.Show(msg);
                    throw new Exception(msg);
                }
                TCPLogin();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：" +
                    "connectToServerCompletedEventHandler");
                Trace.WriteLine(ex.Message);
                // 并没能连接，无操作
            }
        }

        /// <summary>
        /// 账号登陆（上线），
        /// 向服务器发送用户名和密码信息
        /// </summary>
        protected void TCPLogin()
        {
            try
            {
                // 登陆，发送形如"2010011428_net2012"
                string loginText = String.Format("{0}_{1}",
                    textBoxUser.Text, textBoxPw.Text);
                byte[] sendbuf = Encoding.ASCII.GetBytes(loginText);
                SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
                saEA.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(
                        tcpLoginCompletedEventHandler);
                saEA.SetBuffer(sendbuf, 0, sendbuf.Length);
                clientToServer.SendAsync(saEA);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：" +
                    "TCPLogin");
                Trace.WriteLine(ex.Message);
                // 关闭连接
                CloseSocketToServer();
            }
        }

        /// <summary>
        /// 账号登陆信息发送完成的事件处理程序
        /// 准备接收服务器发送的确认信息"lol"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void tcpLoginCompletedEventHandler(object sender, EventArgs ea)
        {
            SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
            saEA.Completed +=
                new EventHandler<SocketAsyncEventArgs>(
                    receiveLOLEventHandler);
            byte[] rcvbuf = new byte[BufferSize];
            saEA.SetBuffer(rcvbuf, 0, rcvbuf.Length);
            clientToServer.ReceiveAsync(saEA);
        }

        /// <summary>
        /// 收到登陆时服务器的确认信息的事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void receiveLOLEventHandler(object sender, EventArgs ea)
        {
            try
            {
                // 提取接收到的文本
                SocketAsyncEventArgs socketAsyncEA = (SocketAsyncEventArgs)ea;
                string rcvString =
                    Encoding.ASCII.GetString(socketAsyncEA.Buffer);
                Trace.WriteLine("Server: " + rcvString);
                Trace.WriteLine("");
                if (rcvString == null || !(rcvString.ToLower()).StartsWith("lol"))
                {
                    throw new Exception("登陆失败！");
                }
                lol = true;
                // 从服务器检查好友在线状态
                this.BeginInvoke(checkStatusAllDelegate);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：" +
                    "receiveLOLEventHandler");
                Trace.WriteLine(ex.Message);
                // 关闭连接
                CloseSocketToServer();
            }
        }

        /// <summary>
        /// 登出（下线）
        /// </summary>
        protected void TCPLogout()
        {
            try
            {
                lol = false;
                if (clientToServer == null)
                {
                    return;
                }
                // 登出，发送形如"logout2010011428"
                string loginText = String.Format("logout{0}",
                    textBoxUser.Text);
                byte[] sendbuf = Encoding.ASCII.GetBytes(loginText);
                SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
                saEA.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(
                        tcpLogoutCompletedEventHandler);
                saEA.SetBuffer(sendbuf, 0, sendbuf.Length);
                clientToServer.SendAsync(saEA);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：" +
                    "TCPLogin");
                Trace.WriteLine(ex.Message);
                // 关闭连接
                CloseSocketToServer();
            }
        }

        /// <summary>
        /// 账号登出信息发送完成的事件处理程序
        /// 接收服务器发送的确认信息"loo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void tcpLogoutCompletedEventHandler(object sender, EventArgs ea)
        {
            SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
            saEA.Completed +=
                new EventHandler<SocketAsyncEventArgs>(
                    receiveLOOEventHandler);
            byte[] rcvbuf = new byte[BufferSize];
            saEA.SetBuffer(rcvbuf, 0, rcvbuf.Length);
            clientToServer.ReceiveAsync(saEA);
        }

        /// <summary>
        /// 收到下线时服务器的确认信息的事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void receiveLOOEventHandler(object sender, EventArgs ea)
        {
            try
            {
                // 提取接收到的文本
                SocketAsyncEventArgs socketAsyncEA = (SocketAsyncEventArgs)ea;
                string rcvString =
                    Encoding.ASCII.GetString(socketAsyncEA.Buffer);
                Trace.WriteLine("Server: " + rcvString);
                Trace.WriteLine("");
                if (rcvString == null || !(rcvString.ToLower()).StartsWith("loo"))
                {
                    throw new Exception("登出失败！");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：" +
                    "receiveLOOEventHandler");
                Trace.WriteLine(ex.Message);
                // 关闭连接
                CloseSocketToServer();
            }
        }

        /// <summary>
        /// 关闭与服务器的Socket连接
        /// </summary>
        protected void CloseSocketToServer()
        {
            MessageBox.Show("您已下线。");
            if (clientToServer == null)
            {
                return;
            }
            try
            {
                clientToServer.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：" +
                    "CloseSocketToServer");
                Trace.WriteLine(ex.Message);
            }
            finally
            {
                clientToServer = null;
            }
        }

        // 以下为登陆成功后的各种查询操作

        /// <summary>
        /// 检查列表中所有好友的在线状态
        /// </summary>
        protected void CheckStatusAll()
        {
            if (clientToServer == null)
            {
                return;
            }
            // 有Socket连接
            if (!lol)
            {
                return;
            }
            // 登陆到服务器
            if (checkingStatus)
            {
                return;
            }
            // 没有正在检查
            int count = listViewFriends.Items.Count;
            if (count <= 0)
            {
                return;
            }
            // 暂存好友名
            checkingUsers = new string[count];
            for (int i = 0; i < count; i++)
            {
                checkingUsers[i] = listViewFriends.Items[i].Text;
            }
            // 开始检查第一个好友
            checkStatus(count - 1);
        }

        /// <summary>
        /// 检查某好友是否在线
        /// </summary>
        /// <param name="index">下标</param>
        private void checkStatus(int index)
        {
            try
            {
                // 检查在线状态，发送形如"q2010011428"
                // 要发送的消息
                string checkMessage = String.Format("q{0}", checkingUsers[index]);
                Trace.WriteLine(checkMessage);
                byte[] sendbuf = Encoding.ASCII.GetBytes(checkMessage);
                SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
                saEA.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(
                        checkStatusSendCompleteEventHandler);
                saEA.SetBuffer(sendbuf, 0, sendbuf.Length);
                saEA.UserToken = (object)(index);
                clientToServer.SendAsync(saEA);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：" +
                    "checkStatus");
                Trace.WriteLine(ex.Message);
                // 关闭连接
                CloseSocketToServer();
            }
        }

        /// <summary>
        /// 检查某好友是否在线的查询要求发送完成的事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void checkStatusSendCompleteEventHandler(
            object sender, EventArgs ea)
        {
            try
            {
                SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
                saEA.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(
                        checkStatusReceiveEventHandler);
                byte[] rcvbuf = new byte[BufferSize];
                saEA.SetBuffer(rcvbuf, 0, rcvbuf.Length);
                SocketAsyncEventArgs saEAOld = (SocketAsyncEventArgs)ea;
                saEA.UserToken = saEAOld.UserToken;
                clientToServer.ReceiveAsync(saEA);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：" +
                    "checkStatusSendCompleteEventHandler");
                Trace.WriteLine(ex.Message);
                // 关闭连接
                CloseSocketToServer();
            }
        }

        /// <summary>
        /// 检查某好友是否在线收到服务器回复的事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void checkStatusReceiveEventHandler(
            object sender, EventArgs ea)
        {
            try
            {
                // 提取接收到的文本
                SocketAsyncEventArgs socketAsyncEA = (SocketAsyncEventArgs)ea;
                string rcvString =
                    Encoding.ASCII.GetString(socketAsyncEA.Buffer);
                Trace.WriteLine("Server: " + rcvString);
                Trace.WriteLine("");
                // 当前好友的序号
                SocketAsyncEventArgs saEAOld = (SocketAsyncEventArgs)ea;
                int index = (int)saEAOld.UserToken;
                // 记录在线好友IP
                if (rcvString != null)
                {
                    if (rcvString.Split('.').Length == 4)
                    {
                        try
                        {
                            // 获取IP地址
                            IPAddress ipAddress =
                                IPAddress.Parse(rcvString.Replace("\0", ""));
                            // 设置IP地址
                            this.BeginInvoke(loginRefreshDelegate, checkingUsers[index]);
                            if (remoteIPaddress.ContainsKey(checkingUsers[index]))
                            {
                                remoteIPaddress[checkingUsers[index]].Update(
                                    ipAddress, PortTCPSend, PortUDPSend);
                            }
                            else
                            {
                                remoteIPaddress.Add(checkingUsers[index],
                                    new IPInfo(
                                        ipAddress, PortTCPSend, PortUDPSend));
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine("异常位置：" +
                                "checkStatusReceiveEventHandler");
                            Trace.WriteLine("设置IP地址时出错");
                            Trace.WriteLine(ex.Message);
                        }
                    }
                }
                // 检查下一个好友直到结束
                if (index > 0)
                {
                    checkStatus(index - 1);
                }
                else
                {
                    checkingStatus = false;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：" +
                    "checkStatusReceiveEventHandler");
                Trace.WriteLine(ex.Message);
                // 关闭连接
                CloseSocketToServer();
            }
        }

        /// <summary>
        /// 检查指定好友的在线状态
        /// </summary>
        /// <param name="friends">要检查的指定的好友</param>
        protected void CheckStatusStrings(string[] friends)
        {
            if (clientToServer == null)
            {
                return;
            }
            // 有Socket连接
            if (!lol)
            {
                return;
            }
            // 登陆到服务器
            if (checkingStatus)
            {
                return;
            }
            // 没有正在检查
            int count = listViewFriends.Items.Count;
            if (count <= 0)
            {
                return;
            }
            // 暂存好友名
            checkingUsers = friends;
            // 开始检查第一个好友
            checkStatus(friends.Length  - 1);
        }
    }
}
