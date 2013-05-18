using CSP2P.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

// 聊天窗口类中与联网有关的部分内容

namespace CSP2P
{
    public partial class FormChat : Form
    {
        /// <summary>
        /// 封装的TCP Socket
        /// </summary>
        public P2PChatClient client;

        /// <summary>
        /// 发送富文本到对方，如果知道对方地址且没有连接则连接
        /// </summary>
        /// <param name="rtfToSend">要发送的rtf格式文本</param>
        private void sendRtfText(string rtfToSend)
        {
            buttonSend.Enabled = false;
            if (client == null)
            {
                createSocket(rtfToSend, true);
                return;     // 先等连接完成或连接失败
            }
            // 可以发送
            client.SendRtfText(rtfToSend);
            buttonSend.Enabled = true;
        }

        /// <summary>
        /// 设置与对方的P2PChatClient，并将P2PChatClient的owner设为自己。
        /// 由于对方连接到自己而调用。
        /// </summary>
        /// <param name="newSocketHandler"></param>
        public void SetHandlerForSocket(P2PChatClient newSocketHandler)
        {
            client = newSocketHandler;
            client.SetOwner(this);
        }

        // 以下内容用于建立P2P连接

        /// <summary>
        /// 发起方请求Socket连接
        /// </summary>
        /// <param name="UserToken">要发送的RTF文本/要发送的文件</param>
        /// <param name="isRtf">决定要发送的是什么</param>
        private void createSocket(string UserToken, bool isRtf)
        {
            try
            {
                // 新建一个封装的P2P通信类
                Socket newSocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                client = new P2PChatClient(owner, this, newSocket);
                if (isRtf)
                {
                    client.rtfToSend = UserToken;
                }
                else
                {
                    client.fileToSend = UserToken;
                }
                // 准备连接
                SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
                saEA.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(
                        client.createSocketCompletedEventHandler);
                saEA.RemoteEndPoint = new IPEndPoint(
                    owner.remoteIPaddress[targetName].IpAddress,
                    owner.PortTCPSend);
                // 连接
                client.socket.ConnectAsync(saEA);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：createSocket");
                Trace.WriteLine(ex.Message);
            }
        }
        // 剩余内容参见P2PChatClient_Create.cs FormMain_Network_TCP.cs
    }
}
