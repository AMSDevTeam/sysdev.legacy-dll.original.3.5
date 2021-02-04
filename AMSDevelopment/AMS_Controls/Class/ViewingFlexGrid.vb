#Region "Imports"
Imports C1.Win.C1FlexGrid
Imports System.ComponentModel
Imports System.Drawing.Design
Imports TarsierEyes
Imports TarsierEyes.Common
Imports TarsierEyes.Common.Simple
Imports TarsierEyes.Common.SQLStrings
Imports TarsierEyes.Common.Synchronization
Imports TarsierEyes.MySQL
Imports System.Drawing.Drawing2D
Imports MetroFramework.Drawing
Imports MetroFramework.Components
Imports MetroFramework.Interfaces
Imports MetroFramework.Controls
Imports MetroFramework
#End Region

''' <summary>
''' Derived from C1.Win.C1FlexGrid for automated data binding.
''' </summary>
''' <remarks></remarks>
Public Class ViewingFlexGrid
    Inherits C1FlexGrid
    Implements IMetroControl

#Region "Enumerations"
    ''' <summary>
    ''' Grid listview item selection mode enumerations.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ListViewItemSelectionEnum
        ''' <summary>
        ''' On mouse hover.
        ''' </summary>
        ''' <remarks></remarks>
        MouseHover = 0
        ''' <summary>
        ''' On mouse click.
        ''' </summary>
        ''' <remarks></remarks>
        MouseClickAndKeyboard = 1
    End Enum
    ''' <summary>
    ''' No data display enumerations.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum NoDataDisplayEnum
        ''' <summary>
        ''' Grid will display default loaded columns when there is no data.
        ''' </summary>
        ''' <remarks></remarks>
        AsDisplayed = 0
        ''' <summary>
        ''' Grid will display 'No item(s) could be displayed in this view' message when there is not data.
        ''' </summary>
        ''' <remarks></remarks>
        NoItemsCouldBeViewedDisplay = 1
    End Enum
#End Region

#Region "Events"
    ''' <summary>
    ''' Fires up after data has been loaded to the grid.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event AfterDataLoading(ByVal sender As Object, ByVal e As DataLoadingEventArgs)
    ''' <summary>
    ''' Fires up when after a row node is selected by the user (fires only if grid is in treeview mode).
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event AfterNodeSelect(ByVal sender As Object, ByVal e As RowNodeEventArgs)

    ''' <summary>
    ''' Fires up after deleting a row
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event AfterDeletingRows(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs)

    ''' <summary>
    ''' Fires up when before deleting a row
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event BeforeDeletingRows(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs)

    Public Event BeforeChangedRowChecked(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs)

    Public Event BeforeDataSourceChanged(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs)

    ''' <summary>
    ''' Fires up after a cell is double clicked by the user (fires only if grid is in listview mode).
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Public Event ListItemDoubleClick(ByVal sender As Object, ByVal e As RowListEventArgs)

    Public Event EditClick(ByVal sender As Object, ByVal e As EventArgs)

    Public Event DeleteClick(ByVal sender As Object, ByVal e As EventArgs)
#End Region

#Region "Custom Classes"
    ''' <summary>
    ''' Data loading event arguments.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DataLoadingEventArgs
        ''' <summary>
        ''' Determines the data table to be / has been binded to the grid.
        ''' </summary>
        ''' <remarks></remarks>
        Public BindedTable As DataTable = Nothing
        ''' <summary>
        ''' Determines number of data soure columns binded to the grid.
        ''' </summary>
        ''' <remarks></remarks>
        Public Columns As Integer = 0
        ''' <summary>
        ''' Determines error message encountered with upon data binding.
        ''' </summary>
        ''' <remarks></remarks>
        Public ErrorMessage As String = String.Empty
        ''' <summary>
        ''' Determines whether the data has been loaded to grid or not.
        ''' </summary>
        ''' <remarks></remarks>
        Public Loaded As Boolean = False
        ''' <summary>
        ''' Determines query statement to / has been executed.
        ''' </summary>
        ''' <remarks></remarks>
        Public QueryStatement As String = String.Empty
        ''' <summary>
        ''' Determines number of rows loaded in the grid.
        ''' </summary>
        ''' <remarks></remarks>
        Public Rows As Integer = 0
    End Class

    ''' <summary>
    ''' Data source binding settings.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DataSourceInfo
        ''' <summary>
        ''' Gets or sets the data source's SQL statement.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CommandText As String = String.Empty
        ''' <summary>
        ''' Gets or sets the data source's database connection string.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConnectionString As String = String.Empty
    End Class

    ''' <summary>
    ''' Gradient color patterns.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GradientColorInfo
        Dim _backcolor1 As Color = Color.Transparent
        ''' <summary>
        ''' Gets or sets the 1st backcolor pattern.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BackColor1 As Color
            Get
                Return _backcolor1
            End Get
            Set(ByVal value As Color)
                _backcolor1 = value : DrawGradient()
            End Set
        End Property

        Dim _backcolor2 As Color = Color.Transparent

        ''' <summary>
        ''' Gets or sets the 2nd backcolor pattern.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BackColor2 As Color
            Get
                Return _backcolor2
            End Get
            Set(ByVal value As Color)
                _backcolor2 = value : DrawGradient()
            End Set
        End Property

        Dim _grid As ViewingFlexGrid = Nothing

        ''' <summary>
        ''' Creates a new instance of GradientColorInfo.
        ''' </summary>
        ''' <remarks></remarks>
        Sub New(ByVal grid As ViewingFlexGrid)
            _grid = grid
        End Sub

        Private Sub DrawGradient()
            If _grid IsNot Nothing Then
                If _grid.IsTreeView Or
                   (BackColor1.ToArgb <> Color.Transparent.ToArgb And
                    BackColor2.ToArgb <> Color.Transparent.ToArgb) Then
                    Simple.Redraw(_grid, False)
                    If _grid.Parent IsNot Nothing Then _grid.Parent.Update()
                    Simple.Redraw(_grid)
                End If
            End If
        End Sub
    End Class

    ''' <summary>
    ''' Collection of list items.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ListItemCollection
        Inherits ListView.ListViewItemCollection

        Dim _grid As ViewingFlexGrid = Nothing
        Dim _imagelist As ImageList = Nothing
        Dim _lstcollection As New List(Of ListViewItem)

        ''' <summary>
        ''' Creates a new instance of ListItemCollection.
        ''' </summary>
        ''' <param name="grid"></param>
        ''' <remarks></remarks>
        Sub New(ByVal grid As ViewingFlexGrid)
            MyBase.New(Nothing)
            _grid = grid : _imagelist = _grid.ImageList
        End Sub

#Region "Overrides"
        ''' <summary>
        ''' Removes all item in the collection.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Sub Clear()
            _lstcollection.Clear()
            If _grid IsNot Nothing Then
                With _grid
                    Simple.Redraw(_grid, False)
                    Try
                        If .DataSource IsNot Nothing Then CType(.DataSource, DataTable).Dispose()
                    Catch ex As Exception
                    Finally : .DataSource = Nothing
                    End Try
                    .Clear(ClearFlags.All) : .Rows.Count = 1 : .Cols.Count = (.Size.Width / 80)
                    For Each c As Column In .Cols
                        c.Width = 100
                    Next
                    .SelectionMode = SelectionModeEnum.Cell
                    .Tree.Clear() : .Subtotal(AggregateEnum.Clear)
                    .Tree.Column = -1 : .Styles.Normal.Margins.Top = 5
                    .Styles.EmptyArea.BackColor = Color.Transparent
                    .Styles.EmptyArea.Border.Color = Color.Transparent
                    .Styles.Normal.Border.Color = Color.Transparent
                    .Styles.Focus.BackgroundImage = My.Resources.highligthedwithborder
                    .Styles.Focus.BackgroundImageLayout = ImageAlignEnum.Stretch
                    .Styles.Normal.WordWrap = True : .Styles.Focus.WordWrap = True
                    .Styles.Normal.ImageAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterTop
                    .Styles.Normal.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterBottom
                    .Cols(0).Visible = False : .Rows(0).Visible = False : .HighLight = HighLightEnum.Always
                    .Styles.Highlight.ForeColor = Color.Black
                    .Styles.Highlight.BackgroundImage = My.Resources.highligthedwithborder
                    .Styles.Highlight.BackgroundImageLayout = ImageAlignEnum.TileStretch
                    .Styles.Highlight.Border.Color = Color.Cornsilk
                    .AllowDelete = False : .AllowAddNew = False : .AllowEditing = False
                    Simple.Redraw(_grid)
                End With
            End If
        End Sub

        Dim _lstimage As Image = Nothing
        Dim _added As Boolean = False

        ''' <summary>
        ''' Creates and adds a ListViewItem in the collection with the specified information.
        ''' </summary>
        ''' <param name="text"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Add(ByVal text As String) As System.Windows.Forms.ListViewItem
            Dim _lst As New ListViewItem(text)
            Return Add(_lst)
        End Function

        ''' <summary>
        ''' Creates and adds a ListViewItem in the collection with the specified information.
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="imageIndex"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Add(ByVal text As String, ByVal imageIndex As Integer) As System.Windows.Forms.ListViewItem
            Dim _lst As New ListViewItem(text, imageIndex)
            Return Add(_lst)
        End Function

        ''' <summary>
        ''' Creates and adds a ListViewItem in the collection with the specified information.
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="imageKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Add(ByVal text As String, ByVal imageKey As String) As System.Windows.Forms.ListViewItem
            Dim _lst As New ListViewItem(text, imageKey)
            Return Add(_lst)
        End Function

        ''' <summary>
        ''' Creates and adds a ListViewItem in the collection with the specified information.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <param name="text"></param>
        ''' <param name="imageIndex"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Add(ByVal key As String, ByVal text As String, ByVal imageIndex As Integer) As System.Windows.Forms.ListViewItem
            Dim _lst As New ListViewItem(text, imageIndex)
            _lst.Name = key
            Return Add(_lst)
        End Function

        ''' <summary>
        ''' Creates and adds a ListViewItem in the collection with the specified information.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <param name="text"></param>
        ''' <param name="imageKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Add(ByVal key As String, ByVal text As String, ByVal imageKey As String) As System.Windows.Forms.ListViewItem
            Dim _lst As New ListViewItem(text, imageKey)
            _lst.Name = key
            Return Add(_lst)
        End Function

        ''' <summary>
        ''' Adds the specified ListViewItem in the collection
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Add(ByVal value As System.Windows.Forms.ListViewItem) As System.Windows.Forms.ListViewItem
            _lstcollection.Add(value)

            If _grid IsNot Nothing Then
                _imagelist = _grid.ImageList
                If _imagelist IsNot Nothing Then
                    If Not String.IsNullOrEmpty(value.ImageKey.Trim) Then : _lstimage = _imagelist.Images(value.ImageKey)
                    Else : _lstimage = _imagelist.Images(value.ImageIndex)
                    End If
                Else : _lstimage = Nothing
                End If

                ResizeImage(_lstimage)

                With _grid
                    _added = False
                    For Each rw As Row In .Rows
                        If rw.Index >= 1 Then
                            _added = False
                            For i As Integer = 1 To .Cols.Count - 1
                                If .GetCellImage(rw.Index, i) Is Nothing And
                                   .GetData(rw.Index, i) Is Nothing Then
                                    .SetCellImage(rw.Index, i, _lstimage)
                                    .SetData(rw.Index, i, value.Text)
                                    .SetUserData(rw.Index, i, value)
                                    .AutoSizeRow(rw.Index) : _added = True
                                    '  .Row = rw.Index : .Col = i
                                    .Row = -1 : .Col = -1
                                    Exit For
                                End If
                            Next
                            If _added Then Exit For
                        End If
                    Next

                    If Not _added Then
                        .Rows.Add()
                        .SetCellImage(.Rows.Count - 1, 1, _lstimage)
                        .SetData(.Rows.Count - 1, 1, value.Text)
                        .SetUserData(.Rows.Count - 1, 1, value)
                        '.Row = .Rows.Count - 1 : .Col = 1
                        .Row = -1 : .Col = -1
                        .AutoSizeRow(.Rows.Count - 1)
                    End If
                End With
            End If

            Return value
        End Function

        ''' <summary>
        ''' Determines whether the specified item is existing within the collection.
        ''' </summary>
        ''' <param name="listviewitem"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Contains(ByVal listviewitem As ListViewItem) As Boolean
            Return _lstcollection.Contains(listviewitem)
        End Function

        ''' <summary>
        ''' Determines whether the collection contains an item with the specified key.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ContainsKey(ByVal key As String) As Boolean
            Dim _contains As Boolean = False

            For Each l As ListViewItem In _lstcollection
                If l.Name = key Then
                    _contains = True : Exit For
                End If
            Next

            Return MyBase.ContainsKey(key)
        End Function

        ''' <summary>
        ''' Removes the specified item from teh collection.
        ''' </summary>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Public Overrides Sub Remove(ByVal item As System.Windows.Forms.ListViewItem)
            _lstcollection.Remove(item)
        End Sub

        ''' <summary>
        ''' Removes the element at the specified index of the collection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <remarks></remarks>
        Public Overrides Sub RemoveAt(ByVal index As Integer)
            _lstcollection.RemoveAt(index)
        End Sub

        ''' <summary>
        ''' Removes the item with the specified key in the collection.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <remarks></remarks>
        Public Overrides Sub RemoveByKey(ByVal key As String)
            For Each l As ListViewItem In _lstcollection
                If l.Name = key Then Remove(l)
            Next
        End Sub

        ''' <summary>
        ''' Retrieves the item in the collection with the specified key.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function IndexOfKey(ByVal key As String) As Integer
            Dim i As Integer = -1

            For l As Integer = 0 To _lstcollection.Count - 1
                If _lstcollection.Item(l).Name = key Then
                    i = l : Exit For
                End If
            Next

            Return i
        End Function

        ''' <summary>
        ''' Insert a ListViewItem in the specified index of the collection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <param name="key"></param>
        ''' <param name="text"></param>
        ''' <param name="imageIndex"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Insert(ByVal index As Integer, ByVal key As String, ByVal text As String, ByVal imageIndex As Integer) As System.Windows.Forms.ListViewItem
            Dim _lst As New ListViewItem(text, imageIndex)
            _lst.Name = key : _lstcollection.Insert(index, _lst)
            Return _lst
        End Function

        ''' <summary>
        ''' Insert a ListViewItem in the specified index of the collection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <param name="key"></param>
        ''' <param name="text"></param>
        ''' <param name="imageKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Insert(ByVal index As Integer, ByVal key As String, ByVal text As String, ByVal imageKey As String) As System.Windows.Forms.ListViewItem
            Dim _lst As New ListViewItem(text, imageKey)
            _lst.Name = key : _lstcollection.Insert(index, _lst)
            Return _lst
        End Function
#End Region

#Region "Overriden Properties"

        ''' <summary>
        ''' Gets the number of elements in the collection.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads ReadOnly Property Count As Integer
            Get
                Return _lstcollection.Count
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the ListViewItem in the specified index of the collection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Overrides Property Item(ByVal index As Integer) As System.Windows.Forms.ListViewItem
            Get
                Return _lstcollection(index)
            End Get
            Set(ByVal value As System.Windows.Forms.ListViewItem)
                _lstcollection(index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the ListViewItem with the specified key in the collection.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Overrides ReadOnly Property Item(ByVal key As String) As System.Windows.Forms.ListViewItem
            Get
                Dim _lst As ListViewItem = Nothing

                For Each l As ListViewItem In _lstcollection
                    If l.Name = key Then
                        _lst = l : Exit For
                    End If
                Next

                Return _lst
            End Get
        End Property
#End Region

        Private Sub ResizeImage(ByRef img As Image)
            If img IsNot Nothing Then
                Dim bm As New Bitmap(img)
                Dim width As Integer = 32 : Dim height As Integer = 32
                Dim thumb As New Bitmap(width, height)
                Dim g As Graphics = Graphics.FromImage(thumb)

                With g
                    .InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    .DrawImage(bm, New Rectangle(0, 0, width, height), New Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel)
                    .Dispose()
                End With

                bm.Dispose()
                img = thumb
            End If
        End Sub
    End Class

    ''' <summary>
    ''' Tree node information
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NodeInfo
        ''' <summary>
        ''' Gets or sets the nodes data / text.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Data As Object = Nothing

        Dim _image As Image = Nothing
        ''' <summary>
        ''' Gets or sets the node's image.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Image As Image
            Get
                Return _image
            End Get
            Set(ByVal value As Image)
                _image = value
                ResizeImage(_image)
            End Set
        End Property

        Dim _imageexpanded As Image = Nothing

        ''' <summary>
        ''' Gets or sets the node's expanded image.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageExpanded As Image
            Get
                Return _imageexpanded
            End Get
            Set(ByVal value As Image)
                _imageexpanded = value
                ResizeImage(_imageexpanded)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the node's key.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Key As Object = Nothing

        Private Sub ResizeImage(ByRef img As Image)
            If img IsNot Nothing Then
                Dim bm As New Bitmap(img)
                Dim width As Integer = 16 : Dim height As Integer = 16
                Dim thumb As New Bitmap(width, height)
                Dim g As Graphics = Graphics.FromImage(thumb)

                With g
                    .InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    .DrawImage(bm, New Rectangle(0, 0, width, height), New Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel)
                    .Dispose()
                End With

                bm.Dispose()
                img = thumb
            End If
        End Sub
    End Class

    ''' <summary>
    ''' Tree node's expenaded image information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NodeExpadedImageInfo
        Dim _imagekey As String = String.Empty
        ''' <summary>
        ''' Gets or sets the node's expanded image key.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageKey As String
            Get
                Return _imagekey
            End Get
            Set(ByVal value As String)
                _imagekey = value
                If Not String.IsNullOrEmpty(value.Trim) Then
                    ImageIndex = -1 : Image = Nothing
                End If
            End Set
        End Property

        Dim _imageindex As Integer = -1
        ''' <summary>
        ''' Gets or sets the node's expanded image index.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageIndex As Integer
            Get
                Return _imageindex
            End Get
            Set(ByVal value As Integer)
                _imageindex = value
                If value >= 0 Then
                    ImageKey = String.Empty : Image = Nothing
                End If
            End Set
        End Property

        Dim _image As Image = Nothing
        ''' <summary>
        ''' Gets or sets the node's expanded image.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Image As Image
            Get
                Return _image
            End Get
            Set(ByVal value As Image)
                _image = value : ResizeImage(_image)
                If value IsNot Nothing Then
                    _imageindex = -1 : _imagekey = String.Empty
                End If
            End Set
        End Property

        Private Sub ResizeImage(ByRef img As Image)
            If img IsNot Nothing Then
                Dim bm As New Bitmap(img)
                Dim width As Integer = 16 : Dim height As Integer = 16
                Dim thumb As New Bitmap(width, height)
                Dim g As Graphics = Graphics.FromImage(thumb)

                With g
                    .InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    .DrawImage(bm, New Rectangle(0, 0, width, height), New Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel)
                    .Dispose()
                End With

                bm.Dispose()
                img = thumb
            End If
        End Sub
    End Class

    ''' <summary>
    ''' Row listviewitem event arguments.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RowListEventArgs
        ''' <summary>
        ''' Determines the current selected row index.
        ''' </summary>
        ''' <remarks></remarks>
        Public Row As Integer = -1
        ''' <summary>
        ''' Determines the current selected column index.
        ''' </summary >
        ''' <remarks></remarks>
        Public Col As Integer = -1
        ''' <summary>
        ''' Determines the current associated ListViewItem with the selected cell.
        ''' </summary>
        ''' <remarks></remarks>
        Public ListViewItem As ListViewItem = Nothing
    End Class

    ''' <summary>
    ''' Row's node event arguments.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RowNodeEventArgs
        ''' <summary>
        ''' Determines the current grid row index.
        ''' </summary>
        ''' <remarks></remarks>
        Public Row As Integer = -1
        ''' <summary>
        ''' Determines the current grid column index.
        ''' </summary>
        ''' <remarks></remarks>
        Public Col As Integer = -1
        ''' <summary>
        ''' Determines the current grid node.
        ''' </summary>
        ''' <remarks></remarks>
        Public Node As Node = Nothing
    End Class

    ''' <summary>
    ''' Tree data source key fields.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TreeSourceInfo
        Dim _grid As C1FlexGrid = Nothing
        Dim _firstlevelnode As NodeInfo = Nothing

        ''' <summary>
        ''' Gets or sets the node's checkbox value field name.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CheckBoxKey As String = String.Empty

        ''' <summary>
        ''' Gets or sets the tree node's default data source (Grid's command text and connection string property will be disregarded if this is set).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSource As DataTable = Nothing

        ''' <summary>
        ''' Gets the first level node's information.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FirstLevelNode As NodeInfo
            Get
                If _firstlevelnode Is Nothing Then _firstlevelnode = New NodeInfo
                Return _firstlevelnode
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the image expanded field name.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageExpadedKey As String = String.Empty

        ''' <summary>
        ''' Gets or sets image field name.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageKey As String = String.Empty
        ''' <summary>
        ''' Gets or sets key field name.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Key As String = String.Empty
        ''' <summary>
        ''' Gets or sets image field name.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TextKey As String = String.Empty
        ''' <summary>
        ''' Gets or sets the tree node's column header text.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TreeHeaderCaption As String = String.Empty
        ''' <summary>
        ''' Gets or sets parent key field name.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ParentKey As String = String.Empty

        ''' <summary>
        ''' Gets or sets whether nodes will be associated with checkboxes or not.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ShowCheckBoxes As Boolean = False

        ''' <summary>
        ''' Gets or sets whether the tree header is shown upon tree loading or not.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ShowHeader As Boolean = False

        ''' <summary>
        ''' Creates a new instance of TreeSourceInfo.
        ''' </summary>
        ''' <param name="grid"></param>
        ''' <remarks></remarks>
        Sub New(ByVal grid As C1FlexGrid)
            _grid = grid
        End Sub
    End Class

    ''' <summary>
    ''' Row grouping information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TreeGroupingInfo
        ''' <summary>
        ''' Gets or sets the aggregate to use for the grouping.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Aggregate As AggregateEnum = AggregateEnum.None
        ''' <summary>
        ''' Gets or sets the grouping tree caption.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Caption As String = "{0}"
        ''' <summary>
        ''' Gets or sets whether column range specified is visible or not.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ColumnsVisible As Boolean = False
        ''' <summary>
        ''' Gets or sets the beginning column index of the grouping tree.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property [From] As Integer = -1
        ''' <summary>
        ''' Gets or sets the tree level.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Level As Integer = -1
        ''' <summary>
        ''' Gets or sets the last column index of the grouping tree.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property [To] As Integer = -1

        ''' <summary>
        ''' Creates a new instance of TreeGroupingInfo.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column to include in the group.</param>
        ''' <param name="lastcolumn">Last column to include in the group.</param>
        ''' <remarks></remarks>
        Sub New(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer)
            Me.New(startingcolumn, lastcolumn, AggregateEnum.None)
        End Sub

        ''' <summary>
        ''' Creates a new instance of TreeGroupingInfo.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column to include in the group.</param>
        ''' <param name="lastcolumn">Last column to include in the group.</param>
        ''' <param name="treeaggregate">Aggregate to use.</param>
        ''' <remarks></remarks>
        Sub New(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer, ByVal treeaggregate As AggregateEnum)
            Me.New(startingcolumn, lastcolumn, treeaggregate, 0)
        End Sub

        ''' <summary>
        ''' Creates a new instance of TreeGroupingInfo.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column to include in the group.</param>
        ''' <param name="lastcolumn">Last column to include in the group.</param>
        ''' <param name="treeaggregate">Aggregate to use.</param>
        ''' <param name="treelevel">Tree level.</param>
        ''' <remarks></remarks>
        Sub New(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer, ByVal treeaggregate As AggregateEnum, ByVal treelevel As Integer)
            Me.New(startingcolumn, lastcolumn, treeaggregate, treelevel, False)
        End Sub

        ''' <summary>
        ''' Creates a new instance of TreeGroupingInfo.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column to include in the group.</param>
        ''' <param name="lastcolumn">Last column to include in the group.</param>
        ''' <param name="treeaggregate">Aggregate to use.</param>
        ''' <param name="treelevel">Tree level.</param>
        ''' <param name="columnvisibility">Determines whether columns included to the group is visible or not.</param>
        ''' <remarks></remarks>
        Sub New(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer, ByVal treeaggregate As AggregateEnum, ByVal treelevel As Integer, ByVal columnvisibility As Boolean)
            From = startingcolumn : [To] = lastcolumn : Aggregate = treeaggregate : Level = treelevel : ColumnsVisible = columnvisibility
        End Sub
    End Class

    ''' <summary>
    ''' Collection of tree grouping information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TreeGroupCollection
        Inherits CollectionBase

        ''' <summary>
        ''' Gets or sets the tree group in the specified index of the collection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Property Groups(ByVal index As Integer) As TreeGroupingInfo
            Get
                Return CType(List(index), TreeGroupingInfo)
            End Get
            Set(ByVal value As TreeGroupingInfo)
                List(index) = value
            End Set
        End Property

        ''' <summary>
        ''' Adds a tree group in the collection.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column to include in the group.</param>
        ''' <param name="lastcolumn">Last column to include in the group.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer) As Integer
            Return Add(startingcolumn, lastcolumn, AggregateEnum.None)
        End Function

        ''' <summary>
        ''' Adds a tree group in the collection.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column to include in the group.</param>
        ''' <param name="lastcolumn">Last column to include in the group.</param>
        ''' <param name="treeaggregate">Aggregate to use.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer, ByVal treeaggregate As AggregateEnum) As Integer
            Return Add(startingcolumn, lastcolumn, treeaggregate, 0)
        End Function

        ''' <summary>
        ''' Adds a tree group in the collection.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column to include in the group.</param>
        ''' <param name="lastcolumn">Last column to include in the group.</param>
        ''' <param name="treeaggregate">Aggregate to use.</param>
        ''' <param name="treelevel">Tree level.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer, ByVal treeaggregate As AggregateEnum, ByVal treelevel As Integer) As Integer
            Dim tg As New TreeGroupingInfo(startingcolumn, lastcolumn, treeaggregate, treelevel, False)
            Return Add(tg)
        End Function

        ''' <summary>
        ''' Adds a tree group in the collection.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column to include in the group.</param>
        ''' <param name="lastcolumn">Last column to include in the group.</param>
        ''' <param name="treeaggregate">Aggregate to use.</param>
        ''' <param name="treelevel">Tree level.</param>
        ''' <param name="caption">Tree group caption.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer, ByVal treeaggregate As AggregateEnum, ByVal treelevel As Integer, ByVal caption As String) As Integer
            Return Add(startingcolumn, lastcolumn, treeaggregate, treelevel, caption, False)
        End Function

        ''' <summary>
        ''' Adds a tree group in the collection.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column to include in the group.</param>
        ''' <param name="lastcolumn">Last column to include in the group.</param>
        ''' <param name="treeaggregate">Aggregate to use.</param>
        ''' <param name="treelevel">Tree level.</param>
        ''' <param name="caption">Tree group caption.</param>
        ''' <param name="columnvisibility">Determines whether columns included to the group is visible or not.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer, ByVal treeaggregate As AggregateEnum, ByVal treelevel As Integer, ByVal caption As String, ByVal columnvisibility As Boolean) As Integer
            Dim tg As New TreeGroupingInfo(startingcolumn, lastcolumn, treeaggregate, treelevel, columnvisibility)
            tg.Caption = caption
            Return Add(tg)
        End Function

        ''' <summary>
        ''' Adds a tree group in the collection.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column to include in the group.</param>
        ''' <param name="lastcolumn">Last column to include in the group.</param>
        ''' <param name="treeaggregate">Aggregate to use.</param>
        ''' <param name="treelevel">Tree level.</param>
        ''' <param name="columnvisibility">Determines whether columns included to the group is visible or not.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer, ByVal treeaggregate As AggregateEnum, ByVal treelevel As Integer, ByVal columnvisibility As Boolean) As Integer
            Dim tg As New TreeGroupingInfo(startingcolumn, lastcolumn, treeaggregate, treelevel, columnvisibility)
            Return Add(tg)
        End Function

        ''' <summary>
        ''' Adds a tree group in the collection.
        ''' </summary>
        ''' <param name="treegroup"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal treegroup As TreeGroupingInfo) As Integer
            Return List.Add(treegroup)
        End Function

        ''' <summary>
        ''' Determines whether the specified tree group is existing within the collection or not.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column included in the group.</param>
        ''' <param name="lastcolumn">Last column included in the group.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Contains(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer) As Boolean
            Dim exists As Boolean = False

            For Each tg As TreeGroupingInfo In List
                If tg.From = startingcolumn And
                   tg.To = lastcolumn Then
                    exists = True : Exit For
                End If
            Next

            Return exists
        End Function

        ''' <summary>
        ''' Determines whether the specified tree group is existing within the collection or not.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column included in the group.</param>
        ''' <param name="lastcolumn">Last column included in the group.</param>
        ''' <param name="treeaggregate">Aggregate used by the group.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Contains(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer, ByVal treeaggregate As AggregateEnum) As Boolean
            Dim exists As Boolean = False

            For Each tg As TreeGroupingInfo In List
                If tg.From = startingcolumn And
                   tg.To = lastcolumn And
                   tg.Aggregate = treeaggregate Then
                    exists = True : Exit For
                End If
            Next

            Return exists
        End Function

        ''' <summary>
        ''' Determines whether the specified tree group is existing within the collection or not.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column included in the group.</param>
        ''' <param name="lastcolumn">Last column included in the group.</param>
        ''' <param name="treeaggregate">Aggregate used by the group.</param>
        ''' <param name="treelevel">Tree level.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Contains(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer, ByVal treeaggregate As AggregateEnum, ByVal treelevel As Integer) As Boolean
            Dim exists As Boolean = False

            For Each tg As TreeGroupingInfo In List
                If tg.From = startingcolumn And
                   tg.To = lastcolumn And
                   tg.Aggregate = treeaggregate And
                   tg.Level = treelevel Then
                    exists = True : Exit For
                End If
            Next

            Return exists
        End Function

        ''' <summary>
        ''' Determines whether the specified tree group is existing within the collection or not.
        ''' </summary>
        ''' <param name="treegroup"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Contains(ByVal treegroup As TreeGroupingInfo) As Boolean
            Return List.Contains(treegroup)
        End Function

        ''' <summary>
        ''' Removes the specified tree group in the collection.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column included in the group.</param>
        ''' <param name="lastcolumn">Last column included in the group.</param>
        ''' <remarks></remarks>
        Public Overloads Sub Remove(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer)
            For Each tg As TreeGroupingInfo In List
                If tg.From = startingcolumn And
                   tg.To = lastcolumn Then List.Remove(tg)
            Next
        End Sub

        ''' <summary>
        ''' Removes the specified tree group in the collection.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column included in the group.</param>
        ''' <param name="lastcolumn">Last column included in the group.</param>
        ''' <param name="treeaggregate">Aggregate used by the group.</param>
        ''' <remarks></remarks>
        Public Overloads Sub Remove(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer, ByVal treeaggregate As AggregateEnum)
            For Each tg As TreeGroupingInfo In List
                If tg.From = startingcolumn And
                   tg.To = lastcolumn And
                   tg.Aggregate = treeaggregate Then List.Remove(tg)
            Next
        End Sub

        ''' <summary>
        ''' Removes the specified tree group in the collection.
        ''' </summary>
        ''' <param name="startingcolumn">Starting column included in the group.</param>
        ''' <param name="lastcolumn">Last column included in the group.</param>
        ''' <param name="treeaggregate">Aggregate used by the group.</param>
        ''' <param name="treelevel">Tree level.</param>
        ''' <remarks></remarks>
        Public Overloads Sub Remove(ByVal startingcolumn As Integer, ByVal lastcolumn As Integer, ByVal treeaggregate As AggregateEnum, ByVal treelevel As Integer)
            For Each tg As TreeGroupingInfo In List
                If tg.From = startingcolumn And
                   tg.To = lastcolumn And
                   tg.Aggregate = treeaggregate And
                   tg.Level = treelevel Then List.Remove(tg)
            Next
        End Sub

        ''' <summary>
        ''' Removes the specified tree group in the collection.
        ''' </summary>
        ''' <param name="treegroup"></param>
        ''' <remarks></remarks>
        Public Overloads Sub Remove(ByVal treegroup As TreeGroupingInfo)
            If List.Contains(treegroup) Then List.Remove(treegroup)
        End Sub
    End Class

#End Region

#Region "Properties"
    Dim _datasourcesettings As New DataSourceInfo
    Dim _nodeexpandedimages As New Hashtable
    Dim _sortingcolumn As Integer = -1

    ''' <summary>
    ''' Gets or sets the data source for the grid.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Property DataSource As Object
        Get
            Return MyBase.DataSource
        End Get
        Set(ByVal value As Object)
            MyBase.DataSource = value
            If value IsNot Nothing Then PreFormatFlexGrid(Me)
        End Set
    End Property

    ''' <summary>
    ''' Gets the data source information that is use to load and provide data binding to the C1FlexGrid. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public ReadOnly Property DataSourceSettings As DataSourceInfo
        Get
            Return _datasourcesettings
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets whether to allow editing to parent nodes and its cell values when in tree mode.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public Property EditableParentNodes As Boolean = False

    Dim _gradientcolor As New GradientColorInfo(Me)

    ''' <summary>
    ''' Gets the gradient color patterns.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public ReadOnly Property GradientColor As GradientColorInfo
        Get
            Return _gradientcolor
        End Get
    End Property

    Dim _groupby As New TreeGroupCollection

    ''' <summary>
    ''' Gets the tree grouping ranges to implement when calling PerformGroup method.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public ReadOnly Property GroupBy As TreeGroupCollection
        Get
            Return _groupby
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the imagelist to refer for the tree node's images.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public Property ImageList As ImageList = Nothing

    Dim _istreeview As Boolean = False

    ''' <summary>
    ''' Gets whether the grid is acting a treeview or not.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public ReadOnly Property IsTreeView As Boolean
        Get
            Return _istreeview
        End Get
    End Property

    Dim _listitems As New ListItemCollection(Me)

    ''' <summary>
    ''' Gets the collection of listviewitems in the grid rendering the grid to act as a listview.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public ReadOnly Property ListItems As ListItemCollection
        Get
            Return _listitems
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the listview item's selection mode when grid acts as a ListView.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListItemsSelectionMode As ListViewItemSelectionEnum = ListViewItemSelectionEnum.MouseHover

    ''' <summary>
    ''' Gets or sets the ajax loader image box for the ViewingFlexGrid.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public Property LoadingImageBox As PictureBox = Nothing

    ''' <summary>
    ''' Gets or sets grid display when there is no row of record is loaded.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public Property NoDataDisplay As NoDataDisplayEnum = NoDataDisplayEnum.AsDisplayed

    ''' <summary>
    ''' Gets the current selected list view item when grid is in ListView mode.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SelectedListItem As ListViewItem
        Get
            Dim lst As ListViewItem = Nothing

            If RowSel > 0 And ColSel > 0 Then
                If GetUserData(RowSel, ColSel).GetType Is GetType(ListViewItem) Then lst = GetUserData(RowSel, ColSel)
            End If

            Return lst
        End Get
    End Property

    Private _displayaction As Boolean

    <DefaultValue(True)>
    Public Property DisplayAction() As Boolean
        Get
            Return _displayaction
        End Get
        Set(ByVal value As Boolean)
            _displayaction = value
            pnlAction.Visible = (value And Rows.Count > 1)
        End Set
    End Property

    Private _showdelete As Boolean

    <DefaultValue(True)>
    Public Property ShowDelete() As Boolean
        Get
            Return _showdelete
        End Get
        Set(ByVal value As Boolean)
            _showdelete = value
            lnkDelete.Visible = _showdelete
        End Set
    End Property

    Private _showedit As Boolean

    <DefaultValue(True)>
    Public Property ShowEdit() As Boolean
        Get
            Return _showedit
        End Get
        Set(ByVal value As Boolean)
            _showedit = value
            lnkEdit.Visible = _showedit
        End Set
    End Property


    Dim _selectednode As Node = Nothing
    Private WithEvents _vertical As MetroFramework.Controls.MetroScrollBar
    Private WithEvents _horizontal As MetroFramework.Controls.MetroScrollBar
    Private WithEvents pnlAction As System.Windows.Forms.Panel
    Private WithEvents lnkDelete As MetroFramework.Controls.MetroLink
    Private WithEvents lnkEdit As MetroFramework.Controls.MetroLink

    ''' <summary>
    ''' Gets or sets the selected tree node.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public Property SelectedNode As Node
        Get
            _selectednode = GetSelectedNode()
            Return _selectednode
        End Get
        Set(ByVal value As Node)
            If IsTreeView Then
                _selectednode = value
                If _selectednode Is Nothing Then
                    RowSel = 0 : Row = 0
                Else
                    RowSel = _selectednode.Row.Index : Row = _selectednode.Row.Index
                    ExpandParentNode(_selectednode)
                End If
            Else : _selectednode = Nothing
            End If
        End Set
    End Property

    Dim _treesourcesettings As New TreeSourceInfo(Me)

    ''' <summary>
    ''' Gets the tree data source key field settings.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public ReadOnly Property TreeSourceSettings As TreeSourceInfo
        Get
            Return _treesourcesettings
        End Get
    End Property
#End Region

#Region "Functions"

    ''' <summary>
    ''' Gets an existing node with the specified data.
    ''' </summary>
    ''' <param name="data"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function FindNodeByData(ByVal data As Object) As Node
        Dim pnode As Node = Nothing

        For Each rw As Row In Rows
            If rw.IsNode Then
                pnode = FindNodeByData(rw.Node, data)
                If pnode IsNot Nothing Then Exit For
            End If
        Next

        Return pnode
    End Function

    ''' <summary>
    ''' Gets an existing node with the specified data within the specified node.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="data"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function FindNodeByData(ByVal node As Node, ByVal data As Object) As Node
        Dim pnode As Node = Nothing

        If node.Data = data Then : pnode = node
        Else
            For Each cnode As Node In node.Nodes
                If cnode.Nodes.Length > 0 Then : pnode = FindNodeByData(cnode, data)
                Else
                    If cnode.Data = data Then
                        pnode = cnode : Exit For
                    End If
                End If
            Next
        End If

        Return pnode
    End Function

    ''' <summary>
    ''' Gets an existing node with the specified key.
    ''' </summary>
    ''' <param name="key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function FindNodeByKey(ByVal key As Object) As Node
        Dim pnode As Node = Nothing

        For Each rw As Row In Rows
            If rw.IsNode Then
                pnode = FindNodeByKey(rw.Node, key)
                If pnode IsNot Nothing Then Exit For
            End If
        Next

        Return pnode
    End Function

    ''' <summary>
    ''' Gets an existing node with the specified key within the specified node.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function FindNodeByKey(ByVal node As Node, ByVal key As String) As Node
        Dim pnode As Node = Nothing

        If node.Key.Equals(key) Then : pnode = node
        Else
            For Each cnode As Node In node.Nodes
                If cnode.Nodes.Length > 0 Then : pnode = FindNodeByKey(cnode, key)
                Else
                    If cnode.Key = key Then
                        pnode = cnode : Exit For
                    End If
                End If
            Next
        End If

        Return pnode
    End Function

    ''' <summary>
    ''' Gets the suited visual style based on the current DevComponents.DotNetBar theme.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetApplicableVisualStyle() As VisualStyle
        Dim vs As VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Silver

        Select Case DevComponents.DotNetBar.StyleManager.Style
            Case DevComponents.DotNetBar.eStyle.Office2007Black : vs = C1.Win.C1FlexGrid.VisualStyle.Office2007Black
            Case DevComponents.DotNetBar.eStyle.Office2007Blue : vs = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
            Case DevComponents.DotNetBar.eStyle.Office2010Black : vs = C1.Win.C1FlexGrid.VisualStyle.Office2010Black
            Case DevComponents.DotNetBar.eStyle.Office2010Blue, DevComponents.DotNetBar.eStyle.Windows7Blue : vs = C1.Win.C1FlexGrid.VisualStyle.Office2010Blue
            Case DevComponents.DotNetBar.eStyle.Office2010Silver, DevComponents.DotNetBar.eStyle.VisualStudio2010Blue : vs = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
            Case Else
        End Select

        Return vs
    End Function

    ''' <summary>
    ''' Returns whether all rows or the specified column were marked as check or not.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetCellCheck(ByVal name As String) As CheckEnum
        Return GetCellCheck(Cols(name))
    End Function

    ''' <summary>
    ''' Returns whether all rows or the specified column were marked as check or not.
    ''' </summary>
    ''' <param name="index"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetCellCheck(ByVal index As Integer) As CheckEnum
        Return GetCellCheck(Cols(index))
    End Function

    ''' <summary>
    ''' Returns whether all rows or the specified column were marked as check or not.
    ''' </summary>
    ''' <param name="column"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetCellCheck(ByVal column As Column) As CheckEnum
        Dim check As CheckEnum = CheckEnum.Checked

        If column.DataType Is GetType(Boolean) Then
            Dim checked As Boolean = True
            For i As Integer = 1 To Rows.Count - 1
                If Rows(i).Item(column.Index) IsNot Nothing And
                   Not Rows(i).IsNode Then
                    checked = checked And CBool(Rows(i).Item(column.Index))
                    If Not checked Then Exit For
                End If
            Next
            If Not checked Then check = CheckEnum.Unchecked
        Else
            Dim checked As Boolean = True
            For i As Integer = 1 To Rows.Count - 1
                If Rows(i).Item(column.Index) IsNot Nothing And
                   Not Rows(i).IsNode Then
                    checked = checked And CBool(GetCellCheck(i, column.Index) = CheckEnum.Checked)
                    If Not checked Then Exit For
                End If
            Next
            If Not checked Then check = CheckEnum.Unchecked
        End If

        Return check
    End Function

    ''' <summary>
    ''' Gets the node expanded image information for the specified node.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNodeExpandedImageInfo(ByVal node As Node) As NodeExpadedImageInfo
        Dim ni As NodeExpadedImageInfo = Nothing

        For Each n As Node In _nodeexpandedimages.Keys
            If n.Key = node.Key Then
                ni = _nodeexpandedimages(n) : Exit For
            End If
        Next

        Return ni
    End Function

    Private Function GetSelectedNode() As Node
        Dim selnode As Node = Nothing

        If RowSel >= 1 Then
            If Rows(RowSel).IsNode Then selnode = Rows(RowSel).Node
        End If

        Return selnode
    End Function

    ''' <summary>
    ''' Loads the grid as a treeview asynchronously.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LoadTreeAsync() As IAsyncResult
        Dim delLoad As New Action(AddressOf LoadTree)
        Return delLoad.BeginInvoke(Nothing, delLoad)
    End Function

    ''' <summary>
    ''' Retrieves and binds the datasource generated into the grid using the details specified in DataSourceSettings property asynchronously.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReloadAsync() As IAsyncResult
        Dim delReload As New Action(AddressOf Reload)
        Return delReload.BeginInvoke(Nothing, delReload)
    End Function
#End Region

#Region "Methods"
    ''' <summary>
    ''' Adds a sub total node for the specified column.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="rootcolumn"></param>
    ''' <param name="totalin"></param>
    ''' <remarks></remarks>
    Public Overloads Sub AddNodeTotals(ByVal node As Node, ByVal rootcolumn As String, ByVal totalin As Column)
        AddNodeTotals(node, rootcolumn, totalin.Index, totalin.Index)
    End Sub

    ''' <summary>
    ''' Adds a sub total node for the specified column.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="rootcolumn"></param>
    ''' <param name="totalin"></param>
    ''' <remarks></remarks>
    Public Overloads Sub AddNodeTotals(ByVal node As Node, ByVal rootcolumn As String, ByVal totalin As String)
        AddNodeTotals(node, rootcolumn, Cols(totalin).Index, Cols(totalin).Index)
    End Sub

    ''' <summary>
    ''' Adds a sub total node for the specified column.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="rootcolumn"></param>
    ''' <param name="totalin"></param>
    ''' <remarks></remarks>
    Public Overloads Sub AddNodeTotals(ByVal node As Node, ByVal rootcolumn As String, ByVal totalin As Integer)
        AddNodeTotals(node, rootcolumn, totalin, totalin)
    End Sub

    ''' <summary>
    ''' Adds a sub total node for the specified column ranges.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="rootcolumn"></param>
    ''' <param name="fromcol"></param>
    ''' <param name="tocol"></param>
    ''' <remarks></remarks>
    Public Overloads Sub AddNodeTotals(ByVal node As Node, ByVal rootcolumn As String, ByVal fromcol As Column, ByVal tocol As Column)
        AddNodeTotals(node, rootcolumn, fromcol.Index, tocol.Index)
    End Sub

    ''' <summary>
    ''' Adds a sub total node for the specified column ranges.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="rootcolumn"></param>
    ''' <param name="fromcol"></param>
    ''' <param name="tocol"></param>
    ''' <remarks></remarks>
    Public Overloads Sub AddNodeTotals(ByVal node As Node, ByVal rootcolumn As String, ByVal fromcol As String, ByVal tocol As String)
        AddNodeTotals(node, rootcolumn, Cols(fromcol).Index, Cols(tocol).Index)
    End Sub

    ''' <summary>
    ''' Adds a sub total node for the specified column ranges.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="rootcolumn"></param>
    ''' <param name="fromcol"></param>
    ''' <param name="tocol"></param>
    ''' <remarks></remarks>
    Public Overloads Sub AddNodeTotals(ByVal node As Node, ByVal rootcolumn As String, ByVal fromcol As Integer, ByVal tocol As Integer)
        Dim dt As DataTable = DataSource
        If dt IsNot Nothing Then
            Dim value As String = String.Empty
            If node.Data IsNot Nothing Then value = node.Data.ToString
            Dim startcol As String = Cols(fromcol).Name : Dim endcol As String = Cols(tocol).Name

            If Not String.IsNullOrEmpty(rootcolumn.Trim) Then
                Dim rws() As DataRow = dt.Select("CONVERT([" & rootcolumn & "], System.String) LIKE '" & ToSqlValidString(value, True) & "'")
                If rws.Length <= 0 Then Exit Sub
            End If

            For c As Integer = dt.Columns(startcol).Ordinal To dt.Columns(endcol).Ordinal
                Dim total As Object = Nothing

                If String.IsNullOrEmpty(rootcolumn.Trim) Then : total = dt.Compute("SUM([" & dt.Columns(c).ColumnName & "])", dt.DefaultView.RowFilter)
                Else
                    'total = dt.Compute("SUM([" & dt.Columns(c).ColumnName & "])", dt.DefaultView.RowFilter.Trim & IIf(Not String.IsNullOrEmpty(dt.DefaultView.RowFilter.Trim), " AND ", String.Empty) & "(CONVERT([" & rootcolumn & "], System.String) LIKE '" & TarsierEyes.Common.SQLStrings.ToSqlValidString(value, True) & "')")
                    Dim filter As String = dt.DefaultView.RowFilter
                    Dim rw As Row = node.Row
                    For i As Integer = 1 To Cols(rootcolumn).Index
                        Dim ctr As Integer = Cols(rootcolumn).Index - i
                        Dim n As Node = node : Dim p As Node = Nothing
                        Dim exitloop As Boolean = False

                        For ic As Integer = 1 To ctr
                            If n IsNot Nothing Then
                                p = n.Parent : n = n.Parent
                            Else
                                If Not String.IsNullOrEmpty(filter.Trim) Then filter = "(" & filter
                                filter &= IIf(Not String.IsNullOrEmpty(filter.Trim), ") AND ", String.Empty) & "(CONVERT([" & Cols(rootcolumn).Name & "], System.String) LIKE '" & ToSqlValidString(value.ToString, True) & "')"
                                exitloop = True : Exit For
                            End If
                        Next

                        If exitloop Then Exit For

                        If p IsNot Nothing Then
                            If p.Data IsNot Nothing Then
                                If Cols(i).DataType Is GetType(Date) And
                                   IsDate(p.Data) Then
                                    Dim rws() As DataRow = dt.Select("([" & Cols(i).Name & "] = '" & ToSqlValidString(CDate(p.Data)) & "')")
                                    If rws.Length > 0 Then
                                        If Not String.IsNullOrEmpty(filter.Trim) Then filter = "(" & filter
                                        filter &= IIf(Not String.IsNullOrEmpty(filter.Trim), ") AND ", String.Empty) & "([" & Cols(i).Name & "] = '" & ToSqlValidString(CDate(p.Data)) & "')"
                                    End If
                                Else
                                    Dim rws() As DataRow = dt.Select("(CONVERT([" & Cols(i).Name & "], System.String) LIKE '" & ToSqlValidString(p.Data.ToString, True) & "')")
                                    If rws.Length > 0 Then
                                        If Not String.IsNullOrEmpty(filter.Trim) Then filter = "(" & filter
                                        filter &= IIf(Not String.IsNullOrEmpty(filter.Trim), ") AND ", String.Empty) & "(CONVERT([" & Cols(i).Name & "], System.String) LIKE '" & ToSqlValidString(p.Data.ToString, True) & "')"
                                    End If
                                End If

                            Else
                                If Not String.IsNullOrEmpty(filter.Trim) Then filter = "(" & filter
                                filter &= IIf(Not String.IsNullOrEmpty(filter.Trim), ") AND ", String.Empty) & "(CONVERT([" & Cols(rootcolumn).Name & "], System.String) LIKE '" & ToSqlValidString(value.ToString, True) & "')"
                            End If
                        Else
                            If Not filter.Contains("(CONVERT([" & Cols(rootcolumn).Name & "], System.String) LIKE '" & ToSqlValidString(value.ToString, True) & "')") Then
                                If Not String.IsNullOrEmpty(filter.Trim) Then filter = "(" & filter
                                filter &= IIf(Not String.IsNullOrEmpty(filter.Trim), ") AND ", String.Empty) & "(CONVERT([" & Cols(rootcolumn).Name & "], System.String) LIKE '" & ToSqlValidString(value.ToString, True) & "')"
                            End If
                        End If
                    Next
                    total = dt.Compute("SUM([" & dt.Columns(c).ColumnName & "])", filter)
                End If

                If Not IsNumeric(total) Then total = 0
                Dim tnode As Node = node.LastChild

                If tnode Is Nothing Then
                    tnode = node.AddNode(NodeTypeEnum.LastChild, "Total " & value)
                    tnode.Row.StyleNew.Font = New Font("Tahoma", 8, FontStyle.Bold)
                Else
                    If tnode.Data IsNot Nothing Then
                        If tnode.Data.ToString <> ("Total " & value) Then
                            tnode = node.AddNode(NodeTypeEnum.LastChild, "Total " & value)
                            tnode.Row.StyleNew.Font = New Font("Tahoma", 8, FontStyle.Bold)
                        End If
                    End If
                End If

                tnode.Row.Item(dt.Columns(c).ColumnName) = Format(CDec(total), "N2")
            Next
        End If
    End Sub

    ''' <summary>
    ''' Collapses all of the nodes.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub CollapseNodes()
        CollapseNodes(0)
    End Sub

    ''' <summary>
    ''' Collapses each of nodes with the level greater than or equal the specified level.
    ''' </summary>
    ''' <param name="minlevel"></param>
    ''' <remarks></remarks>
    Public Overloads Sub CollapseNodes(ByVal minlevel As Integer)
        Simple.Redraw(Me, False)
        For Each rw As Row In Rows
            If rw.IsNode Then
                If rw.Node.Level >= minlevel Then : rw.Node.Collapsed = True
                Else : rw.Node.Expanded = True
                End If
            End If
        Next
        Simple.Redraw(Me)
    End Sub

    ''' <summary>
    ''' Expands the parent node (up to the top most node) of the specified node.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <remarks></remarks>
    Public Sub ExpandParentNode(ByVal node As Node)
        If node.Parent IsNot Nothing Then
            node.Parent.Expanded = True : ExpandParentNode(node.Parent)
        End If
    End Sub

    ''' <summary>
    ''' Loads the grid as a treeview.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadTree()
        _istreeview = False : DrawMode = DrawModeEnum.Normal : VisualStyle = GetApplicableVisualStyle()
        Dim dt As DataTable = TreeSourceSettings.DataSource
        If dt Is Nothing Then
            Dim q As Que = Que.Execute(DataSourceSettings.ConnectionString, DataSourceSettings.CommandText, Que.ExecutionEnum.ExecuteReader)
            If q.Rows > 0 Then
                dt = q.DataTable.Clone
                dt.Load(q.DataTable.CreateDataReader)
            End If
            q.Dispose()
        End If

        If dt IsNot Nothing Then
            With dt
                If .Columns.Contains(TreeSourceSettings.ImageKey) And
                   .Columns.Contains(TreeSourceSettings.Key) And
                   .Columns.Contains(TreeSourceSettings.ParentKey) And
                   .Columns.Contains(TreeSourceSettings.TextKey) Then
                    _istreeview = True

                    BackColor = Color.Transparent : BorderStyle = Util.BaseControls.BorderStyleEnum.None
                    Try
                        If DataSource IsNot Nothing Then CType(DataSource, DataTable).Dispose()
                    Catch ex As Exception
                    Finally : DataSource = Nothing
                    End Try

                    Clear(ClearFlags.All) : Rows.Count = 1 : Cols.Count = 2 + .Columns.Count
                    Tree.Clear() : Tree.Column = 1
                    Cols(0).Visible = False : Tree.Style = TreeStyleFlags.CompleteLeaf

                    For i As Integer = 0 To .Columns.Count - 1
                        Cols(i + 2).Name = .Columns(i).ColumnName
                        Cols(i + 2).DataType = .Columns(i).DataType
                        Cols(i + 2).Caption = .Columns(i).ColumnName
                        Cols(i + 2).Visible = False
                    Next

                    If TreeSourceSettings.ShowCheckBoxes Then : DrawMode = DrawModeEnum.OwnerDraw
                    Else : DrawMode = DrawModeEnum.Normal
                    End If

                    If TreeSourceSettings.FirstLevelNode IsNot Nothing Then
                        Dim fnode As Node = Rows.AddNode(0)
                        With fnode
                            .Key = TreeSourceSettings.FirstLevelNode.Key
                            .Data = TreeSourceSettings.FirstLevelNode.Data
                            .Image = TreeSourceSettings.FirstLevelNode.Image

                            If TreeSourceSettings.FirstLevelNode.ImageExpanded IsNot Nothing Then SetNodeExpandedImage(fnode, TreeSourceSettings.FirstLevelNode.ImageExpanded)

                            If TreeSourceSettings.ShowCheckBoxes Then : .Checked = CheckEnum.Unchecked
                            Else : .Checked = CheckEnum.None
                            End If

                            Rows(0).Item(TreeSourceSettings.Key) = TreeSourceSettings.FirstLevelNode.Key
                            Rows(0).Item(TreeSourceSettings.TextKey) = TreeSourceSettings.FirstLevelNode.Data
                            Rows(0).Item(TreeSourceSettings.ImageKey) = String.Empty
                            Rows(0).Item(TreeSourceSettings.ParentKey) = String.Empty
                        End With
                    End If

                    For Each rw As DataRow In .Rows
                        Dim pnode As Node = FindNodeByKey(rw.Item(TreeSourceSettings.ParentKey))

                        Dim img As Image = Nothing
                        If ImageList IsNot Nothing Then
                            If ImageList.Images.ContainsKey(rw.Item(TreeSourceSettings.ImageKey)) Then
                                img = ImageList.Images(rw.Item(TreeSourceSettings.ImageKey))
                                img.Tag = rw.Item(TreeSourceSettings.ImageKey)
                            End If
                        End If

                        If pnode Is Nothing Then
                            If Nodes.Length <= 0 Then
                                pnode = Rows.AddNode(0)
                                With pnode
                                    .Key = rw.Item(TreeSourceSettings.Key)
                                    .Data = rw.Item(TreeSourceSettings.TextKey)
                                    .Image = img

                                    If dt.Columns.Contains(TreeSourceSettings.ImageExpadedKey) Then
                                        If dt.Columns(TreeSourceSettings.ImageExpadedKey).DataType Is GetType(String) Then
                                            If Not String.IsNullOrEmpty(rw.Item(TreeSourceSettings.ImageExpadedKey)) Then SetNodeExpandedImageKey(pnode, rw.Item(TreeSourceSettings.ImageExpadedKey))
                                        Else
                                            If IsNumeric(rw.Item(TreeSourceSettings.ImageExpadedKey)) Then : SetNodeExpandedImageIndex(pnode, rw.Item(TreeSourceSettings.ImageExpadedKey))
                                            Else
                                                If (dt.Columns(TreeSourceSettings.ImageExpadedKey).DataType.Name.ToLower.Contains("bytes[]") Or
                                                    dt.Columns(TreeSourceSettings.ImageExpadedKey).DataType.Name.ToLower.Contains("bytes()")) And
                                                    Not IsDBNull(rw.Item(TreeSourceSettings.ImageExpadedKey)) Then SetNodeExpandedImage(pnode, ByteArrayToImage(rw.Item(TreeSourceSettings.ImageExpadedKey)))
                                            End If
                                        End If
                                    End If

                                    For Each c As DataColumn In dt.Columns
                                        .Row.Item(c.ColumnName) = rw.Item(c.ColumnName)
                                    Next

                                    If TreeSourceSettings.ShowCheckBoxes Then : .Checked = CheckEnum.Unchecked
                                    Else : .Checked = CheckEnum.None
                                    End If

                                    If RightToLeft = Windows.Forms.RightToLeft.No Then .Row.StyleNew.TextAlign = TextAlignEnum.LeftCenter
                                End With
                            End If
                        Else
                            Dim cnode As Node = pnode.AddNode(NodeTypeEnum.LastChild, rw.Item(TreeSourceSettings.TextKey), rw.Item(TreeSourceSettings.Key), img)
                            With cnode
                                If dt.Columns.Contains(TreeSourceSettings.ImageExpadedKey) Then
                                    If dt.Columns(TreeSourceSettings.ImageExpadedKey).DataType Is GetType(String) Then
                                        If Not String.IsNullOrEmpty(rw.Item(TreeSourceSettings.ImageExpadedKey)) Then SetNodeExpandedImageKey(cnode, rw.Item(TreeSourceSettings.ImageExpadedKey))
                                    Else
                                        If IsNumeric(rw.Item(TreeSourceSettings.ImageExpadedKey)) Then : SetNodeExpandedImageIndex(cnode, rw.Item(TreeSourceSettings.ImageExpadedKey))
                                        Else
                                            If (dt.Columns(TreeSourceSettings.ImageExpadedKey).DataType.Name.ToLower.Contains("bytes[]") Or
                                                dt.Columns(TreeSourceSettings.ImageExpadedKey).DataType.Name.ToLower.Contains("bytes()")) And
                                                Not IsDBNull(rw.Item(TreeSourceSettings.ImageExpadedKey)) Then SetNodeExpandedImage(cnode, ByteArrayToImage(rw.Item(TreeSourceSettings.ImageExpadedKey)))
                                        End If
                                    End If
                                End If

                                If TreeSourceSettings.ShowCheckBoxes Then
                                    If dt.Columns.Contains(TreeSourceSettings.CheckBoxKey) Then
                                        If CBool(.Row.Item(TreeSourceSettings.CheckBoxKey)) Then : .Checked = CheckEnum.Checked
                                        Else : .Checked = CheckEnum.Unchecked
                                        End If
                                    Else : .Checked = CheckEnum.Unchecked
                                    End If
                                    SetCheckMarks(cnode.Row, cnode.Checked)
                                Else : .Checked = CheckEnum.None
                                End If

                                For Each c As DataColumn In dt.Columns
                                    .Row.Item(c.ColumnName) = rw.Item(c.ColumnName)
                                Next

                                If RightToLeft = Windows.Forms.RightToLeft.No Then .Row.StyleNew.TextAlign = TextAlignEnum.LeftCenter
                            End With
                        End If
                    Next

                    AllowEditing = TreeSourceSettings.ShowCheckBoxes
                    Cols(1).AllowEditing = TreeSourceSettings.ShowCheckBoxes
                    AllowDelete = False : AllowAddNew = False : AllowFiltering = False
                    AllowDragging = AllowDraggingEnum.None
                    AllowSorting = AllowSortingEnum.None
                    Rows(0).Caption = TreeSourceSettings.TreeHeaderCaption
                    Rows(0).Visible = TreeSourceSettings.ShowHeader
                    Styles.Normal.Border.Color = Color.Transparent
                    Styles.Focus.BackgroundImage = My.Resources.highligthedwithborder
                    Styles.Focus.BackgroundImageLayout = ImageAlignEnum.TileStretch
                    Styles.EmptyArea.BackColor = Color.Transparent
                    Styles.EmptyArea.Border.Color = Color.Transparent
                    HighLight = HighLightEnum.Always
                    Styles.Highlight.ForeColor = Color.Black
                    Styles.Highlight.BackgroundImage = My.Resources.highligthedwithborder
                    Styles.Highlight.BackgroundImageLayout = ImageAlignEnum.TileStretch
                    Styles.Highlight.Border.Color = Color.Cornsilk
                    VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Custom
                    AutoSizeCols() : ExtendLastCol = True
                End If
            End With
        End If
    End Sub

    ''' <summary>
    ''' Copies all row value from a row into another row.
    ''' </summary>
    ''' <param name="copyrow">From row.</param>
    ''' <param name="copyto">To row.</param>
    ''' <remarks></remarks>
    Public Overloads Sub MimeRow(ByVal copyrow As Row, ByVal copyto As Row)
        MimeRow(copyrow.Index, copyto.Index)
    End Sub

    ''' <summary>
    ''' Copies all row value from a row into another row.
    ''' </summary>
    ''' <param name="copyrowindex">From row index.</param>
    ''' <param name="copytoindex">To row index.</param>
    ''' <remarks></remarks>
    Public Overloads Sub MimeRow(ByVal copyrowindex As Integer, ByVal copytoindex As Integer)
        Dim key As String = String.Empty
        Dim dt As DataTable = DataSource
        If dt IsNot Nothing Then
            For Each c As DataColumn In dt.Columns
                If c.AutoIncrement Then
                    key = c.ColumnName : Exit For
                End If
            Next
        End If

        For Each c As Column In Cols
            If c.Index > 0 Then
                If c.Name.Trim <> key.Trim Then Rows(copytoindex).Item(c.Name) = Rows(copyrowindex).Item(c.Name)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Performs the tree row grouping supplied in the GroupBy property.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub PerformGroup()
        If Rows.Count > 1 Then
            Subtotal(AggregateEnum.Clear)
            For Each tg As TreeGroupingInfo In GroupBy
                Dim firstindex As Integer = -1 : Dim lastindex As Integer = -1
                With tg
                    If (Cols.Count > .From) And
                       (.From >= 0) And
                       (Cols.Count > .To) And
                       (.To >= 0) Then
                        Cols(.To).Visible = .ColumnsVisible
                        Subtotal(.Aggregate, .Level, .From, .To, Tree.Column, .Caption)
                    End If
                End With
            Next
            AllowDragging = AllowDraggingEnum.None : AllowSorting = AllowSortingEnum.None : AutoSizeCols()
        End If
    End Sub

    ''' <summary>
    ''' Renders predefined column formating (for numeric and date fields) of the current FlexGrid.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub PreFormatFlexGrid()
        PreFormatFlexGrid(Me)
    End Sub

    ''' <summary>
    ''' Renders predefined column formating (for numeric and date fields) for the specified C1FlexGrid.
    ''' </summary>
    ''' <param name="c1fg">C1FlexGrid to format.</param>
    ''' <remarks></remarks>
    Public Overloads Shared Sub PreFormatFlexGrid(ByVal c1fg As C1FlexGrid)
        With c1fg
            .Tree.Style = TreeStyleFlags.CompleteLeaf
            For Each col As Column In .Cols
                If col.DataType IsNot Nothing Then
                    Select Case col.DataType.Name
                        Case GetType(Date).Name : col.Format = "dd-MMM-yyyy"
                        Case GetType(Decimal).Name,
                             GetType(Single).Name,
                             GetType(Double).Name : col.Format = "N2"
                        Case Else
                    End Select
                End If
            Next
        End With
    End Sub

    ''' <summary>
    ''' Retrieves and binds the datasource generated into the grid using the details specified in DataSourceSettings property.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Reload()
        Simple.Redraw(Me, False)

        If LoadingImageBox IsNot Nothing Then
            With LoadingImageBox
                .Show() : .BringToFront()
            End With
        End If

        If Cols.Count >= 1 Then Cols(0).Visible = True

        Try
            If DataSource IsNot Nothing Then CType(DataSource, DataTable).Dispose()
        Catch ex As Exception
        Finally : DataSource = Nothing
        End Try

        Dim q As Que = Que.Execute(DataSourceSettings.ConnectionString, DataSourceSettings.CommandText, Que.ExecutionEnum.ExecuteReader)
        Dim args As New DataLoadingEventArgs
        With args
            .QueryStatement = DataSourceSettings.CommandText
            .ErrorMessage = q.ErrorMessage
            .Rows = q.Rows : .Columns = q.Columns
        End With

        If String.IsNullOrEmpty(q.ErrorMessage.Trim) Then
            Dim dtable As New DataTable
            With dtable
                .Columns.Clear()
                For Each dc As DataColumn In q.DataTable.Columns
                    Dim dcol As DataColumn = .Columns.Add(dc.ColumnName, dc.DataType)
                    If dcol.DataType.Name.Contains("Date") Then dcol.DataType = GetType(Date)
                    dcol.ReadOnly = False : dcol.AllowDBNull = True
                Next
            End With
            dtable.Load(q.DataTable.CreateDataReader)

            If dtable.Rows.Count <= 0 Then
                args.Loaded = (NoDataDisplay = NoDataDisplayEnum.AsDisplayed)
                If NoDataDisplay = NoDataDisplayEnum.NoItemsCouldBeViewedDisplay Then
                    Clear(ClearFlags.All)
                    Rows.Count = 1 : Cols.Count = 2
                    Rows(0).Item(1) = "No item(s) could be displayed in this view."
                    Cols(0).Visible = False
                Else
                    DataSource = dtable : PreFormatFlexGrid(Me)
                    AutoSizeCols() : AutoSizeRows()
                    args.Loaded = True
                End If
            Else
                DataSource = dtable : PreFormatFlexGrid(Me)
                AutoSizeCols() : AutoSizeRows()
                args.Loaded = True
            End If

            RaiseEvent AfterDataLoading(Me, args) : ExtendLastCol = True
        Else
            SetNoDisplay()
            RaiseEvent AfterDataLoading(Me, args)
        End If

        VisualStyle = GetApplicableVisualStyle()
        Simple.Redraw(Me) : q.Dispose()
        If LoadingImageBox IsNot Nothing Then LoadingImageBox.Hide()
    End Sub

    ''' <summary>
    ''' Removes the last row in the grid and into the binded data source.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub RemoveItem()
        RemoveRow(Rows(Rows.Count - 1))
    End Sub

    ''' <summary>
    ''' Removes the specified row in the grid and into the binded data source.
    ''' </summary>
    ''' <param name="row"></param>
    ''' <remarks></remarks>
    Public Overloads Sub RemoveItem(ByVal row As Row)
        RemoveRow(row)
    End Sub

    ''' <summary>
    ''' Removes the specified row at the specified index in the grid and into the binded data source.
    ''' </summary>
    ''' <param name="row"></param>
    ''' <remarks></remarks>
    Public Overloads Sub RemoveItem(ByVal row As Integer)
        RemoveRow(Rows(row))
    End Sub

    Private Sub RemoveRow(ByVal row As Row)
        If DataSource IsNot Nothing Then
            Dim dt As DataTable = DataSource
            Dim _filter As String = String.Empty
            Dim _pk As String = String.Empty

            For Each _col As DataColumn In dt.Columns
                If _col.Unique Then
                    _pk = _col.ColumnName : Exit For
                End If
            Next

            If Not String.IsNullOrEmpty(_pk.RLTrim) Then
                If IsNullOrNothing(row.Item(_pk)) Then : _filter = "[" & _pk & "] = NULL"
                Else : _filter = "CONVERT([" & _pk & "], System.String) = '" & row.Item(_pk).ToString.ToSqlValidString(True) & "'"
                End If
            Else
                For Each _col As DataColumn In dt.Columns
                    If IsNullOrNothing(row.Item(_col.ColumnName)) Then : _filter &= IIf(Not String.IsNullOrEmpty(_filter.RLTrim), " AND" & vbNewLine, String.Empty) & "[" & _col.ColumnName & "] = NULL"
                    Else
                        Select Case _col.DataType.Name
                            Case GetType(String).Name : _filter &= IIf(Not String.IsNullOrEmpty(_filter.RLTrim), " AND" & vbNewLine, String.Empty) & "[" & _col.ColumnName & "] = '" & row.Item(_col.ColumnName).ToString.ToSqlValidString(True) & "'"
                            Case GetType(Date).Name : _filter &= IIf(Not String.IsNullOrEmpty(_filter.RLTrim), " AND" & vbNewLine, String.Empty) & "[" & _col.ColumnName & "] = '" & Format(CDate(row.Item(_col.ColumnName)), "MM/dd/yyyy HH:mm:ss") & "'"
                            Case GetType(Boolean).Name, GetType(Decimal).Name, _
                                 GetType(Integer).Name, GetType(Long).Name, _
                                 GetType(SByte).Name, GetType(Byte).Name, _
                                 GetType(Single).Name, GetType(Double).Name : _filter &= IIf(Not String.IsNullOrEmpty(_filter.RLTrim), " AND" & vbNewLine, String.Empty) & "[" & _col.ColumnName & "] = " & row.Item(_col.ColumnName) & ""
                            Case Else
                        End Select
                    End If
                Next
            End If

            If Not String.IsNullOrEmpty(_filter.RLTrim) Then
                Dim rws() As DataRow = dt.Select(_filter)
                If rws.Length > 0 Then rws(0).Delete()
            Else
                Try
                    MyBase.RemoveItem(row.Index)
                Catch ex As Exception
                End Try
            End If
        Else
            Try
                MyBase.RemoveItem(row.Index)
            Catch ex As Exception
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Sets the cell focus on an specific row and column thru the given coordinates.
    ''' </summary>
    ''' <param name="location"></param>
    ''' <remarks></remarks>
    Public Overloads Sub SetCellFocusByPoints(ByVal location As Point)
        SetCellFocusByPoints(location.X, location.Y)
    End Sub

    ''' <summary>
    ''' Sets the cell focus on an specific row and column thru the given coordinates.
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <remarks></remarks>
    Public Overloads Sub SetCellFocusByPoints(ByVal x As Integer, ByVal y As Integer)
        Try
            Dim pt As Point = New Point(x, y)
            For rw As Integer = 0 To Rows.Count - 1
                For c As Integer = 0 To Cols.Count - 1
                    If GetCellRect(rw, c).Contains(pt) Then
                        Row = rw : Col = c
                        RowSel = rw : ColSel = c
                        Exit For
                    End If
                Next

            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Overloads Sub SetCheckMarks(ByVal rw As Row, ByVal check As CheckEnum)
        If rw.IsNode Then
            If rw.Node.Nodes.Count > 0 Then
                For Each cnode As Node In rw.Node.Nodes
                    cnode.Checked = check
                    If cnode.Nodes.Count > 0 Then : SetCheckMarks(cnode.Row, check)
                    Else
                        cnode.Checked = check
                        SetCheckMarks(cnode.Parent)
                    End If
                Next
            Else : SetCheckMarks(rw.Node.Parent)
            End If
        Else : SetCellCheck(rw.Index, 1, check)
        End If
    End Sub

    Private Overloads Sub SetCheckMarks(ByVal parent As Node)
        If parent IsNot Nothing Then
            Dim checked As Boolean = True

            For Each cnode As Node In parent.Nodes
                checked = checked And (cnode.Checked = CheckEnum.Checked)
                If Not checked Then Exit For
            Next

            parent.Checked = IIf(checked, CheckEnum.Checked, CheckEnum.Unchecked)
            If parent.Parent IsNot Nothing Then SetCheckMarks(parent.Parent)
        End If
    End Sub

    ''' <summary>
    ''' Sets the image associated with the specified node when it is selected by the user.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="image"></param>
    ''' <remarks></remarks>
    Public Sub SetNodeExpandedImage(ByVal node As Node, ByVal image As Image)
        If _nodeexpandedimages.ContainsKey(node) Then : CType(_nodeexpandedimages(node), NodeExpadedImageInfo).Image = image
        Else
            Dim ni As New NodeExpadedImageInfo
            ni.Image = image : _nodeexpandedimages.Add(node, ni)
        End If
    End Sub

    ''' <summary>
    ''' Sets the imagelist image at the specified index associated with the specified node when it is selected by the user.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Sub SetNodeExpandedImageIndex(ByVal node As Node, ByVal index As Integer)
        If _nodeexpandedimages.ContainsKey(node) Then : CType(_nodeexpandedimages(node), NodeExpadedImageInfo).ImageIndex = index
        Else
            Dim ni As New NodeExpadedImageInfo
            ni.ImageIndex = index : _nodeexpandedimages.Add(node, ni)
        End If
    End Sub

    ''' <summary>
    ''' Sets the imagelist image with the specified key associated with the specified node when it is selected by the user.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="key"></param>
    ''' <remarks></remarks>
    Public Sub SetNodeExpandedImageKey(ByVal node As Node, ByVal key As String)
        If _nodeexpandedimages.ContainsKey(node) Then : CType(_nodeexpandedimages(node), NodeExpadedImageInfo).ImageKey = key
        Else
            Dim ni As New NodeExpadedImageInfo
            ni.ImageKey = key : _nodeexpandedimages.Add(node, ni)
        End If
    End Sub

    ''' <summary>
    ''' Sets the display grid with the conventional 'No item(s) could be displayed in this view.' message.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub SetNoDisplay()
        SetNoDisplay("No item(s) could be displayed in this view.")
    End Sub

    ''' <summary>
    ''' Sets the display grid with user defined message in a one row- one extended column.
    ''' </summary>
    ''' <param name="caption"></param>
    ''' <remarks></remarks>
    Public Overloads Sub SetNoDisplay(ByVal caption As String)
        Clear(ClearFlags.All)
        Try
            If DataSource IsNot Nothing Then TryCast(DataSource, DataTable).Dispose()
        Catch ex As Exception
        Finally : DataSource = Nothing
        End Try
        Rows.Count = 1 : Cols.Count = 2
        Rows(0).Item(1) = caption
        Cols(0).Visible = False : ExtendLastCol = True : AllowFiltering = False
    End Sub

    ''' <summary>
    ''' Renders the first column with row numberings.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub SetNumberings()
        SetNumberings(0)
    End Sub

    ''' <summary>
    ''' Renders the specified column (with specified name) with row numberings.
    ''' </summary>
    ''' <param name="column"></param>
    ''' <remarks></remarks>
    Public Overloads Sub SetNumberings(ByVal column As String)
        Dim index As Integer = 0
        If Cols.Contains(column) Then index = Cols(column).Index
        SetNumberings(index)
    End Sub

    ''' <summary>
    ''' Renders the specified column index with row numberings.
    ''' </summary>
    ''' <param name="column"></param>
    ''' <remarks></remarks>
    Public Overloads Sub SetNumberings(ByVal column As Integer)
        With Me
            Dim addnew As Boolean = .AllowAddNew : .AllowAddNew = addnew
            .Cols(column).Caption = "#" : Dim count As Integer = 1

            For Each rw As Row In .Rows
                If rw.Index > 0 Then
                    If rw.Visible And
                       Not rw.IsNode Then
                        rw.Item(column) = count
                        count += 1
                    End If
                End If
            Next
            _sortingcolumn = column : .AllowAddNew = addnew
        End With
    End Sub

    ''' <summary>
    ''' Renders the specified column index with row numberings.
    ''' </summary>
    ''' <param name="column"></param>
    ''' <remarks></remarks>
    Public Overloads Sub SetNumberings(ByVal column As Integer, ByVal fixed As Integer)
        With Me
            Dim addnew As Boolean = .AllowAddNew : .AllowAddNew = addnew
            .Cols(column).Caption = "#" : Dim count As Integer = 1

            For Each rw As Row In .Rows
                If rw.Index >= fixed Then
                    If rw.Visible And
                       Not rw.IsNode Then
                        rw.Item(column) = count
                        count += 1
                    End If
                End If
            Next
            _sortingcolumn = column : .AllowAddNew = addnew
        End With
    End Sub

    ''' <summary>
    ''' Sets default values for each of the columns of the specified row.
    ''' </summary>
    ''' <param name="rw"></param>
    ''' <remarks></remarks>
    Public Overloads Sub SetRowDefaultValues(ByVal rw As Row)
        SetRowDefaultValues(rw.Index)
    End Sub

    ''' <summary>
    ''' Sets default values for each of the columns of the specified row index.
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Overloads Sub SetRowDefaultValues(ByVal index As Integer)
        Dim dt As DataTable = DataSource

        For Each c As Column In Cols
            If c.Index > 0 And c.DataType IsNot Nothing Then
                If Not dt.Columns(c.Name).AutoIncrement Then
                    Select Case c.DataType.Name
                        Case GetType(String).Name : Rows(index).Item(c.Name) = String.Empty
                        Case GetType(Date).Name : Rows(index).Item(c.Name) = Now
                        Case GetType(Boolean).Name : Rows(index).Item(c.Name) = False
                        Case GetType(Byte).Name, GetType(Decimal).Name,
                             GetType(Single).Name, GetType(Double).Name,
                             GetType(Integer).Name, GetType(Long).Name : Rows(index).Item(c.Name) = 0
                        Case Else
                    End Select
                End If
            End If
        Next
    End Sub
#End Region

#Region "Functions"

    ''' <summary>
    ''' Gets the summation of the specified column's values.
    ''' </summary>
    ''' <param name="ofcolumn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function ColumnTotal(ByVal ofcolumn As Column) As Double
        Return ColumnTotal(ofcolumn.Index)
    End Function

    ''' <summary>
    ''' Gets the summation of the specified column's values.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function ColumnTotal(ByVal name As String) As Double
        Dim total As Double = 0
        If Cols.Contains(name) Then total = ColumnTotal(Cols(name).Index)
        Return total
    End Function

    ''' <summary>
    ''' Gets the summation of the specified column's values.
    ''' </summary>
    ''' <param name="index"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function ColumnTotal(ByVal index As Integer) As Double
        Dim total As Double = 0 : Dim dt As DataTable = DataSource

        If dt IsNot Nothing Then
            Dim colname As String = Cols(index).Name
            If dt.Columns.Contains(colname) Then
                Dim objtotal As Object = dt.Compute("SUM([" & colname & "])", String.Empty)
                If Not IsNumeric(objtotal) Then objtotal = 0
                total = objtotal
            End If
        Else
            Dim addnew As Boolean = AllowAddNew : AllowAddNew = False
            For rw As Integer = 1 To Rows.Count - 1
                total += CDbl(Rows(rw).Item(index))
            Next
            AllowAddNew = addnew
        End If

        Return total
    End Function

    ''' <summary>
    ''' Exportd the current data in the grid directly to Microsoft Excel.
    ''' </summary>
    ''' <returns>Exported filename.</returns>
    ''' <remarks></remarks>
    Public Overloads Function ExportToExcel() As String
        Return ExportToExcel(False, False)
    End Function

    ''' <summary>
    ''' Exportd the current data in the grid directly to Microsoft Excel.
    ''' </summary>
    ''' <param name="withtree">Include grid's tree structure.</param>
    ''' <returns>Exported filename.</returns>
    ''' <remarks></remarks>
    Public Overloads Function ExportToExcel(ByVal withtree As Boolean) As String
        Return ExportToExcel(withtree, False)
    End Function

    ''' <summary>
    ''' Exportd the current data in the grid directly to Microsoft Excel.
    ''' </summary>
    ''' <param name="withtree">Include grid's tree structure.</param>
    ''' <param name="launchexcel">Launch microsoft excel after data is saved.</param>
    ''' <returns>Exported filename.</returns>
    ''' <remarks></remarks>
    Public Overloads Function ExportToExcel(ByVal withtree As Boolean, ByVal launchexcel As Boolean) As String
        Simple.Redraw(Me, False)

        Dim dir As String = Application.StartupPath & "\Exports"
        Dim filename As String = dir & "\Export.xls"
        Dim treecolumn As Integer = Tree.Column

        If Not withtree Then
            Tree.Column = -1 : Tree.Clear() : Subtotal(AggregateEnum.Clear)
        End If

        If Not System.IO.Directory.Exists(dir) Then
            Try
                My.Computer.FileSystem.CreateDirectory(dir)
            Catch ex As Exception
            End Try
        End If

        If System.IO.Directory.Exists(dir) Then
            If System.IO.File.Exists(filename) Then
                Dim count As Integer = 0

                For Each f As String In IO.Directory.GetFiles(dir)
                    If f.Trim.ToLower.Contains("export") Then count += 1
                Next

                If count > 0 Then filename = dir & "\Export" & count + 1 & ".xls"
            End If

            Try
                SaveExcel(filename, "Exported", FileFlags.AsDisplayed + FileFlags.IncludeFixedCells)
            Catch ex As Exception
                MetroFramework.MetroMessageBox.Show(Me.Parent, "Can't create the file to it's current default destination!<br />Please check if :<br/> - You have specified privilege to access the application's path. <br /> - The export file : " & filename & " is not open or used by other program.", Application.ProductName)
            End Try
        End If

        If launchexcel Then
            If System.IO.File.Exists(filename) Then Process.Start(filename)
        End If

        Tree.Column = treecolumn : Simple.Redraw(Me)

        Return filename
    End Function
#End Region

#Region "Custom Events"
    Private Sub Parent_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
        If (IsTreeView) Or
           (GradientColor.BackColor1.ToArgb <> Color.Transparent.ToArgb And
            GradientColor.BackColor2.ToArgb <> Color.Transparent.ToArgb) Then
            Dim g As Graphics = e.Graphics
            Dim gradientBrush As New LinearGradientBrush(New Point(0, Height), New Point(0, 0), GradientColor.BackColor1, GradientColor.BackColor2)
            g.FillRectangle(gradientBrush, New Rectangle(Location, Size))
        End If
    End Sub
#End Region

#Region "Base Events"

    Private Sub ViewingFlexGrid_AfterAddRow(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles Me.AfterAddRow
        If (_sortingcolumn >= 0) Then SetNumberings(_sortingcolumn)
        If DataSource Is Nothing Then Exit Sub
        If RowSel <= 0 Then Exit Sub
        If Rows(RowSel).IsNode Then Exit Sub
        Dim dt As DataTable = DataSource

        For Each c As Column In Cols
            If c.Index > 0 And c.DataType IsNot Nothing Then
                If Not dt.Columns(c.Name).AutoIncrement Then
                    Select Case c.DataType.Name
                        Case GetType(String).Name : Rows(RowSel).Item(c.Name) = String.Empty
                        Case GetType(Date).Name : Rows(RowSel).Item(c.Name) = Now
                        Case GetType(Boolean).Name : Rows(RowSel).Item(c.Name) = False
                        Case GetType(Byte).Name, GetType(Decimal).Name,
                             GetType(Single).Name, GetType(Double).Name,
                             GetType(Integer).Name, GetType(Long).Name : Rows(RowSel).Item(c.Name) = 0
                        Case Else
                    End Select
                End If
            End If
        Next

        If Me.Rows.Count = 1 Then
            pnlAction.Visible = False
        Else
            pnlAction.Visible = _displayaction
        End If

        Dim _rec As Rectangle = Me.GetCellRect(Me.Row, 0)
        pnlAction.Location = New Point(_rec.Left, _rec.Top)
    End Sub

    Private Sub ViewingFlexGrid_AfterDataRefresh(sender As Object, e As ListChangedEventArgs) Handles Me.AfterDataRefresh
        If Me.Rows.Count = 1 Then
            pnlAction.Visible = False
        Else
            pnlAction.Visible = _displayaction
        End If

        Dim _rec As Rectangle = Me.GetCellRect(Me.Row, 0)
        pnlAction.Location = New Point(_rec.Left, _rec.Top)
    End Sub

    Private Sub ViewingFlexGrid_AfterDeleteRow(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles Me.AfterDeleteRow
        If (_sortingcolumn >= 0) Then
            SetNumberings(_sortingcolumn)
        End If
    End Sub

    Private Sub ViewingFlexGrid_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles Me.AfterEdit
        If IsTreeView And TreeSourceSettings.ShowCheckBoxes Then
            SetCheckMarks(Rows(RowSel), Rows(RowSel).Node.Checked)
        Else
            If e.Row = 0 Then
                RaiseEvent BeforeChangedRowChecked(sender, e)
                If GetCellCheck(0, e.Col) <> CheckEnum.None Then
                    If Cols(e.Col).DataType Is GetType(Boolean) Then
                        For Each rw As Row In Rows
                            If rw.Index >= 1 And
                               Not rw.IsNode Then rw.Item(e.Col) = IIf(GetCellCheck(0, e.Col) = CheckEnum.Checked, 1, 0)
                        Next
                    Else
                        For Each rw As Row In Rows
                            If rw.Index >= 1 And
                               Not rw.IsNode Then SetCellCheck(rw.Index, e.Col, GetCellCheck(0, e.Col))
                        Next
                    End If
                End If
            Else
                RaiseEvent BeforeDataSourceChanged(sender, e)
            End If
        End If
    End Sub

    Private Sub ViewingFlexGrid_AfterFilter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.AfterFilter
        If (_sortingcolumn >= 0) Then SetNumberings(_sortingcolumn)
        ViewingFlexGrid_RowColChange(sender, New EventArgs())
    End Sub

    Private Sub ViewingFlexGrid_AfterScroll(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles Me.AfterScroll
        If IsTreeView Then
            Refresh() : Update() : Simple.Redraw(Me)
        End If

        ViewingFlexGrid_RowColChange(sender, New EventArgs())
    End Sub

    Private Sub ViewingFlexGrid_AfterSelChange(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles Me.AfterSelChange
        If IsTreeView And
           Tag Is Nothing Then
            If RowSel >= 1 Then
                If Rows(RowSel).IsNode Then
                    If e.OldRange.TopRow <> e.NewRange.TopRow Then
                        Dim _eventarg As New RowNodeEventArgs
                        With _eventarg
                            .Col = ColSel : .Row = RowSel
                            .Node = Rows(RowSel).Node
                        End With
                        RaiseEvent AfterNodeSelect(sender, _eventarg)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub ViewingFlexGrid_AfterSort(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.SortColEventArgs) Handles Me.AfterSort
        If (_sortingcolumn >= 0) Then SetNumberings(_sortingcolumn)
    End Sub

    Private Sub ViewingFlexGrid_BeforeDeleteRow(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles Me.BeforeDeleteRow
        If DataSource IsNot Nothing And
           Tag Is Nothing Then
            If e.Row <= 0 Then Exit Sub
            If Rows(e.Row).IsNode Then Exit Sub
            If e.Cancel = True Then Exit Sub
            RaiseEvent BeforeDeletingRows(sender, e)
            RemoveItem(e.Row) : OnAfterDeleteRow(e)
            RaiseEvent AfterDeletingRows(sender, e)
        End If
    End Sub

    Private Sub ViewingFlexGrid_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles Me.BeforeEdit
        If DataSource IsNot Nothing And
           RowSel >= 1 Then
            If Not EditableParentNodes Then e.Cancel = Rows(RowSel).IsNode
        End If
    End Sub

    Private Sub ViewingFlexGrid_BeforeScroll(sender As Object, e As RangeEventArgs) Handles Me.BeforeScroll
        pnlAction.Visible = False
    End Sub

    Private Sub ViewingFlexGrid_BeforeSelChange(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles Me.BeforeSelChange
        If ListItems IsNot Nothing And
           Tag Is Nothing Then
            If ListItems.Count > 0 Then
                If e.NewRange.BottomRow >= 0 And
                   e.NewRange.RightCol >= 0 Then e.Cancel = (GetData(e.NewRange.BottomRow, e.NewRange.RightCol) Is Nothing)
            End If
        End If
    End Sub

    Private Sub ViewingFlexGrid_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataSourceChanged
        Dim dt As DataTable = DataSource
        If dt IsNot Nothing Then
            For Each dc As DataColumn In dt.Columns
                If dc.DataType.Name = GetType(String).Name Then
                    If Cols.Contains(dc.ColumnName) Then Cols(dc.ColumnName).StyleNew.WordWrap = True
                End If
            Next
        End If
    End Sub

    Private Sub ViewingFlexGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
        If ListItems IsNot Nothing Then
            If ListItems.Count > 0 Then
                If RowSel <= 0 Then Exit Sub
                If ColSel <= 0 Then Exit Sub

                If GetData(RowSel, ColSel) IsNot Nothing Then
                    Dim _eventargs As New RowListEventArgs

                    With _eventargs
                        .Row = RowSel : .Col = ColSel
                        .ListViewItem = GetUserData(RowSel, ColSel)
                    End With

                    RaiseEvent ListItemDoubleClick(sender, _eventargs)
                End If
            End If
        End If

        RaiseEvent EditClick(sender, e)
    End Sub

    Private Sub ViewingFlexGrid_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Return, Keys.Space
                lnkEdit.PerformClick()
            Case Keys.Delete
                lnkDelete.PerformClick()
        End Select
    End Sub

    Private Sub ViewingFlexGrid_MouseLeave(sender As Object, e As EventArgs) Handles Me.MouseLeave
        '_horizontal.Visible = False
        '_vertical.Visible = False
    End Sub

    Private Sub ViewingFlexGrid_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If ListItems.Count > 0 And
           Tag Is Nothing And
           ListItemsSelectionMode = ListViewItemSelectionEnum.MouseHover Then
            Try
                Dim _index As Integer = -1 : Dim _col As Integer = -1
                Dim pt As Point = New Point(e.X, e.Y)

                For i As Integer = 0 To Rows.Count - 1
                    For c As Integer = 1 To Cols.Count - 1
                        If GetCellRect(i, c).Contains(pt) Then
                            _index = i : _col = c : Exit For
                        End If
                    Next
                Next

                If _index >= 1 Then
                    If GetUserData(_index, _col) IsNot Nothing Then
                        If Not Focused Then Focus()
                        RowSel = _index : Row = _index
                        ColSel = _col : Col = _col
                    End If
                End If
            Catch ex As Exception
            End Try
        End If


        If e.Y <= Me.Height And e.Y >= Me.Height - 10 Then
            _horizontal.Visible = scrollhelperH.VisibleHorizontalScroll()
        Else
            _horizontal.Visible = true
        End If

        If e.X <= Me.Width And e.X >= Me.Width - 10 Then
            _vertical.Visible = scrollhelper.VisibleVerticalScroll()
        Else
            _vertical.Visible = False
        End If
    End Sub

    Private Sub ViewingFlexGrid_OwnerDrawCell(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.OwnerDrawCellEventArgs) Handles Me.OwnerDrawCell
        Dim rw As Row = Rows(e.Row)

        If rw.IsNode And
           IsTreeView Then
            Dim text As String = e.Text
            e.Text = String.Empty : e.DrawCell()

            Dim rc As Rectangle = e.Bounds
            Dim indent As Integer = Tree.Indent * (rw.Node.Level + 1)

            If RightToLeft = Windows.Forms.RightToLeft.Yes Then
                rc.X = Width - 48
                rc.X -= indent
                rc.Width += indent
            Else
                rc.X += indent
                rc.Width -= indent
            End If

            Dim rcImage As Rectangle = rc
            rcImage.X += 16
            rcImage.Width = 16
            rcImage.Height = 16
            e.Graphics.DrawImage(rw.Node.Image, rcImage)

            Dim rcText As Rectangle = rc

            If RightToLeft = Windows.Forms.RightToLeft.Yes Then
                rcText = e.Bounds : rcText.X -= (indent + 32)
            Else : rcText.X += (16 + rc.Height)
            End If

            Using br As New SolidBrush(e.Style.ForeColor)
                e.Graphics.DrawString(text, e.Style.Font, br, rcText, e.Style.StringFormat)
            End Using
        End If
    End Sub

    Private Sub ViewingFlexGrid_ParentChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ParentChanged
        If Parent IsNot Nothing Then AddHandler Parent.Paint, AddressOf Parent_Paint
        'VisualStyle = GetApplicableVisualStyle()
    End Sub

    Dim _previousimage As Image = Nothing
    Dim _previousnode As Node = Nothing

    Private Sub ViewingFlexGrid_RowColChange(sender As Object, e As EventArgs) Handles Me.RowColChange
        If Me.Rows.Count > 1 Then
            If (Me.Row <= 0) Then Exit Sub
            If (Not (Me.Row >= Me.TopRow And Me.Row <= Me.Bottom)) Then Exit Sub

            pnlAction.Visible = _displayaction
            pnlAction.Height = Me.Rows(Me.Row).HeightDisplay

            Dim _rec As Rectangle = Me.GetCellRect(Me.Row, 0)
            pnlAction.Location = New Point(_rec.Left, _rec.Top)
        Else
            pnlAction.Visible = False
        End If
    End Sub

    Private Sub ViewingFlexGrid_SelChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SelChange
        If Not IsTreeView Then Exit Sub
        If Tag IsNot Nothing Then Exit Sub
        If RowSel <= 0 Then Exit Sub

        If _previousnode IsNot Nothing Then
            Try
                _previousnode.Image = _previousimage
            Catch ex As Exception
            End Try
        End If

        If Rows(RowSel).IsNode Then
            Dim ni As NodeExpadedImageInfo = GetNodeExpandedImageInfo(Rows(RowSel).Node)
            If ni IsNot Nothing Then
                _previousnode = Rows(RowSel).Node : _previousimage = Rows(RowSel).Node.Image
                If ni.Image IsNot Nothing Then : Rows(RowSel).Node.Image = ni.Image
                Else
                    If ImageList IsNot Nothing Then
                        If Not String.IsNullOrEmpty(ni.ImageKey.Trim) Then : Rows(RowSel).Node.Image = ImageList.Images(ni.ImageKey)
                        Else
                            If ni.ImageIndex >= 0 Then Rows(RowSel).Node.Image = ImageList.Images(ni.ImageIndex)
                        End If
                    End If
                End If
            End If
        Else
        End If
    End Sub
#End Region

#Region "Interface"
    <Category("Metro Appearance")> _
    Public Event CustomPaintBackground As EventHandler(Of MetroPaintEventArgs) Implements MetroFramework.Interfaces.IMetroControl.CustomPaintBackground
    Protected Overridable Sub OnCustomPaintBackground(e As MetroPaintEventArgs)
        If GetStyle(ControlStyles.UserPaint) Then
            RaiseEvent CustomPaintBackground(Me, e)
        End If
    End Sub

    <Category("Metro Appearance")> _
    Public Event CustomPaint As EventHandler(Of MetroPaintEventArgs) Implements MetroFramework.Interfaces.IMetroControl.CustomPaint
    Protected Overridable Sub OnCustomPaint(e As MetroPaintEventArgs)
        If GetStyle(ControlStyles.UserPaint) Then
            RaiseEvent CustomPaint(Me, e)
        End If
    End Sub

    <Category("Metro Appearance")> _
    Public Event CustomPaintForeground As EventHandler(Of MetroPaintEventArgs) Implements MetroFramework.Interfaces.IMetroControl.CustomPaintForeground
    Protected Overridable Sub OnCustomPaintForeground(e As MetroPaintEventArgs)
        If GetStyle(ControlStyles.UserPaint) Then
            RaiseEvent CustomPaintForeground(Me, e)
        End If
    End Sub

    Private metroStyle As MetroColorStyle = MetroColorStyle.[Default]
    <Category("Metro Appearance")> _
    <DefaultValue(MetroColorStyle.[Default])> _
    Public Property Style() As MetroColorStyle Implements MetroFramework.Interfaces.IMetroControl.Style
        Get
            If DesignMode OrElse metroStyle <> MetroColorStyle.[Default] Then
                Return metroStyle
            End If

            If StyleManager IsNot Nothing AndAlso metroStyle = MetroColorStyle.[Default] Then
                Return StyleManager.Style
            End If
            If StyleManager Is Nothing AndAlso metroStyle = MetroColorStyle.[Default] Then
                Return MetroColorStyle.Blue
            End If

            Return metroStyle
        End Get
        Set(value As MetroColorStyle)
            metroStyle = value
            StyleGrid()
        End Set
    End Property

    Private metroTheme As MetroThemeStyle = MetroThemeStyle.[Default]
    <Category("Metro Appearance")> _
    <DefaultValue(MetroThemeStyle.[Default])> _
    Public Property Theme() As MetroThemeStyle Implements MetroFramework.Interfaces.IMetroControl.Theme
        Get
            If DesignMode OrElse metroTheme <> MetroThemeStyle.[Default] Then
                Return metroTheme
            End If

            If StyleManager IsNot Nothing AndAlso metroTheme = MetroThemeStyle.[Default] Then
                Return StyleManager.Theme
            End If
            If StyleManager Is Nothing AndAlso metroTheme = MetroThemeStyle.[Default] Then
                Return MetroThemeStyle.Light
            End If

            Return metroTheme
        End Get
        Set(value As MetroThemeStyle)
            metroTheme = value
            StyleGrid()
        End Set
    End Property

    Private metroStyleManager As MetroStyleManager = Nothing
    <Browsable(False)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property StyleManager() As MetroStyleManager Implements MetroFramework.Interfaces.IMetroControl.StyleManager
        Get
            Return metroStyleManager
        End Get
        Set(value As MetroStyleManager)
            metroStyleManager = value
            StyleGrid()
        End Set
    End Property

    Private m_useCustomBackColor As Boolean = False
    <DefaultValue(False)> _
    <Category("Metro Appearance")> _
    Public Property UseCustomBackColor() As Boolean Implements MetroFramework.Interfaces.IMetroControl.UseCustomBackColor
        Get
            Return m_useCustomBackColor
        End Get
        Set(value As Boolean)
            m_useCustomBackColor = value
        End Set
    End Property

    Private m_useCustomForeColor As Boolean = False
    <DefaultValue(False)> _
    <Category("Metro Appearance")> _
    Public Property UseCustomForeColor() As Boolean Implements MetroFramework.Interfaces.IMetroControl.UseCustomForeColor
        Get
            Return m_useCustomForeColor
        End Get
        Set(value As Boolean)
            m_useCustomForeColor = value
        End Set
    End Property

    Private m_useStyleColors As Boolean = False
    <DefaultValue(False)> _
    <Category("Metro Appearance")> _
    Public Property UseStyleColors() As Boolean Implements MetroFramework.Interfaces.IMetroControl.UseStyleColors
        Get
            Return m_useStyleColors
        End Get
        Set(value As Boolean)
            m_useStyleColors = value
        End Set
    End Property

    <Browsable(False)> _
    <Category("Metro Behaviour")> _
    <DefaultValue(True)> _
    Public Property UseSelectable() As Boolean Implements MetroFramework.Interfaces.IMetroControl.UseSelectable
        Get
            Return GetStyle(ControlStyles.Selectable)
        End Get
        Set(value As Boolean)
            SetStyle(ControlStyles.Selectable, value)
        End Set
    End Property

#End Region

#Region "Fields"

    Private displayFocusRectangle As Boolean = False
    <DefaultValue(False)> _
    <Category("Metro Appearance")> _
    Public Property DisplayFocus() As Boolean
        Get
            Return displayFocusRectangle
        End Get
        Set(value As Boolean)
            displayFocusRectangle = value
        End Set
    End Property


    Private metroDateTimeSize As MetroDateTimeSize = metroDateTimeSize.Medium
    <DefaultValue(metroDateTimeSize.Medium)> _
    <Category("Metro Appearance")> _
    Public Property FontSize() As MetroDateTimeSize
        Get
            Return metroDateTimeSize
        End Get
        Set(value As MetroDateTimeSize)
            metroDateTimeSize = value
        End Set
    End Property

    Private metroDateTimeWeight As MetroDateTimeWeight = metroDateTimeWeight.Regular
    <DefaultValue(metroDateTimeWeight.Regular)> _
    <Category("Metro Appearance")> _
    Public Property FontWeight() As MetroDateTimeWeight
        Get
            Return metroDateTimeWeight
        End Get
        Set(value As MetroDateTimeWeight)
            metroDateTimeWeight = value
        End Set
    End Property


    Private isHovered As Boolean = False
    Private isPressed As Boolean = False
    Private isFocused As Boolean = False

#End Region

#Region "Styling"
    Private Sub StyleGrid()
        Me.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None

        Me.Styles.Alternate.BackColor = If((Theme = MetroThemeStyle.Light), Color.FromArgb(244, 244, 244), Color.FromArgb(40, 40, 40))
        Me.Styles.Highlight.BackColor = Color.FromArgb(90, MetroPaint.GetStyleColor(Style).R, MetroPaint.GetStyleColor(Style).G, MetroPaint.GetStyleColor(Style).B)
        ' Color.FromArgb(90,25, 199, 244);
        Me.Styles.Highlight.ForeColor = If((Theme = MetroThemeStyle.Light), Color.FromArgb(17, 17, 17), Color.FromArgb(255, 255, 255))

        Me.Styles.Fixed.BackColor = MetroPaint.GetStyleColor(Style)
        Me.Styles.Fixed.Border.Color = MetroPaint.GetStyleColor(Style)
        Me.Styles.Fixed.ForeColor = MetroPaint.ForeColor.Button.Press(Theme)
        Me.Styles.Fixed.Font = New Font("Tahoma", 9.0F)
        'MetroFonts.Label(MetroLabelSize.Small, MetroLabelWeight.Regular);
        Me.Styles.Normal.BackColor = MetroPaint.BackColor.Form(Theme)
        Me.Styles.Normal.Border.Color = If((Theme = MetroThemeStyle.Light), Color.FromArgb(229, 229, 229), Color.FromArgb(93, 93, 93))
        Me.Styles.Normal.Border.Direction = C1.Win.C1FlexGrid.BorderDirEnum.Vertical
        Me.Styles.Normal.Border.Style = C1.Win.C1FlexGrid.BorderStyleEnum.Flat

        Me.Styles.EmptyArea.BackColor = MetroPaint.BackColor.Form(Theme)
        Me.Styles.EmptyArea.Border.Style = C1.Win.C1FlexGrid.BorderStyleEnum.Flat
        Me.Styles.EmptyArea.Border.Color = If((Theme = MetroThemeStyle.Light), Color.FromArgb(229, 229, 229), Color.FromArgb(93, 93, 93))

        Me.BackColor = MetroPaint.BackColor.Form(Theme)
        Me.ForeColor = MetroPaint.ForeColor.Button.Disabled(MetroThemeStyle.Dark)
        Me.Font = New Font("Segoe UI", 11.0F, FontStyle.Regular, GraphicsUnit.Pixel)
        'MetroFonts.Label(MetroLabelSize.Small, MetroLabelWeight.Regular);
        Me.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid

        Me.Rows(0).Height = 25
    End Sub
#End Region

    Sub New()
        InitializeComponent()

        StyleGrid()

        Me.Controls.Add(_vertical)
        Me.Controls.Add(_horizontal)
        Me.Controls.Add(pnlAction)

        scrollhelper = New MetroGridHelper(_vertical, Me)
        scrollhelperH = New MetroGridHelper(_horizontal, Me, False)

        'AddHandler _vertical.MouseLeave, AddressOf Metro_MouseLeave
        'AddHandler _horizontal.MouseLeave, AddressOf Metro_MouseLeave

        pnlAction.Visible = _displayaction
        If Me.Rows.Count > 1 Then
            Dim _rec As Rectangle = Me.GetCellRect(Me.Row, 0)

            pnlAction.Location = New Point(_rec.Left, _rec.Top)
        End If

        AddHandler lnkEdit.Click, AddressOf lnkEdit_Click
        AddHandler lnkDelete.Click, AddressOf lnkDelete_Click
    End Sub

    Private Sub lnkEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.Row <= 0 Then Exit Sub
        If Me.Col <= 0 Then Exit Sub

        RaiseEvent EditClick(sender, e)
    End Sub

    Private Sub lnkDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.Row <= 0 Then Exit Sub
        If Me.Col <= 0 Then Exit Sub

        RaiseEvent DeleteClick(sender, e)
    End Sub

    Dim scrollhelper As MetroGridHelper
    Dim scrollhelperH As MetroGridHelper

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ViewingFlexGrid))
        Me._vertical = New MetroFramework.Controls.MetroScrollBar()
        Me._horizontal = New MetroFramework.Controls.MetroScrollBar()
        Me.pnlAction = New System.Windows.Forms.Panel()
        Me.lnkDelete = New MetroFramework.Controls.MetroLink()
        Me.lnkEdit = New MetroFramework.Controls.MetroLink()
        Me.pnlAction.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_vertical
        '
        Me._vertical.LargeChange = 10
        Me._vertical.Location = New System.Drawing.Point(0, 0)
        Me._vertical.Maximum = 100
        Me._vertical.Minimum = 0
        Me._vertical.MouseWheelBarPartitions = 10
        Me._vertical.Name = "_vertical"
        Me._vertical.Orientation = MetroFramework.Controls.MetroScrollOrientation.Vertical
        Me._vertical.ScrollbarSize = 10
        Me._vertical.Size = New System.Drawing.Size(10, 200)
        Me._vertical.TabIndex = 0
        Me._vertical.UseSelectable = True
        '
        '_horizontal
        '
        Me._horizontal.LargeChange = 10
        Me._horizontal.Location = New System.Drawing.Point(0, 0)
        Me._horizontal.Maximum = 100
        Me._horizontal.Minimum = 0
        Me._horizontal.MouseWheelBarPartitions = 10
        Me._horizontal.Name = "_horizontal"
        Me._horizontal.Orientation = MetroFramework.Controls.MetroScrollOrientation.Horizontal
        Me._horizontal.ScrollbarSize = 10
        Me._horizontal.Size = New System.Drawing.Size(200, 10)
        Me._horizontal.TabIndex = 0
        Me._horizontal.UseSelectable = True
        '
        'pnlAction
        '
        Me.pnlAction.BackColor = System.Drawing.Color.Transparent
        Me.pnlAction.Controls.Add(Me.lnkDelete)
        Me.pnlAction.Controls.Add(Me.lnkEdit)
        Me.pnlAction.Location = New System.Drawing.Point(120, 86)
        Me.pnlAction.Name = "pnlAction"
        Me.pnlAction.Size = New System.Drawing.Size(44, 20)
        Me.pnlAction.TabIndex = 5
        '
        'lnkDelete
        '
        Me.lnkDelete.Dock = System.Windows.Forms.DockStyle.Left
        Me.lnkDelete.Image = CType(resources.GetObject("lnkDelete.Image"), System.Drawing.Image)
        Me.lnkDelete.ImageSize = 0
        Me.lnkDelete.Location = New System.Drawing.Point(22, 0)
        Me.lnkDelete.Name = "lnkDelete"
        Me.lnkDelete.NoFocusImage = CType(resources.GetObject("lnkDelete.NoFocusImage"), System.Drawing.Image)
        Me.lnkDelete.Size = New System.Drawing.Size(22, 20)
        Me.lnkDelete.TabIndex = 1
        Me.lnkDelete.UseCustomBackColor = True
        Me.lnkDelete.UseSelectable = True
        '
        'lnkEdit
        '
        Me.lnkEdit.Dock = System.Windows.Forms.DockStyle.Left
        Me.lnkEdit.Image = CType(resources.GetObject("lnkEdit.Image"), System.Drawing.Image)
        Me.lnkEdit.ImageSize = 0
        Me.lnkEdit.Location = New System.Drawing.Point(0, 0)
        Me.lnkEdit.Name = "lnkEdit"
        Me.lnkEdit.NoFocusImage = CType(resources.GetObject("lnkEdit.NoFocusImage"), System.Drawing.Image)
        Me.lnkEdit.Size = New System.Drawing.Size(22, 20)
        Me.lnkEdit.TabIndex = 0
        Me.lnkEdit.UseCustomBackColor = True
        Me.lnkEdit.UseSelectable = True
        '
        'ViewingFlexGrid
        '
        Me.Rows.DefaultSize = 19
        Me.pnlAction.ResumeLayout(False)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Private Sub Metro_MouseLeave(ByVal sender As Object, ByVal e As EventArgs)
        _horizontal.Visible = False
        _vertical.Visible = False
    End Sub
End Class


Public Class MetroGridHelper
    ''' <summary>
    ''' The associated scrollbar or scrollbar collector
    ''' </summary>
    Private _scrollbar As MetroScrollBar

    ''' <summary>
    ''' Associated Grid
    ''' </summary>
    Private _grid As C1FlexGrid

    ''' <summary>
    ''' if greater zero, scrollbar changes are ignored
    ''' </summary>
    Private _ignoreScrollbarChange As Integer = 0

    ''' <summary>
    ''' 
    ''' </summary>
    Private _ishorizontal As Boolean = False

    Public Sub New(scrollbar As MetroScrollBar, grid As C1FlexGrid, Optional vertical As Boolean = True)
        _scrollbar = scrollbar
        _scrollbar.UseBarColor = True
        _grid = grid
        _ishorizontal = Not vertical

        grid.ScrollBars = System.Windows.Forms.ScrollBars.None

        AddHandler _grid.AfterScroll, AddressOf _grid_AfterScroll
        AddHandler _grid.AfterAddRow, AddressOf _grid_AfterAddRow
        AddHandler _grid.AfterDeleteRow, AddressOf _grid_AfterDeleteRow
        AddHandler _grid.AfterDataRefresh, AddressOf _grid_AfterDataRefresh
        AddHandler _grid.Resize, AddressOf _grid_Resize
        AddHandler _scrollbar.Scroll, AddressOf _scrollbar_Scroll

        ' += new ScrollValueChangedDelegate(_scrollbar_ValueChanged);
        _scrollbar.Visible = True
        UpdateScrollbar()
    End Sub

    Private Sub _scrollbar_Scroll(sender As Object, e As System.Windows.Forms.ScrollEventArgs)
        If _ignoreScrollbarChange > 0 Then
            Return
        End If

        If _ishorizontal Then
            If _scrollbar.Value >= 0 AndAlso _scrollbar.Value < _grid.Cols.Count Then
                _grid.LeftCol = If((_grid.Cols.Fixed = 0 AndAlso _scrollbar.Value = 1), 0, _scrollbar.Value)
            End If
        Else
            If _scrollbar.Value >= 0 AndAlso _scrollbar.Value < _grid.Rows.Count Then
                _grid.TopRow = _scrollbar.Value
            End If
        End If
    End Sub

    Private Sub BeginIgnoreScrollbarChangeEvents()
        _ignoreScrollbarChange += 1
    End Sub

    Private Sub EndIgnoreScrollbarChangeEvents()
        If _ignoreScrollbarChange > 0 Then
            _ignoreScrollbarChange -= 1
        End If
    End Sub

    ''' <summary>
    ''' Updates the scrollbar values
    ''' </summary>
    Public Sub UpdateScrollbar()
        Try
            BeginIgnoreScrollbarChangeEvents()

            If _ishorizontal Then
                Dim visibleCols As Integer = VisibleFlexGridCols()
                _scrollbar.Maximum = _grid.Cols.Count - visibleCols + 1
                _scrollbar.Minimum = 1
                _scrollbar.SmallChange = 1
                _scrollbar.LargeChange = Math.Max(1, visibleCols - 1)
                _scrollbar.Value = _grid.LeftCol
                _scrollbar.Location = New Point(0, _grid.Height - 10)
                _scrollbar.Width = _grid.Width
                _scrollbar.BringToFront()
            Else
                Dim visibleRows As Integer = VisibleFlexGridRows()
                _scrollbar.Maximum = _grid.Rows.Count - visibleRows + 1
                _scrollbar.Minimum = 1
                _scrollbar.SmallChange = 1
                _scrollbar.LargeChange = Math.Max(1, visibleRows - 1)
                _scrollbar.Value = _grid.TopRow

                _scrollbar.Location = New Point(_grid.Width - 10, 0)
                _scrollbar.Height = _grid.Height
                _scrollbar.BringToFront()
            End If
        Finally
            EndIgnoreScrollbarChangeEvents()
        End Try
    End Sub

    ''' <summary>
    ''' Determine the current count of visible rows
    ''' </summary>
    ''' <returns></returns>
    Private Function VisibleFlexGridRows() As Integer
        Return _grid.BottomRow - _grid.TopRow
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function VisibleFlexGridCols() As Integer
        Return _grid.RightCol - _grid.LeftCol
    End Function

    Public Function VisibleVerticalScroll() As Boolean
        Dim _return As Boolean = False
        Dim _rowheight As Integer = 0

        For Each _row As Row In _grid.Rows
            If _row.Index = 0 Then
                Continue For
            End If
            If Not _row.Visible Then
                Continue For
            End If

            _rowheight += _row.HeightDisplay

            If _rowheight > (_grid.Height - _grid.Rows(0).HeightDisplay) Then
                _return = True
                Exit For
            End If
        Next

        Return _return
    End Function

    Public Function VisibleHorizontalScroll() As Boolean
        Dim _return As Boolean = False
        Dim _colwidth As Integer = 0

        For Each _col As Column In _grid.Cols
            If Not _col.Visible Then
                Continue For
            End If
            _colwidth += _col.WidthDisplay

            If _colwidth > _grid.Width Then
                _return = True
                Exit For
            End If
        Next

        Return _return
    End Function
#Region "Events of interest"

    Private Sub _grid_Resize(sender As Object, e As EventArgs)
        UpdateScrollbar()
    End Sub


    Private Sub _grid_AfterDeleteRow(sender As Object, e As RowColEventArgs)
        UpdateScrollbar()
    End Sub

    Private Sub _grid_AfterAddRow(sender As Object, e As RowColEventArgs)
        UpdateScrollbar()
    End Sub

    Private Sub _grid_AfterScroll(sender As Object, e As RangeEventArgs)
        UpdateScrollbar()
    End Sub

    Private Sub _grid_AfterDataRefresh(sender As Object, e As System.ComponentModel.ListChangedEventArgs)
        UpdateScrollbar()
    End Sub

    Private Sub _scrollbar_ValueChanged(sender As MetroScrollBar, newValue As Integer)
        If _ignoreScrollbarChange > 0 Then
            Return
        End If

        If _ishorizontal Then
            If newValue >= 0 AndAlso newValue < _grid.Cols.Count Then
                _grid.LeftCol = newValue
            End If
        Else
            If newValue >= 0 AndAlso newValue < _grid.Rows.Count Then
                _grid.TopRow = newValue
            End If
        End If
    End Sub
#End Region
End Class
