Partial Public Class MySQL
    ''' <summary>
    ''' Class for MySQL query executions and data loading.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Que
        Implements IDisposable

#Region "Enumerations"
        ''' <summary>
        ''' Query execution.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum ExecutionEnum
            ''' <summary>
            ''' ExecuteReader : Run a query and load its result set into a DataTable.
            ''' </summary>
            ''' <remarks></remarks>
            ExecuteReader
            ''' <summary>
            ''' ExecuteNonQuery : runs a data updating query and count the rows affected by the statement.
            ''' </summary>
            ''' <remarks></remarks>
            ExecuteNonQuery
        End Enum

        ''' <summary>
        ''' Query retrieval method.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum DataRetrieverEnum
            ''' <summary>
            ''' Use sql data reader.
            ''' </summary>
            ''' <remarks></remarks>
            UseDataReader = 0
            ''' <summary>
            ''' Use sql data adapter and dispose the resource adapter.
            ''' </summary>
            ''' <remarks></remarks>
            UseAdapter = 1
            ''' <summary>
            ''' Use sql data adapter and retain the resource adapter.
            ''' </summary>
            ''' <remarks></remarks>
            UseAdapterAndRetain
        End Enum
#End Region

#Region "Sub New"
        ''' <summary>
        ''' Creates a new instance of TarsierEyes.MySQL.Que.
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            _commandtext = String.Empty : _errormessage = String.Empty
            _columns = -1 : _rows = -1
            _datatable.Dispose() : _datatable = Nothing : _maxallowedpacket = 150
        End Sub

        ''' <summary>
        ''' Creates a new instance of TarsierEyes.MySQL.Que.
        ''' </summary>
        ''' <param name="connection">MySQL server database connection string.</param>
        ''' <remarks></remarks>
        Sub New(ByVal connection As String)
            _commandtext = String.Empty : _connectionstring = connection
            _errormessage = String.Empty
            _columns = -1 : _rows = -1
            _datatable.Dispose() : _datatable = Nothing : _maxallowedpacket = 150
        End Sub

        ''' <summary>
        ''' Creates a new instance of TarsierEyes.MySQL.Que.
        ''' </summary>
        ''' <param name="connection">MySQL server database connection string.</param>
        ''' <param name="command">MySQL command to be executed.</param>
        ''' <remarks></remarks>
        Sub New(ByVal connection As String, ByVal command As String)
            _commandtext = command : _connectionstring = connection
            _errormessage = String.Empty
            _columns = -1 : _rows = -1
            _datatable.Dispose() : _datatable = Nothing : _maxallowedpacket = 150
        End Sub

        ''' <summary>
        ''' Creates a new instance of TarsierEyes.MySQL.Que.
        ''' </summary>
        ''' <param name="connection">MySQL server database connection string.</param>
        ''' <param name="command">MySQL command to be executed.</param>
        ''' <param name="maxpacket">Max allowed packet for each execution (in MB).</param>
        ''' <remarks></remarks>
        Sub New(ByVal connection As String, ByVal command As String, ByVal maxpacket As Integer)
            _commandtext = command : _connectionstring = connection
            _errormessage = String.Empty
            _columns = -1 : _rows = -1
            _datatable.Dispose() : _datatable = Nothing : _maxallowedpacket = maxpacket
        End Sub
#End Region

#Region "Properties"
        Dim _adapter As MySqlDataAdapter = Nothing

        ''' <summary>
        ''' Gets the retained sql data adapter.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Adapter() As MySqlDataAdapter
            Get
                Return _adapter
            End Get
        End Property

        Dim _columns As Integer = 0
        ''' <summary>
        ''' Gets resultset column count after query execution. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Columns() As Integer
            Get
                If _datatable IsNot Nothing Then
                    Return _datatable.Columns.Count
                Else
                    Return _columns
                End If
            End Get
        End Property

        Dim _commandtext As String = String.Empty
        ''' <summary>
        ''' Gets or sets MySQL command text to execute.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CommandText() As String
            Get
                Return _commandtext
            End Get
            Set(ByVal value As String)
                _commandtext = value
            End Set
        End Property

        Dim _connectionstring As String = String.Empty
        ''' <summary>
        ''' Gets or sets MySQL server database connection string.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConnectionString() As String
            Get
                Return _connectionstring
            End Get
            Set(ByVal value As String)
                _connectionstring = value
            End Set
        End Property

        Dim _datatable As New DataTable
        ''' <summary>
        ''' Gets resultset after query execution.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DataTable() As DataTable
            Get
                Return _datatable
            End Get
        End Property

        Dim _errormessage As String = String.Empty
        ''' <summary>
        ''' Gets error message catched upon query execution.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ErrorMessage() As String
            Get
                Return _errormessage
            End Get
        End Property

        Dim _maxallowedpacket As Integer = 150
        ''' <summary>
        ''' Gets or sets max allowed packet for each query execution (in MB).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MaxAllowedPacket() As Integer
            Get
                Return _maxallowedpacket
            End Get
            Set(ByVal value As Integer)
                _maxallowedpacket = value
            End Set
        End Property

        Dim _rows As Long = 0
        ''' <summary>
        ''' Gets number of rows retreived or affected by the query execution.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Rows() As Long
            Get
                If _datatable IsNot Nothing Then
                    Return _datatable.Rows.Count
                Else
                    Return _rows
                End If
            End Get
        End Property
#End Region

#Region "Methods"
        ''' <summary>
        ''' Performs data updating queries in the database.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ExecuteNonQuery()
            _errormessage = String.Empty : _columns = -1 : _rows = -1
            If _datatable IsNot Nothing Then
                _datatable.Dispose() : _datatable = Nothing
            End If

            Dim _constring As String = _connectionstring
            If ConnectionStringValue(_connectionstring, "ALLOW USER VARIABLES").ToLower <> "true" Then
                If Not _connectionstring.RLTrim.ToLower.Contains("allow user variables") Then : _constring &= IIf(_constring.RLTrim.EndsWith(";"), String.Empty, ";") & "ALLOW USER VARIABLES=TRUE;"
                Else
                    _constring = "SERVER=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.Server) & ";"
                    _constring &= "DATABASE=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.Database) & ";"
                    _constring &= "UID=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.UID) & ";"
                    _constring &= "PWD=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.PWD) & ";"
                    Dim _portno As String = ConnectionStringValue(_connectionstring, ConnectionDetailEnum.Port)
                    If Not String.IsNullOrEmpty(_portno.RLTrim) Then _constring &= "PORT=" & _portno & ";"
                    _constring &= "ALLOW USER VARIABLES=TRUE;"
                End If
            End If

            Dim mysqlcon As New MySqlConnection(_constring)
            Dim mysqlcmd As New MySqlCommand

            Try
                With mysqlcmd
                    .Connection = mysqlcon
                    If .Connection.State = ConnectionState.Closed Then .Connection.Open()
                    .CommandTimeout = 0

                    .CommandText = "SET GLOBAL max_allowed_packet = " & _maxallowedpacket & " * (1024 * 1024);" & vbNewLine & _
                                   "SHOW VARIABLES;"
                    .ExecuteNonQuery()
                    .CommandText = "START TRANSACTION;" & vbNewLine & _
                                    _commandtext & IIf(Not _commandtext.Trim.EndsWith(";"), ";", String.Empty) & IIf(_commandtext.Trim.EndsWith(vbNewLine), String.Empty, vbNewLine) & _
                                    "COMMIT;"

                    _rows = .ExecuteNonQuery
                End With
            Catch ex As Exception
                _errormessage = ex.Message
            Finally
                With mysqlcmd
                    If .Connection.State = ConnectionState.Open Then .Connection.Close()
                    .Dispose() : mysqlcon.Dispose()
                End With
            End Try
        End Sub

        ''' <summary>
        ''' Performs retreiving and loading of data in the resultset data table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overloads Sub ExecuteReader()
            _errormessage = String.Empty
            _columns = -1 : _rows = -1
            If _datatable IsNot Nothing Then
                _datatable.Dispose() : _datatable = Nothing
            End If

            Dim _constring As String = _connectionstring
            If ConnectionStringValue(_connectionstring, "ALLOW USER VARIABLES").ToLower <> "true" Then
                If Not _connectionstring.RLTrim.ToLower.Contains("allow user variables") Then : _constring &= IIf(_constring.RLTrim.EndsWith(";"), String.Empty, ";") & "ALLOW USER VARIABLES=TRUE;"
                Else
                    _constring = "SERVER=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.Server) & ";"
                    _constring &= "DATABASE=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.Database) & ";"
                    _constring &= "UID=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.UID) & ";"
                    _constring &= "PWD=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.PWD) & ";"
                    Dim _portno As String = ConnectionStringValue(_connectionstring, ConnectionDetailEnum.Port)
                    If Not String.IsNullOrEmpty(_portno.RLTrim) Then _constring &= "PORT=" & _portno & ";"
                    _constring &= "ALLOW USER VARIABLES=TRUE;"
                End If
            End If

            Dim olecon As New OleDb.OleDbConnection("Provider=MSDataShape.1;DRIVER={MySQL ODBC 3.51 Driver};" & _constring)
            Dim olecmd As New OleDb.OleDbCommand : Dim oleadp As New OleDb.OleDbDataAdapter(_commandtext, olecon)

            Try
                With olecmd
                    .Connection = olecon
                    If .Connection.State = ConnectionState.Closed Then .Connection.Open()
                    .CommandTimeout = 0

                    .CommandText = "SET GLOBAL max_allowed_packet = " & _maxallowedpacket & " * (1024 * 1024);"
                    .ExecuteNonQuery()

                    .CommandText = _commandtext

                    _datatable = New DataTable

                    Try
                        oleadp.FillSchema(_datatable, SchemaType.Mapped)
                    Catch ex As Exception
                    Finally
                    End Try

                    Try
                        oleadp.Fill(_datatable)
                        _rows = _datatable.Rows.Count : _columns = _datatable.Columns.Count

                        For Each col As DataColumn In _datatable.Columns
                            col.ReadOnly = False

                            If col.Unique Then
                                If Not col.AutoIncrement Then
                                    Select Case col.DataType.Name
                                        Case GetType(String).Name : col.DefaultValue = String.Empty
                                        Case GetType(Date).Name : col.DefaultValue = #1/1/1900#
                                        Case GetType(SByte).Name, GetType(Byte).Name, _
                                             GetType(Integer).Name, GetType(Long).Name, _
                                             GetType(Int16).Name, GetType(Int32).Name, GetType(Int64).Name, _
                                             GetType(Single).Name, GetType(Decimal).Name, GetType(Double).Name : col.DefaultValue = 0
                                        Case Else
                                    End Select
                                End If
                            Else
                                col.AllowDBNull = True
                                If Not col.AutoIncrement Then
                                    Select Case col.DataType.Name
                                        Case GetType(String).Name : col.DefaultValue = String.Empty
                                        Case GetType(Date).Name : col.DefaultValue = #1/1/1900#
                                        Case GetType(SByte).Name, GetType(Byte).Name, _
                                             GetType(Integer).Name, GetType(Long).Name, _
                                             GetType(Int16).Name, GetType(Int32).Name, GetType(Int64).Name, _
                                             GetType(Single).Name, GetType(Decimal).Name, GetType(Double).Name : col.DefaultValue = 0
                                        Case Else
                                    End Select
                                End If
                            End If
                        Next

                    Catch ex As Exception
                        _errormessage = ex.Message
                    End Try

                End With
            Catch ex As Exception
                _errormessage = ex.Message : _rows = -1
            Finally
                oleadp.Dispose()
                With olecmd
                    If .Connection.State = ConnectionState.Open Then .Connection.Close()
                    .Connection.Dispose() : .Dispose()
                End With
            End Try
        End Sub

        ''' <summary>
        ''' Performs retreiving and loading of data in the resultset data table.
        ''' </summary>
        ''' <param name="retieval">Determines what method will be used for filling the record into the table.</param>
        ''' <remarks></remarks>
        Public Overloads Sub ExecuteReader(ByVal retieval As DataRetrieverEnum)
            If retieval = DataRetrieverEnum.UseDataReader Then : ExecuteReader()
            Else
                _errormessage = String.Empty
                _columns = -1 : _rows = -1
                If _datatable IsNot Nothing Then
                    _datatable.Dispose() : _datatable = Nothing
                End If

                Dim _constring As String = _connectionstring
                If ConnectionStringValue(_connectionstring, "ALLOW USER VARIABLES").ToLower <> "true" Then
                    If Not _connectionstring.RLTrim.ToLower.Contains("allow user variables") Then : _constring &= IIf(_constring.RLTrim.EndsWith(";"), String.Empty, ";") & "ALLOW USER VARIABLES=TRUE;"
                    Else
                        _constring = "SERVER=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.Server) & ";"
                        _constring &= "DATABASE=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.Database) & ";"
                        _constring &= "UID=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.UID) & ";"
                        _constring &= "PWD=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.PWD) & ";"
                        Dim _portno As String = ConnectionStringValue(_connectionstring, ConnectionDetailEnum.Port)
                        If Not String.IsNullOrEmpty(_portno.RLTrim) Then _constring &= "PORT=" & _portno & ";"
                        _constring &= "ALLOW USER VARIABLES=TRUE;"
                    End If
                End If

                Dim con As New MySqlConnection(_constring)
                Dim adp As New MySqlDataAdapter(_commandtext, con)
                Dim cmd As New MySqlCommand

                If _datatable IsNot Nothing Then
                    _datatable.Dispose() : _datatable = Nothing
                End If

                Try
                    If con.State = ConnectionState.Closed Then con.Open()
                Catch ex As Exception
                    _errormessage = ex.Message : _rows = -1
                End Try

                If Not String.IsNullOrEmpty(_errormessage.RLTrim) Then Exit Sub

                Try
                    With cmd
                        .Connection = con : .CommandTimeout = 0
                        .CommandText = "SET GLOBAL max_allowed_packet = " & _maxallowedpacket & " * (1024 * 1024);"
                        .ExecuteNonQuery()
                    End With
                Catch ex As Exception
                Finally : cmd.Dispose()
                End Try
    
                Try
                    _datatable = New DataTable

                    Try
                        adp.FillSchema(_datatable, SchemaType.Mapped)
                    Catch ex As Exception
                    End Try

                    If _datatable.Constraints.Count > 0 Then _datatable.Constraints.Clear()

                    Try
                        adp.Fill(_datatable)
                        _rows = _datatable.Rows.Count : _columns = _datatable.Columns.Count

                        For Each col As DataColumn In _datatable.Columns
                            col.ReadOnly = False

                            If col.Unique Then
                                If Not col.AutoIncrement Then
                                    Select Case col.DataType.Name
                                        Case GetType(String).Name : col.DefaultValue = String.Empty
                                        Case GetType(Date).Name : col.DefaultValue = #1/1/1900#
                                        Case GetType(SByte).Name, GetType(Byte).Name, _
                                             GetType(Integer).Name, GetType(Long).Name, _
                                             GetType(Int16).Name, GetType(Int32).Name, GetType(Int64).Name, _
                                             GetType(Single).Name, GetType(Decimal).Name, GetType(Double).Name : col.DefaultValue = 0
                                        Case Else
                                    End Select
                                End If
                            Else
                                col.AllowDBNull = True
                                If Not col.AutoIncrement Then
                                    Select Case col.DataType.Name
                                        Case GetType(String).Name : col.DefaultValue = String.Empty
                                        Case GetType(Date).Name : col.DefaultValue = #1/1/1900#
                                        Case GetType(SByte).Name, GetType(Byte).Name, _
                                             GetType(Integer).Name, GetType(Long).Name, _
                                             GetType(Int16).Name, GetType(Int32).Name, GetType(Int64).Name, _
                                             GetType(Single).Name, GetType(Decimal).Name, GetType(Double).Name : col.DefaultValue = 0
                                        Case Else
                                    End Select
                                End If 
                            End If
                        Next

                    Catch ex As Exception
                        If _datatable IsNot Nothing Then
                            If ex.Message.ToLower.Contains("constraints") Then
                                _datatable.Constraints.Clear()
                                _rows = _datatable.Rows.Count : _columns = _datatable.Columns.Count
                            Else
                                _errormessage = ex.Message : _rows = -1
                            End If
                        Else
                            _errormessage = ex.Message : _rows = -1
                        End If
                    End Try

                Catch ex As Exception
                    If _datatable IsNot Nothing Then
                        If ex.Message.ToLower.Contains("constraints") Then
                            _datatable.Constraints.Clear()
                            _rows = _datatable.Rows.Count : _columns = _datatable.Columns.Count
                        Else
                            _errormessage = ex.Message : _rows = -1
                        End If
                    Else
                        _errormessage = ex.Message : _rows = -1
                    End If
                Finally
                    If Not String.IsNullOrEmpty(_errormessage.Trim) Then
                        adp.Dispose()
                        With con
                            If .State = ConnectionState.Open Then .Close()
                            .Dispose()
                        End With
                    Else
                        If retieval = DataRetrieverEnum.UseAdapter Then
                            adp.Dispose()
                            With con
                                If .State = ConnectionState.Open Then .Close()
                                .Dispose()
                            End With
                        Else : _adapter = adp
                        End If
                    End If
                End Try
            End If
        End Sub

        Dim progress As Object = Nothing

        Private Sub DataTable_Filled(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)
            If progress IsNot Nothing Then
                If progress.Maximum > progress.Value Then progress.Value += 1
            End If
        End Sub

        ''' <summary>
        ''' Performs retreiving and loading of data in the resultset data table.
        ''' </summary>
        ''' <param name="progressbar">Progress bar object to reflect the loading status of the result set.</param>
        ''' <remarks></remarks>
        Public Overloads Sub ExecuteReader(ByVal progressbar As Object)
            _errormessage = String.Empty
            _columns = -1 : _rows = -1
            If _datatable IsNot Nothing Then
                _datatable.Dispose() : _datatable = Nothing
            End If

            Dim _constring As String = _connectionstring
            If ConnectionStringValue(_connectionstring, "ALLOW USER VARIABLES").ToLower <> "true" Then
                If Not _connectionstring.RLTrim.ToLower.Contains("allow user variables") Then : _constring &= IIf(_constring.RLTrim.EndsWith(";"), String.Empty, ";") & "ALLOW USER VARIABLES=TRUE;"
                Else
                    _constring = "SERVER=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.Server) & ";"
                    _constring &= "DATABASE=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.Database) & ";"
                    _constring &= "UID=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.UID) & ";"
                    _constring &= "PWD=" & ConnectionStringValue(_connectionstring, ConnectionDetailEnum.PWD) & ";"
                    Dim _portno As String = ConnectionStringValue(_connectionstring, ConnectionDetailEnum.Port)
                    If Not String.IsNullOrEmpty(_portno.RLTrim) Then _constring &= "PORT=" & _portno & ";"
                    _constring &= "ALLOW USER VARIABLES=TRUE;"
                End If
            End If

            Dim olecon As New OleDb.OleDbConnection("Provider=MSDataShape.1;DRIVER={MySQL ODBC 3.51 Driver};" & _constring)
            Dim olecmd As New OleDb.OleDbCommand

            Try
                With olecmd
                    .Connection = olecon
                    If .Connection.State = ConnectionState.Closed Then .Connection.Open()
                    .CommandTimeout = 0

                    .CommandText = "SET GLOBAL max_allowed_packet = " & _maxallowedpacket & " * (1024 * 1024);"
                    .ExecuteNonQuery()

                    .CommandText = _commandtext

                    _datatable = New DataTable

                    Dim oledtr As OleDb.OleDbDataReader = .ExecuteReader
                    Dim tablename As String = GetStatementTableName(_commandtext, olecon)

                    Select Case progressbar.GetType.FullName
                        Case "DevComponents.DotNetBar.Controls.ProgressBarX", _
                             "DevComponents.DotNetBar.ProgressBarItem", _
                             "System.Windows.Forms.ProgressBar"
                            Dim max As Integer = oledtr.RecordsAffected
                            progress = progressbar
                            progress.Value = 0
                            progress.Maximum = max

                            AddHandler _datatable.RowChanged, AddressOf DataTable_Filled
                        Case Else
                            If progressbar.GetType.BaseType Is GetType(System.Windows.Forms.ProgressBar) Then
                                Dim max As Integer = oledtr.RecordsAffected
                                progress = progressbar
                                progress.Value = 0
                                progress.Maximum = max

                                AddHandler _datatable.RowChanged, AddressOf DataTable_Filled
                            End If
                    End Select

                    If String.IsNullOrEmpty(tablename.Trim) Then tablename = GetStatementTableName(_commandtext)
                    _datatable.Load(oledtr) : _datatable.TableName = tablename
                    RemoveHandler _datatable.RowChanged, AddressOf DataTable_Filled
                    progress = Nothing
                    _rows = _datatable.Rows.Count : _columns = _datatable.Columns.Count
                End With
            Catch ex As Exception
                _errormessage = ex.Message : _rows = -1
            Finally
                With olecmd
                    If .Connection.State = ConnectionState.Open Then .Connection.Close()
                    .Connection.Dispose() : .Dispose()
                End With
            End Try
        End Sub
#End Region

#Region "Functions"
        ''' <summary>
        ''' Calls ExecuteReader method asynchronously.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Overloads Function InvokeReader() As IAsyncResult
            runningdelegate = Nothing
            Dim delExec As New Action(AddressOf ExecuteReader)
            runningdelegate = delExec
            Return delExec.BeginInvoke(Nothing, delExec)
        End Function

        ''' <summary>
        ''' Calls ExecuteReader method asynchronously.
        ''' </summary>
        ''' <param name="progressbar">Progress bar object to reflect the loading status of the result set.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Overloads Function InvokeReader(ByVal progressbar As Object) As IAsyncResult
            runningdelegate = Nothing
            Dim delExec As New Action(Of Object)(AddressOf ExecuteReader)
            runningdelegate = delExec
            Return delExec.BeginInvoke(progressbar, Nothing, delExec)
        End Function

        ''' <summary>
        ''' Calls ExecuteNonQuery method asynchronously.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function InvokeNonQuery() As IAsyncResult
            runningdelegate = Nothing
            Dim delExec As New Action(AddressOf ExecuteNonQuery)
            runningdelegate = delExec
            Return delExec.BeginInvoke(Nothing, delExec)
        End Function

        ''' <summary>
        ''' Gets output datatable from iterated datareader using the supplied connection string and command text in this instance of TarsierEyes.MySQL.Que.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function RedirectLoad() As DataTable
            Dim con As New MySqlConnection(_connectionstring)
            Dim cmd As New MySqlCommand
            Dim dtr As MySqlDataReader
            Dim dtable As New DataTable

            Try
                With cmd
                    .Connection = con
                    If .Connection.State = ConnectionState.Closed Then .Connection.Open()
                    .CommandTimeout = 0 : .CommandText = _commandtext
                    If .CommandTimeout <= 30 Then .CommandTimeout = 9999999
                    dtr = .ExecuteReader

                    Dim dtColumns As DataTable = dtr.GetSchemaTable
                    If dtColumns.Rows.Count > 0 Then
                        For iRow As Integer = 0 To dtColumns.Rows.Count - 1
                            dtable.Columns.Add(dtColumns.Rows(iRow).Item(0).ToString.Trim, Type.GetType(dtColumns.Rows(iRow).Item("DataType").ToString.Trim))
                        Next
                    End If
                    dtColumns.Dispose()

                    While dtr.Read
                        Dim objRow(dtable.Columns.Count - 1) As Object
                        For iCol As Integer = 0 To dtable.Columns.Count - 1
                            objRow(iCol) = dtr.Item(iCol)
                        Next
                        dtable.Rows.Add(objRow)
                        objRow = Nothing
                    End While

                    If Not dtr.IsClosed Then dtr.Close()
                End With

            Catch ex As Exception
            Finally
                dtr = Nothing

                With cmd
                    If .Connection.State = ConnectionState.Open Then .Connection.Close()
                    .Dispose() : con.Dispose()
                End With
            End Try

            Return dtable
        End Function
#End Region

#Region "GetValue"
        ''' <summary>
        ''' Gets the value of the first column in the first row of the result set.  
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function GetValue() As Object
            Return GetValue(Of Object)()
        End Function

        ''' <summary>
        ''' Gets the value of the first column in the first row of the result set in type-safe manner.
        ''' </summary>
        ''' <typeparam name="TResult">Expected data type of the returning value.</typeparam>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function GetValue(Of TResult)() As TResult
            Dim obj As TResult
            ExecuteReader(DataRetrieverEnum.UseAdapter)
            If _rows > 0 Then
                If Not IsDBNull(_datatable.Rows(0).Item(0)) Then obj = _datatable.Rows(0).Item(0)
            End If
            Return obj
        End Function

        ''' <summary>
        ''' Gets the value of the first column in the first row of the result set.  
        ''' </summary>
        ''' <param name="defaultvalue">Default value to return if there is nothing.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function GetValue(ByVal defaultvalue As Object) As Object
            Return GetValue(Of Object)(Nothing)
        End Function

        ''' <summary>
        ''' Gets the value of the first column in the first row of the result set in type-safe manner.
        ''' </summary>
        ''' <typeparam name="TResult">Expected data type of the returning value.</typeparam>
        ''' <param name="defaultvalue">Default value to return if there is nothing.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function GetValue(Of TResult)(ByVal defaultvalue As TResult) As TResult
            Dim obj As TResult = defaultvalue
            ExecuteReader(DataRetrieverEnum.UseAdapter)
            If _rows > 0 Then
                If Not IsDBNull(_datatable.Rows(0).Item(0)) Then obj = _datatable.Rows(0).Item(0)
            End If
            Return obj
        End Function

        ''' <summary>
        ''' Gets the value of the first column in the first row of the result set.
        ''' </summary>
        ''' <param name="connection">Database connection string.</param>
        ''' <param name="command">Database command text to execute.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function GetValue(ByVal connection As String, ByVal command As String) As Object
            Return GetValue(connection, command, Nothing)
        End Function

        ''' <summary>
        ''' Gets the value of the first column in the first row of the result set.
        ''' </summary>
        ''' <param name="connection">Database connection string.</param>
        ''' <param name="command">Database command text to execute.</param>
        ''' <param name="defaultvalue">Default value to return if there is nothing.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function GetValue(ByVal connection As String, ByVal command As String, ByVal defaultvalue As Object) As Object
            Dim q As New Que(connection, command)
            Dim obj As Object = q.GetValue(defaultvalue)
            q.Dispose()
            Return obj
        End Function

        ''' <summary>
        ''' Gets the value of the first column in the first row of the result set in type-safe manner.
        ''' </summary>
        ''' <typeparam name="TResult">Expected data type of the returning value.</typeparam>
        ''' <param name="connection">Database connection string.</param>
        ''' <param name="command">Database command text to execute.</param>
        ''' <param name="defaultvalue">Default value to return if there is nothing.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function GetValue(Of TResult)(ByVal connection As String, ByVal command As String, ByVal defaultvalue As TResult) As TResult
            Dim q As New Que(connection, command)
            Dim obj As TResult = q.GetValue(Of TResult)(defaultvalue)
            q.Dispose()
            Return obj
        End Function
#End Region

#Region "GetValues"
        ''' <summary>
        ''' Gets the value of the first column of each retreived result set rows.
        ''' </summary>
        ''' <param name="connection">Database connection string.</param>
        ''' <param name="command">Database command text to execute.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function GetValues(ByVal connection As String, ByVal command As String) As List(Of Object)
            Return GetValues(Of Object)(connection, command)
        End Function

        ''' <summary>
        ''' Gets the value of the first column of each retreived result set rows in type-safe manner.
        ''' </summary>
        ''' <typeparam name="TResult">Expected data type of the returning value.</typeparam>
        ''' <param name="connection">Database connection string.</param>
        ''' <param name="command">Database command text to execute.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function GetValues(Of TResult)(ByVal connection As String, ByVal command As String) As List(Of TResult)
            Dim objs As New List(Of TResult)
            objs.Clear()

            Dim q As Que = Que.Execute(connection, command, ExecutionEnum.ExecuteReader)
            If q.Rows > 0 Then
                With q.DataTable
                    For Each drow As DataRow In .Rows
                        If Not IsDBNull(drow.Item(0)) Then objs.Add(drow.Item(0))
                    Next
                End With
            End If
            q.Dispose()

            Return objs
        End Function
#End Region

#Region "Shared Functions"
        Private Shared runningdelegate As Object = Nothing
        Private Shared runningque As Object = Nothing

        ''' <summary>
        ''' Calls TarsierEyes.MySQL.Que class' Invoke functions to perform synchronous query execution.
        ''' </summary>
        ''' <param name="connection">Database server connection string.</param>
        ''' <param name="query">Command text to execute.</param>
        ''' <param name="execute">Type of execution.</param>
        ''' <returns></returns>
        ''' <remarks>Must call EndInvoke function to get que results.</remarks>
        Public Overloads Shared Function BeginInvoke(ByVal connection As String, ByVal query As String, ByVal execute As ExecutionEnum) As IAsyncResult
            Dim arExec As IAsyncResult = Nothing

            runningque = Nothing
            Dim q As New Que(connection, query)
            runningque = q

            Select Case execute
                Case ExecutionEnum.ExecuteReader : arExec = q.InvokeReader
                Case ExecutionEnum.ExecuteNonQuery : arExec = q.InvokeNonQuery
                Case Else
            End Select

            Return arExec
        End Function

        ''' <summary>
        ''' Calls TarsierEyes.MySQL.Que class' Invoke functions to perform synchronous query execution.
        ''' </summary>
        ''' <param name="connection">Database server connection string.</param>
        ''' <param name="query">Command text to execute.</param>
        ''' <param name="execute">Type of execution.</param>
        ''' <param name="progressbar">Progress bar object to reflect the loading status.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function BeginInvoke(ByVal connection As String, ByVal query As String, ByVal execute As ExecutionEnum, ByVal progressbar As Object) As IAsyncResult
            Dim arExec As IAsyncResult = Nothing

            runningque = Nothing
            Dim q As New Que(connection, query)
            runningque = q

            Select Case execute
                Case ExecutionEnum.ExecuteReader : arExec = q.InvokeReader(progressbar)
                Case ExecutionEnum.ExecuteNonQuery : arExec = q.InvokeNonQuery
                Case Else
            End Select

            Return arExec
        End Function

        ''' <summary>
        ''' Ends the initiated BeginInvoke function to get que results and details.
        ''' </summary>
        ''' <param name="ar">AsyncResult interface use to call the BeginInvoke function.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function EndInvoke(ByVal ar As IAsyncResult) As Que
            If ar.IsCompleted Then
                If runningdelegate IsNot Nothing Then
                    If (TypeOf runningdelegate Is Action) Or _
                       (TypeOf runningdelegate Is Action(Of Object)) Then
                        runningdelegate.EndInvoke(ar)
                        Return runningque : Exit Function
                    End If
                End If
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' Calls the TarsierEyes.MySQL.Que class' ExecuteReader or ExecuteNonQuery methods to execute query commands.
        ''' </summary>
        ''' <param name="connection">Database server connection string.</param>
        ''' <param name="query">Database command text.</param>
        ''' <param name="execution">Execution type.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Execute(ByVal connection As String, ByVal query As String, ByVal execution As ExecutionEnum) As Que
            Dim q As New Que(connection, query)
            Select Case execution
                Case ExecutionEnum.ExecuteNonQuery : q.ExecuteNonQuery()
                Case ExecutionEnum.ExecuteReader
                    Dim matches As MatchCollection = Regex.Matches(query, "@[A-Za-z0-9]+:=", RegexOptions.IgnoreCase)
                    If matches.Count > 0 Then : q.ExecuteReader(DataRetrieverEnum.UseAdapter)
                    Else
                        matches = Regex.Matches(query, "@[A-Za-z0-9]+", RegexOptions.IgnoreCase)
                        If matches.Count > 0 Then : q.ExecuteReader(DataRetrieverEnum.UseAdapter)
                        Else : q.ExecuteReader(DataRetrieverEnum.UseDataReader)
                        End If
                    End If
                Case Else
            End Select
            Return q
        End Function

        ''' <summary>
        ''' Calls the TarsierEyes.MySQL.Que class' ExecuteReader or ExecuteNonQuery methods to execute query commands.
        ''' </summary>
        ''' <param name="connection">Database server connection string.</param>
        ''' <param name="query">Database command text.</param>
        ''' <param name="execution">Execution type.</param>
        ''' <param name="retriver">Determines what method will be used for filling up the records into the table.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Execute(ByVal connection As String, ByVal query As String, ByVal execution As ExecutionEnum, ByVal retriver As DataRetrieverEnum) As Que
            Dim q As New Que(connection, query)
            Select Case execution
                Case ExecutionEnum.ExecuteNonQuery : q.ExecuteNonQuery()
                Case ExecutionEnum.ExecuteReader : q.ExecuteReader(retriver)
                Case Else
            End Select
            Return q
        End Function

        ''' <summary>
        ''' Calls the TarsierEyes.MySQL.Que class' ExecuteReader or ExecuteNonQuery methods to execute query commands.
        ''' </summary>
        ''' <param name="connection">Database server connection string.</param>
        ''' <param name="query">Database command text.</param>
        ''' <param name="execution">Execution type.</param>
        ''' <param name="progressbar">Progress bar object to reflect the loading status.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Execute(ByVal connection As String, ByVal query As String, ByVal execution As ExecutionEnum, ByVal progressbar As Object) As Que
            Dim q As New Que(connection, query)
            Select Case execution
                Case ExecutionEnum.ExecuteNonQuery : q.ExecuteNonQuery()
                Case ExecutionEnum.ExecuteReader : q.ExecuteReader(progressbar)
                Case Else
            End Select
            Return q
        End Function

        ''' <summary>
        ''' Gets output datatable from iterated datareader using the supplied connection string and command text.
        ''' </summary>
        ''' <param name="connection">MySQL database connection string.</param>
        ''' <param name="command">MySQL query statement.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function RedirectLoad(ByVal connection As String, ByVal command As String) As DataTable
            Dim q As New Que(connection, command)
            With q
                Dim dtable As DataTable = .RedirectLoad
                .Dispose()
                Return dtable
            End With
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
                    If _adapter IsNot Nothing Then
                        Try
                            Dim con As MySqlConnection = _adapter.SelectCommand.Connection
                            _adapter.Dispose()
                            With con
                                If .State = ConnectionState.Open Then .Close()
                                .Dispose()
                            End With
                        Catch ex As Exception
                        End Try
                    End If
                    If _datatable IsNot Nothing Then _datatable.Dispose()
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
