using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Timers;
using System.Windows.Forms;

// 聊天窗体类的主要部分，各种内容

namespace CSP2P
{
    /// <summary>
    /// 聊天窗口
    /// </summary>
    public partial class FormChat : Form
    {
        /// <summary>
        /// 主窗体
        /// </summary>
        private FormMain owner;

        /// <summary>
        /// 对方的用户名
        /// </summary>
        public string targetName;

        /// <summary>
        /// 完整文件名（含路径）
        /// </summary>
        public string fileName;

        /// <summary>
        /// 表示PanelFile的活动状态
        /// </summary>
        public bool showPanelFile = false;

        /// <summary>
        /// 是否已经开始文件收发
        /// </summary>
        public bool sendingOrReceivingFile = false;

        /// <summary>
        /// 聊天消息窗口中显示的系统消息使用的字体
        /// </summary>
        private Font messageFont = new Font("Ariel", 9, FontStyle.Italic);

        /// <summary>
        /// 定时器
        /// </summary>
        private System.Timers.Timer fileTimer = new System.Timers.Timer(500);

        /// <summary>
        /// 计时器
        /// </summary>
        private Stopwatch fileStopwatch = new Stopwatch();

        /// <summary>
        /// 文件收发的平均速度
        /// </summary>
        private double estimatedSpeed;
        

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="myOwner">主窗体</param>
        /// <param name="myTargetName">对方的用户名</param>
        public FormChat(FormMain myOwner, string myTargetName)
        {
            InitializeComponent();
            owner = myOwner;
            targetName = myTargetName;
            Text = String.Format("与 {0} 聊天中", targetName);
            panelFile.Enabled = false;
            if (owner.remoteIPaddress.ContainsKey(targetName))
            {
                this.Icon = Properties.Resources.chat_on;
            }
            else
            {
                this.Icon = Properties.Resources.chat_off;
            }
            fileTimer.Elapsed += calculateFile;
            initDelegates();
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = textBoxSend.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxSend.Font = fontDialog.Font;
            }
        }

        /// <summary>
        /// 选择颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = textBoxSend.ForeColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxSend.ForeColor = colorDialog.Color;
            }
        }

        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                SendFile(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// “关闭”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 窗口关闭前的事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!PreClose())
            {
                e.Cancel = true;
            }
            if (client != null)
            {
                client.CloseSocket();
            }
            owner.formChats.Remove(targetName);
        }

        /// <summary>
        /// 在输入框中按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 按下了回车，发送
            if (e.KeyChar == '\r')
            {
                buttonSend_Click(this, null);
                e.KeyChar = '\0';
            }
        }

        /// <summary>
        /// “发送”，
        /// 将输入框的文字发送并清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (textBoxSend.Text == null ||
                textBoxSend.Text == "" ||
                !buttonSend.Enabled)
            {
                return;
            }
            // 设置格式（用户名 时间）
            string textToAppend = String.Format(
                "{0} {1:00}:{2:00}:{3:00}\n{4}\n\n",
                owner.MyName,
                DateTime.Now.TimeOfDay.Hours,
                DateTime.Now.TimeOfDay.Minutes,
                DateTime.Now.TimeOfDay.Seconds, textBoxSend.Text);
            // 追加文本
            richTextBoxRcv.AppendText(textToAppend);
            // 清输入区
            textBoxSend.Clear();
            // 设置字体
            richTextBoxRcv.Select(
                richTextBoxRcv.Text.Length - textToAppend.Length,
                richTextBoxRcv.Text.Length);
            richTextBoxRcv.SelectionFont = textBoxSend.Font;
            richTextBoxRcv.SelectionColor = textBoxSend.ForeColor;
            // 滚动到底部
            richTextBoxRcv.ScrollToCaret();
            // 发送RTF
            if (owner.remoteIPaddress.ContainsKey(targetName))
            {
                sendRtfText(richTextBoxRcv.SelectedRtf);
            }
        }

        /// <summary>
        /// 拖入文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormChat_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// 放下文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormChat_DragDrop(object sender, DragEventArgs e)
        {
            string fileName = ((string[])e.Data.
                GetData(DataFormats.FileDrop))[0];
            SendFile(fileName);
        }

        /// <summary>
        /// 点击“接受”（文件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRcvFile_Click(object sender, EventArgs e)
        {
            // 选择保存路径
            FolderBrowserDialog folderBrowserDialog =
                new FolderBrowserDialog();
            try
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = folderBrowserDialog.SelectedPath;
                    if (!fileName.EndsWith("\\"))
                    {
                        fileName += "\\";
                    }
                    fileName += labelFileName.Text;
                    sendingOrReceivingFile = true;
                    this.BeginInvoke(startTimingDelegate);
                    // 告诉对方同意接收
                    client.ClearToSendFile();
                    // 隐藏“接受”按钮
                    buttonRcvFile.Hide();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：buttonRcvFile_Click");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 点击“取消”（文件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancelFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (sendingOrReceivingFile)
                {
                    client.StopToSendFile();
                }
                else
                {
                    client.DeniedToFile();
                }
                onStopSendingOrReceivingFile(null);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：buttonCancelFile_Click");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 计算文件收发速度、剩余时间等
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calculateFile(object sender, EventArgs e)
        {
            try
            {
                // 平均速度
                estimatedSpeed = 0.875 * estimatedSpeed +
                    0.125 * progressBarFile.Value /
                    fileStopwatch.Elapsed.TotalSeconds;
                // 完成度
                double finished = ((double)progressBarFile.Value /
                    (double)progressBarFile.Maximum) * 100.0;
                // 剩余时间
                double timeLeft =
                    (progressBarFile.Maximum -
                    progressBarFile.Value) / estimatedSpeed;
                string timeLeftString =
                    string.Format("{0:0.##}秒", timeLeft % 60);
                if (timeLeft >= 60)
                {
                    timeLeftString =
                        String.Format("{0}分{1}",
                        ((int)timeLeft / 60) % 60, timeLeftString);
                    if (timeLeft >= 3600)
                    {
                        timeLeftString =
                            String.Format("{0}小时{1}",
                            ((int)timeLeft / 3600) % 60, timeLeftString);
                    }
                }
                // 显示到UI
                this.BeginInvoke(updateStatusDelegate,
                    String.Format("平均速度：{0:0,0.##}KB/秒\n剩余时间：{1}\n完成度：{2:0.0000} %",
                    estimatedSpeed * 4, timeLeftString, finished));
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：calculateFile");
                Trace.WriteLine(ex.Message);
            }
        }
    }
}
