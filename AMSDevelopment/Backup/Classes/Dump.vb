Partial Public Class MySQL
    ''' <summary>
    ''' Class for MySQL backup and restoration using MySQL and MySQLDump applications.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Dump
        Implements IDisposable

#Region "Enumerations"
        ''' <summary>
        ''' MySQL dumping tasks.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum TaskEnum
            ''' <summary>
            ''' Backup to a dump file.
            ''' </summary>
            ''' <remarks></remarks>
            Export
            ''' <summary>
            ''' Restore a batch file.
            ''' </summary>
            ''' <remarks></remarks>
            Import
        End Enum

        ''' <summary>
        ''' Common unparameterized MySQL application command prompt parameters.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum MySQLApplicationParameterEnum
            ''' <summary>
            ''' --auto-rehash
            ''' </summary>
            ''' <remarks></remarks>
            AutoRehash = 1
            ''' <summary>
            ''' --batch
            ''' </summary>
            ''' <remarks></remarks>
            Batch = 2
            ''' <summary>
            ''' --column-names
            ''' </summary>
            ''' <remarks></remarks>
            ColumnNames = 3
            ''' <summary>
            ''' --comments
            ''' </summary>
            ''' <remarks></remarks>
            Comments = 4
            ''' <summary>
            ''' --compress
            ''' </summary>
            ''' <remarks></remarks>
            Compress = 5
            ''' <summary>
            ''' --debug-info
            ''' </summary>
            ''' <remarks></remarks>
            DebugInfo = 6
            ''' <summary>
            ''' --force
            ''' </summary>
            ''' <remarks></remarks>
            Force = 7
            ''' <summary>
            ''' --help
            ''' </summary>
            ''' <remarks></remarks>
            Help = 8
            ''' <summary>
            ''' --html
            ''' </summary>
            ''' <remarks></remarks>
            Html = 9
            ''' <summary>
            ''' --ignore-spaces
            ''' </summary>
            ''' <remarks></remarks>
            IgnoreSpaces = 10
            ''' <summary>
            ''' --line-numbers
            ''' </summary>
            ''' <remarks></remarks>
            LineNumbers = 11
            ''' <summary>
            ''' --named-commands
            ''' </summary>
            ''' <remarks></remarks>
            NamedCommands = 12
            ''' <summary>
            ''' --no-auto-rehash
            ''' </summary>
            ''' <remarks></remarks>
            NoAutoRehash = 13
            ''' <summary>
            ''' --no-beep
            ''' </summary>
            ''' <remarks></remarks>
            NoBeep = 14
            ''' <summary>
            ''' --no-named-commands
            ''' </summary>
            ''' <remarks></remarks>
            NoNamedCommands = 15
            ''' <summary>
            ''' --no-pager
            ''' </summary>
            ''' <remarks></remarks>
            NoPager = 16
            ''' <summary>
            ''' --no-tee
            ''' </summary>
            ''' <remarks></remarks>
            NoTee = 17
            ''' <summary>
            ''' --one-database
            ''' </summary>
            ''' <remarks></remarks>
            OneDatabase = 18
            ''' <summary>
            ''' --quick
            ''' </summary>
            ''' <remarks></remarks>
            Quick = 19
            ''' <summary>
            ''' --raw
            ''' </summary>
            ''' <remarks></remarks>
            Raw = 20
            ''' <summary>
            ''' --reconnect
            ''' </summary>
            ''' <remarks></remarks>
            Reconnect = 21
            ''' <summary>
            ''' --safe-updates
            ''' </summary>
            ''' <remarks></remarks>
            SafeUpdates = 22
            ''' <summary>
            ''' --secure-auth
            ''' </summary>
            ''' <remarks></remarks>
            SecureAuth = 23
            ''' <summary>
            ''' --show-warnings
            ''' </summary>
            ''' <remarks></remarks>
            ShowWarnings = 24
            ''' <summary>
            ''' --sigint-ignore
            ''' </summary>
            ''' <remarks></remarks>
            SigintIgnore = 25
            ''' <summary>
            ''' --silent
            ''' </summary>
            ''' <remarks></remarks>
            Silent = 26
            ''' <summary>
            ''' --skip-column-names
            ''' </summary>
            ''' <remarks></remarks>
            SkipColumnNames = 27
            ''' <summary>
            ''' --skip-line-numbers
            ''' </summary>
            ''' <remarks></remarks>
            SkipLineNumbers = 28
            ''' <summary>
            ''' --skip-pager
            ''' </summary>
            ''' <remarks></remarks>
            SkipPager = 29
            ''' <summary>
            ''' --table
            ''' </summary>
            ''' <remarks></remarks>
            Table = 30
            ''' <summary>
            ''' --unbuffered
            ''' </summary>
            ''' <remarks></remarks>
            Unbuffered = 31
            ''' <summary>
            ''' --verbose
            ''' </summary>
            ''' <remarks></remarks>
            Verbose = 32
            ''' <summary>
            ''' --version
            ''' </summary>
            ''' <remarks></remarks>
            Version = 33
            ''' <summary>
            ''' --vertical
            ''' </summary>
            ''' <remarks></remarks>
            Vertical = 34
            ''' <summary>
            ''' --wait
            ''' </summary>
            ''' <remarks></remarks>
            Wait = 35
        End Enum

        ''' <summary>
        ''' Common unparameterized MySQL dump command prompt parameters.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum MySQLDumpParameterEnum
            ''' <summary>
            ''' --add-drop-database
            ''' </summary>
            ''' <remarks></remarks>
            AddDropDatabase = 0
            ''' <summary>
            ''' --add-drop-table
            ''' </summary>
            ''' <remarks></remarks>
            AddDropTable = 1
            ''' <summary>
            ''' --add-locks
            ''' </summary>
            ''' <remarks></remarks>
            AddLocks = 2
            ''' <summary>
            ''' --all-databases
            ''' </summary>
            ''' <remarks></remarks>
            AllDatabases = 3
            ''' <summary>
            ''' --allow-keywords
            ''' </summary>
            ''' <remarks></remarks>
            AllowKeywords = 4
            ''' <summary>
            ''' --comments
            ''' </summary>
            ''' <remarks></remarks>
            Comments = 5
            ''' <summary>
            ''' --compact
            ''' </summary>
            ''' <remarks></remarks>
            Compact = 6
            ''' <summary>
            ''' --compress
            ''' </summary>
            ''' <remarks></remarks>
            Compress = 7
            ''' <summary>
            ''' --complete-insert
            ''' </summary>
            ''' <remarks></remarks>
            CompleteInsert = 8
            ''' <summary>
            ''' --create-options
            ''' </summary>
            ''' <remarks></remarks>
            CreateOptions = 9
            ''' <summary>
            ''' --delayed-insert
            ''' </summary>
            ''' <remarks></remarks>
            DelayedInsert = 10
            ''' <summary>
            ''' --delete-master-logs
            ''' </summary>
            ''' <remarks></remarks>
            DeleteMasterLogs = 11
            ''' <summary>
            ''' --disable-keys
            ''' </summary>
            ''' <remarks></remarks>
            DisableKeys = 12
            ''' <summary>
            ''' --dump-date
            ''' </summary>
            ''' <remarks></remarks>
            DumpDate = 13
            ''' <summary>
            ''' --extended-insert
            ''' </summary>
            ''' <remarks></remarks>
            ExtendedInsert = 14
            ''' <summary>
            ''' --flush-logs
            ''' </summary>
            ''' <remarks></remarks>
            FlushLogs = 15
            ''' <summary>
            ''' --flush-privileges
            ''' </summary>
            ''' <remarks></remarks>
            FlushPrivileges = 16
            ''' <summary>
            ''' --force
            ''' </summary>
            ''' <remarks></remarks>
            Force = 17
            ''' <summary>
            ''' --hex-blob
            ''' </summary>
            ''' <remarks></remarks>
            HexBlob = 18
            ''' <summary>
            ''' --insert-ignore
            ''' </summary>
            ''' <remarks></remarks>
            InsertIgnore = 19
            ''' <summary>
            ''' --lock-all-tables
            ''' </summary>
            ''' <remarks></remarks>
            LockAllTables = 20
            ''' <summary>
            ''' --lock-tables
            ''' </summary>
            ''' <remarks></remarks>
            LockTables = 21
            ''' <summary>
            ''' --no-auto-commit
            ''' </summary>
            ''' <remarks></remarks>
            NoAutoCommit = 22
            ''' <summary>
            ''' --no-create-db
            ''' </summary>
            ''' <remarks></remarks>
            NoCreateDb = 23
            ''' <summary>
            ''' --no-create-info
            ''' </summary>
            ''' <remarks></remarks>
            NoCreateInfo = 24
            ''' <summary>
            ''' --no-data
            ''' </summary>
            ''' <remarks></remarks>
            NoData = 25
            ''' <summary>
            ''' --no-set-names
            ''' </summary>
            ''' <remarks></remarks>
            NoSetNames = 26
            ''' <summary>
            ''' --opt
            ''' </summary>
            ''' <remarks></remarks>
            Opt = 27
            ''' <summary>
            ''' --order-by-primary
            ''' </summary>
            ''' <remarks></remarks>
            OrderByPrimary = 28
            ''' <summary>
            ''' --quick
            ''' </summary>
            ''' <remarks></remarks>
            Quick = 29
            ''' <summary>
            ''' --quote-names
            ''' </summary>
            ''' <remarks></remarks>
            QuoteNames = 30
            ''' <summary>
            ''' --routines
            ''' </summary>
            ''' <remarks></remarks>
            Routines = 31
            ''' <summary>
            ''' --set-charset
            ''' </summary>
            ''' <remarks></remarks>
            SetCharset = 32
            ''' <summary>
            ''' --single-transaction
            ''' </summary>
            ''' <remarks></remarks>
            SingleTransaction = 33
            ''' <summary>
            ''' --skip-add-drop-tables
            ''' </summary>
            ''' <remarks></remarks>
            SkipAddDropTables = 34
            ''' <summary>
            ''' --skip-add-locks
            ''' </summary>
            ''' <remarks></remarks>
            SkipAddLocks = 35
            ''' <summary>
            ''' --skip-comments
            ''' </summary>
            ''' <remarks></remarks>
            SkipComments = 36
            ''' <summary>
            ''' --skip-disable-keys
            ''' </summary>
            ''' <remarks></remarks>
            SkipDisableKeys = 37
            ''' <summary>
            ''' --skip-dump-date
            ''' </summary>
            ''' <remarks></remarks>
            SkipDumpDate = 38
            ''' <summary>
            ''' --skip-opt
            ''' </summary>
            ''' <remarks></remarks>
            SkipOpt = 39
            ''' <summary>
            ''' --skip-set-charset
            ''' </summary>
            ''' <remarks></remarks>
            SkipSetCharset = 40
            ''' <summary>
            ''' --skip-triggers
            ''' </summary>
            ''' <remarks></remarks>
            SkipTriggers = 41
            ''' <summary>
            ''' --skip-tz-utc
            ''' </summary>
            ''' <remarks></remarks>
            SkipTzUtc = 42
            ''' <summary>
            ''' --tables
            ''' </summary>
            ''' <remarks></remarks>
            Tables = 43
            ''' <summary>
            ''' --triggers
            ''' </summary>
            ''' <remarks></remarks>
            Triggers = 44
            ''' <summary>
            ''' --tz-utc
            ''' </summary>
            ''' <remarks></remarks>
            TzUtc = 45
            ''' <summary>
            ''' --verbose
            ''' </summary>
            ''' <remarks></remarks>
            Verbose = 46
            ''' <summary>
            ''' --version
            ''' </summary>
            ''' <remarks></remarks>
            Version = 47
        End Enum
#End Region

#Region "Custom Classes"
        ''' <summary>
        ''' Collection of qualified MySQL and MySQL dump parameters.
        ''' </summary>
        ''' <remarks></remarks>
        Public Class DumpParameterCollection
            Inherits Collections.CollectionBase
            Implements IDisposable

            ''' <summary>
            ''' Gets an item within this instance of DumpParameterCollection
            ''' </summary>
            ''' <param name="index"></param>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Default Public Property Items(ByVal index As Integer) As String
                Get
                    Return CType(List(index), String)
                End Get
                Set(ByVal value As String)
                    List(index) = value
                End Set
            End Property

            ''' <summary>
            ''' Adds an item in this instance of DumpParameterCollaction.
            ''' </summary>
            ''' <param name="value">Valid MySQL dump application parameter string.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Overloads Function Add(ByVal value As String) As Integer
                Return List.Add(value)
            End Function

            ''' <summary>
            ''' Adds an item in this instance of DumpParameterCollaction.
            ''' </summary>
            ''' <param name="value">MySQL dump application parameter.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Overloads Function Add(ByVal value As MySQLDumpParameterEnum) As Integer
                Return List.Add(MySQLDumpParameterString(value))
            End Function

            ''' <summary>
            '''  Adds an item in this instance of DumpParameterCollaction.
            ''' </summary>
            ''' <param name="value">MySQL application parameter.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Overloads Function Add(ByVal value As MySQLApplicationParameterEnum) As Integer
                Return List.Add(MySQLParameterString(value))
            End Function

            ''' <summary>
            ''' Validates if item is existing in this instance of DumpParameterCollection.
            ''' </summary>
            ''' <param name="value">Value to find.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Overloads Function Contains(ByVal value As String) As Boolean
                Return List.Contains(value)
            End Function

            ''' <summary>
            ''' Validates if item is existing in this instance of DumpParameterCollection.
            ''' </summary>
            ''' <param name="value">Value to find.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Overloads Function Contains(ByVal value As MySQLDumpParameterEnum) As Boolean
                Return List.Contains(MySQLDumpParameterString(value))
            End Function

            ''' <summary>
            ''' Validates if item is existing in this instance of DumpParameterCollection.
            ''' </summary>
            ''' <param name="value">Value to find.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Overloads Function Contains(ByVal value As MySQLApplicationParameterEnum) As Boolean
                Return List.Contains(MySQLParameterString(value))
            End Function

            ''' <summary>
            ''' Removes an item in this instance of DumpParameterCollection.
            ''' </summary>
            ''' <param name="value"></param>
            ''' <remarks></remarks>
            Public Overloads Sub Remove(ByVal value As String)
                If List.Contains(value) Then List.Remove(value)
            End Sub

            ''' <summary>
            ''' Removes an item in this instance of DumpParameterCollection.
            ''' </summary>
            ''' <param name="value"></param>
            ''' <remarks></remarks>
            Public Overloads Sub Remove(ByVal value As MySQLDumpParameterEnum)
                Dim param As String = MySQLDumpParameterString(value)
                If List.Contains(param) Then List.Remove(param)
            End Sub

            ''' <summary>
            ''' Removes an item in this instance of DumpParameterCollection.
            ''' </summary>
            ''' <param name="value"></param>
            ''' <remarks></remarks>
            Public Overloads Sub Remove(ByVal value As MySQLApplicationParameterEnum)
                Dim param As String = MySQLParameterString(value)
                If List.Contains(param) Then List.Remove(param)
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
                        List.Clear()
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
        ''' Creates a new instance of TarsierEyes.MySQL.Dump.
        ''' </summary>
        ''' <param name="server">MySQL server IP or Hostname.</param>
        ''' <param name="database">MySQL database name.</param>
        ''' <param name="userid">MySQL database server login id.</param>
        ''' <param name="password">MySQL database server login password.</param>
        ''' <remarks></remarks>
        Sub New(ByVal server As String, ByVal database As String, ByVal userid As String, ByVal password As String)
            _server = server : _database = database : _userid = userid : _password = password
            _parameters.Clear() : _error = String.Empty : If _maxallowedpacket <= 0 Then _maxallowedpacket = 16
        End Sub

        ''' <summary>
        ''' Creates a new instance of TarsierEyes.MySQL.Dump.
        ''' </summary>
        ''' <param name="server">MySQL server IP or Hostname.</param>
        ''' <param name="database">MySQL database name.</param>
        ''' <param name="userid">MySQL database server login id.</param>
        ''' <param name="password">MySQL database server login password.</param>
        ''' <param name="filename">MySQL dump filename.</param>
        ''' <remarks></remarks>
        Sub New(ByVal server As String, ByVal database As String, ByVal userid As String, ByVal password As String, ByVal filename As String)
            _server = server : _database = database : _userid = userid : _password = password
            _parameters.Clear() : _filename = filename : _error = String.Empty : If _maxallowedpacket <= 0 Then _maxallowedpacket = 16
        End Sub
#End Region

#Region "Properties"
        ''' <summary>
        ''' Gets whether MySQL database dumping application is present and database exportation can be performed or not. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CanExport() As Boolean
            Get
                Dim b As Boolean = False : ExtractResourceApplications()
                b = File.Exists(Application.StartupPath & "\mysqldump.exe")
                RemoveResourceApplications()

                Return b
            End Get
        End Property

        ''' <summary>
        ''' Gets whether MySQL application is present and database importation can be performed or not. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CanImport() As Boolean
            Get
                Dim b As Boolean = False : ExtractResourceApplications()
                b = File.Exists(Application.StartupPath & "\mysql.exe")
                RemoveResourceApplications()

                Return b
            End Get
        End Property

        Dim _database As String = String.Empty
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
            End Set
        End Property

        Private Shared _error As String = String.Empty
        ''' <summary>
        ''' Gets error encountered in the last executed exportation or importation.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property ErrorMessage() As String
            Get
                Return _error
            End Get
        End Property

        Dim _filename As String = String.Empty
        ''' <summary>
        ''' Gets or sets MySQL dump filename to be exported or imported.
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

        Private Shared _maxallowedpacket As Integer = 16
        ''' <summary>
        ''' Gets or sets max allowed packet alloted for database restoration and / or execution.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Property MaxAllowedPacket() As Integer
            Get
                If _maxallowedpacket <= 0 Then _maxallowedpacket = 16
                Return _maxallowedpacket
            End Get
            Set(ByVal value As Integer)
                _maxallowedpacket = value
            End Set
        End Property

        Dim _parameters As New DumpParameterCollection
        ''' <summary>
        ''' Gets current list of MySQL dump parameters.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Parameters() As DumpParameterCollection
            Get
                Return _parameters
            End Get
        End Property

        Dim _password As String = String.Empty
        ''' <summary>
        ''' Gets or sets MySQL server database login password.
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
            End Set
        End Property

        Dim _server As String = String.Empty
        ''' <summary>
        ''' Gets or sets MySQL server ip or hostname.
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
            End Set
        End Property

        Dim _userid As String = String.Empty

        ''' <summary>
        ''' Gets or sets MySQL server database login id.
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
            End Set
        End Property

#End Region

#Region "Methods"
        Private Sub ExtractResourceApplications()
            RemoveResourceApplications() : Dim curdir As String = Application.StartupPath & "\MySQL Apps"

            Dim fi As FileInfo = My.Resources.MySQL.ToFileObject("exe", curdir)
            If fi IsNot Nothing Then
                If IO.File.Exists(fi.FullName) Then
                    Try
                        My.Computer.FileSystem.RenameFile(fi.FullName, "mysql.exe")
                    Catch ex As Exception
                    End Try
                End If
            End If

            fi = My.Resources.MySQLDump.ToFileObject("exe", curdir)
            If fi IsNot Nothing Then
                If IO.File.Exists(fi.FullName) Then
                    Try
                        My.Computer.FileSystem.RenameFile(fi.FullName, "mysqldump.exe")
                    Catch ex As Exception
                    End Try
                End If
            End If

            If Directory.Exists(curdir) Then
                If IO.File.Exists(curdir & "\mysql.exe") And _
                   Not File.Exists(Application.StartupPath & "\mysql.exe") Then
                    Try
                        File.Copy(curdir & "\mysql.exe", Application.StartupPath & "\mysql.exe")
                    Catch ex As Exception
                    End Try
                End If

                If IO.File.Exists(curdir & "\mysqldump.exe") And _
                 Not File.Exists(Application.StartupPath & "\mysqldump.exe") Then
                    Try
                        File.Copy(curdir & "\mysqldump.exe", Application.StartupPath & "\mysqldump.exe")
                    Catch ex As Exception
                    End Try
                End If
            End If
        End Sub

        Private Sub RemoveResourceApplications()
            Dim curdir As String = Application.StartupPath & "\MySQL Apps"

            If Directory.Exists(curdir) Then
                Try
                    Directory.Delete(curdir, True)
                Catch ex As Exception
                End Try
            End If

            If File.Exists(Application.StartupPath & "\mysql.exe") Then
                Try
                    File.Delete(Application.StartupPath & "\mysql.exe")
                Catch ex As Exception
                End Try
            End If

            If File.Exists(Application.StartupPath & "\mysqldump.exe") Then
                Try
                    File.Delete(Application.StartupPath & "\mysqldump.exe")
                Catch ex As Exception
                End Try
            End If
        End Sub
#End Region

#Region "Internal Functions"
        ''' <summary>
        ''' Performs MySQL dump exportation.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Export() As Boolean
            Dim bExport As Boolean = False

            If CanExport Then
                ExtractResourceApplications()

                Try
                    If My.Computer.FileSystem.FileExists(_filename) Then My.Computer.FileSystem.DeleteFile(_filename, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
                Catch ex As Exception
                End Try

                Try
                    If My.Computer.FileSystem.FileExists(Application.StartupPath & "\bk.bat") Then My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\bk.bat", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
                Catch ex As Exception
                End Try

                Dim sw As IO.StreamWriter
                sw = IO.File.CreateText(Application.StartupPath & "\bk.bat")

                Dim sParam As String = String.Empty
                For Each s As String In _parameters
                    sParam &= " " & s.Trim
                Next

                With sw
                    .WriteLine("""" & Application.StartupPath & "\mysqldump"" --host=" & _server & " --user=" & _userid & " --password=" & _password & " " & _database & sParam & " --set-charset --default-character-set=utf8 >" & """" & _filename & """")
                    .Close() : .Dispose()
                End With

                Dim proc As New Process
                With proc
                    .StartInfo.FileName = Application.StartupPath & "\bk.bat"
                    .StartInfo.CreateNoWindow = True : .StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    .StartInfo.RedirectStandardError = True : .StartInfo.UseShellExecute = False
                    .Start()

                    While Not .HasExited
                        Application.DoEvents()
                    End While

                    _error = .StandardError.ReadToEnd.Replace("The handle is invalid." & vbNewLine, String.Empty).Replace("The handle is invalid.", String.Empty).Trim
                    .Dispose()
                    bExport = File.Exists(_filename)
                End With
            End If

            Try
                If My.Computer.FileSystem.FileExists(Application.StartupPath & "\bk.bat") Then My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\bk.bat", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
            Catch ex As Exception
            End Try

            RemoveResourceApplications() : Return bExport
        End Function

        ''' <summary>
        ''' Performs MySQL batch file execution.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Import() As Boolean
            Dim bImport As Boolean = False : ExtractResourceApplications()

            Try
                If My.Computer.FileSystem.FileExists(Application.StartupPath & "\rs.bat") = True Then My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\rs.bat", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
            Catch ex As Exception
            End Try

            Dim sw As IO.StreamWriter
            sw = IO.File.CreateText(Application.StartupPath & "\rs.bat")

            With sw
                .WriteLine("""" & Application.StartupPath & "\mysql"" -h " & _server & " -u " & _userid & " -p" & _password & " " & _database & " --max_allowed_packet=" & _maxallowedpacket & "M --default-character-set=utf8 < """ & _filename.Trim.Replace("\", "/") & """")
                .Close() : .Dispose()
            End With

            If File.Exists(Application.StartupPath & "\rs.bat") Then
                Dim proc As New Process
                With proc
                    .StartInfo.FileName = Application.StartupPath & "\rs.bat"
                    .StartInfo.RedirectStandardError = True : .StartInfo.UseShellExecute = False
                    .StartInfo.CreateNoWindow = True : .StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    .Start()

                    While .HasExited = False
                        Application.DoEvents()
                    End While

                    _error = .StandardError.ReadToEnd.Replace("The handle is invalid." & vbNewLine, String.Empty).Replace("The handle is invalid.", String.Empty)
                    bImport = String.IsNullOrEmpty(_error.Trim)

                    .Dispose()
                End With
            End If

            Try
                If My.Computer.FileSystem.FileExists(Application.StartupPath & "\rs.bat") Then My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\rs.bat", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
            Catch ex As Exception
            End Try

            RemoveResourceApplications() : Return bImport
        End Function
#End Region

#Region "Shared Functions"
        ''' <summary>
        ''' Validates whether database dumping can be performed or not.
        ''' </summary>
        ''' <param name="task">Export or Import.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CanDo(ByVal task As TaskEnum) As Boolean
            Dim bDo As Boolean = False

            Dim d As New Dump(String.Empty, String.Empty, String.Empty, String.Empty)
            Select Case task
                Case TaskEnum.Export : bDo = d.CanExport
                Case TaskEnum.Import : bDo = d.CanImport
                Case Else
            End Select
            d.Dispose()

            Return bDo
        End Function

        ''' <summary>
        ''' Performs database backup using MySQL dumping application.
        ''' </summary>
        ''' <param name="server">Server IP or hostname.</param>
        ''' <param name="database">Database name.</param>
        ''' <param name="userid">Database server login id.</param>
        ''' <param name="password">Database server login password.</param>
        ''' <param name="filename">Dump filename.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Backup(ByVal server As String, ByVal database As String, ByVal userid As String, ByVal password As String, ByVal filename As String) As Boolean
            Dim d As New Dump(server, database, userid, password, filename)
            Dim b As Boolean = d.Export
            d.Dispose()
            Return b
        End Function

        ''' <summary>
        ''' Performs database backup using MySQL dumping application.
        ''' </summary>
        ''' <param name="server">Server IP or hostname.</param>
        ''' <param name="database">Database name.</param>
        ''' <param name="userid">Database server login id.</param>
        ''' <param name="password">Database server login password.</param>
        ''' <param name="filename">Dump filename.</param>
        ''' <param name="parameters">Additional list of valid MySQL dump parameters.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Backup(ByVal server As String, ByVal database As String, ByVal userid As String, ByVal password As String, ByVal filename As String, ByVal parameters As DumpParameterCollection) As Boolean
            Dim d As New Dump(server, database, userid, password, filename)
            If parameters.Count > 0 Then
                For Each s As String In parameters
                    d.Parameters.Add(s)
                Next
            End If
            Dim b As Boolean = d.Export
            d.Dispose()
            Return b
        End Function

        ''' <summary>
        ''' Performs and executes the given sql statement in the specified connection using MySQL application itself.
        ''' </summary>
        ''' <param name="connectionstring">MySQL database connecton string.</param>
        ''' <param name="commandtext">Valid SQL statement.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Execute(ByVal connectionstring As String, ByVal commandtext As String) As Boolean
            If Not Directory.Exists(Application.StartupPath & "\tempsql") Then
                Try
                    My.Computer.FileSystem.CreateDirectory(Application.StartupPath & "\tempsql")
                Catch ex As Exception
                End Try
            End If

            _error = String.Empty

            If Directory.Exists(Application.StartupPath & "\tempsql") Then
                Dim b As Boolean = False

                If EDI.EDIWriter.Write(Application.StartupPath & "\tempsql\query.sql", commandtext) Then
                    Dim server As String = ConnectionStringValue(connectionstring, ConnectionDetailEnum.Server)
                    Dim database As String = ConnectionStringValue(connectionstring, ConnectionDetailEnum.Database)
                    Dim uid As String = ConnectionStringValue(connectionstring, ConnectionDetailEnum.UID)
                    Dim pwd As String = ConnectionStringValue(connectionstring, ConnectionDetailEnum.PWD)

                    b = Restore(server, database, uid, pwd, Application.StartupPath & "\tempsql\query.sql")
                Else : _error = "Can't generate a temporary file for the database query."
                End If

                If Directory.Exists(Application.StartupPath & "\tempsql") Then
                    Try
                        My.Computer.FileSystem.DeleteDirectory(Application.StartupPath & "\tempsql", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
                    Catch ex As Exception
                    End Try
                End If

                Return b
            Else
                _error = "Can't generate a temporary path for the database query file." : Return False
            End If
        End Function

        ''' <summary>
        ''' Performs database restoration from a batch file using MySQL application.
        ''' </summary>
        ''' <param name="server">Server IP or hostname.</param>
        ''' <param name="database">Database name.</param>
        ''' <param name="userid">Database server login id.</param>
        ''' <param name="password">Database server login password.</param>
        ''' <param name="filename">Dump filename.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Restore(ByVal server As String, ByVal database As String, ByVal userid As String, ByVal password As String, ByVal filename As String) As Boolean
            Dim d As New Dump(server, database, userid, password, filename)
            Dim b As Boolean = d.Import
            d.Dispose()
            Return b
        End Function

        ''' <summary>
        ''' Returns associated MySQLDump qualified command prompt parameter string.
        ''' </summary>
        ''' <param name="param">MySQL parameter.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function MySQLDumpParameterString(ByVal param As MySQLDumpParameterEnum) As String
            Dim sParams As String = String.Empty
            Dim params() As Char = param.ToString.ToCharArray

            If params.Length > 0 Then
                sParams = "-"
                For Each c As Char In params
                    Select Case c.ToString
                        Case "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", _
                             "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" : sParams &= "-"
                        Case Else
                    End Select
                    sParams &= c.ToString
                Next
            End If

            Return sParams.ToLower
        End Function

        ''' <summary>
        ''' Returns associated MySQL qualified command prompt parameter string.
        ''' </summary>
        ''' <param name="param"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function MySQLParameterString(ByVal param As MySQLApplicationParameterEnum) As String
            Dim sParam As String = String.Empty
            Dim params() As Char = param.ToString.ToCharArray

            If params.Length > 0 Then
                sParam = "-"
                For Each c As Char In params
                    Select Case c.ToString
                        Case "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", _
                             "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" : sParam &= "-"
                        Case Else
                    End Select
                    sParam &= c.ToString
                Next
            End If

            Return sParam.ToLower
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
End Class