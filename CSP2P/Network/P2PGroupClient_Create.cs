using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

// 群聊中建立Socket连接
// 假设群组中有n个好友，那么最多需要握手n*(n-1)/2次
// 按需建立连接。


// # 本文件中涉及的协议：
// 一、群聊建立Socket连接
// 1) 1次确认，由发起方发送
// type = init_gp_request, name = 发送方（发起方）用户名, target = 所有好友（多组）
// 其中name, target使用base64编码
// 2) 2次确认，由接受方发送
// type = init_gp_ok, name = 发送方（接受方）用户名, you = 对方（发起方）用户名
// 其中name, you使用base64编码


// # 建立Socket连接的流程：
// 背景：发起方在群聊窗口中发送数据，
// 发起方知道接受方的地址，但双方之间没有Socket连接。
// 
// Function P2PGroupClient::CreateSocketToSend()
// 发起方：建立Socket连接至接受方TCP监听端口
// Function P2PGroupClient::createSocketCompletedEventHandler()
// 发起方：（建立Socket完成）发送1次确认，type = init_chat_request
// Function P2PGroupClient::createdSocketSendNamesCompletedEventHandler()
// 发起方：准备接收接受方的2次确认
// Function FormMain::newTcpCreated()
// 接受方：（建立Socket完成）准备接收1次确认
// Function P2PClient::newSocketNameReceivedEventHandler()
// 接受方：收到1次确认（type = initrequest），记录对方的身份信息
// 接受方：发送2次确认，type = init_chat_ok
// Function P2PClient::newSocketUsernameSendEventHandler()
// 接受方：准备接收对方发送的正式数据
// Function P2PClient::createSocketNameReceivedEventHandler()
// 发起方：收到2次确认，核对身份信息
// END


// 相关内容参见FormChat_Network.cs FormMain_Network_TCP.cs


namespace CSP2P
{
    public partial class P2PGroupClient
    {
        /// <summary>
        /// 建立连接成功以后发送的RTF文本
        /// </summary>
        private string rtfToSend;

        /// <summary>
        /// 发起方：
        /// 群聊窗口中主动建立到某一个用户的连接，
        /// 然后发送RTF文本
        /// <param name="rtfToSend">RTF文本</param>
        /// </summary>
        public void CreateSocketToSend(string rtfToSend)
        {
            // 记录要发送的RTF文本
            this.rtfToSend = rtfToSend;
            try
            {
                // 新建Socket
                socket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                // 准备连接
                SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
                saEA.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(
                        createSocketCompletedEventHandler);
                saEA.RemoteEndPoint = new IPEndPoint(
                    main.remoteIPaddress[targetName].IpAddress,
                    main.PortTCPSend);
                // 连接
                socket.ConnectAsync(saEA);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：P2PGroupClient.CreateSocketToSend");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 发起方：
        /// 成功建立Socket连接，准备发送1次确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createSocketCompletedEventHandler(
            object sender, EventArgs ea)
        {
            // 取得新建的Socket（如果连接成功）
            // 此Socket应等于this.socket
            SocketAsyncEventArgs socketAsyncEA = (SocketAsyncEventArgs)ea;
            Socket createdSocket = socketAsyncEA.ConnectSocket;
            if (createdSocket != null && createdSocket == socket)  // 连接成功
            {
                // 1次确认，发送name=自己（发起方）用户名
                ProtocalHandler protocalHandler =
                    new ProtocalHandler(main.MyName);
                string protocalText = protocalHandler.Pack("init_gp_request");
                // 发送群中所有好友
                foreach (string friend in owner.friends)
                {
                    string friendBase64 =
                        protocalHandler.StringToBase64string(friend);
                    protocalText = protocalHandler.Append(
                        protocalText, "target", friendBase64);
                }
                byte[] sendbuf = Encoding.ASCII.GetBytes(protocalText);
                SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
                saEA.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(
                        createdSocketSendNamesCompletedEventHandler);
                saEA.SetBuffer(sendbuf, 0, sendbuf.Length);
                socket.SendAsync(saEA);
            }
        }

        /// <summary>
        /// 发起方：
        /// 1次确认发送完成
        /// 准备好接收2次确认（对方（接收方）用户名）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void createdSocketSendNamesCompletedEventHandler(
            object sender, EventArgs ea)
        {
            SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
            saEA.Completed +=
                new EventHandler<SocketAsyncEventArgs>(
                    createSocketNameReceivedEventHandler);
            saEA.SetBuffer(rcvbuf, 0, rcvbuf.Length);
            socket.ReceiveAsync(saEA);
        }

        /// <summary>
        /// 接受方：
        /// 收到新建的TCP的1次确认，记录对方、群中好友的身份信息
        /// 此时确定owner
        /// 发送2次确认
        /// </summary>
        /// <param name="protocalHandler">接收到的协议的封装</param>
        public void newSocketGroupNamesReceived(ProtocalHandler protocalHandler)
        {
            try
            {
                // 对方用户名
                string targetNameBase64 =
                    protocalHandler.GetElementTextByTag("name");
                string targetName =
                    protocalHandler.Base64stringToString(targetNameBase64);
                // 群内好友列表Base64
                string[] targetNames =
                    protocalHandler.GetElementsTextByTag("target");
                // 群内好友列表解码
                for (int i = 0; i < targetNames.Length; i++)
                {
                    targetNames[i] =
                        protocalHandler.Base64stringToString(targetNames[i]);
                }
                // 打开私聊窗口
                main.SetGroupFormClient(this, targetName, targetNames);
                // 2次确认，发送you = 对方（发起方）用户名
                string protocalText = protocalHandler.Pack("init_gp_ok");
                protocalText = protocalHandler.Append(
                    protocalText, "you", targetNameBase64);
                byte[] sendbuf = Encoding.ASCII.GetBytes(protocalText);
                SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
                saEA.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(
                        newSocketUsernameSendEventHandler);
                saEA.SetBuffer(sendbuf, 0, sendbuf.Length);
                socket.SendAsync(saEA);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：" +
                    "newSocketGroupNamesReceived");
                Trace.WriteLine(ex.Message);
                CloseSocket();
            }
        }

        /// <summary>
        /// 接受方：
        /// 2次确认发送完成，开始异步接收数据。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void newSocketUsernameSendEventHandler(
            object sender, EventArgs ea)
        {
            // 打开聊天窗口
            main.Invoke(main.showFormGroupDelegate, owner);
            // 开始接收
            beginReceive();
        }

        /// <summary>
        /// 发起方：
        /// 收到2次确认，核对对方的身份信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void createSocketNameReceivedEventHandler(
            object sender, EventArgs ea)
        {
            try
            {
                SocketAsyncEventArgs socketAsyncEA =
                    (SocketAsyncEventArgs)ea;
                // 获取文本
                string rcvString =
                    Encoding.ASCII.GetString(socketAsyncEA.Buffer);
                ProtocalHandler protocalHandler =
                    new ProtocalHandler(main.MyName);
                // 判断是否是协议
                if (!protocalHandler.SetXmlText(rcvString))
                {
                    throw new MyProtocalException(
                        "不是有效的协议文本，找不到CSP2P标签");
                }
                // 数据包类型
                string type = protocalHandler.GetElementTextByTag("type");
                if (type.Equals("closesocket"))     // 对方要求关闭Socket
                {
                    closeSocketWithoutSend();
                }
                if (!type.Equals("init_gp_ok"))
                {
                    throw new MyProtocalException(
                        "不是有效的2次确认，" +
                        "找不到type标签或type不为init_gp_ok");
                }
                // 对方用户名
                string key = protocalHandler.GetElementTextByTag("name");
                key = protocalHandler.Base64stringToString(key);
                if (!key.Equals(targetName))
                {
                    throw new MyProtocalException(
                        "不是有效的2次确认，" +
                        "找不到name标签或接受方用户名不符");
                }
                // 己方用户名
                string myName = protocalHandler.GetElementTextByTag("you");
                myName = protocalHandler.Base64stringToString(myName);
                if (!myName.Equals(main.MyName))
                {
                    throw new MyProtocalException(
                        "2次确认发现错误，找不到you标签" +
                    "或接受方返回的发送方用户名不符");
                }
                // 开始通信
                owner.BeginInvoke(owner.sendRTFDelegate, rtfToSend);
                rtfToSend = null;
                beginReceive();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：" +
                "createSocketNameReceivedEventHandler");
                Trace.WriteLine(ex.Message);
                CloseSocket();
            }
        }
    }
}
