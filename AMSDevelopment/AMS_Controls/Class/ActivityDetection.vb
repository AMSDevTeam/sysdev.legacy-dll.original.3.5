''' <summary>
''' Class for idle time detection.
''' </summary>
''' <remarks></remarks>
Public Class ActivityDetection
    Implements IDisposable, IMessageFilter

#Region "Enumerations"
    Enum Win32Message
        WM_KEYFIRST = &H100
        WM_KEYDOWN = &H100
        WM_KEYUP = &H101
        WM_CHAR = &H102
        WM_DEADCHAR = &H103
        WM_SYSKEYDOWN = &H104
        WM_SYSKEYUP = &H105
        WM_SYSCHAR = &H106
        WM_SYSDEADCHAR = &H107

        WM_MOUSEFIRST = &H200
        WM_MOUSEMOVE = &H200
        WM_LBUTTONDOWN = &H201
        WM_LBUTTONUP = &H202
        WM_LBUTTONDBLCLK = &H203
        WM_RBUTTONDOWN = &H204
        WM_RBUTTONUP = &H205
        WM_RBUTTONDBLCLK = &H206
        WM_MBUTTONDOWN = &H207
        WM_MBUTTONUP = &H208
        WM_MBUTTONDBLCLK = &H209
        WM_MOUSEWHEEL = &H20A
    End Enum
#End Region

#Region "Properties"
    Private _lastActivity As Date

    Public ReadOnly Property LastActivity() As Date
        Get
            Return _lastActivity
        End Get
    End Property
#End Region

    ''' <summary>
    ''' Creates a new instance of ActivityDetection.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _lastActivity = Date.Now
        Windows.Forms.Application.AddMessageFilter(Me)
    End Sub

    Public Function PreFilterMessage(ByRef m As System.Windows.Forms.Message) As Boolean Implements IMessageFilter.PreFilterMessage
        Select Case m.Msg
            Case Win32Message.WM_KEYUP, Win32Message.WM_KEYDOWN : _lastActivity = Date.Now
            Case Win32Message.WM_LBUTTONDOWN, Win32Message.WM_LBUTTONUP, Win32Message.WM_MBUTTONDOWN, Win32Message.WM_MBUTTONUP, Win32Message.WM_RBUTTONDOWN, Win32Message.WM_RBUTTONUP, Win32Message.WM_MOUSEMOVE, Win32Message.WM_MOUSEWHEEL : _lastActivity = Date.Now
        End Select

        Return False
    End Function

    ''' <summary>
    ''' Dispose off any resources used by the class to free up memory space.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements System.IDisposable.Dispose
        Windows.Forms.Application.RemoveMessageFilter(Me)
    End Sub
End Class
