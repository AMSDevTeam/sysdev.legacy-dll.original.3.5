namespace MetroFramework.Demo
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lnkUser = new MetroFramework.Controls.MetroLink();
            this.txtConcern = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox4 = new MetroFramework.Controls.MetroTextBox();
            this.txtSearch = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox3 = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox2 = new MetroFramework.Controls.MetroTextBox();
            this.metroDateTime1 = new MetroFramework.Controls.MetroDateTime();
            this.metroTextBox1 = new MetroFramework.Controls.MetroTextBox();
            this.metroComboBox1 = new MetroFramework.Controls.MetroComboBox();
            this.metroLink1 = new MetroFramework.Controls.MetroLink();
            this.metroButton2 = new MetroFramework.Controls.MetroButton();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // lnkUser
            // 
            this.lnkUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkUser.AutoSize = true;
            this.lnkUser.FontSize = MetroFramework.MetroLinkSize.Medium;
            this.lnkUser.FontWeight = MetroFramework.MetroLinkWeight.Light;
            this.lnkUser.Image = ((System.Drawing.Image)(resources.GetObject("lnkUser.Image")));
            this.lnkUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkUser.ImageSize = 28;
            this.lnkUser.Location = new System.Drawing.Point(106, 241);
            this.lnkUser.Name = "lnkUser";
            this.lnkUser.NoFocusImage = ((System.Drawing.Image)(resources.GetObject("lnkUser.NoFocusImage")));
            this.lnkUser.Size = new System.Drawing.Size(123, 39);
            this.lnkUser.TabIndex = 88;
            this.lnkUser.Text = "Dennis Magno";
            this.lnkUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lnkUser.UseSelectable = true;
            // 
            // txtConcern
            // 
            // 
            // 
            // 
            this.txtConcern.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.txtConcern.CustomButton.Location = new System.Drawing.Point(232, 1);
            this.txtConcern.CustomButton.Name = "";
            this.txtConcern.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtConcern.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtConcern.CustomButton.TabIndex = 1;
            this.txtConcern.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtConcern.CustomButton.UseSelectable = true;
            this.txtConcern.Lines = new string[0];
            this.txtConcern.Location = new System.Drawing.Point(81, 16);
            this.txtConcern.MaxLength = 32767;
            this.txtConcern.Name = "txtConcern";
            this.txtConcern.PasswordChar = '\0';
            this.txtConcern.ReadOnly = true;
            this.txtConcern.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtConcern.SelectedText = "";
            this.txtConcern.SelectionLength = 0;
            this.txtConcern.SelectionStart = 0;
            this.txtConcern.ShowButton = true;
            this.txtConcern.Size = new System.Drawing.Size(256, 25);
            this.txtConcern.TabIndex = 87;
            this.txtConcern.UseSelectable = true;
            this.txtConcern.WaterMark = "Concern Code Detail";
            this.txtConcern.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtConcern.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtConcern.Click += new System.EventHandler(this.txtConcern_Click);
            // 
            // metroTextBox4
            // 
            // 
            // 
            // 
            this.metroTextBox4.CustomButton.Image = null;
            this.metroTextBox4.CustomButton.Location = new System.Drawing.Point(107, 1);
            this.metroTextBox4.CustomButton.Name = "";
            this.metroTextBox4.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox4.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox4.CustomButton.TabIndex = 1;
            this.metroTextBox4.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox4.CustomButton.UseSelectable = true;
            this.metroTextBox4.CustomButton.Visible = false;
            this.metroTextBox4.Lines = new string[0];
            this.metroTextBox4.Location = new System.Drawing.Point(431, 214);
            this.metroTextBox4.MaxLength = 32767;
            this.metroTextBox4.Name = "metroTextBox4";
            this.metroTextBox4.PasswordChar = '\0';
            this.metroTextBox4.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox4.SelectedText = "";
            this.metroTextBox4.SelectionLength = 0;
            this.metroTextBox4.SelectionStart = 0;
            this.metroTextBox4.Size = new System.Drawing.Size(129, 23);
            this.metroTextBox4.TabIndex = 17;
            this.metroTextBox4.UseSelectable = true;
            this.metroTextBox4.WaterMark = "This is a test";
            this.metroTextBox4.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox4.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtSearch.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            this.txtSearch.CustomButton.Location = new System.Drawing.Point(171, 2);
            this.txtSearch.CustomButton.Name = "";
            this.txtSearch.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtSearch.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtSearch.CustomButton.TabIndex = 1;
            this.txtSearch.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtSearch.CustomButton.UseSelectable = true;
            this.txtSearch.DisplayIcon = true;
            this.txtSearch.Icon = ((System.Drawing.Image)(resources.GetObject("txtSearch.Icon")));
            this.txtSearch.Lines = new string[] {
        "TEST"};
            this.txtSearch.Location = new System.Drawing.Point(356, 45);
            this.txtSearch.MaxLength = 32767;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PasswordChar = '\0';
            this.txtSearch.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSearch.SelectedText = "";
            this.txtSearch.SelectionLength = 0;
            this.txtSearch.SelectionStart = 0;
            this.txtSearch.ShowButton = true;
            this.txtSearch.ShowClearButton = true;
            this.txtSearch.Size = new System.Drawing.Size(195, 26);
            this.txtSearch.TabIndex = 16;
            this.txtSearch.Text = "TEST";
            this.txtSearch.UseSelectable = true;
            this.txtSearch.WaterMark = "Search";
            this.txtSearch.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtSearch.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroTextBox3
            // 
            // 
            // 
            // 
            this.metroTextBox3.CustomButton.Image = null;
            this.metroTextBox3.CustomButton.Location = new System.Drawing.Point(160, 2);
            this.metroTextBox3.CustomButton.Name = "";
            this.metroTextBox3.CustomButton.Size = new System.Drawing.Size(43, 43);
            this.metroTextBox3.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox3.CustomButton.TabIndex = 1;
            this.metroTextBox3.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox3.CustomButton.UseSelectable = true;
            this.metroTextBox3.CustomButton.Visible = false;
            this.metroTextBox3.Lines = new string[] {
        "metroTextBox3"};
            this.metroTextBox3.Location = new System.Drawing.Point(200, 256);
            this.metroTextBox3.MaxLength = 32767;
            this.metroTextBox3.Name = "metroTextBox3";
            this.metroTextBox3.PasswordChar = '\0';
            this.metroTextBox3.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox3.SelectedText = "";
            this.metroTextBox3.SelectionLength = 0;
            this.metroTextBox3.SelectionStart = 0;
            this.metroTextBox3.ShowClearButton = true;
            this.metroTextBox3.Size = new System.Drawing.Size(206, 48);
            this.metroTextBox3.TabIndex = 15;
            this.metroTextBox3.Text = "metroTextBox3";
            this.metroTextBox3.UseSelectable = true;
            this.metroTextBox3.WaterMark = "Testing";
            this.metroTextBox3.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox3.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroTextBox2
            // 
            // 
            // 
            // 
            this.metroTextBox2.CustomButton.Image = null;
            this.metroTextBox2.CustomButton.Location = new System.Drawing.Point(53, 1);
            this.metroTextBox2.CustomButton.Name = "";
            this.metroTextBox2.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox2.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox2.CustomButton.TabIndex = 1;
            this.metroTextBox2.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox2.CustomButton.UseSelectable = true;
            this.metroTextBox2.CustomButton.Visible = false;
            this.metroTextBox2.Lines = new string[0];
            this.metroTextBox2.Location = new System.Drawing.Point(321, 206);
            this.metroTextBox2.MaxLength = 32767;
            this.metroTextBox2.Name = "metroTextBox2";
            this.metroTextBox2.PasswordChar = '\0';
            this.metroTextBox2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox2.SelectedText = "";
            this.metroTextBox2.SelectionLength = 0;
            this.metroTextBox2.SelectionStart = 0;
            this.metroTextBox2.Size = new System.Drawing.Size(75, 23);
            this.metroTextBox2.TabIndex = 14;
            this.metroTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.metroTextBox2.UseSelectable = true;
            this.metroTextBox2.WaterMark = "Year";
            this.metroTextBox2.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox2.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroDateTime1
            // 
            this.metroDateTime1.CustomFormat = "yyyy-MM-dd";
            this.metroDateTime1.FontSize = MetroFramework.MetroDateTimeSize.Small;
            this.metroDateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.metroDateTime1.Location = new System.Drawing.Point(290, 152);
            this.metroDateTime1.MinimumSize = new System.Drawing.Size(0, 25);
            this.metroDateTime1.Name = "metroDateTime1";
            this.metroDateTime1.Size = new System.Drawing.Size(102, 25);
            this.metroDateTime1.TabIndex = 13;
            // 
            // metroTextBox1
            // 
            // 
            // 
            // 
            this.metroTextBox1.CustomButton.Image = null;
            this.metroTextBox1.CustomButton.Location = new System.Drawing.Point(53, 1);
            this.metroTextBox1.CustomButton.Name = "";
            this.metroTextBox1.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox1.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox1.CustomButton.TabIndex = 1;
            this.metroTextBox1.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox1.CustomButton.UseSelectable = true;
            this.metroTextBox1.CustomButton.Visible = false;
            this.metroTextBox1.Lines = new string[] {
        "metroTextBox1"};
            this.metroTextBox1.Location = new System.Drawing.Point(391, 119);
            this.metroTextBox1.MaxLength = 32767;
            this.metroTextBox1.Name = "metroTextBox1";
            this.metroTextBox1.PasswordChar = '\0';
            this.metroTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox1.SelectedText = "";
            this.metroTextBox1.SelectionLength = 0;
            this.metroTextBox1.SelectionStart = 0;
            this.metroTextBox1.Size = new System.Drawing.Size(75, 23);
            this.metroTextBox1.TabIndex = 12;
            this.metroTextBox1.Text = "metroTextBox1";
            this.metroTextBox1.UseSelectable = true;
            this.metroTextBox1.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox1.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroComboBox1
            // 
            this.metroComboBox1.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.metroComboBox1.FormattingEnabled = true;
            this.metroComboBox1.ItemHeight = 19;
            this.metroComboBox1.Items.AddRange(new object[] {
            "And & 1",
            "1 & 2"});
            this.metroComboBox1.Location = new System.Drawing.Point(511, 169);
            this.metroComboBox1.Name = "metroComboBox1";
            this.metroComboBox1.Size = new System.Drawing.Size(102, 25);
            this.metroComboBox1.TabIndex = 11;
            this.metroComboBox1.UseSelectable = true;
            this.metroComboBox1.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroComboBox1.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.metroComboBox1.SelectedIndexChanged += new System.EventHandler(this.metroComboBox1_SelectedIndexChanged);
            // 
            // metroLink1
            // 
            this.metroLink1.Location = new System.Drawing.Point(133, 210);
            this.metroLink1.Name = "metroLink1";
            this.metroLink1.Size = new System.Drawing.Size(90, 23);
            this.metroLink1.TabIndex = 10;
            this.metroLink1.Text = "metroLink1";
            this.metroLink1.UseSelectable = true;
            // 
            // metroButton2
            // 
            this.metroButton2.Image = null;
            this.metroButton2.Location = new System.Drawing.Point(97, 128);
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.Size = new System.Drawing.Size(115, 45);
            this.metroButton2.TabIndex = 9;
            this.metroButton2.Text = "metroButton2";
            this.metroButton2.UseSelectable = true;
            // 
            // metroButton1
            // 
            this.metroButton1.Image = null;
            this.metroButton1.Location = new System.Drawing.Point(103, 67);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(115, 45);
            this.metroButton1.TabIndex = 8;
            this.metroButton1.Text = "metroButton1";
            this.metroButton1.UseSelectable = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 357);
            this.Controls.Add(this.lnkUser);
            this.Controls.Add(this.txtConcern);
            this.Controls.Add(this.metroTextBox4);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.metroTextBox3);
            this.Controls.Add(this.metroTextBox2);
            this.Controls.Add(this.metroDateTime1);
            this.Controls.Add(this.metroTextBox1);
            this.Controls.Add(this.metroComboBox1);
            this.Controls.Add(this.metroLink1);
            this.Controls.Add(this.metroButton2);
            this.Controls.Add(this.metroButton1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.MetroButton metroButton1;
        private Controls.MetroButton metroButton2;
        private Controls.MetroLink metroLink1;
        private Controls.MetroComboBox metroComboBox1;
        private Controls.MetroTextBox metroTextBox1;
        private Controls.MetroDateTime metroDateTime1;
        private Controls.MetroTextBox metroTextBox2;
        private Controls.MetroTextBox metroTextBox3;
        private Controls.MetroTextBox txtSearch;
        private Controls.MetroTextBox metroTextBox4;
        private Controls.MetroTextBox txtConcern;
        private Controls.MetroLink lnkUser;
    }
}