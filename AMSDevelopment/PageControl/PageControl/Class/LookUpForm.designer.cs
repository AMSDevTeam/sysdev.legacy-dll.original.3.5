namespace PageControl.Pager
{
    partial class LookUpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LookUpForm));
            this.pnlBottom = new MetroFramework.Controls.MetroPanel();
            this.txtSearch = new MetroFramework.Controls.MetroTextBox();
            this.lnkAdd = new MetroFramework.Controls.MetroLink();
            this.pnlLine = new MetroFramework.Controls.MetroPanel();
            this.mtgGrid = new PageControl.Pager.MetroGrid();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mtgGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.txtSearch);
            this.pnlBottom.Controls.Add(this.lnkAdd);
            this.pnlBottom.Controls.Add(this.pnlLine);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.HorizontalScrollbarBarColor = true;
            this.pnlBottom.HorizontalScrollbarHighlightOnWheel = false;
            this.pnlBottom.HorizontalScrollbarSize = 12;
            this.pnlBottom.Location = new System.Drawing.Point(0, 451);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.pnlBottom.Size = new System.Drawing.Size(402, 41);
            this.pnlBottom.TabIndex = 1;
            this.pnlBottom.VerticalScrollbarBarColor = true;
            this.pnlBottom.VerticalScrollbarHighlightOnWheel = false;
            this.pnlBottom.VerticalScrollbarSize = 11;
            // 
            // txtSearch
            // 
            // 
            // 
            // 
            this.txtSearch.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.txtSearch.CustomButton.Location = new System.Drawing.Point(342, 2);
            this.txtSearch.CustomButton.Margin = new System.Windows.Forms.Padding(4);
            this.txtSearch.CustomButton.Name = "";
            this.txtSearch.CustomButton.Size = new System.Drawing.Size(29, 29);
            this.txtSearch.CustomButton.Style = MetroFramework.MetroColorStyle.TealDark;
            this.txtSearch.CustomButton.TabIndex = 1;
            this.txtSearch.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtSearch.CustomButton.UseSelectable = true;
            this.txtSearch.DisplayIcon = true;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Icon = ((System.Drawing.Image)(resources.GetObject("txtSearch.Icon")));
            this.txtSearch.Lines = new string[0];
            this.txtSearch.Location = new System.Drawing.Point(0, 7);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtSearch.MaxLength = 32767;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PasswordChar = '\0';
            this.txtSearch.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSearch.SelectedText = "";
            this.txtSearch.SelectionLength = 0;
            this.txtSearch.SelectionStart = 0;
            this.txtSearch.ShowButton = true;
            this.txtSearch.ShowClearButton = true;
            this.txtSearch.Size = new System.Drawing.Size(374, 34);
            this.txtSearch.Style = MetroFramework.MetroColorStyle.TealDark;
            this.txtSearch.TabIndex = 10;
            this.txtSearch.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtSearch.UseSelectable = true;
            this.txtSearch.WaterMark = "Search";
            this.txtSearch.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtSearch.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lnkAdd
            // 
            this.lnkAdd.AutoSize = true;
            this.lnkAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.lnkAdd.Image = ((System.Drawing.Image)(resources.GetObject("lnkAdd.Image")));
            this.lnkAdd.ImageSize = 24;
            this.lnkAdd.Location = new System.Drawing.Point(374, 7);
            this.lnkAdd.Margin = new System.Windows.Forms.Padding(4);
            this.lnkAdd.Name = "lnkAdd";
            this.lnkAdd.NoFocusImage = ((System.Drawing.Image)(resources.GetObject("lnkAdd.NoFocusImage")));
            this.lnkAdd.Size = new System.Drawing.Size(28, 34);
            this.lnkAdd.TabIndex = 60;
            this.lnkAdd.UseSelectable = true;
            this.lnkAdd.Click += new System.EventHandler(this.lnkAdd_Click);
            // 
            // pnlLine
            // 
            this.pnlLine.BackColor = System.Drawing.Color.Silver;
            this.pnlLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLine.HorizontalScrollbarBarColor = true;
            this.pnlLine.HorizontalScrollbarHighlightOnWheel = false;
            this.pnlLine.HorizontalScrollbarSize = 12;
            this.pnlLine.Location = new System.Drawing.Point(0, 6);
            this.pnlLine.Margin = new System.Windows.Forms.Padding(4);
            this.pnlLine.Name = "pnlLine";
            this.pnlLine.Size = new System.Drawing.Size(402, 1);
            this.pnlLine.TabIndex = 59;
            this.pnlLine.UseCustomBackColor = true;
            this.pnlLine.VerticalScrollbarBarColor = true;
            this.pnlLine.VerticalScrollbarHighlightOnWheel = false;
            this.pnlLine.VerticalScrollbarSize = 11;
            // 
            // mtgGrid
            // 
            this.mtgGrid.AllowEditing = false;
            this.mtgGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.mtgGrid.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.mtgGrid.ColumnInfo = "10,0,0,0,0,105,Columns:";
            this.mtgGrid.DisplayAction = false;
            this.mtgGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtgGrid.ExtendLastCol = true;
            this.mtgGrid.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid;
            this.mtgGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.mtgGrid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mtgGrid.Location = new System.Drawing.Point(0, 0);
            this.mtgGrid.Margin = new System.Windows.Forms.Padding(4);
            this.mtgGrid.Name = "mtgGrid";
            this.mtgGrid.Padding = new System.Windows.Forms.Padding(0, 0, 18, 18);
            this.mtgGrid.Rows.DefaultSize = 21;
            this.mtgGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mtgGrid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.mtgGrid.Size = new System.Drawing.Size(402, 451);
            this.mtgGrid.Style = MetroFramework.MetroColorStyle.TealDark;
            this.mtgGrid.StyleInfo = resources.GetString("mtgGrid.StyleInfo");
            this.mtgGrid.TabIndex = 0;
            this.mtgGrid.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // LookUpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 492);
            this.ControlBox = false;
            this.Controls.Add(this.mtgGrid);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LookUpForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "LookUpForm";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mtgGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroPanel pnlBottom;
        private MetroFramework.Controls.MetroTextBox txtSearch;
        private MetroFramework.Controls.MetroPanel pnlLine;
        public MetroFramework.Controls.MetroLink lnkAdd;
        private MetroGrid mtgGrid;
    }
}