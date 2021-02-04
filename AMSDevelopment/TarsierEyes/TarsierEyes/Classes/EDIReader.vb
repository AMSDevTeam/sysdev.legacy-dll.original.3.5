Namespace EDI
    ''' <summary>
    ''' Class to open and read contents of an EDIWriter-generated file.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EDIReader
        Implements IDisposable

#Region "Properties"
        Dim _filename As String = String.Empty

        ''' <summary>
        ''' Gets the filename to read the contents from.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Filename() As String
            Get
                Return _filename
            End Get
        End Property

        Dim _encryptionkey As String = String.Empty
        ''' <summary>
        ''' Gets or sets decryption key to decipher encrypted contents of the file.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DecryptionKey() As String
            Get
                Return _encryptionkey
            End Get
            Set(ByVal value As String)
                _encryptionkey = value
            End Set
        End Property
#End Region

#Region "Sub New"
        ''' <summary>
        ''' Create a new instance of TarsierEyes.EDI.EDIReader.
        ''' </summary>
        ''' <param name="filename">Filename to read the contents from.</param>
        ''' <remarks></remarks>
        Sub New(ByVal filename As String)
            _filename = filename : _encryptionkey = String.Empty
        End Sub

        ''' <summary>
        ''' Create a new instance of TarsierEyes.EDI.EDIReader.
        ''' </summary>
        ''' <param name="filename">Filename to read the contents from.</param>
        ''' <param name="decryptionkey">Decryption key to decipher encrypted contents of the file.</param>
        ''' <remarks></remarks>
        Sub New(ByVal filename As String, ByVal decryptionkey As String)
            _filename = filename : _encryptionkey = decryptionkey
        End Sub
#End Region

#Region "Internal Functions"
        ''' <summary>
        ''' Retreives the unencrypted contents of the specified EDI file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Read() As String
            Dim contents As New StringBuilder
            Dim temp As New StringBuilder
            Dim sr As New StreamReader(_filename)

            Try
                temp.Append(sr.ReadToEnd)
            Catch ex As Exception
            Finally
                sr.Close() : sr.Dispose()
                Common.Simple.RefreshAndManageCurrentProcess()
            End Try

            If Not String.IsNullOrEmpty(_encryptionkey.Trim) And _
               Not String.IsNullOrEmpty(temp.ToString.Trim) Then : contents.Append(Cryptography.Decryption.Decrypt(temp.ToString.Trim, _encryptionkey.Trim).Replace("jsphNewLine;", Chr(13)))
            Else : contents.Append(temp.ToString)
            End If

            Return contents.ToString
        End Function

        ''' <summary>
        ''' Retreives the unencrypted contents of the specified EDI file and breaks it into interpreted lines.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function ReadLines() As List(Of String)
            Dim contents As New StringBuilder
            Dim temp As New StringBuilder
            Dim lines As New List(Of String)
            Dim sr As New StreamReader(_filename)

            Try
                temp.Append(sr.ReadToEnd)
            Catch ex As Exception
            Finally
                sr.Close() : sr.Dispose()
            End Try

            If Not String.IsNullOrEmpty(_encryptionkey.Trim) And _
               Not String.IsNullOrEmpty(temp.ToString.Trim) Then : contents.Append(Cryptography.Decryption.Decrypt(temp.ToString.Trim, _encryptionkey.Trim))
            Else : contents.Append(temp.ToString)
            End If

            If Not String.IsNullOrEmpty(contents.ToString.Trim) Then
                Dim textlines() As String = contents.ToString.Trim.Split(vbNewLine)
                For Each line As String In textlines
                    lines.Add(line.Trim.Replace("jsphNewLine;", Chr(13)))
                Next
            End If

            Return lines
        End Function
#End Region

#Region "Shared Functions"
        ''' <summary>
        '''  Retreives the unencrypted contents of the specified EDI file.
        ''' </summary>
        ''' <param name="filename">Filename of the EDI to retreive the contents from.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Read(ByVal filename As String) As String
            Dim contents As New StringBuilder

            Dim edi As New EDIReader(filename)
            With edi
                contents.Append(.Read) : .Dispose()
            End With

            Return contents.ToString
        End Function

        ''' <summary>
        '''  Retreives the unencrypted contents of the specified EDI file.
        ''' </summary>
        ''' <param name="filename">Filename of the EDI to retreive the contents from.</param>
        ''' <param name="decryptionkey">Decryption key to decipher encrypted contents of the file.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Read(ByVal filename As String, ByVal decryptionkey As String) As String
            Dim contents As New StringBuilder

            Dim edi As New EDIReader(filename, decryptionkey)
            With edi
                contents.Append(.Read) : .Dispose()
            End With

            Return contents.ToString
        End Function

        ''' <summary>
        ''' Retreives the unencrypted contents of the specified EDI file and breaks it into interpreted lines.
        ''' </summary>
        ''' <param name="filename">Filename of the EDI to retreive the contents from.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function ReadLines(ByVal filename As String) As List(Of String)
            Dim lines As New List(Of String)

            Dim edi As New EDIReader(filename)
            With edi
                For Each line As String In .ReadLines
                    lines.Add(line)
                Next
                .Dispose()
            End With

            Return lines
        End Function

        ''' <summary>
        ''' Retreives the unencrypted contents of the specified EDI file and breaks it into interpreted lines.
        ''' </summary>
        ''' <param name="filename">Filename of the EDI to retreive the contents from.</param>
        ''' <param name="decryptionkey">Decryption key to decipher encrypted contents of the file.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function ReadLines(ByVal filename As String, ByVal decryptionkey As String) As List(Of String)
            Dim lines As New List(Of String)

            Dim edi As New EDIReader(filename, decryptionkey)
            With edi
                For Each line As String In .ReadLines
                    lines.Add(line)
                Next
                .Dispose()
            End With

            Return lines
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
End Namespace