''' <summary>
''' Class for file compression and / or archive file extraction.
''' </summary>
''' <remarks></remarks>
Public Class Archiver
    Implements IDisposable

#Region "Enumerations"
    ''' <summary>
    ''' Supported archiving tools.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ArchivingToolEnum
        ''' <summary>
        ''' 7Zip archiving application.
        ''' </summary>
        ''' <remarks></remarks>
        SevenZip = 0
        ''' <summary>
        ''' WinRar archiving application.
        ''' </summary>
        ''' <remarks></remarks>
        WinRar = 1
    End Enum

    ''' <summary>
    ''' Archiving methods.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ArchivingMethodEnum
        ''' <summary>
        ''' Insert : puts the whole specified file / directory in the archive file.
        ''' </summary>
        ''' <remarks></remarks>
        Insert = 0
        ''' <summary>
        ''' Append : puts a copy of specified file / directory inside the archive file.
        ''' </summary>
        ''' <remarks></remarks>
        Append = 1
    End Enum
#End Region

#Region "Sub New"
    ''' <summary>
    ''' Creates a new instance of TarsierEyes.Archiver.
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        _path = String.Empty
        _archivingtool = ArchivingToolEnum.SevenZip
        _processwinstyle = ProcessWindowStyle.Hidden
        _archivemethod = ArchivingMethodEnum.Insert
    End Sub

    ''' <summary>
    '''  Creates a new instance of TarsierEyes.Archiver.
    ''' </summary>
    ''' <param name="path">Path of the file or directory to compress.</param>
    ''' <remarks></remarks>
    Sub New(ByVal path As String)
        _path = path
        _archivingtool = ArchivingToolEnum.SevenZip
        _processwinstyle = ProcessWindowStyle.Hidden
        _archivemethod = ArchivingMethodEnum.Insert
        _ArchivedPath = String.Empty
    End Sub

    ''' <summary>
    '''  Creates a new instance of TarsierEyes.Archiver.
    ''' </summary>
    ''' <param name="path">Path of the file or directory to compress.</param>
    ''' <param name="archiver">Compression tool use.</param>
    ''' <remarks></remarks>
    Sub New(ByVal path As String, ByVal archiver As ArchivingToolEnum)
        _path = path
        _archivingtool = archiver
        _processwinstyle = ProcessWindowStyle.Hidden
        _archivemethod = ArchivingMethodEnum.Insert
        _ArchivedPath = String.Empty
    End Sub

    ''' <summary>
    '''  Creates a new instance of TarsierEyes.Archiver.
    ''' </summary>
    ''' <param name="path">Path of the file or directory to compress.</param>
    ''' <param name="archiver">Archiving tool use.</param>
    ''' <param name="winstyle">Command prompt window visibility upon compression.</param>
    ''' <remarks></remarks>
    Sub New(ByVal path As String, ByVal archiver As ArchivingToolEnum, ByVal winstyle As ProcessWindowStyle)
        _path = path
        _archivingtool = archiver
        _processwinstyle = winstyle
        _archivemethod = ArchivingMethodEnum.Insert
        _ArchivedPath = String.Empty
    End Sub

    ''' <summary>
    '''  Creates a new instance of TarsierEyes.Archiver.
    ''' </summary>
    ''' <param name="path">Path of the file or directory to compress.</param>
    ''' <param name="archiver">Archiving tool use.</param>
    ''' <param name="winstyle">Command prompt window visibility upon compression.</param>
    ''' <param name="method">Archiving method to use.</param>
    ''' <remarks></remarks>
    Sub New(ByVal path As String, ByVal archiver As ArchivingToolEnum, ByVal winstyle As ProcessWindowStyle, ByVal method As ArchivingMethodEnum)
        _path = path
        _archivingtool = archiver
        _processwinstyle = winstyle
        _archivemethod = method
        _ArchivedPath = String.Empty
    End Sub
#End Region

#Region "Properties"
    Dim _archivedpath As String = String.Empty
    ''' <summary>
    ''' Gets archive's path after successful compression.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ArchivedFile() As String
        Get
            Return _archivedpath
        End Get
    End Property

    ''' <summary>
    ''' Gets whether specified compression tool is available or not.
    ''' </summary>
    ''' <param name="archiver"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Archivers(ByVal archiver As ArchivingToolEnum) As Boolean
        Get
            Dim exist As Boolean = False : ExtractResourceApplications()

            Select Case archiver
                Case ArchivingToolEnum.SevenZip : exist = File.Exists(Application.StartupPath & "\7z.exe")
                Case ArchivingToolEnum.WinRar : exist = File.Exists(Application.StartupPath & "\Rar.exe")
                Case Else
            End Select

            RemoveResourceApplications()

            Return exist
        End Get
    End Property

    Dim _archivemethod As ArchivingMethodEnum = ArchivingMethodEnum.Insert
    ''' <summary>
    ''' Gets or sets archiving method.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ArchivingMethod() As ArchivingMethodEnum
        Get
            Return _archivemethod
        End Get
        Set(ByVal value As ArchivingMethodEnum)
            _archivemethod = value
        End Set
    End Property

    Dim _archivingtool As ArchivingToolEnum = ArchivingToolEnum.SevenZip
    ''' <summary>
    ''' Gets or sets compression tool to use.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ArchivingTool() As ArchivingToolEnum
        Get
            Return _archivingtool
        End Get
        Set(ByVal value As ArchivingToolEnum)
            _archivingtool = value
        End Set
    End Property

    Dim _path As String = String.Empty
    ''' <summary>
    ''' Gets or sets file's / directory's path to compress.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Path() As String
        Get
            Return _path
        End Get
        Set(ByVal value As String)
            _path = value
        End Set
    End Property

    Dim _processwinstyle As ProcessWindowStyle = Diagnostics.ProcessWindowStyle.Hidden

    ''' <summary>
    ''' Gets or sets command prompt's window status upon compression.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ProcessWindowStyle() As ProcessWindowStyle
        Get
            Return _processwinstyle
        End Get
        Set(ByVal value As ProcessWindowStyle)
            _processwinstyle = value
        End Set
    End Property
#End Region

#Region "Methods"
    Private Sub ExtractResourceApplications()
        RemoveResourceApplications() : Dim curdir As String = Application.StartupPath & "\Archiving Tools"

        Dim fi As FileInfo = My.Resources.SevenZ.ToFileObject("exe", curdir)
        If fi IsNot Nothing Then
            Try
                If IO.File.Exists(fi.FullName) Then My.Computer.FileSystem.RenameFile(fi.FullName, "7z.exe")
            Catch ex As Exception
            End Try
        End If

        fi = My.Resources.SevenZDll.ToFileObject("dll", curdir)
        If fi IsNot Nothing Then
            Try
                If IO.File.Exists(fi.FullName) Then My.Computer.FileSystem.RenameFile(fi.FullName, "7z.dll")
            Catch ex As Exception
            End Try
        End If

        fi = My.Resources.SevenZG.ToFileObject("exe", curdir)
        If fi IsNot Nothing Then
            Try
                If IO.File.Exists(fi.FullName) Then My.Computer.FileSystem.RenameFile(fi.FullName, "7zG.exe")
            Catch ex As Exception
            End Try
        End If

        fi = My.Resources.WinRAR.ToFileObject("exe", curdir)
        If fi IsNot Nothing Then
            Try
                If IO.File.Exists(fi.FullName) Then My.Computer.FileSystem.RenameFile(fi.FullName, "WinRar.exe")
            Catch ex As Exception
            End Try
        End If

        If Directory.Exists(curdir) Then
            If Not File.Exists(Application.StartupPath & "\7z.dll") And _
               File.Exists(curdir & "\7z.dll") Then
                Try
                    File.Copy(curdir & "\7z.dll", Application.StartupPath & "\7z.dll")
                Catch ex As Exception
                End Try
            End If

            If Not File.Exists(Application.StartupPath & "\7z.exe") And _
             File.Exists(curdir & "\7z.exe") Then
                Try
                    File.Copy(curdir & "\7z.exe", Application.StartupPath & "\7z.exe")
                Catch ex As Exception
                End Try
            End If

            If Not File.Exists(Application.StartupPath & "\7zG.exe") And _
             File.Exists(curdir & "\7zG.exe") Then
                Try
                    File.Copy(curdir & "\7zG.exe", Application.StartupPath & "\7zG.exe")
                Catch ex As Exception
                End Try
            End If

            If Not File.Exists(Application.StartupPath & "\WinRar.exe") And _
             File.Exists(curdir & "\WinRar.exe") Then
                Try
                    File.Copy(curdir & "\WinRar.exe", Application.StartupPath & "\WinRar.exe")
                Catch ex As Exception
                End Try
            End If

            If Directory.Exists(curdir) Then
                Try
                    Directory.Delete(curdir, True)
                Catch ex As Exception
                End Try
            End If
        End If
    End Sub

    Private Sub RemoveResourceApplications()
        Dim curdir As String = Application.StartupPath & "\Archiving Tools"

        If Directory.Exists(curdir) Then
            Try
                Directory.Delete(curdir, True)
            Catch ex As Exception
            End Try
        End If

        If File.Exists(Application.StartupPath & "\7z.dll") Then
            Try
                File.Delete(Application.StartupPath & "\7z.dll")
            Catch ex As Exception
                Dim s As String = ex.Message
            End Try
        End If

        If File.Exists(Application.StartupPath & "\7z.exe") Then
            Try
                File.Delete(Application.StartupPath & "\7z.exe")
            Catch ex As Exception
                Dim s As String = ex.Message
            End Try
        End If

        If File.Exists(Application.StartupPath & "\7zG.exe") Then
            Try
                File.Delete(Application.StartupPath & "\7zG.exe")
            Catch ex As Exception
                Dim s As String = ex.Message
            End Try
        End If

        If File.Exists(Application.StartupPath & "\WinRar.exe") Then
            Try
                File.Delete(Application.StartupPath & "\WinRar.exe")
            Catch ex As Exception
                Dim s As String = ex.Message
            End Try
        End If
    End Sub
#End Region

#Region "Internal Functions"
    ''' <summary>
    ''' Perform file / directory compression.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Archive() As FileInfo
        _archivedpath = String.Empty : Dim pathexist As Boolean = False

        If IO.Path.HasExtension(_path) Then : pathexist = File.Exists(_path)
        Else : pathexist = Directory.Exists(_path)
        End If

        If pathexist Then
            ExtractResourceApplications()

            Select Case _archivingtool
                Case ArchivingToolEnum.SevenZip
                    If File.Exists(Application.StartupPath & "\7z.exe") Then
                        If File.Exists(_path.Replace(IO.Path.GetExtension(_path), ".7z")) Then
                            Try
                                My.Computer.FileSystem.DeleteFile(_path.Replace(IO.Path.GetExtension(_path), ".7z"), FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
                            Catch ex As Exception
                            End Try
                        End If

                        Dim proc As New Process
                        With proc
                            .StartInfo.Arguments = "a """ & _path.Replace(IO.Path.GetExtension(_path), ".7z") & """ """ & _path & """"
                            .StartInfo.FileName = Application.StartupPath & "\7z.exe"
                            .StartInfo.CreateNoWindow = (_processwinstyle = Diagnostics.ProcessWindowStyle.Hidden) : .StartInfo.WindowStyle = _processwinstyle
                            .Start()
                            While Not .HasExited
                                Application.DoEvents()
                            End While
                            .Dispose()
                        End With

                        If File.Exists(_path.Replace(IO.Path.GetExtension(_path), ".7z")) Then
                            _archivedpath = _path.Replace(IO.Path.GetExtension(_path), ".7z")
                            Dim afile As New FileInfo(_path.Replace(IO.Path.GetExtension(_path), ".7z"))
                            If _archivemethod = ArchivingMethodEnum.Insert Then
                                Try
                                    If IO.Path.HasExtension(_path) Then : My.Computer.FileSystem.DeleteFile(_path, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
                                    Else : My.Computer.FileSystem.DeleteDirectory(_path, FileIO.DeleteDirectoryOption.DeleteAllContents, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
                                    End If
                                Catch ex As Exception
                                End Try
                            End If

                            RemoveResourceApplications()
                            Return afile : Exit Function
                        End If
                    End If

                Case ArchivingToolEnum.WinRar
                    If File.Exists(Application.StartupPath & "\WinRar.exe") Then
                        If File.Exists(_path.Replace(IO.Path.GetExtension(_path), ".rar")) Then
                            Try
                                My.Computer.FileSystem.DeleteFile(_path.Replace(IO.Path.GetExtension(_path), ".rar"), FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
                            Catch ex As Exception

                            End Try
                        End If

                        Dim proc As New Process
                        With proc
                            .StartInfo.Arguments = "a -ep """ & _path.Replace(IO.Path.GetExtension(_path), ".rar") & """ """ & _path & """"
                            .StartInfo.FileName = Application.StartupPath & "\WinRar.exe"
                            .StartInfo.CreateNoWindow = (_processwinstyle = Diagnostics.ProcessWindowStyle.Hidden) : .StartInfo.WindowStyle = _processwinstyle
                            .Start()
                            While Not .HasExited
                                Application.DoEvents()
                            End While
                            .Dispose()
                        End With

                        If File.Exists(_path.Replace(IO.Path.GetExtension(_path), ".rar")) Then
                            _archivedpath = _path.Replace(IO.Path.GetExtension(_path), ".rar")
                            Dim afile As New FileInfo(_path.Replace(IO.Path.GetExtension(_path), ".rar"))
                            If _archivemethod = ArchivingMethodEnum.Insert Then
                                Try
                                    If IO.Path.HasExtension(_path) Then : My.Computer.FileSystem.DeleteFile(_path, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
                                    Else : My.Computer.FileSystem.DeleteDirectory(_path, FileIO.DeleteDirectoryOption.DeleteAllContents, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
                                    End If
                                Catch ex As Exception

                                End Try
                            End If
                            RemoveResourceApplications()
                            Return afile : Exit Function
                        End If
                    End If
            End Select
        End If

        RemoveResourceApplications() : Return Nothing
    End Function

    ''' <summary>
    ''' Performs archive extraction using the chosen archiving tool.
    ''' </summary>
    ''' <param name="destinationpath">Destination path for the extracted file(s).</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Extract(ByVal destinationpath As String) As Boolean
        Dim bExtract As Boolean = False : Dim sError As String = "-"
        ExtractResourceApplications()

        Select Case _archivingtool
            Case ArchivingToolEnum.SevenZip
                Dim proc As New Process
                With proc
                    .StartInfo.Arguments = "e """ & _path & """ -o""" & destinationpath & """ *.* -r"
                    .StartInfo.FileName = Application.StartupPath & "\7z.exe"
                    .StartInfo.CreateNoWindow = (_processwinstyle = Diagnostics.ProcessWindowStyle.Hidden)
                    .StartInfo.WindowStyle = _processwinstyle
                    .StartInfo.RedirectStandardError = True
                    .StartInfo.UseShellExecute = False
                    .Start()
                    While .HasExited = False
                        Application.DoEvents()
                    End While

                    sError = .StandardError.ReadToEnd

                    .Dispose()
                End With

            Case ArchivingToolEnum.WinRar
                Dim proc As New Process
                With proc
                    .StartInfo.Arguments = "e """ & _path & """ *.* """ & destinationpath & """"
                    .StartInfo.FileName = Application.StartupPath & "\WinRar.exe"
                    .StartInfo.CreateNoWindow = (_processwinstyle = Diagnostics.ProcessWindowStyle.Hidden)
                    .StartInfo.WindowStyle = _processwinstyle
                    .StartInfo.RedirectStandardError = True
                    .StartInfo.UseShellExecute = False
                    .Start()
                    While .HasExited = False
                        Application.DoEvents()
                    End While

                    sError = .StandardError.ReadToEnd

                    .Dispose()
                End With

            Case Else
        End Select

        bExtract = String.IsNullOrEmpty(sError.Replace("The handle is invalid.", String.Empty).Trim)
        RemoveResourceApplications()

        Return bExtract
    End Function
#End Region

#Region "Shared Functions"
    ''' <summary>
    ''' Validates if archiving tools are existing and can be use to operate.
    ''' </summary>
    ''' <param name="archiver">Archiving tool to check.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CanUse(ByVal archiver As ArchivingToolEnum) As Boolean
        Dim a As New Archiver
        Dim b As Boolean = a.Archivers(archiver)
        a.Dispose()
        Return b
    End Function

    ''' <summary>
    ''' Performs file compression using selected archiving tool, file(s) will just be copied into the archive file.
    ''' </summary>
    ''' <param name="path">File / directory path to archive.</param>
    ''' <param name="archiver">Archiving tool to use.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CompressAdd(ByVal path As String, ByVal archiver As ArchivingToolEnum) As FileInfo
        Dim a As New Archiver(path, archiver, Diagnostics.ProcessWindowStyle.Hidden, ArchivingMethodEnum.Append)
        Dim f As FileInfo = a.Archive : a.Dispose()
        Return f
    End Function

    ''' <summary>
    ''' Performs file compression using selected archiving tool, file(s) will be inserted directly to the archive file.
    ''' </summary>
    ''' <param name="path">File / directory path to archive.</param>
    ''' <param name="archiver">Archiving tool to use.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CompressInsert(ByVal path As String, ByVal archiver As ArchivingToolEnum) As FileInfo
        Dim a As New Archiver(path, archiver, Diagnostics.ProcessWindowStyle.Hidden, ArchivingMethodEnum.Insert)
        Dim f As FileInfo = a.Archive : a.Dispose()
        Return f
    End Function

    ''' <summary>
    ''' Performs file extraction from a compressed file into the specified destination folder using the selected archiving tool.
    ''' </summary>
    ''' <param name="filename">Compressed file's filename.</param>
    ''' <param name="destination">Destination path for the extracted file(s).</param>
    ''' <param name="archiver">Archiving tool to use.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Decompress(ByVal filename As String, ByVal destination As String, ByVal archiver As ArchivingToolEnum) As Boolean
        Dim a As New Archiver(filename, archiver, Diagnostics.ProcessWindowStyle.Hidden)
        Dim b As Boolean = a.Extract(destination) : a.Dispose()
        Return b
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
                RemoveResourceApplications()
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