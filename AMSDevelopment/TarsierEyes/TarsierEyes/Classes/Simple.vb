Namespace Common
    ''' <summary>
    ''' Common and simple shared functions and methods.
    ''' </summary>
    ''' <remarks></remarks>
    Partial Public Class Simple

#Region "API Declarations"
        Private Declare Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal process As IntPtr, ByVal minimumWorkingSetSize As Integer, ByVal maximumWorkingSetSize As Integer) As Integer
#End Region

#Region "Functions"
        ''' <summary>
        ''' Converts the given byte array to its hexadecimal string representation.
        ''' </summary>
        ''' <param name="bytes">Array of byte to be interpreted.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteArrayToHexaDecimalString(ByVal bytes As Byte()) As String
            Dim hex As New StringBuilder
            Dim result As String = String.Empty

            For ctr As Integer = 0 To bytes.Length - 1
                hex.Append(bytes(ctr).ToString("X2"))
            Next

            Try
                result = BitConverter.ToString(BitConverter.GetBytes(CLng(hex.ToString))).Replace("-", String.Empty)
            Catch ex As Exception
                result = hex.ToString
            End Try

            Return result
        End Function

        ''' <summary>
        ''' Converts the given byte array to its corresponding file with the specified file extension.
        ''' </summary>
        ''' <param name="bytes">Array of byte to be interpreted.</param>
        ''' <param name="fileextension">Output file extension.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteArrayToFileObject(ByVal bytes As Byte(), ByVal fileextension As String) As FileInfo
            Return ByteArrayToFileObject(bytes, fileextension, Application.StartupPath & "\Exports")
        End Function

        ''' <summary>
        ''' Converts the given byte array to its corresponding file with the specified file extension.
        ''' </summary>
        ''' <param name="bytes">Array of byte to be interpreted.</param>
        ''' <param name="fileextension">Output file extension.</param>
        ''' <param name=" outputdirectory">The output directory for the specified file.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteArrayToFileObject(ByVal bytes As Byte(), ByVal fileextension As String, ByVal outputdirectory As String) As FileInfo
            Dim fi As FileInfo = Nothing

            If Not Directory.Exists(outputdirectory) Then
                Try
                    My.Computer.FileSystem.CreateDirectory(outputdirectory)
                Catch ex As Exception
                End Try
            End If

            If Directory.Exists(outputdirectory) Then
                If File.Exists(outputdirectory & "\file." & fileextension) Then My.Computer.FileSystem.DeleteFile(outputdirectory & "\file." & fileextension, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
                File.WriteAllBytes(outputdirectory & "\file." & fileextension, bytes)
                If File.Exists(outputdirectory & "\file." & fileextension) Then fi = New FileInfo(outputdirectory & "\file." & fileextension)
            End If

            Return fi
        End Function

        ''' <summary>
        ''' Converts the given blob byte array to its image representation.
        ''' </summary>
        ''' <param name="bytes">Array of byte to be interpreted.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteArrayToImage(ByVal bytes As Byte()) As Image
            Dim img As Image = Nothing
            Dim ms As New IO.MemoryStream(bytes)

            Try
                img = Image.FromStream(ms)
            Catch ex As Exception
            Finally
                With ms
                    .Close() : .Dispose()
                End With
                RefreshAndManageCurrentProcess()
            End Try

            Return img
        End Function

        ''' <summary>
        ''' Gets a field value from the data source using the supplied text filter expression.
        ''' </summary>
        ''' <param name="datasource">Data source to evaluate.</param>
        ''' <param name="filterexpression">Data table row filter expression.</param>
        ''' <param name="field">Field's name to fetch the value from.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function DataSourceValue(ByVal datasource As DataTable, ByVal filterexpression As String, ByVal field As String) As Object
            Return DataSourceValue(Of Object)(datasource, filterexpression, field, Nothing)
        End Function

        ''' <summary>
        ''' Gets a field value from the data source using the supplied text filter expression.
        ''' </summary>
        ''' <param name="datasource">Data source to evaluate.</param>
        ''' <param name="filterexpression">Data table row filter expression.</param>
        ''' <param name="field">Field's name to fetch the value from.</param>
        ''' <param name="defaultvalue">Default value if there is nothing to return.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function DataSourceValue(ByVal datasource As DataTable, ByVal filterexpression As String, ByVal field As String, ByVal defaultvalue As Object) As Object
            Return DataSourceValue(Of Object)(datasource, filterexpression, field, defaultvalue)
        End Function

        ''' <summary>
        ''' Gets a field value from the data source using the supplied text filter expression.
        ''' </summary>
        ''' <typeparam name="TResult"></typeparam>
        ''' <param name="datasource">Data source to evaluate.</param>
        ''' <param name="filterexpression">Data table row filter expression.</param>
        ''' <param name="field">Field's name to fetch the value from.</param>
        ''' <param name="defaultvalue">Default value if there is nothing to return.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function DataSourceValue(Of TResult)(ByVal datasource As DataTable, ByVal filterexpression As String, ByVal field As String, ByVal defaultvalue As TResult) As TResult
            Dim obj As TResult = defaultvalue

            If datasource IsNot Nothing Then
                Try
                    If datasource.Select(filterexpression).Length > 0 Then
                        Dim dr As DataRow = datasource.Select(filterexpression)(0)
                        obj = dr.Item(field)
                    End If
                Catch ex As Exception
                End Try
            End If

            Return obj
        End Function

        ''' <summary>
        ''' Gets the commonly used "." password character.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function DotPasswordChar() As String
            Return Global.Microsoft.VisualBasic.ChrW(8226)
        End Function

        ''' <summary>
        ''' Converts the file (in the given path) to its byte array representation.
        ''' </summary>
        ''' <param name="filename">File's full path.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function FileObjectToByteArray(ByVal filename As String) As Byte()
            Dim bytes(1) As Byte

            If File.Exists(filename) Then
                Dim sr As New StreamReader(filename)
                Dim br As New BinaryReader(sr.BaseStream)

                Try
                    Dim tempbytes() As Byte = br.ReadBytes(br.BaseStream.Length)
                    ReDim bytes(tempbytes.Length)
                    bytes = tempbytes
                Catch ex As Exception
                Finally
                    br.Close() : br = Nothing
                    With sr
                        .Close() : .Dispose()
                    End With
                    RefreshAndManageCurrentProcess()
                End Try
            End If

            Return bytes
        End Function

        ''' <summary>
        ''' Converts the file (in the given path) to its hexadecimal string representation.
        ''' </summary>
        ''' <param name="filename">File's full path.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function FileObjectToHexaDecimalString(ByVal filename As String) As String
            Dim hex As New StringBuilder

            If File.Exists(filename) Then
                Dim sr As New IO.StreamReader(filename)
                Dim br As New IO.BinaryReader(sr.BaseStream)
                Try
                    hex.Append(BitConverter.ToString(br.ReadBytes(br.BaseStream.Length)).Replace("-", String.Empty))
                Catch ex As Exception
                Finally
                    br.Close() : br = Nothing
                    With sr
                        .Close() : .Dispose()
                    End With
                    RefreshAndManageCurrentProcess()
                End Try
            End If

            Return hex.ToString
        End Function

        ''' <summary>
        ''' Gets the list of countries and loads it in a System.Data.DataTable.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetCountryTable() As DataTable
            Dim dtable As New DataTable
            With dtable
                .Columns.Clear() : .Rows.Clear()
                .Columns.Add("Country", GetType(String))
                .Columns("Country").AllowDBNull = True

                Dim cultures() As CultureInfo = CultureInfo.GetCultures(CultureTypes.AllCultures And Not CultureTypes.NeutralCultures)

                For Each culture As CultureInfo In cultures
                    Try
                        Dim region As New RegionInfo(culture.Name)

                        If .Select("(Country LIKE '" & region.EnglishName.ToSqlValidString & "')").Length <= 0 Then
                            Dim drow As DataRow = .Rows.Add
                            drow.Item("Country") = region.EnglishName
                        End If
                    Catch ex As Exception
                    End Try
                Next

                Dim rw() As DataRow = .Select("[Country] LIKE 'Afghanistan'")
                If rw.Length <= 0 Then .Rows.Add("Afghanistan")

                .DefaultView.Sort = "Country"
            End With

            Return dtable
        End Function

        ''' <summary>
        ''' Converts the specified image to its bytes array representation.
        ''' </summary>
        ''' <param name="image">Image object to be interpreted.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function ImageToByteArray(ByVal image As Image) As Byte()
            Dim bytes(1) As Byte
            Dim ms As New IO.MemoryStream

            Try
                Dim img As Image = image.Clone
                With img
                    .Save(ms, System.Drawing.Imaging.ImageFormat.Png) : .Dispose()
                End With

                Dim tempbytes() As Byte = ms.ToArray
                ReDim bytes(tempbytes.Length)
                bytes = tempbytes
            Catch ex As Exception
            Finally
                With ms
                    .Close() : .Dispose()
                End With
            End Try

            Return bytes
        End Function

        ''' <summary>
        ''' Converts the given image to its hexadecimal string representation.
        ''' </summary>
        ''' <param name="image">Image object to be interpreted.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function ImageToHexaDecimalString(ByVal image As Image) As String
            Dim hex As New StringBuilder
            Dim ms As New IO.MemoryStream

            Try
                Dim img As Image = image.Clone
                With img
                    .Save(ms, System.Drawing.Imaging.ImageFormat.Png) : .Dispose()
                End With

                hex.Append(BitConverter.ToString(ms.ToArray).Replace("-", String.Empty))
            Catch ex As Exception
            Finally
                With ms
                    .Close() : .Dispose()
                End With
            End Try

            Return hex.ToString
        End Function

        ''' <summary>
        ''' Validates if the specified value is Null.Value or Nothing.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IsNullOrNothing(ByVal value As Object) As Boolean
            Try
                Return IsDBNull(value) Or (value Is Nothing)
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Gets the proper case (first letter : upper case and preceeding in lower case) string value of the specified string.
        ''' </summary>
        ''' <param name="value">String value to convert into.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ToProperString(ByVal value As String) As String
            Return StrConv(value, VbStrConv.ProperCase, 0)
        End Function

        ''' <summary>
        ''' Parses the specified value to return its corresponding type-safe representation.
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="value">Value to evaluate.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function ToSafeValue(Of T)(ByVal value As T) As T
            Dim defaultvalue As Object = Nothing

            Select Case value.GetType().Name
                Case GetType(String).Name : defaultvalue = String.Empty
                Case "UInt32", "UInt64", GetType(UInteger).Name, GetType(Decimal).Name, GetType(Single).Name, GetType(Double).Name, _
                     GetType(SByte).Name, GetType(Byte).Name, GetType(Integer).Name, GetType(Long).Name : defaultvalue = 0
                Case GetType(Date).Name : defaultvalue = #1/1/1900#
                Case GetType(Boolean).Name : defaultvalue = False
                Case Else
            End Select

            Return ToSafeValue(Of T)(value, defaultvalue)
        End Function

        ''' <summary>
        ''' Parses the specified value to return its corresponding type-safe representation.
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="value">Value to evaluate.</param>
        ''' <param name="defaultvalue">Default value to return just in case it is unsafe.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function ToSafeValue(Of T)(ByVal value As T, ByVal defaultvalue As T) As T
            If IsNullOrNothing(value) Then : Return defaultvalue
            Else : Return value
            End If
        End Function

        ''' <summary>
        ''' Works like TryCast function but this time supports assigned value types (ea. Integer, Decimal, Date and etc.).
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="expression">Expression to convert.</param>
        ''' <returns>Returns Nothing if convertion fails.</returns>
        ''' <remarks></remarks>
        Public Shared Function TryChangeType(Of T)(ByVal expression As Object) As T
            Try
                Return CType(expression, T)
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Validates whether condition was satisfied otherwise control specified will be highligthed (validator should be a DevComponents.DotNetBar.Validator.SuperValidator).
        ''' </summary>
        ''' <param name="validator">DevComponents.DotNetBar.Validator.SuperValidator to use as the control notifying object.</param>
        ''' <param name="control">Control to place a notifier with.</param>
        ''' <param name="condition">The true part of the satisfying condition to evaluate.</param>
        ''' <param name="notification">Notification message to be shown.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Valid(ByVal validator As Object, ByVal control As Control, ByVal condition As Boolean, ByVal notification As String) As Boolean
            If validator.GetType.FullName = "DevComponents.DotNetBar.Validator.SuperValidator" Or _
               validator.GetType.BaseType.FullName = "DevComponents.DotNetBar.Validator.SuperValidator" Then
                validator.ErrorProvider.Clear()
                validator.Highlighter.SetHighlightColor(control, 0)

                If Not condition Then
                    With validator
                        .Highlighter.SetHighlightColor(control, 1)
                        .ErrorProvider.SetError(control, notification)
                        control.Focus()
                    End With
                End If
            End If
            Return condition
        End Function
#End Region

#Region "Methods"
        Private Shared Sub txt_ButtonCustomClick(ByVal sender As Object, ByVal e As System.EventArgs)
            If sender Is Nothing Then Exit Sub
            If Not sender.Enabled Then Exit Sub
            sender.Text = String.Empty
        End Sub

        ''' <summary>
        ''' Clears each of the controls in the specified form or control (or specified control itself) text, items, and data sources.
        ''' </summary>
        ''' <param name="control">Control, form or container to be iterated.</param>
        ''' <remarks></remarks>
        Public Shared Sub ClearContents(ByVal control As Object)
            Select Case control.GetType.FullName
                Case "DevComponents.DotNetBar.Controls.TextBoxX", _
                     "System.Windows.Forms.TextBox"
                    With control
                        .Enabled = False : .Text = String.Empty
                        If .GetType.FullName = "DevComponents.DotNetBar.Controls.TextBoxX" And _
                           .Name.Trim.ToLower.Contains("txtsearch") Then
                            .ButtonCustom.Visible = True : .ButtonCustom.Image = ClearTextImage
                            Dim clickevent As EventInfo = control.GetType().GetEvent("ButtonCustomClick", BindingFlags.Public + BindingFlags.NonPublic + BindingFlags.Instance)
                            If clickevent IsNot Nothing Then clickevent.AddEventHandler(control, New EventHandler(AddressOf txt_ButtonCustomClick))
                        End If
                        .Enabled = True
                    End With

                Case "P5.P5TextBox", _
                     "TarsierEyes.Controls.SideLabelledTextBox"
                    With control
                        .Enabled = False : .Multiline = Not .Multiline
                        .Text = String.Empty
                        .Multiline = Not .Multiline : .Enabled = True
                    End With

                Case "DevComponents.DotNetBar.Controls.ComboBoxEx", _
                     "System.Windows.Forms.ComboBox", _
                     "FMS.FMSComboBox"
                    With control
                        .Enabled = False
                        Try
                            If .DataSource IsNot Nothing Then CType(.DataSource, DataTable).Dispose()
                        Catch ex As Exception
                        Finally : .DataSource = Nothing
                        End Try
                        .Items.Clear() : .SelectedIndex = -1
                        .Text = String.Empty : .Enabled = True
                    End With

                Case "DevComponents.DotNetBar.Controls.CheckBoxX", _
                     "System.Windows.Forms.CheckBox"
                    With control
                        .Enabled = False : .Checked = False : .Enabled = True
                    End With

                Case "DevComponents.Editors.IntegerInput", _
                     "DevComponents.Editors.DoubleInput"
                    With control
                        .Enabled = False
                        .Value = 0 : .MinValue = 0 : .AllowEmptyState = False
                        .Enabled = True
                    End With

                Case "System.Windows.Forms.NumericUpDown"
                    With control
                        .Enabled = False
                        .Value = 0 : .Minimum = 0
                        .Enabled = True
                    End With

                Case "DevComponents.Editors.DateTimeAdv.DateTimeInput"
                    With control
                        .Enabled = False
                        .CustomFormat = "dd-MMM-yyyy" : .Format = 0
                        .AllowEmptyState = False : .Value = Now : .MaxDate = Now
                        .Enabled = True
                    End With

                Case "DevComponents.DotNetBar.Controls.ComboTree"
                    With control
                        .Enabled = False
                        Try
                            If .DataSource IsNot Nothing Then CType(.DataSource, DataTable).Dispose()
                        Catch ex As Exception
                        Finally : .DataSource = Nothing
                        End Try
                        .Nodes.Clear() : .Enabled = True
                    End With

                Case "C1.Win.C1List.C1Combo"
                    With control
                        .Enabled = False : .ClearItems()
                        Try
                            If .DataSource IsNot Nothing Then CType(.DataSource, DataTable).Dispose()
                        Catch ex As Exception
                        Finally : .DataSource = Nothing
                        End Try
                        .BorderStyle = BorderStyle.FixedSingle
                        .AutoDropDown = True : .AutoCompletion = True
                        .RowTracking = True : .ScrollTrack = True : .ScrollTips = True
                        .HeadingStyle.Font = New Font("Tahoma", 8, FontStyle.Regular)
                        .CaptionStyle.Font = New Font("Tahoma", 8, FontStyle.Regular)
                        .Style.Font = New Font("Tahoma", 8, FontStyle.Regular)
                        .MaxDropDownItems = 20 : .CellTips = 2
                        .DeadAreaBackColor = Color.White
                        .RowDivider.Color = Color.Gainsboro
                        .Style.Borders.Color = Color.Black
                        .RowDivider.Style = 1 : .VisualStyle = 2
                        .Enabled = True
                    End With

                Case "C1.Win.C1FlexGrid.C1FlexGrid"
                    With control
                        .BeginUpdate() : .Tag = "1"
                        .AutoSearch = 0 : .SelectionMode = 3
                        .KeyActionEnter = 3 : .KeyActionTab = 3
                        .Styles.EmptyArea.BackColor = Color.White
                        .Styles.EmptyArea.Border.Color = Color.White
                        .Styles.Normal.Border.Color = Color.Gainsboro
                        .ClipSeparators = "|;" : .TabStop = True
                        .Tree.LineStyle = Drawing2D.DashStyle.Dot
                        .Tree.LineColor = Color.DimGray : .VisualStyle = 3

                        For style As Integer = 0 To 10
                            Try
                                .Styles.Item("SubTotal" & style).BackColor = Color.Transparent
                                .Styles.Item("SubTotal" & style).Font = New Font("tahoma", 8, FontStyle.Bold)
                                .Styles.Item("SubTotal" & style).ForeColor = Color.Black
                            Catch ex As Exception
                                .Styles.Add("SubTotal" & style)
                                .Styles.Item("SubTotal" & style).BackColor = Color.Transparent
                                .Styles.Item("SubTotal" & style).Font = New Font("tahoma", 8, FontStyle.Bold)
                                .Styles.Item("SubTotal" & style).ForeColor = Color.Black
                            End Try
                        Next

                        .AllowEditing = False : .AllowDelete = False : .AllowAddNew = False
                    End With

                Case Else
                    If control.GetType.BaseType.FullName = "C1.Win.C1FlexGrid.C1FlexGrid" Then
                        With control
                            .BeginUpdate() : .Tag = "1"
                            .AutoSearch = 0 : .SelectionMode = 3
                            .KeyActionEnter = 3 : .KeyActionTab = 3
                            .Styles.EmptyArea.BackColor = Color.White
                            .Styles.EmptyArea.Border.Color = Color.White
                            .Styles.Normal.Border.Color = Color.Gainsboro
                            .ClipSeparators = "|;" : .TabStop = True
                            .Tree.LineStyle = Drawing2D.DashStyle.Dot
                            .Tree.LineColor = Color.DimGray : .VisualStyle = 3

                            For style As Integer = 0 To 10
                                Try
                                    .Styles.Item("SubTotal" & style).BackColor = Color.Transparent
                                    .Styles.Item("SubTotal" & style).Font = New Font("tahoma", 8, FontStyle.Bold)
                                    .Styles.Item("SubTotal" & style).ForeColor = Color.Black
                                Catch ex As Exception
                                    .Styles.Add("SubTotal" & style)
                                    .Styles.Item("SubTotal" & style).BackColor = Color.Transparent
                                    .Styles.Item("SubTotal" & style).Font = New Font("tahoma", 8, FontStyle.Bold)
                                    .Styles.Item("SubTotal" & style).ForeColor = Color.Black
                                End Try
                            Next

                            .AllowEditing = False : .AllowDelete = False : .AllowAddNew = False
                        End With

                    ElseIf control.GetType.BaseType.FullName = "C1.Win.C1Input.C1DropDownControl" Then
                        With control
                            .Enabled = False : .Value = String.Empty
                            Try
                                .Text = String.Empty
                            Catch ex As Exception
                            End Try
                            .Enabled = True
                        End With

                    Else
                        Try
                            If CType(control, Control).Controls.Count > 0 Then
                                For Each ctrl As Control In CType(control, Control).Controls
                                    ClearContents(ctrl)
                                Next
                            End If
                        Catch ex As Exception
                        End Try
                    End If
            End Select
        End Sub

        ''' <summary>
        ''' Applies row filter to the given data source (to all posible datasource columns) with the specified filtering values.
        ''' </summary>
        ''' <param name="datasource">Datasource to apply the filter with.</param>
        ''' <param name="valuefilter">Value to search from the datasource.</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub DataSourceRowFilter(ByVal datasource As DataTable, ByVal valuefilter As String)
            DataSourceRowFilter(datasource, valuefilter, String.Empty)
        End Sub

        ''' <summary>
        ''' Applies row filter to the given data source (to all posible datasource columns) with the specified filtering values.
        ''' </summary>
        ''' <param name="datasource">Datasource to apply the filter with.</param>
        ''' <param name="valuefilter">Value to search from the datasource.</param>
        ''' <param name="excludedfields">Field names to be excluded from the search criteria.</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub DataSourceRowFilter(ByVal datasource As DataTable, ByVal valuefilter As String, ByVal ParamArray excludedfields() As String)
            If datasource IsNot Nothing Then
                Dim filter As New StringBuilder

                With datasource
                    For Each col As DataColumn In .Columns
                        Dim excluded As Boolean = False

                        If excludedfields.Length > 0 Then
                            excluded = excludedfields.Contains(col.ColumnName)
                        End If

                        If Not excluded Then
                            Select Case col.DataType.Name
                                Case GetType(String).Name : filter.Append(IIf(Not String.IsNullOrEmpty(filter.ToString.Trim), " OR" & vbNewLine, String.Empty) & "(`" & col.ColumnName & "` LIKE '%" & valuefilter.ToSqlValidString(True).Replace(" ", "%') AND (`" & col.ColumnName & "` LIKE '%") & "%')")
                                Case GetType(Date).Name, _
                                     GetType(Decimal).Name, _
                                     "UInt32", "UInt64", GetType(UInteger).Name, GetType(Integer).Name, _
                                     GetType(Long).Name, _
                                     GetType(Single).Name, _
                                     GetType(Double).Name, _
                                     GetType(Byte).Name, _
                                    GetType(Boolean).Name : filter.Append(IIf(Not String.IsNullOrEmpty(filter.ToString.Trim), " OR" & vbNewLine, String.Empty) & "(CONVERT(`" & col.ColumnName & "`, System.String) LIKE '%" & valuefilter.ToSqlValidString(True).Replace(" ", "%') AND (CONVERT(`" & col.ColumnName & "`, System.String) LIKE '%") & "%')")
                                Case Else
                            End Select
                        End If
                    Next

                    Try
                        .DefaultView.RowFilter = filter.ToString
                    Catch ex As Exception
                    End Try
                End With
            End If
        End Sub

        ''' <summary>
        ''' Enables all input controls and button within the specified form, control or container.
        ''' </summary>
        ''' <param name="control">Control, form or container to be iterated</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub EnableFields(ByVal control As Object)
            EnableFields(control, True)
        End Sub

        ''' <summary>
        ''' Enables all input controls and button within the specified form, control or container.
        ''' </summary>
        ''' <param name="control">Control, form or container to be iterated.</param>
        ''' <param name="enabled">Determines whether the controls will be disable or not.</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub EnableFields(ByVal control As Object, ByVal enabled As Boolean)
            Select Case control.GetType.FullName
                Case "DevComponents.DotNetBar.Controls.TextBoxX", _
                     "System.Windows.Forms.TextBox", _
                     "P5.P5TextBox", _
                     "TarsierEyes.Controls.SideLabelledTextBox", _
                     "DevComponents.DotNetBar.Controls.ComboBoxEx", _
                     "System.Windows.Forms.ComboBox", _
                     "DevComponents.DotNetBar.Controls.CheckBoxX", _
                     "System.Windows.Forms.CheckBox", _
                     "DevComponents.Editors.IntegerInput", _
                     "DevComponents.Editors.DoubleInput", _
                     "C1.Win.C1List.C1Combo", _
                     "DevComponents.DotNetBar.ButtonX", _
                     "System.Windows.Forms.Button", _
                     "System.Windows.Forms.NumericUpDown", _
                     "DevComponents.DotNetBar.Controls.ComboTree", _
                     "DevComponents.Editors.DateTimeAdv.DateTimeInput", _
                     "C1.Win.C1Input.C1DropDownControl", _
                     "FMS.FMSComboBox" : control.Enabled = enabled
                    If control.GetType.FullName = "C1.Win.C1List.C1Combo" And enabled = False Then
                        control.EditorBackColor = Color.Gainsboro
                    ElseIf control.GetType.FullName = "C1.Win.C1List.C1Combo" And enabled = True Then
                        control.EditorBackColor = Color.White
                    End If

                Case "DevComponents.DotNetBar.Bar"
                    For iControls As Integer = 0 To control.Items.Count - 1
                        control.Items(iControls).Enabled = enabled
                    Next
                   

                Case Else
                    Select Case control.GetType().BaseType.Name
                        Case "DevComponents.DotNetBar.Controls.TextBoxX", _
                             "System.Windows.Forms.TextBox", _
                             "P5.P5TextBox", _
                             "TarsierEyes.Controls.SideLabelledTextBox", _
                             "DevComponents.DotNetBar.Controls.ComboBoxEx", _
                             "System.Windows.Forms.ComboBox", _
                             "DevComponents.DotNetBar.Controls.CheckBoxX", _
                             "System.Windows.Forms.CheckBox", _
                             "DevComponents.Editors.IntegerInput", _
                             "DevComponents.Editors.DoubleInput", _
                             "C1.Win.C1List.C1Combo", _
                             "DevComponents.DotNetBar.ButtonX", _
                             "System.Windows.Forms.Button", _
                             "System.Windows.Forms.NumericUpDown", _
                             "DevComponents.DotNetBar.Controls.ComboTree", _
                             "DevComponents.Editors.DateTimeAdv.DateTimeInput", _
                             "C1.Win.C1Input.C1DropDownControl", _
                             "FMS.FMSComboBox" : control.Enabled = enabled

                        Case "DevComponents.DotNetBar.Bar"
                            For iControls As Integer = 0 To control.Items.Count - 1
                                control.Items(iControls).Enabled = enabled
                            Next

                        Case Else
                            Try
                                If control.Controls.Count > 0 Then
                                    For Each ctrl As Control In control.Controls
                                        EnableFields(ctrl, enabled)
                                    Next
                                End If
                            Catch ex As Exception
                            End Try
                    End Select

            End Select
        End Sub

        ''' <summary>
        ''' Places an "*" character in the form's caption as indicator that a form has been modified.
        ''' </summary>
        ''' <param name="form">Form to place a marking.</param>
        ''' <remarks></remarks>
        Public Shared Sub MarkFormAsEdited(ByVal form As Form)
            If Not form.Text.Trim.EndsWith("*") Then form.Text &= " *"
        End Sub

        ''' <summary>
        ''' Redraw an object; basically use for C1.Win.C1FlexGrid.C1FlexGrid, DevComponents.AdvTree.AdvTree and DevComponents.DotNetBar.Controls.ListViewEx using its EndUpdate method.
        ''' </summary>
        ''' <param name="control"></param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub Redraw(ByVal control As Object)
            Redraw(control, True)
        End Sub

        ''' <summary>
        ''' Redraw an object; basically use for C1.Win.C1FlexGrid.C1FlexGrid, DevComponents.AdvTree.AdvTree and DevComponents.DotNetBar.Controls.ListViewEx using its BeginUpdate and EndUpdate method.
        ''' </summary>
        ''' <param name="control"></param>
        ''' <param name="redraw"></param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub Redraw(ByVal control As Object, ByVal redraw As Boolean)
            If control.GetType.FullName = "C1.Win.C1FlexGrid.C1FlexGrid" Or _
               control.GetType.FullName = "DevComponents.DotNetBar.Controls.ListViewEx" Or _
               control.GetType.FullName = "DevComponents.AdvTree.AdvTree" Or _
               control.GetType.BaseType.FullName = "C1.Win.C1FlexGrid.C1FlexGrid" Then
                With control
                    .EndUpdate() : .Tag = Nothing
                    If Not redraw Then
                        .BeginUpdate() : .Tag = "1"
                    End If
                End With
            End If
        End Sub

        ''' <summary>
        ''' Refreshes and force the release of the entire unmanaged resources of the current application's process. Resources (API calls and attached method) c / o : DMA.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub RefreshAndManageCurrentProcess()
            GC.Collect()
            If (Environment.OSVersion.Platform = PlatformID.Win32NT) Then
                Dim p As Process = Process.GetCurrentProcess
                SetProcessWorkingSetSize(p.Handle, -1, -1)
                p.Close() : p.Refresh() : p.Dispose()
            End If
        End Sub
#End Region

    End Class

End Namespace


