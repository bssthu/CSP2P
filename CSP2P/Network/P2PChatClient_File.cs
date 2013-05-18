using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Timers;

// 本文件是对封装的Socket类的补充，用于发送文件。一次只能发送一个文件。
// 停等协议

// # 本本文件中涉及的协议：
// 一、发送文件
// 1) 申请发送，由发起方发送
// type = file_requesttosend, name = 发送方（发起方）用户名, maxseq = 序号总数
// file = 文件名（不含路径）
// 其中name, file使用base64编码
// 
// 2) 同意接收，由接受方发送
// type = file_cleartosend, name = 发送方（接受方）用户名, file = 文件名（不含路径）
// 其中name, file使用base64编码
// 
// 3) 拒绝接收，由接受方发送
// type = file_denied, name = 发送方（接受方）用户名, file = 文件名（不含路径）
// 其中name使用base64编码, file使用base64编码
// 
// 4) 文件数据包，由发起方发送
// type = file_data, name = 发送方（发起方）用户名, file = 文件名, 
// seq = 文件片段的序号, len = 片段长度, data = 文件内容, checksum = 解码后的data的校验和
// 其中name, file, data使用base64编码
// 
// 5) 文件数据包确认，由接受方发送
// type = file_ack, name = 发送方（发起方）用户名, file = 文件名, 
// ack = 期望的文件片段序号
// 其中name, file使用base64编码
// 
// 6) 取消文件收发
// type = file_abort, name = 发送方（发起方）用户名, file = 文件名
// 其中name, file使用base64编码


// # 准备发送文件的流程：
// Function P2PClient::receiveOneEventHandler()
// 所有收到消息的处理函数
// 
// Function P2PClient::RequestToSendFile()
// 发起方：申请传送文件
// 接受方：用户确认
// Function P2PClient::ClearToSendFile()
// 接受方：同意接收文件

namespace CSP2P
{
    public partial class P2PChatClient
    {
        /// <summary>
        /// 发送或接收的文件名（不含路径），使用Base64编码
        /// </summary>
        private string safeFileNameBase64;

        /// <summary>
        /// 文件发送类
        /// </summary>
        private FilePacker filePacker;

        /// <summary>
        /// 文件接收类
        /// </summary>
        private FileWriter fileWriter;

        /// <summary>
        /// 接收方等待接收的下一个序号
        /// </summary>
        private uint currentSeq;

        /// <summary>
        /// 该文件的分包数量
        /// </summary>
        private uint maxSeq;

        /// <summary>
        /// 发送文件的超时定时器
        /// </summary>
        private Timer sendTimeOutTimer;

        /// <summary>
        /// 发送文件时的异常总数
        /// </summary>
        private int sendErrCount;

        /// <summary>
        /// 收到的协议是file类型时的判断
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="protocalHandler">协议处理类</param>
        protected void ReceivedTypeFile(string type, ProtocalHandler protocalHandler)
        {
            // 文件收发时接收到的文件名
            string receivedSafeFileNameBase64 =
                protocalHandler.GetElementTextByTag("file");
            try
            {
                switch (type)
                {
                    case "file_requesttosend":     // 收到接收文件的请求
                        if (owner.showPanelFile)
                        {
                            // 直接拒收
                            DeniedToFile();
                            break;
                        }
                        if (receivedSafeFileNameBase64 == null)
                        {
                            throw new MyProtocalException(
                                "不是有效的协议文本，type为file但找不到file标签");
                        }
                        string receivedSafeFileName =
                            protocalHandler.Base64stringToString(receivedSafeFileNameBase64);
                        if (receivedSafeFileName == null)
                        {
                            throw new MyProtocalException(
                                "不是有效的协议文本，文件名解码错误");
                        }
                        safeFileNameBase64 = receivedSafeFileNameBase64;
                        maxSeq = Convert.ToUInt32(
                            protocalHandler.GetElementTextByTag("maxseq"));
                        owner.BeginInvoke(owner.setProgressDelegate, (int)maxSeq);
                        currentSeq = 0;
                        // 让用户选择是否接收文件
                        owner.BeginInvoke(owner.askedForReceivingFileDelegate,
                            receivedSafeFileName);
                        break;

                    case "file_cleartosend":
                        if (!owner.showPanelFile)
                        {
                            // 拒收
                            StopToSendFile();
                            break;
                        }
                        // 检查文件名匹配情况
                        CheckFileName(receivedSafeFileNameBase64);
                        // 开始发送
                        try
                        {
                            owner.sendingOrReceivingFile = true;
                            owner.BeginInvoke(owner.startTimingDelegate);
                            sendingFilePack(0);
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine("无法开始发送文件。");
                            throw ex;
                        }
                        break;

                    case "file_denied":      // 发文件的请求被拒绝
                        if (!owner.showPanelFile)
                        {
                            // 没有在准备收发文件的状态
                            break;
                        }
                        if (owner.sendingOrReceivingFile)
                        {
                            // 已经在收发文件，取消的type应该为file_abort
                            StopToSendFile();
                            break;
                        }
                        // 检查文件名匹配情况
                        CheckFileName(receivedSafeFileNameBase64);
                        // 取消文件发送
                        fileDenied();
                        break;

                    case "file_data":
                        // 检查是否正在收发文件
                        CheckWhileSendingOrReceiving();
                        // 超时定时器重置
                        sendTimeOutTimer.Start();
                        // 检查文件名匹配情况
                        CheckFileName(receivedSafeFileNameBase64);
                        string seqString = protocalHandler.GetElementTextByTag("seq");
                        uint receivedSeq = Convert.ToUInt32(seqString);
                        string data = protocalHandler.GetElementTextByTag("data");
                        byte[] buf = Convert.FromBase64String(data);
                        // 检查checksum
                        uint len = Convert.ToUInt32(
                            protocalHandler.GetElementTextByTag("len"));
                        int checksum = Convert.ToInt32(
                            protocalHandler.GetElementTextByTag("checksum"));
                        for (uint i = 0; i < len; i++)
                        {
                            checksum += buf[i];
                        }
                        // 顺序正确 && 校验正确
                        if (receivedSeq == currentSeq && checksum == 0)
                        {
                            owner.BeginInvoke(owner.showProgressDelegate, (int)currentSeq);
                            fileWriter.WriteBytes(currentSeq, buf, len);
                            sendAck(++currentSeq);
                            if (currentSeq >= maxSeq)
                            {
                                finishSendingOrReceivingFile();
                            }
                        }
                        else
                        {
                            sendAck(currentSeq);
                        }
                        break;

                    case "file_ack":
                        // 检查是否正在收发文件
                        CheckWhileSendingOrReceiving();
                        // 检查文件名匹配情况
                        CheckFileName(receivedSafeFileNameBase64);
                        string ackString = protocalHandler.GetElementTextByTag("ack");
                        receivedAck(Convert.ToUInt32(ackString));
                        break;

                    case "file_abort":
                        if (!owner.showPanelFile)
                        {
                            // 没有在准备收发文件的状态
                            break;
                        }
                        if (!owner.sendingOrReceivingFile)
                        {
                            // 没有正在收发文件
                            break;
                        }
                        // 取消文件发送
                        owner.BeginInvoke(owner.stopSendingOrReceivingFileDelegate,
                            String.Format("文件{0}的收发已被取消。",
                            protocalHandler.Base64stringToString(safeFileNameBase64)));
                        fileAborted();
                        break;

                    default:
                        throw new MyProtocalException("协议类型未知");
                        //break;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：ReceivedTypeFile");
                Trace.WriteLine(ex.Message);
                sendErrCount++;
                if (owner.sendingOrReceivingFile)
                {
                    sendAck(currentSeq);
                }
                
                //StopToSendFile();
            }
        }

        /// <summary>
        /// 发起方：
        /// 申请发送文件
        /// </summary>
        /// <param name="fileName">文件名（含路径）</param>
        public void RequestToSendFile(string fileName)
        {
            // 打开文件
            filePacker = new FilePacker(owner.fileName);
            maxSeq = filePacker.GetPackNumbers();
            owner.BeginInvoke(owner.setProgressDelegate, (int)maxSeq);
            // 取得文件名，并将文件名编码以便使用
            safeFileNameBase64 = owner.GetSafeFileName(fileName);
            ProtocalHandler protocalHandler =
                new ProtocalHandler(owner.targetName);
            safeFileNameBase64 = protocalHandler.StringToBase64string(safeFileNameBase64);
            // 封装协议文本
            string protocalText = protocalHandler.Pack("file_requesttosend");
            protocalText =
                protocalHandler.Append(protocalText, "file", safeFileNameBase64);
            protocalText =
                protocalHandler.Append(protocalText, "maxseq", maxSeq.ToString());
            try
            {
                // 发送
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
                Trace.WriteLine("异常位置：RequestToSendFile");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 接受方：
        /// 同意接收文件
        /// </summary>
        public void ClearToSendFile()
        {
            ProtocalHandler protocalHandler =
                new ProtocalHandler(owner.targetName);
            // 封装协议文本
            string protocalText = protocalHandler.Pack("file_cleartosend");
            protocalText =
                protocalHandler.Append(protocalText, "file", safeFileNameBase64);
            try
            {
                fileWriter = new FileWriter(owner.fileName);
                // 发送
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
                Trace.WriteLine("异常位置：ClearToSendFile");
                Trace.WriteLine(ex.Message);
            }
            sendTimeOutTimer.Start();
            sendErrCount = 0;
        }

        /// <summary>
        /// 发送方：
        /// 发送文件的某个片段
        /// </summary>
        /// <param name="seq">当前序号</param>
        private void sendingFilePack(uint seq)
        {
            ProtocalHandler protocalHandler =
                new ProtocalHandler(owner.targetName);
            // 封装协议文本
            string protocalText = protocalHandler.Pack("file_data");
            protocalText =
                protocalHandler.Append(protocalText, "file", safeFileNameBase64);
            protocalText =
                protocalHandler.Append(protocalText, "seq", seq.ToString());
            uint len = FilePacker.BytesOfPack;
            if (seq == maxSeq - 1)
            {
                len = filePacker.GetSizeOfLastPack();
            }
            protocalText =
                protocalHandler.Append(protocalText, "len", len.ToString());
            // 文件内容
            byte[] buf = filePacker.GetPack(seq);
            string dataBase64 = Convert.ToBase64String(buf, 0, (int)len);
            protocalText =
                protocalHandler.Append(protocalText, "data", dataBase64);
            int checksum = 0;
            for (int i = 0; i < len; i++)
            {
                checksum += buf[i];
            }
            checksum = -checksum;
            protocalText =
                protocalHandler.Append(protocalText, "checksum",
                checksum.ToString());
            try
            {
                // 发送
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
                Trace.WriteLine("异常位置：SendingFilePack");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 发送方收到ACK确认
        /// </summary>
        /// <param name="newAck">新收到的ACK号</param>
        private void receivedAck(uint newAck)
        {
            if (newAck < maxSeq)
            {
                owner.BeginInvoke(owner.showProgressDelegate, (int)newAck);
                sendingFilePack(newAck);
            }
            else
            {
                finishSendingOrReceivingFile();
            }
        }

        /// <summary>
        /// 接收方发送确认序号
        /// </summary>
        /// <param name="ack">下一个等待接收的序号</param>
        private void sendAck(uint ack)
        {
            ProtocalHandler protocalHandler =
                new ProtocalHandler(owner.targetName);
            try
            {
                // 封装协议文本
                string protocalText = protocalHandler.Pack("file_ack");
                protocalText =
                    protocalHandler.Append(protocalText, "file", safeFileNameBase64);
                protocalText =
                    protocalHandler.Append(protocalText, "ack", ack.ToString());
                // 发送
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
                Trace.WriteLine("异常位置：sendAck");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 拒绝接收文件
        /// </summary>
        public void DeniedToFile()
        {
            ProtocalHandler protocalHandler =
                new ProtocalHandler(owner.targetName);
            try
            {
                // 封装协议文本
                string protocalText = protocalHandler.Pack("file_denied");
                protocalText =
                    protocalHandler.Append(protocalText, "file", safeFileNameBase64);
                // 发送
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
                Trace.WriteLine("异常位置：DeniedToFile");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 中途主动取消文件收发
        /// </summary>
        public void StopToSendFile()
        {
            ProtocalHandler protocalHandler =
                new ProtocalHandler(owner.targetName);
            try
            {
                // 封装协议文本
                string protocalText = protocalHandler.Pack("file_abort");
                protocalText =
                    protocalHandler.Append(protocalText, "file", safeFileNameBase64);
                // 发送
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
                Trace.WriteLine("异常位置：StopToSendFile");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 文件发送/接收请求被对方取消
        /// </summary>
        private void fileDenied()
        {
            sendTimeOutTimer.Stop();
            owner.BeginInvoke(owner.stopSendingOrReceivingFileDelegate,
                String.Format("已拒收文件{0}",
                Convert.FromBase64String(safeFileNameBase64)));
            safeFileNameBase64 = null;
        }

        /// <summary>
        /// 文件发送/接收过程中被对方终止
        /// </summary>
        private void fileAborted()
        {
            sendTimeOutTimer.Stop();
            if (filePacker != null)
            {
                filePacker.Close();
                filePacker = null;
            }
            if (fileWriter != null)
            {
                fileWriter.Close();
                fileWriter = null;
            }
            safeFileNameBase64 = null;
        }

        /// <summary>
        /// 完成文件收发
        /// </summary>
        private void finishSendingOrReceivingFile()
        {
            // 关定时器
            sendTimeOutTimer.Stop();
            // 关文件
            if (filePacker != null)
            {
                filePacker.Close();
                filePacker = null;
            }
            if (fileWriter != null)
            {
                fileWriter.Close();
                fileWriter = null;
            }
            ProtocalHandler protocalHandler = new ProtocalHandler();
            owner.BeginInvoke(owner.stopSendingOrReceivingFileDelegate,
                String.Format("文件{0}的收发已完成。",
                protocalHandler.Base64stringToString(safeFileNameBase64)));
            safeFileNameBase64 = null;
        }

        /// <summary>
        /// 检查是否正在发送文件
        /// </summary>
        protected void CheckWhileSendingOrReceiving()
        {
            if (!owner.showPanelFile)
            {
                StopToSendFile();
                throw new Exception("没有在准备收发文件的状态");
            }
            if (!owner.sendingOrReceivingFile)
            {
                // 尚未开始收发文件，取消的type应该为file_denied
                StopToSendFile();
                throw new Exception("尚未开始收发文件");
            }
        }

        /// <summary>
        /// 判断所指文件是否是正在处理的文件，
        /// 若不是则抛出异常
        /// </summary>
        /// <param name="safeFileNameBase64"></param>
        /// <returns></returns>
        protected void CheckFileName(string safeFileNameBase64)
        {
            if (safeFileNameBase64 == null)
            {
                throw new MyProtocalException(
                    "收到了错误的协议，目前不在收发文件状态，文件名尚未设置");
            }
            if (!safeFileNameBase64.Equals(safeFileNameBase64))
            {
                throw new MyProtocalException(
                    "收到了错误的协议，文件名不匹配");
            }
        }

        /// <summary>
        /// 发送文件超时的事件处理程序
        /// 理解为丢包，要求重传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void sendFileTimeOutEventHandler(
            object sender, EventArgs ea)
        {
            sendAck(currentSeq);
        }
    }
}
