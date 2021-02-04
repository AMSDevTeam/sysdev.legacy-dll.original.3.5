namespace PageControl.Pager
{
    partial class MetroGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetroGrid));
            this._vertical = new MetroFramework.Controls.MetroScrollBar();
            this._horizontal = new MetroFramework.Controls.MetroScrollBar();
            this.pnlAction = new System.Windows.Forms.Panel();
            this.lnkDelete = new MetroFramework.Controls.MetroLink();
            this.lnkEdit = new MetroFramework.Controls.MetroLink();
            this.pnlAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // _vertical
            // 
            this._vertical.LargeChange = 10;
            this._vertical.Location = new System.Drawing.Point(0, 0);
            this._vertical.Maximum = 100;
            this._vertical.Minimum = 0;
            this._vertical.MouseWheelBarPartitions = 10;
            this._vertical.Name = "_vertical";
            this._vertical.Orientation = MetroFramework.Controls.MetroScrollOrientation.Vertical;
            this._vertical.ScrollbarSize = 10;
            this._vertical.Size = new System.Drawing.Size(10, 200);
            this._vertical.TabIndex = 0;
            this._vertical.UseSelectable = true;
            // 
            // _horizontal
            // 
            this._horizontal.LargeChange = 10;
            this._horizontal.Location = new System.Drawing.Point(0, 0);
            this._horizontal.Maximum = 100;
            this._horizontal.Minimum = 0;
            this._horizontal.MouseWheelBarPartitions = 10;
            this._horizontal.Name = "_horizontal";
            this._horizontal.Orientation = MetroFramework.Controls.MetroScrollOrientation.Horizontal;
            this._horizontal.ScrollbarSize = 10;
            this._horizontal.Size = new System.Drawing.Size(200, 10);
            this._horizontal.TabIndex = 0;
            this._horizontal.UseSelectable = true;
            // 
            // pnlAction
            // 
            this.pnlAction.BackColor = System.Drawing.Color.Transparent;
            this.pnlAction.Controls.Add(this.lnkDelete);
            this.pnlAction.Controls.Add(this.lnkEdit);
            this.pnlAction.Location = new System.Drawing.Point(120, 86);
            this.pnlAction.Name = "pnlAction";
            this.pnlAction.Size = new System.Drawing.Size(44, 20);
            this.pnlAction.TabIndex = 5;
            // 
            // lnkDelete
            // 
            this.lnkDelete.Dock = System.Windows.Forms.DockStyle.Left;
            this.lnkDelete.Image = ((System.Drawing.Image)(resources.GetObject("lnkDelete.Image")));
            this.lnkDelete.ImageSize = 0;
            this.lnkDelete.Location = new System.Drawing.Point(22, 0);
            this.lnkDelete.Name = "lnkDelete";
            this.lnkDelete.NoFocusImage = ((System.Drawing.Image)(resources.GetObject("lnkDelete.NoFocusImage")));
            this.lnkDelete.Size = new System.Drawing.Size(22, 20);
            this.lnkDelete.TabIndex = 1;
            this.lnkDelete.UseCustomBackColor = true;
            this.lnkDelete.UseSelectable = true;
            // 
            // lnkEdit
            // 
            this.lnkEdit.Dock = System.Windows.Forms.DockStyle.Left;
            this.lnkEdit.Image = ((System.Drawing.Image)(resources.GetObject("lnkEdit.Image")));
            this.lnkEdit.ImageSize = 0;
            this.lnkEdit.Location = new System.Drawing.Point(0, 0);
            this.lnkEdit.Name = "lnkEdit";
            this.lnkEdit.NoFocusImage = ((System.Drawing.Image)(resources.GetObject("lnkEdit.NoFocusImage")));
            this.lnkEdit.Size = new System.Drawing.Size(22, 20);
            this.lnkEdit.TabIndex = 0;
            this.lnkEdit.UseCustomBackColor = true;
            this.lnkEdit.UseSelectable = true;
            // 
            // MetroGrid
            // 
            this.Padding = new System.Windows.Forms.Padding(0, 0, 15, 15);
            this.Rows.DefaultSize = 19;
            this.pnlAction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroScrollBar _vertical;
        private MetroFramework.Controls.MetroScrollBar _horizontal;
        private System.Windows.Forms.Panel pnlAction;
        private MetroFramework.Controls.MetroLink lnkDelete;
        private MetroFramework.Controls.MetroLink lnkEdit;
    }
}
