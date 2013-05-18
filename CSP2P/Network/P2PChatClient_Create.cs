using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

// # 本文件中涉及的协议：
// 一、建立Socket连接
// 1) 1次确认，由发起方发送
// type = init_chat_request, name = 发送方（发起方）用户名
// 其中name使用base64编码
// 2) 2次确认，由接受方发送
// type = init_chat_ok, name = 发送方（接受方）用户名, you = 对方（发起方）用户名
// 其中name, you使用base64编码


// # 建立Socket连接的流程：
// 背景：发起方打开了与接受方的聊天窗口并发送数据，
// 发起方知道接受方的地址，但双方之间没有Socket连接。
// 
// Function FormChat::createSocket()
// 发起方：建立Socket连接至接受方TCP监听端口
// Function P2PChatClient::createSocketCompletedEventHandler()
// 发起方：（建立Socket完成）发送1次确认，type = init_chat_request
// Function P2PChatClient::createdSocketSendNameCompletedEventHandler()
// 发起方：准备接收接受方的2次确认
// Function FormMain::newTcpCreated()
// 接受方：（建立Socket完成）准备接收1次确认
// Function FormMain::firstAckReceivedEventHandler()
// 接受方：收到1次确认，判断类型
// Function P2PChatClient::newSocketChatNameReceivedEventHandler()
// 接受方：1次确认type = init_chat_request，记录对方的身份信息
// 接受方：发送2次确认，type = init_chat_ok
// Function P2PChatClient::newSocketUsernameSendEventHandler()
// 接受方：准备接收对方发送的正式数据
// Function P2PChatClient::createSocketNameReceivedEventHandler()
// 发起方：收到2次确认，核对身份信息
// END


// 相关内容参见FormChat_Network.cs FormMain_Network_TCP.cs


namespace CSP2P
{
    public partial class P2PChatClient
    {
        /// <summary>
        /// 缓存的RTF文本
        /// 仅在创建时使用
        /// </summary>
        public string rtfToSend = null;

        /// <summary>
        /// 缓存的要发送的文件名（含路径）
        /// 仅在创建时使用
        /// </summary>
        public string fileToSend = null;


        /// <summary>
        /// 发起方：
        /// Socket连接完成的事件处理程序
        /// 发送1次确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        public void createSocketCompletedEventHandler(
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
                string protocalText = protocalHandler.Pack("init_chat_request");
                byte[] sendbuf = Encoding.ASCII.GetBytes(protocalText);
                SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
                saEA.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(
                        createdSocketSendNameCompletedEventHandler);
                saEA.SetBuffer(sendbuf, 0, sendbuf.Length);
                createdSocket.SendAsync(saEA);
            }
        }

        /// <summary>
        /// 发起方：
        /// 1次确认发送完成
        /// 准备好接收2次确认（对方（接收方）用户名）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void createdSocketSendNameCompletedEventHandler(
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
        /// 收到的1次确认为私聊，记录对方的身份信息
        /// 此时确定owner
        /// 发送2次确认
        /// </summary>
        /// <param name="protocalHandler">接收到的协议的封装</param>
        public void newSocketChatNameReceived(ProtocalHandler protocalHandler)
        {
            try
            {
                // 对方用户名
                string targetNameBase64 =
                    protocalHandler.GetElementTextByTag("name");
                string targetName =
                    protocalHandler.Base64stringToString(targetNameBase64);
                // 打开私聊窗口
                main.SetChatFormClient(this, targetName);
                // 2次确认，发送you = 对方（发起方）用户名
                string protocalText = protocalHandler.Pack("init_chat_ok");
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
                    "newSocketChatNameReceived");
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
            main.Invoke(main.showFormChatDelegate, owner);
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
                if (!type.Equals("init_chat_ok"))
                {
                    throw new MyProtocalException(
                        "不是有效的2次确认，" +
                        "找不到type标签或type不为init_chat_ok");
                }
                // 对方用户名
                string key = protocalHandler.GetElementTextByTag("name");
                key = protocalHandler.Base64stringToString(key);
                if (!key.Equals(owner.targetName))
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
                if (rtfToSend != null)
                {
                    owner.BeginInvoke(owner.sendRtfDelegate, rtfToSend);
                    rtfToSend = null;
                }
                else if (fileToSend != null)
                {
                    owner.BeginInvoke(owner.sendFileDelegate, fileToSend);
                    fileToSend = null;
                }
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
