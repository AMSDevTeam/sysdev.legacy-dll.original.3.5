namespace PageControl.Pager
{
    partial class PagerControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PagerControl));
            this.flpPage = new System.Windows.Forms.FlowLayoutPanel();
            this.btnFirst = new MetroFramework.Controls.MetroLink();
            this.btnPrev = new MetroFramework.Controls.MetroLink();
            this.lblDisplay = new MetroFramework.Controls.MetroLabel();
            this.btnNext = new MetroFramework.Controls.MetroLink();
            this.btnLast = new MetroFramework.Controls.MetroLink();
            this.lblRecord = new MetroFramework.Controls.MetroLabel();
            this.lblShow = new MetroFramework.Controls.MetroLabel();
            this.cboLimit = new MetroFramework.Controls.MetroComboBox();
            this.lblPipe = new MetroFramework.Controls.MetroLabel();
            this.chkLocked = new System.Windows.Forms.CheckBox();
            this.flpPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpPage
            // 
            this.flpPage.BackColor = System.Drawing.Color.Transparent;
            this.flpPage.Controls.Add(this.btnFirst);
            this.flpPage.Controls.Add(this.btnPrev);
            this.flpPage.Controls.Add(this.lblDisplay);
            this.flpPage.Controls.Add(this.btnNext);
            this.flpPage.Controls.Add(this.btnLast);
            this.flpPage.Location = new System.Drawing.Point(2, 6);
            this.flpPage.Name = "flpPage";
            this.flpPage.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.flpPage.Size = new System.Drawing.Size(487, 31);
            this.flpPage.TabIndex = 33;
            // 
            // btnFirst
            // 
            this.btnFirst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFirst.FontWeight = MetroFramework.MetroLinkWeight.Light;
            this.btnFirst.Image = ((System.Drawing.Image)(resources.GetObject("btnFirst.Image")));
            this.btnFirst.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFirst.ImageSize = 18;
            this.btnFirst.Location = new System.Drawing.Point(8, 3);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.NoFocusImage = ((System.Drawing.Image)(resources.GetObject("btnFirst.NoFocusImage")));
            this.btnFirst.Size = new System.Drawing.Size(27, 22);
            this.btnFirst.TabIndex = 20;
            this.btnFirst.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFirst.UseSelectable = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrev.FontWeight = MetroFramework.MetroLinkWeight.Light;
            this.btnPrev.Image = ((System.Drawing.Image)(resources.GetObject("btnPrev.Image")));
            this.btnPrev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrev.ImageSize = 18;
            this.btnPrev.Location = new System.Drawing.Point(41, 3);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.NoFocusImage = ((System.Drawing.Image)(resources.GetObject("btnPrev.NoFocusImage")));
            this.btnPrev.Size = new System.Drawing.Size(27, 22);
            this.btnPrev.TabIndex = 21;
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrev.UseSelectable = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblDisplay
            // 
            this.lblDisplay.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblDisplay.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblDisplay.Location = new System.Drawing.Point(74, 0);
            this.lblDisplay.Name = "lblDisplay";
            this.lblDisplay.Size = new System.Drawing.Size(105, 31);
            this.lblDisplay.TabIndex = 31;
            this.lblDisplay.Text = "Page 5 of 100";
            this.lblDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.FontWeight = MetroFramework.MetroLinkWeight.Light;
            this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
            this.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNext.ImageSize = 18;
            this.btnNext.Location = new System.Drawing.Point(185, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.NoFocusImage = ((System.Drawing.Image)(resources.GetObject("btnNext.NoFocusImage")));
            this.btnNext.Size = new System.Drawing.Size(27, 22);
            this.btnNext.TabIndex = 32;
            this.btnNext.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNext.UseSelectable = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnLast
            // 
            this.btnLast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLast.FontWeight = MetroFramework.MetroLinkWeight.Light;
            this.btnLast.Image = ((System.Drawing.Image)(resources.GetObject("btnLast.Image")));
            this.btnLast.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLast.ImageSize = 18;
            this.btnLast.Location = new System.Drawing.Point(218, 3);
            this.btnLast.Name = "btnLast";
            this.btnLast.NoFocusImage = ((System.Drawing.Image)(resources.GetObject("btnLast.NoFocusImage")));
            this.btnLast.Size = new System.Drawing.Size(27, 22);
            this.btnLast.TabIndex = 33;
            this.btnLast.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLast.UseSelectable = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // lblRecord
            // 
            this.lblRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecord.AutoSize = true;
            this.lblRecord.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblRecord.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblRecord.Location = new System.Drawing.Point(765, 14);
            this.lblRecord.Name = "lblRecord";
            this.lblRecord.Size = new System.Drawing.Size(90, 15);
            this.lblRecord.TabIndex = 36;
            this.lblRecord.Text = "record per page";
            // 
            // lblShow
            // 
            this.lblShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblShow.AutoSize = true;
            this.lblShow.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblShow.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblShow.Location = new System.Drawing.Point(644, 14);
            this.lblShow.Name = "lblShow";
            this.lblShow.Size = new System.Drawing.Size(45, 15);
            this.lblShow.TabIndex = 35;
            this.lblShow.Text = "Display";
            // 
            // cboLimit
            // 
            this.cboLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLimit.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cboLimit.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.cboLimit.FormattingEnabled = true;
            this.cboLimit.ItemHeight = 17;
            this.cboLimit.Items.AddRange(new object[] {
            "100",
            "200",
            "300",
            "400",
            "500",
            "ALL"});
            this.cboLimit.Location = new System.Drawing.Point(693, 9);
            this.cboLimit.Name = "cboLimit";
            this.cboLimit.Size = new System.Drawing.Size(70, 23);
            this.cboLimit.TabIndex = 34;
            this.cboLimit.UseSelectable = true;
            this.cboLimit.WaterMark = "Top";
            this.cboLimit.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.cboLimit.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblPipe
            // 
            this.lblPipe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPipe.AutoSize = true;
            this.lblPipe.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblPipe.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblPipe.Location = new System.Drawing.Point(637, 13);
            this.lblPipe.Name = "lblPipe";
            this.lblPipe.Size = new System.Drawing.Size(10, 15);
            this.lblPipe.TabIndex = 38;
            this.lblPipe.Text = "|";
            // 
            // chkLocked
            // 
            this.chkLocked.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLocked.AutoSize = true;
            this.chkLocked.BackColor = System.Drawing.Color.Transparent;
            this.chkLocked.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLocked.Location = new System.Drawing.Point(575, 15);
            this.chkLocked.Name = "chkLocked";
            this.chkLocked.Size = new System.Drawing.Size(62, 17);
            this.chkLocked.TabIndex = 39;
            this.chkLocked.Text = "Locked";
            this.chkLocked.UseVisualStyleBackColor = false;
            this.chkLocked.CheckedChanged += new System.EventHandler(this.chkLocked_CheckedChanged);
            // 
            // PagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkLocked);
            this.Controls.Add(this.lblPipe);
            this.Controls.Add(this.flpPage);
            this.Controls.Add(this.lblRecord);
            this.Controls.Add(this.lblShow);
            this.Controls.Add(this.cboLimit);
            this.Name = "PagerControl";
            this.Size = new System.Drawing.Size(858, 37);
            this.Style = MetroFramework.MetroColorStyle.Black;
            this.Load += new System.EventHandler(this.PagerControl_Load);
            this.flpPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpPage;
        private MetroFramework.Controls.MetroLink btnFirst;
        private MetroFramework.Controls.MetroLink btnPrev;
        private MetroFramework.Controls.MetroLabel lblDisplay;
        private MetroFramework.Controls.MetroLink btnNext;
        private MetroFramework.Controls.MetroLink btnLast;
        private MetroFramework.Controls.MetroLabel lblRecord;
        private MetroFramework.Controls.MetroLabel lblShow;
        public MetroFramework.Controls.MetroComboBox cboLimit;
        private MetroFramework.Controls.MetroLabel lblPipe;
        public System.Windows.Forms.CheckBox chkLocked;
    }
}
