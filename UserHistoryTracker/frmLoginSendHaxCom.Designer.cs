namespace UserHistoryTracker
{
    partial class frmLoginSendHaxCom
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
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.optSel = new System.Windows.Forms.RadioButton();
            this.optDays = new System.Windows.Forms.RadioButton();
            this.optAll = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdSend = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.pnlSubmit = new System.Windows.Forms.Panel();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.CHKBug = new System.Windows.Forms.CheckBox();
            this.chkOther = new System.Windows.Forms.CheckBox();
            this.chkMeeting = new System.Windows.Forms.CheckBox();
            this.chkPBI = new System.Windows.Forms.CheckBox();
            this.chkCapitalized = new System.Windows.Forms.CheckBox();
            this.chkNonBillable = new System.Windows.Forms.CheckBox();
            this.chkPrjtLess = new System.Windows.Forms.CheckBox();
            this.pnlSubmit.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(12, 46);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(453, 468);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WebBrowser1_DocumentCompleted);
            // 
            // optSel
            // 
            this.optSel.AutoSize = true;
            this.optSel.Location = new System.Drawing.Point(46, 3);
            this.optSel.Name = "optSel";
            this.optSel.Size = new System.Drawing.Size(95, 17);
            this.optSel.TabIndex = 1;
            this.optSel.Text = "Selected Items";
            this.optSel.UseVisualStyleBackColor = true;
            // 
            // optDays
            // 
            this.optDays.AutoSize = true;
            this.optDays.Location = new System.Drawing.Point(147, 3);
            this.optDays.Name = "optDays";
            this.optDays.Size = new System.Drawing.Size(94, 17);
            this.optDays.TabIndex = 1;
            this.optDays.Text = "Selected Days";
            this.optDays.UseVisualStyleBackColor = true;
            // 
            // optAll
            // 
            this.optAll.AutoSize = true;
            this.optAll.Checked = true;
            this.optAll.Location = new System.Drawing.Point(247, 3);
            this.optAll.Name = "optAll";
            this.optAll.Size = new System.Drawing.Size(64, 17);
            this.optAll.TabIndex = 1;
            this.optAll.TabStop = true;
            this.optAll.Text = "All Items";
            this.optAll.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Send:";
            // 
            // cmdSend
            // 
            this.cmdSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSend.Location = new System.Drawing.Point(309, 520);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.Size = new System.Drawing.Size(75, 23);
            this.cmdSend.TabIndex = 3;
            this.cmdSend.Text = "Ok";
            this.cmdSend.UseVisualStyleBackColor = true;
            this.cmdSend.Visible = false;
            this.cmdSend.Click += new System.EventHandler(this.CmdSend_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Location = new System.Drawing.Point(390, 520);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.CmdCancel_Click);
            // 
            // pnlSubmit
            // 
            this.pnlSubmit.Controls.Add(this.label1);
            this.pnlSubmit.Controls.Add(this.optSel);
            this.pnlSubmit.Controls.Add(this.optDays);
            this.pnlSubmit.Controls.Add(this.optAll);
            this.pnlSubmit.Location = new System.Drawing.Point(12, 12);
            this.pnlSubmit.Name = "pnlSubmit";
            this.pnlSubmit.Size = new System.Drawing.Size(351, 28);
            this.pnlSubmit.TabIndex = 4;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.groupBox1);
            this.pnlSearch.Controls.Add(this.chkCapitalized);
            this.pnlSearch.Controls.Add(this.chkNonBillable);
            this.pnlSearch.Controls.Add(this.chkPrjtLess);
            this.pnlSearch.Location = new System.Drawing.Point(12, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(454, 49);
            this.pnlSearch.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAll);
            this.groupBox1.Controls.Add(this.CHKBug);
            this.groupBox1.Controls.Add(this.chkOther);
            this.groupBox1.Controls.Add(this.chkMeeting);
            this.groupBox1.Controls.Add(this.chkPBI);
            this.groupBox1.Location = new System.Drawing.Point(3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 43);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type";
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Checked = true;
            this.chkAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAll.Location = new System.Drawing.Point(6, 19);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(37, 17);
            this.chkAll.TabIndex = 0;
            this.chkAll.Text = "All";
            this.chkAll.UseVisualStyleBackColor = true;
            // 
            // CHKBug
            // 
            this.CHKBug.AutoSize = true;
            this.CHKBug.Location = new System.Drawing.Point(92, 19);
            this.CHKBug.Name = "CHKBug";
            this.CHKBug.Size = new System.Drawing.Size(45, 17);
            this.CHKBug.TabIndex = 0;
            this.CHKBug.Text = "Bug";
            this.CHKBug.UseVisualStyleBackColor = true;
            // 
            // chkOther
            // 
            this.chkOther.AutoSize = true;
            this.chkOther.Location = new System.Drawing.Point(213, 19);
            this.chkOther.Name = "chkOther";
            this.chkOther.Size = new System.Drawing.Size(52, 17);
            this.chkOther.TabIndex = 0;
            this.chkOther.Text = "Other";
            this.chkOther.UseVisualStyleBackColor = true;
            // 
            // chkMeeting
            // 
            this.chkMeeting.AutoSize = true;
            this.chkMeeting.Location = new System.Drawing.Point(143, 19);
            this.chkMeeting.Name = "chkMeeting";
            this.chkMeeting.Size = new System.Drawing.Size(64, 17);
            this.chkMeeting.TabIndex = 0;
            this.chkMeeting.Text = "Meeting";
            this.chkMeeting.UseVisualStyleBackColor = true;
            // 
            // chkPBI
            // 
            this.chkPBI.AutoSize = true;
            this.chkPBI.Location = new System.Drawing.Point(43, 19);
            this.chkPBI.Name = "chkPBI";
            this.chkPBI.Size = new System.Drawing.Size(43, 17);
            this.chkPBI.TabIndex = 0;
            this.chkPBI.Text = "User Story";
            this.chkPBI.UseVisualStyleBackColor = true;
            // 
            // chkCapitalized
            // 
            this.chkCapitalized.AutoSize = true;
            this.chkCapitalized.Location = new System.Drawing.Point(368, 26);
            this.chkCapitalized.Name = "chkCapitalized";
            this.chkCapitalized.Size = new System.Drawing.Size(77, 17);
            this.chkCapitalized.TabIndex = 0;
            this.chkCapitalized.Text = "Capitalized";
            this.chkCapitalized.UseVisualStyleBackColor = true;
            // 
            // chkNonBillable
            // 
            this.chkNonBillable.AutoSize = true;
            this.chkNonBillable.Location = new System.Drawing.Point(281, 26);
            this.chkNonBillable.Name = "chkNonBillable";
            this.chkNonBillable.Size = new System.Drawing.Size(81, 17);
            this.chkNonBillable.TabIndex = 0;
            this.chkNonBillable.Text = "Non-billable";
            this.chkNonBillable.UseVisualStyleBackColor = true;
            // 
            // chkPrjtLess
            // 
            this.chkPrjtLess.AutoSize = true;
            this.chkPrjtLess.Location = new System.Drawing.Point(281, 7);
            this.chkPrjtLess.Name = "chkPrjtLess";
            this.chkPrjtLess.Size = new System.Drawing.Size(105, 17);
            this.chkPrjtLess.TabIndex = 0;
            this.chkPrjtLess.Text = "Projectless Items";
            this.chkPrjtLess.UseVisualStyleBackColor = true;
            // 
            // frmLoginSendHaxCom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 555);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlSubmit);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdSend);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLoginSendHaxCom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Please Login to HaxCom";
            this.Load += new System.EventHandler(this.FrmLoginSendHaxCom_Load);
            this.pnlSubmit.ResumeLayout(false);
            this.pnlSubmit.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.RadioButton optAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdSend;
        private System.Windows.Forms.Button cmdCancel;
        public System.Windows.Forms.RadioButton optSel;
        public System.Windows.Forms.RadioButton optDays;
        private System.Windows.Forms.Panel pnlSubmit;
        private System.Windows.Forms.Panel pnlSearch;
        public System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.CheckBox CHKBug;
        public System.Windows.Forms.CheckBox chkMeeting;
        public System.Windows.Forms.CheckBox chkPBI;
        public System.Windows.Forms.CheckBox chkCapitalized;
        public System.Windows.Forms.CheckBox chkNonBillable;
        public System.Windows.Forms.CheckBox chkPrjtLess;
        public System.Windows.Forms.CheckBox chkOther;
    }
}