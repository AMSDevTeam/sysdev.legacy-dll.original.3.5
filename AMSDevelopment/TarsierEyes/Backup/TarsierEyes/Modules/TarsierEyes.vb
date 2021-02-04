Public Module TarsierEyes

#Region "API Declarations"
    Private Declare Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal process As IntPtr, ByVal minimumWorkingSetSize As Integer, ByVal maximumWorkingSetSize As Integer) As Integer
#End Region

#Region "Properties"

    Private _cleartextimage As Image = My.Resources.ClearText

    ''' <summary>
    ''' Gets or sets the globally used clear text button image in each of the search textboxes.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ClearTextImage() As Image
        Get
            Return _cleartextimage
        End Get
        Set(ByVal value As Image)
            _cleartextimage = value
        End Set
    End Property
#End Region

#Region "Methods"
    ''' <summary>
    ''' Calls the TarsierEyes.Common.Simple.Redraw(drawableobject, False) to start the updating of the drawable objects display area.
    ''' </summary>
    ''' <param name="value">Drawable object.</param>
    ''' <remarks></remarks>
    Public Sub BeginRedraw(ByVal value As Object)
        Common.Simple.Redraw(value, False)
    End Sub

    ''' <summary>
    ''' Calls the TarsierEyes.Common.Simple.Redraw(drawableobject) to display the drawable objects rectangular area.
    ''' </summary>
    ''' <param name="value">Drawable object.</param>
    ''' <remarks></remarks>
    Public Sub EndRedraw(ByVal value As Object)
        Common.Simple.Redraw(value) : Common.Simple.Redraw(value)
    End Sub
#End Region

#Region "Functions"
    ''' <summary>
    ''' Works like TryCast function but this time supports assigned-value data types (ea. Integer, Decimal, Date and etc.).
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="expression">Expression / value to be converted.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TryConvert(Of T)(ByVal expression As Object) As T
        Return Common.Simple.TryChangeType(Of T)(expression)
    End Function
#End Region

#Region "Extensions"

#Region "Between"
    ''' <summary>
    ''' Returns whether the current character value is within the specified scope of ranges.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="from"></param>
    ''' <param name="to"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [Between](ByVal value As Char, ByVal [from] As Char, ByVal [to] As Char) As Boolean
        Return (value >= [from]) And (value <= [to])
    End Function

    ''' <summary>
    ''' Returns whether the current date value is within the specified scope of ranges.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="from"></param>
    ''' <param name="to"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [Between](ByVal value As Date, ByVal [from] As Date, ByVal [to] As Date) As Boolean
        Return (value >= [from]) And (value <= [to])
    End Function

    ''' <summary>
    ''' Returns whether the current numeric value is within the specified scope of ranges.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="from"></param>
    ''' <param name="to"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [Between](ByVal value As Decimal, ByVal [from] As Decimal, ByVal [to] As Decimal) As Boolean
        Return (value >= [from]) And (value <= [to])
    End Function

    ''' <summary>
    ''' Returns whether the current numeric value is within the specified scope of ranges.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="from"></param>
    ''' <param name="to"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [Between](ByVal value As Double, ByVal [from] As Double, ByVal [to] As Double) As Boolean
        Return (value >= [from]) And (value <= [to])
    End Function

    ''' <summary>
    ''' Returns whether the current numeric value is within the specified scope of ranges.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="from"></param>
    ''' <param name="to"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [Between](ByVal value As Single, ByVal [from] As Single, ByVal [to] As Single) As Boolean
        Return (value >= [from]) And (value <= [to])
    End Function

    ''' <summary>
    ''' Returns whether the current numeric value is within the specified scope of ranges.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="from"></param>
    ''' <param name="to"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [Between](ByVal value As Long, ByVal [from] As Long, ByVal [to] As Long) As Boolean
        Return (value >= [from]) And (value <= [to])
    End Function

    ''' <summary>
    ''' Returns whether the current numeric value is within the specified scope of ranges.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="from"></param>
    ''' <param name="to"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [Between](ByVal value As Integer, ByVal [from] As Integer, ByVal [to] As Integer) As Boolean
        Return (value >= [from]) And (value <= [to])
    End Function

    ''' <summary>
    ''' Returns whether the current numeric value is within the specified scope of ranges.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="from"></param>
    ''' <param name="to"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [Between](ByVal value As SByte, ByVal [from] As SByte, ByVal [to] As SByte) As Boolean
        Return (value >= [from]) And (value <= [to])
    End Function

    ''' <summary>
    ''' Returns whether the current numeric value is within the specified scope of ranges.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="from"></param>
    ''' <param name="to"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [Between](ByVal value As Byte, ByVal [from] As Byte, ByVal [to] As Byte) As Boolean
        Return (value >= [from]) And (value <= [to])
    End Function
#End Region

    ''' <summary>
    ''' Clears and initializes each of the container's control(textboxes, comboboxes, grids, datetimepickers and etc.) datasources and input area values.
    ''' </summary>
    ''' <param name="container"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub ClearFieldContents(ByVal container As System.Windows.Forms.ScrollableControl)
        Common.Simple.ClearContents(container)
    End Sub

#Region "Archiver"
    ''' <summary>
    ''' Performs file compression in the specified directory using the selected archiving tool. Files under the specified directory will just be copied into the result archive file retaining file(s) in the original directory. 
    ''' </summary>
    ''' <param name="directory"></param>
    ''' <param name="archivingtool">Archiving tool.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function CompressAdd(ByVal directory As IO.DirectoryInfo, ByVal archivingtool As Archiver.ArchivingToolEnum) As IO.FileInfo
        Return Archiver.CompressAdd(directory.FullName, archivingtool)
    End Function

    ''' <summary>
    ''' Performs file compression in the specified directory using the selected archiving tool. Files under the specified directory will be inserted directly into the result archive file. 
    ''' </summary>
    ''' <param name="directory"></param>
    ''' <param name="archivingtool">Archiving tool.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function CompressInsert(ByVal directory As IO.DirectoryInfo, ByVal archivingtool As Archiver.ArchivingToolEnum) As IO.FileInfo
        Return Archiver.CompressInsert(directory.FullName, archivingtool)
    End Function

    ''' <summary>
    ''' Performs file compression in the specified file using the selected archiving tool. File  will just be copied into the archive file retaining then file in the original directory. 
    ''' </summary>
    ''' <param name="file"></param>
    ''' <param name="archivingtool">Archiving tool.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function CompressAdd(ByVal file As IO.FileInfo, ByVal archivingtool As Archiver.ArchivingToolEnum) As IO.FileInfo
        Return Archiver.CompressAdd(file.FullName, archivingtool)
    End Function

    ''' <summary>
    ''' Performs file compression in the file using the selected archiving tool. File will be inserted directly into the result archive file. 
    ''' </summary>
    ''' <param name="file"></param>
    ''' <param name="archivingtool">Archiving tool.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function CompressInsert(ByVal file As IO.FileInfo, ByVal archivingtool As Archiver.ArchivingToolEnum) As IO.FileInfo
        Return Archiver.CompressInsert(file.FullName, archivingtool)
    End Function

    ''' <summary>
    ''' Performs file extraction from the specified archive file into the specified destination folder using the selected archiving tool.
    ''' </summary>
    ''' <param name="file"></param>
    ''' <param name="destination">Destination path for the extracted file(s).</param>
    ''' <param name="archivingtool">Archiving tool.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function Decompress(ByVal file As IO.FileInfo, ByVal destination As String, ByVal archivingtool As Archiver.ArchivingToolEnum) As Boolean
        Return Archiver.Decompress(file.FullName, destination, archivingtool)
    End Function
#End Region

    ''' <summary>
    ''' Decrypts the specified string using TripleDES and MD5 algorithm based on the specified decryption key.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="key">Decryption key</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function Decrypt(ByVal value As String, ByVal key As String) As String
        Return Cryptography.Decryption.Decrypt(value, key)
    End Function


    ''' <summary>
    ''' Enables or disabled controls under the specified control container.
    ''' </summary>
    ''' <param name="container"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub EnableContainedFields(ByVal container As System.Windows.Forms.ScrollableControl)
        container.EnableContainedFields(True)
    End Sub

    ''' <summary>
    ''' Enables or disabled controls under the specified control container.
    ''' </summary>
    ''' <param name="container"></param>
    ''' <param name="enabled">Enabled or disabled</param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub EnableContainedFields(ByVal container As System.Windows.Forms.ScrollableControl, ByVal enabled As Boolean)
        Common.Simple.EnableFields(container, enabled)
    End Sub

    ''' <summary>
    ''' Encrypts the specified string using TripleDES and MD5 algorithm based on the specified encryption key.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="key">Encryption key</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function Encrypt(ByVal value As String, ByVal key As String) As String
        Return Cryptography.Encryption.Encrypt(value, key)
    End Function

    ''' <summary>
    ''' Gets an assigned SQL connection string value from the specified section.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="section">SQL connection string section.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function GetConnectionStringPart(ByVal value As String, ByVal section As MySQL.ConnectionDetailEnum) As String
        Return MySQL.ConnectionStringValue(value, section)
    End Function


#Region "In"
    ''' <summary>
    ''' Returns whether the current enumeration value is existing within the list of reference enumeration values or not.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="references"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [In](ByVal value As System.Enum, ByVal ParamArray references() As System.Enum) As Boolean
        Dim exists As Boolean = False

        For Each ref As System.Enum In references
            exists = exists Or (value.Equals(ref))
            If exists Then Exit For
        Next

        Return exists
    End Function

    ''' <summary>
    ''' Returns whether the current character value is existing within the list of reference character values or not.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="references"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [In](ByVal value As Char, ByVal ParamArray references() As Char) As Boolean
        Dim exists As Boolean = False

        For Each ref As Char In references
            exists = exists Or (value = ref)
            If exists Then Exit For
        Next

        Return exists
    End Function

    ''' <summary>
    ''' Returns whether the current string value is existing within the list of reference string values or not.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="references"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [In](ByVal value As String, ByVal ParamArray references() As String) As Boolean
        Dim exists As Boolean = False

        For Each ref As String In references
            exists = exists Or (RTrim(value.Trim) = RTrim(ref.Trim))
            If exists Then Exit For
        Next

        Return exists
    End Function

    ''' <summary>
    ''' Returns whether the current date value is existing within the list of reference date values or not.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="references"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [In](ByVal value As Date, ByVal ParamArray references() As Date) As Boolean
        Dim exists As Boolean = False

        For Each ref As Date In references
            exists = exists Or (value = ref)
            If exists Then Exit For
        Next

        Return exists
    End Function

    ''' <summary>
    ''' Returns whether the current numeric value is existing within the list of reference numeric values or not.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="references"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [In](ByVal value As Double, ByVal ParamArray references() As Double) As Boolean
        Dim exists As Boolean = False

        For Each ref As Double In references
            exists = exists Or (value = ref)
            If exists Then Exit For
        Next

        Return exists
    End Function

    ''' <summary>
    ''' Returns whether the current numeric value is existing within the list of reference numeric values or not.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="references"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [In](ByVal value As Single, ByVal ParamArray references() As Single) As Boolean
        Dim exists As Boolean = False

        For Each ref As Single In references
            exists = exists Or (value = ref)
            If exists Then Exit For
        Next

        Return exists
    End Function

    ''' <summary>
    ''' Returns whether the current numeric value is existing within the list of reference numeric values or not.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="references"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [In](ByVal value As Decimal, ByVal ParamArray references() As Decimal) As Boolean
        Dim exists As Boolean = False

        For Each ref As Decimal In references
            exists = exists Or (value = ref)
            If exists Then Exit For
        Next

        Return exists
    End Function

    ''' <summary>
    ''' Returns whether the current numeric value is existing within the list of reference numeric values or not.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="references"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [In](ByVal value As Byte, ByVal ParamArray references() As Byte) As Boolean
        Dim exists As Boolean = False

        For Each ref As Byte In references
            exists = exists Or (value = ref)
            If exists Then Exit For
        Next

        Return exists
    End Function

    ''' <summary>
    ''' Returns whether the current numeric value is existing within the list of reference numeric values or not.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="references"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [In](ByVal value As SByte, ByVal ParamArray references() As SByte) As Boolean
        Dim exists As Boolean = False

        For Each ref As SByte In references
            exists = exists Or (value = ref)
            If exists Then Exit For
        Next

        Return exists
    End Function

    ''' <summary>
    ''' Returns whether the current numeric value is existing within the list of reference numeric values or not.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="references"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [In](ByVal value As Long, ByVal ParamArray references() As Long) As Boolean
        Dim exists As Boolean = False

        For Each ref As Long In references
            exists = exists Or (value = ref)
            If exists Then Exit For
        Next

        Return exists
    End Function

    ''' <summary>
    ''' Returns whether the current numeric value is existing within the list of reference numeric values or not.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="references"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function [In](ByVal value As Integer, ByVal ParamArray references() As Integer) As Boolean
        Dim exists As Boolean = False

        For Each ref As Integer In references
            exists = exists Or (value = ref)
            If exists Then Exit For
        Next

        Return exists
    End Function
#End Region

    ''' <summary>
    ''' Returns whether the specified string is an email or not.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function IsEmail(ByVal value As String) As Boolean
        Dim _isemail As Boolean = False

        Dim _regex As New Regex("^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?" & _
                                "[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?" & _
                                "[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$")
        _isemail = _regex.IsMatch(value)

        Return _isemail
    End Function

    ''' <summary>
    ''' Returns whether the specified string is a IP address
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function IsIPAddress(ByVal value As String) As Boolean
        Dim _isip As Boolean = False

        Dim _regex As New Regex("\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b")
        _isip = _regex.IsMatch(value)

        Return _isip
    End Function

    ''' <summary>
    ''' Loads list of countries into the specified control. 
    ''' </summary>
    ''' <param name="control"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub LoadCountries(ByVal control As Windows.Forms.Control)
        Dim controlobj As Object = control

        If controlobj IsNot Nothing Then
            Dim datasourceprop As PropertyInfo = controlobj.GetType().GetProperty("DataSource")
            If datasourceprop IsNot Nothing Then
                Dim dt As DataTable = Common.Simple.GetCountryTable
                With controlobj
                    .Enabled = False

                    If .DataSource IsNot Nothing Then
                        Try
                            TryCast(.DataSource, DataTable).Dispose()
                        Catch ex As Exception
                        Finally : .DataSource = Nothing
                        End Try
                    End If

                    .DataSource = dt

                    Dim displaymemberprop As PropertyInfo = controlobj.GetType().GetProperty("DisplayMember")
                    Dim valuememberprop As PropertyInfo = controlobj.GetType().GetProperty("ValueMember")
                    If displaymemberprop IsNot Nothing And _
                       valuememberprop IsNot Nothing Then
                        .DisplayMember = "Country" : .ValueMember = "Country"
                    End If

                    Dim autocompleteprop As PropertyInfo = controlobj.GetType().GetProperty("AutoCompleteMode")
                    If autocompleteprop IsNot Nothing Then .AutoCompleteMode = Windows.Forms.AutoCompleteMode.SuggestAppend

                    Dim autocompletesourceprop As PropertyInfo = controlobj.GetType().GetProperty("AutoCompleteSource")
                    If autocompletesourceprop IsNot Nothing Then .AutoCompleteSource = Windows.Forms.AutoCompleteSource.ListItems

                    Dim selindexprop As PropertyInfo = controlobj.GetType().GetProperty("SelectedIndex")
                    If selindexprop IsNot Nothing Then .SelectedIndex = -1

                    .Enabled = True
                End With
            End If
        End If
    End Sub

#Region "LoadData (DataTable)"
    ''' <summary>
    ''' Loads the current DataTable object with the resultset of the specified database query statement thru the specified database connection string.
    ''' </summary>
    ''' <param name="table"></param>
    ''' <param name="connection">Database connection string.</param>
    ''' <param name="commandtext">Database command statement.</param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub LoadData(ByRef table As DataTable, ByVal connection As String, ByVal commandtext As String)
        Dim q As MySQL.Que = MySQL.Que.Execute(connection, commandtext, MySQL.Que.ExecutionEnum.ExecuteReader)

        If String.IsNullOrEmpty(q.ErrorMessage.Trim) Then
            If table IsNot Nothing Then
                Try
                    table.Dispose()
                Catch ex As Exception
                Finally : table = Nothing
                End Try
            End If
            table = q.DataTable.Clone() : table.Load(q.DataTable.CreateDataReader)
        End If
        q.Dispose()
    End Sub
#End Region

#Region "LoadData (Control)"
    ''' <summary>
    ''' Loads data into the specified bindable control.
    ''' </summary>
    ''' <param name="control"></param>
    ''' <param name="connectionstring">Database connection string.</param>
    ''' <param name="commandtext">Datasource commandtext</param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub LoadData(ByVal control As System.Windows.Forms.Control, ByVal connectionstring As String, ByVal commandtext As String)
        control.LoadData(connectionstring, commandtext, String.Empty)
    End Sub

    ''' <summary>
    ''' Loads data into the specified bindable control.
    ''' </summary>
    ''' <param name="control"></param>
    ''' <param name="connectionstring">Database connection string.</param>
    ''' <param name="commandtext">Datasource commandtext</param>
    ''' <param name="displaymember">Displaymember name for the binding.</param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub LoadData(ByVal control As System.Windows.Forms.Control, ByVal connectionstring As String, ByVal commandtext As String, ByVal displaymember As String)
        control.LoadData(connectionstring, commandtext, displaymember, String.Empty)
    End Sub

    ''' <summary>
    ''' Loads data into the specified bindable control.
    ''' </summary>
    ''' <param name="control"></param>
    ''' <param name="connectionstring">Database connection string.</param>
    ''' <param name="commandtext">Datasource commandtext</param>
    ''' <param name="displaymember">Displaymember name for the binding.</param>
    ''' <param name="valuemember">Valuemember name for the binding.</param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub LoadData(ByVal control As System.Windows.Forms.Control, ByVal connectionstring As String, ByVal commandtext As String, ByVal displaymember As String, ByVal valuemember As String)
        Dim controlobj As Object = control

        If controlobj IsNot Nothing Then
            Dim datasourceprop As PropertyInfo = controlobj.GetType().GetProperty("DataSource")
            If datasourceprop IsNot Nothing Then
                Dim q As MySQL.Que = MySQL.Que.Execute(connectionstring, commandtext, MySQL.Que.ExecutionEnum.ExecuteReader)
                If String.IsNullOrEmpty(q.ErrorMessage.Trim) Then
                    Dim dt As DataTable = q.DataTable.Clone
                    dt.Load(q.DataTable.CreateDataReader)

                    With controlobj
                        .Enabled = False

                        If .DataSource IsNot Nothing Then
                            Try
                                TryCast(.DataSource, DataTable).Dispose()
                            Catch ex As Exception
                            Finally : .DataSource = Nothing
                            End Try
                        End If

                        .DataSource = dt

                        Dim displaymemberprop As PropertyInfo = controlobj.GetType().GetProperty("DisplayMember")
                        Dim valuememberprop As PropertyInfo = controlobj.GetType().GetProperty("ValueMember")
                        If displaymemberprop IsNot Nothing And _
                           valuememberprop IsNot Nothing Then
                            Dim vm As String = dt.Columns(0).ColumnName : Dim dm As String = dt.Columns(0).ColumnName

                            If Not String.IsNullOrEmpty(valuemember.Trim) Then
                                If dt.Columns.Contains(valuemember.Trim) Then vm = valuemember
                            End If

                            If Not String.IsNullOrEmpty(displaymember.Trim) Then
                                If dt.Columns.Contains(displaymember.Trim) Then dm = displaymember
                            End If

                            .DisplayMember = dm : .ValueMember = vm
                        End If

                        Dim autocompleteprop As PropertyInfo = controlobj.GetType().GetProperty("AutoCompleteMode")
                        If autocompleteprop IsNot Nothing Then .AutoCompleteMode = Windows.Forms.AutoCompleteMode.SuggestAppend

                        Dim autocompletesourceprop As PropertyInfo = controlobj.GetType().GetProperty("AutoCompleteSource")
                        If autocompletesourceprop IsNot Nothing Then .AutoCompleteSource = Windows.Forms.AutoCompleteSource.ListItems

                        Dim selindexprop As PropertyInfo = controlobj.GetType().GetProperty("SelectedIndex")
                        If selindexprop IsNot Nothing Then .SelectedIndex = -1

                        .Enabled = True
                    End With
                End If
                q.Dispose()
            End If
        End If
    End Sub
#End Region

    Private Sub DisposingForm_FormClosed(ByVal sender As Object, ByVal e As Windows.Forms.FormClosedEventArgs)
        Dim frm As Windows.Forms.Form = sender
        If frm IsNot Nothing Then
            GC.Collect() : GC.SuppressFinalize(frm)
            If (Environment.OSVersion.Platform = PlatformID.Win32NT) Then
                Dim p As Process = Process.GetCurrentProcess
                SetProcessWorkingSetSize(p.Handle, -1, -1)
                p.Close() : p.Refresh() : p.Dispose()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Releases all relative resources (including processes) of the whole application after the current form is disposed. Resources (API calls and attached method) c / o : DMA.
    ''' </summary>
    ''' <param name="form"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub ManageOnDispose(ByVal form As Windows.Forms.Form)
        AddHandler form.FormClosed, AddressOf DisposingForm_FormClosed
    End Sub

    ''' <summary>
    ''' Places a '*' character in the form's caption as an indicator that form's fields is modified.
    ''' </summary>
    ''' <param name="form"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub MarkAsEdited(ByVal form As Windows.Forms.Form)
        Common.Simple.MarkFormAsEdited(form)
    End Sub

#Region "NumberOfDayInRange"
    ''' <summary>
    ''' Returns the count of specified day between the given date range(s).
    ''' </summary>
    ''' <param name="day">Searchee day between Sunday-Saturday (1-7).</param>
    ''' <param name="fromdate">Searching start date.</param>
    ''' <param name="todate">Searching end date.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function NumberOfDayInRange(ByVal day As Integer, ByVal fromdate As Date, ByVal todate As Date) As Integer
        Dim daycount As Integer = 0

        If day.Between(1, 7) Then
            If fromdate.Date <= todate.Date Then
                Dim startdate As Date = fromdate.Date
                For i As Integer = 0 To DateDiff(DateInterval.Day, fromdate.Date, todate.Date)
                    If startdate.AddDays(i).DayOfWeek = day Then daycount += 1
                Next
            Else : Throw New ArgumentException("Date ranges is not valid.")
            End If
        Else : Throw New ArgumentException("Searchee day must only be between Sunday-Saturday (1-7) : where day 1 starts at Sunday.")
        End If

        Return daycount
    End Function

    ''' <summary>
    ''' Returns the count of specified day between the given date range(s).
    ''' </summary>
    ''' <param name="day">Searchee day between Sunday-Saturday (1-7).</param>
    ''' <param name="fromdate">Searching start date.</param>
    ''' <param name="todate">Searching end date.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function NumberOfDayInRange(ByVal day As DayOfWeek, ByVal fromdate As Date, ByVal todate As Date) As Integer
        Dim daycount As Integer = 0

        If CInt(day).Between(1, 7) Then
            If fromdate.Date <= todate.Date Then
                Dim startdate As Date = fromdate.Date
                For i As Integer = 0 To DateDiff(DateInterval.Day, fromdate.Date, todate.Date)
                    If startdate.AddDays(i).DayOfWeek = day Then daycount += 1
                Next
            Else : Throw New ArgumentException("Date ranges is not valid.")
            End If
        Else : Throw New ArgumentException("Searchee day must only be between Sunday-Saturday (1-7) : where day 1 starts at Sunday.")
        End If

        Return daycount
    End Function
#End Region

    ''' <summary>
    ''' Loads row data from the first Spreadsheet of the specified Microsoft Excel file into the current DataTable.
    ''' </summary>
    ''' <param name="table"></param>
    ''' <param name="filename"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub ReadExcel(ByVal table As DataTable, ByVal filename As String)
        table.ReadExcel(filename, String.Empty)
    End Sub

    ''' <summary>
    ''' Loads row data from the specified Spreadsheet of the specified Microsoft Excel file into the current DataTable.
    ''' </summary>
    ''' <param name="table"></param>
    ''' <param name="filename"></param>
    ''' <param name="sheetname"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub ReadExcel(ByVal table As DataTable, ByVal filename As String, ByVal sheetname As String)
        Dim dt As DataTable = Common.MSExcel.Import(filename, sheetname)
        If dt IsNot Nothing Then
            If table IsNot Nothing Then
                Try
                    table.Dispose()
                Catch ex As Exception
                Finally : table = Nothing
                End Try
            End If
            table = dt.Clone : table.Load(dt.CreateDataReader) : dt.Dispose()
        End If
    End Sub

#Region "ResizeImage"
    ''' <summary>
    ''' Sets the display size of the specified image.
    ''' </summary>
    ''' <param name="img"></param>
    ''' <param name="width"></param>
    ''' <param name="height"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub ResizeImage(ByRef img As Image, ByVal width As Integer, ByVal height As Integer)
        img.ResizeImage(New Size(width, height))
    End Sub

    ''' <summary>
    ''' Sets the display size of the specified image.
    ''' </summary>
    ''' <param name="img"></param>
    ''' <param name="size"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub ResizeImage(ByRef img As Image, ByVal size As Size)
        If img IsNot Nothing Then
            Dim bm As New Bitmap(img)
            Dim width As Integer = size.Width : Dim height As Integer = size.Height
            If width <= 0 Then width = 1
            If height <= 0 Then height = 1
            Dim thumb As New Bitmap(width, height)
            Dim g As Graphics = Graphics.FromImage(thumb)

            With g
                .InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                .DrawImage(bm, New Rectangle(0, 0, width, height), New Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel)
                .Dispose()
            End With

            bm.Dispose() : img = thumb
        End If
    End Sub
#End Region

    ''' <summary>
    ''' Returns a complete space trimmed representation of the specified value (combination of RTrim and String.Trim() functions).
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function RLTrim(ByVal value As String) As String
        Return RTrim(value.Trim)
    End Function

    ''' <summary>
    ''' Sets the specified control with a required field indicator.
    ''' </summary>
    ''' <param name="control"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub SetAsRequired(ByVal control As Windows.Forms.Control)
        control.SetAsRequired(True)
    End Sub

    ''' <summary>
    ''' Sets the specified control with a required field indicator.
    ''' </summary>
    ''' <param name="control"></param>
    ''' <param name="required">Mark as required or not.</param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub SetAsRequired(ByVal control As Windows.Forms.Control, ByVal required As Boolean)
        Controls.RequiredFieldMarker.SetAsRequired(control, required)
    End Sub

    ''' <summary>
    ''' Decrypts the specified value in a simple manner based on the decryption key's character length.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="key">Decryption key.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function SimpleDecrypt(ByVal value As String, ByVal key As String) As String
        Return Cryptography.Decryption.SimpleDecrypt(value, key)
    End Function

    ''' <summary>
    ''' Encrypts the specified value in a simple manner based on the encryption key's character length.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="key">Encryption key.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function SimpleEncrypt(ByVal value As String, ByVal key As String) As String
        Return Cryptography.Encryption.SimpleEncrypt(value, key)
    End Function

    ''' <summary>
    ''' Converts the specified file into its byte array representation.
    ''' </summary>
    ''' <param name="file"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToByteArray(ByVal file As IO.FileInfo) As Byte()
        Return Common.Simple.FileObjectToByteArray(file.FullName)
    End Function

    ''' <summary>
    ''' Converts the specified image into its byte array representation.
    ''' </summary>
    ''' <param name="picture"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToByteArray(ByVal picture As Image) As Byte()
        Return Common.Simple.ImageToByteArray(picture)
    End Function

    ''' <summary>
    ''' Converts the specified byte array into a file object with the specified extension name.
    ''' </summary>
    ''' <param name="bytes"></param>
    ''' <param name="extension">File extension</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToFileObject(ByVal bytes() As Byte, ByVal extension As String) As IO.FileInfo
        Return Common.Simple.ByteArrayToFileObject(bytes, extension)
    End Function

    ''' <summary>
    ''' Converts the specified byte array into a file object with the specified extension name.
    ''' </summary>
    ''' <param name="bytes"></param>
    ''' <param name="extension">File extension</param>
    ''' <param name="outputdirectory">Output directory for the exported file.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToFileObject(ByVal bytes() As Byte, ByVal extension As String, ByVal outputdirectory As String) As IO.FileInfo
        Return Common.Simple.ByteArrayToFileObject(bytes, extension, outputdirectory)
    End Function

    ''' <summary>
    ''' Returns the hexadecimal string representation of the specified byte array.
    ''' </summary>
    ''' <param name="bytes"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToHexadecimalString(ByVal bytes() As Byte) As String
        Return Common.Simple.ByteArrayToHexaDecimalString(bytes)
    End Function

    ''' <summary>
    ''' Returns the hexadecimal string representation of the specified file.
    ''' </summary>
    ''' <param name="file"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToHexadecimalString(ByVal file As IO.FileInfo) As String
        Return Common.Simple.FileObjectToHexaDecimalString(file.FullName)
    End Function

    ''' <summary>
    ''' Returns the hexadecimal string representation of the specified image.
    ''' </summary>
    ''' <param name="picture"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToHexadecimalString(ByVal picture As Image) As String
        Return Common.Simple.ImageToHexaDecimalString(picture)
    End Function

    ''' <summary>
    ''' Returns the image representation of the specified byte array.
    ''' </summary>
    ''' <param name="bytes"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToImage(ByVal bytes() As Byte) As Image
        Return Common.Simple.ByteArrayToImage(bytes)
    End Function

    ''' <summary>
    ''' Returns the proper case (first letter capitalized and small caps for the preceeding letters) representation of the specified string.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToProper(ByVal value As String) As String
        Return Common.Simple.ToProperString(value)
    End Function

#Region "ToSafeValue"

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Boolean) As Boolean
        Return (value.ToSafeValue(False))
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="defaultvalue">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Boolean, ByVal defaultvalue As Boolean) As Boolean
        Return Common.Simple.ToSafeValue(Of Boolean)(value, defaultvalue)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Byte) As Byte
        Return value.ToSafeValue(0)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="defaultvalue">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Byte, ByVal defaultvalue As Byte) As Byte
        Return Common.Simple.ToSafeValue(Of Byte)(value, defaultvalue)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Char) As Char
        Return value.ToSafeValue("")
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="defaultvalue">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Char, ByVal defaultvalue As Char) As Char
        Return Common.Simple.ToSafeValue(Of Char)(value, defaultvalue)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Date) As Date
        Return value.ToSafeValue(#1/1/1900#)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="defaultvalue">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Date, ByVal defaultvalue As Date) As Date
        Return Common.Simple.ToSafeValue(Of Date)(value, defaultvalue)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Decimal) As Decimal
        Return value.ToSafeValue(0)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="defaultvalue">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Decimal, ByVal defaultvalue As Decimal) As Decimal
        Return Common.Simple.ToSafeValue(Of Decimal)(value, defaultvalue)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Double) As Double
        Return value.ToSafeValue(0)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="defaultvalue">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Double, ByVal defaultvalue As Double) As Double
        Return Common.Simple.ToSafeValue(Of Double)(value, defaultvalue)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Integer) As Integer
        Return value.ToSafeValue(0)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="defaultvalue">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Integer, ByVal defaultvalue As Integer) As Integer
        Return Common.Simple.ToSafeValue(Of Integer)(value, defaultvalue)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Long) As Long
        Return value.ToSafeValue(0)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="defaultvalue">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As Long, ByVal defaultvalue As Long) As Long
        Return Common.Simple.ToSafeValue(Of Long)(value, defaultvalue)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As SByte) As SByte
        Return value.ToSafeValue(0)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="defaultvalue">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As SByte, ByVal defaultvalue As SByte) As SByte
        Return Common.Simple.ToSafeValue(Of SByte)(value, defaultvalue)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As String) As String
        Return value.ToSafeValue(String.Empty)
    End Function

    ''' <summary>
    ''' Parses the specified value to return its type-safe representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="defaultvalue">Default value just in case it is not type-safe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSafeValue(ByVal value As String, ByVal defaultvalue As String) As String
        Return Common.Simple.ToSafeValue(Of String)(value, defaultvalue)
    End Function

#End Region

#Region "ToSqlValidString"

    ''' <summary>
    ''' Converts the specified date value in its SQL-qualified string representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSqlValidString(ByVal value As Date) As String
        Return Common.SQLStrings.ToSqlValidString(value)
    End Function

    ''' <summary>
    ''' Converts the specified date value in its SQL-qualified string representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="withhrours">Determines whether to include time part of the current value in the result or not.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSqlValidString(ByVal value As Date, ByVal withhrours As Boolean) As String
        Return Common.SQLStrings.ToSqlValidString(value, withhrours)
    End Function

    ''' <summary>
    '''  Converts the specified numeric value in its SQL-qualified string representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSqlValidString(ByVal value As Decimal) As String
        Return CDbl(value).ToSqlValidString
    End Function

    ''' <summary>
    ''' Converts the specified numeric value (with the specified decimal places) in its SQL-qualified string representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="decimals">Number of decimal places.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSqlValidString(ByVal value As Decimal, ByVal decimals As Integer) As String
        Return CDbl(value).ToSqlValidString(decimals)
    End Function

    ''' <summary>
    ''' Converts the specified numeric value in its SQL-qualified string representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSqlValidString(ByVal value As Double) As String
        Return Common.SQLStrings.ToSqlValidString(value)
    End Function

    ''' <summary>
    ''' Converts the specified numeric value (with the specified decimal places) in its SQL-qualified string representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="decimals">Number of decimal places.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSqlValidString(ByVal value As Double, ByVal decimals As Integer) As String
        Return Common.SQLStrings.ToSqlValidString(value, decimals)
    End Function

    ''' <summary>
    '''  Converts the specified numeric value in its SQL-qualified string representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSqlValidString(ByVal value As Single) As String
        Return CDbl(value).ToSqlValidString
    End Function

    ''' <summary>
    ''' Converts the specified numeric value (with the specified decimal places) in its SQL-qualified string representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="decimals">Number of decimal places.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSqlValidString(ByVal value As Single, ByVal decimals As Integer) As String
        Return CDbl(value).ToSqlValidString(decimals)
    End Function

    ''' <summary>
    ''' Converts the specified string value in its SQL-qualified string representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSqlValidString(ByVal value As String) As String
        Return Common.SQLStrings.ToSqlValidString(value)
    End Function

    ''' <summary>
    ''' Converts the specified string value in its SQL-qualified string representation.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="fordatatableexpression">Determines whether this will be used for a DataTable expression or not.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToSqlValidString(ByVal value As String, ByVal fordatatableexpression As Boolean) As String
        Return Common.SQLStrings.ToSqlValidString(value, fordatatableexpression)
    End Function
#End Region

#Region "ToWords"
    ''' <summary>
    ''' Returns the english-word representation of the specified value.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToWords(ByVal value As SByte) As String
        Return CDbl(value).ToWords
    End Function

    ''' <summary>
    ''' Returns the english-word representation of the specified value.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="currency">Suffixing currency.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToWords(ByVal value As SByte, ByVal currency As String) As String
        Return CDbl(value).ToWords(currency)
    End Function

    ''' <summary>
    ''' Returns the english-word representation of the specified value.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToWords(ByVal value As Byte) As String
        Return CDbl(value).ToWords
    End Function

    ''' <summary>
    ''' Returns the english-word representation of the specified value.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="currency">Suffixing currency.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToWords(ByVal value As Byte, ByVal currency As String) As String
        Return CDbl(value).ToWords(currency)
    End Function

    ''' <summary>
    ''' Returns the english-word representation of the specified value.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToWords(ByVal value As Integer) As String
        Return CDbl(value).ToWords
    End Function

    ''' <summary>
    ''' Returns the english-word representation of the specified value.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="currency">Suffixing currency.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToWords(ByVal value As Integer, ByVal currency As String) As String
        Return CDbl(value).ToWords(currency)
    End Function

    ''' <summary>
    ''' Returns the english-word representation of the specified value.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToWords(ByVal value As Long) As String
        Return CDbl(value).ToWords
    End Function

    ''' <summary>
    ''' Returns the english-word representation of the specified value.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="currency">Suffixing currency.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToWords(ByVal value As Long, ByVal currency As String) As String
        Return CDbl(value).ToWords(currency)
    End Function

    ''' <summary>
    ''' Returns the english-word representation of the specified value.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToWords(ByVal value As Decimal) As String
        Return CDbl(value).ToWords
    End Function

    ''' <summary>
    ''' Returns the english-word representation of the specified value.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="currency">Suffixing currency.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToWords(ByVal value As Decimal, ByVal currency As String) As String
        Return CDbl(value).ToWords(currency)
    End Function

    ''' <summary>
    ''' Returns the english-word representation of the specified value.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToWords(ByVal value As Single) As String
        Return CDbl(value).ToWords
    End Function

    ''' <summary>
    ''' Returns the english-word representation of the specified value.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="currency">Suffixing currency.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToWords(ByVal value As Single, ByVal currency As String) As String
        Return CDbl(value).ToWords(currency)
    End Function

    ''' <summary>
    ''' Returns the english-word representation of the specified value.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToWords(ByVal value As Double) As String
        Return Common.Amounts.AmountToWords(value)
    End Function

    ''' <summary>
    ''' Returns the english-word representation of the specified value.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="currency">Suffixing currency.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function ToWords(ByVal value As Double, ByVal currency As String) As String
        Return Common.Amounts.AmountToWords(value, currency)
    End Function
#End Region

#Region "WaitToFinish"
    ''' <summary>
    ''' Waits the specified sync result to be finished.
    ''' </summary>
    ''' <param name="async"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub WaitToFinish(ByVal async As IAsyncResult)
        Common.Synchronization.WaitToFinish(async)
    End Sub

    ''' <summary>
    ''' Waits the specified sync result to be finished.
    ''' </summary>
    ''' <param name="async"></param>
    ''' <param name="progressbar">Progressbar object to show the current status of the delegate.</param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub WaitToFinish(ByVal async As IAsyncResult, ByVal progressbar As Object)
        Common.Synchronization.WaitToFinish(async, progressbar)
    End Sub

    ''' <summary>
    ''' Waits the specified sync result to be finished.
    ''' </summary>
    ''' <param name="async"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub WaitToFinish(ByVal async As Threading.Thread)
        Common.Synchronization.WaitToFinish(async)
    End Sub

    ''' <summary>
    ''' Waits the specified sync result to be finished.
    ''' </summary>
    ''' <param name="async"></param>
    ''' <param name="progressbar">Progressbar object to show the current status of the delegate.</param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub WaitToFinish(ByVal async As Threading.Thread, ByVal progressbar As Object)
        Common.Synchronization.WaitToFinish(async, progressbar)
    End Sub
#End Region

    ''' <summary>
    ''' Writes the contents of the specified DataTable into the specified file (file must be in .csv format otherwise method will automatically alter the file's extension into .csv).
    ''' </summary>
    ''' <param name="table"></param>
    ''' <param name="filename"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub WriteExcel(ByVal table As DataTable, ByRef filename As String)
        table.WriteExcel(filename, vbTab)
    End Sub

    ''' <summary>
    '''  Writes the contents of the specified DataTable into the specified file (file must be in .csv format otherwise method will automatically alter the file's extension into .csv).
    ''' </summary>
    ''' <param name="table"></param>
    ''' <param name="filename"></param>
    ''' <param name="separator"></param>
    ''' <remarks></remarks>
    <Extension()> _
    Public Sub WriteExcel(ByVal table As DataTable, ByRef filename As String, ByVal separator As String)
        Common.MSExcel.Export(table, filename, separator)
    End Sub

#End Region

End Module
