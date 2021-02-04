Namespace Common
    ''' <summary>
    ''' Class for internal MS Excel customizations.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MSExcel

        ''' <summary>
        ''' Adds column filters for each available columns in the specified excel worksheet.
        ''' </summary>
        ''' <param name="filename"></param>
        ''' <param name="sheetname"></param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub ExcelAutoColumnFilters(ByVal filename As String, ByVal sheetname As String)
            Dim excelApp As New Excel.Application
            Dim excelBook As Excel.Workbook = excelApp.Workbooks.Add(System.Reflection.Missing.Value)
            Dim excelSheet As Excel.Worksheet = Nothing
            Try
                excelBook = excelApp.Workbooks.Open(filename)
                excelSheet = excelBook.Worksheets(sheetname)
                With excelSheet
                    .Range("A1:Z1").AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlFilterValues, Type.Missing, Type.Missing)
                End With
                excelBook.Save()
            Catch ex As Exception

            Finally
                ReleaseResourceObject(excelSheet)
                excelBook.Close(False)
                ReleaseResourceObject(excelBook)
                excelApp.Workbooks.Close()
                ReleaseResourceObject(excelApp.Workbooks)
                excelApp.Quit()
                ReleaseResourceObject(excelApp)
                GC.Collect() : Common.Simple.RefreshAndManageCurrentProcess()
            End Try
        End Sub

        ''' <summary>
        '''  Adds column filters for each available columns in the first excel worksheet of the specified excel file.
        ''' </summary>
        ''' <param name="filename"></param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub ExcelAutoColumnFilters(ByVal filename As String)
            Dim _worksheets As New List(Of String)
            _worksheets = ExcelWorkSheets(filename)
            If _worksheets.Count > 0 Then ExcelAutoColumnFilters(filename, _worksheets(0))
        End Sub

        ''' <summary>
        ''' Retreives list of excel worksheet names inside the specified excel file.
        ''' </summary>
        ''' <param name="filename"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ExcelWorkSheets(ByVal filename As String) As List(Of String)
            Dim _worksheets As New List(Of String)
            _worksheets.Clear()

            Dim _connectionstring As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & filename & ";Extended Properties=Excel 8.0;"
            If Path.GetExtension(filename).ToLower.Replace(".", "") = "xlsx" Then _connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & filename & ";Extended Properties=Excel 12.0;"
            Dim con As New OleDbConnection(_connectionstring)

            Try
                With con
                    If .State = ConnectionState.Closed Then .Open()
                    Dim dtable As DataTable = .GetSchema("Tables")

                    If dtable.Rows.Count > 0 Then
                        For iRow As Integer = 0 To dtable.Rows.Count - 1
                            Dim _schemaname As String = dtable.Rows(iRow).Item("TABLE_NAME").ToString.Trim
                            If Not _schemaname.StartsWith("_xlnm#") Then _worksheets.Add(_schemaname.Replace("$", String.Empty).Replace("'", ""))
                        Next
                    End If

                    dtable.Dispose()
                End With
            Catch ex As Exception

            Finally
                With con
                    If .State = ConnectionState.Open Then .Close() : .Dispose()
                End With
            End Try

            Return _worksheets
        End Function

        ''' <summary>
        ''' Gets excel sheet column name for the given column index.
        ''' </summary>
        ''' <param name="index">Column index.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ExcelSheetColumn(ByVal index As Integer) As String
            Dim sReturn As String

            sReturn = Chr(64 + index)

            Return sReturn
        End Function

        ''' <summary>
        ''' Exports the specified DataTable's row data into the specified Microsoft Excel file (file must be in .csv format; method will automatcially alter the extension when it detects that it is not in .csv).
        ''' </summary>
        ''' <param name="table"></param>
        ''' <param name="filename"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Export(ByVal table As DataTable, ByRef filename As String) As IO.FileInfo
            Return Export(table, filename, vbTab)
        End Function

        ''' <summary>
        ''' Exports the specified DataTable's row data into the specified Microsoft Excel file (file must be in .csv format; method will automatcially alter the extension when it detects that it is not in .csv).
        ''' </summary>
        ''' <param name="table"></param>
        ''' <param name="filename"></param>
        ''' <param name="separator"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Export(ByVal table As DataTable, ByRef filename As String, ByVal separator As String) As IO.FileInfo
            Dim fi As IO.FileInfo = Nothing

            If table IsNot Nothing Then
                Dim sb As New System.Text.StringBuilder
                Dim rwcontents As String = String.Empty

                For Each col As DataColumn In table.Columns
                    rwcontents &= IIf(String.IsNullOrEmpty(rwcontents.Trim), String.Empty, separator) & col.ColumnName
                Next

                sb.Append(rwcontents & vbNewLine)

                For Each rw As DataRow In table.Rows
                    rwcontents = String.Empty
                    For Each col As DataColumn In table.Columns
                        rwcontents &= IIf(String.IsNullOrEmpty(rwcontents.Trim), String.Empty, separator) & TryConvert(Of String)(rw.Item(col.ColumnName)).ToSafeValue
                    Next
                    sb.Append(rwcontents & vbNewLine)
                Next

                If IO.Path.GetExtension(filename).Replace(".", String.Empty).ToLower <> "csv" Then filename = (IO.Path.GetPathRoot(filename) & "\" & IO.Path.GetFileNameWithoutExtension(filename) & ".csv")
                If IO.File.Exists(filename) Then
                    Try
                        IO.File.Delete(filename)
                    Catch ex As Exception
                    End Try
                End If

                IO.File.WriteAllText(filename, sb.ToString, System.Text.Encoding.Unicode)
                If IO.File.Exists(filename) Then fi = New IO.FileInfo(filename)
            End If

            Return fi
        End Function

        ''' <summary>
        ''' Write specified text into the bottom row of the specified excel worksheet.
        ''' </summary>
        ''' <param name="filename">Excel filename.</param>
        ''' <param name="sheetname">Excel worksheet name.</param>
        ''' <param name="text">Text to be written.</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub WriteTextToExcel(ByVal filename As String, ByVal sheetname As String, ByVal text As String)
            Dim excelApp As New Excel.Application
            Dim excelBook As Excel.Workbook = excelApp.Workbooks.Add(System.Reflection.Missing.Value)
            Dim excelSheet As Excel.Worksheet = Nothing
            Try
                excelBook = excelApp.Workbooks.Open(filename)
                excelSheet = excelBook.Worksheets(sheetname)
                With excelSheet
                    .Cells(.Rows.Count - 1, .Columns.Count - 1) = text
                End With
            Catch ex As Exception
                MsgBox(ex)
            Finally
                ReleaseResourceObject(excelSheet)
                excelBook.Close(False)
                ReleaseResourceObject(excelBook)
                excelApp.Workbooks.Close()
                ReleaseResourceObject(excelApp.Workbooks)
                excelApp.Quit()
                ReleaseResourceObject(excelApp)
                GC.Collect() : Common.Simple.RefreshAndManageCurrentProcess()
            End Try
        End Sub

        ''' <summary>
        ''' Write specified text into the supplied excel coordinates (cell) of the specified excel worksheet.
        ''' </summary>
        ''' <param name="filename">Excel filename.</param>
        ''' <param name="sheetname">Excel worksheet name.</param>
        ''' <param name="text">Text to be written.</param>
        ''' <param name="coordinate">Excel coordinates (cell) to where the text will be written.</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub WriteTextToExcel(ByVal filename As String, ByVal sheetname As String, ByVal text As String, ByVal coordinate As Point)
            Dim excelApp As New Excel.Application
            Dim excelBook As Excel.Workbook = excelApp.Workbooks.Add(System.Reflection.Missing.Value)
            Dim excelSheet As Excel.Worksheet = Nothing
            Try
                excelBook = excelApp.Workbooks.Open(filename)
                excelSheet = excelBook.Worksheets(sheetname)
                With excelSheet
                    .Range(ExcelSheetColumn(coordinate.Y) & coordinate.X.ToString).Value = text
                End With
                excelBook.Save()
            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                ReleaseResourceObject(excelSheet)
                excelBook.Close(False)
                ReleaseResourceObject(excelBook)
                excelApp.Workbooks.Close()
                ReleaseResourceObject(excelApp.Workbooks)
                excelApp.Quit()
                ReleaseResourceObject(excelApp)
                GC.Collect() : Common.Simple.RefreshAndManageCurrentProcess()
            End Try
        End Sub

        ''' <summary>
        ''' Write specified text into the bottom row of the first excel worksheet in the specified excel file.
        ''' </summary>
        ''' <param name="filename">Excel filename.</param>
        ''' <param name="text">Text to be written.</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub WriteTextToExcel(ByVal filename As String, ByVal text As String)
            Dim _worksheets As New List(Of String)
            _worksheets = ExcelWorkSheets(filename)
            If _worksheets.Count > 0 Then WriteTextToExcel(filename, _worksheets(0), text)
        End Sub

        ''' <summary>
        ''' Write specified text into the supplied excel coordinates (cell) of the first excel worksheet in the specified excel file.
        ''' </summary>
        ''' <param name="filename">Excel filename.</param>
        ''' <param name="text">Text to be written.</param>
        ''' <param name="coordinate">Excel coordinates (cell) to where the text will be written.</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub WriteTextToExcel(ByVal filename As String, ByVal text As String, ByVal coordinate As Point)
            Dim _worksheets As New List(Of String)
            _worksheets = ExcelWorkSheets(filename)
            If _worksheets.Count > 0 Then WriteTextToExcel(filename, _worksheets(0), text, coordinate)
        End Sub

        ''' <summary>
        ''' Imports the first worksheet of the given excel file into a System.DataTable.
        ''' </summary>
        ''' <param name="filename">File path to be imported.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Import(ByVal filename As String) As DataTable
            Dim _worksheets As New List(Of String)
            Dim worksheet As String = String.Empty
            _worksheets = ExcelWorkSheets(filename)
            If _worksheets.Count > 0 Then worksheet = _worksheets(0)
            Return Import(filename, worksheet)
        End Function

        ''' <summary>
        ''' Imports the selected worksheet of the given excel file into a System.DataTable.
        ''' </summary>
        ''' <param name="filename">File path to be imported.</param>
        ''' <param name="sheetname">Excel work sheet within the file to be imported.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Import(ByVal filename As String, ByVal sheetname As String) As DataTable
            Dim dtable As New DataTable

            Dim _connectionstring As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & filename & ";Extended Properties=Excel 8.0;"
            If Path.GetExtension(filename).ToLower.Replace(".", "") = "xlsx" Then _connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & filename & ";Extended Properties=Excel 12.0;"

            Dim con As New OleDbConnection(_connectionstring)
            Dim cmd As New OleDbCommand

            With cmd
                .Connection = con
                Try
                    If .Connection.State = ConnectionState.Closed Then .Connection.Open()
                    .CommandTimeout = 0
                    .CommandText = "SELECT * FROM [" & sheetname & "$]"
                    dtable.Load(.ExecuteReader)
                Catch ex As Exception
                    dtable.Dispose() : dtable = Nothing
                End Try

                If .Connection.State = ConnectionState.Open Then .Connection.Close()
                .Connection.Dispose() : .Dispose()
            End With

            If dtable IsNot Nothing Then
                Try
                    If dtable.Columns.Contains("F1") Then
                        If dtable.Columns.CanRemove(dtable.Columns("F1")) Then dtable.Columns.Remove("F1")
                    End If
                Catch ex As Exception
                End Try
            End If

            Return dtable
        End Function

        Private Shared Sub ReleaseResourceObject(ByVal obj As Object)
            Try
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            Catch ex As Exception
            Finally
                obj = Nothing
            End Try
        End Sub
    End Class
End Namespace