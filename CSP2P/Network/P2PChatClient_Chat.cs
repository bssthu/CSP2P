using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;


// 本文件用于聊天文本

// # 本文件中涉及的协议：
// 
// 三、RTF文本传送
// type = chat_rtf, name = 发送方用户名, data = 发送的RTF文本
// 其中name, data使用base64编码


namespace CSP2P
{
    public partial class P2PChatClient
    {
        /// <summary>
        /// 发送RTF文本
        /// </summary>
        /// <param name="rtfToSend"></param>
        public void SendRtfText(string rtfToSend)
        {
            try
            {
                ProtocalHandler protocalHandler = new ProtocalHandler(main.MyName);
                string protocalText = protocalHandler.Pack(
                    "chat_rtf", protocalHandler.StringToBase64string(rtfToSend));
                byte[] sendbuf = Encoding.ASCII.GetBytes(protocalText);
                SocketAsyncEventArgs saEA = new SocketAsyncEventArgs();
                saEA.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(
                        SendCompletedDoNothing);
                saEA.SetBuffer(sendbuf, 0, sendbuf.Length);
                socket.SendAsync(saEA);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：SendRtfText");
                Trace.WriteLine(ex.Message);
            }
        }
    }
}
