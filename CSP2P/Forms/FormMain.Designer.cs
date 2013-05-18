namespace CSP2P
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripNI = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemNIShowHide = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemNISpilt1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemNIQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripFL = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemFLChat = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFLDeleteFriend = new System.Windows.Forms.ToolStripMenuItem();
            this.panelList = new CSP2P.GradientPanel();
            this.buttonAddFriend = new System.Windows.Forms.Button();
            this.comboBoxFriends = new System.Windows.Forms.ComboBox();
            this.buttonStartChatting = new System.Windows.Forms.Button();
            this.buttonUdpBroadcast = new System.Windows.Forms.Button();
            this.listViewFriends = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPing = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelLogin = new CSP2P.GradientPanel();
            this.checkBoxRemUser = new System.Windows.Forms.CheckBox();
            this.buttonSetLocalIP = new System.Windows.Forms.Button();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.textBoxUser = new CSP2P.DefaultInputTextBox();
            this.textBoxPw = new CSP2P.DefaultInputTextBox();
            this.pictureBoxCloud = new System.Windows.Forms.PictureBox();
            this.contextMenuStripNI.SuspendLayout();
            this.contextMenuStripFL.SuspendLayout();
            this.panelList.SuspendLayout();
            this.panelLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCloud)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "client_off.ico");
            this.imageList.Images.SetKeyName(1, "client_on.ico");
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStripNI;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "P2P客户端";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // contextMenuStripNI
            // 
            this.contextMenuStripNI.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemNIShowHide,
            this.toolStripMenuItemNISpilt1,
            this.toolStripMenuItemNIQuit});
            this.contextMenuStripNI.Name = "contextMenuStripNotifyIcon";
            this.contextMenuStripNI.Size = new System.Drawing.Size(119, 54);
            // 
            // toolStripMenuItemNIShowHide
            // 
            this.toolStripMenuItemNIShowHide.Name = "toolStripMenuItemNIShowHide";
            this.toolStripMenuItemNIShowHide.Size = new System.Drawing.Size(118, 22);
            this.toolStripMenuItemNIShowHide.Text = "隐藏(&H)";
            this.toolStripMenuItemNIShowHide.Click += new System.EventHandler(this.toolStripMenuItemNIShowHide_Click);
            // 
            // toolStripMenuItemNISpilt1
            // 
            this.toolStripMenuItemNISpilt1.Name = "toolStripMenuItemNISpilt1";
            this.toolStripMenuItemNISpilt1.Size = new System.Drawing.Size(115, 6);
            // 
            // toolStripMenuItemNIQuit
            // 
            this.toolStripMenuItemNIQuit.Name = "toolStripMenuItemNIQuit";
            this.toolStripMenuItemNIQuit.Size = new System.Drawing.Size(118, 22);
            this.toolStripMenuItemNIQuit.Text = "退出(&Q)";
            this.toolStripMenuItemNIQuit.Click += new System.EventHandler(this.toolStripMenuItemNIQuit_Click);
            // 
            // contextMenuStripFL
            // 
            this.contextMenuStripFL.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFLChat,
            this.toolStripMenuItemFLDeleteFriend});
            this.contextMenuStripFL.Name = "contextMenuStripFL";
            this.contextMenuStripFL.Size = new System.Drawing.Size(152, 48);
            // 
            // toolStripMenuItemFLChat
            // 
            this.toolStripMenuItemFLChat.Enabled = false;
            this.toolStripMenuItemFLChat.Name = "toolStripMenuItemFLChat";
            this.toolStripMenuItemFLChat.Size = new System.Drawing.Size(151, 22);
            this.toolStripMenuItemFLChat.Text = "开始对话(&C)";
            this.toolStripMenuItemFLChat.Click += new System.EventHandler(this.toolStripMenuItemFLChat_Click);
            // 
            // toolStripMenuItemFLDeleteFriend
            // 
            this.toolStripMenuItemFLDeleteFriend.Enabled = false;
            this.toolStripMenuItemFLDeleteFriend.Name = "toolStripMenuItemFLDeleteFriend";
            this.toolStripMenuItemFLDeleteFriend.Size = new System.Drawing.Size(151, 22);
            this.toolStripMenuItemFLDeleteFriend.Text = "删除好友(&Del)";
            this.toolStripMenuItemFLDeleteFriend.Click += new System.EventHandler(this.toolStripMenuItemFLDeleteFriend_Click);
            // 
            // panelList
            // 
            this.panelList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelList.Color1 = System.Drawing.Color.RoyalBlue;
            this.panelList.Color2 = System.Drawing.Color.AliceBlue;
            this.panelList.Controls.Add(this.buttonAddFriend);
            this.panelList.Controls.Add(this.comboBoxFriends);
            this.panelList.Controls.Add(this.buttonStartChatting);
            this.panelList.Controls.Add(this.buttonUdpBroadcast);
            this.panelList.Controls.Add(this.listViewFriends);
            this.panelList.LinearGradientAngle = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelList.Location = new System.Drawing.Point(0, 0);
            this.panelList.Name = "panelList";
            this.panelList.Size = new System.Drawing.Size(309, 530);
            this.panelList.TabIndex = 1;
            this.panelList.Visible = false;
            // 
            // buttonAddFriend
            // 
            this.buttonAddFriend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddFriend.BackColor = System.Drawing.Color.Transparent;
            this.buttonAddFriend.FlatAppearance.BorderSize = 0;
            this.buttonAddFriend.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gold;
            this.buttonAddFriend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddFriend.Image = ((System.Drawing.Image)(resources.GetObject("buttonAddFriend.Image")));
            this.buttonAddFriend.Location = new System.Drawing.Point(265, 4);
            this.buttonAddFriend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonAddFriend.Name = "buttonAddFriend";
            this.buttonAddFriend.Size = new System.Drawing.Size(32, 32);
            this.buttonAddFriend.TabIndex = 4;
            this.buttonAddFriend.UseVisualStyleBackColor = false;
            this.buttonAddFriend.Click += new System.EventHandler(this.buttonAddFriend_Click);
            // 
            // comboBoxFriends
            // 
            this.comboBoxFriends.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFriends.FormattingEnabled = true;
            this.comboBoxFriends.Location = new System.Drawing.Point(12, 9);
            this.comboBoxFriends.Name = "comboBoxFriends";
            this.comboBoxFriends.Size = new System.Drawing.Size(247, 24);
            this.comboBoxFriends.TabIndex = 2;
            this.comboBoxFriends.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxFriends_KeyPress);
            // 
            // buttonStartChatting
            // 
            this.buttonStartChatting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStartChatting.Enabled = false;
            this.buttonStartChatting.Location = new System.Drawing.Point(207, 472);
            this.buttonStartChatting.Name = "buttonStartChatting";
            this.buttonStartChatting.Size = new System.Drawing.Size(90, 28);
            this.buttonStartChatting.TabIndex = 1;
            this.buttonStartChatting.Text = "聊天(&C)";
            this.buttonStartChatting.UseVisualStyleBackColor = true;
            this.buttonStartChatting.Click += new System.EventHandler(this.buttonStartChatting_Click);
            // 
            // buttonUdpBroadcast
            // 
            this.buttonUdpBroadcast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUdpBroadcast.Location = new System.Drawing.Point(12, 472);
            this.buttonUdpBroadcast.Name = "buttonUdpBroadcast";
            this.buttonUdpBroadcast.Size = new System.Drawing.Size(90, 28);
            this.buttonUdpBroadcast.TabIndex = 1;
            this.buttonUdpBroadcast.Text = "刷新(&R)";
            this.buttonUdpBroadcast.UseVisualStyleBackColor = true;
            this.buttonUdpBroadcast.Click += new System.EventHandler(this.buttonUdpBroadcast_Click);
            // 
            // listViewFriends
            // 
            this.listViewFriends.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewFriends.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderPing});
            this.listViewFriends.ContextMenuStrip = this.contextMenuStripFL;
            this.listViewFriends.FullRowSelect = true;
            this.listViewFriends.GridLines = true;
            this.listViewFriends.LargeImageList = this.imageList;
            this.listViewFriends.Location = new System.Drawing.Point(12, 43);
            this.listViewFriends.Name = "listViewFriends";
            this.listViewFriends.Size = new System.Drawing.Size(285, 414);
            this.listViewFriends.SmallImageList = this.imageList;
            this.listViewFriends.TabIndex = 0;
            this.listViewFriends.UseCompatibleStateImageBehavior = false;
            this.listViewFriends.View = System.Windows.Forms.View.Details;
            this.listViewFriends.SelectedIndexChanged += new System.EventHandler(this.listViewFriends_SelectedIndexChanged);
            this.listViewFriends.DoubleClick += new System.EventHandler(this.listViewFriends_DoubleClick);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "用户名";
            this.columnHeaderName.Width = 200;
            // 
            // columnHeaderPing
            // 
            this.columnHeaderPing.Text = "Ping";
            // 
            // panelLogin
            // 
            this.panelLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelLogin.Color1 = System.Drawing.Color.RoyalBlue;
            this.panelLogin.Color2 = System.Drawing.Color.AliceBlue;
            this.panelLogin.Controls.Add(this.checkBoxRemUser);
            this.panelLogin.Controls.Add(this.buttonSetLocalIP);
            this.panelLogin.Controls.Add(this.buttonLogin);
            this.panelLogin.Controls.Add(this.textBoxUser);
            this.panelLogin.Controls.Add(this.textBoxPw);
            this.panelLogin.Controls.Add(this.pictureBoxCloud);
            this.panelLogin.LinearGradientAngle = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelLogin.Location = new System.Drawing.Point(0, 0);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(309, 530);
            this.panelLogin.TabIndex = 0;
            // 
            // checkBoxRemUser
            // 
            this.checkBoxRemUser.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.checkBoxRemUser.AutoSize = true;
            this.checkBoxRemUser.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxRemUser.Enabled = false;
            this.checkBoxRemUser.Location = new System.Drawing.Point(54, 270);
            this.checkBoxRemUser.Name = "checkBoxRemUser";
            this.checkBoxRemUser.Size = new System.Drawing.Size(83, 20);
            this.checkBoxRemUser.TabIndex = 4;
            this.checkBoxRemUser.Text = "记住学号";
            this.checkBoxRemUser.UseVisualStyleBackColor = false;
            // 
            // buttonSetLocalIP
            // 
            this.buttonSetLocalIP.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonSetLocalIP.Location = new System.Drawing.Point(165, 309);
            this.buttonSetLocalIP.Name = "buttonSetLocalIP";
            this.buttonSetLocalIP.Size = new System.Drawing.Size(90, 28);
            this.buttonSetLocalIP.TabIndex = 1;
            this.buttonSetLocalIP.Text = "修改IP(&I)";
            this.buttonSetLocalIP.UseVisualStyleBackColor = false;
            this.buttonSetLocalIP.Click += new System.EventHandler(this.buttonSetLocalIP_Click);
            // 
            // buttonLogin
            // 
            this.buttonLogin.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonLogin.Location = new System.Drawing.Point(54, 309);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(90, 28);
            this.buttonLogin.TabIndex = 1;
            this.buttonLogin.Text = "登陆(&L)";
            this.buttonLogin.UseVisualStyleBackColor = false;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // textBoxUser
            // 
            this.textBoxUser.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textBoxUser.ForeColor = System.Drawing.SystemColors.GrayText;
            this.textBoxUser.Location = new System.Drawing.Point(54, 163);
            this.textBoxUser.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Note = "请输入学号";
            this.textBoxUser.SavedPasswordChar = '\0';
            this.textBoxUser.Size = new System.Drawing.Size(201, 24);
            this.textBoxUser.TabIndex = 2;
            this.textBoxUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxUser_KeyPress);
            // 
            // textBoxPw
            // 
            this.textBoxPw.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textBoxPw.ForeColor = System.Drawing.SystemColors.GrayText;
            this.textBoxPw.Location = new System.Drawing.Point(54, 212);
            this.textBoxPw.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPw.Name = "textBoxPw";
            this.textBoxPw.Note = "请输入密码";
            this.textBoxPw.PasswordChar = '*';
            this.textBoxPw.SavedPasswordChar = '*';
            this.textBoxPw.Size = new System.Drawing.Size(201, 24);
            this.textBoxPw.TabIndex = 3;
            this.textBoxPw.Text = "net2012";
            this.textBoxPw.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPw_KeyPress);
            // 
            // pictureBoxCloud
            // 
            this.pictureBoxCloud.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBoxCloud.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxCloud.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBoxCloud.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxCloud.Image")));
            this.pictureBoxCloud.Location = new System.Drawing.Point(32, 12);
            this.pictureBoxCloud.Name = "pictureBoxCloud";
            this.pictureBoxCloud.Size = new System.Drawing.Size(245, 128);
            this.pictureBoxCloud.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxCloud.TabIndex = 1;
            this.pictureBoxCloud.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 530);
            this.Controls.Add(this.panelLogin);
            this.Controls.Add(this.panelList);
            this.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(283, 401);
            this.Name = "FormMain";
            this.Text = "CSP2P";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
            this.contextMenuStripNI.ResumeLayout(false);
            this.contextMenuStripFL.ResumeLayout(false);
            this.panelList.ResumeLayout(false);
            this.panelLogin.ResumeLayout(false);
            this.panelLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCloud)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GradientPanel panelLogin;
        private DefaultInputTextBox textBoxPw;
        private DefaultInputTextBox textBoxUser;
        private System.Windows.Forms.PictureBox pictureBoxCloud;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.CheckBox checkBoxRemUser;
        private GradientPanel panelList;
        private System.Windows.Forms.ListView listViewFriends;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button buttonUdpBroadcast;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripNI;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNIShowHide;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItemNISpilt1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNIQuit;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFL;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFLChat;
        private System.Windows.Forms.Button buttonStartChatting;
        private System.Windows.Forms.ComboBox comboBoxFriends;
        private System.Windows.Forms.Button buttonAddFriend;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFLDeleteFriend;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderPing;
        private System.Windows.Forms.Button buttonSetLocalIP;
    }
}

