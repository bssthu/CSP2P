using CSP2P.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// 主窗体的杂项

namespace CSP2P
{
    public partial class FormMain
    {
        /// <summary>
        /// 载入设置
        /// </summary>
        private void loadSettings()
        {
            if (Settings.Default.remUser)
            {
                checkBoxRemUser.Checked = true;
                textBoxUser.Text = Settings.Default.User;
            }
            if (Settings.Default.remWindowMain)
            {
                Size = Settings.Default.windowSizeMain;
                Left = Settings.Default.windowPosMain.X;
                Top = Settings.Default.windowPosMain.Y;
            }
        }

        /// <summary>
        /// 存档设置
        /// </summary>
        private void saveSettings()
        {
            if (checkBoxRemUser.Checked)
            {
                Settings.Default.remUser = true;
                Settings.Default.User = textBoxUser.Text;
            }
            else
            {
                Settings.Default.remUser = false;
            }
            Settings.Default.windowSizeMain = Size;
            Settings.Default.windowPosMain = new Point(Left, Top);
            Settings.Default.Save();
        }

        /// <summary>
        /// 将窗口从托盘中恢复
        /// </summary>
        protected void ShowFromNI()
        {
            Show();
            WindowState = FormWindowState.Normal;
            toolStripMenuItemNIShowHide.Text = "隐藏(&H)";
        }

        /// <summary>
        /// 将窗口隐藏到托盘
        /// </summary>
        protected void HideToNI()
        {
            Hide();
            toolStripMenuItemNIShowHide.Text = "显示(&S)";
        }

        /// <summary>
        /// 开始对话，如果已经打开对话框则跳转
        /// </summary>
        protected void StartChatting()
        {
            // 选中的好友数
            int count = listViewFriends.SelectedItems.Count;
            if (count == 1)       // 私聊
            {
                string key = listViewFriends.SelectedItems[0].Text;
                if (!formChats.ContainsKey(key))
                {
                    formChats.Add(key, new FormChat(this, key));
                }
                else if (formChats[key].Disposing || formChats[key].IsDisposed)
                {
                    formChats.Remove(key);
                    formChats.Add(key, new FormChat(this, key));
                }
                formChats[key].Show();
            }
            else if (count > 1)       // 群聊
            {
                string[] keys = new string[count];
                // 保证自己在列表内
                bool contained = false;
                for (int i = 0; i < count; i++)
                {
                    keys[i] = listViewFriends.SelectedItems[i].Text;
                    if (keys[i] == MyName)
                    {
                        contained = true;
                    }
                }
                if (!contained)
                {
                    string[] tmp = keys;
                    keys = new string[count + 1];
                    keys[0] = MyName;
                    for (int i = 1; i < count + 1; i++)
                    {
                        keys[i] = tmp[i - 1];
                    }
                }
                FormGroup fg = new FormGroup(this, keys);
                formGroups.Add(fg);
                foreach (ListViewItem lvi in listViewFriends.SelectedItems)
                {
                    if (lvi.ImageIndex == 0)    // 不在线
                    {
                        fg.OnLogoffRefresh(lvi.Text);
                    }
                    else    // 在线
                    {
                        fg.OnLoginRefresh(lvi.Text);
                    }
                }
                fg.Show();
            }
        }

        /// <summary>
        /// 将P2PChatClient与FormChat互相绑定
        /// 如果没有FormChat则创建
        /// </summary>
        /// <param name="p2pClient"></param>
        /// <param name="targetName">发起方的用户名</param>
        public void SetChatFormClient(P2PChatClient p2pClient, string targetName)
        {
            if (!formChats.ContainsKey(targetName))
            {
                formChats.Add(targetName, new FormChat(this, targetName));
            }
            formChats[targetName].SetHandlerForSocket(p2pClient);
        }

        /// <summary>
        /// 将P2PChatClient与新建的FormGroup互相绑定
        /// </summary>
        /// <param name="p2pClient"></param>
        /// <param name="targetName">发起方的用户名</param>
        /// <param name="targetNames">群聊的好友列表</param>
        public void SetGroupFormClient(P2PGroupClient p2pClient, string targetName, string[] targetNames)
        {
            FormGroup fg = new FormGroup(this, targetNames);
            formGroups.Add(fg);
            fg.SetHandlerForSocket(p2pClient, targetName);
        }

        /// <summary>
        /// 添加一个好友
        /// </summary>
        /// <param name="friendName">好友用户名</param>
        protected void AddFriend(string friendName)
        {
            ListViewItem lvi = new ListViewItem(friendName);
            lvi.ImageIndex = 0;
            listViewFriends.Items.Add(lvi);
            comboBoxFriends.Items.Add(lvi.Text);
        }
    }
}
