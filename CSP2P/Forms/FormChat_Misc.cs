using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

// 聊天窗口的杂项

namespace CSP2P
{
    public partial class FormChat
    {
        /// <summary>
        /// 窗口关闭前检查是否真的要关闭
        /// </summary>
        /// <returns>true:关闭; false:不关闭</returns>
        protected bool PreClose()
        {
            if (textBoxSend.Text != null && textBoxSend.Text != "")
            {
                if (MessageBox.Show(
                    "您还有消息未发送，是否关闭？", "提示",
                    MessageBoxButtons.YesNo) !=
                    DialogResult.Yes)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="fileName">文件名（含路径）</param>
        protected void SendFile(string fileName)
        {
            // 已经在（准备）收发文件
            if (showPanelFile)
            {
                return;
            }
            // 文件不存在或不是文件
            if (!File.Exists(fileName))
            {
                return;
            }
            Trace.WriteLine("send file: " + fileName);
            this.fileName = fileName;
            sendingOrReceivingFile = false;
            try
            {
                if (client == null)
                {
                    createSocket(fileName, false);
                    return;
                }
                client.RequestToSendFile(fileName);
                labelFileName.Text = GetSafeFileName(fileName);
                buttonRcvFile.Enabled = false;
                progressBarFile.Value = 0;
                ShowPanelFile();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常位置：SendFile");
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 取得不含路径的文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetSafeFileName(string fileName)
        {
            string pattern = @".*\\(.*)";
            try
            {
                Regex regex = new Regex(pattern);
                if (regex.IsMatch(fileName))
                {
                    return regex.Match(fileName).Groups[1].Value;
                }
                else
                {
                    return fileName;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("错误位置：GetSafeFileName");
                Trace.WriteLine(ex.Message);
                return fileName;
            }
        }

        /// <summary>
        /// 显示PanelFile，含动画
        /// </summary>
        protected void ShowPanelFile()
        {
            if (showPanelFile)
            {
                return;
            }
            showPanelFile = true;
            // UI
            progressBarFile.Value = 0;
            this.Enabled = false;
            int dx = 1;
            int i = 0;
            // 期望的窗口宽度
            int w = Width + 200;
            if (Width > Screen.PrimaryScreen.WorkingArea.Width - 100)
            {
                w = Width;
                Width -= 200;
            }
            // 修改panel与窗体边缘的绑定
            panelChat.Anchor = AnchorStyles.Left | AnchorStyles.Top |
                AnchorStyles.Bottom;
            // 计时器
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while (Width < w)
            {
                if (Width + dx < w)
                {
                    Width += dx;
                }
                else
                {
                    Width = w;
                }
                if (++i == 5)
                {
                    // 根据运行速度调整dx
                    dx += (int)(stopWatch.ElapsedMilliseconds / 3);
                    stopWatch.Stop();
                }
            }
            if (i < 5)
            {
                stopWatch.Stop();
            }
            // 恢复panel与窗体边缘的绑定
            panelChat.Anchor = AnchorStyles.Left | AnchorStyles.Top |
                AnchorStyles.Bottom | AnchorStyles.Right;
            this.Enabled = true;
            panelFile.Enabled = true;
        }

        /// <summary>
        /// 隐藏PanelFile，含动画
        /// </summary>
        protected void HidePanelFile()
        {
            if (!showPanelFile)
            {
                return;
            }
            showPanelFile = false;
            this.Enabled = false;
            panelFile.Enabled = false;
            int dx = 1;
            int i = 0;
            // 期望的窗口宽度
            int w = panelChat.Width + 16;
            // 修改panel与窗体边缘的绑定
            panelChat.Anchor = AnchorStyles.Left | AnchorStyles.Top |
                AnchorStyles.Bottom;
            // 计时器
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while (Width > w)
            {
                if (Width - dx > w)
                {
                    Width -= dx;
                }
                else
                {
                    Width = w;
                }
                if (++i == 5)
                {
                    // 根据运行速度调整dx
                    dx += (int)(stopWatch.ElapsedMilliseconds / 3);
                    stopWatch.Stop();
                }
            }
            if (i < 5)
            {
                stopWatch.Stop();
            }
            // 恢复panel与窗体边缘的绑定
            panelChat.Anchor = AnchorStyles.Left | AnchorStyles.Top |
                AnchorStyles.Bottom | AnchorStyles.Right;
            this.Enabled = true;
        }
    }
}
