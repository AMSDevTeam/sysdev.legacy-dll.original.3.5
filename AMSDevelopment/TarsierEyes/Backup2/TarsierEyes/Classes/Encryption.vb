Partial Public Class Cryptography
    ''' <summary>
    ''' Class to encrypt plain text using MD5 and TripleDES Crypto Service Providers.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Encryption
        Implements IDisposable

#Region "Variable Declarations"
        Private DES As New TripleDESCryptoServiceProvider
#End Region

#Region "Sub New"
        ''' <summary>
        ''' Creates a new instance of TarsierEyes.Cryptography.Encryption.
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            _text = String.Empty : _key = String.Empty
        End Sub

        ''' <summary>
        ''' Creates a new instance of TarsierEyes.Cryptography.Encryption.
        ''' </summary>
        ''' <param name="text">Text to be encrypted.</param>
        ''' <param name="key">Encryption key.</param>
        ''' <remarks></remarks>
        Sub New(ByVal text As String, ByVal key As String)
            _text = text : _key = key
        End Sub
#End Region

#Region "Properties"
        Dim _key As String
        ''' <summary>
        ''' Gets or sets encryption key to be use as the encryption pattern.
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

        Dim _text As String
        ''' <summary>
        ''' Gets or sets text to be encrypted.
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
        ''' Gets the encrypted text of the specified plain readable text.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String
            Dim sEncrypted As String = _text
            Dim mh As New MD5Hash(_key)

            Try
                With DES
                    .Key = mh.Hash : .Mode = CipherMode.ECB
                End With
                Dim Buffer As Byte() = ASCIIEncoding.ASCII.GetBytes(_text)
                sEncrypted = Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(Buffer, 0, Buffer.Length))
            Catch ex As Exception
            Finally : mh.Dispose()
            End Try

            Return sEncrypted
        End Function
#End Region

#Region "Shared Functions"
        ''' <summary>
        ''' Calls Encryption class' ToString overriden function to encrypt a plain text using the given encryption key as its pattern.
        ''' </summary>
        ''' <param name="text">Plain text to encrypt.</param>
        ''' <param name="key">Encryption key.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Encrypt(ByVal text As String, ByVal key As String) As String
            Dim e As New Encryption(text, key)
            Dim value As String = e.ToString
            e.Dispose()
            Return value
        End Function

        ''' <summary>
        ''' Encrypts the contents of the specified file using the specified encryption key pattern string.
        ''' </summary>
        ''' <param name="file"></param>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Encrypt(ByVal file As IO.FileInfo, ByVal key As String) As Boolean
            Dim encrypted As Boolean = False

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
                            .Write(Encrypt(contents.ToString, key)) : encrypted = True
                        Catch ex As Exception
                        Finally
                            .Close() : .Dispose()
                        End Try
                    End With
                    file.IsReadOnly = ro
                End If
            End If

            Return encrypted
        End Function

        ''' <summary>
        ''' Encrypts the given string in a simple manner using the encryption key's number of character as its basis.
        ''' </summary>
        ''' <param name="text">Plain text to encrypt.</param>
        ''' <param name="key">Encryption key.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function SimpleEncrypt(ByVal text As String, ByVal key As String) As String
            Dim _result As String = String.Empty : Dim keycount As Integer = key.Trim.Length

            Dim chars() As Char = text.ToCharArray
            If keycount <= 7 Then keycount = 7

            For Each c As Char In chars
                _result &= Chr(Asc(c) + keycount)
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
