namespace CSP2P
{
    partial class FormSetLocalIP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetLocalIP));
            this.panelIP = new CSP2P.GradientPanel();
            this.comboBoxSettings = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelDefaultGateway = new System.Windows.Forms.Label();
            this.maskedTextBoxDefaultGateway = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxSubnetMask = new System.Windows.Forms.MaskedTextBox();
            this.labelSubnetMask = new System.Windows.Forms.Label();
            this.labelIPAddress = new System.Windows.Forms.Label();
            this.maskedTextBoxIPAddress = new System.Windows.Forms.MaskedTextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.comboBoxNIC = new System.Windows.Forms.ComboBox();
            this.panelIP.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelIP
            // 
            this.panelIP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelIP.Color1 = System.Drawing.Color.RoyalBlue;
            this.panelIP.Color2 = System.Drawing.Color.AliceBlue;
            this.panelIP.Controls.Add(this.comboBoxSettings);
            this.panelIP.Controls.Add(this.buttonCancel);
            this.panelIP.Controls.Add(this.buttonApply);
            this.panelIP.Controls.Add(this.buttonOK);
            this.panelIP.Controls.Add(this.labelDefaultGateway);
            this.panelIP.Controls.Add(this.maskedTextBoxDefaultGateway);
            this.panelIP.Controls.Add(this.maskedTextBoxSubnetMask);
            this.panelIP.Controls.Add(this.labelSubnetMask);
            this.panelIP.Controls.Add(this.labelIPAddress);
            this.panelIP.Controls.Add(this.maskedTextBoxIPAddress);
            this.panelIP.Controls.Add(this.buttonAdd);
            this.panelIP.Controls.Add(this.buttonDelete);
            this.panelIP.Controls.Add(this.comboBoxNIC);
            this.panelIP.LinearGradientAngle = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelIP.Location = new System.Drawing.Point(0, 0);
            this.panelIP.Margin = new System.Windows.Forms.Padding(4);
            this.panelIP.Name = "panelIP";
            this.panelIP.Size = new System.Drawing.Size(284, 225);
            this.panelIP.TabIndex = 0;
            // 
            // comboBoxSettings
            // 
            this.comboBoxSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSettings.FormattingEnabled = true;
            this.comboBoxSettings.Location = new System.Drawing.Point(13, 48);
            this.comboBoxSettings.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxSettings.Name = "comboBoxSettings";
            this.comboBoxSettings.Size = new System.Drawing.Size(155, 24);
            this.comboBoxSettings.TabIndex = 2;
            this.comboBoxSettings.TextChanged += new System.EventHandler(this.comboBoxSettings_TextChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCancel.Location = new System.Drawing.Point(109, 185);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(66, 28);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "取消(&C)";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.Location = new System.Drawing.Point(205, 185);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(66, 28);
            this.buttonApply.TabIndex = 8;
            this.buttonApply.Text = "应用(&A)";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOK.Location = new System.Drawing.Point(13, 185);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(66, 28);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "确定(&O)";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelDefaultGateway
            // 
            this.labelDefaultGateway.AutoSize = true;
            this.labelDefaultGateway.BackColor = System.Drawing.Color.Transparent;
            this.labelDefaultGateway.Location = new System.Drawing.Point(10, 157);
            this.labelDefaultGateway.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDefaultGateway.Name = "labelDefaultGateway";
            this.labelDefaultGateway.Size = new System.Drawing.Size(68, 16);
            this.labelDefaultGateway.TabIndex = 13;
            this.labelDefaultGateway.Text = "默认网关:";
            // 
            // maskedTextBoxDefaultGateway
            // 
            this.maskedTextBoxDefaultGateway.Location = new System.Drawing.Point(152, 154);
            this.maskedTextBoxDefaultGateway.Margin = new System.Windows.Forms.Padding(4);
            this.maskedTextBoxDefaultGateway.Mask = "000.000.000.000";
            this.maskedTextBoxDefaultGateway.Name = "maskedTextBoxDefaultGateway";
            this.maskedTextBoxDefaultGateway.Size = new System.Drawing.Size(121, 24);
            this.maskedTextBoxDefaultGateway.TabIndex = 5;
            this.maskedTextBoxDefaultGateway.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // maskedTextBoxSubnetMask
            // 
            this.maskedTextBoxSubnetMask.Location = new System.Drawing.Point(152, 122);
            this.maskedTextBoxSubnetMask.Margin = new System.Windows.Forms.Padding(4);
            this.maskedTextBoxSubnetMask.Mask = "000.000.000.000";
            this.maskedTextBoxSubnetMask.Name = "maskedTextBoxSubnetMask";
            this.maskedTextBoxSubnetMask.Size = new System.Drawing.Size(121, 24);
            this.maskedTextBoxSubnetMask.TabIndex = 4;
            this.maskedTextBoxSubnetMask.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelSubnetMask
            // 
            this.labelSubnetMask.AutoSize = true;
            this.labelSubnetMask.BackColor = System.Drawing.Color.Transparent;
            this.labelSubnetMask.Location = new System.Drawing.Point(10, 125);
            this.labelSubnetMask.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSubnetMask.Name = "labelSubnetMask";
            this.labelSubnetMask.Size = new System.Drawing.Size(68, 16);
            this.labelSubnetMask.TabIndex = 12;
            this.labelSubnetMask.Text = "子网掩码:";
            // 
            // labelIPAddress
            // 
            this.labelIPAddress.AutoSize = true;
            this.labelIPAddress.BackColor = System.Drawing.Color.Transparent;
            this.labelIPAddress.Location = new System.Drawing.Point(7, 93);
            this.labelIPAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIPAddress.Name = "labelIPAddress";
            this.labelIPAddress.Size = new System.Drawing.Size(52, 16);
            this.labelIPAddress.TabIndex = 11;
            this.labelIPAddress.Text = "IP地址:";
            // 
            // maskedTextBoxIPAddress
            // 
            this.maskedTextBoxIPAddress.Location = new System.Drawing.Point(152, 90);
            this.maskedTextBoxIPAddress.Margin = new System.Windows.Forms.Padding(4);
            this.maskedTextBoxIPAddress.Mask = "000.000.000.000";
            this.maskedTextBoxIPAddress.Name = "maskedTextBoxIPAddress";
            this.maskedTextBoxIPAddress.Size = new System.Drawing.Size(121, 24);
            this.maskedTextBoxIPAddress.TabIndex = 3;
            this.maskedTextBoxIPAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.BackColor = System.Drawing.Color.Transparent;
            this.buttonAdd.FlatAppearance.BorderSize = 0;
            this.buttonAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gold;
            this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdd.Image = global::CSP2P.Properties.Resources.save;
            this.buttonAdd.Location = new System.Drawing.Point(176, 38);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(43, 43);
            this.buttonAdd.TabIndex = 9;
            this.buttonAdd.UseVisualStyleBackColor = false;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.BackColor = System.Drawing.Color.Transparent;
            this.buttonDelete.FlatAppearance.BorderSize = 0;
            this.buttonDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gold;
            this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDelete.Image = global::CSP2P.Properties.Resources.del;
            this.buttonDelete.Location = new System.Drawing.Point(230, 38);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(43, 43);
            this.buttonDelete.TabIndex = 10;
            this.buttonDelete.UseVisualStyleBackColor = false;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // comboBoxNIC
            // 
            this.comboBoxNIC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxNIC.FormattingEnabled = true;
            this.comboBoxNIC.Location = new System.Drawing.Point(13, 5);
            this.comboBoxNIC.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxNIC.Name = "comboBoxNIC";
            this.comboBoxNIC.Size = new System.Drawing.Size(257, 24);
            this.comboBoxNIC.TabIndex = 1;
            // 
            // FormSetLocalIP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 225);
            this.Controls.Add(this.panelIP);
            this.Font = new System.Drawing.Font("Arial", 10.5F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(299, 263);
            this.Name = "FormSetLocalIP";
            this.Text = "本地连接属性";
            this.panelIP.ResumeLayout(false);
            this.panelIP.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GradientPanel panelIP;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.ComboBox comboBoxNIC;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIPAddress;
        private System.Windows.Forms.Label labelIPAddress;
        private System.Windows.Forms.Label labelDefaultGateway;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxDefaultGateway;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxSubnetMask;
        private System.Windows.Forms.Label labelSubnetMask;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ComboBox comboBoxSettings;
    }
}