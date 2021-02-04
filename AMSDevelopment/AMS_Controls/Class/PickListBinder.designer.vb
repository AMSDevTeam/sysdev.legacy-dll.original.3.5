<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PickListBinder
    Inherits MetroFramework.Controls.MetroUserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PickListBinder))
        Me.mcbPrint = New MetroFramework.Controls.MetroCheckBox()
        Me.mprProgress = New MetroFramework.Controls.MetroProgressBar()
        Me.lblStatus = New MetroFramework.Controls.MetroLabel()
        Me.lblTitle = New MetroFramework.Controls.MetroLabel()
        Me.lnkSave = New MetroFramework.Controls.MetroLink()
        Me.lnkCancel = New MetroFramework.Controls.MetroLink()
        Me.pnlLineHeader = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        'mcbPrint
        '
        Me.mcbPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mcbPrint.AutoSize = True
        Me.mcbPrint.Location = New System.Drawing.Point(388, 328)
        Me.mcbPrint.Name = "mcbPrint"
        Me.mcbPrint.Size = New System.Drawing.Size(101, 15)
        Me.mcbPrint.TabIndex = 20
        Me.mcbPrint.Text = "&print after save"
        Me.mcbPrint.UseSelectable = True
        '
        'mprProgress
        '
        Me.mprProgress.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.mprProgress.Location = New System.Drawing.Point(0, 349)
        Me.mprProgress.Name = "mprProgress"
        Me.mprProgress.Size = New System.Drawing.Size(517, 8)
        Me.mprProgress.TabIndex = 19
        Me.mprProgress.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.AutoSize = True
        Me.lblStatus.FontSize = MetroFramework.MetroLabelSize.Small
        Me.lblStatus.FontWeight = MetroFramework.MetroLabelWeight.Regular
        Me.lblStatus.Location = New System.Drawing.Point(0, 331)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(0, 0)
        Me.lblStatus.TabIndex = 21
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblStatus.Visible = False
        '
        'lblTitle
        '
        Me.lblTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTitle.FontWeight = MetroFramework.MetroLabelWeight.Regular
        Me.lblTitle.Location = New System.Drawing.Point(9, 4)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(411, 35)
        Me.lblTitle.TabIndex = 22
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lnkSave
        '
        Me.lnkSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lnkSave.AutoSize = True
        Me.lnkSave.Image = CType(resources.GetObject("lnkSave.Image"), System.Drawing.Image)
        Me.lnkSave.ImageSize = 32
        Me.lnkSave.Location = New System.Drawing.Point(423, 5)
        Me.lnkSave.Name = "lnkSave"
        Me.lnkSave.NoFocusImage = CType(resources.GetObject("lnkSave.NoFocusImage"), System.Drawing.Image)
        Me.lnkSave.Size = New System.Drawing.Size(32, 34)
        Me.lnkSave.TabIndex = 41
        Me.lnkSave.UseSelectable = True
        '
        'lnkCancel
        '
        Me.lnkCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lnkCancel.AutoSize = True
        Me.lnkCancel.Image = CType(resources.GetObject("lnkCancel.Image"), System.Drawing.Image)
        Me.lnkCancel.ImageSize = 32
        Me.lnkCancel.Location = New System.Drawing.Point(461, 5)
        Me.lnkCancel.Name = "lnkCancel"
        Me.lnkCancel.NoFocusImage = CType(resources.GetObject("lnkCancel.NoFocusImage"), System.Drawing.Image)
        Me.lnkCancel.Size = New System.Drawing.Size(32, 34)
        Me.lnkCancel.TabIndex = 42
        Me.lnkCancel.UseSelectable = True
        '
        'pnlLineHeader
        '
        Me.pnlLineHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLineHeader.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.pnlLineHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlLineHeader.Location = New System.Drawing.Point(3, 43)
        Me.pnlLineHeader.Name = "pnlLineHeader"
        Me.pnlLineHeader.Size = New System.Drawing.Size(512, 1)
        Me.pnlLineHeader.TabIndex = 43
        '
        'PickListBinder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pnlLineHeader)
        Me.Controls.Add(Me.lnkCancel)
        Me.Controls.Add(Me.lnkSave)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.mcbPrint)
        Me.Controls.Add(Me.mprProgress)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "PickListBinder"
        Me.Size = New System.Drawing.Size(517, 357)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents mcbPrint As MetroFramework.Controls.MetroCheckBox
    Private WithEvents mprProgress As MetroFramework.Controls.MetroProgressBar
    Private WithEvents lblStatus As MetroFramework.Controls.MetroLabel
    Friend WithEvents lblTitle As MetroFramework.Controls.MetroLabel
    Private WithEvents lnkSave As MetroFramework.Controls.MetroLink
    Private WithEvents lnkCancel As MetroFramework.Controls.MetroLink
    Friend WithEvents pnlLineHeader As System.Windows.Forms.Panel
End Class
