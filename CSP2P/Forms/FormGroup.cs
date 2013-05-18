using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// 群聊窗口类的主要内容

namespace CSP2P
{
    /// <summary>
    /// 群聊窗口
    /// </summary>
    public partial class FormGroup : Form
    {
        /// <summary>
        /// 主窗体
        /// </summary>
        private FormMain owner;

        /// <summary>
        /// 与群组里各位好友相连的Socket的封装
        /// </summary>
        private Dictionary<string, P2PGroupClient> clients =
            new Dictionary<string,P2PGroupClient>();

        /// <summary>
        /// 字符串表示的好友列表
        /// </summary>
        public string[] friends;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormGroup(FormMain owner, string[] friends)
        {
            InitializeComponent();
            this.owner = owner;
            this.friends = friends;
            foreach (string friend in friends)
            {
                ListViewItem lvi = new ListViewItem(friend);
                lvi.ImageIndex = 0;
                listViewFriends.Items.Add(lvi);
            }
            owner.checkStatusStringsDelegate(friends);
            initDelegates();
        }

        /// <summary>
        /// 点击右键“私聊”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemFLChat_Click(object sender, EventArgs e)
        {
            StartChatting();
        }

        /// <summary>
        /// 双击好友聊天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewFriends_DoubleClick(object sender, EventArgs e)
        {
            StartChatting();
        }

        /// <summary>
        /// 开始与选中的好友私聊
        /// </summary>
        protected void StartChatting()
        {
            // 判断是否选中
            if (listViewFriends.SelectedItems.Count > 0)
            {
                string key = listViewFriends.SelectedItems[0].Text;
                FormChat formChat;
                // 判断是否已经在聊天
                if (!owner.formChats.ContainsKey(key))
                {
                    formChat = new FormChat(owner, key);
                    owner.formChats.Add(key, formChat);
                }
                else
                {
                    formChat = owner.formChats[key];
                }
                owner.showFormChatDelegate(formChat);
            }
        }

        /// <summary>
        /// 点击“关闭”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 点击“发送”
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
            sendRtfText(richTextBoxRcv.SelectedRtf);
        }
        
        /// <summary>
        /// 按下按键
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
        /// 依次发送富文本到群聊好友，如果知道对方地址且没有连接则连接
        /// </summary>
        /// <param name="rtfToSend">要发送的rtf格式文本</param>
        private void sendRtfText(string rtfToSend)
        {
            buttonSend.Enabled = false;
            foreach (ListViewItem lvi in listViewFriends.Items)
            {
                // 不给自己发送
                if (lvi.Text == owner.MyName)
                {
                    continue;
                }
                // 如果已经连接
                if (clients.ContainsKey(lvi.Text))
                {
                    if (clients[lvi.Text].socket != null)
                    {
                        clients[lvi.Text].SendRtfText(rtfToSend);
                    }
                    else
                    {
                        clients.Remove(lvi.Text);
                    }
                }
                if (!clients.ContainsKey(lvi.Text))     // 如果知道地址则创建连接
                {
                    if (owner.remoteIPaddress.ContainsKey(lvi.Text))
                    {
                        clients.Add(lvi.Text,
                            new P2PGroupClient(owner, this, lvi.Text));
                        clients[lvi.Text].CreateSocketToSend(rtfToSend);
                    }
                }
            }
            // 可以发送
            buttonSend.Enabled = true;
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
        /// 设置与对方的P2PGroupClient，并将P2PGroupClient的owner设为自己。
        /// 由于对方连接到自己而调用。
        /// </summary>
        /// <param name="newSocketHandler"></param>
        /// <param name="targetName">发起方的用户名</param>
        public void SetHandlerForSocket(
            P2PGroupClient newSocketHandler, string targetName)
        {
            clients.Add(targetName, newSocketHandler);
            newSocketHandler.SetOwner(this);
        }

        /// <summary>
        /// 窗口即将被关闭时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 关闭所有Socket
            int count = clients.Count;
            P2PGroupClient[] socketHandlers = new P2PGroupClient[count];
            int i = 0;
            foreach (P2PGroupClient socketHandler in clients.Values)
            {
                socketHandlers[i++] = socketHandler;
            }
            for (i = 0; i < count; i++)
            {
                socketHandlers[i].CloseSocket();
            }
        }
    }
}
