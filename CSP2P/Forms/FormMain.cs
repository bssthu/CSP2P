using CSP2P.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

// 主窗体类的主要部分
// 本文件包含登陆界面、好友列表的UI设计

namespace CSP2P
{
    public partial class FormMain : Form
    {
        [DllImport("Shell32.dll", EntryPoint = "IsUserAnAdmin")]
        private static extern bool IsUserAnAdmin();

        /// <summary>
        /// 聊天窗口集合
        /// </summary>
        public Dictionary<string, FormChat> formChats =
            new Dictionary<string, FormChat>();

        /// <summary>
        /// 群聊窗口集合
        /// </summary>
        public List<FormGroup> formGroups = new List<FormGroup>();

        /// <summary>
        /// 修改本地IP的窗口
        /// </summary>
        public FormSetLocalIP formSetLocalIP = null;

        /// <summary>
        /// 是否已经取得管理员权限
        /// </summary>
        private bool isAdmin = false;

        /// <summary>
        /// 登陆用户名
        /// </summary>
        public string MyName
        {
            get
            {
                return textBoxUser.Text;
            }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
     /*   public FormMain()
        {
        }*/

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormMain(string[] args)
        {
            CheckAdmin(args);

            // 初始化
            InitializeComponent();
            loadSettings();
            IPHostEntry iphe = Dns.GetHostEntry(
                Dns.GetHostName());
            textBoxUser.Text = iphe.AddressList[0].ToString();
            foreach (IPAddress ip in iphe.AddressList)
            {
                if (ip.ToString().Split('.').Length == 4)
                {
                    textBoxUser.Text = ip.ToString();
                }
            }

            textBoxUser.NoteInactive();
            textBoxPw.NoteInactive();

            initDelegates();
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /*public void CheckAdmin(string[] args)
        {
            if ((args.Length == 0) || (args[0] != "-asAdmin"))
            {
                if ((AtLeastVista()) && (!IsUserAnAdmin()))
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        Arguments = "-asAdmin",
                        ErrorDialog = true,
                        ErrorDialogParentHandle = Handle,
                        FileName = Application.ExecutablePath,
                        Verb = "runas"
                    };
                    try
                    {
                        Process.Start(psi);
                        System.Environment.Exit(0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n可能无法使用修改IP等功能。");
                    }
                }
            }
            if ((args.Length != 0) && (args[0] == "-asAdmin"))
            {
                if ((AtLeastVista()) && (!IsUserAnAdmin()))
                {
                    MessageBox.Show("没有取得管理员权限，可能无法使用修改IP等功能。");
                }
            }
        }*/

         //// <summary>
        /// 检查权限
        /// </summary>
        public void CheckAdmin(string[] args)
        {
            if (!AtLeastVista())
            {
                isAdmin = true;
                return;
            }
            isAdmin = IsUserAnAdmin();
        }

        /// <summary>
        /// 判断操作系统版本
        /// </summary>
        /// <returns></returns>
        private static bool AtLeastVista()
        {
            return ((Environment.OSVersion.Platform == PlatformID.Win32NT)
                && (Environment.OSVersion.Version.Major >= 6));
        }

        /// <summary>
        /// 窗口关闭时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosed(
            object sender, FormClosedEventArgs e)
        {
            try
            {
                udpListenerThread.Abort();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常：无法终止UDP监听线程");
                Trace.WriteLine(ex.Message);
            }
            try
            {
                // 通知其他用户下线
                StartLogoffUDPBroadcast();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("异常：无法发送登出UDP消息");
                Trace.WriteLine(ex.Message);
            }
            // 通知服务器下线
            TCPLogout();
            // 隐藏图标
            notifyIcon.Visible = false;
            // 保存设置
            saveSettings();
            // 关闭正在使用的聊天窗口
            if (formChats.Count > 0)
            {
                // FormChat.Close()会修改formChats，
                // 所以不能直接枚举formChats
                FormChat[] formChatsToClose =
                    new FormChat[formChats.Count];
                int i = 0;
                foreach (FormChat formChat in formChats.Values)
                {
                    formChatsToClose[i++] = formChat;
                }
                for (i = 0; i < formChatsToClose.Length; i++)
                {
                    try
                    {
                        formChatsToClose[i].Close();
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("异常：无法关闭聊天窗口");
                        Trace.WriteLine(ex.Message);
                    }
                }
            }
            Thread.Sleep(100);
            // 强退，不再等待，终止还未退出的线程
            System.Environment.Exit(0);
        }

        /// <summary>
        /// 点击“登陆”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            // 保存设置
            saveSettings();
            if (!textBoxUser.isEmpty() && !textBoxPw.isEmpty())
            {
                panelLogin.Enabled = false;
                //ConnectToServer();
                StartTCPListening();
                notifyIcon.Text =
                    String.Format("P2P客户端({0})", textBoxUser.Text);
                ProtocalHandler.UserName = textBoxUser.Text;
                StartUDPListen();
                StartLoginUDPBroadcast();
                panelList.Show();
                panelLogin.Fade();
                this.Text = String.Format(
                    "CSP2P - [{0}]", textBoxUser.Text);
            }
        }

        /// <summary>
        /// 在密码输入框中按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxPw_KeyPress(
            object sender, KeyPressEventArgs e)
        {
            // 按下了回车，登陆
            if (e.KeyChar == '\r')
            {
                buttonLogin_Click(this, null);
                e.KeyChar = '\0';
            }
        }

        /// <summary>
        /// 在用户名输入框中按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxUser_KeyPress(
            object sender, KeyPressEventArgs e)
        {
            // 按下了回车，登陆
            if (e.KeyChar == '\r')
            {
                buttonLogin_Click(this, null);
                e.KeyChar = '\0';
            }
        }

        /// <summary>
        /// 窗口尺寸改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            // 最小化时隐藏，只显示在右下角托盘
            if (WindowState == FormWindowState.Minimized)
            {
                Thread.Sleep(200);
                HideToNI();
            }
        }

        /// <summary>
        /// 双击托盘图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (!Visible)
            {
                ShowFromNI();
            }
            else
            {
                Activate();
            }
        }

        /// <summary>
        /// 在托盘右键菜单中点击“显示/隐藏”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemNIShowHide_Click(
            object sender, EventArgs e)
        {
            if (Visible)    // 目前可见
            {
                HideToNI();
            }
            else    // 目前被隐藏
            {
                ShowFromNI();
            }
        }

        /// <summary>
        /// 在托盘右键菜单中点击“退出”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemNIQuit_Click(
            object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 点击“刷新”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUdpBroadcast_Click(
            object sender, EventArgs e)
        {
            // 从服务器检查好友在线状态
            CheckStatusAll();
            // 发送UDP广播
            StartLoginUDPBroadcast();
        }

        /// <summary>
        /// 在好友右键菜单中点击“开始对话”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemFLChat_Click(
            object sender, EventArgs e)
        {
            StartChatting();
        }

        /// <summary>
        /// 双击好友与之聊天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewFriends_DoubleClick(
            object sender, EventArgs e)
        {
            StartChatting();
        }

        /// <summary>
        /// 开始聊天/群聊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStartChatting_Click(
            object sender, EventArgs e)
        {
            StartChatting();
        }

        /// <summary>
        /// 选中的好友发生变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewFriends_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            // 选中的好友人数变化
            if (listViewFriends.SelectedItems.Count == 0)
            {
                buttonStartChatting.Enabled = false;
                toolStripMenuItemFLChat.Enabled = false;
                toolStripMenuItemFLDeleteFriend.Enabled = false;
            }
            else if (listViewFriends.SelectedItems.Count == 1)
            {
                buttonStartChatting.Enabled = true;
                buttonStartChatting.Text = "聊天(&C)";
                toolStripMenuItemFLChat.Enabled = true;
                toolStripMenuItemFLDeleteFriend.Enabled = true;
            }
            else
            {
                buttonStartChatting.Enabled = true;
                buttonStartChatting.Text = "群聊(&C)";
                toolStripMenuItemFLChat.Enabled = true;
                toolStripMenuItemFLDeleteFriend.Enabled = true;
            }
        }

        /// <summary>
        /// 点击“搜索/添加好友”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddFriend_Click(object sender, EventArgs e)
        {
            if (comboBoxFriends.Text != null &&
                !comboBoxFriends.Text.Equals(""))
            {
                // 检查是否已经存在
                foreach (ListViewItem lvi in listViewFriends.Items)
                {
                    // 若已经存在则选中
                    if (comboBoxFriends.Text.Equals(lvi.Text))
                    {
                        foreach (ListViewItem lvis
                            in listViewFriends.SelectedItems)
                        {
                            lvis.Selected = false;
                        }
                        lvi.Selected = true;
                        buttonStartChatting_Click(this, null);
                        return;
                    }
                }
                //AddFriend(comboBoxFriends.Text);
                string[] ipStrings = comboBoxFriends.Text.Split('.');
                if (ipStrings.Length == 4)
                {
                    try
                    {
                        byte[] ipBytes = new byte[4];
                        for (int i = 0; i < 4; i++)
                        {
                            ipBytes[i] = byte.Parse(ipStrings[i]);
                        }
                        IPAddress ipa = new IPAddress(ipBytes);
                        StartLoginUDPBroadcast(ipa);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("异常位置：buttonAddFriend_Click");
                        Trace.WriteLine(ex.Message);
                    }
                }
                // 刷新好友列表
                //buttonUdpBroadcast_Click(this, null);
            }
        }

        /// <summary>
        /// 在好友搜索框内按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxFriends_KeyPress(
            object sender, KeyPressEventArgs e)
        {
            // 按下了回车，发送
            if (e.KeyChar == '\r')
            {
                buttonAddFriend_Click(this, null);
                e.KeyChar = '\0';
            }
        }

        /// <summary>
        /// 点击“删除好友”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemFLDeleteFriend_Click(
            object sender, EventArgs e)
        {
            int count = listViewFriends.SelectedItems.Count;
            if (count <= 0)
            {
                return;
            }
            // 记下需要删除的元素
            ListViewItem[] lvis = new ListViewItem[count];
            for (int i = 0; i < count; i++)
            {
                lvis[i] = listViewFriends.SelectedItems[i];
            }
            for (int i = 0; i < count; i++)
            {
                comboBoxFriends.Items.Remove(lvis[i].Text);
                listViewFriends.Items.Remove(lvis[i]);
            }
                
        }

        /// <summary>
        /// 点击“修改IP”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSetLocalIP_Click(object sender, EventArgs e)
        {
            if (!isAdmin)
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    Arguments = "-asAdmin",
                    ErrorDialog = true,
                    ErrorDialogParentHandle = Handle,
                    FileName = Application.ExecutablePath,
                    Verb = "runas"
                };
                try
                {
                    Process.Start(psi);
                    System.Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n可能无法使用修改IP等功能。");
                }
            }
            formSetLocalIP = new FormSetLocalIP();
            formSetLocalIP.ShowDialog();
        }
    }
}
 