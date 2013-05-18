using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSP2P
{
    public partial class FormGroup
    {
        /// <summary>
        /// 移除一个client的委托
        /// </summary>
        /// <param name="targetName">要移除的好友名</param>
        public delegate void RemoveClientDelegate(string targetName);

        /// <summary>
        /// 移除一个client
        /// </summary>
        public RemoveClientDelegate removeClientDelegate;

        /// <summary>
        /// 发送RTF文本的委托
        /// </summary>
        /// <param name="rtf"></param>
        public delegate void SendRTFDelegate(string rtf);

        /// <summary>
        /// 发送RTF文本
        /// </summary>
        public SendRTFDelegate sendRTFDelegate;

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
        /// 初始化委托
        /// </summary>
        private void initDelegates()
        {
            removeClientDelegate = new RemoveClientDelegate(onRemoveClient);
            receiveRTFDelegate = new ReceiveRTFDelegate(onReceiveRTF);
            sendRTFDelegate = new SendRTFDelegate(sendRtfText);
        }

        /// <summary>
        /// 移除一个client
        /// </summary>
        /// <param name="targetName">要移除的好友名</param>
        private void onRemoveClient(string targetName)
        {
            clients.Remove(targetName);
        }

        /// <summary>
        /// 好友上线时的UI处理
        /// </summary>
        /// <param name="friendName">好友名</param>
        public void OnLoginRefresh(string friendName)
        {
            foreach (ListViewItem lvItem in listViewFriends.Items)
            {
                if (lvItem.Text == friendName)
                {
                    lvItem.ImageIndex = 1;
                    break;
                }
            }
            // 列表中没有也不添加
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
        /// 好友下线时的UI处理
        /// </summary>
        /// <param name="friendName">好友名</param>
        public void OnLogoffRefresh(string friendName)
        {
            foreach (ListViewItem lvItem in listViewFriends.Items)
            {
                if (lvItem.Text == friendName)
                {
                    lvItem.ImageIndex = 0;
                    break;
                }
            }
        }
    }
}
