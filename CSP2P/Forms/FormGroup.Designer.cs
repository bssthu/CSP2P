namespace CSP2P
{
    partial class FormGroup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGroup));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStripFL = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemFLChat = new System.Windows.Forms.ToolStripMenuItem();
            this.panelGroup = new CSP2P.GradientPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxSend = new System.Windows.Forms.TextBox();
            this.buttonColor = new System.Windows.Forms.Button();
            this.buttonFont = new System.Windows.Forms.Button();
            this.listViewFriends = new System.Windows.Forms.ListView();
            this.richTextBoxRcv = new System.Windows.Forms.RichTextBox();
            this.contextMenuStripFL.SuspendLayout();
            this.panelGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "client_off.ico");
            this.imageList.Images.SetKeyName(1, "client_on.ico");
            // 
            // contextMenuStripFL
            // 
            this.contextMenuStripFL.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFLChat});
            this.contextMenuStripFL.Name = "contextMenuStripFL";
            this.contextMenuStripFL.Size = new System.Drawing.Size(115, 26);
            // 
            // toolStripMenuItemFLChat
            // 
            this.toolStripMenuItemFLChat.Name = "toolStripMenuItemFLChat";
            this.toolStripMenuItemFLChat.Size = new System.Drawing.Size(114, 22);
            this.toolStripMenuItemFLChat.Text = "私聊(&C)";
            this.toolStripMenuItemFLChat.Click += new System.EventHandler(this.toolStripMenuItemFLChat_Click);
            // 
            // panelGroup
            // 
            this.panelGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGroup.Color1 = System.Drawing.Color.RoyalBlue;
            this.panelGroup.Color2 = System.Drawing.Color.AliceBlue;
            this.panelGroup.Controls.Add(this.label1);
            this.panelGroup.Controls.Add(this.buttonClose);
            this.panelGroup.Controls.Add(this.buttonSend);
            this.panelGroup.Controls.Add(this.textBoxSend);
            this.panelGroup.Controls.Add(this.buttonColor);
            this.panelGroup.Controls.Add(this.buttonFont);
            this.panelGroup.Controls.Add(this.listViewFriends);
            this.panelGroup.Controls.Add(this.richTextBoxRcv);
            this.panelGroup.LinearGradientAngle = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelGroup.Location = new System.Drawing.Point(0, 0);
            this.panelGroup.Name = "panelGroup";
            this.panelGroup.Size = new System.Drawing.Size(503, 414);
            this.panelGroup.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(345, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 21);
            this.label1.TabIndex = 13;
            this.label1.Text = "好友列表";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(153, 375);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(90, 26);
            this.buttonClose.TabIndex = 12;
            this.buttonClose.Text = "关闭(&C)";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.Location = new System.Drawing.Point(249, 375);
            this.buttonSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(90, 26);
            this.buttonSend.TabIndex = 11;
            this.buttonSend.Text = "发送(&S)";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textBoxSend
            // 
            this.textBoxSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSend.Location = new System.Drawing.Point(3, 257);
            this.textBoxSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxSend.MaxLength = 1000;
            this.textBoxSend.Multiline = true;
            this.textBoxSend.Name = "textBoxSend";
            this.textBoxSend.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxSend.Size = new System.Drawing.Size(336, 110);
            this.textBoxSend.TabIndex = 10;
            this.textBoxSend.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSend_KeyPress);
            // 
            // buttonColor
            // 
            this.buttonColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonColor.BackColor = System.Drawing.Color.Transparent;
            this.buttonColor.FlatAppearance.BorderSize = 0;
            this.buttonColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonColor.Image = global::CSP2P.Properties.Resources.color;
            this.buttonColor.Location = new System.Drawing.Point(41, 217);
            this.buttonColor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonColor.Name = "buttonColor";
            this.buttonColor.Size = new System.Drawing.Size(32, 32);
            this.buttonColor.TabIndex = 9;
            this.buttonColor.UseVisualStyleBackColor = false;
            this.buttonColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // buttonFont
            // 
            this.buttonFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonFont.BackColor = System.Drawing.Color.Transparent;
            this.buttonFont.FlatAppearance.BorderSize = 0;
            this.buttonFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFont.Image = global::CSP2P.Properties.Resources.font;
            this.buttonFont.Location = new System.Drawing.Point(3, 217);
            this.buttonFont.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonFont.Name = "buttonFont";
            this.buttonFont.Size = new System.Drawing.Size(32, 32);
            this.buttonFont.TabIndex = 8;
            this.buttonFont.UseVisualStyleBackColor = false;
            this.buttonFont.Click += new System.EventHandler(this.buttonFont_Click);
            // 
            // listViewFriends
            // 
            this.listViewFriends.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewFriends.ContextMenuStrip = this.contextMenuStripFL;
            this.listViewFriends.FullRowSelect = true;
            this.listViewFriends.GridLines = true;
            this.listViewFriends.LargeImageList = this.imageList;
            this.listViewFriends.Location = new System.Drawing.Point(345, 28);
            this.listViewFriends.MultiSelect = false;
            this.listViewFriends.Name = "listViewFriends";
            this.listViewFriends.Size = new System.Drawing.Size(155, 383);
            this.listViewFriends.SmallImageList = this.imageList;
            this.listViewFriends.TabIndex = 7;
            this.listViewFriends.UseCompatibleStateImageBehavior = false;
            this.listViewFriends.View = System.Windows.Forms.View.SmallIcon;
            this.listViewFriends.DoubleClick += new System.EventHandler(this.listViewFriends_DoubleClick);
            // 
            // richTextBoxRcv
            // 
            this.richTextBoxRcv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxRcv.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxRcv.Location = new System.Drawing.Point(3, 4);
            this.richTextBoxRcv.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.richTextBoxRcv.Name = "richTextBoxRcv";
            this.richTextBoxRcv.ReadOnly = true;
            this.richTextBoxRcv.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxRcv.Size = new System.Drawing.Size(336, 205);
            this.richTextBoxRcv.TabIndex = 6;
            this.richTextBoxRcv.Text = "";
            // 
            // FormGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 414);
            this.Controls.Add(this.panelGroup);
            this.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "FormGroup";
            this.Text = "群聊";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGroup_FormClosing);
            this.contextMenuStripFL.ResumeLayout(false);
            this.panelGroup.ResumeLayout(false);
            this.panelGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GradientPanel panelGroup;
        private System.Windows.Forms.RichTextBox richTextBoxRcv;
        private System.Windows.Forms.ListView listViewFriends;
        private System.Windows.Forms.Button buttonColor;
        private System.Windows.Forms.Button buttonFont;
        private System.Windows.Forms.TextBox textBoxSend;
        private System.Windows.Forms.Button buttonClose;
        internal System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFL;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFLChat;
    }
}