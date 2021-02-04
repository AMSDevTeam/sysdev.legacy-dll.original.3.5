#Region "Imports"
Imports C1.Win.C1FlexGrid
Imports C1.Win.C1Input
Imports C1.Win.C1List
Imports DevComponents.DotNetBar
Imports DevComponents.DotNetBar.Controls
Imports DevComponents.Editors
Imports DevComponents.Editors.DateTimeAdv
Imports AMS_Controls.Settings
Imports MySql.Data.MySqlClient
Imports System.ComponentModel
Imports System.Text
Imports System.Threading
Imports TarsierEyes.Common
Imports TarsierEyes.Common.Simple
Imports TarsierEyes.Common.SQLStrings
Imports TarsierEyes.Common.Synchronization
Imports TarsierEyes.MySQL
Imports MetroFramework.Controls
Imports AMS_Controls.Controls
Imports System.ComponentModel.Design
#End Region

''' <summary>
''' Pick list / maintenance forms data binder and save functionality provider.
''' </summary> 
''' <remarks></remarks>
<ProvideProperty("FieldName", GetType(Control)), DefaultEvent("AfterSave")>
<Designer("System.Windows.Forms.Design.ParentControlDesigner,System.Design", GetType(IDesigner))>
Public Class PickListBinder
    Implements IExtenderProvider

    'Private Declare Function ReleaseCapture Lib "user32" () As Long
    'Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Long, ByVal wMsg As Long, ByVal wParam As Long, lParam As Any) As Long

    'Private Const HTCAPTION = 2
    'Private Const WM_NCLBUTTONDOWN = &HA1

    'Private Const WM_SYSCOMMAND = &H112

    Private Sub PickListBinder_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        Me.Capture = False

        Const WM_NCLBUTTONDOWN As Integer = &HA1S
        Const HTCAPTION As Integer = 2
        Dim msg As Message = _
            Message.Create(ParentForm.Handle, WM_NCLBUTTONDOWN, _
                New IntPtr(HTCAPTION), IntPtr.Zero)
        Me.DefWndProc(msg)
    End Sub

#Region "Events"
    ''' <summary>
    ''' Fires up after data has been loaded (for header - detail : fires up after header data has been loaded).
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event AfterDataLoading(ByVal sender As Object, ByVal e As DataLoadingEventArgs)
    ''' <summary>
    ''' Fires up after data is loaded into a detail grid.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event AfterDetailDataLoading(ByVal sender As Object, ByVal e As DataLoadingEventArgs)
    ''' <summary>
    ''' Fires up after performing data sourced field validations.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event AfterDefaultValidation(ByVal sender As Object, ByVal e As ValidationEventArgs)
    ''' <summary>
    ''' Fires up after default form load events specified by the control have occured.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event AfterFormLoad(ByVal sender As Object, ByVal e As System.EventArgs)
    ''' <summary>
    ''' Fires up after default form shown events specified by the control have occured.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event AfterFormShown(ByVal sender As Object, ByVal e As System.EventArgs)
    ''' <summary>
    ''' Fires up after performing data saving (when in header-detail approach).
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event AfterHeaderSave(ByVal sender As Object, ByVal e As SavingEventArgs)
    ''' <summary>
    ''' Fires up after performing data saving.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event AfterSave(ByVal sender As Object, ByVal e As SavingEventArgs)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Public Event AfterCancel(ByVal sender As Object)
    ''' <summary>
    ''' Fires up before performing data sourced field validations.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event BeforeDefaultValidation(ByVal sender As Object, ByVal e As ValidationEventArgs)
    ''' <summary>
    ''' Fires up before default form load events specified by the control have occured.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event BeforeFormLoad(ByVal sender As Object, ByVal e As System.EventArgs)
    ''' <summary>
    ''' Fires up before default form shown events specified by the control have occured.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event BeforeFormShown(ByVal sender As Object, ByVal e As System.EventArgs)
    ''' <summary>
    ''' Fires up after validations and before performing data saving.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event BeforeSave(ByVal sender As Object, ByVal e As SavingEventArgs)

#End Region

#Region "Enumerations"
    ''' <summary>
    ''' Query statement generation enumerations.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum QueryGenerationEnum
        ''' <summary>
        ''' Auto-generated based on the the data source.
        ''' </summary>
        ''' <remarks></remarks>
        Auto = 1
        ''' <summary>
        ''' Custom data source query statement.
        ''' </summary>
        ''' <remarks></remarks>
        Custom = 0
    End Enum

    ''' <summary>
    ''' Print check box visibility enumerations.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum PrintCheckVisibilityEnum
        ''' <summary>
        ''' Visible always.
        ''' </summary>
        ''' <remarks></remarks>
        Always
        ''' <summary>
        ''' Visible only if new record is currently created.
        ''' </summary>
        ''' <remarks></remarks>
        OnNewRecord
        ''' <summary>
        ''' Not visible.
        ''' </summary>
        ''' <remarks></remarks>
        Invisible
    End Enum
#End Region

#Region "Custom Classes"
    ''' <summary>
    ''' Control binding information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class BindingInfo
        ''' <summary>
        ''' Gets or sets binded control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BindedControl As Control = Nothing

        ''' <summary>
        ''' Gets or sets the datasource field name binded to the control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FieldName As String = String.Empty

        ''' <summary>
        ''' Creates a new instance of BindingInfo.
        ''' </summary>
        ''' <param name="control"></param>
        ''' <param name="field"></param>
        ''' <remarks></remarks>
        Sub New(ByVal control As Control, ByVal field As String)
            BindedControl = control : FieldName = field
        End Sub
    End Class

    ''' <summary>
    ''' Data loading event arguments.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DataLoadingEventArgs
        ''' <summary>
        ''' Determines the loaded data source generated from the given QueryStatement.
        ''' </summary>
        ''' <remarks></remarks>
        Public DataSource As DataTable = Nothing

        ''' <summary>
        ''' Determines encountered error message upon data loading.
        ''' </summary>
        ''' <remarks></remarks>
        Public ErrorMessage As String = String.Empty

        ''' <summary>
        ''' Determines the loaded C1FlexGrid.
        ''' </summary>
        ''' <remarks></remarks>
        Public Grid As C1FlexGrid = Nothing

        ''' <summary>
        ''' Determines the actual parented detail data source information.
        ''' </summary>
        ''' <remarks></remarks>
        Public Info As DetailDataSourceInfo = Nothing

        ''' <summary>
        ''' Determines the sql statement used for the data loading.
        ''' </summary>
        ''' <remarks></remarks>
        Public QueryStatement As String = String.Empty
    End Class

    ''' <summary>
    ''' Data source information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DataSourceInfo
        ''' <summary>
        ''' Gets or sets the database connection string.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ConnectionString As String = String.Empty
        ''' <summary>
        ''' Gets or sets the data source select statement.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property CommandText As String = String.Empty

        Dim _details As New DetailDataSourceCollection

        ''' <summary>
        ''' Gets the specified details value data source in the specified index (can also be accessed using Details.DataSources(index).ViewingGrid.DataSource).
        ''' </summary>
        ''' <param name="index"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads ReadOnly Property DetailDataSources(ByVal index As Integer) As DataTable
            Get
                Dim dt As DataTable = Nothing

                If _details.Count > index Then
                    If _details.DataSources(index).ViewingGrid.DataSource IsNot Nothing Then dt = _details.DataSources(index).ViewingGrid.DataSource
                End If

                Return dt
            End Get
        End Property

        ''' <summary>
        ''' Gets the specified details value data source with the specified pattern table name (can also be accessed using Details.DataSources(index).ViewingGrid.DataSource).
        ''' </summary>
        ''' <param name="table">Pattern table name.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads ReadOnly Property DetailDataSources(ByVal table As String) As DataTable
            Get
                Dim dt As DataTable = Nothing

                If Not String.IsNullOrEmpty(table.Trim) Then
                    For Each di As DetailDataSourceInfo In _details
                        With di
                            If .ViewingGrid.DataSource IsNot Nothing Then
                                Dim tablename As String = CType(.ViewingGrid.DataSource, DataTable).TableName
                                If table.Trim.ToLower = tablename.Trim.ToLower Then Return .ViewingGrid.DataSource
                            End If
                        End With
                    Next
                End If

                Return dt
            End Get
        End Property
        ''' <summary>
        ''' Gets the collection of detail data sources.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Details As DetailDataSourceCollection
            Get
                Return _details
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the command text used to view the complete viewing fields and values.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ViewingCommandText As String = String.Empty

#Region "Methods"
        ''' <summary>
        ''' Sets the value of the whole field in the specified detail grid data source table.
        ''' </summary>
        ''' <param name="index ">Table detail index.</param>
        ''' <param name="field">Field name.</param>
        ''' <param name="value">Value to assign.</param>
        ''' <remarks></remarks>
        Public Overloads Sub SetDetailDataSourceValue(ByVal index As Integer, ByVal field As String, ByVal value As Object)
            SetDetailDataSourceValue(DetailDataSources(index), field, value)
        End Sub

        ''' <summary>
        ''' Sets the value of the whole field in the specified detail grid data source table.
        ''' </summary>
        ''' <param name="table">Table name.</param>
        ''' <param name="field">Field name.</param>
        ''' <param name="value">Value to assign.</param>
        ''' <remarks></remarks>
        Public Overloads Sub SetDetailDataSourceValue(ByVal table As String, ByVal field As String, ByVal value As Object)
            SetDetailDataSourceValue(DetailDataSources(table), field, value)
        End Sub

        ''' <summary>
        ''' Sets the value of the whole field in the specified detail grid data source table.
        ''' </summary>
        ''' <param name="table">Table</param>
        ''' <param name="field">Field name.</param>
        ''' <param name="value">Value to assign.</param>
        ''' <remarks></remarks>
        Public Overloads Sub SetDetailDataSourceValue(ByVal table As DataTable, ByVal field As String, ByVal value As Object)
            If table IsNot Nothing Then
                If table.Columns.Contains(field) Then
                    For Each dr As DataRow In table.Rows
                        If dr.RowState <> DataRowState.Deleted And
                           dr.RowState <> DataRowState.Detached Then dr.Item(field) = value
                    Next
                End If
            End If
        End Sub
#End Region

    End Class

    ''' <summary>
    ''' Collection of detail data source information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DetailDataSourceCollection
        Inherits CollectionBase

        ''' <summary>
        ''' Gets the detail data source information in the specified index of the collection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Overloads ReadOnly Property DataSources(ByVal index As Integer) As DetailDataSourceInfo
            Get
                Return CType(List(index), DetailDataSourceInfo)
            End Get
        End Property

        ''' <summary>
        ''' Gets the detail data source information with the specified table name in the collection.
        ''' </summary>
        ''' <param name="tablename"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Overloads ReadOnly Property DataSources(ByVal tablename As String) As DetailDataSourceInfo
            Get
                Dim di As DetailDataSourceInfo = Nothing

                For Each d As DetailDataSourceInfo In List
                    If d.DetailTable.TableName = tablename.ToLower Then
                        di = d : Exit For
                    End If
                Next

                Return di
            End Get
        End Property

        ''' <summary>
        ''' Adds a new detail data source information in the collection.
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Add(ByVal detail As DetailDataSourceInfo) As Integer
            Return List.Add(detail)
        End Function

        ''' <summary>
        ''' Determines whether the specified data source information is existing within the collection or not.
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Contains(ByVal detail As DetailDataSourceInfo) As Boolean
            Return List.Contains(detail)
        End Function

        ''' <summary>
        ''' Determines whether the specified data source information with the specified main database table name is existing within the collection or not.
        ''' </summary>
        ''' <param name="tablename"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Contains(ByVal tablename As String) As Boolean
            Dim exists As Boolean = False

            For Each di As DetailDataSourceInfo In List
                If di.QueryStatements.TableName.ToLower = tablename.ToLower Then
                    exists = True : Exit For
                End If
            Next

            Return exists
        End Function

        ''' <summary>
        ''' Removes the specified detail data source information within the collection.
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <remarks></remarks>
        Public Overloads Sub Remove(ByVal detail As DetailDataSourceInfo)
            If List.Contains(detail) Then List.Remove(detail)
        End Sub

        ''' <summary>
        ''' Removes the specified detail data source information with the specified main database table name within the collection.
        ''' </summary>
        ''' <param name="tablename"></param>
        ''' <remarks></remarks>
        Public Overloads Sub Remove(ByVal tablename As String)
            For Each di As DetailDataSourceInfo In List
                If di.QueryStatements.TableName.ToLower = tablename.ToLower Then
                    List.Remove(di) : Exit For
                End If
            Next
        End Sub

    End Class

    ''' <summary>
    ''' Details data source information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DetailDataSourceInfo

#Region "Properties"
        Dim _connection As MySqlConnection = Nothing
        Dim _adapter As MySqlDataAdapter = Nothing
        Dim _commandbuilder As MySqlCommandBuilder = Nothing
        Dim _deletecommand As MySqlCommand = Nothing
        Dim _insertcommand As MySqlCommand = Nothing
        Dim _updatecommand As MySqlCommand = Nothing
        Dim _transaction As MySqlTransaction = Nothing

        ''' <summary>
        ''' Gets or sets whether to apply relative foreign key value from header primary key value or not.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ApplyForeignKeyAssignment As Boolean = True

        ''' <summary>
        ''' Gets or sets the main database table's command text.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CommandText As String = String.Empty

        ''' <summary>
        ''' Gets or sets the detail data source connection.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Connection As MySqlConnection
            Get
                Return _connection
            End Get
            Set(ByVal value As MySqlConnection)
                _connection = value
            End Set
        End Property

        Dim _connectionstring As String = Settings.ConnectionString

        ''' <summary>
        ''' Gets the connection string that was used to load data to the data sources.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ConnectionString As String
            Get
                Return _connectionstring
            End Get
        End Property

        Dim _detailtable As DataTable = Nothing

        ''' <summary>
        ''' Gets the detail table to where all changes and modifications are currently taking effect into.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DetailTable As DataTable
            Get
                Return _detailtable
            End Get
        End Property

        ''' <summary>
        ''' Gets the delete command generated from the initial connection.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DeleteCommand As MySqlCommand
            Get
                Return _deletecommand
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the monitoring event argument.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property EventArgument As DataLoadingEventArgs = Nothing

        Dim _errormessage As String = String.Empty

        ''' <summary>
        ''' Gets the last encountered error message upon executing data update.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ErrorMessage As String
            Get
                Return _errormessage
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the associated loading picture box upon data loading of the binded viewing grid.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LoadingImageBox As PictureBox = Nothing

        ''' <summary>
        '''  Gets the insert command generated from the initial connection.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property InsertCommand As MySqlCommand
            Get
                Return _insertcommand
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the current parent binder.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ParentBinder As PickListBinder = Nothing

        Dim _querystatements As New QueryStatementInfo

        ''' <summary>
        '''  Gets the current referenced query statements.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property QueryStatements As QueryStatementInfo
            Get
                Return _querystatements
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the current header and detail connection transaction.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Transaction As MySqlTransaction
            Get
                Return _transaction
            End Get
            Set(ByVal value As MySqlTransaction)
                _transaction = value
            End Set
        End Property

        ''' <summary>
        '''  Gets the update command generated from the initial connection.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UpdateCommand As MySqlCommand
            Get
                Return _updatecommand
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the viewing command text (if there is) that can be populated in the specified ViewingGrid.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ViewingCommandText As String = String.Empty

        Dim _viewinggrid As C1FlexGrid = Nothing
        ''' <summary>
        ''' Gets or sets the viewing grid to where the data source generated from the ViewingCommandText will be displayed.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ViewingGrid As C1FlexGrid
            Get
                Return _viewinggrid
            End Get
            Set(ByVal value As C1FlexGrid)
                _viewinggrid = value
                If value IsNot Nothing Then
                    AddHandler _viewinggrid.AfterDeleteRow, AddressOf ViewingGrid_AfterDeleteRow
                    AddHandler _viewinggrid.AfterEdit, AddressOf ViewingGrid_AfterEdit
                End If
            End Set
        End Property

        Public Event AfterDataEditing(ByVal sender As Object)

        Public Event AfterDataDeleting(ByVal sender As Object)

#End Region

        ''' <summary>
        ''' Creates a new instance of DetailDataSourceInfo.
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            _querystatements = Nothing
            _querystatements = New QueryStatementInfo(_connectionstring)
        End Sub

        ''' <summary>
        ''' Creates a new instance of DetailDataSourceInfo.
        ''' </summary>
        ''' <param name="connection">Connection string that was used to load records in the data sources.</param>
        ''' <remarks></remarks>
        Sub New(ByVal connection As String)
            _connectionstring = connection
            _querystatements = Nothing
            _querystatements = New QueryStatementInfo(connection)
        End Sub

#Region "Custom Events"
        Private Sub DetailTableRowDeleting(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)
            Dim currentrow As DataRow = e.Row
            Dim detailrow As DataRow = Nothing

            Dim keyname As String = QueryStatements.KeyName
            If String.IsNullOrEmpty(keyname.Trim) Then keyname = _detailtable.Columns(QueryStatements.KeyIndex).ColumnName

            If currentrow.RowState <> DataRowState.Detached Then
                Dim drs() As DataRow = _detailtable.Select("(`" & keyname & "` = " & currentrow.Item(keyname) & ")")
                If drs.Length > 0 Then
                    drs(0).Delete()

                End If
                Exit Sub
            End If

            RaiseEvent AfterDataDeleting(sender)
        End Sub

        Private Sub DetailTableRowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)
            Dim currentrow As DataRow = e.Row
            Dim detailrow As DataRow = Nothing

            Dim keyname As String = QueryStatements.KeyName
            If String.IsNullOrEmpty(keyname.Trim) Then keyname = _detailtable.Columns(QueryStatements.KeyIndex).ColumnName

            Select Case e.Action
                Case DataRowAction.Add
                    Dim drs() As DataRow = _detailtable.Select("(`" & keyname & "` = " & currentrow.Item(keyname) & ")")
                    If drs.Length > 0 Then : detailrow = drs(0)
                    Else
                        Dim dt As DataTable = currentrow.Table
                        Dim values(_detailtable.Columns.Count - 1) As Object
                        values(_detailtable.Columns(keyname).Ordinal) = currentrow.Item(keyname)

                        For i As Integer = 0 To _detailtable.Columns.Count - 1
                            If i <> dt.Columns(keyname).Ordinal Then
                                If dt.Columns.Contains(_detailtable.Columns(i).ColumnName) Then values(i) = currentrow.Item(_detailtable.Columns(i).ColumnName)
                            End If
                        Next

                        detailrow = _detailtable.Rows.Add(values)
                    End If

                    For Each col As DataColumn In currentrow.Table.Columns
                        If col.ColumnName.Trim.ToLower <> keyname.Trim.ToLower Then
                            If IsDBNull(currentrow.Item(col.ColumnName)) Then
                                Select Case col.DataType.Name
                                    Case GetType(String).Name : currentrow.Item(col.ColumnName) = String.Empty
                                    Case GetType(Date).Name : currentrow.Item(col.ColumnName) = Now.Date
                                    Case GetType(Integer).Name, GetType(Long).Name,
                                        GetType(Single).Name, GetType(Double).Name,
                                        GetType(Byte).Name, GetType(Decimal).Name,
                                        GetType(Boolean).Name : currentrow.Item(col.ColumnName) = 0
                                    Case Else : currentrow.Item(col.ColumnName) = DBNull.Value
                                End Select
                            End If
                        End If
                    Next

                    If ParentBinder IsNot Nothing Then
                        Settings.MarkControlAsEdited(ParentBinder.lblTitle)
                    End If

                Case DataRowAction.Delete
                    If currentrow.RowState <> DataRowState.Detached Then
                        Dim drs() As DataRow = _detailtable.Select("(`" & keyname & "` = " & currentrow.Item(keyname) & ")")
                        If drs.Length > 0 Then drs(0).Delete()
                        Exit Sub
                    End If

                    If ParentBinder IsNot Nothing Then
                        If ParentBinder.ParentFormShown Then MarkFormAsEdited(ParentBinder.ContainerForm)
                    End If

                Case DataRowAction.Change, DataRowAction.ChangeCurrentAndOriginal, DataRowAction.ChangeOriginal
                    If Not IsDBNull(currentrow.Item(keyname)) Then
                        Dim drs() As DataRow = _detailtable.Select("(`" & keyname & "` = " & currentrow.Item(keyname) & ")")
                        If drs.Length > 0 Then : detailrow = drs(0)
                        Else
                            Dim dt As DataTable = currentrow.Table
                            Dim values(_detailtable.Columns.Count - 1) As Object
                            values(_detailtable.Columns(keyname).Ordinal) = currentrow.Item(keyname)

                            For i As Integer = 0 To _detailtable.Columns.Count - 1
                                If i <> dt.Columns(keyname).Ordinal Then
                                    If dt.Columns.Contains(_detailtable.Columns(i).ColumnName) Then values(i) = currentrow.Item(_detailtable.Columns(i).ColumnName)
                                End If
                            Next

                            detailrow = _detailtable.Rows.Add(values)
                        End If
                    End If

                    If ParentBinder IsNot Nothing Then
                        If ParentBinder.ParentFormShown Then MarkFormAsEdited(ParentBinder.ContainerForm)
                    End If

                Case Else
            End Select

            If detailrow IsNot Nothing Then
                For Each col As DataColumn In _detailtable.Columns
                    If currentrow.Table.Columns.Contains(col.ColumnName) Then detailrow.Item(col.ColumnName) = currentrow.Item(col.ColumnName)
                Next
            End If

            RaiseEvent AfterDataEditing(sender)
        End Sub

        Private Sub RemoveRow(ByVal row As Row)
            If ViewingGrid.DataSource IsNot Nothing Then
                If CType(ViewingGrid.DataSource, DataTable).Rows.Count >= row.Index Then
                    Dim match As Boolean = True
                    Dim dr As DataRow = CType(ViewingGrid.DataSource, DataTable).Rows(row.Index - 1)
                    For Each dc As DataColumn In CType(ViewingGrid.DataSource, DataTable).Columns
                        If Not dr.Item(dc.ColumnName).Equals(row.Item(dc.ColumnName)) Then
                            match = False : Exit Sub
                        End If
                    Next
                    If match Then dr.Delete()
                End If
            End If
        End Sub

        Private Sub ViewingGrid_AfterAddRow(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs)
            If ViewingGrid.DataSource IsNot Nothing Then
                Dim dt As DataTable = ViewingGrid.DataSource : dt.AcceptChanges()
                Dim dr As DataRow = Nothing

                Try
                    dr = dt.Rows(ViewingGrid.RowSel - 1)
                    For Each col As DataColumn In dt.Columns
                        Select Case col.DataType.Name
                            Case GetType(String).Name : dr.Item(col.ColumnName) = String.Empty
                            Case GetType(Decimal).Name, GetType(Double).Name, GetType(Single).Name,
                                GetType(Long).Name, GetType(Integer).Name, GetType(Byte).Name,
                                GetType(Boolean).Name : dr.Item(col.ColumnName) = 0
                            Case GetType(Date).Name : dr.Item(col.ColumnName) = Now
                            Case Else
                        End Select
                    Next

                    With ViewingGrid
                        If .RowSel >= 1 And
                           Not .Rows(.RowSel).IsNode Then
                            For Each col As Column In .Cols
                                If dt.Columns.Contains(col.Name) Then dr.Item(col.Name) = .Rows(.RowSel).Item(col.Name)
                            Next
                        End If
                    End With
                Catch ex As Exception
                End Try

                dt.AcceptChanges()
            End If
        End Sub

        Private Sub ViewingGrid_AfterDeleteRow(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs)
            'RemoveRow(ViewingGrid.Rows(e.Row))
            Dim frm As Form = ViewingGrid.FindForm()
            If frm IsNot Nothing Then MarkFormAsEdited(frm)

        End Sub

        Private Sub ViewingGrid_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs)
            Dim frm As Form = ViewingGrid.FindForm()
            If frm IsNot Nothing Then MarkFormAsEdited(frm)

        End Sub

        Private Sub ViewingGrid_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            If _detailtable IsNot Nothing Then _detailtable.Rows.Clear()
            If ViewingGrid.DataSource IsNot Nothing Then
                CType(ViewingGrid.DataSource, DataTable).TableName = _detailtable.TableName
                AddHandler CType(ViewingGrid.DataSource, DataTable).RowChanged, AddressOf DetailTableRowChanged
                AddHandler CType(ViewingGrid.DataSource, DataTable).RowDeleting, AddressOf DetailTableRowDeleting
            End If
        End Sub
#End Region

#Region "Methods"
        ''' <summary>
        ''' Provides binding handler to the viewing grid's events.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AttachDataSourceHandler()
            Dim dt As DataTable = ViewingGrid.DataSource

            If dt IsNot Nothing Then
                AddHandler dt.RowDeleting, AddressOf DetailTableRowDeleting
                AddHandler dt.RowChanged, AddressOf DetailTableRowChanged
            End If
        End Sub

        ''' <summary>
        ''' Reloads the patter data table to generate the pattern sql statements. 
        ''' </summary>
        ''' <param name="connection">Database connection string.</param>
        ''' <remarks></remarks>
        Public Sub ReloadPatternTable(ByVal connection As String)
            If _connection Is Nothing Then _connection = New MySqlConnection(connection)

            Try
                _adapter = New MySqlDataAdapter(CommandText, _connection)

                _commandbuilder = New MySqlCommandBuilder(_adapter)
                _detailtable = New DataTable

                _adapter.FillSchema(_detailtable, SchemaType.Mapped)
                _adapter.Fill(_detailtable)
                If _connection.State = ConnectionState.Closed Then _connection.Open()

                _deletecommand = _commandbuilder.GetDeleteCommand
                _insertcommand = _commandbuilder.GetInsertCommand
                _updatecommand = _commandbuilder.GetUpdateCommand

                Dim keyname As String = QueryStatements.KeyName
                If String.IsNullOrEmpty(keyname.Trim) Then keyname = _detailtable.Columns(QueryStatements.KeyIndex).ColumnName

                _insertcommand.CommandText &= "; SELECT * FROM " & _detailtable.TableName & " WHERE " & keyname & " = LAST_INSERT_ID()"
                _insertcommand.UpdatedRowSource = UpdateRowSource.Both

                Dim update As String = _updatecommand.CommandText.Replace("WHERE", "|")
                Dim delete As String = _deletecommand.CommandText.Split("WHERE")(0)
                Dim insert As String = _insertcommand.CommandText.Split("WHERE")(0)

                update = update.Split("|")(0)

                update &= "WHERE (`" & keyname & "` = @p" & _detailtable.Columns.Count & ")"
                delete &= "WHERE (`" & keyname & "` = @p1)" 'AND (`{ParentField}` = '{ParentValue}')"

                _deletecommand.CommandText = delete : _updatecommand.CommandText = update
                _querystatements.TableName = _detailtable.TableName
            Catch ex As Exception
            Finally
                If _connection.State = ConnectionState.Open Then _connection.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Reloads the data source of a binded grid using the command text assigned in the ViewingCommandText.
        ''' </summary>
        ''' <param name="connection"></param>
        ''' <remarks></remarks>
        Public Sub ReloadViewingPane(ByVal connection As String)
            If ViewingGrid IsNot Nothing Then
                'Simple.Redraw(ViewingGrid, False)

                With ViewingGrid
                    Try
                        If .DataSource IsNot Nothing Then CType(.DataSource, DataTable).Dispose()
                    Catch ex As Exception
                    Finally : .DataSource = Nothing
                    End Try

                    If LoadingImageBox IsNot Nothing Then
                        With LoadingImageBox
                            .Show() : .BringToFront()
                        End With
                    End If

                    Dim q As Que = Que.Execute(connection, ViewingCommandText, Que.ExecutionEnum.ExecuteReader)

                    If EventArgument IsNot Nothing Then
                        With EventArgument
                            .DataSource = Nothing : .ErrorMessage = q.ErrorMessage
                            .Grid = ViewingGrid : .QueryStatement = ViewingCommandText
                        End With
                    End If

                    If String.IsNullOrEmpty(q.ErrorMessage.Trim) Then
                        If EventArgument IsNot Nothing Then EventArgument.DataSource = q.DataTable

                        Dim dtable As New DataTable
                        With dtable
                            .Columns.Clear()
                            For Each col As DataColumn In q.DataTable.Columns
                                Dim dc As DataColumn = .Columns.Add(col.ColumnName, col.DataType)
                                If dc.DataType.Name.Contains("Date") Then dc.DataType = GetType(Date)
                                dc.AllowDBNull = True : dc.ReadOnly = False
                            Next
                            .Load(q.DataTable.CreateDataReader)
                        End With

                        Dim keyname As String = QueryStatements.KeyName
                        If _detailtable IsNot Nothing Then
                            If String.IsNullOrEmpty(keyname.Trim) And
                               _detailtable.Columns.Count > 0 Then keyname = _detailtable.Columns(QueryStatements.KeyIndex).ColumnName
                        Else
                            If String.IsNullOrEmpty(keyname.Trim) And
                               dtable.Columns.Count > 0 Then keyname = dtable.Columns(QueryStatements.KeyIndex).ColumnName
                        End If

                        If dtable.Columns.Contains(keyname) Then
                            With dtable.Columns(keyname)
                                If _detailtable IsNot Nothing Then
                                    If _detailtable.Rows.Count > 0 Then : .AutoIncrementSeed += (CLng(_detailtable.Compute("MAX([" & keyname & "])", String.Empty)) + 1)
                                    Else : .AutoIncrementSeed += 1
                                    End If
                                Else : .AutoIncrementSeed += 1
                                End If
                                .Unique = True
                                .AutoIncrement = True
                            End With
                        End If

                        .DataSource = dtable : ViewingFlexGrid.PreFormatFlexGrid(ViewingGrid)
                        If Not String.IsNullOrEmpty(CommandText.Trim) Then AttachDataSourceHandler()
                        .AutoSizeCols() : .AutoSizeRows()
                    Else
                        .Clear(ClearFlags.All)
                        .Rows.Count = 1 : .Cols.Count = 2 : .Cols(0).Visible = False
                        .Rows(0).Item(1) = "No item(s) could be displayed in this view."
                    End If
                    q.Dispose()
                End With
            End If
        End Sub

        ''' <summary>
        ''' Removes the viewing grid's datasource event handlers.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RemoveDatasourceHandler()
            Dim dt As DataTable = ViewingGrid.DataSource

            If dt IsNot Nothing Then
                RemoveHandler dt.RowDeleting, AddressOf DetailTableRowDeleting
                RemoveHandler dt.RowChanged, AddressOf DetailTableRowChanged
            End If
        End Sub

        ''' <summary>
        ''' Execute saving of each detail's field information to the database.
        ''' </summary>
        ''' <param name="parentkeyname">Parent transaction's key name.</param>
        ''' <param name="value">Parent transaction's key value.</param>
        ''' <remarks></remarks>
        Public Sub SaveDetails(ByVal parentkeyname As String, ByVal value As String)
            If _deletecommand IsNot Nothing Then _deletecommand.CommandText = _deletecommand.CommandText.Replace("{ParentField}", parentkeyname).Replace("{ParentValue}", value)

            _errormessage = String.Empty

            If _connection.State = ConnectionState.Closed Then _connection.Open()
            If _transaction Is Nothing Then _transaction = _connection.BeginTransaction

            If _deletecommand IsNot Nothing Then _deletecommand.Transaction = _transaction
            If _insertcommand IsNot Nothing Then _insertcommand.Transaction = _transaction
            If _updatecommand IsNot Nothing Then _updatecommand.Transaction = _transaction

            If _detailtable.Columns.Contains(parentkeyname) And
               ApplyForeignKeyAssignment Then
                Dim rws() As DataRow = _detailtable.Select("", "", DataViewRowState.Added)
                For Each rw As DataRow In rws
                    With rw
                        .BeginEdit() : .Item(parentkeyname) = value : .EndEdit()
                    End With
                Next
            End If

            Dim daMain As New MySqlDataAdapter
            daMain.UpdateCommand = _updatecommand
            daMain.InsertCommand = _insertcommand
            daMain.DeleteCommand = _deletecommand
            daMain.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord
            daMain.MissingSchemaAction = MissingSchemaAction.AddWithKey

            Dim dtchanges As DataTable = _detailtable.GetChanges(DataRowState.Added + DataRowState.Modified +
                                                                 DataRowState.Deleted + DataRowState.Detached)

            AddHandler daMain.RowUpdated, New MySqlRowUpdatedEventHandler(AddressOf OnRowUpdated)

            If dtchanges IsNot Nothing Then
                If dtchanges.Rows.Count > 0 Then

                    daMain.Update(_detailtable)
                    '_detailtable.Merge(dtchanges, False, MissingSchemaAction.Ignore)
                    _detailtable.AcceptChanges()
                    Try
                        _transaction.Commit()
                    Catch ex As Exception
                        If Not IsCommitted(ex) Then
                            _errormessage = ex.Message
                            _transaction.Rollback()
                        End If
                    Finally
                        If _connection.State = ConnectionState.Open Then _connection.Close()
                    End Try
                End If
            End If
        End Sub
        Private Sub OnRowUpdated(ByVal sender As Object, ByVal e As MySqlRowUpdatedEventArgs)
            ' If this is an insert, then skip this row.
            If e.StatementType = StatementType.Insert Then
                e.Status = UpdateStatus.SkipCurrentRow
            End If
        End Sub
#End Region

#Region "Functions"
        Private Overloads Function GetDeletedRows() As DataRow()
            Return GetDeletedRows(_detailtable)
        End Function

        Private Overloads Function GetDeletedRows(ByVal table As DataTable) As DataRow()
            Dim drs As New List(Of DataRow) : drs.Clear()

            For Each rw As DataRow In table.Rows
                If rw.RowState = DataRowState.Deleted Then drs.Add(rw)
            Next

            Return drs.ToArray
        End Function

        Private Function GetInsertedRows() As DataRow()
            Return _detailtable.Select(String.Empty, String.Empty, DataViewRowState.Added)
        End Function

        Private Function GetUpdatedRows() As DataRow()
            Return _detailtable.Select(String.Empty, String.Empty, DataViewRowState.ModifiedCurrent)
        End Function

        Private Function IsCommitted(ByVal ex As Exception) As Boolean
            Return ex.Message.Trim.ToLower.Contains("Transaction has already been committed or is not pending".ToLower)
        End Function

        ''' <summary>
        ''' Reloads the patter data table to generate the pattern sql statements asynchronously.
        ''' </summary>
        ''' <param name="connection"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ReloadPatternTableAsync(ByVal connection As String) As IAsyncResult
            Dim delReload As New Action(Of String)(AddressOf ReloadPatternTable)
            Return delReload.BeginInvoke(connection, Nothing, delReload)
        End Function

        ''' <summary>
        ''' Reloads the data source of a binded grid using the command text assigned in the ViewingCommandText asynchronously.
        ''' </summary>
        ''' <param name="connection"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ReloadViewingPaneAsync(ByVal connection As String) As IAsyncResult
            Dim delReload As New Action(Of String)(AddressOf ReloadViewingPane)
            Return delReload.BeginInvoke(connection, Nothing, delReload)
        End Function

        ''' <summary>
        ''' Execute saving of each detail's field information to the database asynchronously.
        ''' </summary>
        ''' <param name="parentkeyname">Parent transaction's key name.</param>
        ''' <param name="value">Parent transaction's key value.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SaveDetailsAsync(ByVal parentkeyname As String, ByVal value As String) As IAsyncResult
            Dim delSave As New Action(Of String, String)(AddressOf SaveDetails)
            Return delSave.BeginInvoke(parentkeyname, value, Nothing, delSave)
        End Function
#End Region

    End Class

    ''' <summary>
    ''' Field information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class FieldInfo
        Dim _fieldname As String = String.Empty

        ''' <summary>
        ''' Gets the field name.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FieldName As String
            Get
                Return _fieldname
            End Get
        End Property

        Dim _datatype As Type = Nothing
        ''' <summary>
        ''' Gets the field's data type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DataType As Type
            Get
                Return _datatype
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the field's value.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FieldValue As Object = Nothing

        ''' <summary>
        ''' Creates a new instance of FieldInfo.
        ''' </summary>
        ''' <remarks></remarks>
        Sub New(ByVal name As String, ByVal type As Type, ByVal value As Object)
            _fieldname = name : _datatype = type : FieldValue = value
        End Sub
    End Class

    ''' <summary>
    ''' Collection of field information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class FieldCollection
        Inherits CollectionBase

        ''' <summary>
        ''' Gets the specified field information in the specified index of the collection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Overloads ReadOnly Property Fields(ByVal index As Integer) As FieldInfo
            Get
                Return CType(List(index), FieldInfo)
            End Get
        End Property

        ''' <summary>
        ''' Gets the specified field information with the specified field name in the collection.
        ''' </summary>
        ''' <param name="fieldname">Field name.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Overloads ReadOnly Property Fields(ByVal fieldname As String) As FieldInfo
            Get
                Dim fi As FieldInfo = Nothing

                For Each f As FieldInfo In List
                    If f.FieldName.ToLower = fieldname.ToLower Then
                        fi = f : Exit For
                    End If
                Next

                Return fi
            End Get
        End Property

        ''' <summary>
        ''' Adds a new field information in the collection.
        ''' </summary>
        ''' <param name="fi"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Add(ByVal fi As FieldInfo) As Integer
            Return List.Add(fi)
        End Function

        ''' <summary>
        ''' Validates whether the specified field information is already existing in the collection or not.
        ''' </summary>
        ''' <param name="fi"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Contains(ByVal fi As FieldInfo) As Boolean
            Return List.Contains(fi)
        End Function

        ''' <summary>
        ''' Validates whether the specified field information with the specified field name is already existing in the collection or not.
        ''' </summary>
        ''' <param name="fieldname">Field name.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Contains(ByVal fieldname As String) As Boolean
            Dim exists As Boolean = False

            For Each fi As FieldInfo In List
                If fi.FieldName.ToLower = fieldname.ToLower Then
                    exists = True : Exit For
                End If
            Next

            Return exists
        End Function
    End Class

    ''' <summary>
    ''' Insert and update statements.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QueryStatementInfo

#Region "Properties"
        Dim _connectionstring As String = String.Empty

        ''' <summary>
        ''' Gets the connection string used to generate the command texts.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ConnectionString As String
            Get
                Return _connectionstring
            End Get
        End Property

        Dim _datasource As DataTable = Nothing
        ''' <summary>
        ''' Gets or sets the query statement's reference data source.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSource As DataTable
            Get
                Return _datasource
            End Get
            Set(ByVal value As DataTable)
                _datasource = value
                'GenerateQueryStatements()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether query to be executed is either the auto-generated or the custom-generated.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Execution As QueryGenerationEnum = QueryGenerationEnum.Auto

        Dim _insertstatement As String = String.Empty

        ''' <summary>
        ''' Gets the datasource-generated insert statement.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property InsertStatement As String
            Get
                Return _insertstatement
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the custom insert statement to execute when Execution property is set to Custom.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InsertStatementCustom As String = String.Empty

        ''' <summary>
        ''' Gets or sets primary key field's index.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property KeyIndex As Integer = 0

        Dim _keyname As String = String.Empty

        ''' <summary>
        ''' Gets or sets primary key field's name.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property KeyName As String
            Get
                Return _keyname
            End Get
            Set(ByVal value As String)
                _keyname = value
                If Not String.IsNullOrEmpty(value.Trim) Then KeyIndex = 0
            End Set
        End Property

        Public Property TableName As String = String.Empty

        Dim _updatestatement As String = String.Empty

        ''' <summary>
        ''' Gets the datasource-generated update statement.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UpdateStatement As String
            Get
                Return _updatestatement
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the custom update statement to execute when Execution property is set to Custom 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UpdateStatementCustom As String = String.Empty
#End Region

#Region "Sub New"
        ''' <summary>
        ''' Creates a new instance of QueryStatementInfo.
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            _connectionstring = Settings.ConnectionString
        End Sub

        ''' <summary>
        ''' Creates a new instance of QueryStatementInfo.
        ''' </summary>
        ''' <param name="connection">Determines the connection string to be used as .</param>
        ''' <remarks></remarks>
        Sub New(ByVal connection As String)
            _connectionstring = connection
        End Sub
#End Region

        Private Function IsAutoId(ByVal table As String, ByVal column As String) As Boolean
            Dim isauto As Boolean = False
            Dim db As String = ConnectionStringValue(_connectionstring, ConnectionDetailEnum.Database)

            Dim query As String = "SELECT" & vbNewLine & _
                                  "CASE WHEN cols.EXTRA LIKE 'auto_increment' THEN 1 ELSE 0 END AS Auto" & vbNewLine & _
                                  "FROM" & vbNewLine & _
                                  "information_schema.`COLUMNS` AS cols" & vbNewLine & _
                                  "WHERE" & vbNewLine & _
                                  "(cols.TABLE_SCHEMA LIKE '" & ToSqlValidString(db) & "') AND" & vbNewLine & _
                                  "(cols.TABLE_NAME LIKE '" & ToSqlValidString(table) & "') AND" & vbNewLine & _
                                  "(cols.COLUMN_NAME LIKE '" & ToSqlValidString(column) & "')"

            isauto = Que.GetValue(Of Boolean)(_connectionstring, query, False)

            Return isauto
        End Function

        Private Sub GenerateQueryStatements()
            If _datasource IsNot Nothing Then
                _insertstatement = String.Empty : _updatestatement = String.Empty

                Dim _fields1 As String = String.Empty
                Dim _fields2 As String = String.Empty
                Dim _parameters As String = String.Empty

                With _datasource

                    Dim table As String = String.Empty
                    If String.IsNullOrEmpty(TableName.Trim) Then : table = .TableName
                    Else : table = TableName
                    End If

                    _insertstatement = "INSERT INTO `" & ToSqlValidString(table) & "`" & vbNewLine
                    _updatestatement = "UPDATE `" & ToSqlValidString(table) & "` SET" & vbNewLine

                    For Each col As DataColumn In .Columns
                        If Not IsAutoId(table, col.ColumnName) Then
                            _fields1 &= IIf(Not String.IsNullOrEmpty(_fields1.Trim), ", ", String.Empty) & "`" & col.ColumnName & "`"
                            _fields2 &= IIf(Not String.IsNullOrEmpty(_fields2.Trim), ", ", String.Empty) & "`" & col.ColumnName & "` = '{" & col.ColumnName & "}'"
                            _parameters &= IIf(Not String.IsNullOrEmpty(_parameters.Trim), ", ", String.Empty) & IIf(col.DataType.Name.Contains("Bytes"), String.Empty, "'") & "{" & col.ColumnName & "}" & IIf(col.DataType.Name.Contains("Bytes"), String.Empty, "'")
                        End If
                    Next

                    _insertstatement &= "(" & _fields1 & ")" & vbNewLine &
                                        "VALUES" & vbNewLine &
                                       "(" & _parameters & ");" & vbNewLine

                    Dim pk As String = KeyName
                    If String.IsNullOrEmpty(KeyName.Trim) Then
                        If KeyIndex < 0 Then KeyIndex = 0
                        If KeyIndex >= 0 Then pk = _datasource.Columns(KeyIndex).ColumnName
                    End If

                    If .Rows.Count > 0 And
                       .Columns.Contains(pk) Then
                        Dim pkvalue As String = String.Empty

                        If Not IsDBNull(.Rows(0).Item(pk)) Then
                            Select Case .Columns(pk).DataType.Name
                                Case GetType(String).Name : pkvalue = ToSqlValidString(.Rows(0).Item(pk).ToString)
                                Case GetType(Decimal).Name, GetType(Double).Name,
                                     GetType(Single).Name, GetType(Byte).Name : pkvalue = ToSqlValidString(CDbl(.Rows(0).Item(pk)))
                                Case GetType(Date).Name : pkvalue = ToSqlValidString(CDate(.Rows(0).Item(pk)))
                                Case Else : pkvalue = .Rows(0).Item(pk).ToString
                            End Select
                        End If

                        _updatestatement &= _fields2 & vbNewLine &
                                        "WHERE" & vbNewLine &
                                        "(`" & pk & "` = '" & pkvalue & "');" & vbNewLine
                    Else
                        _updatestatement &= _fields2 & vbNewLine &
                                            "WHERE" & vbNewLine &
                                            "(`" & pk & "` = '{" & pk & "}');" & vbNewLine
                    End If

                End With
            End If
        End Sub
    End Class

    ''' <summary>
    ''' Required field information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RequiredFieldInfo
        ''' <summary>
        ''' Gets or sets field name clip separator string.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ClipSeparator As String = ","
        ''' <summary>
        ''' Gets or sets required field list of names.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property FieldNames As String = String.Empty
    End Class

    ''' <summary>
    ''' Data saving events arguments.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SavingEventArgs
        ''' <summary>
        ''' Determines whether to cancel to whole saving process or not.
        ''' </summary>
        ''' <remarks></remarks>
        Public Cancel As Boolean = False

        ''' <summary>
        ''' If not saved, states the encountered error.
        ''' </summary>
        ''' <remarks></remarks>
        Public ErrorMessage As String = String.Empty

        ''' <summary>
        ''' States the query statement that will be / has been executed.
        ''' </summary>
        ''' <remarks></remarks>
        Public QueryStatement As String = String.Empty

        ''' <summary>
        ''' Determines whether the data is saved or not.
        ''' </summary>
        ''' <remarks></remarks>
        Public Saved As Boolean = False
    End Class

    ''' <summary>
    ''' Data validation event arguments.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ValidationEventArgs
        ''' <summary>
        ''' Determines whether to cancel the whole saving process or not.
        ''' </summary>
        ''' <remarks></remarks>
        Public Cancel As Boolean = False
        ''' <summary>
        ''' Determines whether data have passed all necessary validations.
        ''' </summary>
        ''' <remarks></remarks>
        Public Valid As Boolean = True
        ''' <summary>
        ''' If not valid, states the last invalidated control.
        ''' </summary>
        ''' <remarks></remarks>
        Public ValidatedControl As Control = Nothing
    End Class
#End Region

#Region "Properties"
    Dim _connection As MySqlConnection = Nothing
    Dim _adapter As MySqlDataAdapter = Nothing
    Dim _transaction As MySqlTransaction = Nothing
    Dim _insertcommand As MySqlCommand = Nothing
    Dim _updatecommand As MySqlCommand = Nothing
    Dim _datatable As New DataTable
    Dim _controltable As New Hashtable
    Dim _requiredfieldmarker As New RequiredFieldMarker
    Dim _withimage As Boolean = False


    '@NME
    'Dim _HeaderBinderWhereClause As String = String.Empty
    'Public ReadOnly Property HeaderBinderWhereClause As String
    '    Get
    '        Return _HeaderBinderWhereClause
    '    End Get
    'End Property

    Public ReadOnly Property HeaderDataTable As DataTable
        Get
            Return _datatable
        End Get
    End Property

    ''' <summary>
    ''' Gets the PickListBinder's 'Back' button.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property BackButton As MetroFramework.Controls.MetroLink
        Get
            Return lnkCancel
        End Get
    End Property

    ''' <summary>
    ''' Gets the current form container of the PickListBinder.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ContainerForm As Form
        Get
            Return GetParentForm(Me)
        End Get
    End Property

    Dim _datasource As New DataSourceInfo

    ''' <summary>
    ''' Gets the current data source information of the PickListBinder.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public ReadOnly Property DataSource As DataSourceInfo
        Get
            With _datasource
                If .CommandText Is Nothing Then .CommandText = String.Empty
                If .ConnectionString Is Nothing Then .ConnectionString = String.Empty
            End With
            Return _datasource
        End Get
    End Property

    Dim _fields As New FieldCollection

    ''' <summary>
    ''' Gets the data source field collection.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public ReadOnly Property Fields As FieldCollection
        Get
            Return _fields
        End Get
    End Property

    Dim _isnew As Boolean = True

    ''' <summary>
    ''' Gets whether current state of creating new record or not.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public ReadOnly Property IsNew As Boolean
        Get
            Return _isnew
        End Get
    End Property

    ''' <summary>
    ''' Gets the PickListBinder's progress bar.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ProgressBar As MetroFramework.Controls.MetroProgressBar
        Get
            Return mprProgress
        End Get
    End Property

    Dim _querystatements As New QueryStatementInfo

    ''' <summary>
    ''' Gets the uery statement information of then PickListBinder.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public ReadOnly Property QueryStatements As QueryStatementInfo
        Get
            Return _querystatements
        End Get
    End Property

    Dim _parentformshown As Boolean = False

    ''' <summary>
    ''' Gets whether the current residing form is already shown or not.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public ReadOnly Property ParentFormShown As Boolean
        Get
            Return _parentformshown
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets how the print upon saving check box will be available to the user.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public Property PrintCheckVisibility As PrintCheckVisibilityEnum = PrintCheckVisibilityEnum.Invisible

    ''' <summary>
    ''' Gets whether the print upon saving checkbox is checked or not.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public ReadOnly Property PrintChecked As Boolean
        Get
            Return mcbPrint.Checked
        End Get
    End Property

    Dim _checkvisible As Boolean = False

    ''' <summary>
    ''' Gets or sets whether the print upon saving checkbox is visible or not.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PrintCheckVisible As Boolean
        Get
            Return _checkvisible
        End Get
        Set(ByVal value As Boolean)
            _checkvisible = value
            With mcbPrint
                .Enabled = _checkvisible : .Visible = _checkvisible
            End With
        End Set
    End Property

    Dim _requiredfields As New RequiredFieldInfo

    ''' <summary>
    ''' Gets the PickListBinder's required field information.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public ReadOnly Property RequiredFields As RequiredFieldInfo
        Get
            With _requiredfields
                If .ClipSeparator Is Nothing Then : .ClipSeparator = ","
                Else
                    If String.IsNullOrEmpty(.ClipSeparator.Trim) Then .ClipSeparator = ","
                End If

                If .FieldNames Is Nothing Then .FieldNames = String.Empty
            End With

            Return _requiredfields
        End Get
    End Property

    ''' <summary>
    ''' Gets the PickListBinder's 'Save' button.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SaveButton As MetroFramework.Controls.MetroLink
        Get
            Return lnkSave
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the status bar's text association (in the leftmost side).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StatusBarText As String
        Get
            Return RTrim(lblStatus.Text.Trim)
        End Get
        Set(ByVal value As String)
            lblStatus.Text = "  " & value
        End Set
    End Property

    Public Property TitleControl As String
        Get
            Return RTrim(lblTitle.Text.Trim)
        End Get
        Set(ByVal value As String)
            lblTitle.Text = value
        End Set
    End Property
#End Region

#Region "Methods"

    Private Sub PopulateDetails()
        If _connection Is Nothing Then _connection = New MySqlConnection(DataSource.ConnectionString)
        Dim _load As New DataLoadingEventArgs
        _load.QueryStatement = DataSource.CommandText : _fields.Clear()
        If _adapter IsNot Nothing Then
            _adapter.Dispose() : _adapter = Nothing
        End If

        If _datatable IsNot Nothing Then
            _datatable.Dispose() : _datatable = Nothing
        End If

        Try
            If _connection.State = ConnectionState.Closed Then _connection.Open()
            If _adapter Is Nothing Then _adapter = New MySqlDataAdapter(DataSource.CommandText, _connection)
            If _datatable Is Nothing Then _datatable = New DataTable
            _adapter.FillSchema(_datatable, SchemaType.Mapped)
            _adapter.Fill(_datatable) : _isnew = (_datatable.Rows.Count <= 0)
            _load.DataSource = _datatable

            Dim dtDisplay As DataTable = Nothing

            If Not String.IsNullOrEmpty(DataSource.ViewingCommandText.Trim) Then
                Dim dtView As New DataTable
                Dim _adpView As New MySqlDataAdapter(DataSource.ViewingCommandText, _connection)
                _adpView.Fill(dtView) : dtDisplay = dtView
            Else : dtDisplay = _datatable
            End If

            Dim _dc As DataColumn = _datatable.Columns(0)

            If (_datatable.Columns(0).DataType IsNot GetType(String) And _dc.AutoIncrement) And _datatable.Columns(0).DataType IsNot GetType(Date) Then
                With _dc
                    Dim max As Object = Nothing
                    If _datatable.Rows.Count > 0 Then
                        max = _datatable.Compute("MAX([" & _dc.ColumnName & "])", String.Empty)
                        If Not IsNumeric(max) Then max = 1
                    Else : max = 1
                    End If
                    .AutoIncrement = True : .AutoIncrementSeed = max : .AutoIncrementStep = +1
                End With

                If dtDisplay.Columns.Contains(_datatable.Columns(0).ColumnName) Then
                    _dc = dtDisplay.Columns(0)
                    With _dc
                        Dim max As Object = Nothing
                        If _datatable.Rows.Count > 0 Then
                            max = dtDisplay.Compute("MAX([" & _dc.ColumnName & "])", String.Empty)
                            If Not IsNumeric(max) Then max = 1
                        Else : max = 1
                        End If
                        .AutoIncrement = True : .AutoIncrementSeed = max : .AutoIncrementStep = +1
                    End With
                End If
            End If

            If dtDisplay.Rows.Count > 0 Then
                For Each col As DataColumn In dtDisplay.Columns
                    Dim ctrl As Control = GetControlByFieldName(col.ColumnName)
                    If ctrl IsNot Nothing Then
                        With dtDisplay.Rows(0)
                            _fields.Add(New FieldInfo(col.ColumnName, col.DataType, .Item(col.ColumnName)))
                            If col.DataType Is GetType(Date) Then
                                If IsDate(.Item(col.ColumnName)) Then
                                    If TypeOf ctrl Is DateTimeInput Then : CType(ctrl, DateTimeInput).Value = CDate(.Item(col.ColumnName))
                                    ElseIf TypeOf ctrl Is DateTimePicker Then : CType(ctrl, DateTimePicker).Value = CDate(.Item(col.ColumnName))
                                    ElseIf TypeOf ctrl Is MetroDateTime Then : CType(ctrl, MetroDateTime).Value = CDate(.Item(col.ColumnName))
                                    Else : ctrl.Text = Format(CDate(.Item(col.ColumnName)), "dd-MMM-yyyy")
                                    End If
                                Else
                                    If TypeOf ctrl Is DateTimeInput Then
                                        CType(ctrl, DateTimeInput).AllowEmptyState = True : CType(ctrl, DateTimeInput).IsEmpty = True
                                    ElseIf TypeOf ctrl Is DateTimePicker Then : CType(ctrl, DateTimePicker).Value = #1/1/1900#
                                    ElseIf TypeOf ctrl Is MetroDateTime Then : CType(ctrl, MetroDateTime).Value = #1/1/1900#
                                    Else : ctrl.Text = String.Empty
                                    End If
                                End If
                            Else
                                If col.DataType Is GetType(Int16) Or
                                   col.DataType Is GetType(Int32) Or
                                   col.DataType Is GetType(Int64) Or
                                   col.DataType Is GetType(Decimal) Or
                                   col.DataType Is GetType(SByte) Or
                                   IsNumeric(.Item(col.ColumnName)) Then
                                    If TypeOf ctrl Is CheckBoxX Then : CType(ctrl, CheckBoxX).Checked = CBool(.Item(col.ColumnName))
                                    ElseIf TypeOf ctrl Is CheckBox Then
                                        If IsDBNull(.Item(col.ColumnName)) Then
                                            CType(ctrl, CheckBox).Checked = False
                                        Else
                                            CType(ctrl, CheckBox).Checked = CBool(.Item(col.ColumnName))
                                        End If
                                    ElseIf TypeOf ctrl Is ComboBox Or
                                           TypeOf ctrl Is ComboBoxEx Or
                                           TypeOf ctrl Is C1Combo Or
                                           ctrl.GetType().BaseType Is GetType(C1DropDownControl) Then
                                        Dim combo As Object = ctrl
                                        If combo.DataSource IsNot Nothing Then
                                            Try
                                                combo.SelectedValue = .Item(col.ColumnName)
                                            Catch ex As Exception
                                            End Try

                                            If TypeOf ctrl Is ComboBox Or
                                               TypeOf ctrl Is ComboBoxEx Then
                                                Try
                                                    Dim dt As DataTable = combo.DataSource
                                                    Dim _display As String = combo.DisplayMember
                                                    If Not dt.Columns.Contains(_display) Then _display = dt.Columns(0).ColumnName
                                                    Dim _value As String = combo.ValueMember
                                                    If Not dt.Columns.Contains(_value) Then _value = dt.Columns(0).ColumnName
                                                    Dim dr() As DataRow = dt.Select("CONVERT(`" & _value & "`, System.String) = '" & ToSqlValidString(.Item(col.ColumnName).ToString, True) & "'")
                                                    If dr.Length > 0 Then combo.Text = dr(0).Item(_display)
                                                Catch ex As Exception
                                                End Try
                                            End If
                                        Else
                                            If Not IsDBNull(.Item(col.ColumnName)) Then
                                                ctrl.Text = .Item(col.ColumnName)
                                            Else
                                                ctrl.Text = ""
                                            End If
                                        End If

                                    ElseIf TypeOf ctrl Is NumericUpDown Or
                                           TypeOf ctrl Is IntegerInput Or
                                           TypeOf ctrl Is DoubleInput Then
                                        Dim control As Object = ctrl
                                        If IsDBNull(.Item(col.ColumnName)) Then : control.Value = 0
                                        Else : control.Value = .Item(col.ColumnName)
                                        End If
                                    ElseIf ctrl.GetType.Name = "SearchControl" Then
                                        Dim _ctl As Object = ctrl
                                        If Not IsDBNull(.Item(col.ColumnName)) Then : _ctl.Value = .Item(col.ColumnName)
                                        Else : _ctl.Value = ""
                                        End If
                                    Else
                                        If TypeOf ctrl Is MetroTextBox Then
                                            If col.MaxLength > -1 Then DirectCast(ctrl, MetroTextBox).MaxLength = col.MaxLength
                                        End If

                                        If TypeOf ctrl Is TextBoxX Then
                                            If col.MaxLength > -1 Then DirectCast(ctrl, TextBoxX).MaxLength = col.MaxLength
                                        End If

                                        If IsDBNull(.Item(col.ColumnName)) Then : ctrl.Text = String.Empty
                                        Else : ctrl.Text = .Item(col.ColumnName)
                                        End If
                                    End If
                                Else
                                    If TypeOf ctrl Is PictureBox Then
                                        If Not IsDBNull(.Item(col.ColumnName)) Then CType(ctrl, PictureBox).Image = ByteArrayToImage(.Item(col.ColumnName))

                                    ElseIf TypeOf ctrl Is ComboBox Or
                                           TypeOf ctrl Is ComboBoxEx Or
                                           TypeOf ctrl Is C1Combo Or
                                           ctrl.GetType().BaseType Is GetType(C1DropDownControl) Then
                                        If IsDBNull(.Item(col.ColumnName)) Then : ctrl.Text = String.Empty
                                        Else
                                            Dim combo As Object = ctrl
                                            If combo.DataSource IsNot Nothing Then : combo.SelectedValue = .Item(col.ColumnName)
                                            Else
                                                If Not IsDBNull(.Item(col.ColumnName)) Then : ctrl.Text = .Item(col.ColumnName)
                                                Else : ctrl.Text = String.Empty
                                                End If
                                            End If
                                        End If
                                    ElseIf ctrl.GetType.Name = "SearchControl" Then
                                        Dim _ctl As Object = ctrl
                                        If Not IsDBNull(.Item(col.ColumnName)) Then : _ctl.Value = .Item(col.ColumnName)
                                        Else : _ctl.Value = ""
                                        End If
                                    Else
                                        If TypeOf ctrl Is MetroTextBox Then
                                            If col.MaxLength > -1 Then DirectCast(ctrl, MetroTextBox).MaxLength = col.MaxLength
                                        End If

                                        If TypeOf ctrl Is TextBoxX Then
                                            If col.MaxLength > -1 Then DirectCast(ctrl, TextBoxX).MaxLength = col.MaxLength
                                        End If

                                        If IsDBNull(.Item(col.ColumnName)) Then : ctrl.Text = String.Empty
                                        Else : ctrl.Text = .Item(col.ColumnName)
                                        End If
                                    End If
                                End If
                            End If
                        End With
                    End If

                    If Not _fields.Contains(col.ColumnName) Then _fields.Add(New FieldInfo(col.ColumnName, col.DataType, dtDisplay.Rows(0).Item(col.ColumnName)))
                Next
            Else
                For Each col As DataColumn In dtDisplay.Columns
                    Dim ctrl As Control = GetControlByFieldName(col.ColumnName)

                    If Not IsNothing(ctrl) Then
                        If TypeOf ctrl Is MetroTextBox Then
                            If col.MaxLength > -1 Then DirectCast(ctrl, MetroTextBox).MaxLength = col.MaxLength
                        End If

                        If TypeOf ctrl Is TextBoxX Then
                            If col.MaxLength > -1 Then DirectCast(ctrl, TextBoxX).MaxLength = col.MaxLength
                        End If
                    End If

                    Dim value As Object = Nothing
                    Select Case col.DataType.Name
                        Case GetType(String).Name : value = String.Empty
                        Case GetType(Date).Name : value = Now.Date
                        Case GetType(Boolean).Name, GetType(Integer).Name,
                             GetType(Long).Name, GetType(Double).Name,
                             GetType(Single).Name, GetType(Decimal).Name,
                             GetType(Byte).Name, GetType(SByte).Name : value = DBNull.Value
                        Case Else : value = Nothing
                    End Select
                    _fields.Add(New FieldInfo(col.ColumnName, col.DataType, value))
                Next
            End If

            _querystatements.DataSource = _datatable

            Dim _commandbuilder As New MySqlCommandBuilder(_adapter)

            If _insertcommand Is Nothing Then _insertcommand = New MySqlCommand
            _insertcommand = _commandbuilder.GetInsertCommand

            If _updatecommand Is Nothing Then _updatecommand = New MySqlCommand
            _updatecommand = _commandbuilder.GetUpdateCommand

            _commandbuilder.Dispose()

            Dim keyname As String = _querystatements.KeyName
            If Not _datatable.Columns.Contains(keyname) Then keyname = _datatable.Columns(0).ColumnName

            If _datatable.Rows.Count > 0 Then
                Dim update As String = _updatecommand.CommandText.Substring(0, _updatecommand.CommandText.IndexOf("WHERE"))
                Dim _PKValue As String = _datatable.Rows(0).Item(keyname).ToString
                If _datatable.Rows(0).Item(keyname).GetType.ToString = "System.DateTime" Then
                    _PKValue = Format(CDate(_datatable.Rows(0).Item(keyname).ToString), "yyyy-MM-dd")
                End If

                _updatecommand.CommandText = update & " WHERE (`" & keyname & "` = '" & ToSqlValidString(_PKValue) & "')"
                '@NME 02062013
                'To get the where clause of command text on update
                '_HeaderBinderWhereClause = " WHERE (`" & keyname & "` = '" & ToSqlValidString(_PKValue) & "')"
            End If

            If _datatable.TableName = "" Then
                Dim sSQLFilter() As String = Microsoft.VisualBasic.Split(_insertcommand.CommandText, " ")
                If Not sSQLFilter(2).Contains("`") Then
                    sSQLFilter(2) = "`" & sSQLFilter(2) & "`"
                End If
                _datatable.TableName = sSQLFilter(2)
            End If

            If _datatable.Columns(0).DataType IsNot GetType(String) And _datatable.Columns(0).AutoIncrement Then _insertcommand.CommandText &= "; SELECT * FROM " & _datatable.TableName & " WHERE " & keyname & " = LAST_INSERT_ID()"

        Catch ex As Exception
            _load.ErrorMessage = ex.Message
        Finally
            If _connection.State = ConnectionState.Open Then _connection.Close()
        End Try

        RaiseEvent AfterDataLoading(Me, _load)
    End Sub

    ''' <summary>
    ''' Calls and triggers the save button's event.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RaiseSaveClick()
        lnkSave_Click(lnkSave, Nothing)
    End Sub

    Private DataSetCollection As New DataSet
    Private Function IsNullOrEmpty(ByVal obj As Object) As Boolean
        Dim bReturn As Boolean

        If IsDBNull(obj) Then
            bReturn = True
        ElseIf obj Is Nothing Then
            bReturn = True
        Else
            If obj.GetType.ToString = "System.String" Then
                If obj = "" Then
                    bReturn = True
                End If
            End If
        End If

        Return bReturn
    End Function

    Private Function IfNullOrEmpty(ByVal obj1 As Object, ByVal obj2 As Object) As Object
        Dim oReturn As Object

        If IsNullOrEmpty(obj1) Then
            oReturn = obj2
        Else
            oReturn = obj1
        End If

        Return oReturn
    End Function

    Private Function GetDefaultNullValue(ByVal tmpObj As Object, ByVal tmpObjType As Object) As Object
        Dim tmpResult As Object
        Try
            If tmpObjType.ToString.ToLower.Contains("string") Then
                If IfNullOrEmpty(tmpObj, String.Empty) <> String.Empty Then : tmpResult = IfNullOrEmpty(tmpObj, String.Empty)
                Else : tmpResult = String.Empty
                End If
            ElseIf tmpObjType.ToString.ToLower.Contains("date") Then
                If tmpObj IsNot DBNull.Value Then : tmpResult = tmpObj
                Else : tmpResult = DBNull.Value
                End If
            ElseIf tmpObjType.ToString.ToLower.Contains("int") _
                            Or tmpObjType.ToString.ToLower.Contains("long") _
                            Or tmpObjType.ToString.ToLower.Contains("single") _
                            Or tmpObjType.ToString.ToLower.Contains("double") _
                            Or tmpObjType.ToString.ToLower.Contains("decimal") _
                            Or tmpObjType.ToString.ToLower.Contains("bool") Then

                If Val(IfNullOrEmpty(tmpObj, 0)) <> 0 Then
                    Select Case tmpObj.GetType.Name
                        Case "Boolean" : tmpResult = 1
                        Case Else : tmpResult = Val(IfNullOrEmpty(tmpObj, 0))
                    End Select

                Else : tmpResult = 0
                End If
            Else
                If tmpObj IsNot DBNull.Value Then : tmpResult = tmpObj
                Else : tmpResult = String.Empty
                End If
            End If

            Return tmpResult
        Catch ex As Exception
#If DEBUG Then
            ErrorMessenger(ex.Message)
#End If
            Return DBNull.Value
        End Try
    End Function

    Private Function GetLogAction() As String
        Dim _Changes As String = String.Empty
        For Each dttable As DataTable In DataSetCollection.Tables
            If dttable.TableName = "Header" Then
                Dim dr As DataRow = dttable.Rows(0)
                If dr.RowState = DataRowState.Modified Then
                    For Each col As DataColumn In dttable.Columns
                        If dr.Item(col.ColumnName, DataRowVersion.Original).ToString() <> dr.Item(col.ColumnName).ToString() Then
                            _Changes &= col.ColumnName & " [" & dr.Item(col.ColumnName, DataRowVersion.Original).ToString() & "] - > [" & dr.Item(col.ColumnName).ToString() & "]" & vbNewLine
                        End If
                    Next
                End If
            Else
                For Each dr As DataRow In dttable.Rows
                    Dim _logs As String = ""
                    If dr.RowState = DataRowState.Added Then
                        For Each col As DataColumn In dttable.Columns
                            _logs &= vbNewLine & " [" & col.ColumnName & "- " & dr.Item(col.ColumnName).ToString() & "] "
                        Next
                        _Changes &= "Added row for Detail Table [" & dttable.TableName & "]-> " & _logs & "" & vbNewLine
                    ElseIf dr.RowState = DataRowState.Modified Then
                        For Each col As DataColumn In dttable.Columns
                            _logs &= vbNewLine & " [" & col.ColumnName & "- " & dr.Item(col.ColumnName).ToString() & "] "
                        Next
                        _Changes &= "Modified row for Detail Table [" & dttable.TableName & "]-> " & _logs & "" & vbNewLine
                    ElseIf dr.RowState = DataRowState.Deleted Then
                        For Each col As DataColumn In dttable.Columns
                            _logs &= vbNewLine & " [" & col.ColumnName & "- " & dr.Item(col.ColumnName, DataRowVersion.Original).ToString() & "] " & vbNewLine
                        Next
                        _Changes &= "Modified row for Detail Table [" & dttable.TableName & "]-> " & _logs & "" & vbNewLine
                    End If
                Next
            End If
        Next
        Return _Changes
    End Function

    Public Function ProcessGetLogAction() As String
        Try
            If Me.IsNew Then Return String.Empty
            Dim delLog As New Func(Of PickListBinder, String)(AddressOf GetLogAction)
            Dim _logasync As IAsyncResult = delLog.BeginInvoke(Me, Nothing, delLog)
            WaitToFinish(_logasync)
            DataSetCollection.Dispose()
            Return delLog.EndInvoke(_logasync)
        Catch ex As Exception
            DataSetCollection.Dispose()
            Return String.Empty
        End Try
    End Function

    Private Sub GetCurrentTable()
        DataSetCollection.Tables.Add(_datatable)
        DataSetCollection.Tables(0).TableName = "Header"
        For Each di As DetailDataSourceInfo In DataSource.Details
            DataSetCollection.Tables.Add(di.DetailTable)
        Next
    End Sub

    ''' <summary>
    ''' Reloads and reinitialize binding values.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ReloadBindings()
        If Not Validators.Contains(ParentForm) Then Validators.Add(ParentForm)
        RaiseEvent BeforeFormShown(ContainerForm, Nothing)

        CheckForIllegalCrossThreadCalls = False
        Dim enabled As Boolean = lnkSave.Enabled
        EnableFields(ContainerForm, False) : lnkSave.Enabled = False

        With mprProgress
            .Value = 0 : .Show() : .BringToFront()
        End With

        If _connection Is Nothing Then _connection = New MySqlConnection(DataSource.ConnectionString)

        Dim thrd As New Thread(AddressOf PopulateDetails)
        thrd.Start() : WaitToFinish(thrd, mprProgress)

        Dim imgs As New List(Of PictureBox) : imgs.Clear()

        If DataSource.Details.Count > 0 Then
            For Each di As DetailDataSourceInfo In DataSource.Details
                di.Connection = _connection
                Dim arPattern As IAsyncResult = di.ReloadPatternTableAsync(DataSource.ConnectionString)
                Dim arViewing As IAsyncResult = di.ReloadViewingPaneAsync(DataSource.ConnectionString)

                Dim ev As New DataLoadingEventArgs
                di.EventArgument = ev

                While Not arPattern.IsCompleted Or
                      Not arViewing.IsCompleted
                    If mprProgress.Value < (mprProgress.Maximum - 10) Then mprProgress.Value += 1
                    Thread.Sleep(1) : Application.DoEvents()
                End While

                If di.ViewingGrid IsNot Nothing Then
                    If di.ViewingGrid.DataSource IsNot Nothing Then CType(di.ViewingGrid.DataSource, DataTable).TableName = di.DetailTable.TableName
                End If

                RaiseEvent AfterDetailDataLoading(Me, ev)

                If di.LoadingImageBox IsNot Nothing Then imgs.Add(di.LoadingImageBox)

                If di.ViewingGrid IsNot Nothing Then
                    With di.ViewingGrid
                        .ExtendLastCol = True : Simple.Redraw(di.ViewingGrid)
                    End With
                End If
            Next
        End If

        EndProgress(mprProgress)

        For Each pb As PictureBox In imgs
            pb.Hide()
        Next

        Dim delenable As New Action(Of Form)(AddressOf EnableFields)
        Dim arenable As IAsyncResult = delenable.BeginInvoke(ContainerForm, Nothing, delenable)
        WaitToFinish(arenable)

        With lnkSave
            .Enabled = enabled : .Visible = enabled
        End With
        SetEditingEvents(ContainerForm)
        With mprProgress
            .Value = 0 : .Maximum = 80 : .Hide()
        End With

        RaiseEvent AfterDataLoading(ContainerForm, Nothing)
        RaiseEvent AfterFormShown(ContainerForm, Nothing)

        SetRequiredFields() : GetCurrentTable()
        Me.TitleControl = Me.TitleControl.Replace("*", "")
    End Sub

    Private Sub SetDatasourceValues()
        Dim dr As DataRow = Nothing
        If _isnew Then : dr = _datatable.NewRow
        Else : dr = _datatable.Rows(0)
        End If

        For Each col As DataColumn In _datatable.Columns
            Dim value As Object = GetFieldValue(Of Object)(col.ColumnName, True)
            If value Is Nothing Or
               IsDBNull(value) Then
                Select Case col.DataType.Name
                    'Case GetType(String).Name : value = String.Empty
                    Case GetType(Boolean).Name : value = False
                    Case GetType(Integer).Name, GetType(Long).Name,
                         GetType(Double).Name, GetType(Single).Name,
                         GetType(Decimal).Name, GetType(Byte).Name : value = DBNull.Value
                    Case GetType(Date).Name : value = DBNull.Value
                    Case Else : value = DBNull.Value
                End Select

                dr.Item(col.ColumnName) = value
            Else
                Select Case col.DataType.Name
                    Case GetType(Integer).Name, GetType(Long).Name,
                         GetType(Double).Name, GetType(Single).Name,
                         GetType(Decimal).Name, GetType(Byte).Name
                        If Not IsNumeric(value) Then value = DBNull.Value
                End Select

                dr.Item(col.ColumnName) = value
            End If
        Next

        dr.EndEdit()
        If _isnew Then _datatable.Rows.Add(dr)
    End Sub

    Private Sub SetEditingEvents(ByVal container As Object)
        For Each ctrl As Control In container.Controls
            Select Case ctrl.GetType.Name
                Case GetType(TextBox).Name, GetType(TextBoxX).Name,
                     GetType(ComboBox).Name, GetType(ComboBoxEx).Name,
                     GetType(C1Combo).Name, GetType(RichTextBox).Name,
                     GetType(NumericUpDown).Name, GetType(DateTimePicker).Name,
                     GetType(MetroDateTime).Name,
                     GetType(MetroTextBox).Name

                    AddHandler ctrl.TextChanged, AddressOf Fields_TextChanged
                Case GetType(CheckBox).Name : AddHandler CType(ctrl, CheckBox).CheckedChanged, AddressOf Fields_TextChanged
                Case GetType(CheckBoxX).Name : AddHandler CType(ctrl, CheckBoxX).CheckedChanged, AddressOf Fields_TextChanged
                Case GetType(DoubleInput).Name
                    AddHandler ctrl.TextChanged, AddressOf Fields_TextChanged
                    AddHandler CType(ctrl, DoubleInput).LockUpdateChanged, AddressOf Fields_TextChanged

                Case GetType(IntegerInput).Name
                    AddHandler ctrl.TextChanged, AddressOf Fields_TextChanged
                    AddHandler CType(ctrl, IntegerInput).LockUpdateChanged, AddressOf Fields_TextChanged

                Case GetType(DateTimeInput).Name
                    AddHandler ctrl.TextChanged, AddressOf Fields_TextChanged
                    AddHandler CType(ctrl, DateTimeInput).LockUpdateChanged, AddressOf Fields_TextChanged

                Case Else
                    Select Case ctrl.GetType().BaseType.Name
                        Case GetType(TextBox).Name, GetType(ComboBox).Name, GetType(C1DropDownControl).Name : AddHandler ctrl.TextChanged, AddressOf Fields_TextChanged
                        Case Else
                            If ctrl.Controls.Count > 0 Then SetEditingEvents(ctrl)
                    End Select
            End Select
        Next
    End Sub

    Dim _seltab As Object = Nothing
    Private Sub SetControlFocus(ByVal ctrl As Control)
        If ctrl.Parent IsNot Nothing Then
            Select Case ctrl.Parent.GetType.Name
                Case GetType(MetroTabPage).Name
                    CType(ctrl.Parent.Parent, MetroTabControl).SelectedTab = ctrl.Parent
                Case GetType(TabControl).Name
                    If _seltab IsNot Nothing Then CType(ctrl.Parent, TabControl).SelectedTab = _seltab
                    _seltab = Nothing
                    If ctrl.Parent.Parent IsNot Nothing Then SetControlFocus(ctrl.Parent)

                Case GetType(SuperTabControl).Name
                    If _seltab IsNot Nothing Then CType(ctrl.Parent, SuperTabControl).SelectedTab = _seltab
                    _seltab = Nothing
                    If ctrl.Parent.Parent IsNot Nothing Then SetControlFocus(ctrl.Parent)

                Case GetType(DevComponents.DotNetBar.TabControl).Name
                    If _seltab IsNot Nothing Then CType(ctrl.Parent, DevComponents.DotNetBar.TabControl).SelectedTab = _seltab
                    _seltab = Nothing
                    If ctrl.Parent.Parent IsNot Nothing Then SetControlFocus(ctrl.Parent)

                Case GetType(ExpandablePanel).Name : CType(ctrl.Parent, ExpandablePanel).Expanded = True
                Case GetType(TabControlPanel).Name
                    _seltab = CType(ctrl.Parent, TabControlPanel).TabItem : SetControlFocus(ctrl.Parent)
                Case GetType(DevComponents.DotNetBar.TabControlPanel).Name
                    _seltab = CType(ctrl.Parent, DevComponents.DotNetBar.TabControlPanel).TabItem : SetControlFocus(ctrl.Parent)
                Case GetType(SuperTabControlPanel).Name
                    _seltab = CType(ctrl.Parent, SuperTabControlPanel).TabItem : SetControlFocus(ctrl.Parent)
                Case Else
                    SetControlFocus(ctrl.Parent)
            End Select
        End If
        ctrl.Focus()
    End Sub

    ''' <summary>
    ''' Sets the datasource field name for the given control.
    ''' </summary>
    ''' <param name="ctrl">Binded control.</param>
    ''' <param name="name">Field name.</param>
    ''' <remarks></remarks>
    <Description("Associated field name from the generated data source.")>
    Public Sub SetFieldName(ByVal ctrl As Control, ByVal name As String)
        If _controltable.ContainsKey(ctrl) Then : _controltable(ctrl) = name
        Else : _controltable.Add(ctrl, name)
        End If
    End Sub

    Private Sub SetRequiredFields()
        Dim requiredfieldnames() As String = RequiredFields.FieldNames.Split(RequiredFields.ClipSeparator)
        For Each field As String In requiredfieldnames
            field = RTrim(field.Trim)
            If Not String.IsNullOrEmpty(field.Trim) Then
                Dim ctrl As Control = GetControlByFieldName(field)
                If ctrl IsNot Nothing Then _requiredfieldmarker.SetRequired(ctrl, True)
            End If
        Next
    End Sub
    Public Sub ShowProgress(ByVal Current As Long, ByVal total As Long, Optional ByVal sText As String = "")
        ProgressBar.Value = (Current / total) * 100
        If ProgressBar.Value = mprProgress.Maximum Then
            ProgressBar.Visible = False
            lblStatus.Text = ""
        Else
            ProgressBar.Text = ProgressBar.Value & " %"
            ProgressBar.Visible = True
            lblStatus.Text = sText
        End If
        Application.DoEvents()
    End Sub
#End Region

#Region "Functions"
    ''' <summary>
    ''' Validates whether control is supported or not.
    ''' </summary>
    ''' <param name="extendee"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CanExtend(ByVal extendee As Object) As Boolean Implements System.ComponentModel.IExtenderProvider.CanExtend
        Dim issupported As Boolean = False

        Select Case extendee.GetType.Name
            Case GetType(TextBoxX).Name, GetType(TextBox).Name, GetType(PictureBox).Name,
                 GetType(ComboBoxEx).Name, GetType(ComboBox).Name,
                 GetType(C1Combo).Name, GetType(ComboTree).Name,
                 GetType(LabelX).Name, GetType(Label).Name,
                 GetType(RichTextBox).Name, GetType(CheckBoxX).Name,
                 GetType(CheckBox).Name, GetType(DateTimeInput).Name,
                 GetType(DateTimePicker).Name, GetType(NumericUpDown).Name,
                 GetType(DoubleInput).Name, GetType(IntegerInput).Name,
                 GetType(MetroTextBox).Name, "SearchControl", GetType(MetroDateTime).Name,
                 GetType(MetroTabControl).Name
                issupported = True
            Case Else
                Select Case extendee.GetType.BaseType.Name
                    Case GetType(TextBox).Name, GetType(ComboBox).Name, GetType(PictureBox).Name,
                          GetType(CheckBox).Name, GetType(RichTextBox).Name, GetType(C1DropDownControl).Name,
                          GetType(NumericUpDown).Name, GetType(DateTimePicker).Name, GetType(MetroDateTime).Name : issupported = True
                    Case Else
                End Select
        End Select

        Return issupported
    End Function

    Private Function GetControlByAccesibleDescription(ByVal name As String, ByVal container As Object) As Control
        Dim ctrl As Control = Nothing

        For Each c As Control In container.Controls
            If c.AccessibleDescription = name Then
                ctrl = c : Exit For
            Else
                If c.Controls.Count > 0 Then
                    ctrl = GetControlByAccesibleDescription(name, c) : Exit For
                End If
            End If
        Next

        Return ctrl
    End Function

    Public Function GetControlByFieldName(ByVal name As String) As Control
        Dim ctrl As Control = Nothing

        For Each c As Control In _controltable.Keys
            If _controltable(c) = name Then
                ctrl = c : Exit For
            End If
        Next

        Return ctrl
    End Function

    Private Function GetParentForm(ByVal control As Object) As Form
        Dim frm As Form = Nothing

        If control.FindForm() IsNot Nothing Then : frm = control.FindForm()
        Else
            If control.Parent IsNot Nothing Then
                frm = GetParentForm(control.Parent)
            End If
        End If

        Return frm
    End Function

    ''' <summary>
    ''' Gets the field name binded to the specified control.
    ''' </summary>
    ''' <param name="ctrl">Binded control.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("Associated field name from the generated data source.")>
    Public Function GetFieldName(ByVal ctrl As Control) As String
        If _controltable.ContainsKey(ctrl) Then : Return _controltable(ctrl)
        Else : Return String.Empty
        End If
    End Function

    Private Overloads Function GetFieldValue(Of TResult)(ByVal name As String) As TResult
        Dim value As TResult : Dim defaultvalue As Object = Nothing

        Select Case _datatable.Columns(name).DataType.Name
            Case GetType(Integer).Name, GetType(Long).Name, GetType(Decimal).Name,
                GetType(Double).Name, GetType(Single).Name,
                GetType(Byte).Name : defaultvalue = DBNull.Value
            Case GetType(String).Name : defaultvalue = String.Empty
            Case GetType(Date).Name : defaultvalue = Now.Date
            Case GetType(Boolean).Name : defaultvalue = False
            Case Else
                If _datatable.Columns(name).DataType.Name.Contains("Bytes") Then defaultvalue = String.Empty
        End Select

        value = defaultvalue

        Dim ctrl As Object = GetControlByFieldName(name)

        If ctrl IsNot Nothing Then
            Select Case ctrl.GetType().Name
                Case GetType(TextBox).Name, GetType(TextBoxX).Name, GetType(MetroTextBox).Name,
                    GetType(RichTextBox).Name : value = ctrl.Text
                Case GetType(CheckBox).Name, GetType(CheckBoxX).Name : value = IIf(ctrl.Checked, 1, 0)
                Case GetType(DateTimeInput).Name, GetType(DateTimePicker).Name, "SearchControl",
                    GetType(MetroDateTime).Name
                    value = ctrl.Value
                Case GetType(NumericUpDown).Name, GetType(IntegerInput).Name,
                     GetType(DoubleInput).Name : value = ctrl.Value
                Case GetType(ComboBox).Name, GetType(ComboBoxEx).Name, GetType(C1Combo).Name
                    If ctrl.DataSource IsNot Nothing And
                       Not String.IsNullOrEmpty(ctrl.ValueMember) Then
                        If ctrl.SelectedValue Is Nothing Or
                           IsDBNull(ctrl.SelectedValue) Then
                            Dim nullvalue As Object = DBNull.Value
                            value = nullvalue
                        Else : value = ctrl.SelectedValue
                        End If
                    Else : value = ctrl.Text
                    End If
                Case GetType(PictureBox).Name
                    Dim hex As Object = "x'" & ImageToHexaDecimalString(ctrl.Image) & "'" : value = hex
                Case Else
                    Select Case ctrl.GetType().BaseType.Name
                        Case GetType(TextBox).Name : value = ctrl.Text
                        Case GetType(ComboBox).Name, GetType(C1DropDownControl).Name
                            If ctrl.DataSource IsNot Nothing And
                               Not String.IsNullOrEmpty(ctrl.ValueMember) Then
                                If ctrl.SelectedValue Is Nothing Or
                                   IsDBNull(ctrl.SelectedValue) Then
                                    Dim nullvalue As Object = DBNull.Value
                                    value = nullvalue
                                Else : value = ctrl.SelectedValue
                                End If
                            Else : value = ctrl.Text
                            End If
                        Case Else
                    End Select
            End Select
        Else
            Dim fi As FieldInfo = _fields(name)
            If fi IsNot Nothing Then value = fi.FieldValue
        End If

        Return value
    End Function

    Private Overloads Function GetFieldValue(Of TResult)(ByVal name As String, ByVal directvalue As Boolean) As TResult
        If Not directvalue Then
            Return GetFieldValue(Of TResult)(name) : Exit Function
        End If

        Dim value As TResult : Dim defaultvalue As Object = Nothing

        Select Case _datatable.Columns(name).DataType.Name
            Case GetType(Integer).Name, GetType(Long).Name, GetType(Decimal).Name,
                GetType(Double).Name, GetType(Single).Name,
                GetType(Byte).Name, GetType(SByte).Name : defaultvalue = DBNull.Value

            Case GetType(String).Name : defaultvalue = String.Empty
            Case GetType(Date).Name : defaultvalue = Now.Date
            Case GetType(Boolean).Name : defaultvalue = False
            Case Else
                If _datatable.Columns(name).DataType.Name.Contains("Bytes") Then defaultvalue = DBNull.Value
        End Select

        value = defaultvalue

        Dim ctrl As Object = GetControlByFieldName(name)

        If ctrl IsNot Nothing Then
            Select Case ctrl.GetType().Name
                Case GetType(TextBox).Name, GetType(TextBoxX).Name, GetType(MetroTextBox).Name,
                    GetType(RichTextBox).Name : value = ctrl.Text
                Case GetType(CheckBox).Name, GetType(CheckBoxX).Name, GetType(MetroCheckBox).Name
                    value = IIf(ctrl.Checked, 1, 0)
                Case GetType(DateTimeInput).Name, GetType(DateTimePicker).Name
                    If ctrl.isempty Then
                        defaultvalue = DBNull.Value
                        value = defaultvalue
                    Else : value = ctrl.Value
                    End If
                Case GetType(MetroDateTime).Name
                    If DirectCast(ctrl, MetroDateTime).Value = #1/1/1900# Then
                        defaultvalue = DBNull.Value
                        value = defaultvalue
                    Else
                        value = ctrl.Value
                    End If
                Case "SearchControl"
                    If String.IsNullOrEmpty(ctrl.Value) Then
                        defaultvalue = DBNull.Value
                        value = defaultvalue
                    Else
                        value = ctrl.Value
                    End If

                Case GetType(NumericUpDown).Name, GetType(IntegerInput).Name,
                        GetType(DoubleInput).Name : value = ctrl.Value
                Case GetType(ComboBox).Name, GetType(ComboBoxEx).Name, GetType(C1Combo).Name
                    If ctrl.DataSource IsNot Nothing And
                       Not String.IsNullOrEmpty(ctrl.ValueMember) Then
                        If Not String.IsNullOrEmpty(ctrl.SelectedValue) Then : value = ctrl.SelectedValue
                        Else
                            defaultvalue = DBNull.Value : value = defaultvalue
                        End If
                    Else
                        If Not String.IsNullOrEmpty(ctrl.Text) Then : value = ctrl.Text
                        Else
                            defaultvalue = DBNull.Value : value = defaultvalue
                        End If
                    End If
                Case GetType(PictureBox).Name
                    Dim bytes As Object = ImageToByteArray(CType(ctrl, PictureBox).Image)
                    value = bytes
                Case Else
                    Select Case ctrl.GetType().BaseType.Name
                        Case GetType(TextBox).Name : value = ctrl.Text
                        Case GetType(ComboBox).Name, GetType(C1DropDownControl).Name
                            If ctrl.DataSource IsNot Nothing And
                               Not String.IsNullOrEmpty(ctrl.ValueMember) Then
                                If ctrl.SelectedValue Is Nothing Or
                                   IsDBNull(ctrl.SelectedValue) Then
                                    Dim nullvalue As Object = DBNull.Value
                                    value = nullvalue
                                Else : value = ctrl.SelectedValue
                                End If
                            Else : value = ctrl.Text
                            End If
                        Case Else
                    End Select
            End Select
        Else
            Dim fi As FieldInfo = _fields(name)
            If fi IsNot Nothing Then value = fi.FieldValue
        End If

        Return value
    End Function

    Private Overloads Function GetFieldValue(ByVal name As String, ByVal datasourcetable As DataTable, ByVal currentrow As DataRow) As Object
        Dim value As Object = Nothing : Dim defaultvalue As Object = Nothing

        Select Case datasourcetable.Columns(name).DataType.Name
            Case GetType(Integer).Name, GetType(Long).Name, GetType(Decimal).Name,
                GetType(Double).Name, GetType(Single).Name,
                GetType(Byte).Name, GetType(SByte).Name : defaultvalue = DBNull.Value
            Case GetType(String).Name : defaultvalue = String.Empty
            Case GetType(Date).Name : defaultvalue = Now.Date
            Case GetType(Boolean).Name : defaultvalue = False
            Case Else
                If datasourcetable.Columns(name).DataType.Name.Contains("Bytes") Then defaultvalue = String.Empty
        End Select

        value = defaultvalue
        Dim actualvalue As Object = Nothing

        If datasourcetable.Columns.Contains(name) Then
            If currentrow.Item(name) IsNot Nothing And
                Not IsDBNull(currentrow.Item(name)) Then
                Select Case datasourcetable.Columns(name).DataType.Name
                    Case GetType(Integer).Name, GetType(Long).Name : actualvalue = CInt(currentrow.Item(name))
                    Case GetType(Decimal).Name,
                         GetType(Double).Name, GetType(Single).Name,
                         GetType(Byte).Name, GetType(SByte).Name : actualvalue = ToSqlValidString(CDbl(currentrow.Item(name)), 4)
                    Case GetType(String).Name : actualvalue = ToSqlValidString(currentrow.Item(name).ToString)
                    Case GetType(Date).Name : actualvalue = ToSqlValidString(CDate(currentrow.Item(name)))
                    Case GetType(Boolean).Name : actualvalue = IIf(CBool(currentrow.Item(name)), 1, 0)
                    Case Else
                        If datasourcetable.Columns(name).DataType.Name.Contains("Bytes") Then
                            actualvalue = "x'" & ByteArrayToHexaDecimalString(currentrow.Item(name)) & "'"
                            _withimage = True
                        End If
                End Select
                value = actualvalue
            End If
        End If

        Return value
    End Function

    Private Function GetQueryString() As String
        Dim query As New StringBuilder
        Dim keyname As String = String.Empty

        With QueryStatements
            If .DataSource IsNot Nothing Then
                If IsNew Then
                    Select Case .Execution
                        Case QueryGenerationEnum.Auto : query.Append(.InsertStatement)
                        Case QueryGenerationEnum.Custom : query.Append(.InsertStatementCustom)
                        Case Else
                    End Select
                Else
                    Select Case .Execution
                        Case QueryGenerationEnum.Auto : query.Append(.UpdateStatement)
                        Case QueryGenerationEnum.Custom : query.Append(.UpdateStatementCustom)
                        Case Else
                    End Select
                End If

                If String.IsNullOrEmpty(.KeyName.Trim) Then : keyname = .DataSource.Columns(.KeyIndex).ColumnName
                Else : keyname = .KeyName
                End If
            End If
        End With

        _withimage = False

        For Each col As DataColumn In _datatable.Columns
            Dim value As String = String.Empty

            Dim ctrl As Control = GetControlByFieldName(col.ColumnName)
            If ctrl IsNot Nothing Then _withimage = _withimage Or (TypeOf ctrl Is PictureBox)

            Dim fieldvalue As Object = GetFieldValue(Of Object)(col.ColumnName)
            If fieldvalue IsNot Nothing Then
                Select Case col.DataType.Name
                    Case GetType(String).Name : value = ToSqlValidString(fieldvalue.ToString)
                    Case GetType(Decimal).Name, GetType(Double).Name,
                         GetType(Single).Name, GetType(Byte).Name : value = ToSqlValidString(CDbl(fieldvalue))
                    Case GetType(Date).Name : value = ToSqlValidString(CDate(fieldvalue))
                    Case Else : value = fieldvalue.ToString
                End Select
            Else
                If _datatable.Rows.Count > 0 Then
                    Select Case col.DataType.Name
                        Case GetType(String).Name : value = ToSqlValidString(_datatable.Rows(0).Item(col.ColumnName).ToString)
                        Case GetType(Decimal).Name, GetType(Double).Name,
                             GetType(Single).Name, GetType(Byte).Name : value = ToSqlValidString(CDbl(_datatable.Rows(0).Item(col.ColumnName)))
                        Case GetType(Date).Name : value = ToSqlValidString(CDate(_datatable.Rows(0).Item(col.ColumnName)))
                        Case Else : value = _datatable.Rows(0).Item(col.ColumnName).ToString
                    End Select
                End If
            End If

            query = query.Replace("{" & col.ColumnName & "}", value)
        Next

        Return query.ToString
    End Function

    Private Function IsAutoId(ByVal table As String, ByVal column As String) As Boolean
        Dim isauto As Boolean = False
        Dim db As String = ConnectionStringValue(DataSource.ConnectionString, ConnectionDetailEnum.Database)

        Dim query As String = "SELECT" & vbNewLine & _
                              "CASE WHEN cols.EXTRA LIKE 'auto_increment' THEN 1 ELSE 0 END AS Auto" & vbNewLine & _
                              "FROM" & vbNewLine & _
                              "information_schema.`COLUMNS` AS cols" & vbNewLine & _
                              "WHERE" & vbNewLine & _
                              "(cols.TABLE_SCHEMA LIKE '" & ToSqlValidString(db) & "') AND" & vbNewLine & _
                              "(cols.TABLE_NAME LIKE '" & ToSqlValidString(table) & "') AND" & vbNewLine & _
                              "(cols.COLUMN_NAME LIKE '" & ToSqlValidString(column) & "')"

        isauto = Que.GetValue(Of Boolean)(DataSource.ConnectionString, query, False)

        Return isauto
    End Function

    Private Function IsValid(ByVal info As ValidationEventArgs) As Boolean
        Dim _isvalid As Boolean = True : info.Valid = True

        If String.IsNullOrEmpty(RequiredFields.FieldNames.Trim) Then
            Return True : Exit Function
        End If

        Dim requiredfieldnames() As String = RequiredFields.FieldNames.Split(RequiredFields.ClipSeparator)
        Dim validator As Components.Validator = Validators(ContainerForm)

        For Each field As String In requiredfieldnames
            field = RTrim(field.Trim)
            Dim ctrl As Control = GetControlByFieldName(field)
            If _controltable.ContainsKey(field) Then ctrl = CType(_controltable(field), BindingInfo).BindedControl
            If ctrl IsNot Nothing Then
                Dim control As Object = ctrl
                Select Case ctrl.GetType().Name
                    Case GetType(MetroTextBox).Name
                        _isvalid = Valid(validator, control, Not String.IsNullOrEmpty(ctrl.Text.Trim), "Please specify a value for this field.")
                        If Not _isvalid Then
                            info.Valid = False : info.ValidatedControl = ctrl
                            validator.Highlighter.SetHighlightColor(ctrl, 0)
                            SetControlFocus(ctrl) : Exit For
                        End If
                    Case "SearchControl"
                        _isvalid = Valid(validator, control, Not String.IsNullOrEmpty(ctrl.Text.Trim), "Please specify a value for this field.")
                        If Not _isvalid Then
                            info.Valid = False : info.ValidatedControl = ctrl
                            validator.Highlighter.SetHighlightColor(ctrl, 0)
                            SetControlFocus(ctrl) : Exit For
                        End If
                    Case GetType(IntegerInput).Name, GetType(DoubleInput).Name,
                         GetType(NumericUpDown).Name
                        _isvalid = Valid(validator, control, control.Value > 0, "Please specify a value for this field.")
                        If Not _isvalid Then
                            info.Valid = False : info.ValidatedControl = ctrl
                            SetControlFocus(ctrl) : Exit For
                        End If

                    Case GetType(ComboBoxEx).Name, GetType(ComboBox).Name,
                         GetType(C1Combo).Name
                        _isvalid = Valid(validator, control, control.SelectedIndex >= 0, "Please specify a valid value for this field.")
                        If Not _isvalid Then
                            info.Valid = False : info.ValidatedControl = ctrl
                            SetControlFocus(ctrl) : Exit For
                        End If

                    Case Else
                        Select Case ctrl.GetType().BaseType.Name
                            Case GetType(TextBox).Name
                                _isvalid = Valid(validator, control, Not String.IsNullOrEmpty(ctrl.Text.Trim), "Please specify a value for this field.")
                                If Not _isvalid Then
                                    info.Valid = False : info.ValidatedControl = ctrl
                                    SetControlFocus(ctrl) : Exit For
                                End If

                            Case GetType(ComboBox).Name, GetType(C1DropDownControl).Name
                                _isvalid = Valid(validator, control, control.SelectedIndex >= 0, "Please specify a valid value for this field.")
                                If Not _isvalid Then
                                    info.Valid = False : info.ValidatedControl = ctrl
                                    SetControlFocus(ctrl) : Exit For
                                End If

                            Case Else
                        End Select
                End Select
            End If
        Next

        Return _isvalid
    End Function

#End Region

#Region "Custom Events"

    Private Sub daDetails_MyRowUpdated(ByVal sender As Object, ByVal e As MySql.Data.MySqlClient.MySqlRowUpdatedEventArgs)
        If e.StatementType = StatementType.Delete Then e.Status = UpdateStatus.SkipCurrentRow
    End Sub

    Private Sub daMain_MyRowUpdated(ByVal sender As Object, ByVal e As MySql.Data.MySqlClient.MySqlRowUpdatedEventArgs)
        If e.StatementType = StatementType.Insert Then
            Dim keyname As String = QueryStatements.KeyName
            If String.IsNullOrEmpty(keyname.Trim) Then keyname = QueryStatements.DataSource.Columns(QueryStatements.KeyIndex).ColumnName
            Fields(keyname).FieldValue = e.Row(0)
            'e.Status = UpdateStatus.SkipCurrentRow
        End If

        If e.StatementType = StatementType.Delete Then e.Status = UpdateStatus.SkipCurrentRow

        If e.StatementType = StatementType.Update Then
            If e.RecordsAffected = 0 Then
                e.Status = UpdateStatus.SkipCurrentRow
            End If
        End If
    End Sub

    Private Sub Fields_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender IsNot Nothing Then
            If sender.Enabled Then
                Dim frm As Form = ContainerForm

                If frm IsNot Nothing Then
                    If Validators.Contains(frm) Then
                        Validators(frm).ClearFailedValidations()
                        Validators(frm).ErrorProvider.Clear()
                    End If

                    If lnkSave.Enabled And
                       lnkSave.Visible And
                       Not String.IsNullOrEmpty(frm.Text.Trim) Then MarkControlAsEdited(lblTitle)
                End If
            End If
        End If
    End Sub


#End Region

#Region "Control Events"
    Private Sub lnkSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSave.Click
        If lnkSave.Enabled Then
            Dim _validation As New ValidationEventArgs
            RaiseEvent BeforeDefaultValidation(Me, _validation)
            If _validation.Cancel = True Then Exit Sub

            For Each di As DetailDataSourceInfo In DataSource.Details
                If di.ViewingGrid IsNot Nothing Then di.ViewingGrid.FinishEditing()
            Next

            lnkSave.Focus()

            Dim _isvalid As Boolean = IsValid(_validation)
            If Not _isvalid Then Exit Sub

            RaiseEvent AfterDefaultValidation(Me, _validation)
            If _validation.Cancel Then Exit Sub

            Dim _saving As New SavingEventArgs
            With _saving
                .Cancel = False : .Saved = False : .ErrorMessage = String.Empty
            End With

            RaiseEvent BeforeSave(Me, _saving)

            If _saving.Cancel Then
                lnkSave.Enabled = True : lnkCancel.Enabled = True : Exit Sub
            End If

            EnableFields(ContainerForm, False) : lnkSave.Enabled = False
            With mprProgress
                .Value = 0 : .Show() : .BringToFront()
            End With

            Dim delSetValues As New Action(AddressOf SetDatasourceValues)
            Dim arSetValues As IAsyncResult = delSetValues.BeginInvoke(Nothing, delSetValues)
            WaitToFinish(arSetValues, mprProgress)

            If _isnew Then
                If _insertcommand IsNot Nothing Then _saving.QueryStatement = _insertcommand.CommandText
            Else
                If _updatecommand IsNot Nothing Then _saving.QueryStatement = _updatecommand.CommandText
            End If

            If _connection.State = ConnectionState.Closed Then _connection.Open()
            Dim _transaction As MySqlTransaction = _connection.BeginTransaction
            Dim dt As DataTable = Nothing
            _insertcommand.Transaction = _transaction
            _updatecommand.Transaction = _transaction

            With _adapter
                .InsertCommand = _insertcommand : .UpdateCommand = _updatecommand
                .InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord
                AddHandler .RowUpdated, AddressOf daMain_MyRowUpdated
                .ContinueUpdateOnError = False

                Try
                    dt = _datatable.GetChanges(DataRowState.Added + DataRowState.Modified)
                    If dt IsNot Nothing Then
                        If dt.Rows.Count > 0 Then
                            Dim del As New Func(Of DataTable, Integer)(AddressOf .Update)
                            Dim ar As IAsyncResult = del.BeginInvoke(dt, Nothing, del)
                            WaitToFinish(ar, ProgressBar) : Dim i As Integer = del.EndInvoke(ar)
                        End If
                    End If
                Catch ex As Exception
                    _datatable.Clear()
                    With _saving
                        .Saved = False : .Cancel = True
                    End With

                    Dim _Message As String = ex.Message.Substring(0, ex.Message.IndexOf("'", 18) + 1)
                    If ex.Message.ToLower.Contains("duplicate") Then
                        MetroFramework.MetroMessageBox.Show(ParentForm, "Key-field value already exists. " & vbCrLf & _Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        Settings.ErrorMessenger(ex.Message.Trim)
#If DEBUG Then
                        MetroFramework.MetroMessageBox.Show(ParentForm, _Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
#End If
                    End If
                    EndProgress(mprProgress)
                    EnableFields(ContainerForm) : lnkSave.Enabled = True : lnkCancel.Enabled = True
                    Exit Sub
                End Try
            End With

            RaiseEvent AfterHeaderSave(Me, _saving)

            Dim keyname As String = QueryStatements.KeyName
            If String.IsNullOrEmpty(keyname.Trim) Then keyname = QueryStatements.DataSource.Columns(QueryStatements.KeyIndex).ColumnName
            Dim keyvalue As String = Fields(keyname).FieldValue

            If (_datatable.Columns.Contains(keyname)) Then
                Dim _temp As String = _datatable.Rows(0)(keyname).ToString

                If _temp <> "0" And _temp <> keyvalue Then
                    keyvalue = _temp
                End If
            End If

            For Each di As DetailDataSourceInfo In DataSource.Details
                If Not String.IsNullOrEmpty(di.CommandText.Trim) Then
                    di.Transaction = _transaction
                    WaitToFinish(di.SaveDetailsAsync(keyname, keyvalue), mprProgress)
                    Dim errormsg As String = di.ErrorMessage
                    If Not String.IsNullOrEmpty(errormsg.Trim) Then
                        With _saving
                            .Saved = False : .Cancel = True
                        End With
                        ErrorMessenger(errormsg) : EndProgress(mprProgress)
                        EnableFields(ContainerForm) : lnkSave.Enabled = True : lnkCancel.Enabled = True
                        Exit Sub
                    End If
                End If
            Next

            Try
                Dim delcommit As New Action(AddressOf _transaction.Commit)
                Dim arcommit As IAsyncResult = delcommit.BeginInvoke(Nothing, delcommit)
                WaitToFinish(arcommit, mprProgress) : EndProgress(mprProgress)
                If _connection.State = ConnectionState.Open Then _connection.Close()
                _connection.Dispose() : _adapter.Dispose() : _insertcommand.Dispose() : _updatecommand.Dispose()

                With _saving
                    .Cancel = False : .Saved = True
                End With

                EnableFields(ContainerForm) : lnkSave.Enabled = True : lnkCancel.Enabled = True
                RaiseEvent AfterSave(Me, _saving)

            Catch ex As Exception
                Dim delrb As New Action(AddressOf _transaction.Rollback)
                Dim arrb As IAsyncResult = delrb.BeginInvoke(Nothing, delrb)
                WaitToFinish(arrb, mprProgress)
                If Not String.IsNullOrEmpty(ex.Message.Trim) Then
                    With _saving
                        .Saved = False : .Cancel = True
                    End With
                    ErrorMessenger(ex.Message.Trim) : EndProgress(mprProgress)
                End If

                EnableFields(ContainerForm) : lnkSave.Enabled = True : lnkCancel.Enabled = True
                RaiseEvent AfterSave(Me, _saving)
            Finally
            End Try
        End If
    End Sub

#End Region

#Region " DMA Max Length "

    Private Sub PickListBinder_AfterDataLoading(ByVal sender As Object, ByVal e As DataLoadingEventArgs) Handles Me.AfterDataLoading
        SetMaxLength(Me.Parent, Me, _datatable)
    End Sub

    Private Sub SetMaxLength(ByVal ctlParent As Control, ByVal plb As PickListBinder, ByVal dt As DataTable)
        'On Error Resume Next

        'Dim sField As String = ""

        'For Each ctl As Control In _controltable.Keys
        '    sField = _controltable(ctl)
        '    If dt.Columns.Contains(sField) Then
        '        Select Case ctl.GetType.Name
        '            Case GetType(TextBoxX).Name
        '                If dt.Columns(sField).MaxLength > -1 Then DirectCast(ctl, TextBoxX).MaxLength = dt.Columns(sField).MaxLength
        '            Case GetType(MetroTextBox).Name
        '                If dt.Columns(sField).MaxLength > -1 Then DirectCast(ctl, MetroTextBox).MaxLength = dt.Columns(sField).MaxLength
        '        End Select
        '    End If
        'Next
    End Sub

#End Region

    Private Sub PickListBinder_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        lnkCancel.Left = Width - (lnkCancel.Width + 4)
        lnkSave.Left = lnkCancel.Left - (lnkSave.Width + 4)
        pnlLineHeader.Width = Width - 4
    End Sub

    Private Sub lnkCancel_Click(sender As Object, e As EventArgs) Handles lnkCancel.Click
        If Not lnkSave.Enabled Or
         Not lnkSave.Visible Then
            If _adapter IsNot Nothing Then _adapter.Dispose()
            If _connection IsNot Nothing Then
                With _connection
                    If .State = ConnectionState.Open Then .Close()
                    .Dispose()
                End With
            End If
            RaiseEvent AfterCancel(sender)
            Exit Sub
        End If

        If ContainerForm Is Nothing Then Exit Sub

        If lblTitle.Text.EndsWith("*") Then
            Dim result As Windows.Forms.DialogResult
            result = MetroFramework.MetroMessageBox.Show(ParentForm, "Do you want to save changes you made to this record?", "Confirm Save Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            Select Case result
                Case DialogResult.Yes
                    lnkSave_Click(Nothing, Nothing)
                    If _adapter IsNot Nothing Then _adapter.Dispose()
                    If _connection IsNot Nothing Then
                        With _connection
                            If .State = ConnectionState.Open Then .Close()
                            .Dispose()
                        End With
                    End If
                Case DialogResult.Cancel
                    Exit Sub
                Case DialogResult.No
                    RaiseEvent AfterCancel(sender)
            End Select

            'If MetroFramework.MetroMessageBox.Show(ParentForm, "Do you want to save changes you made to this record?", "Confirm Save Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '    lnkSave_Click(Nothing, Nothing)
            '    If _adapter IsNot Nothing Then _adapter.Dispose()
            '    If _connection IsNot Nothing Then
            '        With _connection
            '            If .State = ConnectionState.Open Then .Close()
            '            .Dispose()
            '        End With
            '    End If
            'Else
            '    RaiseEvent AfterCancel(sender)
            'End If
        Else
            If _adapter IsNot Nothing Then _adapter.Dispose()
            If _connection IsNot Nothing Then
                With _connection
                    If .State = ConnectionState.Open Then .Close()
                    .Dispose()
                End With
            End If
            RaiseEvent AfterCancel(sender)
        End If

    End Sub

End Class

