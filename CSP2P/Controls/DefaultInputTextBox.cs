using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

// 增加新功能的控件
// 输入框，没有内容时可以灰色显示提示信息，如“请输入XXX”

namespace CSP2P
{
    // 用于输入信息，在原有基础上增加默认值功能
    public class DefaultInputTextBox : TextBox
    {
        /// <summary>
        /// 不使用时显示的文字
        /// </summary>
        private String _Note = "";

        /// <summary>
        /// 密码字符
        /// </summary>
        private char _SavedPasswordChar = '\0';

        /// <summary>
        /// 密码字符
        /// </summary>
        public char SavedPasswordChar
        {
            get
            {
                return _SavedPasswordChar;
            }
            set
            {
                _SavedPasswordChar = value;
                PasswordChar = value;
            }
        }

        /// <summary>
        /// 正在显示提示消息
        /// </summary>
        private bool _NoteShowing = false;

        /// <summary>
        /// 不使用时显示的文字
        /// </summary>
        public String Note
        {
            get
            {
                return _Note;
            }
            set
            {
                _Note = value;
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            if (_NoteShowing && Text == _Note)
            {
                ForeColor = System.Drawing.SystemColors.WindowText;
                Text = "";
                PasswordChar = SavedPasswordChar;
                _NoteShowing = false;
            }
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            NoteInactive();
            base.OnLeave(e);
        }

        /// <summary>
        /// 不活动时对Note的处理，在为空时显示Note
        /// </summary>
        public void NoteInactive()
        {
            if (Text == "" && _Note != "" && Text != _Note)
            {
                ForeColor = System.Drawing.SystemColors.GrayText;
                Text = _Note;
                PasswordChar = '\0';
                _NoteShowing = true;
            }
            else
            {
                ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        /// <summary>
        /// 判断输入框是否有值
        /// </summary>
        /// <returns></returns>
        public bool isEmpty()
        {
            if (_NoteShowing || Text == "" || Text == null)
            {
                return true;
            }
            return false;
        }
    }
}
