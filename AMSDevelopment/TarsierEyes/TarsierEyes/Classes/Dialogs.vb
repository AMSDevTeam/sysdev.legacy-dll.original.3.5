Namespace Common
    Partial Public Class Simple
        ''' <summary>
        ''' Simplified class for creating file dialogs
        ''' </summary>
        ''' <remarks></remarks>
        Public Class Dialogs

#Region "Enumerations"
            ''' <summary>
            ''' Commonly used file extensions.
            ''' </summary>
            ''' <remarks></remarks>
            Public Enum FilterEnum
                ''' <summary>
                ''' Bitmap images.
                ''' </summary>
                ''' <remarks></remarks>
                BMP
                ''' <summary>
                ''' Microsoft Word Document.
                ''' </summary>
                ''' <remarks></remarks>
                DOC
                ''' <summary>
                ''' Microsoft Word Document - Open XML.
                ''' </summary>
                ''' <remarks></remarks>
                DOCX
                ''' <summary>
                ''' Graphics Interchange Format.
                ''' </summary>
                ''' <remarks></remarks>
                GIF
                ''' <summary>
                ''' Icon Images.
                ''' </summary>
                ''' <remarks></remarks>
                ICO
                ''' <summary>
                ''' Joint Photographic Experts Group Images.
                ''' </summary>
                ''' <remarks></remarks>
                JPG
                ''' <summary>
                ''' Portable Document Format.
                ''' </summary>
                ''' <remarks></remarks>
                PDF
                ''' <summary>
                ''' Portable Network Graphics.
                ''' </summary>
                ''' <remarks></remarks>
                PNG
                ''' <summary>
                ''' Text Files.
                ''' </summary>
                ''' <remarks></remarks>
                TXT
                ''' <summary>
                ''' Microsoft Excel Worrksheet.
                ''' </summary>
                ''' <remarks></remarks>
                XLS
                ''' <summary>
                ''' Microsoft Excel Worksheet - Open XML.
                ''' </summary>
                ''' <remarks></remarks>
                XLSX
                ''' <summary>
                ''' Extensive Markup Language Files.
                ''' </summary>
                ''' <remarks></remarks>
                XML
            End Enum
#End Region

#Region "Shared Functions"
            ''' <summary>
            ''' Provides standard dialog filter string format for the file type specified.
            ''' </summary>
            ''' <param name="filter">File type.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Function DialogFilter(ByVal filter As FilterEnum) As String
                Dim sFilter As String = String.Empty

                Select Case filter
                    Case FilterEnum.BMP : sFilter = "Bitmap Images (*.bmp)|*.bmp"
                    Case FilterEnum.DOC : sFilter = "Microsoft Word Document (*.doc)|*.doc"
                    Case FilterEnum.DOCX : sFilter = "Microsoft Word Document - Open XML (*.docx)|*.docx"
                    Case FilterEnum.GIF : sFilter = "Graphics Interchange Format (*.gif)|*.gif"
                    Case FilterEnum.ICO : sFilter = "Icon Images (*.ico)|*.ico"
                    Case FilterEnum.JPG : sFilter = "Joint Photographic Experts Group Images (*.jpg; *.jpeg)|*.jpg; *.jpeg"
                    Case FilterEnum.PDF : sFilter = "Portable Document Format (*.pdf)|*.pdf"
                    Case FilterEnum.PNG : sFilter = "Portbale Network Graphic Images (*.png)|*.png"
                    Case FilterEnum.TXT : sFilter = "Text File (*.txt)|*.txt"
                    Case FilterEnum.XLS : sFilter = "Microsoft Excel Files (*.xls)|*.xls"
                    Case FilterEnum.XLSX : sFilter = "Microsoft Excel Files - Open XML (*.xlsx)|*.xlsx"
                    Case FilterEnum.XML : sFilter = "Extensive Markup Language Files (*.xml)|*.xml"
                    Case Else
                End Select

                Return sFilter
            End Function
#End Region

#Region "SaveDialog"
            ''' <summary>
            ''' Creates an instance of a SaveFileDialog.
            ''' </summary>
            ''' <param name="title">Dialog title text.</param>
            ''' <param name="filter">Dialog filters.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Function SaveDialog(ByVal title As String, ByVal filter As String) As SaveFileDialog
                Return SaveDialog(title, filter, String.Empty)
            End Function

            ''' <summary>
            ''' Creates an instance of a SaveFileDialog.
            ''' </summary>
            ''' <param name="title">Dialog title text.</param>
            ''' <param name="filter">Dialog filters.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Function SaveDialog(ByVal title As String, ByVal filter As DialogFilterCollection) As SaveFileDialog
                Return SaveDialog(title, filter, String.Empty)
            End Function

            ''' <summary>
            ''' Creates an instance of a SaveFileDialog.
            ''' </summary>
            ''' <param name="title">Dialog title text.</param>
            ''' <param name="filter">Dialog filters.</param>
            ''' <param name="defaultextension">Default file extension.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Function SaveDialog(ByVal title As String, ByVal filter As String, ByVal defaultextension As String) As SaveFileDialog
                Dim dlg As New SaveFileDialog
                With dlg
                    .InitialDirectory = String.Empty
                    .CheckPathExists = True : .Title = title
                    .Filter = filter : .DefaultExt = defaultextension
                    Return dlg
                End With
            End Function

            ''' <summary>
            ''' Creates an instance of a SaveFileDialog.
            ''' </summary>
            ''' <param name="title">Dialog title text.</param>
            ''' <param name="filter">Dialog filters.</param>
            ''' <param name="defaultextension">Default file extension.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Function SaveDialog(ByVal title As String, ByVal filter As DialogFilterCollection, ByVal defaultextension As String) As SaveFileDialog
                Dim filters As String = String.Empty

                For Each f As String In filter
                    filters &= IIf(String.IsNullOrEmpty(filters.Trim), String.Empty, IIf(filters.Trim.EndsWith("|"), String.Empty, "|")) & f
                Next

                Return SaveDialog(title, filters, defaultextension)
            End Function
#End Region

#Region "Browse Dialog"
            ''' <summary>
            ''' Creates an instance of OpenFileDialog.
            ''' </summary>
            ''' <param name="title">Dialog title text.</param>
            ''' <param name="filter">Dialog filters.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Function BrowseDialog(ByVal title As String, ByVal filter As String) As OpenFileDialog
                Return BrowseDialog(title, filter, String.Empty)
            End Function

            ''' <summary>
            ''' Creates an instance of OpenFileDialog.
            ''' </summary>
            ''' <param name="title">Dialog title text.</param>
            ''' <param name="filter">Dialog filters.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Function BrowseDialog(ByVal title As String, ByVal filter As DialogFilterCollection) As OpenFileDialog
                Return BrowseDialog(title, filter, String.Empty)
            End Function

            ''' <summary>
            ''' Creates an instance of OpenFileDialog.
            ''' </summary>
            ''' <param name="title">Dialog title text.</param>
            ''' <param name="filter">Dialog filters.</param>
            ''' <param name="defaultextension">Default file extension.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Function BrowseDialog(ByVal title As String, ByVal filter As String, ByVal defaultextension As String) As OpenFileDialog
                Dim dlg As New OpenFileDialog
                With dlg
                    .InitialDirectory = String.Empty
                    .CheckFileExists = True : .CheckPathExists = True
                    .Filter = filter : .DefaultExt = defaultextension
                    .Title = title
                    Return dlg
                End With
            End Function

            ''' <summary>
            ''' Creates an instance of OpenFileDialog.
            ''' </summary>
            ''' <param name="title">Dialog title text.</param>
            ''' <param name="filter">Dialog filters.</param>
            ''' <param name="defaultextension">Default file extension.</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Function BrowseDialog(ByVal title As String, ByVal filter As DialogFilterCollection, ByVal defaultextension As String) As OpenFileDialog
                Dim filters As String = String.Empty

                For Each f As String In filter
                    filters &= IIf(String.IsNullOrEmpty(filters.Trim), String.Empty, IIf(filters.Trim.EndsWith("|"), String.Empty, "|")) & f
                Next

                Return BrowseDialog(title, filters, defaultextension)
            End Function
#End Region

#Region "Custom Classes"
            ''' <summary>
            ''' Collection of common dialog filter strings.
            ''' </summary>
            ''' <remarks></remarks>
            Public Class DialogFilterCollection
                Inherits CollectionBase
                Implements IDisposable

                ''' <summary>
                ''' Gets an item within this instance of DialogFilterCollection
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
                ''' Adds an item in this instance of DialogFilterCollection
                ''' </summary>
                ''' <param name="value">Valid common dialog filter string.</param>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Function Add(ByVal value As String) As Integer
                    Return List.Add(value)
                End Function

                ''' <summary>
                ''' Validates if item is existing in this instance of DialogFilterCollection
                ''' </summary>
                ''' <param name="value">Value to find</param>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Function Contains(ByVal value As String) As Boolean
                    Return List.Contains(value)
                End Function

                ''' <summary>
                ''' Removes an item in this instance of DialogFilterCollection
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
                            List.Clear() : Common.Simple.RefreshAndManageCurrentProcess()
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

        End Class
    End Class
End Namespace
