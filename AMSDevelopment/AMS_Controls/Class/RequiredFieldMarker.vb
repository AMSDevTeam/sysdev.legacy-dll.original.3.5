Imports System.ComponentModel
Imports TarsierEyes

Namespace Controls
    ''' <summary>
    ''' Control extender for marking input controls with a tiny indicator.
    ''' </summary>
    ''' <remarks></remarks>
    <ProvideProperty("Required", GetType(Control)), ProvideProperty("RequiredIndicatorColor", GetType(Control)), _
     ProvideProperty("RequiredIndicatorToolTip", GetType(Control)), Description("Control extender for marking input controls with a tiny indicator."), ToolboxBitmap(GetType(RequiredFieldMarker), "RequiredFieldMarker.bmp")> _
    Public Class RequiredFieldMarker
        Inherits Component
        Implements IExtenderProvider

#Region "Enumerations"
        ''' <summary>
        ''' Required field marking's position within the controls bounds.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum IndicatorPositionEnum
            ''' <summary>
            ''' Upper left corner of the control.
            ''' </summary>
            ''' <remarks></remarks>
            LeftTop = 0
            ''' <summary>
            ''' Upper right corner of the control.
            ''' </summary>
            ''' <remarks></remarks>
            RigthTop = 1
        End Enum

#End Region

#Region "Variable Declarations"
        Private _ht As New Hashtable()
#End Region

#Region "Sub New"
        ''' <summary>
        ''' Creates a new instance of TarsierEyes.Controls.RequiredFieldMarker.
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            _indicatorposition = IndicatorPositionEnum.LeftTop
        End Sub
#End Region

#Region "Properties"
        Private _indicatorposition As IndicatorPositionEnum = IndicatorPositionEnum.LeftTop

        ''' <summary>
        ''' Gets or sets required field indicator's position within the control's bounds.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DefaultValue(GetType(IndicatorPositionEnum), "LeftTop"), Description("Gets or sets required field indicator's position within the control's bounds.")> _
        Public Property IndicatorPosition() As IndicatorPositionEnum
            Get
                Return _indicatorposition
            End Get
            Set(ByVal value As IndicatorPositionEnum)
                _indicatorposition = value
                UpdateIndicators()
            End Set
        End Property

        ''' <summary>
        ''' Gets required field indicator's presence for this control.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates required field indicator's presence for this control."), _
         DefaultValue(GetType(Boolean), "False")> _
        Public Function GetRequired(ByVal ctrl As Control) As Boolean
            If CanExtend(ctrl) Then
                Return _ht.Contains(ctrl)
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Sets required field indicator's presence for this control.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <param name="req"></param>
        ''' <remarks></remarks>
        <Description("Indicates required field indicator's presence for this control."), _
         DefaultValue(GetType(Boolean), "False")> _
        Public Sub SetRequired(ByVal ctrl As Control, ByVal req As Boolean)
            If CanExtend(ctrl) Then
                If req Then
                    If Not _ht.Contains(ctrl) Then
                        _ht.Add(ctrl, Color.FromArgb(213, 65, 44))
                    Else
                        _ht(ctrl) = Color.FromArgb(213, 65, 44)
                    End If

                    UnpinIndicator(ctrl)
                    AddHandler ctrl.Resize, AddressOf PinIndicator
                    AddHandler ctrl.LocationChanged, AddressOf PinIndicator
                    AddHandler ctrl.ParentChanged, AddressOf PinIndicator
                    AddHandler ctrl.VisibleChanged, AddressOf PinIndicator
                    PinIndicator(ctrl)
                Else
                    If _ht.Contains(ctrl) Then _ht.Remove(ctrl)
                    RemoveHandler ctrl.Resize, AddressOf PinIndicator
                    RemoveHandler ctrl.LocationChanged, AddressOf PinIndicator
                    RemoveHandler ctrl.ParentChanged, AddressOf PinIndicator
                    RemoveHandler ctrl.VisibleChanged, AddressOf PinIndicator
                    UnpinIndicator(ctrl)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Gets required field indicator's fill color.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates required field indicator's fill color."), _
         DefaultValue(GetType(Color), "OrangeRed")> _
        Public Function GetRequiredIndicatorColor(ByVal ctrl As Control) As Color
            If CanExtend(ctrl) Then
                If _ht.Contains(ctrl) Then
                    Return _ht(ctrl)
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Sets required field indicator's fill color.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <param name="clr"></param>
        ''' <remarks></remarks>
        <Description("Indicates required field indicator's fill color."), _
         DefaultValue(GetType(Color), "OrangeRed")> _
        Public Sub SetRequiredIndicatorColor(ByVal ctrl As Control, ByVal clr As Color)
            If CanExtend(ctrl) Then
                If IsRequired(ctrl) Then
                    If Not _ht.Contains(ctrl) Then
                        _ht.Add(ctrl, clr)
                    Else
                        _ht(ctrl) = clr
                    End If

                    UnpinIndicator(ctrl)
                    AddHandler ctrl.Resize, AddressOf PinIndicator
                    AddHandler ctrl.LocationChanged, AddressOf PinIndicator
                    AddHandler ctrl.ParentChanged, AddressOf PinIndicator
                    AddHandler ctrl.VisibleChanged, AddressOf PinIndicator
                    PinIndicator(ctrl)
                Else
                    If _ht.Contains(ctrl) Then _ht.Remove(ctrl)
                    RemoveHandler ctrl.Resize, AddressOf PinIndicator
                    RemoveHandler ctrl.LocationChanged, AddressOf PinIndicator
                    RemoveHandler ctrl.ParentChanged, AddressOf PinIndicator
                    RemoveHandler ctrl.VisibleChanged, AddressOf PinIndicator
                    UnpinIndicator(ctrl)
                End If
            End If
        End Sub
#End Region

#Region "Exposed Functions"
        ''' <summary>
        ''' Validates if evaluated control is supported by P5Require.
        ''' </summary>
        ''' <param name="extendee">Control to be evaluated.</param>
        ''' <returns>True or false</returns>
        ''' <remarks></remarks>
        Public Function CanExtend(ByVal extendee As Object) As Boolean Implements System.ComponentModel.IExtenderProvider.CanExtend
            Select Case extendee.GetType.FullName.Trim
                Case "DevComponents.DotNetBar.Controls.TextBoxX", _
                    "MetroFramework.Controls.MetroTextBox", _
                     "DevComponents.DotNetBar.Controls.ComboBoxEx", _
                     "DevComponents.Editors.DateTimeAdv.DateTimeInput", _
                     "DevComponents.Editors.DoubleInput", _
                     "DevComponents.Editors.IntegerInput", _
                     "DevComponents.DotNetBar.Controls.ComboTree", _
                     "System.Windows.Forms.TextBox", _
                     "System.Windows.Forms.ComboBox", _
                     "System.Windows.Forms.DateTimePicker", _
                     "C1.Win.C1List.C1Combo", _
                     "System.Windows.Forms.NumericUpDown", _
                     "System.Windows.Forms.RichTextBox", _
                     "System.Windows.Forms.MaskedTextBox", _
                     "C1.Win.C1Input.C1DropDownControl", _
                     "FMS.FMSComboBox" : Return True
                Case Else
                    If extendee.GetType.FullName.ToLower.Contains("FilterComboControl".ToLower) Or _
                       extendee.GetType.FullName.ToLower.Contains("SearchCtrl".ToLower) Then : Return True
                    Else
                        Return extendee.GetType.BaseType Is GetType(TextBox) Or _
                               extendee.GetType.BaseType Is GetType(ComboBox) Or _
                               extendee.GetType.BaseType Is GetType(DateTimePicker) Or _
                               extendee.GetType.BaseType Is GetType(NumericUpDown) Or _
                               extendee.GetType.BaseType Is GetType(RichTextBox) Or _
                               extendee.GetType.BaseType Is GetType(MaskedTextBox) Or _
                               extendee.GetType.BaseType.FullName.Trim.In("DevComponents.DotNetBar.Controls.TextBoxX", _
                                                                          "MetroFramework.Controls.MetroTextBox", _
                                                                          "DevComponents.DotNetBar.Controls.ComboBoxEx", _
                                                                          "DevComponents.Editors.DateTimeAdv.DateTimeInput", _
                                                                          "DevComponents.Editors.DoubleInput", _
                                                                          "DevComponents.Editors.IntegerInput", _
                                                                          "DevComponents.DotNetBar.Controls.ComboTree", _
                                                                          "System.Windows.Forms.TextBox", _
                                                                          "System.Windows.Forms.ComboBox", _
                                                                          "System.Windows.Forms.DateTimePicker", _
                                                                          "C1.Win.C1List.C1Combo", _
                                                                          "System.Windows.Forms.NumericUpDown", _
                                                                          "System.Windows.Forms.RichTextBox", _
                                                                          "System.Windows.Forms.MaskedTextBox", _
                                                                          "C1.Win.C1Input.C1DropDownControl", _
                                                                          "FMS.FMSComboBox")
                    End If

            End Select
        End Function

        ''' <summary>
        ''' Returns whether the specified control is marked as required using the RequiredFieldMarker.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ControlIsRequired(ByVal ctrl As Control) As Boolean
            Dim req As Boolean = False
            Dim rf As New RequiredFieldMarker
            With rf
                req = .IsRequired(ctrl) : .Dispose()
            End With

            Return req
        End Function

        ''' <summary>
        ''' True if enclosed control was marked as required, otherwise false.
        ''' </summary>
        ''' <param name="ctrl">Control to be evaluated.</param>
        ''' <returns>True or false</returns>
        ''' <remarks></remarks>
        Public Function IsRequired(ByVal ctrl As Control) As Boolean
            Dim req As Boolean = _ht.Contains(ctrl)

            If Not req And _
               ctrl IsNot Nothing Then
                If ctrl.Controls.Count > 0 Then
                    For Each c As Control In ctrl.Controls
                        If TypeOf c Is Label Then
                            req = (c.Name = "lbl_" & ctrl.Name)
                            If req Then Exit For
                        End If
                    Next
                End If
            End If

            Return req
        End Function

        ''' <summary>
        ''' Sets a control with a required field indicator.
        ''' </summary>
        ''' <param name="ctrl">Control to be mark with</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub SetAsRequired(ByVal ctrl As Control)
            Dim req As New RequiredFieldMarker
            req.SetRequired(ctrl, True)
            req.Dispose()
        End Sub

        ''' <summary>
        ''' Sets and attaches each of the specified controls with a rquired field indicator.
        ''' </summary>
        ''' <param name="ctrls"></param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub SetAsRequired(ByVal ParamArray ctrls() As Control)
            For Each ctrl As Control In ctrls
                SetAsRequired(ctrl)
            Next
        End Sub

        ''' <summary>
        ''' Sets or unsets a control with a required field indicator.
        ''' </summary>
        ''' <param name="ctrl">Control to be mark / unmark with</param>
        ''' <param name="required">Determines whether to place a mark or not</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub SetAsRequired(ByVal ctrl As Control, ByVal required As Boolean)
            Dim req As New RequiredFieldMarker
            req.SetRequired(ctrl, required)
            req.Dispose()
        End Sub

        ''' <summary>
        ''' Sets a control with a required field indicator in the specified position within the control's bounds..
        ''' </summary>
        ''' <param name="ctrl">Control to be mark with</param>
        ''' <param name="position">Position to place the mark</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub SetAsRequired(ByVal ctrl As Control, ByVal position As IndicatorPositionEnum)
            Dim req As New RequiredFieldMarker
            With req
                .IndicatorPosition = position
                .SetRequired(ctrl, True)
                .Dispose()
            End With
        End Sub

        ''' <summary>
        ''' Sets a control with a required field indicator in the specified position within the control's bounds..
        ''' </summary>
        ''' <param name="ctrl">Control to be mark with</param>
        ''' <param name="position">Position to place the mark</param>
        ''' <param name="color">Required field indicator's fill color</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub SetAsRequired(ByVal ctrl As Control, ByVal position As IndicatorPositionEnum, ByVal color As Color)
            Dim req As New RequiredFieldMarker
            With req
                .IndicatorPosition = position
                .SetRequired(ctrl, True)
                .SetRequiredIndicatorColor(ctrl, color)
                .Dispose()
            End With
        End Sub
#End Region

#Region "Exposed Routines"
        ''' <summary>
        ''' Updates 'required' marked control just in case there is a repainting of the marked control and
        ''' the indicator attached to it needs to be repositioned and redrawn also.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdateIndicator(ByVal ctrl As Control)
            If CanExtend(ctrl) Then
                If IsRequired(ctrl) Then PinIndicator(ctrl)
            End If
        End Sub

        ''' <summary>
        ''' Updates 'required' marked controls just in case there is a repainting of each marked control(s) and
        ''' the indicator(s) attached to it needs to be repositioned and redrawn also.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdateIndicators()
            For Each obj As Object In _ht.Keys
                If TypeOf obj Is Control Then PinIndicator(CType(obj, Control))
            Next
        End Sub
#End Region

#Region "Internal Functions and Routines"
        Private Overloads Function GetValue(ByVal propType As Object, ByVal propName As String) As Object
            Dim obj As Object = Nothing
            Try
                obj = propType.GetType.GetProperty(propName).GetValue(propType, Nothing)
            Catch ex As Exception

            End Try
            Return obj
        End Function

        Private Overloads Function GetValue(ByVal propType As Object, ByVal propName As String, ByVal propDefault As Object) As Object
            Dim obj As Object = propDefault

            Try
                obj = propType.GetType.GetProperty(propName).GetValue(propType, Nothing)
            Catch ex As Exception
            End Try

            Return obj
        End Function

        Private Overloads Sub PinIndicator(ByVal ctrl As Control)
            UnpinIndicator(ctrl)
            If ctrl.Visible Then
                Dim clr As Color = Color.FromArgb(213, 65, 44)

                If _ht.Contains(ctrl) Then
                    If _ht(ctrl) IsNot Nothing Then clr = _ht(ctrl)
                End If

                Dim lbl As New Label

                With lbl
                    .Name = "lbl_" & ctrl.Name
                    .Size = New Size(5, 5) : .BackColor = clr
                End With

                Try
                    If ctrl.Parent IsNot Nothing Then
                        If ctrl.FindForm IsNot Nothing Then
                            RemoveControlFrom(ctrl.FindForm.Controls, lbl.Name)
                            For Each c As Control In ctrl.FindForm.Controls
                                If c.Controls.Count > 0 Then RemoveControlFrom(c.Controls, lbl.Name)
                            Next
                        End If
                        RemoveControlFrom(ctrl.Parent.Controls, lbl.Name)
                        ctrl.Parent.Controls.Add(lbl)
                    Else
                        If ctrl.FindForm IsNot Nothing Then
                            RemoveControlFrom(ctrl.FindForm.Controls, lbl.Name)
                            For Each c As Control In ctrl.FindForm.Controls
                                If c.Controls.Count > 0 Then RemoveControlFrom(c.Controls, lbl.Name)
                            Next
                            ctrl.FindForm.Controls.Add(lbl)
                        Else
                            If ctrl.Controls.ContainsKey(lbl.Name) Then ctrl.Controls.RemoveByKey(lbl.Name)
                        End If
                    End If

                    With lbl
                        Select Case _indicatorposition
                            Case IndicatorPositionEnum.RigthTop
                                Dim xLoc As Integer = ctrl.Location.X + ctrl.Size.Width

                                If ctrl.GetType.FullName.Trim = "System.Windows.Forms.TextBox" Or _
                                    ctrl.GetType.FullName.Trim = "MetroFramework.Controls.MetroTextBox" Then
                                    Try
                                        If (GetValue(GetValue(ctrl, "ButtonCustom"), "Visible", False)) Or _
                                           (GetValue(GetValue(ctrl, "ButtonCustom2"), "Visible", False)) Then
                                            If (GetValue(GetValue(ctrl, "ButtonCustom"), "Visible", False)) And _
                                               (GetValue(GetValue(ctrl, "ButtonCustom2"), "Visible", False)) Then : .Location = New Point(xLoc - 38, ctrl.Top + 2)
                                            Else : .Location = New Point(xLoc - 21, ctrl.Top + 2)
                                            End If
                                        Else
                                            If (GetValue(ctrl, "Multiline", False)) And _
                                               (GetValue(ctrl, "ScrollBars", ScrollBars.None) = ScrollBars.Both Or _
                                                GetValue(ctrl, "ScrollBars", ScrollBars.None) = ScrollBars.Vertical) Then : .Location = New Point(xLoc - 21, ctrl.Top)
                                            Else : .Location = New Point(xLoc - 5, ctrl.Top)
                                            End If
                                        End If
                                    Catch ex As Exception
                                        .Location = New Point(xLoc - 5, ctrl.Top)
                                    End Try

                                ElseIf ctrl.GetType.FullName.Trim = "DevComponents.DotNetBar.Controls.TextBoxX" Then
                                    If GetValue(GetValue(ctrl, "ButtonCustom"), "Visible", False) Or _
                                       GetValue(GetValue(ctrl, "ButtonCustom2"), "Visible", False) Then
                                        If GetValue(GetValue(ctrl, "ButtonCustom"), "Visible", False) And _
                                           GetValue(GetValue(ctrl, "ButtonCustom2"), "Visible", False) Then : .Location = New Point(xLoc - 38, ctrl.Top + 2)
                                        Else : .Location = New Point(xLoc - 21, ctrl.Top + 2)
                                        End If
                                    Else
                                        If GetValue(ctrl, "Multiline", False) And _
                                          (GetValue(ctrl, "ScrollBars", ScrollBars.None) = ScrollBars.Both Or _
                                           GetValue(ctrl, "ScrollBars", ScrollBars.None) = ScrollBars.Vertical) Then : .Location = New Point(xLoc - 21, ctrl.Top)
                                        Else : .Location = New Point(xLoc - 5, ctrl.Top)
                                        End If
                                    End If

                                ElseIf ctrl.GetType.FullName.Trim = "DevComponents.Editors.DateTimeAdv.DateTimeInput" Then
                                    If GetValue(GetValue(ctrl, "ButtonDropDown"), "Visible", False) Then : .Location = New Point(xLoc - 21, ctrl.Top + 2)
                                    Else : .Location = New Point(xLoc - 5, ctrl.Top)
                                    End If

                                ElseIf ctrl.GetType.FullName.Trim = "DevComponents.Editors.DoubleInput" Then
                                    If GetValue(ctrl, "ShowUpDown", False) Then : .Location = New Point(xLoc - 21, ctrl.Top + 2)
                                    Else : .Location = New Point(xLoc - 5, ctrl.Top)
                                    End If

                                ElseIf ctrl.GetType.FullName.Trim = "DevComponents.Editors.IntegerInput" Then
                                    If GetValue(ctrl, "ShowUpDown", False) Then : .Location = New Point(xLoc - 21, ctrl.Top + 2)
                                    Else : .Location = New Point(xLoc - 5, ctrl.Top)
                                    End If

                                ElseIf ctrl.GetType.FullName.Trim = "DevComponents.DotNetBar.Controls.ComboTree" Or _
                                       ctrl.GetType.FullName.Trim = "DevComponents.DotNetBar.Controls.ComboBoxEx" Or _
                                       ctrl.GetType.FullName.Trim = "System.Windows.Forms.ComboBox" Then : .Location = New Point(xLoc - 21, ctrl.Top + 2)

                                ElseIf ctrl.GetType.FullName.Trim = "C1.Win.C1List.C1Combo" Then : .Location = New Point(xLoc - 24, ctrl.Top)
                                Else
                                End If

                            Case Else : .Location = ctrl.Location
                        End Select

                        .Visible = True : .Show() : .BringToFront()
                    End With
                Catch ex As Exception

                Finally

                End Try
            End If
        End Sub

        Private Overloads Sub PinIndicator(ByVal controls As Control.ControlCollection)
            For Each ctrl As Control In controls
                If CanExtend(ctrl) Then
                    If IsRequired(ctrl) Then : PinIndicator(ctrl)
                    Else : UnpinIndicator(ctrl)
                    End If
                Else : UnpinIndicator(ctrl)
                End If
                If ctrl.Controls.Count > 0 Then PinIndicator(ctrl.Controls)
            Next
        End Sub

        Private Overloads Sub PinIndicator(ByVal sender As Object, ByVal e As System.EventArgs)
            If TypeOf sender Is Control Then
                If IsRequired(CType(sender, Control)) Then : PinIndicator(CType(sender, Control))
                Else : UnpinIndicator(CType(sender, Control))
                End If
            End If
        End Sub

        Private Sub RemoveControlFrom(ByVal ctrls As Windows.Forms.Control.ControlCollection, ByVal ctrlName As String)
            For Each ctrl As Control In ctrls
                If ctrl.Name.ToLower = ctrlName.ToLower Then
                    ctrls.Remove(ctrl) : ctrls.Owner.Update()
                End If
            Next
        End Sub

        Private Sub UnpinIndicator(ByVal ctrl As Control)
            Dim lbl As New Label
            lbl.Name = "lbl_" & ctrl.Name

            Try
                If ctrl.Parent IsNot Nothing Then
                    If ctrl.FindForm IsNot Nothing Then
                        RemoveControlFrom(ctrl.FindForm.Controls, lbl.Name)
                        For Each c As Control In ctrl.FindForm.Controls
                            If c.Controls.Count > 0 Then RemoveControlFrom(c.Controls, lbl.Name)
                        Next
                    End If
                    RemoveControlFrom(ctrl.Parent.Controls, lbl.Name)
                Else
                    If ctrl.FindForm IsNot Nothing Then
                        RemoveControlFrom(ctrl.FindForm.Controls, lbl.Name)
                        For Each c As Control In ctrl.FindForm.Controls
                            If c.Controls.Count > 0 Then RemoveControlFrom(c.Controls, lbl.Name)
                        Next
                    End If
                End If
            Catch ex As Exception

            Finally

            End Try
        End Sub
#End Region

    End Class
End Namespace