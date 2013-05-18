using CSP2P.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

// 聊天窗体使用的委托

namespace CSP2P
{
    public partial class FormChat
    {
        /// <summary>
        /// 启用/禁用控件的委托
        /// </summary>
        /// <param name="en"></param>
        public delegate void EnableDelegate(bool en);

        /// <summary>
        /// 改变图标表明在线/离线状况
        /// </summary>
        public EnableDelegate setOnlineDelegate;

        /// <summary>
        /// 设置“发送”按钮可用/禁用
        /// </summary>
        public EnableDelegate enableSendDelegate;

        /// <summary>
        /// 发送RTF文本的委托
        /// </summary>
        /// <param name="rtfToSend">要发送的RTF文本</param>
        public delegate void SendRtfDelegate(string rtfToSend);

        /// <summary>
        /// 发送RTF文本
        /// </summary>
        public SendRtfDelegate sendRtfDelegate;

        /// <summary>
        /// 发送文件的委托
        /// </summary>
        /// <param name="fileToSend">要发送的文件名（含路径）</param>
        public delegate void SendFileDelegate(string fileToSend);

        /// <summary>
        /// 发送文件
        /// </summary>
        public SendFileDelegate sendFileDelegate;
   

        /// <summary>
        /// 接收到RTF文本的委托
        /// </summary>
        /// <param name="rtf"></param>
        public delegate void ReceiveRTFDelegate(string rtf);

        /// <summary>
        /// 接收到RTF文本
        /// </summary>
        public ReceiveRTFDelegate receiveRTFDelegate;

        /// <summary>
        /// 重置client为null的委托
        /// </summary>
        public delegate void RemoveClientDelegate();

        /// <summary>
        /// 重置client为null
        /// </summary>
        public RemoveClientDelegate removeClientDelegate;

        /// <summary>
        /// 收到对方发送文件的请求的委托
        /// </summary>
        /// <param name="safeFileName"></param>
        public delegate void AskedForReceivingFileDelegate(string safeFileName);

        /// <summary>
        /// 收到对方发送文件的请求
        /// </summary>
        public AskedForReceivingFileDelegate askedForReceivingFileDelegate;

        /// <summary>
        /// 退出收发文件状态的委托
        /// </summary>
        /// <param name="msg">显示的消息</param>
        public delegate void StopSendingOrReceivingFileDelegate(string msg);

        /// <summary>
        /// 退出收发文件状态
        /// </summary>
        public StopSendingOrReceivingFileDelegate
            stopSendingOrReceivingFileDelegate;

        /// <summary>
        /// 在消息框内显示消息的委托
        /// </summary>
        /// <param name="message">要显示的消息</param>
        public delegate void ShowMessageDelegate(string message);

        /// <summary>
        /// 在消息框内显示消息
        /// </summary>
        public ShowMessageDelegate showMessageDelegate;

        /// <summary>
        /// 设置文件收发的最大序号的委托
        /// </summary>
        /// <param name="maxSeq">最大序号</param>
        public delegate void SetProgressDelegate(int maxSeq);

        /// <summary>
        /// 设置文件收发的最大序号
        /// </summary>
        public SetProgressDelegate setProgressDelegate;

        /// <summary>
        /// 设置文件收发的当前序号的委托
        /// </summary>
        /// <param name="seq">当前序号</param>
        public delegate void ShowProgressDelegate(int seq);

        /// <summary>
        /// 设置文件收发的当前序号
        /// </summary>
        public ShowProgressDelegate showProgressDelegate;

        /// <summary>
        /// 显示文件收发的状态的委托
        /// </summary>
        /// <param name="status"></param>
        public delegate void UpdateStatusDelegate(string status);

        /// <summary>
        /// 显示文件收发的状态
        /// </summary>
        public UpdateStatusDelegate updateStatusDelegate;

        /// <summary>
        /// 启动定时器计时器的委托
        /// </summary>
        public delegate void StartTimingDelegate();

        /// <summary>
        /// 启动定时器计时器
        /// </summary>
        public StartTimingDelegate startTimingDelegate;


        /// <summary>
        /// 初始化委托
        /// </summary>
        private void initDelegates()
        {
            setOnlineDelegate = new EnableDelegate(onLog);
            enableSendDelegate = new EnableDelegate(onEnableSend);
            sendRtfDelegate = new SendRtfDelegate(onSendRtfDelegate);
            sendFileDelegate = new SendFileDelegate(SendFile);
            receiveRTFDelegate = new ReceiveRTFDelegate(onReceiveRTF);
            removeClientDelegate = new RemoveClientDelegate(onRemoveClient);
            askedForReceivingFileDelegate =
                new AskedForReceivingFileDelegate(onAskedForReceivingFile);
            stopSendingOrReceivingFileDelegate =
                new StopSendingOrReceivingFileDelegate(onStopSendingOrReceivingFile);
            showMessageDelegate = new ShowMessageDelegate(onShowMessage);
            setProgressDelegate = new SetProgressDelegate(onSetProgress);
            showProgressDelegate = new ShowProgressDelegate(onShowProgress);
            updateStatusDelegate = new UpdateStatusDelegate(onUpdateStatus);
            startTimingDelegate = new StartTimingDelegate(onStartTiming);
        }

        /// <summary>
        /// 设置用户在线/离线
        /// </summary>
        /// <param name="online"></param>
        private void onLog(bool online)
        {
            if (online)
            {
                Icon = Resources.chat_on;
            }
            else
            {
                Icon = Resources.chat_off;
            }
        }

        /// <summary>
        /// 启用/禁用“发送”按钮
        /// </summary>
        /// <param name="en"></param>
        private void onEnableSend(bool en)
        {
            buttonSend.Enabled = en;
        }

        /// <summary>
        /// 收到RTF字符串后的处理
        /// </summary>
        /// <param name="rcvRTF"></param>
        private void onReceiveRTF(string rcvRTF)
        {
            richTextBoxRcv.Select(richTextBoxRcv.Text.Length,
                richTextBoxRcv.Text.Length);
            try
            {
                richTextBoxRcv.SelectedRtf = rcvRTF;
            }
            catch
            {
                Trace.WriteLine("收到的RTF文本有误");
            }
            richTextBoxRcv.ScrollToCaret();
        }

        /// <summary>
        /// 发送RTF文本
        /// </summary>
        /// <param name="rtfToSend">要发送的RTF文本</param>
        private void onSendRtfDelegate(string rtfToSend)
        {
            sendRtfText(rtfToSend);
        }

        /// <summary>
        /// 将client重置为null
        /// </summary>
        private void onRemoveClient()
        {
            client = null;
        }

        /// <summary>
        /// 收到对方发送文件的请求
        /// </summary>
        /// <param name="safeFileName">文件名</param>
        private void onAskedForReceivingFile(string safeFileName)
        {
            if (showPanelFile)
            {
                ////////////发拒绝
                return;
            }
            labelFileName.Text = safeFileName;
            // 此处this.fileName不含路径，但在点“接受”时会要求选择路径
            fileName = safeFileName;
            buttonRcvFile.Enabled = true;
            // 显示“接受”按钮
            buttonRcvFile.Show();
            ShowPanelFile();
        }

        /// <summary>
        /// 取消收发文件状态
        /// </summary>
        /// <param name="msg">要显示的消息</param>
        private void onStopSendingOrReceivingFile(string msg)
        {
            if (!showPanelFile)
            {
                // 没有在（准备）收发文件的状态
                return;
            }
            // Hide panelFile
            HidePanelFile();
            // 如果已经在收发文件，则终止
            if (sendingOrReceivingFile)
            {
                sendingOrReceivingFile = false;
                if (msg != null)
                {
                    onShowMessage(msg);
                }
                // IO was stoped
            }
            // 停止时间
            try
            {
                fileTimer.Stop();
                fileStopwatch.Reset();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：StopSendingOrReceivingFile");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 在消息框内显示消息
        /// </summary>
        /// <param name="message">要显示的消息</param>
        private void onShowMessage(string message)
        {
            if (message != null)
            {
                message += "\n\n";
                // 追加文本
                richTextBoxRcv.AppendText(message);
                // 设置字体
                richTextBoxRcv.Select(
                    richTextBoxRcv.Text.Length - message.Length,
                    richTextBoxRcv.Text.Length);
                richTextBoxRcv.SelectionFont = messageFont;
                richTextBoxRcv.SelectionColor = Color.DarkGray;
                // 滚动到底部
                richTextBoxRcv.ScrollToCaret();
            }
        }

        /// <summary>
        /// 设置文件收发的最大序号
        /// </summary>
        /// <param name="maxSeq">最大序号</param>
        private void onSetProgress(int maxSeq)
        {
            try
            {
                progressBarFile.Maximum = maxSeq;
                labelFileStatus.Text = "准备传送";
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：StopSendingOrReceivingFile");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 设置文件收发的当前序号
        /// </summary>
        /// <param name="seq">当前序号</param>
        private void onShowProgress(int seq)
        {
            try
            {
                progressBarFile.Value = seq;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：ShowProgress");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 显示文件收发的状态
        /// </summary>
        /// <param name="status"></param>
        private void onUpdateStatus(string status)
        {
            labelFileStatus.Text = status;
        }

        /// <summary>
        /// 启动定时器计时器
        /// </summary>
        private void onStartTiming()
        {
            try
            {
                // 开始时间
                fileTimer.Start();
                fileStopwatch.Start();
                progressBarFile.Value = 0;
                estimatedSpeed = 0;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：StartTiming");
                Trace.WriteLine(ex.Message);
            }
        }
    }
}
