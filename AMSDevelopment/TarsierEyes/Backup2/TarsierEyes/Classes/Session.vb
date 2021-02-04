Partial Public Class MySQL

#Region "Enumerations"

    ''' <summary>
    ''' Database session table enumerations.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum SessionTables
        ''' <summary>
        ''' Database session's BaseTable
        ''' </summary>
        ''' <remarks></remarks>
        BaseTable = 0
        ''' <summary>
        ''' Database session's ViewTable
        ''' </summary>
        ''' <remarks></remarks>
        ViewTable = 1
    End Enum

#End Region

    ''' <summary>
    ''' Database session and control binding class.
    ''' </summary>
    ''' <remarks></remarks>
    <Description("Database session and control binding class.")> _
    Public Class Session
        Implements IDisposable

#Region "Events"
        ''' <summary>
        ''' Occurs after the BaseTable and ViewTable data loading routines has been called and executed.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        <Description("Occurs after the BaseTable and ViewTable data loading routines has been called and executed.")> _
        Public Event AfterDataLoading(ByVal sender As Object, ByVal e As SessionEventArgs)

        ''' <summary>
        ''' Occurs after the database updating routines were done.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        <Description("Occurs after the database updating routines were done.")> _
        Public Event AfterUpdate(ByVal sender As Object, ByVal e As SessionEventArgs)

        ''' <summary>
        ''' Occurs before BaseTable and ViewTable will be filled by data.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        <Description("Occurs before BaseTable and ViewTable will be filled by data.")> _
        Public Event BeforeDataLoading(ByVal sender As Object, ByVal e As SessionEventArgs)

        ''' <summary>
        ''' Occurs before the actual database updating takes effect.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        <Description("Occurs before the actual database updating takes effect.")> _
        Public Event BeforeUpdate(ByVal sender As Object, ByVal e As SessionEventArgs)

        ''' <summary>
        ''' Occurs when database updating failed due to an unhandled exception.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        <Description("Occurs when database updating failed due to an unhandled exception.")> _
        Public Event UpdateFailed(ByVal sender As Object, ByVal e As SessionEventArgs)

        ''' <summary>
        ''' Occurs when all of the updates into the current database session and its dependent detail sessions has been committed.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        <Description("Occurs when all of the updates into the current database session and its dependent detail sessions has been committed.")> _
        Public Event UpdateFinalized(ByVal sender As Object, ByVal e As SessionEventArgs)

        ''' <summary>
        ''' Occurs when committing all of the updates into the current database session and its dependent detail sessions.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        <Description("Occurs when committing all of the updates into the current database session and its dependent detail sessions.")> _
        Public Event UpdateFinalizing(ByVal sender As Object, ByVal e As SessionEventArgs)

        ''' <summary>
        ''' Occurs when database updating succeeds.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        <Description("Occurs when database updating succeeds.")> _
        Public Event UpdateSucceed(ByVal sender As Object, ByVal e As SessionEventArgs)

#End Region

#Region "Properties"
        Private _adapter As MySqlDataAdapter = Nothing

        ''' <summary>
        ''' Gets the adapter object for this current database session.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> _
        Public ReadOnly Property Adapter() As MySqlDataAdapter
            Get
                Return _adapter
            End Get
        End Property

        Private _basecommandtext As String = String.Empty

        ''' <summary>
        ''' Gets or sets the current database session's sql statement that is used to load data into the current session's table.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BaseCommandText() As String
            Get
                Return _basecommandtext
            End Get
            Set(ByVal value As String)
                _basecommandtext = value
            End Set
        End Property

        Private _basetable As DataTable = Nothing

        ''' <summary>
        ''' Gets the updating table for this current database session.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> _
        Public ReadOnly Property BaseTable() As DataTable
            Get
                Return _basetable
            End Get
        End Property

        Private _connection As MySqlConnection = Nothing

        ''' <summary>
        ''' Gets the current database session's database connection object.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> _
        Public ReadOnly Property Connection() As MySqlConnection
            Get
                Return _connection
            End Get
        End Property

        Private _connectionstring As String = "SERVER=localhost;DATABASE=mysql;UID=root;PWD=nl2009;"

        ''' <summary>
        ''' Gets or sets the current database session's connection string.
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

        Private _currenteventargument As SessionEventArgs = Nothing

        ''' <summary>
        ''' Gets the current running base event argument of the current database session.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> _
        Public ReadOnly Property CurrentEventArgument() As SessionEventArgs
            Get
                Return _currenteventargument
            End Get
        End Property

        Private _databinding As New List(Of SessionDataBinding)

        Private _details As New SessionCollection(Me)

        ''' <summary>
        ''' Gets the collection of detail database session parented into the current database session (header-detail table relationship).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> _
        Public ReadOnly Property Details() As SessionCollection
            Get
                Return _details
            End Get
        End Property

        Private _foreignkey As String = String.Empty

        ''' <summary>
        ''' Gets or sets the session table's assigned primary key field. Field value will then be refered to the parent's PrimaryKey field (for header-detail table relationship). 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ForeignKey() As String
            Get
                Return _foreignkey
            End Get
            Set(ByVal value As String)
                _foreignkey = value
            End Set
        End Property

        Private _header As Session = Nothing

        ''' <summary>
        ''' Gets the current database session's parent (header-detail table relationship).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> _
        Public ReadOnly Property Header() As Session
            Get
                Return _header
            End Get
        End Property

        Private _primarykey As String = String.Empty

        ''' <summary>
        ''' Gets or sets the session table's assigned primary key field. This will then be refered into dependent details thru ForeignKey assigned value (for header-detail table relationship).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PrimaryKey() As String
            Get
                Return _primarykey
            End Get
            Set(ByVal value As String)
                _primarykey = value
            End Set
        End Property

        Private _sqlstatements As New SessionStatements(Me)

        ''' <summary>
        ''' Gets the current database session's table-updating sql command statements.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> _
        Public ReadOnly Property SqlStatements() As SessionStatements
            Get
                Return _sqlstatements
            End Get
        End Property

        Private _transaction As MySqlTransaction = Nothing

        ''' <summary>
        ''' Gets the current database session's updating transaction object.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> _
        Public ReadOnly Property Transaction() As MySqlTransaction
            Get
                Return _transaction
            End Get
        End Property

        Private _viewcommandtext As String = String.Empty

        ''' <summary>
        ''' Gets or sets the customized schema database session table's sql commadn statement that is currently representing the BaseTable. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ViewCommandText() As String
            Get
                Return _viewcommandtext
            End Get
            Set(ByVal value As String)
                _viewcommandtext = value
            End Set
        End Property

        Private _viewtable As DataTable = Nothing

        ''' <summary>
        ''' Gets the customized schema database session table that is currently representing the BaseTable.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> _
        Public ReadOnly Property ViewTable() As DataTable
            Get
                Return _viewtable
            End Get
        End Property

#End Region

#Region "New Instance"
        ''' <summary>
        ''' Creates a new instance of MySQL.Session.
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
        End Sub

        ''' <summary>
        ''' Creates a new instance of MySQL.Session.
        ''' </summary>
        ''' <param name="dbconnectionstring">Database connection string.</param>
        ''' <param name="basesqlstatement">Base table commandtext.</param>
        ''' <remarks></remarks>
        Sub New(ByVal dbconnectionstring As String, ByVal basesqlstatement As String)
            ConnectionString = dbconnectionstring : BaseCommandText = basesqlstatement : ViewCommandText = basesqlstatement
        End Sub

        ''' <summary>
        ''' Creates a new instance of MySQL.Session.
        ''' </summary>
        ''' <param name="dbconnectionstring">Database connection string.</param>
        ''' <param name="basesqlstatement">Base table commandtext.</param>
        ''' <param name="viewsqlstatement">View table commandtext.</param>
        ''' <remarks></remarks>
        Sub New(ByVal dbconnectionstring As String, ByVal basesqlstatement As String, ByVal viewsqlstatement As String)
            ConnectionString = dbconnectionstring : BaseCommandText = basesqlstatement : ViewCommandText = viewsqlstatement
        End Sub

        ''' <summary>
        ''' Creates a new instance of MySQL.Session. 
        ''' </summary>
        ''' <param name="parent">Header database session.</param>
        ''' <remarks></remarks>
        Sub New(ByVal parent As Session)
            _header = parent
        End Sub

        ''' <summary>
        ''' Creates a new instance of MySQL.Session. 
        ''' </summary>
        ''' <param name="parent">Header database session.</param>
        ''' <param name="dbconnectionstring">Database connection string.</param>
        ''' <param name="basesqlstatement">Base table commandtext.</param>
        ''' <remarks></remarks>
        Sub New(ByVal parent As Session, ByVal dbconnectionstring As String, ByVal basesqlstatement As String)
            _header = parent : ConnectionString = dbconnectionstring
            BaseCommandText = basesqlstatement : ViewCommandText = basesqlstatement
        End Sub

        ''' <summary>
        ''' Creates a new instance of MySQL.Session. 
        ''' </summary>
        ''' <param name="parent">Header database session.</param>
        ''' <param name="dbconnectionstring">Database connection string.</param>
        ''' <param name="basesqlstatement">Base table commandtext.</param>
        ''' <param name="viewsqlstatement">View table commandtext.</param>
        ''' <remarks></remarks>
        Sub New(ByVal parent As Session, ByVal dbconnectionstring As String, ByVal basesqlstatement As String, ByVal viewsqlstatement As String)
            _header = parent : ConnectionString = dbconnectionstring
            BaseCommandText = basesqlstatement : ViewCommandText = viewsqlstatement
        End Sub

#End Region

#Region "ViewTable Handlers"
        Private Sub ViewTable_RowChanged(ByVal sender As Object, ByVal e As DataRowChangeEventArgs)
            If BaseTable IsNot Nothing And _
               e.Row IsNot Nothing Then
                Dim rw As DataRow = e.Row : Dim _filter As String = String.Empty

                Select Case e.Action
                    Case DataRowAction.Add
                        Dim values(BaseTable.Columns.Count - 1) As Object

                        For Each col As DataColumn In BaseTable.Columns
                            If Not col.AutoIncrement Then values(col.Ordinal) = rw.Item(col.Ordinal)
                        Next

                        BaseTable.Rows.Add(values)

                    Case DataRowAction.Change, DataRowAction.ChangeCurrentAndOriginal, _
                         DataRowAction.ChangeOriginal
                        If Not String.IsNullOrEmpty(PrimaryKey.RLTrim) Then
                            If ViewTable.Columns.Contains(PrimaryKey) Then
                                _filter = "CONVERT([" & PrimaryKey & "], System.String) = '" & rw.Item(PrimaryKey).ToString.ToSqlValidString(True) & "'"
                                Dim rws() As DataRow = BaseTable.Select(_filter)
                                If rws.Length <= 0 Then _filter = String.Empty
                            End If
                        End If

                        If String.IsNullOrEmpty(_filter.RLTrim) Then

                            For Each col As DataColumn In BaseTable.Columns
                                Dim value As Object = rw.Item(col.ColumnName)

                                Try
                                    value = rw.Item(col.ColumnName, DataRowVersion.Original)
                                Catch ex As Exception
                                    value = rw.Item(col.ColumnName)
                                End Try

                                If Not Common.Simple.IsNullOrNothing(value) Then
                                    Select Case col.DataType.Name
                                        Case GetType(String).Name : _filter &= IIf(Not String.IsNullOrEmpty(_filter.RLTrim), " AND" & vbNewLine, String.Empty) & "([" & col.ColumnName & "] LIKE '" & value.ToString.ToSqlValidString(True) & "')"
                                        Case GetType(Date).Name : _filter &= IIf(Not String.IsNullOrEmpty(_filter.RLTrim), " AND" & vbNewLine, String.Empty) & "([" & col.ColumnName & "] = '" & Format(CDate(value), "MM/dd/yyyy HH:mm:ss") & "')"
                                        Case GetType(Integer).Name, GetType(Long).Name, _
                                             GetType(SByte).Name, GetType(Byte).Name, _
                                             GetType(Decimal).Name, GetType(Single).Name, GetType(Double).Name : _filter &= IIf(Not String.IsNullOrEmpty(_filter.RLTrim), " AND" & vbNewLine, String.Empty) & "([" & col.ColumnName & "] = " & value.ToString & ")"
                                    End Select
                                Else : _filter &= IIf(Not String.IsNullOrEmpty(_filter.RLTrim), " AND" & vbNewLine, String.Empty) & "([" & col.ColumnName & "] = NULL)"
                                End If
                            Next
                        End If

                        Dim dr() As DataRow = BaseTable.Select(_filter)
                        If dr.Length <= 0 Then
                            Dim values(BaseTable.Columns.Count - 1) As Object

                            For Each col As DataColumn In BaseTable.Columns
                                If Not col.AutoIncrement Then values(col.Ordinal) = rw.Item(col.Ordinal)
                            Next

                            BaseTable.Rows.Add(values)
                        Else
                            For Each col As DataColumn In BaseTable.Columns
                                If Not col.AutoIncrement Then dr(0).Item(col.ColumnName) = rw.Item(col.ColumnName)
                            Next
                        End If
                    Case Else
                End Select

                RemoveViewTableHandler() : rw.Table.AcceptChanges() : AttachViewTableHandlers()
            End If
        End Sub

        Private Sub ViewTable_RowDeleting(ByVal sender As Object, ByVal e As DataRowChangeEventArgs)
            If BaseTable IsNot Nothing And _
               e.Row IsNot Nothing Then
                Dim rw As DataRow = e.Row : Dim _filter As String = String.Empty

                If Not String.IsNullOrEmpty(PrimaryKey.RLTrim) Then
                    If ViewTable.Columns.Contains(PrimaryKey) Then
                        _filter = "CONVERT([" & PrimaryKey & "], System.String) = '" & rw.Item(PrimaryKey).ToString.ToSqlValidString(True) & "'"
                        Dim rws() As DataRow = BaseTable.Select(_filter)
                        If rws.Length <= 0 Then _filter = String.Empty
                    End If
                End If

                If String.IsNullOrEmpty(_filter.RLTrim) Then
                    For Each col As DataColumn In BaseTable.Columns
                        Dim value As Object = rw.Item(col.ColumnName)

                        Try
                            value = rw.Item(col.ColumnName, DataRowVersion.Original)
                        Catch ex As Exception
                            value = rw.Item(col.ColumnName)
                        End Try

                        If Not Common.Simple.IsNullOrNothing(value) Then
                            Select Case col.DataType.Name
                                Case GetType(String).Name : _filter &= IIf(Not String.IsNullOrEmpty(_filter.RLTrim), " AND" & vbNewLine, String.Empty) & "[`" & col.ColumnName & "] LIKE '" & value.ToString.ToSqlValidString(True) & "')"
                                Case GetType(Date).Name : _filter &= IIf(Not String.IsNullOrEmpty(_filter.RLTrim), " AND" & vbNewLine, String.Empty) & "[`" & col.ColumnName & "] = '" & Format(CDate(value), "MM/dd/yyyy HH:mm:ss") & "')"
                                Case GetType(Integer).Name, GetType(Long).Name, _
                                     GetType(SByte).Name, GetType(Byte).Name, _
                                     GetType(Decimal).Name, GetType(Single).Name, GetType(Double).Name : _filter &= IIf(Not String.IsNullOrEmpty(_filter.RLTrim), " AND" & vbNewLine, String.Empty) & "([" & col.ColumnName & "] = " & value.ToString & ")"
                            End Select
                        Else : _filter &= IIf(Not String.IsNullOrEmpty(_filter.RLTrim), " AND" & vbNewLine, String.Empty) & "([" & col.ColumnName & "] = NULL)"
                        End If
                    Next
                End If

                If Not String.IsNullOrEmpty(_filter.RLTrim) Then
                    Dim dr() As DataRow = BaseTable.Select(_filter)
                    If dr.Length > 0 Then dr(0).Delete()
                End If
            End If
        End Sub

#End Region

#Region "Functions"
        Private _currentloaddelegate As Action = Nothing
        Private _currentsavedelegate As Action = Nothing

        ''' <summary>
        ''' Loads data into the current database session's tables asynchronously.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BeginLoad() As IAsyncResult
            If _currentloaddelegate IsNot Nothing Then
                Try
                    _currentloaddelegate = Nothing
                Catch ex As Exception
                End Try
            End If

            _currentloaddelegate = New Action(AddressOf Load)
            Return _currentloaddelegate.BeginInvoke(Nothing, _currentloaddelegate)
        End Function

        ''' <summary>
        ''' Loads data into the current database session's tables and into its associated detail database sessions asynchronously.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BeginLoadAll() As IAsyncResult
            If _currentloaddelegate IsNot Nothing Then
                Try
                    _currentloaddelegate = Nothing
                Catch ex As Exception
                End Try
            End If

            _currentloaddelegate = New Action(AddressOf LoadAll)
            Return _currentloaddelegate.BeginInvoke(Nothing, _currentloaddelegate)
        End Function

        ''' <summary>
        ''' Saves the changes made from the current database session and its dependent detail sessions into the connected database tables asynchronously.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BeginSave() As IAsyncResult
            If _currentsavedelegate IsNot Nothing Then
                Try
                    _currentsavedelegate = Nothing
                Catch ex As Exception
                End Try
            End If

            _currentsavedelegate = New Action(AddressOf Save)
            Return _currentsavedelegate.BeginInvoke(Nothing, _currentsavedelegate)
        End Function

        Private Function SaveManually(ByVal args As SessionEventArgs, ByVal connectiontrans As MySqlTransaction) As Boolean
            Dim _saved As Boolean = False : Dim table As DataTable = args.Changes

            If table IsNot Nothing Then
                If table.Rows.Count > 0 Then
                    Dim _query As String = String.Empty
                    For Each rw As DataRow In table.Rows
                        Select Case rw.RowState
                            Case DataRowState.Added
                                Dim _sql As String = SqlStatements.InsertStatement
                                For i As Integer = 0 To table.Columns.Count - 1
                                    Dim col As DataColumn = table.Columns((table.Columns.Count - 1) - i)
                                    If Not Common.Simple.IsNullOrNothing(rw.Item(col.ColumnName)) Then
                                        Select Case col.DataType.Name
                                            Case GetType(String).Name : _sql = _sql.Replace("@p" & col.Ordinal, "'" & rw.Item(col.ColumnName).ToString.ToSqlValidString & "'")
                                            Case GetType(Date).Name : _sql = _sql.Replace("@p" & col.Ordinal, "'" & CDate(rw.Item(col.ColumnName)).ToSqlValidString(True) & "'")
                                            Case GetType(SByte).Name, GetType(Byte).Name, _
                                                 GetType(Integer).Name, GetType(Long).Name, _
                                                 GetType(Decimal).Name, GetType(Single).Name, GetType(Long).Name, _
                                                 "Int32", "Int64" : _sql = _sql.Replace("@p" & col.Ordinal, rw.Item(col.ColumnName).ToString)
                                            Case GetType(Boolean).Name : _sql = _sql.Replace("@p" & col.Ordinal, IIf(CBool(rw.Item(col.ColumnName)), 1, 0))
                                            Case Else
                                        End Select
                                    Else : _sql = _sql.Replace("@p" & col.Ordinal, "NULL")
                                    End If
                                Next

                                If Not _sql.RLTrim.EndsWith(";") Then _sql &= ";"
                                _query &= IIf(Not String.IsNullOrEmpty(_query.RLTrim), vbNewLine, String.Empty) & _sql

                            Case DataRowState.Modified
                                Dim _sql As String = SqlStatements.UpdateStatement
                                For i As Integer = 0 To table.Columns.Count - 1
                                    Dim col As DataColumn = table.Columns((table.Columns.Count - 1) - i)
                                    If Not Common.Simple.IsNullOrNothing(rw.Item(col.ColumnName)) Then
                                        Select Case col.DataType.Name
                                            Case GetType(String).Name : _sql = _sql.Replace("@p" & col.Ordinal, "'" & rw.Item(col.ColumnName).ToString.ToSqlValidString & "'")
                                            Case GetType(Date).Name : _sql = _sql.Replace("@p" & col.Ordinal, "'" & CDate(rw.Item(col.ColumnName)).ToSqlValidString(True) & "'")
                                            Case GetType(SByte).Name, GetType(Byte).Name, _
                                                 GetType(Integer).Name, GetType(Long).Name, _
                                                 GetType(Decimal).Name, GetType(Single).Name, GetType(Long).Name, _
                                                 "Int32", "Int64" : _sql = _sql.Replace("@p" & col.Ordinal, rw.Item(col.ColumnName).ToString)
                                            Case GetType(Boolean).Name : _sql = _sql.Replace("@p" & col.Ordinal, IIf(CBool(rw.Item(col.ColumnName)), 1, 0))
                                            Case Else
                                        End Select
                                    Else : _sql = _sql.Replace("@p" & col.Ordinal, "NULL")
                                    End If
                                Next

                                If Not _sql.RLTrim.EndsWith(";") Then _sql &= ";"
                                _query &= IIf(Not String.IsNullOrEmpty(_query.RLTrim), vbNewLine, String.Empty) & _sql

                            Case DataRowState.Detached, DataRowState.Deleted
                                Try
                                    Dim _sql As String = SqlStatements.UpdateStatement
                                    For i As Integer = 0 To table.Columns.Count - 1
                                        Dim col As DataColumn = table.Columns((table.Columns.Count - 1) - i)
                                        If Not Common.Simple.IsNullOrNothing(rw.Item(col.ColumnName, DataRowVersion.Original)) Then
                                            Select Case col.DataType.Name
                                                Case GetType(String).Name : _sql = _sql.Replace("@p" & col.Ordinal, "'" & rw.Item(col.ColumnName, DataRowVersion.Original).ToString.ToSqlValidString & "'")
                                                Case GetType(Date).Name : _sql = _sql.Replace("@p" & col.Ordinal, "'" & CDate(rw.Item(col.ColumnName, DataRowVersion.Original)).ToSqlValidString(True) & "'")
                                                Case GetType(SByte).Name, GetType(Byte).Name, _
                                                     GetType(Integer).Name, GetType(Long).Name, _
                                                     GetType(Decimal).Name, GetType(Single).Name, GetType(Long).Name, _
                                                     "Int32", "Int64" : _sql = _sql.Replace("@p" & col.Ordinal, rw.Item(col.ColumnName, DataRowVersion.Original).ToString)
                                                Case GetType(Boolean).Name : _sql = _sql.Replace("@p" & col.Ordinal, IIf(CBool(rw.Item(col.ColumnName, DataRowVersion.Original)), 1, 0))
                                                Case Else
                                            End Select
                                        Else : _sql = _sql.Replace("@p" & col.Ordinal, "NULL")
                                        End If
                                    Next

                                    If Not _sql.RLTrim.EndsWith(";") Then _sql &= ";"
                                    _query &= IIf(Not String.IsNullOrEmpty(_query.RLTrim), vbNewLine, String.Empty) & _sql
                                Catch ex As Exception
                                End Try

                            Case Else
                        End Select
                    Next

                    If Not String.IsNullOrEmpty(_query.RLTrim) Then
                        Dim _cmd As New MySqlCommand(_query, Connection, connectiontrans)

                        Try
                            _cmd.ExecuteNonQuery() : _saved = True
                        Catch ex As Exception
                            args.Exception = ex
                        Finally : _cmd.Dispose()
                        End Try
                    End If
                End If
            End If

            Return _saved
        End Function
#End Region

#Region "Methods"
        ''' <summary>
        ''' Attach row validating handlers into the ViewTable making the changes on the ViewTable to take effect in the BaseTable.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AttachViewTableHandlers()
            If ViewTable IsNot Nothing Then
                AddHandler _viewtable.RowChanged, AddressOf ViewTable_RowChanged
                AddHandler _viewtable.RowDeleting, AddressOf ViewTable_RowDeleting
            End If
        End Sub

        ''' <summary>
        ''' Binds the specified control's DataSource property with the current database session's ViewTable.
        ''' </summary>
        ''' <param name="control">Binded control.</param>
        ''' <remarks></remarks>
        Public Overloads Sub Bind(ByVal control As Object)
            If control IsNot Nothing Then
                Dim exists As Boolean = False

                For Each _binding As SessionDataBinding In _databinding
                    If _binding.Control Is control Then
                        exists = True : Exit For
                    End If
                Next

                If Not exists Then _databinding.Add(New SessionDataBinding(Me, control))
            End If
        End Sub

        ''' <summary>
        ''' Binds the specified control's value with a particular database session ViewTable field. Specifying a blank field name will have the whole ViewTable binded into the control (if it has an available DataSource property).
        ''' </summary>
        ''' <param name="control">Binded control.</param>
        ''' <param name="fieldname">ViewTable / BaseTable field name</param>
        ''' <remarks></remarks>
        Public Overloads Sub Bind(ByVal control As Object, ByVal fieldname As String)
            If control IsNot Nothing Then
                Dim exists As Boolean = False

                For Each _binding As SessionDataBinding In _databinding
                    If _binding.Control Is control Then
                        exists = True : Exit For
                    End If
                Next

                If Not exists Then _databinding.Add(New SessionDataBinding(Me, control, fieldname))
            End If
        End Sub

        Private Overloads Sub ClearResources()
            ClearResources(True)
        End Sub

        Private Overloads Sub ClearResources(ByVal disposeconnnection As Boolean)
            If _basetable IsNot Nothing Then
                Try
                    _basetable.Dispose()
                Catch ex As Exception
                Finally : _basetable = Nothing
                End Try
            End If

            If _viewtable IsNot Nothing Then
                Try
                    _viewtable.Dispose()
                Catch ex As Exception
                Finally : _viewtable = Nothing
                End Try
            End If

            If _adapter IsNot Nothing Then
                Try
                    _adapter.Dispose()
                Catch ex As Exception
                End Try
            End If

            If disposeconnnection Then
                If _connection IsNot Nothing Then
                    Try
                        If _connection.State = ConnectionState.Open Then _connection.Close()
                        _connection.Dispose()
                    Catch ex As Exception
                    Finally : _connection = Nothing
                    End Try
                End If
            End If
        End Sub

        Private Sub ConfigureTable(ByRef table As DataTable)
            If table IsNot Nothing Then
                Dim _withunsigned As Boolean = False

                For Each col As DataColumn In table.Columns
                    _withunsigned = _withunsigned Or col.DataType.Name.ToLower.Contains("uint")
                    If _withunsigned Then Exit For
                Next

                If _withunsigned Then
                    Dim temptable As DataTable = table.Clone

                    For Each col As DataColumn In temptable.Columns
                        If col.DataType.Name.ToLower.Contains("uint") Then col.DataType = GetType(Long)
                    Next

                    temptable.Load(table.CreateDataReader) : table.Dispose()
                    table = Nothing : table = temptable
                End If

                Dim pk As String = PrimaryKey

                If String.IsNullOrEmpty(pk.RLTrim) Then : pk = table.Columns(0).ColumnName
                Else
                    If Not table.Columns.Contains(pk) Then pk = table.Columns(0).ColumnName
                End If

                Select Case table.Columns(pk).DataType.Name
                    Case GetType(Long).Name, GetType(Integer).Name, _
                        GetType(SByte).Name, GetType(Byte).Name
                        table.Columns(pk).AutoIncrement = True

                        Dim max As Object = table.Compute("MAX([" & pk & "])", String.Empty)
                        If Not IsNumeric(max) Then : max = 0
                        Else : max = CLng(max) - 1
                        End If

                        table.Columns(pk).AutoIncrementSeed = CLng(max) + 1
                        table.Columns(pk).AutoIncrementStep = 1
                    Case Else
                End Select

                For Each col As DataColumn In table.Columns
                    If Not col.AutoIncrement Then
                        Select Case col.DataType.Name
                            Case GetType(String).Name : col.DefaultValue = String.Empty
                            Case GetType(Date).Name : col.DefaultValue = #1/1/1900#
                            Case GetType(Integer).Name, GetType(Long).Name, _
                                 GetType(SByte).Name, GetType(Byte).Name, _
                                 GetType(Decimal).Name, GetType(Single).Name, GetType(Double).Name : col.DefaultValue = 0
                            Case Else
                        End Select
                    End If
                Next

                AttachViewTableHandlers()
            End If
        End Sub

        ''' <summary>
        ''' Ends the asynchronous call of the initialized BeginLoad or BaginLoadAll methods.
        ''' </summary>
        ''' <param name="initbeginload"></param>
        ''' <remarks></remarks>
        Public Sub EndLoad(ByVal initbeginload As IAsyncResult)
            If _currentloaddelegate IsNot Nothing Then
                _currentloaddelegate.EndInvoke(initbeginload)
                _currentloaddelegate = Nothing
            End If
        End Sub

        ''' <summary>
        ''' Ends the asynchronous call of the initialized BeginSave method.
        ''' </summary>
        ''' <param name="initibeginsave"></param>
        ''' <remarks></remarks>
        Public Sub EndSave(ByVal initibeginsave As IAsyncResult)
            If _currentsavedelegate IsNot Nothing Then
                _currentsavedelegate.EndInvoke(initibeginsave)
                _currentsavedelegate = Nothing
            End If
        End Sub

        ''' <summary>
        ''' Loads data into the current database session's tables.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overloads Sub Load()
            Dim _args As New SessionEventArgs(Me)
            RaiseEvent BeforeDataLoading(Me, _args) : _currenteventargument = _args
            Load(SessionTables.BaseTable) : Load(SessionTables.ViewTable)

            For Each _binding As SessionDataBinding In _databinding
                If _binding.Control IsNot Nothing Then
                    If String.IsNullOrEmpty(_binding.Field.RLTrim) Then
                        Dim dtsource As PropertyInfo = _binding.Control.GetType().GetProperty("DataSource")
                        If dtsource IsNot Nothing Then
                            Try
                                dtsource.SetValue(_binding.Control, ViewTable, Nothing)
                            Catch ex As Exception
                            End Try
                        End If
                    Else
                        If ViewTable IsNot Nothing Then
                            If ViewTable.Columns.Contains(_binding.Field) Then
                                If ViewTable.Columns(_binding.Field).DataType.Name = GetType(String).Name And _
                                   ViewTable.Columns(_binding.Field).MaxLength > 0 Then
                                    Dim maxlength As PropertyInfo = _binding.Control.GetType().GetProperty("MaxLength")
                                    If maxlength IsNot Nothing Then
                                        Try
                                            maxlength.SetValue(_binding.Control, ViewTable.Columns(_binding.Field).MaxLength, Nothing)
                                        Catch ex As Exception
                                        End Try
                                    End If
                                End If

                                If ViewTable.Rows.Count > 0 Then
                                    Dim rw As DataRow = ViewTable.Rows(0)

                                    Dim dtsource As PropertyInfo = _binding.Control.GetType().GetProperty("DataSource")
                                    If dtsource IsNot Nothing Then
                                        Dim selval As PropertyInfo = _binding.Control.GetType().GetProperty("SelectedValue")
                                        If selval IsNot Nothing Then
                                            Try
                                                selval.SetValue(_binding.Control, rw.Item(_binding.Field), Nothing)
                                            Catch ex As Exception
                                            End Try
                                        End If
                                    Else
                                        If IsNumeric(rw.Item(_binding.Field)) Then
                                            Dim selindex As PropertyInfo = _binding.Control.GetType().GetProperty("SelectedIndex")
                                            If selindex IsNot Nothing Then
                                                Try
                                                    selindex.SetValue(_binding.Control, rw.Item(_binding.Field), Nothing)
                                                Catch ex As Exception
                                                End Try
                                            Else
                                                Dim value As PropertyInfo = _binding.Control.GetType().GetProperty("Value")
                                                If value IsNot Nothing Then
                                                    Try
                                                        value.SetValue(_binding.Control, rw.Item(_binding.Field), Nothing)
                                                    Catch ex As Exception
                                                    End Try
                                                Else
                                                    Dim text As PropertyInfo = _binding.Control.GetType().GetProperty("Text")
                                                    If text IsNot Nothing Then
                                                        Try
                                                            text.SetValue(_binding.Control, rw.Item(_binding.Field), Nothing)
                                                        Catch ex As Exception
                                                        End Try
                                                    End If
                                                End If
                                            End If
                                        Else
                                            If IsDate(rw.Item(_binding.Field)) Then
                                                Dim value As PropertyInfo = _binding.Control.GetType().GetProperty("Value")
                                                If value IsNot Nothing Then
                                                    Try
                                                        value.SetValue(_binding.Control, rw.Item(_binding.Field), Nothing)
                                                    Catch ex As Exception
                                                    End Try
                                                Else
                                                    Dim text As PropertyInfo = _binding.Control.GetType().GetProperty("Text")
                                                    If text IsNot Nothing Then
                                                        Try
                                                            text.SetValue(_binding.Control, rw.Item(_binding.Field), Nothing)
                                                        Catch ex As Exception
                                                        End Try
                                                    End If
                                                End If
                                            Else
                                                If Not Common.Simple.IsNullOrNothing(rw.Item(_binding.Field)) Then
                                                    If ViewTable.Columns(_binding.Field).DataType.Name.ToLower.Contains("byte[]") Or _
                                                       ViewTable.Columns(_binding.Field).DataType.Name.ToLower.Contains("bytes[]") Or _
                                                       ViewTable.Columns(_binding.Field).DataType.Name.ToLower.Contains("byte()") Or _
                                                       ViewTable.Columns(_binding.Field).DataType.Name.ToLower.Contains("bytes()") Then
                                                        Dim image As PropertyInfo = _binding.Control.GetType().GetProperty("Image")
                                                        If image IsNot Nothing Then
                                                            Try
                                                                image.SetValue(_binding.Control, CType(rw.Item(_binding.Field), Byte()).ToImage, Nothing)
                                                            Catch ex As Exception
                                                            End Try
                                                        End If
                                                    Else
                                                        Dim text As PropertyInfo = _binding.Control.GetType().GetProperty("Text")
                                                        If text IsNot Nothing Then
                                                            Try
                                                                text.SetValue(_binding.Control, rw.Item(_binding.Field), Nothing)
                                                            Catch ex As Exception
                                                            End Try
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next

            RaiseEvent AfterDataLoading(Me, _args) : _currenteventargument = Nothing
        End Sub

        Private Overloads Sub Load(ByVal table As SessionTables)
            Dim query As String = String.Empty
            Select Case table
                Case SessionTables.BaseTable : query = BaseCommandText
                Case SessionTables.ViewTable : query = IIf(String.IsNullOrEmpty(ViewCommandText.RLTrim), BaseCommandText, ViewCommandText)
                Case Else
            End Select

            Select Case table
                Case SessionTables.BaseTable
                    ClearResources(CBool(Header Is Nothing))

                    If Header IsNot Nothing Then _connection = Header.Connection
                    If _connection Is Nothing Then _connection = New MySqlConnection(ConnectionString)

                    If _connection.State <> ConnectionState.Open Then _connection.Open()
                    _adapter = New MySqlDataAdapter(query, _connection)

                    _basetable = New DataTable

                    Try
                        _adapter.FillSchema(_basetable, SchemaType.Mapped)
                    Catch ex As Exception
                    End Try

                    Try
                        _adapter.Fill(_basetable)
                    Catch ex As Exception
                        If _currenteventargument IsNot Nothing Then _currenteventargument.Exception = ex
                    End Try

                    ConfigureTable(_basetable)
                    SqlStatements.InitializeStatements()

                Case SessionTables.ViewTable
                    If String.IsNullOrEmpty(ViewCommandText.RLTrim) And _
                      _basetable IsNot Nothing Then
                        If _viewtable IsNot Nothing Then
                            Try
                                _viewtable.Dispose()
                            Catch ex As Exception
                            Finally : _viewtable = Nothing
                            End Try
                        End If

                        _viewtable = _basetable.Clone : _viewtable.Load(_basetable.CreateDataReader)
                    Else
                        _viewtable.LoadData(ConnectionString, query)
                        If ViewTable IsNot Nothing And _
                            BaseTable IsNot Nothing Then ViewTable.TableName = BaseTable.TableName
                        ConfigureTable(_viewtable)
                    End If
              
                Case Else
            End Select
        End Sub

        ''' <summary>
        ''' Loads data into the current database session's tables and into its associated detail database sessions.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub LoadAll()
            Load()

            For Each _session As Session In Details
                _session.LoadAll()
            Next
        End Sub

        ''' <summary>
        ''' Removes the row validating handlers from the ViewTable.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RemoveViewTableHandler()
            If ViewTable IsNot Nothing Then
                RemoveHandler _viewtable.RowChanged, AddressOf ViewTable_RowChanged
                RemoveHandler _viewtable.RowDeleting, AddressOf ViewTable_RowDeleting
            End If
        End Sub

        ''' <summary>
        ''' Saves the changes made from the current database session and its dependent detail sessions into the connected database tables.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Save()
            For Each _binding As SessionDataBinding In _databinding
                If Not String.IsNullOrEmpty(_binding.Field.RLTrim) Then
                    If ViewTable.Columns.Contains(_binding.Field) And _
                       _binding.Control IsNot Nothing Then
                        Dim rw As DataRow = Nothing

                        If ViewTable.Rows.Count > 0 Then : rw = ViewTable.Rows(0)
                        Else : rw = ViewTable.Rows.Add
                        End If

                        Dim dtsource As PropertyInfo = _binding.Control.GetType().GetProperty("DataSource")
                        If dtsource IsNot Nothing Then
                            Dim selval As PropertyInfo = _binding.Control.GetType().GetProperty("SelectedValue")
                            If selval IsNot Nothing Then
                                Try
                                    rw.Item(_binding.Field) = selval.GetValue(_binding.Control, Nothing)
                                Catch ex As Exception
                                End Try
                            End If
                        Else
                            Select Case ViewTable.Columns(_binding.Field).DataType.Name
                                Case GetType(Integer).Name, GetType(Long).Name, _
                                     GetType(SByte).Name, GetType(Byte).Name, _
                                     GetType(Decimal).Name, GetType(Single).Name, GetType(Double).Name
                                    Dim selindex As PropertyInfo = _binding.Control.GetType().GetProperty("SelectedIndex")
                                    If selindex IsNot Nothing Then
                                        Try
                                            rw.Item(_binding.Field) = selindex.GetValue(_binding.Control, Nothing)
                                        Catch ex As Exception
                                        End Try
                                    Else
                                        Dim value As PropertyInfo = _binding.Control.GetType().GetProperty("Value")
                                        If value IsNot Nothing Then
                                            Try
                                                rw.Item(_binding.Field) = value.GetValue(_binding.Control, Nothing)
                                            Catch ex As Exception
                                            End Try
                                        Else
                                            Dim text As PropertyInfo = _binding.Control.GetType().GetProperty("Text")
                                            If text IsNot Nothing Then
                                                Try
                                                    rw.Item(_binding.Field) = text.GetValue(_binding.Control, Nothing)
                                                Catch ex As Exception
                                                End Try
                                            End If
                                        End If
                                    End If

                                Case GetType(Date).Name, GetType(String).Name
                                    Dim value As PropertyInfo = _binding.Control.GetType().GetProperty("Value")
                                    If value IsNot Nothing Then
                                        Try
                                            rw.Item(_binding.Field) = value.GetValue(_binding.Control, Nothing)
                                        Catch ex As Exception
                                        End Try
                                    Else
                                        Dim text As PropertyInfo = _binding.Control.GetType().GetProperty("Text")
                                        If text IsNot Nothing Then
                                            Try
                                                rw.Item(_binding.Field) = text.GetValue(_binding.Control, Nothing)
                                            Catch ex As Exception
                                            End Try
                                        End If
                                    End If

                                Case Else
                                    If ViewTable.Columns(_binding.Field).DataType.Name.ToLower.Contains("byte[]") Or _
                                       ViewTable.Columns(_binding.Field).DataType.Name.ToLower.Contains("bytes[]") Or _
                                       ViewTable.Columns(_binding.Field).DataType.Name.ToLower.Contains("byte()") Or _
                                       ViewTable.Columns(_binding.Field).DataType.Name.ToLower.Contains("bytes()") Then
                                        Dim image As PropertyInfo = _binding.Control.GetType().GetProperty("Image")
                                        If image IsNot Nothing Then
                                            Try
                                                rw.Item(_binding.Field) = CType(image.GetValue(_binding.Control, Nothing), Image).ToByteArray
                                            Catch ex As Exception
                                            End Try
                                        End If
                                    End If

                            End Select
                        End If
                    End If
                End If
            Next

            If Header IsNot Nothing Then
                Dim fk As String = ForeignKey : Dim fkval As Object = Nothing

                If Not String.IsNullOrEmpty(fk.RLTrim) Then
                    If Header.BaseTable IsNot Nothing Then
                        Dim hpk As String = Header.PrimaryKey
                        If Not String.IsNullOrEmpty(hpk.RLTrim) Then
                            If Header.BaseTable.Columns.Contains(hpk) Then
                                If Header.BaseTable.Rows.Count = 1 Then fkval = Header.BaseTable.Rows(0).Item(hpk)
                            End If
                        End If
                    End If
                End If

                If Not String.IsNullOrEmpty(fk.RLTrim) And _
                   Not Common.Simple.IsNullOrNothing(fkval) Then
                    RemoveViewTableHandler()

                    For Each rw As DataRow In ViewTable.Rows
                        If rw.RowState <> DataRowState.Deleted And _
                           rw.RowState <> DataRowState.Detached Then rw.Item(fk) = fkval
                    Next

                    For Each rw As DataRow In BaseTable.Rows
                        If rw.RowState <> DataRowState.Deleted And _
                           rw.RowState <> DataRowState.Detached Then rw.Item(fk) = fkval
                    Next

                    AttachViewTableHandlers()
                End If
            End If

            If _currenteventargument IsNot Nothing Then _currenteventargument = Nothing
            _currenteventargument = New SessionEventArgs(Me) : RaiseEvent BeforeUpdate(Me, CurrentEventArgument)
            If Header IsNot Nothing Then
                If Header.CurrentEventArgument IsNot Nothing Then Header.CurrentEventArgument.Cancel = CurrentEventArgument.Cancel
            End If

            If Not CurrentEventArgument.Cancel Then
                If Header Is Nothing Then : _transaction = Connection.BeginTransaction
                Else
                    Dim fk As String = ForeignKey : Dim fkval As Object = Nothing

                    If Not String.IsNullOrEmpty(fk.RLTrim) Then
                        If Header.BaseTable IsNot Nothing Then
                            Dim hpk As String = Header.PrimaryKey
                            If Not String.IsNullOrEmpty(hpk.RLTrim) Then
                                If Header.BaseTable.Columns.Contains(hpk) Then
                                    If Header.BaseTable.Rows.Count = 1 Then fkval = Header.BaseTable.Rows(0).Item(hpk)
                                End If
                            End If
                        End If
                    End If

                    If Not String.IsNullOrEmpty(fk.RLTrim) And _
                       Not Common.Simple.IsNullOrNothing(fkval) Then
                        RemoveViewTableHandler()

                        For Each rw As DataRow In ViewTable.Rows
                            If rw.RowState <> DataRowState.Deleted And _
                               rw.RowState <> DataRowState.Detached Then rw.Item(fk) = fkval
                        Next

                        For Each rw As DataRow In BaseTable.Rows
                            If rw.RowState <> DataRowState.Deleted And _
                               rw.RowState <> DataRowState.Detached Then rw.Item(fk) = fkval
                        Next

                        AttachViewTableHandlers()
                    End If

                    If Header.Transaction IsNot Nothing Then : _transaction = Header.Transaction
                    Else : _transaction = Connection.BeginTransaction
                    End If
                End If

                If Adapter.InsertCommand IsNot Nothing Then
                    _adapter.InsertCommand.Transaction = Transaction
                    _adapter.InsertCommand.CommandText = SqlStatements.InsertStatement
                End If

                If Adapter.UpdateCommand IsNot Nothing Then
                    _adapter.UpdateCommand.Transaction = Transaction
                    _adapter.UpdateCommand.CommandText = SqlStatements.UpdateStatement
                End If

                If Adapter.DeleteCommand IsNot Nothing Then
                    _adapter.DeleteCommand.Transaction = Transaction
                    _adapter.DeleteCommand.CommandText = SqlStatements.DeleteStatement
                End If

                CurrentEventArgument.GetChanges()

                If CurrentEventArgument.Changes IsNot Nothing Then
                    If CurrentEventArgument.Changes.Rows.Count > 0 Then
                        Dim b As Boolean = SaveManually(CurrentEventArgument, Transaction)
                        If b Then
                            RaiseEvent UpdateSucceed(Me, CurrentEventArgument)
                            BaseTable.AcceptChanges() : SqlStatements.InitializeStatements()
                        Else
                            Try
                                If Transaction IsNot Nothing Then Transaction.Rollback()
                            Catch ex2 As Exception
                            End Try
                            CurrentEventArgument.Cancel = True
                            RaiseEvent UpdateFailed(Me, CurrentEventArgument)
                        End If
                    End If
                End If

                If Header IsNot Nothing Then
                    If Header.CurrentEventArgument IsNot Nothing Then Header.CurrentEventArgument.Cancel = CurrentEventArgument.Cancel
                End If

                If Not CurrentEventArgument.Cancel Then
                    RaiseEvent AfterUpdate(Me, CurrentEventArgument)

                    If Header IsNot Nothing Then
                        If Header.CurrentEventArgument IsNot Nothing Then Header.CurrentEventArgument.Cancel = CurrentEventArgument.Cancel
                    End If

                    If Not CurrentEventArgument.Cancel Then
                        For Each _session As Session In Details
                            _session.Save()
                        Next
                    End If

                    If Not CurrentEventArgument.Cancel Then
                        If Header Is Nothing Then
                            RaiseEvent UpdateFinalizing(Me, CurrentEventArgument)

                            If Transaction IsNot Nothing Then
                                Try
                                    Transaction.Commit()
                                Catch ex As Exception
                                    CurrentEventArgument.Exception = ex
                                End Try
                            End If

                            RaiseEvent UpdateFinalized(Me, CurrentEventArgument)
                            _currenteventargument = Nothing
                        End If
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Unbinds the current control into the current database session's ViewTable and any of its fields.
        ''' </summary>
        ''' <param name="control"></param>
        ''' <remarks></remarks>
        Public Sub Unbind(ByVal control As Object)
            For Each _binding As SessionDataBinding In _databinding
                If _binding.Control Is control Then
                    _databinding.Remove(_binding) : Exit For
                End If
            Next
        End Sub

#End Region

#Region "IDisposable Support"

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
                    ClearResources(CBool(Header Is Nothing))
                End If

                ' TODO: free your own state (unmanaged objects).
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

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

    ''' <summary>
    ''' Collection of database detail session.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SessionCollection
        Inherits CollectionBase

#Region "Properties"

        ''' <summary>
        ''' Gets the database session in the specified index of the collection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Overloads ReadOnly Property Items(ByVal index As Integer) As Session
            Get
                Return TryCast(List(index), Session)
            End Get
        End Property

        ''' <summary>
        ''' Gets the a database session with the specified BaseTable table name from the collection.
        ''' </summary>
        ''' <param name="tablename"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Overloads ReadOnly Property Items(ByVal tablename As String) As Session
            Get
                Return GetSessionByTableName(tablename)
            End Get
        End Property

        Private _parent As Session = Nothing

        ''' <summary>
        ''' Gets the current header session for the current collection of database session.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Parent() As Session
            Get
                Return _parent
            End Get
        End Property

#End Region

#Region "New Instance"
        ''' <summary>
        ''' Creates a new instance of SessionCollection.
        ''' </summary>
        ''' <param name="header">Header database session.</param>
        ''' <remarks></remarks>
        Sub New(ByVal header As Session)
            _parent = header
        End Sub

#End Region

#Region "Functions"
        ''' <summary>
        ''' Adds a new detail database session into the collection.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Add() As Session
            Return Add(String.Empty)
        End Function

        ''' <summary>
        ''' Adds a new detail database session into the collection.
        ''' </summary>
        ''' <param name="basecommandtext">Base table commandtext.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal basecommandtext As String) As Session
            Return Add(basecommandtext, basecommandtext)
        End Function

        ''' <summary>
        ''' Adds a new detail database session into the collection.
        ''' </summary>
        ''' <param name="basecommandtext">Base table commandtext.</param>
        ''' <param name="viewcommandtext">View table commandtext.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal basecommandtext As String, ByVal viewcommandtext As String) As Session
            Dim _session As Session = Nothing
            If Parent IsNot Nothing Then
                _session = New Session(Parent, Parent.ConnectionString, basecommandtext, viewcommandtext)
                _session.ForeignKey = Parent.PrimaryKey
            End If
            Dim index As Integer = List.Add(_session) : Return TryCast(List(index), Session)
        End Function

        ''' <summary>
        ''' Returns whether the current session already exists in the collection.
        ''' </summary>
        ''' <param name="session"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Contains(ByVal session As Session) As Boolean
            Return List.Contains(session)
        End Function

        ''' <summary>
        ''' Returns whether a database session with the specified BaseTable name already exists in the collection.
        ''' </summary>
        ''' <param name="tablename"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Contains(ByVal tablename As String) As Boolean
            Return (GetSessionByTableName(tablename) IsNot Nothing)
        End Function

        Private Function GetSessionByTableName(ByVal tablename As String) As Session
            Dim _session As Session = Nothing

            For Each s As Session In List
                If s.BaseTable IsNot Nothing Then
                    If s.BaseTable.TableName = tablename Then
                        _session = s : Exit For
                    End If
                End If
            Next

            Return _session
        End Function

#End Region

#Region "Methods"
        ''' <summary>
        ''' Removes the specified database session from the collection.
        ''' </summary>
        ''' <param name="session"></param>
        ''' <remarks></remarks>
        Public Overloads Sub Remove(ByVal session As Session)
            If Contains(session) Then List.Remove(session)
        End Sub

        ''' <summary>
        ''' Removes a particular database session with the specified BaseTable name from the collection.
        ''' </summary>
        ''' <param name="tablename"></param>
        ''' <remarks></remarks>
        Public Overloads Sub Remove(ByVal tablename As String)
            Dim _session As Session = GetSessionByTableName(tablename)
            If _session IsNot Nothing Then List.Remove(_session)
        End Sub
#End Region

    End Class

    ''' <summary>
    ''' Database session binding information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SessionDataBinding

#Region "Properties"
        Private _contol As Object = Nothing

        ''' <summary>
        ''' Gets the object to where the current associated database session's ViewTable or ViewTable field value is binded into.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Control() As Object
            Get
                Return _contol
            End Get
        End Property

        Private _field As String = String.Empty

        ''' <summary>
        ''' Gets the field name to where the binding takes effect. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Field() As String
            Get
                Return _field
            End Get
        End Property


        Private _session As Session = Nothing

        ''' <summary>
        ''' Gets the current associated database session.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Session() As Session
            Get
                Return _session
            End Get
        End Property

#End Region

#Region "New Instance"
        ''' <summary>
        ''' Creates a new instance of SessionDataBinding.
        ''' </summary>
        ''' <param name="owner">Associated database session.</param>
        ''' <param name="bindedcontrol">Binded control</param>
        ''' <remarks></remarks>
        Sub New(ByVal owner As Session, ByVal bindedcontrol As Object)
            _session = owner : _contol = bindedcontrol
        End Sub

        ''' <summary>
        ''' Creates a new instance of SessionDataBinding.
        ''' </summary>
        ''' <param name="owner">Associated database session.</param>
        ''' <param name="bindedcontrol">Binded control</param>
        ''' <param name="bindedfield">Binded database session ViewTable field name.</param>
        ''' <remarks></remarks>
        Sub New(ByVal owner As Session, ByVal bindedcontrol As Object, ByVal bindedfield As String)
            _session = owner : _contol = bindedcontrol : _field = bindedfield
        End Sub
#End Region

    End Class

    ''' <summary>
    ''' Database session data loading and saving event arguments.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SessionEventArgs

        Private _cancel As Boolean = False

        ''' <summary>
        ''' Gets or sets whether to cancel the preceeding events after calling the current event that returns this argument.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cancel() As Boolean
            Get
                Return _cancel
            End Get
            Set(ByVal value As Boolean)
                _cancel = value
            End Set
        End Property

        Private _changes As DataTable = Nothing

        ''' <summary>
        ''' Gets the gathered changes into the current database session's BaseTable.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Changes() As DataTable
            Get
                Return _changes
            End Get
        End Property

        Private _exception As Exception = Nothing

        ''' <summary>
        ''' Gets or sets the returning exception indicating a error occurance while loading and / or saving data.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Exception() As Exception
            Get
                Return _exception
            End Get
            Set(ByVal value As Exception)
                _exception = value
            End Set
        End Property

        Private _loaded As Boolean = False

        ''' <summary>
        ''' Gets whether the current calling database session's data has been successully loaded or not.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Loaded() As Boolean
            Get
                If Session IsNot Nothing Then _loaded = (Session.BaseTable IsNot Nothing And _
                                                         Session.ViewTable IsNot Nothing)

                Return _loaded
            End Get
        End Property

        Private _session As Session = Nothing

        ''' <summary>
        ''' Gets the current evaluated / calling database session object.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Session() As Session
            Get
                Return _session
            End Get
        End Property

        ''' <summary>
        ''' Creates a new instance of SessionEventArgs.
        ''' </summary>
        ''' <param name="callingsession"></param>
        ''' <remarks></remarks>
        Sub New(ByVal callingsession As Session)
            _session = callingsession
        End Sub

        ''' <summary>
        ''' Loads all the changes made from the current database session into the Changes table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub GetChanges()
            If Session IsNot Nothing Then
                If Session.BaseTable IsNot Nothing Then
                    If _changes IsNot Nothing Then
                        Try
                            _changes.Dispose()
                        Catch ex As Exception
                        Finally : _changes = Nothing
                        End Try
                    End If

                    _changes = Session.BaseTable.GetChanges(DataRowState.Added + DataRowState.Modified + _
                                                            DataRowState.Deleted + DataRowState.Detached)
                End If
            End If
        End Sub

    End Class

    ''' <summary>
    ''' Session statement information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SessionStatements

#Region "Properties"
        Private _deletestatement As String = String.Empty

        ''' <summary>
        ''' Gets or sets the current database session's associated delete sql statements for removed table rows.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DeleteStatement() As String
            Get
                Return _deletestatement
            End Get
            Set(ByVal value As String)
                _deletestatement = value
            End Set
        End Property

        Private _insertstatement As String = String.Empty

        ''' <summary>
        ''' Gets or sets the current database session's associated insert sql statements for newly added table rows.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InsertStatement() As String
            Get
                Return _insertstatement
            End Get
            Set(ByVal value As String)
                _insertstatement = value
            End Set
        End Property

        Private _session As Session

        ''' <summary>
        ''' Gets the parented session for this sql statements.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Session() As Session
            Get
                Return _session
            End Get
        End Property

        Private _updatestatement As String = String.Empty

        ''' <summary>
        ''' Gets or sets the current database session's associated update sql statements for changed table rows.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UpdateStatement() As String
            Get
                Return _updatestatement
            End Get
            Set(ByVal value As String)
                _updatestatement = value
            End Set
        End Property

#End Region

#Region "New Instance"
        ''' <summary>
        ''' Creates a new instance of SessionStatements.
        ''' </summary>
        ''' <param name="owner"></param>
        ''' <remarks></remarks>
        Sub New(ByVal owner As Session)
            _session = owner
        End Sub

#End Region

#Region "Methods"
        ''' <summary>
        ''' Generates suggested command statements for the current database session.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub InitializeStatements()
            DeleteStatement = String.Empty : InsertStatement = String.Empty : UpdateStatement = String.Empty

            If Session IsNot Nothing Then
                If Session.Adapter IsNot Nothing And _
                   Session.BaseTable IsNot Nothing Then

                    Dim pk As String = Session.PrimaryKey : Dim fk As String = Session.ForeignKey
                    Dim fkval As String = String.Empty

                    If String.IsNullOrEmpty(pk.RLTrim) Then
                        For Each col As DataColumn In Session.BaseTable.Columns
                            If col.Unique Then
                                pk = col.ColumnName : Exit For
                            End If
                        Next

                        If String.IsNullOrEmpty(pk.RLTrim) Then pk = Session.BaseTable.Columns(0).ColumnName
                    Else
                        If Not Session.BaseTable.Columns.Contains(pk) Then
                            pk = String.Empty

                            For Each col As DataColumn In Session.BaseTable.Columns
                                If col.Unique Then
                                    pk = col.ColumnName : Exit For
                                End If
                            Next

                            If String.IsNullOrEmpty(pk.RLTrim) Then pk = Session.BaseTable.Columns(0).ColumnName
                        End If
                    End If

                    Session.PrimaryKey = pk

                    If Session.Header IsNot Nothing Then
                        Dim hpk As String = Session.Header.PrimaryKey

                        If String.IsNullOrEmpty(fk.RLTrim) Then
                            If Not String.IsNullOrEmpty(hpk.RLTrim) Then
                                If Session.BaseTable.Columns.Contains(hpk) Then fk = hpk
                            End If
                        Else
                            If Not Session.BaseTable.Columns.Contains(fk) Then
                                fk = String.Empty
                                If Not String.IsNullOrEmpty(hpk.RLTrim) Then
                                    If Session.BaseTable.Columns.Contains(hpk) Then fk = hpk
                                End If
                            End If
                        End If

                        Session.ForeignKey = fk

                        If Not String.IsNullOrEmpty(hpk.RLTrim) Then
                            If Session.Header.BaseTable IsNot Nothing Then
                                If Session.Header.BaseTable.Columns.Contains(hpk) Then
                                    If Session.Header.BaseTable.Rows.Count > 0 Then
                                        Dim rw As DataRow = Session.Header.BaseTable.Rows(0)
                                        If Not Common.Simple.IsNullOrNothing(rw.Item(hpk)) Then
                                            Select Case Session.Header.BaseTable.Columns(hpk).DataType.Name
                                                Case GetType(String).Name : fkval = "'" & rw.Item(hpk).ToString.ToSqlValidString & "'"
                                                Case GetType(Date).Name : fkval = "'" & CDate(rw.Item(hpk)).ToSqlValidString(True) & "'"
                                                Case Else
                                                    If IsNumeric(rw.Item(hpk)) Then fkval = rw.Item(hpk).ToString
                                            End Select
                                        Else : fkval = "NULL"
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    Else
                        fk = String.Empty : Session.ForeignKey = String.Empty
                    End If

                    InsertStatement = "INSERT INTO `" & Session.BaseTable.TableName & "` ("
                    UpdateStatement = "UPDATE `" & Session.BaseTable.TableName & "` SET "
                    DeleteStatement = "DELETE FROM `" & Session.BaseTable.TableName & "` WHERE (`" & Session.PrimaryKey & "` = @p" & Session.BaseTable.Columns(Session.PrimaryKey).Ordinal & ")"

                    Dim _withfields As Boolean = False

                    For i As Integer = 0 To Session.BaseTable.Columns.Count - 1
                        Dim col As DataColumn = Session.BaseTable.Columns(i)
                        If Not col.AutoIncrement Then
                            InsertStatement &= IIf(Not _withfields, String.Empty, ", ") & "`" & col.ColumnName & "`"
                            UpdateStatement &= IIf(Not _withfields, String.Empty, ", ") & "`" & col.ColumnName & "` = @p" & col.Ordinal
                            _withfields = True
                        End If
                    Next

                    UpdateStatement &= " WHERE (`" & Session.PrimaryKey & "` = @p" & Session.BaseTable.Columns(Session.PrimaryKey).Ordinal & ")"
                    InsertStatement &= ") VALUES (" : _withfields = False

                    For i As Integer = 0 To Session.BaseTable.Columns.Count - 1
                        Dim col As DataColumn = Session.BaseTable.Columns(i)
                        If Not col.AutoIncrement Then
                            InsertStatement &= IIf(Not _withfields, String.Empty, ", ") & "@p" & col.Ordinal
                            _withfields = True
                        End If
                    Next

                    InsertStatement &= ")"

                    If Not String.IsNullOrEmpty(fk.RLTrim) Then
                        If String.IsNullOrEmpty(fkval.RLTrim) Then fkval = "NULL"
                        UpdateStatement &= " AND (`" & fk & "` = " & fkval & ")"
                        DeleteStatement &= " AND (`" & fk & "` = " & fkval & ")"
                    End If

                    Dim _sqlbuilder As New MySqlCommandBuilder(Session.Adapter)
                    Dim _cmdinsert As MySqlCommand = _sqlbuilder.GetInsertCommand : _cmdinsert.CommandText = InsertStatement
                    Dim _cmdupdate As MySqlCommand = _sqlbuilder.GetUpdateCommand : _cmdupdate.CommandText = UpdateStatement
                    Dim _cmddelete As MySqlCommand = _sqlbuilder.GetDeleteCommand : _cmddelete.CommandText = DeleteStatement

                    If Session.Adapter.InsertCommand Is Nothing Then : Session.Adapter.InsertCommand = _cmdinsert
                    Else : Session.Adapter.InsertCommand.CommandText = InsertStatement
                    End If

                    If Session.Adapter.UpdateCommand Is Nothing Then : Session.Adapter.UpdateCommand = _cmdupdate
                    Else : Session.Adapter.UpdateCommand.CommandText = UpdateStatement
                    End If

                    If Session.Adapter.DeleteCommand Is Nothing Then : Session.Adapter.DeleteCommand = _cmddelete
                    Else : Session.Adapter.DeleteCommand.CommandText = DeleteStatement
                    End If
                End If
            End If
        End Sub

#End Region

    End Class

End Class
