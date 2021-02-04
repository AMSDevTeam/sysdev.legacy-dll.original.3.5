''' <summary>
''' Loading form.
''' </summary>
''' <remarks></remarks>
<Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
Public Class LoadingForm

#Region "Variable Declarations"
    Dim _sync As IAsyncResult = Nothing
    Dim _thrd As Thread = Nothing
#End Region

#Region "Sub New"
    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Sub New(ByVal thread As Thread)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _thrd = thread
    End Sub

    Sub New(ByVal sync As IAsyncResult)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _sync = sync
    End Sub

#End Region

#Region "Properties"
    Private _isdialog As Boolean = False

    ''' <summary>
    ''' Gets or sets whether form is currently show in modal mode or not.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsDialog() As Boolean
        Get
            Return _isdialog
        End Get
        Set(ByVal value As Boolean)
            _isdialog = value
        End Set
    End Property

    Dim _isopen As Boolean = False
    ''' <summary>
    ''' Gets whether loading form is currently displayed in the screen or not.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsOpen() As Boolean
        Get
            Return _isopen
        End Get
    End Property
#End Region

#Region "Methods"
    Private Sub PictureBoxToCenter()
        Dim pctlocation As Point = New Point((Size.Width / 2) - (pctLoad.Size.Width / 2), _
                                             (Size.Height / 2) - (pctLoad.Size.Height / 2))
        pctLoad.Location = pctlocation
        pctBlack.Location = pctlocation
        pctBlue.Location = pctlocation
        pctRed.Location = pctlocation
        pctLoad.BringToFront()
    End Sub
#End Region

#Region "Functions"
    ''' <summary>
    ''' Gets the default loading images.
    ''' </summary>
    ''' <param name="image"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLoadImage(ByVal image As LoadScreen.LoadingImageEnum) As Image
        Dim img As Image = pctBlack.Image

        Select Case image
            Case LoadScreen.LoadingImageEnum.AjaxLoadBlue : img = pctBlue.Image
            Case LoadScreen.LoadingImageEnum.AjaxLoadRed : img = pctRed.Image
            Case LoadScreen.LoadingImageEnum.Custom : img = LoadScreen.CustomImage
            Case Else
        End Select

        Return img
    End Function
#End Region

#Region "Events"
    Private Sub LoadingForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        _isopen = False
    End Sub

    Private Sub LoadingForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Select Case LoadScreen.LoadingImage
            Case LoadScreen.LoadingImageEnum.AjaxLoadBlue : pctLoad.Image = pctBlue.Image
            Case LoadScreen.LoadingImageEnum.AjaxLoadRed : pctLoad.Image = pctRed.Image
            Case LoadScreen.LoadingImageEnum.Custom
                If LoadScreen.CustomImage IsNot Nothing Then pctLoad.Image = LoadScreen.CustomImage
            Case Else
        End Select

        'ShowInTaskbar = IsDialog : ShowIcon = IsDialog
        Opacity = 0.64 : WindowState = FormWindowState.Maximized

        Dim mainfrm As Form = Nothing

        For Each frm As Form In Application.OpenForms
            If frm.Name.In("frmMain", "frmMain".ToLower, "frmMain".ToProper, _
                           "MainForm", "MainForm".ToLower, "MainForm".ToUpper) Then
                mainfrm = frm : Exit For
            End If
        Next

        If mainfrm IsNot Nothing Then
            If mainfrm.Visible Then
                WindowState = FormWindowState.Normal
                Size = mainfrm.Size : Location = mainfrm.Location
            Else : WindowState = FormWindowState.Maximized
            End If
        Else
            mainfrm = Form.ActiveForm
            If mainfrm Is Nothing Then : WindowState = FormWindowState.Maximized
            Else
                If mainfrm.Visible Then
                    WindowState = FormWindowState.Normal
                    Size = mainfrm.Size : Location = mainfrm.Location
                Else : WindowState = FormWindowState.Maximized
                End If
            End If
        End If

        ControlBox = False : MaximizeBox = False : MinimizeBox = False : PictureBoxToCenter()
    End Sub

    Private Sub LoadingForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        _isopen = True

        If _sync IsNot Nothing Then
            While Not _sync.IsCompleted
                Thread.Sleep(1) : Application.DoEvents()
            End While
            Me.Close()
        End If

        If _thrd IsNot Nothing Then
            While _thrd.IsAlive
                Thread.Sleep(1) : Application.DoEvents()
            End While
            Me.Close()
        End If
    End Sub

    Private Sub LoadingForm_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        PictureBoxToCenter()
    End Sub
#End Region

End Class