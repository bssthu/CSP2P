namespace CSP2P
{
    partial class FormChat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChat));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panelChat = new CSP2P.GradientPanel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.buttonFile = new System.Windows.Forms.Button();
            this.buttonColor = new System.Windows.Forms.Button();
            this.buttonFont = new System.Windows.Forms.Button();
            this.textBoxSend = new System.Windows.Forms.TextBox();
            this.richTextBoxRcv = new System.Windows.Forms.RichTextBox();
            this.panelFile = new CSP2P.GradientPanel();
            this.buttonCancelFile = new System.Windows.Forms.Button();
            this.buttonRcvFile = new System.Windows.Forms.Button();
            this.progressBarFile = new System.Windows.Forms.ProgressBar();
            this.labelFileStatus = new System.Windows.Forms.Label();
            this.labelFileName = new System.Windows.Forms.Label();
            this.panelChat.SuspendLayout();
            this.panelFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "font.ico");
            this.imageList.Images.SetKeyName(1, "color.ico");
            this.imageList.Images.SetKeyName(2, "send.ico");
            // 
            // panelChat
            // 
            this.panelChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelChat.Color1 = System.Drawing.Color.RoyalBlue;
            this.panelChat.Color2 = System.Drawing.Color.AliceBlue;
            this.panelChat.Controls.Add(this.buttonClose);
            this.panelChat.Controls.Add(this.buttonSend);
            this.panelChat.Controls.Add(this.buttonFile);
            this.panelChat.Controls.Add(this.buttonColor);
            this.panelChat.Controls.Add(this.buttonFont);
            this.panelChat.Controls.Add(this.textBoxSend);
            this.panelChat.Controls.Add(this.richTextBoxRcv);
            this.panelChat.LinearGradientAngle = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelChat.Location = new System.Drawing.Point(0, 0);
            this.panelChat.MinimumSize = new System.Drawing.Size(200, 0);
            this.panelChat.Name = "panelChat";
            this.panelChat.Size = new System.Drawing.Size(297, 436);
            this.panelChat.TabIndex = 0;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(108, 406);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(90, 26);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "关闭(&C)";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.Location = new System.Drawing.Point(204, 406);
            this.buttonSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(90, 26);
            this.buttonSend.TabIndex = 1;
            this.buttonSend.Text = "发送(&S)";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // buttonFile
            // 
            this.buttonFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonFile.BackColor = System.Drawing.Color.Transparent;
            this.buttonFile.FlatAppearance.BorderSize = 0;
            this.buttonFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFile.Image = global::CSP2P.Properties.Resources.send;
            this.buttonFile.Location = new System.Drawing.Point(79, 244);
            this.buttonFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonFile.Name = "buttonFile";
            this.buttonFile.Size = new System.Drawing.Size(32, 32);
            this.buttonFile.TabIndex = 4;
            this.buttonFile.UseVisualStyleBackColor = false;
            this.buttonFile.Click += new System.EventHandler(this.buttonFile_Click);
            // 
            // buttonColor
            // 
            this.buttonColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonColor.BackColor = System.Drawing.Color.Transparent;
            this.buttonColor.FlatAppearance.BorderSize = 0;
            this.buttonColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonColor.Image = global::CSP2P.Properties.Resources.color;
            this.buttonColor.Location = new System.Drawing.Point(41, 244);
            this.buttonColor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonColor.Name = "buttonColor";
            this.buttonColor.Size = new System.Drawing.Size(32, 32);
            this.buttonColor.TabIndex = 4;
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
            this.buttonFont.Location = new System.Drawing.Point(3, 244);
            this.buttonFont.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonFont.Name = "buttonFont";
            this.buttonFont.Size = new System.Drawing.Size(32, 32);
            this.buttonFont.TabIndex = 3;
            this.buttonFont.UseVisualStyleBackColor = false;
            this.buttonFont.Click += new System.EventHandler(this.buttonFont_Click);
            // 
            // textBoxSend
            // 
            this.textBoxSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSend.Location = new System.Drawing.Point(3, 284);
            this.textBoxSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxSend.MaxLength = 1000;
            this.textBoxSend.Multiline = true;
            this.textBoxSend.Name = "textBoxSend";
            this.textBoxSend.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxSend.Size = new System.Drawing.Size(291, 114);
            this.textBoxSend.TabIndex = 0;
            this.textBoxSend.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSend_KeyPress);
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
            this.richTextBoxRcv.Size = new System.Drawing.Size(291, 232);
            this.richTextBoxRcv.TabIndex = 5;
            this.richTextBoxRcv.Text = "";
            // 
            // panelFile
            // 
            this.panelFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFile.Color1 = System.Drawing.Color.RoyalBlue;
            this.panelFile.Color2 = System.Drawing.Color.AliceBlue;
            this.panelFile.Controls.Add(this.buttonCancelFile);
            this.panelFile.Controls.Add(this.buttonRcvFile);
            this.panelFile.Controls.Add(this.progressBarFile);
            this.panelFile.Controls.Add(this.labelFileStatus);
            this.panelFile.Controls.Add(this.labelFileName);
            this.panelFile.LinearGradientAngle = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelFile.Location = new System.Drawing.Point(97, 0);
            this.panelFile.MinimumSize = new System.Drawing.Size(200, 0);
            this.panelFile.Name = "panelFile";
            this.panelFile.Size = new System.Drawing.Size(200, 436);
            this.panelFile.TabIndex = 6;
            // 
            // buttonCancelFile
            // 
            this.buttonCancelFile.Location = new System.Drawing.Point(98, 57);
            this.buttonCancelFile.Name = "buttonCancelFile";
            this.buttonCancelFile.Size = new System.Drawing.Size(90, 26);
            this.buttonCancelFile.TabIndex = 10;
            this.buttonCancelFile.Text = "取消";
            this.buttonCancelFile.UseVisualStyleBackColor = true;
            this.buttonCancelFile.Click += new System.EventHandler(this.buttonCancelFile_Click);
            // 
            // buttonRcvFile
            // 
            this.buttonRcvFile.Location = new System.Drawing.Point(3, 57);
            this.buttonRcvFile.Name = "buttonRcvFile";
            this.buttonRcvFile.Size = new System.Drawing.Size(90, 26);
            this.buttonRcvFile.TabIndex = 9;
            this.buttonRcvFile.Text = "接受";
            this.buttonRcvFile.UseVisualStyleBackColor = true;
            this.buttonRcvFile.Click += new System.EventHandler(this.buttonRcvFile_Click);
            // 
            // progressBarFile
            // 
            this.progressBarFile.Location = new System.Drawing.Point(3, 28);
            this.progressBarFile.Name = "progressBarFile";
            this.progressBarFile.Size = new System.Drawing.Size(185, 23);
            this.progressBarFile.TabIndex = 8;
            // 
            // labelFileStatus
            // 
            this.labelFileStatus.AutoSize = true;
            this.labelFileStatus.BackColor = System.Drawing.Color.Transparent;
            this.labelFileStatus.Location = new System.Drawing.Point(3, 86);
            this.labelFileStatus.Name = "labelFileStatus";
            this.labelFileStatus.Size = new System.Drawing.Size(64, 16);
            this.labelFileStatus.TabIndex = 7;
            this.labelFileStatus.Text = "准备传送";
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.BackColor = System.Drawing.Color.Transparent;
            this.labelFileName.Location = new System.Drawing.Point(3, 9);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(50, 16);
            this.labelFileName.TabIndex = 7;
            this.labelFileName.Text = "文件名";
            // 
            // FormChat
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 436);
            this.Controls.Add(this.panelChat);
            this.Controls.Add(this.panelFile);
            this.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(300, 400);
            this.Name = "FormChat";
            this.Text = "聊天窗口";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormChat_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormChat_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormChat_DragEnter);
            this.panelChat.ResumeLayout(false);
            this.panelChat.PerformLayout();
            this.panelFile.ResumeLayout(false);
            this.panelFile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GradientPanel panelChat;
        private GradientPanel panelFile;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonColor;
        private System.Windows.Forms.Button buttonFont;
        private System.Windows.Forms.TextBox textBoxSend;
        private System.Windows.Forms.RichTextBox richTextBoxRcv;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button buttonFile;
        internal System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.ProgressBar progressBarFile;
        private System.Windows.Forms.Button buttonCancelFile;
        private System.Windows.Forms.Button buttonRcvFile;
        private System.Windows.Forms.Label labelFileStatus;

    }
}