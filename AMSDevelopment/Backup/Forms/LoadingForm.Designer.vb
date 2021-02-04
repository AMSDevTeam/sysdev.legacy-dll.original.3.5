<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoadingForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LoadingForm))
        Me.pctBlack = New System.Windows.Forms.PictureBox
        Me.pctBlue = New System.Windows.Forms.PictureBox
        Me.pctRed = New System.Windows.Forms.PictureBox
        Me.pctLoad = New System.Windows.Forms.PictureBox
        CType(Me.pctBlack, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pctBlue, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pctRed, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pctLoad, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pctBlack
        '
        Me.pctBlack.Image = CType(resources.GetObject("pctBlack.Image"), System.Drawing.Image)
        Me.pctBlack.Location = New System.Drawing.Point(365, 221)
        Me.pctBlack.Name = "pctBlack"
        Me.pctBlack.Size = New System.Drawing.Size(76, 70)
        Me.pctBlack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pctBlack.TabIndex = 1
        Me.pctBlack.TabStop = False
        '
        'pctBlue
        '
        Me.pctBlue.Image = CType(resources.GetObject("pctBlue.Image"), System.Drawing.Image)
        Me.pctBlue.Location = New System.Drawing.Point(365, 221)
        Me.pctBlue.Name = "pctBlue"
        Me.pctBlue.Size = New System.Drawing.Size(76, 70)
        Me.pctBlue.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pctBlue.TabIndex = 2
        Me.pctBlue.TabStop = False
        '
        'pctRed
        '
        Me.pctRed.Image = CType(resources.GetObject("pctRed.Image"), System.Drawing.Image)
        Me.pctRed.Location = New System.Drawing.Point(365, 221)
        Me.pctRed.Name = "pctRed"
        Me.pctRed.Size = New System.Drawing.Size(76, 70)
        Me.pctRed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pctRed.TabIndex = 3
        Me.pctRed.TabStop = False
        '
        'pctLoad
        '
        Me.pctLoad.Image = CType(resources.GetObject("pctLoad.Image"), System.Drawing.Image)
        Me.pctLoad.Location = New System.Drawing.Point(365, 221)
        Me.pctLoad.Name = "pctLoad"
        Me.pctLoad.Size = New System.Drawing.Size(76, 70)
        Me.pctLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pctLoad.TabIndex = 4
        Me.pctLoad.TabStop = False
        '
        'LoadingForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.DimGray
        Me.ClientSize = New System.Drawing.Size(1460, 920)
        Me.Controls.Add(Me.pctBlack)
        Me.Controls.Add(Me.pctLoad)
        Me.Controls.Add(Me.pctRed)
        Me.Controls.Add(Me.pctBlue)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "LoadingForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Loading..."
        Me.TransparencyKey = System.Drawing.Color.White
        CType(Me.pctBlack, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pctBlue, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pctRed, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pctLoad, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pctBlack As System.Windows.Forms.PictureBox
    Friend WithEvents pctBlue As System.Windows.Forms.PictureBox
    Friend WithEvents pctRed As System.Windows.Forms.PictureBox
    Friend WithEvents pctLoad As System.Windows.Forms.PictureBox
End Class
