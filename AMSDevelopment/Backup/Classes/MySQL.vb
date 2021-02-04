''' <summary>
''' Class for MySQL functionalities.
''' </summary>
''' <remarks></remarks>
Partial Public Class MySQL
    Implements IDisposable

#Region "Enumerations"
    ''' <summary>
    ''' Common MySQL connection string parameter values sections.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ConnectionDetailEnum
        ''' <summary>
        ''' DRIVER part of a connection string.
        ''' </summary>
        ''' <remarks></remarks>
        Driver = 0
        ''' <summary>
        ''' SERVER part of a connection string.
        ''' </summary>
        ''' <remarks></remarks>
        Server = 1
        ''' <summary>
        ''' DATABASE part of a connection string.
        ''' </summary>
        ''' <remarks></remarks>
        Database = 2
        ''' <summary>
        ''' UID (User Id) part of a connection string.
        ''' </summary>
        ''' <remarks></remarks>
        UID = 3
        ''' <summary>
        ''' PWD (Password) part of a connection string.
        ''' </summary>
        ''' <remarks></remarks>
        PWD = 4
        ''' <summary>
        ''' PORT (Port Number) part of a connection string.
        ''' </summary>
        ''' <remarks></remarks>
        Port = 5
        None = -1
    End Enum
#End Region

#Region "Custom Classes"
    ''' <summary>
    ''' Collection of database table names.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TableNameCollection
        Inherits Collections.CollectionBase
        Implements IDisposable

        ''' <summary>
        ''' Gets or sets item in this instance of TableNameCollection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Property Items(ByVal index As Integer) As String
            Get
                Return CType(List.Item(index), String)
            End Get
            Set(ByVal value As String)
                List.Item(index) = value
            End Set
        End Property

        ''' <summary>
        ''' Add new item in this instance of TableNameCollection.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Add(ByVal value As String) As Integer
            Return List.Add(value)
        End Function

        ''' <summary>
        ''' Validates if specified item is existing in this instance of TableNameCollection.
        ''' </summary>
        ''' <param name="value">Value to find</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Contains(ByVal value As String) As Boolean
            Return List.Contains(value)
        End Function

        ''' <summary>
        ''' Removes an existing item in this instance of TableNameCollection.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Public Sub Remove(ByVal value As String)
            If List.Contains(value) Then List.Remove(value)
        End Sub

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
#End Region

#Region "Sub New"
    ''' <summary>
    ''' Class for connecting into a MySQL database.
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        _connectionstring = "DRIVER={MySQL ODBC 3.51 Driver};SERVER=localhost;DATABASE=mysql;UID=root;PWD=nl2009;PORT=3306;"
        _Database = "mysql" : _Driver = "MySQL ODBC 3.51 Driver"
        _Password = "nl2009" : _PortNo = 3306
        _Server = "localhost" : _UserID = "root"
    End Sub

    ''' <summary>
    ''' Class for connecting into a MySQL database.
    ''' </summary>
    ''' <param name="connection">A valid MySQL connection string.</param>
    ''' <remarks></remarks>
    Sub New(ByVal connection As String)
        _connectionstring = connection
        _Database = GetConnectionValue(ConnectionDetailEnum.Database) : _Driver = GetConnectionValue(ConnectionDetailEnum.Driver)
        _Password = GetConnectionValue(ConnectionDetailEnum.PWD) : _PortNo = GetConnectionValue(ConnectionDetailEnum.Port)
        _Server = GetConnectionValue(ConnectionDetailEnum.Server) : _UserID = GetConnectionValue(ConnectionDetailEnum.UID)
    End Sub

    ''' <summary>
    ''' Class for connecting into a MySQL database.
    ''' </summary>
    ''' <param name="server">Server IP or Hostname.</param>
    ''' <param name="database">MySQL Database name.</param>
    ''' <param name="userid">MySQL server login id.</param>
    ''' <param name="password">MySQL server login password.</param>
    ''' <remarks></remarks>
    Sub New(ByVal server As String, ByVal database As String, ByVal userid As String, ByVal password As String)
        _connectionstring = "DRIVER={MySQL ODBC 3.51 Driver};SERVER=" & server & ";DATABASE=" & database & ";UID=" & userid & ";PWD=" & password & ";PORT=3306;"
        _Database = database : _Driver = "MySQL ODBC 3.51 Driver"
        _Password = password : _PortNo = 3306
        _Server = server : _UserID = userid
    End Sub

    ''' <summary>
    ''' Class for connecting into a MySQL database.
    ''' </summary>
    ''' <param name="server">Server IP or Hostname.</param>
    ''' <param name="database">MySQL Database name.</param>
    ''' <param name="userid">MySQL server login id.</param>
    ''' <param name="password">MySQL server login password.</param>
    ''' <param name="port">MySQL connection port number.</param>
    ''' <remarks></remarks>
    Sub New(ByVal server As String, ByVal database As String, ByVal userid As String, ByVal password As String, ByVal port As Integer)
        _connectionstring = "DRIVER={MySQL ODBC 3.51 Driver};SERVER=" & server & ";DATABASE=" & database & ";UID=" & userid & ";PWD=" & password & ";PORT=" & port & ";"
        _Database = database : _Driver = "MySQL ODBC 3.51 Driver"
        _Password = password : _PortNo = port
        _Server = server : _UserID = userid
    End Sub

    ''' <summary>
    ''' Class for connecting into a MySQL database.
    ''' </summary>
    ''' <param name="server">Server IP or Hostname.</param>
    ''' <param name="database">MySQL Database name.</param>
    ''' <param name="userid">MySQL server login id.</param>
    ''' <param name="password">MySQL server login password.</param>
    ''' <param name="port">MySQL connection port number.</param>
    ''' <param name="driver">MySQL database connection driver name.</param>
    ''' <remarks></remarks>
    Sub New(ByVal server As String, ByVal database As String, ByVal userid As String, ByVal password As String, ByVal port As Integer, ByVal driver As String)
        _connectionstring = "DRIVER={" & driver & "};SERVER=" & server & ";DATABASE=" & database & ";UID=" & userid & ";PWD=" & password & ";PORT=" & port & ";"
        _Database = database : _Driver = driver
        _Password = password : _PortNo = port
        _Server = server : _UserID = userid
    End Sub
#End Region

#Region "Properties"
    Dim _connectionstring As String = "DRIVER={MySQL ODBC 3.51 Driver};SERVER=localhost;DATABASE=mysql;UID=root;PWD=nl2009;PORT=3306;"
    ''' <summary>
    ''' Gets or sets MySQL connection string.
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
            _database = GetConnectionValue(ConnectionDetailEnum.Database) : _driver = GetConnectionValue(ConnectionDetailEnum.Driver)
            _password = GetConnectionValue(ConnectionDetailEnum.PWD) : _portno = GetConnectionValue(ConnectionDetailEnum.Port)
            _server = GetConnectionValue(ConnectionDetailEnum.Server) : _userid = GetConnectionValue(ConnectionDetailEnum.UID)
        End Set
    End Property

    Dim _database As String = "mysql"
    ''' <summary>
    ''' Gets or sets MySQL database name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Database() As String
        Get
            Return _database
        End Get
        Set(ByVal value As String)
            _database = value
            _connectionstring = "DRIVER={" & Driver & "};SERVER=" & Server & ";DATABASE=" & Database & ";UID=" & UserID & ";PWD=" & Password & ";PORT=" & Port & ";"
        End Set
    End Property

    Dim _driver As String = "MySQL ODBC 3.51 Driver"
    ''' <summary>
    ''' Gets or sets MySQL server database driver.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Driver() As String
        Get
            Return _driver
        End Get
        Set(ByVal value As String)
            _driver = value
            _connectionstring = "DRIVER={" & Driver & "};SERVER=" & Server & ";DATABASE=" & Database & ";UID=" & UserID & ";PWD=" & Password & ";PORT=" & Port & ";"
        End Set
    End Property

    Dim _server As String = "localhost"
    ''' <summary>
    ''' Gets or sets MySQl server name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Server() As String
        Get
            Return _server
        End Get
        Set(ByVal value As String)
            _server = value
            _connectionstring = "DRIVER={" & Driver & "};SERVER=" & Server & ";DATABASE=" & Database & ";UID=" & UserID & ";PWD=" & Password & ";PORT=" & Port & ";"
        End Set
    End Property

    Dim _userid As String = "root"

    ''' <summary>
    ''' Gets or sets MySQL server login ID.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UserID() As String
        Get
            Return _userid
        End Get
        Set(ByVal value As String)
            _userid = value
            _connectionstring = "DRIVER={" & Driver & "};SERVER=" & Server & ";DATABASE=" & Database & ";UID=" & UserID & ";PWD=" & Password & ";PORT=" & Port & ";"
        End Set
    End Property

    Dim _password As String = "FMS2011"
    ''' <summary>
    ''' Gets or sets MySQL server login password.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Password() As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
            _connectionstring = "DRIVER={" & Driver & "};SERVER=" & Server & ";DATABASE=" & Database & ";UID=" & UserID & ";PWD=" & Password & ";PORT=" & Port & ";"
        End Set
    End Property

    Dim _portno As Integer = 3306
    ''' <summary>
    ''' Gets or sets MySQL database connection port number.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Port() As Integer
        Get
            Return _portno
        End Get
        Set(ByVal value As Integer)
            _portno = value
            _connectionstring = "DRIVER={" & Driver & "};SERVER=" & Server & ";DATABASE=" & Database & ";UID=" & UserID & ";PWD=" & Password & ";PORT=" & Port & ";"
        End Set
    End Property

    ''' <summary>
    ''' Gets whether connection can be established using supplied database connection string and / or details.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CanConnect() As Boolean
        Get
            Return TryConnect()
        End Get
    End Property

    ''' <summary>
    ''' Gets current connected server's date and time.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DateAndTime() As Date
        Get
            Return GetServerDateAndTime()
        End Get
    End Property
#End Region

#Region "Functions"
    ''' <summary>
    ''' Calls the connection validator asynchronously.
    ''' </summary>
    ''' <returns>Boolean by calling EndInvoke method.</returns>
    ''' <remarks></remarks>
    Public Function InvokeConnection() As IAsyncResult
        runningdelegate = Nothing
        Dim delConnect As New Func(Of Boolean)(AddressOf TryConnect)
        runningdelegate = delConnect
        Return delConnect.BeginInvoke(Nothing, delConnect)
    End Function

    Private Function TryConnect() As Boolean
        Dim bConnect As Boolean = False

        Dim mysqlcon As New MySqlConnection("SERVER=" & _server & ";DATABASE=" & _database & ";UID=" & _userid & ";PWD=" & _password & ";PORT=" & _portno & ";")

        Try
            With mysqlcon
                If .State = ConnectionState.Closed Then .Open()
                bConnect = (.State = ConnectionState.Open)
            End With
        Catch ex As Exception
        Finally
            With mysqlcon
                If .State = ConnectionState.Open Then .Close() : .Dispose()
            End With
        End Try

        Return bConnect
    End Function

    ''' <summary>
    ''' Calls TarsierEyes.MySQL class' CanConnect Property to verify whether a connection can be established using given connection string values.
    ''' </summary>
    ''' <param name="server">Server's IP or hostname.</param>
    ''' <param name="database">Database name.</param>
    ''' <param name="userid">Database server login id.</param>
    ''' <param name="password">Database server login password.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Connects(ByVal server As String, ByVal database As String, ByVal userid As String, ByVal password As String) As Boolean
        Dim m As New MySQL(server, database, userid, password)
        Dim value As Boolean = m.CanConnect
        m.Dispose()
        Return value
    End Function

    Private Shared runningdelegate As Object

    ''' <summary>
    ''' Calls TarsierEyes.MySQL class' CanConnect Property asynchronously to verify whether a connection can be established using given connection string values.
    ''' </summary>
    ''' <param name="server">Server's IP or hostname.</param>
    ''' <param name="database">Database name</param>
    ''' <param name="userid">Database server login id.</param>
    ''' <param name="password">Database server login password.</param>
    ''' <returns>Boolean upon calling EndInvoke method.</returns>
    ''' <remarks>Must call EndConnect to get the value.</remarks>
    Public Shared Function BeginConnect(ByVal server As String, ByVal database As String, ByVal userid As String, ByVal password As String) As IAsyncResult
        Dim m As New MySQL(server, database, userid, password)
        Dim value As IAsyncResult = m.InvokeConnection
        m.Dispose()
        Return value
    End Function

    ''' <summary>
    ''' Calls the result of the BeginConnect function to verify whether a connection can be established using given connection string values.
    ''' </summary>
    ''' <param name="ar">AsyncResult interface used to invoke the connection validator.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function EndConnect(ByVal ar As IAsyncResult) As Boolean
        If runningdelegate IsNot Nothing Then
            If TypeOf runningdelegate Is Func(Of Boolean) Then
                If ar.IsCompleted Then
                    Return CType(runningdelegate, Func(Of Boolean)).EndInvoke(ar)
                    Exit Function
                End If
            End If
        End If

        Return False
    End Function

    ''' <summary>
    ''' Gets an assigned SQL connection string parameter value.
    ''' </summary>
    ''' <param name="connection">Connection string to evaluate.</param>
    ''' <param name="detail">Detail parameter to get.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ConnectionStringValue(ByVal connection As String, ByVal detail As ConnectionDetailEnum) As String
        Return GetConnectionValue(detail, connection)
    End Function

    ''' <summary>
    ''' Gets an assigned SQL connection string parameter value.
    ''' </summary>
    ''' <param name="connection">Connection string to evaluate.</param>
    ''' <param name="detail">Detail parameter to get.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ConnectionStringValue(ByVal connection As String, ByVal detail As String) As String
        Return GetConnectionValue(detail, connection)
    End Function

    ''' <summary>
    ''' Gets the server's current date and time using the supplied connection string.
    ''' </summary>
    ''' <param name="connection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ServerDateAndTime(ByVal connection As String) As Date
        Return GetServerDateAndTime(connection)
    End Function

    Private Overloads Function GetServerDateAndTime() As Date
        Dim q As New Que(_connectionstring, "SELECT NOW() AS `Now`")
        Dim dNow As Date = Now

        With q
            .ExecuteReader()
            If .Rows > 0 Then dNow = .DataTable.Rows(0).Item("Now")
            .Dispose()
        End With

        Return dNow
    End Function

    Private Overloads Shared Function GetServerDateAndTime(ByVal connection As String) As Date
        Dim q As New Que(connection, "SELECT NOW() AS `Now`")
        Dim dNow As Date = Now

        With q
            .ExecuteReader()
            If .Rows > 0 Then dNow = .DataTable.Rows(0).Item("Now")
            .Dispose()
        End With

        Return dNow
    End Function

    Private Overloads Function GetConnectionValue(ByVal detail As ConnectionDetailEnum) As String
        Return GetConnectionValue(detail, _connectionstring)
    End Function

    Private Overloads Function GetConnectionValue(ByVal detail As String) As String
        Return GetConnectionValue(detail, _connectionstring)
    End Function

    Private Overloads Shared Function GetConnectionValue(ByVal detail As ConnectionDetailEnum, ByVal connection As String) As String
        Dim sDetail As String = String.Empty
        Dim sKeyword As String = detail.ToString.Trim.ToUpper & "="

        If connection.Trim.ToUpper.IndexOf(sKeyword) >= 0 Then
            Dim iCtr As Integer = connection.Trim.ToUpper.IndexOf(sKeyword) + sKeyword.Length
            Dim sValue As String = String.Empty

            Try
                While sValue.Trim <> ";"
                    sDetail &= connection.ToCharArray()(iCtr).ToString
                    sValue = connection.ToCharArray()(iCtr).ToString
                    iCtr += 1
                End While
            Catch ex As Exception

            End Try
        End If

        Return sDetail.Replace(";", String.Empty)
    End Function

    Private Overloads Shared Function GetConnectionValue(ByVal detail As String, ByVal connection As String) As String
        Dim sDetail As String = String.Empty
        Dim sKeyword As String = detail.ToUpper & "="

        If connection.Trim.ToUpper.IndexOf(sKeyword) >= 0 Then
            Dim iCtr As Integer = connection.Trim.ToUpper.IndexOf(sKeyword) + sKeyword.Length
            Dim sValue As String = String.Empty

            Try
                While sValue.Trim <> ";"
                    sDetail &= connection.ToCharArray()(iCtr).ToString
                    sValue = connection.ToCharArray()(iCtr).ToString
                    iCtr += 1
                End While
            Catch ex As Exception

            End Try
        End If

        Return sDetail.Replace(";", String.Empty)
    End Function

    ''' <summary>
    ''' Gets list of table names inside the connected database.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DatabaseTables() As TableNameCollection
        Dim tbl As New TableNameCollection
        tbl.Clear()

        Dim sQuery As String = "SELECT" & vbNewLine & _
                               "`tables`.TABLE_NAME AS `Table`" & vbNewLine & _
                               "FROM" & vbNewLine & _
                               "information_schema.`TABLES` AS `tables`" & vbNewLine & _
                               "WHERE" & vbNewLine & _
                               "(`tables`.TABLE_SCHEMA LIKE '" & Common.SQLStrings.ToSqlValidString(ConnectionStringValue(_connectionstring, ConnectionDetailEnum.Database)) & "') AND" & vbNewLine & _
                               "(`tables`.TABLE_COMMENT NOT LIKE 'VIEW')" & vbNewLine & _
                               "ORDER BY" & vbNewLine & _
                               "`Table`"

        Dim q As New Que(_connectionstring, sQuery)
        With q
            .ExecuteReader()
            If .Rows > 0 Then
                For iRow As Integer = 0 To .DataTable.Rows.Count - 1
                    tbl.Add(.DataTable.Rows(iRow).Item("Table").ToString.Trim)
                Next
            End If
            .Dispose()
        End With

        Return tbl
    End Function

    ''' <summary>
    ''' Gets list of table names inside the database using the given connection string.
    ''' </summary>
    ''' <param name="connection">MySQL valid connection string.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DatabaseTables(ByVal connection As String) As TableNameCollection
        Dim server As String = ConnectionStringValue(connection, ConnectionDetailEnum.Server)
        Dim database As String = ConnectionStringValue(connection, ConnectionDetailEnum.Database)
        Dim userid As String = ConnectionStringValue(connection, ConnectionDetailEnum.UID)
        Dim password As String = ConnectionStringValue(connection, ConnectionDetailEnum.PWD)

        Dim m As New MySQL(server, database, userid, password)
        Dim tbl As TableNameCollection = m.DatabaseTables
        m.Dispose()

        Return tbl
    End Function

    ''' <summary>
    ''' Gets the first instance of table name in the specified query statement.
    ''' </summary>
    ''' <param name="query">Select statement</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function GetStatementTableName(ByVal query As String) As String
        Dim table As String = String.Empty

        Dim start As Integer = query.ToLower.IndexOf("from ")
        If start >= 0 Then
            Dim letters() As Char = query.ToLower.ToCharArray

            For i As Integer = start + 5 To letters.Length - 1
                If Not Char.IsWhiteSpace(letters(i)) Then : table &= letters(i).ToString
                Else
                    If String.IsNullOrEmpty(RTrim(table.Trim)) Then : table &= RTrim(letters(i).ToString)
                    Else : Exit For
                    End If
                End If
            Next
        End If

        Return RTrim(table.Trim)
    End Function

    ''' <summary>
    ''' Gets the first instance of table name in the specified query statement.
    ''' </summary>
    ''' <param name="query">Select statement.</param>
    ''' <param name="con">Connection object.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function GetStatementTableName(ByVal query As String, ByVal con As OleDb.OleDbConnection) As String
        Dim tablename As String = String.Empty

        Try
            Dim restricts(3) As Object
            restricts(3) = "TABLE"
            Dim dtschema As DataTable = con.GetOleDbSchemaTable(OleDb.OleDbSchemaGuid.Tables, restricts)
            For Each drow As DataRow In dtschema.Rows
                If query.Trim.ToLower.Contains(drow.Item(2).ToString.Trim.ToLower) Then
                    tablename = drow.Item(2).ToString.ToLower : Exit For
                End If
            Next
        Catch ex As Exception
        End Try

        Return tablename
    End Function

    ''' <summary>
    ''' Gets the first instance of table name in the specified query statement.
    ''' </summary>
    ''' <param name="query">Select statement.</param>
    ''' <param name="con">Connection object.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function GetStatementTableName(ByVal query As String, ByVal con As MySqlConnection) As String
        Dim tablename As String = String.Empty
        Dim isopen As Boolean = (con.State = ConnectionState.Open)

        Try
            If con.State = ConnectionState.Closed Then con.Open()
            Dim restricts(0) As String
            Dim dtschema As DataTable = con.GetSchema("Tables", restricts)
            For Each drow As DataRow In dtschema.Rows
                If query.Trim.ToLower.Contains(drow.Item(2).ToString.Trim.ToLower) Then
                    tablename = drow.Item(2).ToString.ToLower : Exit For
                End If
            Next
        Catch ex As Exception
        Finally
            If Not isopen Then
                If con.State = ConnectionState.Open Then con.Close()
            End If
        End Try

        Return tablename
    End Function

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    ''' <summary>
    ''' Dispose off any resources used by the class to free up memory space.
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                Common.Simple.RefreshAndManageCurrentProcess()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

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