namespace UserHistoryTracker
{
    partial class frmMain
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
            this.lvwHistory = new System.Windows.Forms.ListView();
            this.colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTFSType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPrjCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTimeSpan = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHours = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBillType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colErr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmdSearch = new System.Windows.Forms.Button();
            this.mnuHour = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recalculateDaysHoursToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAllTasksThatAreOneDayLongToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendKeysToDevTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openInBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteThisItemFromDatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highlightItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleVerifiedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToStandupEmailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdOpenDevtime = new System.Windows.Forms.Button();
            this.cmdEditDS = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chkWeekends = new System.Windows.Forms.CheckBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.cmdSend = new System.Windows.Forms.Button();
            this.optNew = new System.Windows.Forms.RadioButton();
            this.optHaxcom = new System.Windows.Forms.RadioButton();
            this.addTFSItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHour.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwHistory
            // 
            this.lvwHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colID,
            this.colTFSType,
            this.colPrjCode,
            this.colTimeSpan,
            this.colHours,
            this.colTitle,
            this.colBillType,
            this.colErr});
            this.lvwHistory.FullRowSelect = true;
            this.lvwHistory.HideSelection = false;
            this.lvwHistory.Location = new System.Drawing.Point(12, 41);
            this.lvwHistory.Name = "lvwHistory";
            this.lvwHistory.Size = new System.Drawing.Size(1062, 602);
            this.lvwHistory.TabIndex = 0;
            this.lvwHistory.UseCompatibleStateImageBehavior = false;
            this.lvwHistory.View = System.Windows.Forms.View.Details;
            this.lvwHistory.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvwHistory_MouseClick);
            this.lvwHistory.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwHistory_MouseDoubleClick);
            // 
            // colID
            // 
            this.colID.Text = "ID";
            // 
            // colTFSType
            // 
            this.colTFSType.Text = "TFS Type";
            this.colTFSType.Width = 62;
            // 
            // colPrjCode
            // 
            this.colPrjCode.Text = "Project Code";
            this.colPrjCode.Width = 76;
            // 
            // colTimeSpan
            // 
            this.colTimeSpan.Text = "Original Time Span";
            this.colTimeSpan.Width = 123;
            // 
            // colHours
            // 
            this.colHours.Text = "HaxCom Hours";
            this.colHours.Width = 86;
            // 
            // colTitle
            // 
            this.colTitle.Text = "Title";
            this.colTitle.Width = 578;
            // 
            // colBillType
            // 
            this.colBillType.Text = "Billing Type";
            this.colBillType.Width = 82;
            // 
            // colErr
            // 
            this.colErr.Text = "Sending Response";
            this.colErr.Width = 121;
            // 
            // cmdSearch
            // 
            this.cmdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSearch.Location = new System.Drawing.Point(974, 12);
            this.cmdSearch.Name = "cmdSearch";
            this.cmdSearch.Size = new System.Drawing.Size(100, 23);
            this.cmdSearch.TabIndex = 1;
            this.cmdSearch.Text = "Search";
            this.cmdSearch.UseVisualStyleBackColor = true;
            this.cmdSearch.Click += new System.EventHandler(this.cmdSearch_Click);
            // 
            // mnuHour
            // 
            this.mnuHour.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.recalculateDaysHoursToolStripMenuItem,
            this.removeAllTasksThatAreOneDayLongToolStripMenuItem,
            this.sendKeysToDevTimeToolStripMenuItem,
            this.openInBrowserToolStripMenuItem,
            this.deleteThisItemFromDatesToolStripMenuItem,
            this.highlightItemToolStripMenuItem,
            this.toggleVerifiedToolStripMenuItem,
            this.copyToStandupEmailToolStripMenuItem,
            this.addTFSItemToolStripMenuItem});
            this.mnuHour.Name = "mnuHour";
            this.mnuHour.Size = new System.Drawing.Size(365, 268);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(364, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(364, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // recalculateDaysHoursToolStripMenuItem
            // 
            this.recalculateDaysHoursToolStripMenuItem.Name = "recalculateDaysHoursToolStripMenuItem";
            this.recalculateDaysHoursToolStripMenuItem.Size = new System.Drawing.Size(364, 22);
            this.recalculateDaysHoursToolStripMenuItem.Text = "Re-calculate Day\'s hours";
            this.recalculateDaysHoursToolStripMenuItem.Click += new System.EventHandler(this.recalculateDaysHoursToolStripMenuItem_Click);
            // 
            // removeAllTasksThatAreOneDayLongToolStripMenuItem
            // 
            this.removeAllTasksThatAreOneDayLongToolStripMenuItem.Name = "removeAllTasksThatAreOneDayLongToolStripMenuItem";
            this.removeAllTasksThatAreOneDayLongToolStripMenuItem.Size = new System.Drawing.Size(364, 22);
            this.removeAllTasksThatAreOneDayLongToolStripMenuItem.Text = "Remove all tasks that are one day long and re-calculate";
            this.removeAllTasksThatAreOneDayLongToolStripMenuItem.Click += new System.EventHandler(this.removeAllTasksThatAreOneDayLongToolStripMenuItem_Click);
            // 
            // sendKeysToDevTimeToolStripMenuItem
            // 
            this.sendKeysToDevTimeToolStripMenuItem.Name = "sendKeysToDevTimeToolStripMenuItem";
            this.sendKeysToDevTimeToolStripMenuItem.Size = new System.Drawing.Size(364, 22);
            this.sendKeysToDevTimeToolStripMenuItem.Text = "SendKeys to DevTime";
            this.sendKeysToDevTimeToolStripMenuItem.Click += new System.EventHandler(this.sendKeysToDevTimeToolStripMenuItem_Click);
            // 
            // openInBrowserToolStripMenuItem
            // 
            this.openInBrowserToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openInBrowserToolStripMenuItem.Name = "openInBrowserToolStripMenuItem";
            this.openInBrowserToolStripMenuItem.Size = new System.Drawing.Size(364, 22);
            this.openInBrowserToolStripMenuItem.Text = "Open in browser";
            this.openInBrowserToolStripMenuItem.Click += new System.EventHandler(this.openInBrowserToolStripMenuItem_Click);
            // 
            // deleteThisItemFromDatesToolStripMenuItem
            // 
            this.deleteThisItemFromDatesToolStripMenuItem.Name = "deleteThisItemFromDatesToolStripMenuItem";
            this.deleteThisItemFromDatesToolStripMenuItem.Size = new System.Drawing.Size(364, 22);
            this.deleteThisItemFromDatesToolStripMenuItem.Text = "Delete this item from dates";
            this.deleteThisItemFromDatesToolStripMenuItem.Click += new System.EventHandler(this.deleteThisItemFromDatesToolStripMenuItem_Click);
            // 
            // highlightItemToolStripMenuItem
            // 
            this.highlightItemToolStripMenuItem.Name = "highlightItemToolStripMenuItem";
            this.highlightItemToolStripMenuItem.Size = new System.Drawing.Size(364, 22);
            this.highlightItemToolStripMenuItem.Text = "Highlight Item";
            this.highlightItemToolStripMenuItem.Click += new System.EventHandler(this.highlightItemToolStripMenuItem_Click);
            // 
            // toggleVerifiedToolStripMenuItem
            // 
            this.toggleVerifiedToolStripMenuItem.Name = "toggleVerifiedToolStripMenuItem";
            this.toggleVerifiedToolStripMenuItem.Size = new System.Drawing.Size(364, 22);
            this.toggleVerifiedToolStripMenuItem.Text = "Toggle Verified";
            this.toggleVerifiedToolStripMenuItem.Click += new System.EventHandler(this.toggleVerifiedToolStripMenuItem_Click);
            // 
            // copyToStandupEmailToolStripMenuItem
            // 
            this.copyToStandupEmailToolStripMenuItem.Name = "copyToStandupEmailToolStripMenuItem";
            this.copyToStandupEmailToolStripMenuItem.Size = new System.Drawing.Size(364, 22);
            this.copyToStandupEmailToolStripMenuItem.Text = "Copy to Stand-up email";
            this.copyToStandupEmailToolStripMenuItem.Click += new System.EventHandler(this.copyToStandupEmailToolStripMenuItem_Click);
            // 
            // cmdOpenDevtime
            // 
            this.cmdOpenDevtime.Location = new System.Drawing.Point(127, 12);
            this.cmdOpenDevtime.Name = "cmdOpenDevtime";
            this.cmdOpenDevtime.Size = new System.Drawing.Size(93, 23);
            this.cmdOpenDevtime.TabIndex = 1;
            this.cmdOpenDevtime.Text = "Open Devtime";
            this.cmdOpenDevtime.UseVisualStyleBackColor = true;
            this.cmdOpenDevtime.Click += new System.EventHandler(this.cmdOpenDevtime_Click);
            // 
            // cmdEditDS
            // 
            this.cmdEditDS.Location = new System.Drawing.Point(12, 12);
            this.cmdEditDS.Name = "cmdEditDS";
            this.cmdEditDS.Size = new System.Drawing.Size(109, 23);
            this.cmdEditDS.TabIndex = 1;
            this.cmdEditDS.Text = "Edit Data Sources";
            this.cmdEditDS.UseVisualStyleBackColor = true;
            this.cmdEditDS.Click += new System.EventHandler(this.cmdEditDS_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(830, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Since";
            // 
            // chkWeekends
            // 
            this.chkWeekends.AutoSize = true;
            this.chkWeekends.Location = new System.Drawing.Point(226, 16);
            this.chkWeekends.Name = "chkWeekends";
            this.chkWeekends.Size = new System.Drawing.Size(78, 17);
            this.chkWeekends.TabIndex = 5;
            this.chkWeekends.Text = "Weekends";
            this.chkWeekends.UseVisualStyleBackColor = true;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(870, 13);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(98, 20);
            this.dtpFrom.TabIndex = 6;
            // 
            // cmdSend
            // 
            this.cmdSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSend.Location = new System.Drawing.Point(974, 649);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.Size = new System.Drawing.Size(100, 23);
            this.cmdSend.TabIndex = 7;
            this.cmdSend.Text = "Send to HaxCom";
            this.cmdSend.UseVisualStyleBackColor = true;
            this.cmdSend.Click += new System.EventHandler(this.cmdSend_Click);
            // 
            // optNew
            // 
            this.optNew.AutoSize = true;
            this.optNew.Checked = true;
            this.optNew.Location = new System.Drawing.Point(316, 15);
            this.optNew.Name = "optNew";
            this.optNew.Size = new System.Drawing.Size(75, 17);
            this.optNew.TabIndex = 8;
            this.optNew.TabStop = true;
            this.optNew.Text = "New Items";
            this.optNew.UseVisualStyleBackColor = true;
            // 
            // optHaxcom
            // 
            this.optHaxcom.AutoSize = true;
            this.optHaxcom.Location = new System.Drawing.Point(407, 15);
            this.optHaxcom.Name = "optHaxcom";
            this.optHaxcom.Size = new System.Drawing.Size(85, 17);
            this.optHaxcom.TabIndex = 8;
            this.optHaxcom.Text = "Edit Haxcom";
            this.optHaxcom.UseVisualStyleBackColor = true;
            // 
            // addTFSItemToolStripMenuItem
            // 
            this.addTFSItemToolStripMenuItem.Name = "addTFSItemToolStripMenuItem";
            this.addTFSItemToolStripMenuItem.Size = new System.Drawing.Size(364, 22);
            this.addTFSItemToolStripMenuItem.Text = "Add TFS Item";
            this.addTFSItemToolStripMenuItem.Click += new System.EventHandler(this.AddTFSItemToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 680);
            this.Controls.Add(this.optHaxcom);
            this.Controls.Add(this.optNew);
            this.Controls.Add(this.cmdSend);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.chkWeekends);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdOpenDevtime);
            this.Controls.Add(this.cmdEditDS);
            this.Controls.Add(this.cmdSearch);
            this.Controls.Add(this.lvwHistory);
            this.Name = "frmMain";
            this.Text = "User History Tracker";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.mnuHour.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvwHistory;
        private System.Windows.Forms.Button cmdSearch;
        private System.Windows.Forms.ColumnHeader colID;
        private System.Windows.Forms.ColumnHeader colTimeSpan;
        private System.Windows.Forms.ColumnHeader colTitle;
        private System.Windows.Forms.ColumnHeader colHours;
        private System.Windows.Forms.ContextMenuStrip mnuHour;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recalculateDaysHoursToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAllTasksThatAreOneDayLongToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendKeysToDevTimeToolStripMenuItem;
        private System.Windows.Forms.Button cmdOpenDevtime;
        private System.Windows.Forms.Button cmdEditDS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem openInBrowserToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkWeekends;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.ToolStripMenuItem deleteThisItemFromDatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem highlightItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleVerifiedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToStandupEmailToolStripMenuItem;
        private System.Windows.Forms.Button cmdSend;
        private System.Windows.Forms.ColumnHeader colBillType;
        private System.Windows.Forms.ColumnHeader colErr;
        private System.Windows.Forms.ColumnHeader colTFSType;
        private System.Windows.Forms.ColumnHeader colPrjCode;
        private System.Windows.Forms.RadioButton optNew;
        private System.Windows.Forms.RadioButton optHaxcom;
        private System.Windows.Forms.ToolStripMenuItem addTFSItemToolStripMenuItem;
    }
}

