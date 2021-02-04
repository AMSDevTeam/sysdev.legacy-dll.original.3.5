Namespace Controls
    ''' <summary>
    ''' Textbox editor with sizeable grip border.
    ''' </summary>
    ''' <remarks></remarks>
    <Description("Multiline textbox editor with sizeable grip border."), ToolboxBitmap(GetType(GrippedTextBox), "GrippedTextBox.bmp")> _
    Public Class GrippedTextBox
        Inherits TextBox

#Region "Variable Declarations"
        Friend WithEvents lblGrip As System.Windows.Forms.Label
        Private components As Container = Nothing
        Private pointdown As Point
        Private sizedown As Size
#End Region

#Region "Sub New"
        ''' <summary>
        ''' Creates a new instance of TarsierEyes.Controls.GrippedTextBox.
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            InitializeComponent()
        End Sub
#End Region

#Region "Methods"
        ''' <summary>
        ''' Releases the unmanaged resources used by the TarsierEyes.Controls.GrippedTextBox and optionally releases the managed resources.
        ''' </summary>
        ''' <param name="disposing"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If components IsNot Nothing Then components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub InitializeComponent()
            Me.lblGrip = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            'lblGrip
            '
            Me.lblGrip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblGrip.BackColor = System.Drawing.Color.Transparent
            Me.lblGrip.Cursor = System.Windows.Forms.Cursors.SizeNWSE
            Me.lblGrip.Image = Global.TarsierEyes.My.Resources.Resources.Grip3
            Me.lblGrip.Location = New System.Drawing.Point(100, 0)
            Me.lblGrip.Name = "lblGrip"
            Me.lblGrip.Size = New System.Drawing.Size(16, 16)
            Me.lblGrip.TabIndex = 1
            '
            'GrippedTextBox
            '
            Me.BackColor = System.Drawing.Color.White
            Me.Controls.Add(Me.lblGrip)
            Me.MinimumSize = New System.Drawing.Size(120, 20)
            Me.Multiline = True
            Me.Size = New System.Drawing.Size(120, 20)
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
#End Region

#Region "Events"
        Private Sub lblGrip_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblGrip.MouseDown
            pointdown = Control.MousePosition
            sizedown = Size
        End Sub

        Private Sub lblGrip_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblGrip.MouseMove
            If e.Button = Windows.Forms.MouseButtons.Left Then
                Dim point As Point = Control.MousePosition
                Size = New Size(sizedown.Width + (point.X - pointdown.X), _
                                sizedown.Height + (point.Y - pointdown.Y))
            End If
        End Sub
#End Region

    End Class
End Namespace

