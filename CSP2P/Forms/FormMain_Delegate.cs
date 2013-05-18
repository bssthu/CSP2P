using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// 主窗体使用的委托

namespace CSP2P
{
    public partial class FormMain
    {
        /// <summary>
        /// 刷新好友列表等UI的委托
        /// </summary>
        /// <param name="key"></param>
        public delegate void LogRefreshDelegate(string key);

        /// <summary>
        /// 好友上线刷新UI
        /// </summary>
        public LogRefreshDelegate loginRefreshDelegate;

        /// <summary>
        /// 刷新好友列表等UI的委托，有ping
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ping"></param>
        public delegate void LogRefreshDelegateWithPing(string key, string ping);

        /// <summary>
        /// 好友上线刷新UI，有ping
        /// </summary>
        public LogRefreshDelegateWithPing loginRefreshWithPingDelegate;

        /// <summary>
        /// 好友下线刷新UI
        /// </summary>
        public LogRefreshDelegate logoffRefreshDelegate;

        /// <summary>
        /// 由主窗体打开FormChat的委托
        /// </summary>
        /// <param name="formChat"></param>
        public delegate void ShowFormChatDelegate(FormChat formChat);

        /// <summary>
        /// 由主窗体打开FormChat
        /// </summary>
        public ShowFormChatDelegate showFormChatDelegate;

        /// <summary>
        /// 由主窗体打开FormGroup的委托
        /// </summary>
        /// <param name="formGroup"></param>
        public delegate void ShowFormGroupDelegate(FormGroup formGroup);

        /// <summary>
        /// 由主窗体打开FormGroup
        /// </summary>
        public ShowFormGroupDelegate showFormGroupDelegate;

        /// <summary>
        /// 检查列表中所有好友的在线状态的委托
        /// </summary>
        public delegate void CheckStatusAllDelegate();

        /// <summary>
        /// 检查列表中所有好友的在线状态
        /// </summary>
        public CheckStatusAllDelegate checkStatusAllDelegate;

        /// <summary>
        /// 检查指定好友的在线状态的委托
        /// </summary>
        /// <param name="friends">要检查的指定的好友</param>
        public delegate void CheckStatusStringsDelegate(string[] friends);

        /// <summary>
        /// 检查指定好友的在线状态
        /// </summary>
        public CheckStatusStringsDelegate checkStatusStringsDelegate;


        /// <summary>
        /// 初始化委托
        /// </summary>
        private void initDelegates()
        {
            loginRefreshDelegate = new LogRefreshDelegate(
                OnLoginRefresh);
            loginRefreshWithPingDelegate = new LogRefreshDelegateWithPing(
                OnLoginRefreshWithPing);
            logoffRefreshDelegate = new LogRefreshDelegate(
                OnLogoffRefresh);
            showFormChatDelegate = new ShowFormChatDelegate(OnShowFormChat);
            showFormGroupDelegate = new ShowFormGroupDelegate(OnShowFormGroup);
            checkStatusAllDelegate = new CheckStatusAllDelegate(CheckStatusAll);
            checkStatusStringsDelegate =
                new CheckStatusStringsDelegate(CheckStatusStrings);
        }

        /// <summary>
        /// 好友上线时的UI处理
        /// </summary>
        /// <param name="friendName">好友名</param>
        public void OnLoginRefresh(string friendName)
        {
            // 群聊窗口中
            foreach (FormGroup formGroup in formGroups)
            {
                formGroup.OnLoginRefresh(friendName);
            }
            // 主窗体中
            foreach (ListViewItem lvItem in listViewFriends.Items)
            {
                if (lvItem.Text == friendName)
                {
                    lvItem.ImageIndex = 1;
                    if (formChats.ContainsKey(friendName))
                    {
                        formChats[friendName].Icon = Properties.Resources.chat_on;
                    }
                    return;
                }
            }
            // 列表中没有则添加
            ListViewItem newLvItem = new ListViewItem(friendName);
            newLvItem.ImageIndex = 1;
            listViewFriends.Items.Add(newLvItem);
            if (formChats.ContainsKey(friendName))
            {
                formChats[friendName].Icon = Properties.Resources.chat_on;
            }
        }

        /// <summary>
        /// 好友上线时的UI处理，收到ping
        /// </summary>
        /// <param name="friendName">好友名</param>
        /// <param name="ping">ping字符串</param>
        public void OnLoginRefreshWithPing(string friendName, string ping)
        {
            // 群聊窗口中
            foreach (FormGroup formGroup in formGroups)
            {
                formGroup.OnLoginRefresh(friendName);
            }
            // 主窗体中
            foreach (ListViewItem lvItem in listViewFriends.Items)
            {
                if (lvItem.Text == friendName)
                {
                    lvItem.ImageIndex = 1;
                    if (formChats.ContainsKey(friendName))
                    {
                        formChats[friendName].Icon = Properties.Resources.chat_on;
                    }
                    if (lvItem.SubItems.Count == 1)
                    {
                        lvItem.SubItems.Add(ping);
                    }
                    else
                    {
                        lvItem.SubItems[1].Text = ping;
                    }
                    return;
                }
            }
            // 列表中没有则添加
            ListViewItem newLvItem = new ListViewItem(friendName);
            newLvItem.ImageIndex = 1;
            listViewFriends.Items.Add(newLvItem);
            if (formChats.ContainsKey(friendName))
            {
                formChats[friendName].Icon = Properties.Resources.chat_on;
            }
        }

        /// <summary>
        /// 好友下线时的UI处理
        /// </summary>
        /// <param name="friendName">好友名</param>
        public void OnLogoffRefresh(string friendName)
        {
            // 群聊窗口中
            foreach (FormGroup formGroup in formGroups)
            {
                formGroup.OnLogoffRefresh(friendName);
            }
            // 主窗体中
            if (friendName == textBoxUser.Text)
            {
                return;
            }
            foreach (ListViewItem lvItem in listViewFriends.Items)
            {
                if (lvItem.Text == friendName)
                {
                    lvItem.ImageIndex = 0;
                    if (formChats.ContainsKey(friendName))
                    {
                        formChats[friendName].Icon = Properties.Resources.chat_off;
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 由主窗体来打开指定的FormChat
        /// </summary>
        /// <param name="formChat">要打开的FormChat</param>
        public void OnShowFormChat(FormChat formChat)
        {
            formChat.Show();
        }

        /// <summary>
        /// 由主窗体来打开指定的FormGroup
        /// </summary>
        /// <param name="formGroup">要打开的FormGroup</param>
        public void OnShowFormGroup(FormGroup formGroup)
        {
            formGroup.Show();
        }
    }
}
