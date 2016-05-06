namespace CarPCSolutions.SystemTrayApplication.Forms
{
    partial class ConfigurationPageForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cbAlwaysSendTVEnable = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbDelayBetweenTVEnableMsgs = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbFFRWDelay = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.tbMessageReaderDelay = new System.Windows.Forms.TextBox();
            this.tbAcceptanceMask = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbAcceptanceCode = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbBaudRate = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(686, 336);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbAlwaysSendTVEnable);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.tbDelayBetweenTVEnableMsgs);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.tbFFRWDelay);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(678, 310);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cbAlwaysSendTVEnable
            // 
            this.cbAlwaysSendTVEnable.AutoSize = true;
            this.cbAlwaysSendTVEnable.Location = new System.Drawing.Point(22, 95);
            this.cbAlwaysSendTVEnable.Name = "cbAlwaysSendTVEnable";
            this.cbAlwaysSendTVEnable.Size = new System.Drawing.Size(182, 17);
            this.cbAlwaysSendTVEnable.TabIndex = 15;
            this.cbAlwaysSendTVEnable.Text = "Always send TV enable message";
            this.cbAlwaysSendTVEnable.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 72);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(153, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Delay before enabling TV input";
            // 
            // tbTVEnableDelay
            // 
            this.tbDelayBetweenTVEnableMsgs.Location = new System.Drawing.Point(188, 69);
            this.tbDelayBetweenTVEnableMsgs.Name = "tbTVEnableDelay";
            this.tbDelayBetweenTVEnableMsgs.Size = new System.Drawing.Size(127, 20);
            this.tbDelayBetweenTVEnableMsgs.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "FF  / RW Delay";
            // 
            // tbFFRWDelay
            // 
            this.tbFFRWDelay.Location = new System.Drawing.Point(188, 43);
            this.tbFFRWDelay.Name = "tbFFRWDelay";
            this.tbFFRWDelay.Size = new System.Drawing.Size(127, 20);
            this.tbFFRWDelay.TabIndex = 9;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(108, 48);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.tbMessageReaderDelay);
            this.tabPage3.Controls.Add(this.tbAcceptanceMask);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.tbAcceptanceCode);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.tbBaudRate);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(678, 310);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "CANUSB Adapter Settings";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(163, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "CANUSB Message Reader delay";
            // 
            // tbMessageReaderDelay
            // 
            this.tbMessageReaderDelay.Location = new System.Drawing.Point(207, 109);
            this.tbMessageReaderDelay.Name = "tbMessageReaderDelay";
            this.tbMessageReaderDelay.Size = new System.Drawing.Size(195, 20);
            this.tbMessageReaderDelay.TabIndex = 9;
            // 
            // tbAcceptanceMask
            // 
            this.tbAcceptanceMask.Location = new System.Drawing.Point(207, 83);
            this.tbAcceptanceMask.Name = "tbAcceptanceMask";
            this.tbAcceptanceMask.Size = new System.Drawing.Size(195, 20);
            this.tbAcceptanceMask.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(33, 86);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(97, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Acceptance Mask:";
            // 
            // tbAcceptanceCode
            // 
            this.tbAcceptanceCode.Location = new System.Drawing.Point(207, 57);
            this.tbAcceptanceCode.Name = "tbAcceptanceCode";
            this.tbAcceptanceCode.Size = new System.Drawing.Size(195, 20);
            this.tbAcceptanceCode.TabIndex = 3;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(33, 60);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(96, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Acceptance Code:";
            // 
            // tbBaudRate
            // 
            this.tbBaudRate.Location = new System.Drawing.Point(207, 31);
            this.tbBaudRate.Name = "tbBaudRate";
            this.tbBaudRate.Size = new System.Drawing.Size(195, 20);
            this.tbBaudRate.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(33, 34);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Baud Rate:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(262, 354);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(343, 354);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ConfigurationPageForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(710, 388);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ConfigurationPageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CarPC Interface Configuration";
            this.Load += new System.EventHandler(this.ConfigurationPageForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbFFRWDelay;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbDelayBetweenTVEnableMsgs;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox tbAcceptanceMask;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbAcceptanceCode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbBaudRate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbMessageReaderDelay;
        private System.Windows.Forms.CheckBox cbAlwaysSendTVEnable;
    }
}