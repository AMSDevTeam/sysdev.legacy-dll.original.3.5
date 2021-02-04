Partial Public Class Cryptography
    ''' <summary>
    ''' Class for decrypting text encrypted by TarsierEyes.Cryptography.Encrypt class (using MD5 and TripleDES).
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Decryption
        Implements IDisposable

#Region "Variable Declarations"
        Private DES As New TripleDESCryptoServiceProvider
#End Region

#Region "Sub New"
        ''' <summary>
        ''' Creates a new instance of TarsierEyes.Cryptography.Decryption.
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            _text = String.Empty : _key = String.Empty
        End Sub

        ''' <summary>
        ''' Creates a new instance of TarsierEyes.Cryptography.Decryption.
        ''' </summary>
        ''' <param name="text">Text to be decrypted</param>
        ''' <param name="key">Encryption key</param>
        ''' <remarks></remarks>
        Sub New(ByVal text As String, ByVal key As String)
            _text = text : _key = key
        End Sub
#End Region

#Region "Properties"
        Dim _key As String = String.Empty
        ''' <summary>
        ''' Gets or sets encryption key to be use as the decryption pattern.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Key() As String
            Get
                Return _key
            End Get
            Set(ByVal value As String)
                _key = value
            End Set
        End Property

        Dim _text As String = String.Empty
        ''' <summary>
        ''' Gets or sets text to be decrypted.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Text() As String
            Get
                Return _text
            End Get
            Set(ByVal value As String)
                _text = value
            End Set
        End Property
#End Region

#Region "Overrides"
        ''' <summary>
        ''' Gets the decrypted plain text of the specified encrypted text.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String
            Dim sDecrypted As String = _text
            Dim mh As New MD5Hash(_key)

            Try
                With DES
                    .Key = mh.Hash : .Mode = CipherMode.ECB
                End With
                Dim Buffer As Byte() = Convert.FromBase64String(_text)
                sDecrypted = ASCIIEncoding.ASCII.GetString(DES.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length))
            Catch ex As Exception

            Finally : mh.Dispose()
            End Try

            Return sDecrypted
        End Function
#End Region

#Region "Shared Functions"
        ''' <summary>
        ''' Calls Decryption class' ToString overriden function to decipher an encrypted text using the supplied decryption key.
        ''' </summary>
        ''' <param name="text">Text to decrypt.</param>
        ''' <param name="key">Encryption key.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Decrypt(ByVal text As String, ByVal key As String) As String
            Dim d As New Decryption(text, key)
            Dim value As String = d.ToString
            d.Dispose()
            Return value
        End Function

        ''' <summary>
        ''' Decrypts the specified file's contents using the supplied decryption key string pattern.
        ''' </summary>
        ''' <param name="file"></param>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Decrypt(ByVal file As IO.FileInfo, ByVal key As String) As Boolean
            Dim decrypted As Boolean = False

            If IO.File.Exists(file.FullName) Then
                Dim sr As New IO.StreamReader(file.FullName, Encoding.UTF8)
                Dim contents As New StringBuilder

                With sr
                    Try
                        contents.Append(.ReadToEnd)
                    Catch ex As Exception
                    Finally
                        .Close() : .Dispose()
                    End Try
                End With

                If Not String.IsNullOrEmpty(contents.ToString) Then
                    Dim ro As Boolean = file.IsReadOnly : file.IsReadOnly = False
                    Dim sw As New IO.StreamWriter(file.FullName, False, Encoding.UTF8)
                    With sw
                        Try
                            .Write(Decrypt(contents.ToString, key)) : decrypted = True
                        Catch ex As Exception
                        Finally
                            .Close() : .Dispose()
                        End Try
                    End With
                    file.IsReadOnly = ro
                End If
            End If

            Return decrypted
        End Function

        ''' <summary>
        ''' Decrypts the given simply encrypted text based on its decryption key's character count.
        ''' </summary>
        ''' <param name="text">Text to decrypt.</param>
        ''' <param name="key">Encryption key.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function SimpleDecrypt(ByVal text As String, ByVal key As String) As String
            Dim _result As String = String.Empty : Dim keycount As Integer = key.Trim.Length

            Dim chars() As Char = text.ToCharArray
            If keycount <= 7 Then keycount = 7

            For Each c As Char In chars
                _result &= Chr(Asc(c) - keycount)
            Next

            Return _result
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
                    DES.Clear() : Common.Simple.RefreshAndManageCurrentProcess()
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