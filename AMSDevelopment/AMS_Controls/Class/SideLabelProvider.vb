Imports System.ComponentModel
Imports System.Windows.Forms
Namespace Controls

    ''' <summary>
    ''' Control extender to provide input controls with side label.
    ''' </summary>
    ''' <remarks></remarks>
    <ProvideProperty("SideLabelText", GetType(Control)), ProvideProperty("SideLabelFont", GetType(Control)), _
     ProvideProperty("SideLabelForeColor", GetType(Control)), ProvideProperty("SideLabelBackColor", GetType(Control)), _
     ProvideProperty("SideLabelWidth", GetType(Control)), Description("Control extender to provide input controls with side label"), ToolboxBitmap(GetType(SideLabelProvider), "SideLabelProvider.bmp")> _
    Public Class SideLabelProvider
        Inherits Component
        Implements IExtenderProvider

#Region "Enumerations"
        ''' <summary>
        ''' Side label's custom styles.
        ''' </summary>
        ''' <remarks></remarks>
        Public Structure SideLabelStyle
            ''' <summary>
            ''' Background color.
            ''' </summary>
            ''' <remarks></remarks>
            Dim FillColor As Color
            ''' <summary>
            ''' Font face.
            ''' </summary>
            ''' <remarks></remarks>
            Dim Font As Font
            ''' <summary>
            ''' Font color.
            ''' </summary>
            ''' <remarks></remarks>
            Dim ForeColor As Color
            ''' <summary>
            ''' Display width.
            ''' </summary>
            ''' <remarks></remarks>
            Dim Width As Integer
        End Structure

        ''' <summary>
        ''' Side label's position within each of the associated controls.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum LabelPositionEnum
            ''' <summary>
            ''' Left side of the control.
            ''' </summary>
            ''' <remarks></remarks>
            LeftSide
            ''' <summary>
            ''' Right side of the control.
            ''' </summary>
            ''' <remarks></remarks>
            RightSide
        End Enum
#End Region

#Region "Structures"
        ''' <summary>
        ''' Side label features.
        ''' </summary>
        ''' <remarks></remarks>
        Public Structure SideLabel
            ''' <summary>
            ''' Associated text.
            ''' </summary>
            ''' <remarks></remarks>
            Dim Text As String
            ''' <summary>
            ''' Font color.
            ''' </summary>
            ''' <remarks></remarks>
            Dim ForeColor As Color
            ''' <summary>
            ''' Background color.
            ''' </summary>
            ''' <remarks></remarks>
            Dim BackColor As Color
            ''' <summary>
            ''' Font face.
            ''' </summary>
            ''' <remarks></remarks>
            Dim Font As Font
            ''' <summary>
            ''' Display width.
            ''' </summary>
            ''' <remarks></remarks>
            Dim Width As Integer
        End Structure
#End Region

#Region "Variable Declarations"
        Dim _ht As New Hashtable()
        Private _labelposition As LabelPositionEnum = LabelPositionEnum.LeftSide
#End Region

#Region "Sub New"
        ''' <summary>
        ''' Creates a new instance of TarsierEyes.Controls.SidelabelProvider.
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            _labelposition = LabelPositionEnum.LeftSide
        End Sub
#End Region

#Region "Properties"
        ''' <summary>
        ''' Gets side label's position for each of the assigned controls.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Gets or sets side label's position for each of the assigned controls."), _
            DefaultValue(GetType(LabelPositionEnum), "LeftSide")> _
        Public Property SideLabelPosition() As LabelPositionEnum
            Get
                Return _labelposition
            End Get
            Set(ByVal value As LabelPositionEnum)
                _labelposition = value
                UpdateSideLabels()
            End Set
        End Property

        ''' <summary>
        ''' Gets assigned side label's fill background color.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates assigned side label's fill background color."), _
            DefaultValue(GetType(Color), "DarkGreen")> _
        Public Function GetSideLabelBackColor(ByVal ctrl As Control) As Color
            If _ht.Contains(ctrl) Then
                Return CType(_ht(ctrl), SideLabel).BackColor
                Exit Function
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' Sets assigned side label's fill background color.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <param name="clr"></param>
        ''' <remarks></remarks>
        <Description("Indicates assigned side label's fill background color."), _
            DefaultValue(GetType(Color), "DarkGreen")> _
        Public Sub SetSideLabelBackColor(ByVal ctrl As Control, ByVal clr As Color)
            If CanExtend(ctrl) Then
                If _ht.Contains(ctrl) Then
                    Dim _sl As New SideLabel
                    With _sl
                        .BackColor = clr
                        .Font = _ht(ctrl).Font
                        .ForeColor = _ht(ctrl).ForeColor
                        .Text = _ht(ctrl).Text
                        .Width = _ht(ctrl).Width
                    End With

                    _ht.Remove(ctrl)

                    If Not String.IsNullOrEmpty(_sl.Text.Trim) Then
                        _ht.Add(ctrl, _sl)
                        AddHandler ctrl.SizeChanged, AddressOf PinSideLabel
                        AddHandler ctrl.LocationChanged, AddressOf PinSideLabel
                        AddHandler ctrl.ParentChanged, AddressOf PinSideLabel
                        PinSideLabel(ctrl)
                    End If
                End If
            Else
                If _ht.Contains(ctrl) Then _ht.Remove(ctrl)
                RemoveHandler ctrl.SizeChanged, AddressOf PinSideLabel
                RemoveHandler ctrl.LocationChanged, AddressOf PinSideLabel
                RemoveHandler ctrl.ParentChanged, AddressOf PinSideLabel
                UnpinSideLabel(ctrl)
            End If
        End Sub

        ''' <summary>
        ''' Gets assigned side label display font.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates assigned side label display font.")> _
        Public Function GetSideLabelFont(ByVal ctrl As Control) As Font
            If _ht.Contains(ctrl) Then
                Return CType(_ht(ctrl), SideLabel).Font : Exit Function
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' Sets assigned side label display font.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <param name="fnt"></param>
        ''' <remarks></remarks>
        <Description("Indicates assigned side label display font.")> _
        Public Sub SetSideLabelFont(ByVal ctrl As Control, ByVal fnt As Font)
            If CanExtend(ctrl) Then
                If _ht.Contains(ctrl) Then
                    Dim _sl As New SideLabel
                    With _sl
                        .BackColor = _ht(ctrl).BackColor
                        .Font = fnt
                        .ForeColor = _ht(ctrl).ForeColor
                        .Text = _ht(ctrl).Text
                        .Width = _ht(ctrl).Width
                    End With

                    _ht.Remove(ctrl)

                    If Not String.IsNullOrEmpty(_sl.Text.Trim) Then
                        _ht.Add(ctrl, _sl)
                        AddHandler ctrl.SizeChanged, AddressOf PinSideLabel
                        AddHandler ctrl.LocationChanged, AddressOf PinSideLabel
                        AddHandler ctrl.ParentChanged, AddressOf PinSideLabel
                        PinSideLabel(ctrl)
                    End If
                End If
            Else
                If _ht.Contains(ctrl) Then _ht.Remove(ctrl)
                RemoveHandler ctrl.SizeChanged, AddressOf PinSideLabel
                RemoveHandler ctrl.LocationChanged, AddressOf PinSideLabel
                RemoveHandler ctrl.ParentChanged, AddressOf PinSideLabel
                UnpinSideLabel(ctrl)
            End If
        End Sub

        ''' <summary>
        ''' Gets assigned side label's display text color.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates assigned side label's display text color."), _
         DefaultValue(GetType(Color), "White")> _
        Public Function GetSideLabelForeColor(ByVal ctrl As Control) As Color
            If _ht.Contains(ctrl) Then
                Return CType(_ht(ctrl), SideLabel).ForeColor : Exit Function
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' Sets assigned side label's display text color.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <param name="clr"></param>
        ''' <remarks></remarks>
        <Description("Indicates assigned side label's display text color."), _
         DefaultValue(GetType(Color), "White")> _
        Public Sub SetSideLabelForeColor(ByVal ctrl As Control, ByVal clr As Color)
            If CanExtend(ctrl) Then
                If _ht.Contains(ctrl) Then
                    Dim _sl As New SideLabel
                    With _sl
                        .BackColor = _ht(ctrl).BackColor
                        .Font = _ht(ctrl).Font
                        .ForeColor = clr
                        .Text = _ht(ctrl).Text
                        .Width = _ht(ctrl).Width

                    End With

                    _ht.Remove(ctrl)

                    If Not String.IsNullOrEmpty(_sl.Text.Trim) Then
                        _ht.Add(ctrl, _sl)
                        AddHandler ctrl.SizeChanged, AddressOf PinSideLabel
                        AddHandler ctrl.LocationChanged, AddressOf PinSideLabel
                        AddHandler ctrl.ParentChanged, AddressOf PinSideLabel
                        PinSideLabel(ctrl)
                    End If
                End If
            Else
                If _ht.Contains(ctrl) Then _ht.Remove(ctrl)
                RemoveHandler ctrl.SizeChanged, AddressOf PinSideLabel
                RemoveHandler ctrl.LocationChanged, AddressOf PinSideLabel
                RemoveHandler ctrl.ParentChanged, AddressOf PinSideLabel
                UnpinSideLabel(ctrl)
            End If
        End Sub

        ''' <summary>
        ''' Gets side label's text for the selected input control.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates side label's text for the selected input control.")> _
        Public Function GetSideLabelText(ByVal ctrl As Control) As String
            Dim sText As String = String.Empty

            If _ht.Contains(ctrl) Then sText = _ht(ctrl).Text

            Return sText
        End Function

        ''' <summary>
        ''' Sets side label's text for the selected input control.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <param name="text"></param>
        ''' <remarks></remarks>
        <Description("Indicates side label's text for the selected input control.")> _
        Public Sub SetSideLabelText(ByVal ctrl As Control, ByVal text As String)
            If CanExtend(ctrl) Then
                If String.IsNullOrEmpty(text.Trim) Then
                    If _ht.Contains(ctrl) Then _ht.Remove(ctrl)
                    RemoveHandler ctrl.SizeChanged, AddressOf PinSideLabel
                    RemoveHandler ctrl.LocationChanged, AddressOf PinSideLabel
                    RemoveHandler ctrl.ParentChanged, AddressOf PinSideLabel
                    UnpinSideLabel(ctrl)
                Else
                    If _ht.Contains(ctrl) Then
                        Dim _sl As New SideLabel
                        With _sl
                            .BackColor = _ht(ctrl).BackColor
                            .Font = _ht(ctrl).Font
                            .ForeColor = _ht(ctrl).ForeColor
                            .Text = text
                            .Width = _ht(ctrl).Width
                        End With

                        _ht.Remove(ctrl)

                        If Not String.IsNullOrEmpty(_sl.Text.Trim) Then
                            _ht.Add(ctrl, _sl)
                            AddHandler ctrl.SizeChanged, AddressOf PinSideLabel
                            AddHandler ctrl.LocationChanged, AddressOf PinSideLabel
                            AddHandler ctrl.ParentChanged, AddressOf PinSideLabel
                            PinSideLabel(ctrl)
                        End If
                    Else
                        Dim _sl As New SideLabel
                        With _sl
                            .BackColor = Color.DarkGreen
                            .Font = New Font(ctrl.Font.Name, ctrl.Font.Size, FontStyle.Bold)
                            .ForeColor = Color.White
                            .Text = text
                            .Width = 35
                        End With
                        _ht.Add(ctrl, _sl)
                    End If
                End If

                AddHandler ctrl.SizeChanged, AddressOf PinSideLabel
                AddHandler ctrl.LocationChanged, AddressOf PinSideLabel
                AddHandler ctrl.ParentChanged, AddressOf PinSideLabel
                PinSideLabel(ctrl)
            Else
                If _ht.Contains(ctrl) Then _ht.Remove(ctrl)
                RemoveHandler ctrl.SizeChanged, AddressOf PinSideLabel
                RemoveHandler ctrl.LocationChanged, AddressOf PinSideLabel
                RemoveHandler ctrl.ParentChanged, AddressOf PinSideLabel
                UnpinSideLabel(ctrl)
            End If
        End Sub

        ''' <summary>
        ''' Gets assigned side label's display width.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates assigned side label's display width."), _
         DefaultValue(GetType(Integer), "35")> _
        Public Function GetSideLabelWidth(ByVal ctrl As Control) As Integer
            If _ht.Contains(ctrl) Then
                Return CType(_ht(ctrl), SideLabel).Width : Exit Function
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' Sets assigned side label's display width.
        ''' </summary>
        ''' <param name="ctrl"></param>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        <Description("Indicates assigned side label's display width."), _
         DefaultValue(GetType(Integer), "35")> _
        Public Sub SetSideLabelWidth(ByVal ctrl As Control, ByVal value As Integer)
            If CanExtend(ctrl) Then
                If _ht.Contains(ctrl) Then
                    Dim _sl As New SideLabel
                    With _sl
                        .BackColor = _ht(ctrl).BackColor
                        .Font = _ht(ctrl).Font
                        .ForeColor = _ht(ctrl).ForeColor
                        .Text = _ht(ctrl).Text
                        .Width = value
                    End With

                    _ht.Remove(ctrl)

                    If Not String.IsNullOrEmpty(_sl.Text.Trim) Then
                        _ht.Add(ctrl, _sl)
                        AddHandler ctrl.SizeChanged, AddressOf PinSideLabel
                        AddHandler ctrl.LocationChanged, AddressOf PinSideLabel
                        AddHandler ctrl.ParentChanged, AddressOf PinSideLabel
                        PinSideLabel(ctrl)
                    End If
                End If
            Else
                If _ht.Contains(ctrl) Then _ht.Remove(ctrl)
                RemoveHandler ctrl.SizeChanged, AddressOf PinSideLabel
                RemoveHandler ctrl.LocationChanged, AddressOf PinSideLabel
                RemoveHandler ctrl.ParentChanged, AddressOf PinSideLabel
                UnpinSideLabel(ctrl)
            End If
        End Sub
#End Region

#Region "Exposed Functions"
        ''' <summary>
        ''' Validation if object was supported by side label.
        ''' </summary>
        ''' <param name="extendee">Control to evaluate.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CanExtend(ByVal extendee As Object) As Boolean Implements System.ComponentModel.IExtenderProvider.CanExtend
            Return (extendee.GetType.FullName.Trim = "DevComponents.DotNetBar.Controls.TextBoxX" Or _
                    extendee.GetType.FullName.Trim = "DevComponents.Editors.DoubleInput" Or _
                    extendee.GetType.FullName.Trim = "DevComponents.Editors.IntegerInput" Or _
                    extendee.GetType.FullName.Trim = "System.Windows.Forms.TextBox" Or _
                    extendee.GetType.FullName.Trim = "System.Windows.Forms.NumericUpDown" Or _
                    extendee.GetType.FullName.Trim = "System.Windows.Forms.MaskedTextBox" Or _
                    extendee.GetType.BaseType Is GetType(TextBox) Or _
                    extendee.GetType.BaseType Is GetType(ComboBox) Or _
                    extendee.GetType.BaseType Is GetType(DateTimePicker) Or _
                    extendee.GetType.BaseType Is GetType(NumericUpDown) Or _
                    extendee.GetType.BaseType Is GetType(RichTextBox) Or _
                    extendee.GetType.BaseType Is GetType(MaskedTextBox) Or _
                    extendee.GetType.BaseType Is GetType(MetroFramework.Controls.MetroTextBox))
        End Function
#End Region

#Region "Exposed Routines"
        ''' <summary>
        ''' Sets side label exclusively in runtime thru code.
        ''' </summary>
        ''' <param name="ctrl">Control to attach a side label.</param>
        ''' <param name="label">The side label's structure.</param>
        ''' <remarks></remarks>
        Public Sub SetSideLabel(ByVal ctrl As Control, ByVal label As SideLabel)
            If CanExtend(ctrl) Then
                If _ht.Contains(ctrl) Then _ht.Remove(ctrl)
                _ht.Add(ctrl, label)
            End If
        End Sub

        ''' <summary>
        ''' Updates control just in case there is a repainting of the host control and
        ''' the side label attached to it needs to be repositioned and redrawn also.
        ''' </summary>
        ''' <param name="ctrl">Control attached with side label.</param>
        ''' <remarks></remarks>
        Public Sub UpdateSideLabel(ByVal ctrl As Control)
            If CanExtend(ctrl) And _ht.Contains(ctrl) Then PinSideLabel(ctrl)
        End Sub

        ''' <summary>
        ''' Updates control(s) just in case there is a repainting of each hosted control(s) and
        ''' the side label(s) attached to it needs to be repositioned and redrawn also.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdateSideLabels()
            For Each obj As Object In _ht.Keys
                If TypeOf obj Is Control Then PinSideLabel(CType(obj, Control))
            Next
        End Sub

        ''' <summary>
        ''' Attaches a label with the specified caption at the specified side of the control
        ''' </summary>
        ''' <param name="ctrl">Control to attach a label with</param>
        ''' <param name="caption">Label's text caption</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub AttachSideLabel(ByVal ctrl As Control, ByVal caption As String)
            Dim s As New SideLabelProvider
            With s
                .SetSideLabelText(ctrl, caption)
                .Dispose()
            End With
        End Sub

        ''' <summary>
        ''' Attaches a label with the specified caption at the specified side of the control
        ''' </summary>
        ''' <param name="ctrl">Control to attach a label with</param>
        ''' <param name="caption">Label's text caption</param>
        ''' <param name="position">Label's position</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub AttachSideLabel(ByVal ctrl As Control, ByVal caption As String, ByVal position As LabelPositionEnum)
            Dim s As New SideLabelProvider
            With s
                .SideLabelPosition = position
                .SetSideLabelText(ctrl, caption)
                .Dispose()
            End With
        End Sub

        ''' <summary>
        ''' Attaches a label with the specified caption at the specified side of the control
        ''' </summary>
        ''' <param name="ctrl">Control to attach a label with</param>
        ''' <param name="caption">Label's text caption</param>
        ''' <param name="position">Label's position</param>
        ''' <param name="backcolor">Label's background color</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub AttachSideLabel(ByVal ctrl As Control, ByVal caption As String, ByVal position As LabelPositionEnum, ByVal backcolor As Color)
            Dim s As New SideLabelProvider
            With s
                .SideLabelPosition = position
                .SetSideLabelBackColor(ctrl, backcolor)
                .SetSideLabelText(ctrl, caption)
                .Dispose()
            End With
        End Sub

        ''' <summary>
        ''' Attaches a label with the specified caption at the specified side of the control
        ''' </summary>
        ''' <param name="ctrl">Control to attach a label with</param>
        ''' <param name="caption">Label's text caption</param>
        ''' <param name="position">Label's position</param>
        ''' <param name="backcolor">Label's background color</param>
        ''' <param name="forecolor">Label's forecolor</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub AttachSideLabel(ByVal ctrl As Control, ByVal caption As String, ByVal position As LabelPositionEnum, ByVal backcolor As Color, ByVal forecolor As Color)
            Dim s As New SideLabelProvider
            With s
                .SideLabelPosition = position
                .SetSideLabelBackColor(ctrl, backcolor)
                .SetSideLabelForeColor(ctrl, forecolor)
                .SetSideLabelText(ctrl, caption)
                .Dispose()
            End With
        End Sub

        ''' <summary>
        ''' Attaches a label with the specified caption at the specified side of the control
        ''' </summary>
        ''' <param name="ctrl">Control to attach a label with</param>
        ''' <param name="caption">Label's text caption</param>
        ''' <param name="position">Label's position</param>
        ''' <param name="backcolor">Label's background color</param>
        ''' <param name="forecolor">Label's forecolor</param>
        ''' <param name="font">Label's font face</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub AttachSideLabel(ByVal ctrl As Control, ByVal caption As String, ByVal position As LabelPositionEnum, ByVal backcolor As Color, ByVal forecolor As Color, ByVal font As Font)
            Dim s As New SideLabelProvider
            With s
                .SideLabelPosition = position
                .SetSideLabelFont(ctrl, font)
                .SetSideLabelBackColor(ctrl, backcolor)
                .SetSideLabelForeColor(ctrl, forecolor)
                .SetSideLabelText(ctrl, caption)
                .Dispose()
            End With
        End Sub

        ''' <summary>
        ''' Attaches a label with the specified caption at the specified side of the control
        ''' </summary>
        ''' <param name="ctrl">Control to attach a label with</param>
        ''' <param name="caption">Label's text caption</param>
        ''' <param name="position">Label's position</param>
        ''' <param name="backcolor">Label's background color</param>
        ''' <param name="forecolor">Label's forecolor</param>
        ''' <param name="font">Label's font face</param>
        ''' <param name="width">Label's width</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub AttachSideLabel(ByVal ctrl As Control, ByVal caption As String, ByVal position As LabelPositionEnum, ByVal backcolor As Color, ByVal forecolor As Color, ByVal font As Font, ByVal width As Integer)
            Dim s As New SideLabelProvider
            With s
                .SideLabelPosition = position
                .SetSideLabelFont(ctrl, font)
                .SetSideLabelBackColor(ctrl, backcolor)
                .SetSideLabelForeColor(ctrl, forecolor)
                .SetSideLabelText(ctrl, caption)
                .SetSideLabelWidth(ctrl, width)
                .Dispose()
            End With
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

        Private Overloads Sub PinSideLabel(ByVal ctrl As Control)
            Dim lbl As New Label

            With lbl
                .Name = "slbl_" & ctrl.Name
                .BackColor = Color.Transparent
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
                    End If
                End If

                With lbl
                    .Size = New Size(CType(_ht(ctrl), SideLabel).Width, ctrl.Height + 5)
                    .BackColor = CType(_ht(ctrl), SideLabel).BackColor
                    .Font = CType(_ht(ctrl), SideLabel).Font
                    .ForeColor = CType(_ht(ctrl), SideLabel).ForeColor
                    .Text = CType(_ht(ctrl), SideLabel).Text
                    .TextAlign = ContentAlignment.MiddleCenter
                    Dim xLoc As Integer = IIf(_labelposition = LabelPositionEnum.LeftSide, ctrl.Location.X - .Size.Width, ctrl.Location.X + ctrl.Width)

                    .Location = New Point(xLoc, ctrl.Top)
                    .Visible = True
                    .Show() : .BringToFront()
                End With
            Catch ex As Exception

            Finally

            End Try
        End Sub

        Private Overloads Sub PinSideLabel(ByVal sender As Object, ByVal e As System.EventArgs)
            If TypeOf sender Is Control Then
                If CanExtend(CType(sender, Control)) Then
                    If _ht.Contains(CType(sender, Control)) Then : PinSideLabel(CType(sender, Control))
                    Else : UnpinSideLabel(CType(sender, Control))
                    End If
                End If
            End If
        End Sub

        Private Sub RemoveControlFrom(ByVal ctrls As Windows.Forms.Control.ControlCollection, ByVal ctrlName As String)
            For Each ctrl As Control In ctrls
                If ctrl.Name.Contains(ctrlName) Then
                    ctrls.Remove(ctrl)
                    ctrls.Owner.Update()
                End If
            Next
        End Sub

        Private Sub UnpinSideLabel(ByVal ctrl As Control)
            Dim lbl As New Label
            lbl.Name = "slbl_" & ctrl.Name

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