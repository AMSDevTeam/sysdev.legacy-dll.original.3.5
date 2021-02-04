''' <summary>
''' Loading form dialog.
''' </summary>
''' <remarks></remarks>
Public Class LoadScreen
    Implements IDisposable

#Region "Enumerations"
    ''' <summary>
    ''' Loading image.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum LoadingImageEnum
        ''' <summary>
        ''' Conventional ajax circle loading image (black).
        ''' </summary>
        ''' <remarks></remarks>
        AjaxLoadBlack = 0
        ''' <summary>
        ''' Conventional ajax circle loading image (blue).
        ''' </summary>
        ''' <remarks></remarks>
        AjaxLoadBlue = 1
        ''' <summary>
        ''' Conventional ajax circle loading image (red).
        ''' </summary>
        ''' <remarks></remarks>
        AjaxLoadRed = 2
        ''' <summary>
        ''' Custom loading image assigned thru CustomLoadImage Shared Property.
        ''' </summary>
        ''' <remarks></remarks>
        Custom = 3
    End Enum
#End Region

    Shared _CustomImage As Image = Nothing
    Shared _LoadingImage As LoadingImageEnum = LoadingImageEnum.AjaxLoadBlack
    Dim _load As New LoadingForm

#Region "Properties"
    ''' <summary>
    ''' Gets or sets loading image to use.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property LoadingImage() As LoadingImageEnum
        Get
            Return _LoadingImage
        End Get
        Set(ByVal value As LoadingImageEnum)
            _LoadingImage = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets custom loading image when LoadingImage Shared Property is set to Custom.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property CustomImage() As Image
        Get
            Return _CustomImage
        End Get
        Set(ByVal value As Image)
            _CustomImage = value
        End Set
    End Property

    ''' <summary>
    ''' Gets whether LoadingScreen is currently displayed on the screen or not.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsDisplayed() As Boolean
        Get
            Dim bDisplayed As Boolean = False
            If Not _load.IsDisposed Then bDisplayed = _load.IsOpen
            Return bDisplayed
        End Get
    End Property

#End Region

#Region "Sub New"
    ''' <summary>
    ''' Creates a new instance of LoadScreen.
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        If Not _load.IsDisposed Then
            Try
                _load.Dispose()
            Catch ex As Exception

            End Try
        End If
        _load = Nothing
        _load = New LoadingForm
    End Sub

    ''' <summary>
    ''' Creates a new instance of LoadScreen.
    ''' </summary>
    ''' <param name="thread">Thread to determine the timeline of the loading display.</param>
    ''' <remarks></remarks>
    Sub New(ByVal thread As Thread)
        If Not _load.IsDisposed Then
            Try
                _load.Dispose()
            Catch ex As Exception

            End Try
        End If
        _load = Nothing
        _load = New LoadingForm(thread)
    End Sub

    ''' <summary>
    ''' Creates a new instance of LoadScreen.
    ''' </summary>
    ''' <param name="sync">IASyncResult interface to determine the timeline of the loading display.</param>
    ''' <remarks></remarks>
    Sub New(ByVal sync As IAsyncResult)
        If Not _load.IsDisposed Then
            Try
                _load.Dispose()
            Catch ex As Exception

            End Try
        End If
        _load = Nothing
        _load = New LoadingForm(sync)
    End Sub

#End Region

#Region "Functions"
    ''' <summary>
    ''' Gets the default loading images.
    ''' </summary>
    ''' <param name="image"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLoadImage(ByVal image As LoadingImageEnum) As Image
        Return _load.GetLoadImage(image)
    End Function
#End Region

    ''' <summary>
    ''' Displays the LoadScreen to the user.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Show()
        With _load
            .StartPosition = FormStartPosition.CenterScreen : .IsDialog = False
            .ManageOnDispose() : .Show()
        End With
    End Sub

    ''' <summary>
    ''' Displays the LoadScreen to the user.
    ''' </summary>
    ''' <param name="owner">Any object that implements IWin32Window and represents the top-level window that will own the LoadingScreen.</param>
    ''' <remarks></remarks>
    Public Overloads Sub Show(ByVal owner As IWin32Window)
        With _load
            .StartPosition = FormStartPosition.CenterScreen : .IsDialog = False
            .ManageOnDispose() : .Show(owner)
        End With
    End Sub

    ''' <summary>
    ''' Shows the LoadingScreen as a modal dialog box up until timeline defending on the given System.Threading.Thread or System.IASyncResult is finished or until the Close() method is called.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function ShowDialog() As DialogResult
        With _load
            .ManageOnDispose() : .StartPosition = FormStartPosition.CenterScreen : .IsDialog = True
            Return .ShowDialog
        End With
    End Function

    ''' <summary>
    ''' Shows the LoadingScreen as a modal dialog box up until timeline defending on the given System.Threading.Thread or System.IASyncResult is finished or until the Close() method is called.
    ''' </summary>
    ''' <param name="owner">Any object that implements IWin32Window and represents the top-level window that will own the LoadingScreen.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function ShowDialog(ByVal owner As IWin32Window) As DialogResult
        With _load
            .ManageOnDispose() : .StartPosition = FormStartPosition.CenterScreen : .IsDialog = True
            Return .ShowDialog(owner)
        End With
    End Function

    ''' <summary>
    ''' Closes the LoadingScreen.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Close()
        If _load.IsOpen Then _load.Close()
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
                If Not _load.IsDisposed Then
                    Try
                        _load.Dispose()
                    Catch ex As Exception
                    Finally : _load = Nothing
                    End Try
                End If
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
