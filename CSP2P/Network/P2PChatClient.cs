using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;

// 本文件包含封装的Socket类，用于建立连接、发送消息等。


// # 本文件中涉及的协议：
// 关闭Socket连接
// type = ctl_closesocket, name = 发送方用户名


namespace CSP2P
{
    /// <summary>
    /// 在私聊窗口中使用
    /// P2P通信的Socket的封装
    /// 要么是发起方，要么是接受方
    /// 既能发送又能接收
    /// 收发的所有内容都为ASCII码
    /// </summary>
    public partial class P2PChatClient
    {
        /// <summary>
        /// 缓冲区字节数
        /// </summary>
        public const uint BufferSize = 0x2000;

        /// <summary>
        /// 主窗体
        /// </summary>
        private FormMain main;

        /// <summary>
        /// 绑定的聊天窗口，可以为null
        /// </summary>
        private FormChat owner; 

        /// <summary>
        /// 被封装的Socket
        /// </summary>
        public Socket socket;

        /// <summary>
        /// 接收缓冲区
        /// </summary>
        public byte[] rcvbuf = new byte[BufferSize];

        /// <summary>
        /// 构造函数，接收连接时
        /// </summary>
        /// <param name="main">主窗体</param>
        /// <param name="socket"></param>
        public P2PChatClient(FormMain main, Socket socket)
        {
            this.main = main;
            this.socket = socket;

            sendTimeOutTimer = new System.Timers.Timer(10000);
            sendTimeOutTimer.AutoReset = true;
            sendTimeOutTimer.Elapsed += sendFileTimeOutEventHandler;
        }

        /// <summary>
        /// 构造函数，主动连接时
        /// </summary>
        /// <param name="main">主窗体</param>
        /// <param name="owner">聊天窗口</param>
        /// <param name="socket"></param>
        public P2PChatClient(FormMain main, FormChat owner, Socket socket)
        {
            this.main = main;
            this.owner = owner;
            this.socket = socket;
        }

        /// <summary>
        /// 将P2PChatClient与的FormChat绑定
        /// </summary>
        /// <param name="owner">拥有此Client的聊天窗口</param>
        public void SetOwner(FormChat owner)
        {
            if (this.owner != null)
            {
                // 此处注释允许与自己聊天
                // throw new Exception("P2PChatClient.SetOwner只允许调用一次。");
            }
            this.owner = owner;
        }
        
        /// <summary>
        /// 开始接收对方发送的数据（循环）
        /// 失败则尝试关闭连接（也可能是由于对方关闭了连接）
        /// </summary>
        private void beginReceive()
        {
            try
            {
                SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
                saEA.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(
                        receiveOneEventHandler);
                saEA.SetBuffer(rcvbuf, 0, rcvbuf.Length);
                socket.ReceiveAsync(saEA);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：beginReceive");
                Trace.WriteLine(ex.Message);
                CloseSocket();
            }
        }

        /// <summary>
        /// 收到一条数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void receiveOneEventHandler(object sender, EventArgs ea)
        {
            try
            {
                SocketAsyncEventArgs socketAsyncEA =
                    (SocketAsyncEventArgs)ea;
                // 获取文本
                string rcvString =
                    Encoding.ASCII.GetString(socketAsyncEA.Buffer);
                ProtocalHandler protocalHandler = new ProtocalHandler();
                // 判断是否是协议
                protocalHandler.SetXmlText(rcvString);
                // 对方用户名
                string targetNameBase64 =
                    protocalHandler.GetElementTextByTag("name");
                string targetName =
                    protocalHandler.Base64stringToString(targetNameBase64);
                if (targetName == null)
                {
                    throw new MyProtocalException(
                        "不是有效协议，" +
                        "找不到name标签或无法解码");
                }
                // 类型
                string type = protocalHandler.GetElementTextByTag("type");
                if (type == null)
                {
                    throw new MyProtocalException(
                        "不是有效的协议文本，找不到type标签");
                }
                if (type.StartsWith("file_"))
                {
                    ReceivedTypeFile(type, protocalHandler);
                }
                else
                {
                    switch (type)
                    {
                        case "ctl_closesocket":     // 对方要求关闭Socket
                            closeSocketWithoutSend();
                            break;
                        case "chat_rtf":     // 收到聊天消息（RTF文本）
                            string rtfText = protocalHandler.GetElementTextByTag("data");
                            if (rtfText == null)
                            {
                                throw new MyProtocalException(
                                    "不是有效的协议文本，type为chat_rtf但找不到data标签");
                            }
                            rtfText = protocalHandler.Base64stringToString(rtfText);
                            if (rtfText == null)
                            {
                                throw new MyProtocalException(
                                    "不是有效的协议文本，rtf文本解码错误");
                            }
                            // 启用“发送”（RTF文本）按钮
                            owner.BeginInvoke(owner.enableSendDelegate, true);
                            // 显示收到的消息
                            owner.BeginInvoke(owner.receiveRTFDelegate,
                                new object[] { rtfText });
                            break;
                        default:
                            throw new MyProtocalException("type类型未知: " + type);
                    }   // End Switch
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：P2PChatClient.receiveOneEventHandler");
                Trace.WriteLine(ex.Message);
            }
            finally
            {
                // 继续接收数据
                if (socket != null)
                {
                    beginReceive();
                }
            }
        }

        /// <summary>
        /// 准备关闭与对方的Socket连接，关闭之前通知对方
        /// </summary>
        public void CloseSocket()
        {
            // 通知对方关闭Socket连接
            try
            {
                ProtocalHandler protocalHandler =
                    new ProtocalHandler(main.MyName);
                string protocalText = protocalHandler.Pack("ctl_closesocket");
                byte[] sendbuf = Encoding.ASCII.GetBytes(protocalText);
                SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
                saEA.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(
                        closeSocketCompletedEventHandler);
                saEA.SetBuffer(sendbuf, 0, sendbuf.Length);
                socket.SendAsync(saEA);
            }
            catch   // 异常则已经关闭
            {
                socket = null;
            }
            try
            {
                owner.removeClientDelegate();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：P2PChatClient.closeSocket");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 要求关闭的请求发送完成，尝试关闭Socket
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void closeSocketCompletedEventHandler(
            object sender, EventArgs ea)
        {
            try
            {
                socket.Close();
                socket = null;
                owner.client = null;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置："
                + "P2PChatClient.closeSocketCompletedEventHandler");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 接到对方通知关闭Socket连接，不再通告对方
        /// </summary>
        private void closeSocketWithoutSend()
        {
            try
            {
                socket.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：P2PChatClient.closeSocketWithoutSend");
                Trace.WriteLine(ex.Message);
            }
            socket = null;
            owner.client = null;
        }

        /// <summary>
        /// 发送完成，不进行任何处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void SendCompletedDoNothing(
            object sender, EventArgs ea)
        {
        }
    }
}
