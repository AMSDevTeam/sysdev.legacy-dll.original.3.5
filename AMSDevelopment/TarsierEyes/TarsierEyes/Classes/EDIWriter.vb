Namespace EDI
    ''' <summary>
    ''' Class for creating a custom-extensioned text file with database importations and encryption features.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EDIWriter
        Implements IDisposable

#Region "Varuable Declarations"
        Private runningdelegate As Object = Nothing
#End Region

#Region "Properties"
        Dim _encryptionkey As String = String.Empty
        ''' <summary>
        ''' Gets or sets encryption key to use; setting to an empty string will result to unencrypted text contents.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EncrytionKey() As String
            Get
                Return _encryptionkey
            End Get
            Set(ByVal value As String)
                _encryptionkey = value
            End Set
        End Property

        Dim _filename As String = String.Empty
        ''' <summary>
        ''' Gets the filename spcified upon creating a new instance.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Filename() As String
            Get
                Return _filename
            End Get
        End Property

        Dim _text As String = String.Empty
        ''' <summary>
        ''' Gets the text to be written in the file specified upon creating a new instance.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Text() As String
            Get
                Return _text
            End Get
        End Property
#End Region

#Region "Sub New"
        ''' <summary>
        ''' Creates new instance of TarsierEyes.EDI.EDIWriter.
        ''' </summary>
        ''' <param name="filename">EDI filename</param>
        ''' <param name="text">Text to be written</param>
        ''' <remarks>Unencrypted text contents if EncryptionKey property is not set.</remarks>
        Sub New(ByVal filename As String, ByVal text As String)
            _filename = filename
            _encryptionkey = String.Empty : _text = text
            runningdelegate = Nothing
        End Sub

        ''' <summary>
        ''' Creates new instance of TarsierEyes.EDI.EDIWriter.
        ''' </summary>
        ''' <param name="filename">EDI filename</param>
        ''' <param name="text">Text to be written</param>
        ''' <param name="enryptionkey">Encryption key to use for text encrypting.</param>
        ''' <remarks></remarks>
        Sub New(ByVal filename As String, ByVal text As String, ByVal enryptionkey As String)
            _filename = filename
            _encryptionkey = enryptionkey : _text = text
            runningdelegate = Nothing
        End Sub
#End Region

#Region "Internal Functions"
        ''' <summary>
        ''' Returns IAsyncResult interface of a .Write call.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BeginInvoke() As IAsyncResult
            runningdelegate = New Func(Of Boolean)(AddressOf Write)
            Dim ar As IAsyncResult = CType(runningdelegate, Func(Of Boolean)).BeginInvoke(Nothing, runningdelegate)
            Return ar
        End Function

        ''' <summary>
        ''' Gets the result of .Write call and ending the running delegate.
        ''' </summary>
        ''' <param name="ar">IAsyncResult interface that was ended.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EndInvoke(ByVal ar As IAsyncResult) As Boolean
            If runningdelegate IsNot Nothing Then
                Try
                    Return CType(runningdelegate, Func(Of Boolean)).EndInvoke(ar)
                Catch ex As Exception
                    Return False
                End Try
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Writes the specified text in this instance to the specified file.
        ''' </summary>
        ''' <returns>False if writing fails.</returns>
        ''' <remarks></remarks>
        Public Overloads Function Write() As Boolean
            Dim bWrite As Boolean = False

            Dim text As String = _text
            If Not String.IsNullOrEmpty(_encryptionkey.Trim) Then text = Cryptography.Encryption.Encrypt(_text, _encryptionkey.Trim)

            Dim sw As New StreamWriter(_filename)
            With sw
                Try
                    .Write(text) : bWrite = True
                Catch ex As Exception
                Finally
                    .Close() : .Dispose()
                End Try
            End With

            Return bWrite
        End Function
#End Region

#Region "Shared Functions"
        ''' <summary>
        '''  Writes the specified text to the specified file.
        ''' </summary>
        ''' <param name="filename">EDI filename.</param>
        ''' <param name="text">Text to be written.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Write(ByVal filename As String, ByVal text As String) As Boolean
            Dim edi As New EDIWriter(filename, text)
            With edi
                Dim b As Boolean = .Write
                .Dispose()
                Return b
            End With
        End Function

        ''' <summary>
        ''' Writes the specified text to the specified file.
        ''' </summary>
        ''' <param name="filename">EDI filename.</param>
        ''' <param name="text">Text to be written.</param>
        ''' <param name="encryptionkey">Encryption key to use.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Write(ByVal filename As String, ByVal text As String, ByVal encryptionkey As String) As Boolean
            Dim edi As New EDIWriter(filename, text, encryptionkey)
            With edi
                Dim b As Boolean = .Write
                .Dispose()
                Return b
            End With
        End Function

        ''' <summary>
        ''' Writes clip-delimited database resultset contents to the specified file. 
        ''' </summary>
        ''' <param name="filename">EDI filename.</param>
        ''' <param name="connection">MySQL connection string</param>
        ''' <param name="commandtext">MySQL select statement</param>
        ''' <param name="encryptionkey">Encryption key to use</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Write(ByVal filename As String, ByVal connection As String, ByVal commandtext As String, ByVal encryptionkey As String) As Boolean
            Dim b As Boolean = False

            Dim q As New MySQL.Que(connection, commandtext)
            With q
                .ExecuteReader()
                If .Rows > 0 Then
                    Dim contents As New StringBuilder

                    For iRow As Integer = 0 To .Rows - 1
                        For iCol As Integer = 0 To .Columns - 1
                            With .DataTable.Rows(iRow)
                                If IsDBNull(.Item(iCol)) Then
                                    contents.Append("|")
                                Else
                                    Select Case q.DataTable.Columns(iCol).DataType.Name
                                        Case GetType(SByte).Name, GetType(Byte).Name, "Byte[]", "SByte[]"
                                            Try
                                                contents.Append("|" & BitConverter.ToString(.Item(iCol)).Replace("-", "").Trim.Replace("|", "jsphClip;").Replace(Chr(13), "jsphNewLine;"))
                                            Catch ex As Exception
                                                contents.Append("|")
                                            End Try

                                        Case Else
                                            contents.Append("|" & .Item(iCol).ToString.Trim.Replace("|", "jsphClip;").Replace(Chr(13), "jsphNewLine;"))
                                    End Select
                                End If
                            End With
                        Next
                        contents.Append(vbNewLine)
                    Next

                    If Not String.IsNullOrEmpty(contents.ToString.Trim) Then
                        Dim edi As New EDIWriter(filename, contents.ToString, encryptionkey)
                        With edi
                            b = .Write : .Dispose()
                        End With
                    End If
                End If
                .Dispose()
            End With

            Return b
        End Function

        ''' <summary>
        ''' Writes contents using MySQL dump into an specified file.
        ''' </summary>
        ''' <param name="filename">EDI filename.</param>
        ''' <param name="connection">MySQL connection string.</param>
        ''' <param name="dumpparameters">Valid MySQL dump parameter list.</param>
        ''' <param name="encryptionkey">Encryption key to use.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Write(ByVal filename As String, ByVal connection As String, ByVal dumpparameters As MySQL.Dump.DumpParameterCollection, ByVal encryptionkey As String) As Boolean
            Dim b As Boolean = False

            Dim server As String = MySQL.ConnectionStringValue(connection, MySQL.ConnectionDetailEnum.Server)
            Dim database As String = MySQL.ConnectionStringValue(connection, MySQL.ConnectionDetailEnum.Database)
            Dim userid As String = MySQL.ConnectionStringValue(connection, MySQL.ConnectionDetailEnum.UID)
            Dim password As String = MySQL.ConnectionStringValue(connection, MySQL.ConnectionDetailEnum.PWD)

            Dim dump As New MySQL.Dump(server, database, userid, password, filename)
            With dump
                If .CanExport Then
                    .Parameters.Clear()
                    For Each s As String In dumpparameters
                        .Parameters.Add(s)
                    Next
                    b = .Export
                End If
                .Dispose()
            End With

            If File.Exists(filename) Then
                Try
                    Dim sr As New StreamReader(filename)
                    Dim contents As New StringBuilder
                    With sr
                        contents.Append(.ReadToEnd)
                        .Close() : .Dispose()
                    End With

                    If Not String.IsNullOrEmpty(contents.ToString.Trim) Then
                        Dim edi As New EDIWriter(filename, contents.ToString, encryptionkey)
                        With edi
                            b = .Write : .Dispose()
                        End With
                    End If
                Catch ex As Exception
                    b = False
                End Try
            End If

            Return b
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
                    runningdelegate = Nothing : Common.Simple.RefreshAndManageCurrentProcess()
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