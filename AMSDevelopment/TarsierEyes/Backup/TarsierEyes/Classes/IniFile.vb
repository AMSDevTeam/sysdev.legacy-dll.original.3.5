Namespace EDI
    ''' <summary>
    ''' Ini file reader and writer.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IniFile
        Implements IDisposable

#Region "Sub New"
        ''' <summary>
        ''' Creates a new instance of IniReader.
        ''' </summary>
        ''' <param name="filename"></param>
        ''' <remarks></remarks>
        Sub New(ByVal filename As String)
            _filename = filename
        End Sub
#End Region

#Region "Properties"
        Dim _filename As String = String.Empty
        ''' <summary>
        ''' Gets or sets file to read the settings from.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Filename() As String
            Get
                Return _filename
            End Get
            Set(ByVal value As String)
                _filename = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the assigned value of a key within the specified section in the ini file.
        ''' </summary>
        ''' <param name="section">Ini file section.</param>
        ''' <param name="key">Ini file section key.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Sections(ByVal section As String, ByVal key As String) As String
            Get
                Return ReadSection(section, key)
            End Get
        End Property
#End Region

#Region "Functions"
        ''' <summary>
        ''' Validates whether the filename specified is a ini file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsIniFile() As Boolean
            Return (Path.GetExtension(_filename.Trim).Replace(".", String.Empty).ToLower = "ini") And _
                   (File.Exists(_filename))
        End Function

        ''' <summary>
        ''' Gets the ini file's contents.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function Contents() As String
            Dim filecontents As New StringBuilder

            If IsIniFile() Then
                Dim sr As New StreamReader(_filename, Encoding.UTF8)
                Try
                    filecontents.Append(sr.ReadToEnd)
                Catch ex As Exception

                Finally
                    With sr
                        .Close() : .Dispose()
                    End With
                End Try
            End If

            Return filecontents.ToString
        End Function

        Dim _filecontents As String = String.Empty

        ''' <summary>
        ''' Gets the assigned value of a key within the specified section in the ini file.
        ''' </summary>
        ''' <param name="section"></param>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ReadSection(ByVal section As String, ByVal key As String) As String
            Dim value As String = String.Empty

            If String.IsNullOrEmpty(_filecontents.Trim) Then _filecontents = Contents()
            Dim filecontents As String = _filecontents

            If Not String.IsNullOrEmpty(filecontents.Trim) Then
                Dim lines() As String = filecontents.Trim.Split(vbNewLine)
                Dim startline As Integer = 0 : Dim endline As Integer = 0
                Dim withstarting As Boolean = False : Dim withending As Boolean = False

                For iRow As Integer = 0 To lines.Length - 1
                    If lines(iRow).Trim.ToUpper = "[" & section.Trim.ToUpper & "]" Then
                        startline = iRow : withstarting = True : Exit For
                    End If
                Next

                If withstarting Then
                    If (startline + 1) <= (lines.Length - 1) Then
                        Dim curline As Integer = 0
                        For iRow As Integer = startline + 1 To lines.Length - 1
                            If lines(iRow).Trim.StartsWith("[") And _
                               lines(iRow).Trim.EndsWith("]") Then
                                If iRow <> startline Then
                                    endline = iRow - 1 : withending = True
                                End If
                                Exit For
                            End If
                            curline = iRow
                        Next

                        If curline <= (lines.Length - 1) Then
                            withending = True : endline = curline
                        End If
                    End If
                End If

                If withstarting And withending Then
                    For iRow As Integer = startline To endline
                        Dim chars() As Char = lines(iRow).Trim.ToCharArray
                        Dim seekingkey As String = String.Empty

                        If chars.Length > 0 Then
                            For Each c As Char In chars
                                If (c.ToString <> " ") And _
                                   (Not String.IsNullOrEmpty(c.ToString.Trim)) Then seekingkey &= c.ToString
                                If seekingkey.Trim.StartsWith(key.Trim & "=") And _
                                   seekingkey.RLTrim <> key.RLTrim & "=" Then value &= c.ToString
                            Next
                        End If
                    Next
                End If
            End If

            Return value
        End Function

        ''' <summary>
        ''' Sets a value in the specified key of the specified section within the ini file's contents.
        ''' </summary>
        ''' <param name="section">Ini file section.</param>
        ''' <param name="key">Ini file section key.</param>
        ''' <param name="value">Key value to assign.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetValue(ByVal section As String, ByVal key As String, ByVal value As String) As Boolean
            Dim setavalue As Boolean = False
            If String.IsNullOrEmpty(_filecontents.Trim) Then _filecontents = Contents()

            If Not String.IsNullOrEmpty(_filecontents.Trim) Then
                Dim lines() As String = _filecontents.Trim.Split(vbNewLine)
                If lines.Length > 0 Then
                    Dim linestring As String = String.Empty
                    Dim startline As Integer = 0 : Dim endline As Integer = 0

                    For iRow As Integer = 0 To lines.Length - 1
                        If lines(iRow).Trim.ToUpper = "[" & section.Trim.ToUpper & "]" Then
                            startline = iRow : Exit For
                        End If
                    Next

                    If (startline + 1) <= (lines.Length - 1) Then
                        For iRow As Integer = startline + 1 To lines.Length - 1
                            If lines(iRow).Trim.StartsWith("[") And _
                               lines(iRow).Trim.EndsWith("]") Then Exit For
                            endline = iRow
                        Next

                        For iRow As Integer = startline + 1 To endline
                            Dim line() As Char = lines(iRow).Trim.ToCharArray
                            Dim keyline As String = String.Empty
                            For Each c As Char In line
                                If c.ToString.Trim <> " " Then keyline &= c.ToString
                                If keyline.Trim.ToUpper.Contains(key.Trim.ToUpper & "=") Then
                                    lines(iRow) = key & "=" & value
                                    Dim sw As New StreamWriter(_filename)
                                    Try
                                        For iLine As Integer = 0 To lines.Length - 1
                                            sw.WriteLine(lines(iLine))
                                        Next
                                        setavalue = True
                                    Catch ex As Exception

                                    Finally
                                        sw.Close() : sw.Dispose()
                                    End Try

                                    If setavalue Then
                                        Return setavalue : Exit Function
                                    End If
                                End If
                            Next
                        Next

                        Dim filecontents As New StringBuilder
                        filecontents.Append(_filecontents)
                        If endline < (lines.Length - 1) Then : filecontents.ToString.Trim.Replace(lines(endline).Trim, lines(endline).Trim & vbNewLine & key & "=" & value)
                        Else : filecontents.Append(IIf(Not filecontents.ToString.Trim.EndsWith(vbNewLine), vbNewLine, String.Empty) & key & "=" & value)
                        End If

                        Dim sf As New StreamWriter(_filename)
                        Try
                            sf.Write(filecontents.ToString)
                            setavalue = True
                        Catch ex As Exception

                        Finally
                            sf.Close() : sf.Dispose()
                        End Try
                    End If
                End If
            End If

            Return setavalue
        End Function

        ''' <summary>
        ''' Calls the IniFile class' Section property to get an specific key value under the specified section within the specified ini file.
        ''' </summary>
        ''' <param name="filename">Ini file path.</param>
        ''' <param name="section">Section in the ini file.</param>
        ''' <param name="key">Key to get the value from.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetKeyValue(ByVal filename As String, ByVal section As String, ByVal key As String) As String
            Dim value As String = String.Empty
            Dim ini As New IniFile(filename)
            value = ini.Sections(section, key)
            ini.Dispose()
            Return value
        End Function

        ''' <summary>
        ''' Calls the IniFile class' SetValue function to assign a value in the specified key under the specified section of the specified ini file.
        ''' </summary>
        ''' <param name="filename"></param>
        ''' <param name="section"></param>
        ''' <param name="key"></param>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function SetKeyValue(ByVal filename As String, ByVal section As String, ByVal key As String, ByVal value As String) As Boolean
            Dim setavalue As Boolean = False
            Dim ini As New IniFile(filename)
            setavalue = ini.SetValue(section, key, value)
            ini.Dispose()
            Return setavalue
        End Function
#End Region

#Region "IDisposable"

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
