''' <summary>
''' Class for text encryption and decryption.
''' </summary>
''' <remarks></remarks>
Partial Public Class Cryptography
    ''' <summary>
    ''' Hash byte generator using MD5.
    ''' </summary>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never), Browsable(False)> _
    Public Class MD5Hash
        Implements IDisposable

        Private MD5 As New MD5CryptoServiceProvider
        Dim _value As String

#Region "Sub New"
        ''' <summary>
        ''' Creates a new instance of TarsierEyes.Cryptography.MD5Hash.
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            _value = String.Empty
        End Sub

        ''' <summary>
        ''' Creates a new instance of TarsierEyes.Cryptography.MD5Hash.
        ''' </summary>
        ''' <param name="value">Text to be evaluated for the hash creation</param>
        ''' <remarks></remarks>
        Sub New(ByVal value As String)
            _value = value
        End Sub
#End Region

#Region "Properties"
        ''' <summary>
        ''' Gets the hash byte value generated for the given key value.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Hash() As Byte()
            Get
                Return GetHash()
            End Get
        End Property
#End Region

#Region "Functions"
        Private Function GetHash() As Byte()
            Return MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(_value))
        End Function
#End Region

#Region " IDisposable Support "
        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        ''' <summary>
        ''' Dispose off any resources used by the class to free up memory space.
        ''' </summary>
        ''' <param name="disposing"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free other state (managed objects).
                    Common.Simple.RefreshAndManageCurrentProcess()
                End If

                ' TODO: free your own state (unmanaged objects).
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        ''' <summary>
        ''' Dispose off any resources used by the class to free up memory space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Class