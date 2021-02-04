#Region "Imports"
Imports System.Text
Imports TarsierEyes.EDI
Imports TarsierEyes.MySQL
#End Region

''' <summary>
''' System script reader.
''' </summary>
''' <remarks></remarks>
Public Class ScriptReader
    Implements IDisposable

#Region "Properties"
    Dim _author As String = String.Empty

    ''' <summary>
    ''' Gets the script's author.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Author As String
        Get
            Return _author
        End Get
    End Property

    Dim _contents As StringBuilder = Nothing

    ''' <summary>
    ''' Gets the sctual decrypted contents of the specified script file.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Contents As String
        Get
            If _contents Is Nothing Then : Return String.Empty
            Else : Return _contents.ToString
            End If
        End Get
    End Property

    Dim _filename As String = String.Empty

    ''' <summary>
    ''' Gets the script's filename.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Filename As String
        Get
            Return _filename
        End Get
    End Property

    Dim _lastupdated As Date = Now

    ''' <summary>
    ''' Gets the date and time when the script was last released / updated.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LastUpdated As Date
        Get
            Return _lastupdated
        End Get
    End Property

    Dim _message As String = String.Empty

    ''' <summary>
    ''' Gets the script's message.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Message As String
        Get
            Return _message
        End Get
    End Property

    Dim _referenceno As String = String.Empty

    ''' <summary>
    ''' Gets the script's reference number.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ReferenceNo As String
        Get
            Return _referenceno
        End Get
    End Property

    Dim _requiredauthorization As Boolean = False

    ''' <summary>
    ''' Gets whether script requires authorization before it can be executed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property RequiredAuthorization As Boolean
        Get
            Return _requiredauthorization
        End Get
    End Property

    Dim _requiredbackup As Boolean = False

    ''' <summary>
    ''' Gets whether script will execute a backup first before executing.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property RequiredBackup As Boolean
        Get
            Return _requiredbackup
        End Get
    End Property

    Dim _requiredexit As Boolean = False

    ''' <summary>
    ''' Gets whther application exit after execution.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property RequiredExit As Boolean
        Get
            Return _requiredexit
        End Get
    End Property

    Dim _requiredshutdown As Boolean

    ''' <summary>
    ''' Gets whether workstation will shutdown after script is executed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property RequiredShutDown As Boolean
        Get
            Return _requiredshutdown
        End Get
    End Property

    Dim _script As StringBuilder = Nothing

    ''' <summary>
    ''' Gets the script's sql statement.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Script As String
        Get
            If _script Is Nothing Then : Return String.Empty
            Else : Return _script.ToString()
            End If
        End Get
    End Property

    Dim _systemversion As String = Application.ProductVersion

    ''' <summary>
    ''' Gets the script's running system version.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SystemVersion As String
        Get
            Return _systemversion
        End Get
    End Property

    Dim _title As String = String.Empty

    ''' <summary>
    ''' Gets the script's title.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Title As String
        Get
            Return _title
        End Get
    End Property
#End Region

#Region "Sub New"
    Dim ConnectionString As String = ""

    ''' <summary>
    ''' Creates a new instance of ScriptReader.
    ''' </summary>
    ''' <param name="filename"></param>
    ''' <remarks></remarks>
    Sub New(ByVal filename As String, ByVal ConString As String)
        _filename = filename
        ConnectionString = ConString
    End Sub
#End Region

#Region "Methods"

    Private Sub ClearValues()
        _author = String.Empty : _contents = Nothing : _lastupdated = Now
        _message = String.Empty : _referenceno = String.Empty : _requiredauthorization = False
        _requiredbackup = False : _requiredexit = False : _requiredshutdown = False
        _script = Nothing : _systemversion = Application.ProductVersion : _title = String.Empty
    End Sub

    ''' <summary>
    ''' Reads the specified script file to load the scripts information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Read()
        ClearValues()
        _contents = Nothing : _contents = New StringBuilder
        _contents.Append(EDIReader.Read(Filename, "ftx"))

        If Not String.IsNullOrEmpty(_contents.ToString.Trim) Then
            Dim values() As String = _contents.ToString.Trim.Split("|")

            If values.Length > 1 Then
                _referenceno = values(1)
                _systemversion = values(2)
                _author = values(3)
                _title = values(4)
                _script = Nothing : _script = New StringBuilder
                _script.Append(values(5).Replace("jsphNewLine;", Chr(13)).Replace("jsphClip;", "|"))
                _message = values(6)
                _requiredauthorization = CBool(values(7))
                _requiredbackup = CBool(values(8))
                _requiredexit = CBool(values(9))
                _requiredshutdown = CBool(values(10))
                _lastupdated = CDate(values(11))
            End If
        End If
    End Sub
#End Region

#Region "Functions"
    Dim runningdelegate As Func(Of Boolean) = Nothing

    ''' <summary>
    ''' Executes the loaded sql script asynchronously (must call EndExecute function to get the actual result).
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BeginExecute() As IAsyncResult
        runningdelegate = Nothing
        runningdelegate = New Func(Of Boolean)(AddressOf ExecuteScript)
        Return runningdelegate.BeginInvoke(Nothing, runningdelegate)
    End Function

    ''' <summary>
    ''' Determines whether script was executed successfully executed asynchronously or not.
    ''' </summary>
    ''' <param name="asyncresult">IAsyncResult interface produced by a BeginExecute call.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EndExecute(ByVal asyncresult As IAsyncResult) As Boolean
        If runningdelegate Is Nothing Then : Return False
        Else : Return runningdelegate.EndInvoke(asyncresult)
        End If
    End Function

    ''' <summary>
    ''' Executes the loaded sql script.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExecuteScript() As Boolean
        Dim executed As Boolean = False

        If _script IsNot Nothing Then
            If Not String.IsNullOrEmpty(_script.ToString.Trim) Then
                Dump.MaxAllowedPacket = 500
                executed = Dump.Execute(ConnectionString, _script.ToString.Replace("jsphNewLine;", vbNewLine))
            End If
        End If

        Return executed
    End Function

    ''' <summary>
    ''' Reads the specified script file to load the scripts information asynchronously.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReadAsync() As IAsyncResult
        Dim delRead As New Action(AddressOf Read)
        Return delRead.BeginInvoke(Nothing, delRead)
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