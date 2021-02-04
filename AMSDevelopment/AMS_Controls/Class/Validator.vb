#Region "Imports"
Imports C1.Win.C1Input
Imports C1.Win.C1List
Imports DevComponents.Editors
Imports DevComponents.Editors.DateTimeAdv
Imports DevComponents.DotNetBar.Controls
Imports DevComponents.DotNetBar.Validator
Imports MetroFramework.Controls

#End Region

Namespace Components
    ''' <summary>
    ''' Derived class from a DevComponents.DotNetBar.Validator.SuperValidator to explicitly implement it even not thru design time.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Validator
        Inherits SuperValidator

        Dim _higlighter As Highlighter = Nothing
        Dim _errorprovider As ErrorProvider = Nothing

        ''' <summary>
        ''' Creates a new instance of TAMS20.Components.Validator.
        ''' </summary>
        ''' <param name="owner">Any object that implements IWin32Window interface to serve as the Validator's container.</param>
        ''' <remarks></remarks>
        Sub New(ByVal owner As IWin32Window)
            _higlighter = New Highlighter : _higlighter.ContainerControl = owner
            _errorprovider = New ErrorProvider
            With _errorprovider
                .ContainerControl = owner
                .Icon = My.Resources.validator
            End With
            MyBase.ContainerControl = owner
            MyBase.ErrorProvider = _errorprovider : MyBase.Highlighter = _higlighter
            ApplyHighlighter(CType(owner, Form), _higlighter)
        End Sub

        ''' <summary>
        ''' Applies control highlights to each of the supported controls inside the specified form.
        ''' </summary>
        ''' <param name="owner">Form to apply the control highlights into.</param>
        ''' <param name="highlighter">Highlighter control to refer.</param>
        ''' <remarks></remarks>
        Public Shared Sub ApplyHighlighter(ByVal owner As Form, ByVal highlighter As Highlighter)
            For Each ctrl As Control In owner.Controls
                'ApplyHighlighter(ctrl, highlighter)
            Next
        End Sub

        ''' <summary>
        ''' Applies control highlights to the specified control (or for each controls inside it).
        ''' </summary>
        ''' <param name="control">Control to apply the highlights into.</param>
        ''' <param name="highlighter">Highlighter control to refer.</param>
        ''' <remarks></remarks>
        Public Shared Sub ApplyHighlighter(ByVal control As Control, ByVal highlighter As Highlighter)
            highlighter.FocusHighlightColor = eHighlightColor.Orange

            If TypeOf control Is TextBoxX Or _
               TypeOf control Is ComboBoxEx Or _
               TypeOf control Is C1Combo Or _
               TypeOf control Is ComboTree Or _
               TypeOf control Is IntegerInput Or _
               TypeOf control Is DoubleInput Or _
               TypeOf control Is DateTimeInput Or _
               TypeOf control Is CheckBoxX Then : highlighter.SetHighlightOnFocus(control, True)
            Else
                Select Case control.GetType.BaseType.Name
                    Case "SearchControl"
                    Case GetType(MetroTextBox).Name
                    Case GetType(TextBox).Name, GetType(ComboBox).Name, _
                         GetType(DateTimePicker).Name, GetType(NumericUpDown).Name, _
                         GetType(CheckBox).Name, GetType(C1DropDownControl).Name : highlighter.SetHighlightOnFocus(control, True)
                    Case Else
                        If control.Controls.Count > 0 Then
                            For Each ctrl As Control In control.Controls
                                ApplyHighlighter(ctrl, highlighter)
                            Next
                        End If
                End Select
            End If
        End Sub
    End Class

    ''' <summary>
    ''' Collection of the system's form validators.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ValidatorCollection
        Inherits CollectionBase
        Implements IDisposable

        ''' <summary>
        ''' Gets or sets the Validator object the specified index of the collection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Overloads Property Validator(ByVal index As Integer) As Validator
            Get
                Return CType(List(index), Validator)
            End Get
            Set(ByVal value As Validator)
                List(index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the Validator object with the specified owner in the collection.
        ''' </summary>
        ''' <param name="owner"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Overloads Property Validator(ByVal owner As IWin32Window) As Validator
            Get
                For Each v As Validator In List
                    If v.ContainerControl Is owner Then
                        Return v : Exit Property
                    End If
                Next

                Return Nothing
            End Get
            Set(ByVal value As Validator)
                For Each v As Validator In List
                    If v.ContainerControl Is owner Then
                        v = value : Exit Property
                    End If
                Next
            End Set
        End Property

        ''' <summary>
        ''' Adds a Validator object with the specified owner in the collection.
        ''' </summary>
        ''' <param name="owner"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Add(ByVal owner As IWin32Window) As Integer
            If Not Contains(owner) Then
                Dim validator As New Validator(owner)
                Return List.Add(validator) : Exit Function
            Else
                For i As Integer = 0 To List.Count - 1
                    If CType(List(i), Validator).ContainerControl Is owner Then
                        Return i : Exit Function
                    End If
                Next
            End If
            Return -1
        End Function

        ''' <summary>
        ''' Determines whether a validator associated with the specified owner already exists within the collection.
        ''' </summary>
        ''' <param name="owner"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Contains(ByVal owner As IWin32Window) As Boolean
            For Each v As Validator In List
                If v.ContainerControl Is owner Then
                    Return True : Exit Function
                End If
            Next
            Return False
        End Function

        ''' <summary>
        ''' Removes the specified Validator with the specified owner within the collection.
        ''' </summary>
        ''' <param name="owner"></param>
        ''' <remarks></remarks>
        Public Sub Remove(ByVal owner As IWin32Window)
            For Each v As Validator In List
                If v.ContainerControl Is owner Then
                    List.Remove(v) : Exit Sub
                End If
            Next
        End Sub

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
                    For Each v As Validator In List
                        v.Dispose()
                    Next
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
End Namespace


