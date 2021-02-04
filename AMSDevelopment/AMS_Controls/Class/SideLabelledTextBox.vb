Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Windows
Namespace Controls
    ''' <summary>
    ''' Customized text field with side label and required field marking features.
    ''' </summary>
    ''' <remarks></remarks>
    <ToolboxBitmap(GetType(SideLabelledTextBox), "SideLabelledTextBox.bmp")> _
    Public Class SideLabelledTextBox
        Inherits TextBox

#Region "Enumerations"
        ''' <summary>
        ''' Side label's side position.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum SideLabelPositionEnum
            ''' <summary>
            ''' Left side of the text box.
            ''' </summary>
            ''' <remarks></remarks>
            LeftSide
            ''' <summary>
            ''' Right side of the text box.
            ''' </summary>
            ''' <remarks></remarks>
            RightSide
        End Enum

        ''' <summary>
        ''' Required field indicator position in the text box.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum RequiredIndicatorPositionEnum
            ''' <summary>
            ''' Upper left corner of the text box.
            ''' </summary>
            ''' <remarks></remarks>
            LeftTop
            ''' <summary>
            ''' Upper right corner of the text box.
            ''' </summary>
            ''' <remarks></remarks>
            RightTop
        End Enum
#End Region

#Region "Variable Declarations"
        Dim _font As Font = Nothing
        Dim _sidelabeltext As String
        Dim _sidelabelposition As SideLabelPositionEnum
        Dim _sidelabelfont As Font
        Dim _sidesabelforecolor As Color
        Dim _sidelabelbackcolor As Color
        Dim _sidelabelwidth As Integer
        Dim _required As Boolean
        Dim _requiredindicatorcolor As Color
        Dim _requiredIndicatorPosition As RequiredIndicatorPositionEnum
        Dim _multiline As Boolean
        Dim _scrollBars As Forms.ScrollBars
        Dim _focushighlightcolor As Color
        Dim _focushighlightenabled As Boolean
        Dim _text As String

        Dim _watermarkenabled As Boolean
        Dim _watermarkfont As Font
        Dim _watermarkforecolor As Color
        Dim _watermarktext As String

        Dim lbl As New Label
        Dim txt As New TextBox
        Dim lblReq As New Label
#End Region

#Region "Events"
        Public Event ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
#End Region

#Region "Sub New"
        ''' <summary>
        ''' Creates a new instance of TarsierEyes.Controls.SideLabelledtextBox.
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()

            ' Add any initialization after the InitializeComponent() call.

            Font = New Font("Tahoma", 8, FontStyle.Regular)

            _focushighlightcolor = Color.FromArgb(255, 255, 136)
            _focushighlightenabled = False

            _required = False
            _requiredindicatorcolor = Color.OrangeRed
            _requiredIndicatorPosition = RequiredIndicatorPositionEnum.LeftTop

            _sidelabelfont = New Font("Tahoma", 8, FontStyle.Bold)
            _sidesabelforecolor = Color.FromKnownColor(KnownColor.Window)
            _sidelabelbackcolor = Color.DarkGreen
            _sidelabelposition = SideLabelPositionEnum.LeftSide
            _sidelabeltext = "USD" : _sidelabelwidth = 35

            _text = String.Empty

            _watermarkenabled = True
            _watermarkfont = New Font("Tahoma", 8, FontStyle.Regular)
            _watermarkforecolor = Color.FromKnownColor(KnownColor.GrayText)
            _watermarktext = String.Empty

            MyBase.BorderStyle = Forms.BorderStyle.FixedSingle
            JoinEvents(True)
        End Sub
#End Region

#Region "Properties"
        <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), _
         DefaultValue(GetType(BorderStyle), "FixedSingle")> _
        Public Overloads Property BorderStyle() As BorderStyle
            Get
                Return Forms.BorderStyle.FixedSingle
            End Get
            Set(ByVal value As BorderStyle)
                MyBase.BorderStyle = Forms.BorderStyle.FixedSingle
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets control background color when cursor is focusing in the control and FocusHighlightEnabled is set to True.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates control background color when cursor is focusing in the control and FocusHighlightEnabled is set to True."), _
         DefaultValue(GetType(Color), "255, 255, 136")> _
        Public Property FocusHighlightColor() As Color
            Get
                Return _focushighlightcolor
            End Get
            Set(ByVal value As Color)
                _focushighlightcolor = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether FocusHighlightColor would be the control background color when cursor is focusing in the control or not.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Determines whether FocusHighlightColor would be the control background color when cursor is focusing in the control or not."), _
         DefaultValue(GetType(Boolean), "False")> _
        Public Property FocusHighlightEnabled() As Boolean
            Get
                Return _focushighlightenabled
            End Get
            Set(ByVal value As Boolean)
                _focushighlightenabled = value
            End Set
        End Property

        <DefaultValue(GetType(Font), "Tahoma, 8.25pt")> _
        Public Overrides Property Font() As Font
            Get
                Return MyBase.Font
            End Get
            Set(ByVal value As Font)
                MyBase.Font = value

                For Each ctrl As Control In MyBase.Controls
                    If TypeOf ctrl Is TextBox And ctrl.Name.Contains("txt_") Then
                        ctrl.Font = value
                    End If
                Next

                If _multiline Then
                    For Each ctrl As Control In MyBase.Controls
                        If (TypeOf ctrl Is TextBox And ctrl.Name.Contains("txt_")) Or _
                           (TypeOf ctrl Is Label And ctrl.Name.Contains("lbl_")) Then ctrl.Size = New Size(ctrl.Size.Width, MyBase.Size.Height)
                    Next
                End If
            End Set
        End Property

        Public Overrides Property ForeColor() As Color
            Get
                Return MyBase.ForeColor
            End Get
            Set(ByVal value As Color)
                MyBase.ForeColor = value
                Dim txt As TextBox = Nothing

                For Each ctrl As Control In Controls
                    If ctrl.Name.Contains("txt_") Then
                        txt = ctrl : Exit For
                    End If
                Next

                If txt IsNot Nothing Then txt.ForeColor = value
            End Set
        End Property

        Public Overrides Property MaxLength() As Integer
            Get
                Return MyBase.MaxLength
            End Get
            Set(ByVal value As Integer)
                MyBase.MaxLength = value
                Dim txt As TextBox = Nothing

                For Each ctrl As Control In Controls
                    If ctrl.Name.Contains("txt_") Then
                        txt = ctrl : Exit For
                    End If
                Next

                If txt IsNot Nothing Then txt.MaxLength = value
            End Set
        End Property

        Public Overrides Property MultiLine() As Boolean
            Get
                Return _multiline
            End Get
            Set(ByVal value As Boolean)
                MyBase.Multiline = value
                _multiline = value
                Dim txt As TextBox = Nothing

                For Each ctrl As Control In Controls
                    If ctrl.Name.Contains("txt_") Then
                        txt = ctrl : Exit For
                    End If
                Next

                If txt IsNot Nothing Then txt.Multiline = value
            End Set
        End Property

        Dim _numericonly As Boolean = False

        ''' <summary>
        ''' Gets or sets whether control will just accept numeric values or not.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Determines whether control will just accept numeric values or not"), DefaultValue(GetType(Boolean), "False")> _
        Public Property NumericOnly() As Boolean
            Get
                Return _numericonly
            End Get
            Set(ByVal value As Boolean)
                _numericonly = value
                ShortcutsEnabled = Not value
                If Not IsNumeric(Text) Then Text = 0
                If value Then
                    TextAlign = HorizontalAlignment.Right

                    If Not String.IsNullOrEmpty(NumberFormat.Trim) Then
                        For Each ctrl As Control In MyBase.Controls
                            If (TypeOf ctrl Is TextBox And ctrl.Name.Contains("txt_")) Then
                                If Not IsNumeric(ctrl.Text) Then ctrl.Text = "0"
                                CType(ctrl, TextBox).TextAlign = HorizontalAlignment.Right
                                Dim contents As String = ctrl.Text
                                ctrl.Text = Format(CDbl(contents), NumberFormat)
                            End If
                        Next
                    End If

                Else : TextAlign = HorizontalAlignment.Left
                End If
            End Set
        End Property

        Dim _numberformat As String = String.Empty

        ''' <summary>
        ''' Gets or sets the numbering format when NumericOnly is set to true.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Determines the numbering format when NumericOnly is set to true."), DefaultValue(GetType(String), "")> _
        Public Property NumberFormat() As String
            Get
                Return _numberformat
            End Get
            Set(ByVal value As String)
                _numberformat = value
                If NumericOnly Then
                    For Each ctrl As Control In MyBase.Controls
                        If (TypeOf ctrl Is TextBox And ctrl.Name.Contains("txt_")) Then
                            If Not IsNumeric(ctrl.Text) Then ctrl.Text = "0"
                            CType(ctrl, TextBox).TextAlign = HorizontalAlignment.Right
                            Dim contents As String = ctrl.Text
                            ctrl.Text = Format(CDbl(contents), value)
                        End If
                    Next
                End If
            End Set
        End Property

        Public Overloads Property PasswordChar() As Char
            Get
                Return MyBase.PasswordChar
            End Get
            Set(ByVal value As Char)
                MyBase.PasswordChar = value
                Dim txt As TextBox = Nothing

                For Each ctrl As Control In Controls
                    If ctrl.Name.Contains("txt_") Then
                        txt = ctrl : Exit For
                    End If
                Next

                If txt IsNot Nothing Then txt.PasswordChar = value
            End Set
        End Property

        Public Overloads Property [ReadOnly]() As Boolean
            Get
                Return MyBase.ReadOnly
            End Get
            Set(ByVal value As Boolean)
                MyBase.ReadOnly = value
                Dim txt As TextBox = Nothing

                For Each ctrl As Control In Controls
                    If ctrl.Name.Contains("txt_") Then
                        txt = ctrl : Exit For
                    End If
                Next

                If txt IsNot Nothing Then txt.ReadOnly = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether the control is marked with a required indicator.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Determines whether the control is marked with a required indicator."), _
         DefaultValue(GetType(Boolean), "False")> _
        Public Property Required() As Boolean
            Get
                Return _required
            End Get
            Set(ByVal value As Boolean)
                _required = value
                UpdateRequiredIndicator()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets required indicator background color attached to this control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates required indicator background color attached to this control."), _
         DefaultValue(GetType(Color), "OrangeRed")> _
        Public Property RequiredIndicatorBackColor() As Color
            Get
                Return _requiredindicatorcolor
            End Get
            Set(ByVal value As Color)
                _requiredindicatorcolor = value
                UpdateRequiredIndicator()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets required indicator position within the control's bounds.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates required indicator position within the control's bounds."), _
         DefaultValue(GetType(RequiredIndicatorPositionEnum), "LeftTop")> _
        Public Property RequiredIndicatorPosition() As RequiredIndicatorPositionEnum
            Get
                Return _requiredIndicatorPosition
            End Get
            Set(ByVal value As RequiredIndicatorPositionEnum)
                _requiredIndicatorPosition = value
                UpdateRequiredIndicator()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets value indicating whether control's elements are aligned to support the locales using right-to-left fonts.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Property RightToLeft() As RightToLeft
            Get
                Return MyBase.RightToLeft
            End Get
            Set(ByVal value As RightToLeft)
                MyBase.RightToLeft = value
                Dim txt As TextBox = Nothing

                For Each ctrl As Control In Controls
                    If ctrl.Name.Contains("txt_") Then
                        txt = ctrl : Exit For
                    End If
                Next

                If txt IsNot Nothing Then txt.RightToLeft = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets which scrollbar will appear in a multiline textbox control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Property ScrollBars() As Forms.ScrollBars
            Get
                Return _scrollBars
            End Get
            Set(ByVal value As Forms.ScrollBars)
                _scrollBars = value
                MyBase.ScrollBars = value
                Dim txt As TextBox = Nothing

                For Each ctrl As Control In Controls
                    If ctrl.Name.Contains("txt_") Then
                        txt = ctrl : Exit For
                    End If
                Next

                If txt IsNot Nothing Then txt.ScrollBars = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets side label's background color for this color.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates side label's background color for this control."), _
         DefaultValue(GetType(Color), "DarkGreen")> _
        Public Property SideLabelBackColor() As Color
            Get
                Return _sidelabelbackcolor
            End Get
            Set(ByVal value As Color)
                _sidelabelbackcolor = value
                For Each ctrl As Control In MyBase.Controls
                    If TypeOf ctrl Is Label And _
                       ctrl.Name.Contains("lbl_") Then ctrl.BackColor = value
                Next
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets side label's font for this control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates side label's font for this control."), _
         DefaultValue(GetType(Font), "Tahoma, 8pt, style=Bold")> _
        Public Property SideLabelFont() As Font
            Get
                Return _sidelabelfont
            End Get
            Set(ByVal value As Font)
                _sidelabelfont = value
                For Each ctrl As Control In MyBase.Controls
                    If TypeOf ctrl Is Label And _
                       ctrl.Name.Contains("lbl_") Then ctrl.Font = value
                Next
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets side label's font forecolor for this control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates side label's font forecolor for this control."), _
         DefaultValue(GetType(Color), "Window")> _
        Public Property SideLabelForeColor() As Color
            Get
                Return _sidesabelforecolor
            End Get
            Set(ByVal value As Color)
                _sidesabelforecolor = value
                For Each ctrl As Control In MyBase.Controls
                    If TypeOf ctrl Is Label And _
                       ctrl.Name.Contains("lbl_") Then ctrl.ForeColor = value
                Next
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets side label's visibility position for this control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates side label's position attached to this textbox."), _
         DefaultValue(GetType(SideLabelPositionEnum), "LeftSide")> _
        Public Property SideLabelPosition() As SideLabelPositionEnum
            Get
                Return _sidelabelposition
            End Get
            Set(ByVal value As SideLabelPositionEnum)
                _sidelabelposition = value
                For Each ctrl As Control In MyBase.Controls
                    If TypeOf ctrl Is Label And _
                       ctrl.Name.Contains("lbl_") Then ctrl.Dock = IIf(value = SideLabelPositionEnum.RightSide, DockStyle.Right, DockStyle.Left)
                Next
                UpdateRequiredIndicator()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the text associated for the side label attached to this control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates the text associated for the side label attached to this control."), _
         DefaultValue("USD")> _
        Public Property SideLabelText() As String
            Get
                Return _sidelabeltext
            End Get
            Set(ByVal value As String)
                _sidelabeltext = value

                If MyBase.Controls.ContainsKey("lbl_" & MyBase.Name.ToLower) Then MyBase.Controls.RemoveByKey("lbl_" & MyBase.Name.ToLower)
                If MyBase.Controls.ContainsKey("txt_" & MyBase.Name.ToLower) Then MyBase.Controls.RemoveByKey("txt_" & MyBase.Name.ToLower)

                With lbl
                    .BackColor = _sidelabelbackcolor
                    .Text = _sidelabeltext : .Font = _sidelabelfont : .ForeColor = _sidesabelforecolor
                    .Size = New Point(_sidelabelwidth, MyBase.Height)
                    .Name = "lbl_" & MyBase.Name.ToLower
                    .Dock = IIf(_sidelabelposition = SideLabelPositionEnum.RightSide, DockStyle.Right, DockStyle.Left)
                    .TextAlign = ContentAlignment.MiddleCenter
                    .Visible = True
                End With

                Dim txt As New TextBox
                With txt
                    AddHandler txt.Click, AddressOf txt_Click
                    AddHandler txt.CursorChanged, AddressOf txt_CursorChanged
                    AddHandler txt.GotFocus, AddressOf txt_GotFocus
                    AddHandler txt.KeyPress, AddressOf txt_KeyPress
                    AddHandler txt.LostFocus, AddressOf txt_LostFocus
                    AddHandler txt.TextChanged, AddressOf txt_TextChanged
                    .AutoCompleteCustomSource = MyBase.AutoCompleteCustomSource
                    .AutoCompleteMode = MyBase.AutoCompleteMode : .AutoCompleteSource = MyBase.AutoCompleteSource
                    .Size = New Point(MyBase.Width - _sidelabelwidth, MyBase.Height)
                    .Name = "txt_" & MyBase.Name.ToLower : .Font = MyBase.Font
                    .BorderStyle = Forms.BorderStyle.FixedSingle : .MaxLength = MyBase.MaxLength
                    .Multiline = _multiline : .ScrollBars = Forms.ScrollBars.None
                    .Dock = DockStyle.Fill : .Text = MyBase.Text
                    If NumericOnly And _
                       Not IsNumeric(NumberFormat.Trim) Then
                        If Not IsNumeric(.Text) Then .Text = "0"
                        Dim contents As String = .Text
                        .Text = Format(CDbl(contents), NumberFormat)
                    End If
                    .ReadOnly = MyBase.ReadOnly : .TextAlign = MyBase.TextAlign
                    .PasswordChar = MyBase.PasswordChar : .ShortcutsEnabled = Not NumericOnly
                    .Visible = True
                End With

                MyBase.Controls.RemoveByKey("lbl_" & MyBase.Name.ToLower)
                MyBase.Controls.RemoveByKey("txt_" & MyBase.Name.ToLower)

                If Not String.IsNullOrEmpty(_sidelabeltext) Then
                    MyBase.Controls.Add(lbl)
                    With txt
                        MyBase.Controls.Add(txt) : .Font = MyBase.Font
                        .Text = MyBase.Text : .BringToFront()
                        If NumericOnly And _
                           Not IsNumeric(NumberFormat.Trim) Then
                            If Not IsNumeric(.Text) Then .Text = "0"
                            Dim contents As String = .Text
                            .Text = Format(CDbl(contents), NumberFormat)
                        End If
                    End With
                End If

                UpdateRequiredIndicator()
                With Me
                    .Refresh() : .Update()
                End With
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets side label's display width attached to this control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates side label's display width attached to this control."), _
         DefaultValue(GetType(Integer), "35")> _
        Public Property SideLabelWidth() As Integer
            Get
                Return _sidelabelwidth
            End Get
            Set(ByVal value As Integer)
                _sidelabelwidth = value
                If MyBase.Controls.ContainsKey("lbl_" & MyBase.Name.ToLower) Then MyBase.Controls.Item("lbl_" & MyBase.Name.ToLower).Width = value
                UpdateRequiredIndicator()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the current text for the control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <EditorBrowsable(EditorBrowsableState.Always), Browsable(True)> _
        Public Overrides Property Text() As String
            Get
                Return MyBase.Text
            End Get
            Set(ByVal value As String)
                MyBase.Text = value
                Dim txt As TextBox = Nothing

                For Each ctrl As Control In Controls
                    If ctrl.Name.Contains("txt_") Then
                        txt = ctrl : Exit For
                    End If
                Next

                If txt IsNot Nothing Then
                    With txt
                        .Text = value : .Font = MyBase.Font
                        If NumericOnly And _
                           Not String.IsNullOrEmpty(NumberFormat.Trim) Then
                            If Not IsNumeric(.Text) Then .Text = "0"
                            Dim contents As String = .Text
                            .Text = Format(CDbl(contents), NumberFormat)
                        End If
                    End With
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets how text is aligned in the control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Property TextAlign() As Forms.HorizontalAlignment
            Get
                Return MyBase.TextAlign
            End Get
            Set(ByVal value As Forms.HorizontalAlignment)
                MyBase.TextAlign = value
                Dim txt As TextBox = Nothing

                For Each ctrl As Control In Controls
                    If ctrl.Name.Contains("txt_") Then
                        txt = ctrl : Exit For
                    End If
                Next

                If txt IsNot Nothing Then txt.TextAlign = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether watermark text will associate the control when control text is empty.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Determines whether watermark text will associate the control when control text is empty."), _
         DefaultValue(GetType(Boolean), "True"), Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
        Public Property WatermarkEnabled() As Boolean
            Get
                Return _watermarkenabled
            End Get
            Set(ByVal value As Boolean)
                _watermarkenabled = value
                'If MyBase.Controls.ContainsKey("txt_" & MyBase.Name.ToLower) Then CType(MyBase.Controls("txt_" & MyBase.Name.ToLower), TextBox).WatermarkEnabled = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets watermark text display font for the edit control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates watermark text display font for the edit control."), _
         DefaultValue(GetType(Font), "Tahoma, 8pt"), Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
        Public Property WatermarkFont() As Font
            Get
                Return _watermarkfont
            End Get
            Set(ByVal value As Font)
                _watermarkfont = value
                'If MyBase.Controls.ContainsKey("txt_" & MyBase.Name.ToLower) Then CType(MyBase.Controls("txt_" & MyBase.Name.ToLower), TextBox).WatermarkFont = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets watemark text color for the edit control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("Indicates watemark text color for the edit control."), _
         DefaultValue(GetType(Color), "GrayText"), Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
        Public Property WatermarkForeColor() As Color
            Get
                Return _watermarkforecolor
            End Get
            Set(ByVal value As Color)
                _watermarkforecolor = value
                'If MyBase.Controls.ContainsKey("txt_" & MyBase.Name.ToLower) Then CType(MyBase.Controls("txt_" & MyBase.Name.ToLower), TextBox).WatermarkForeColor = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets watermark text associated with the edit control when the control is empty.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Description("The watermark text associated with the edit control when the control is empty."), Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
        Public Property WatermarkText() As String
            Get
                Return _watermarktext
            End Get
            Set(ByVal value As String)
                _watermarktext = value
                'If MyBase.Controls.ContainsKey("txt_" & MyBase.Name.ToLower) Then CType(MyBase.Controls("txt_" & MyBase.Name.ToLower), TextBox).WatermarkText = value
                Me.Invalidate()
            End Set
        End Property
#End Region

#Region "Exposed Routines"
        ''' <summary>
        ''' Updates required field indicator position if there is a need to relocate or there is a 
        ''' need to repaint the host control of the required indicator.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdateRequiredIndicator()
            If MyBase.Parent IsNot Nothing Then
                MyBase.Parent.Controls.RemoveByKey("lblReq_" & MyBase.Name)
            Else
                If MyBase.FindForm IsNot Nothing Then MyBase.FindForm.Controls.RemoveByKey("lblReq_" & MyBase.Name)
            End If

            If _required Then
                With lblReq
                    .Name = "lblReq_" & MyBase.Name
                    .BackColor = _requiredindicatorcolor
                    Dim xLoc As Integer = IIf(_requiredIndicatorPosition = RequiredIndicatorPositionEnum.RightTop, (MyBase.Location.X + MyBase.Size.Width) - 5, MyBase.Location.X)
                    If Not String.IsNullOrEmpty(_sidelabeltext.Trim) Then
                        Select Case _sidelabelposition
                            Case SideLabelPositionEnum.RightSide
                                If _requiredIndicatorPosition = RequiredIndicatorPositionEnum.RightTop Then xLoc -= _sidelabelwidth
                            Case SideLabelPositionEnum.LeftSide
                                If _requiredIndicatorPosition = RequiredIndicatorPositionEnum.LeftTop Then xLoc += _sidelabelwidth + 1
                        End Select
                    End If
                    .Size = New Size(5, 5)
                    .Location = New Point(xLoc - IIf(MultiLine And _requiredIndicatorPosition = RequiredIndicatorPositionEnum.RightTop, 17, 0), MyBase.Location.Y)

                    If MyBase.Parent IsNot Nothing Then : MyBase.Parent.Controls.Add(lblReq)
                    Else
                        If MyBase.FindForm IsNot Nothing Then MyBase.FindForm.Controls.Add(lblReq)
                    End If
                    .Visible = True : .BringToFront()
                End With
            End If
        End Sub
#End Region

#Region "Base Events"
        Const _numerickeys As String = "0123456789.-"
        Dim _editingflag As Boolean = False

        Private Sub SideLabelledTextBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
            If (Not MyBase.ReadOnly) And _
                MyBase.Enabled Then
                If _focushighlightenabled Then MyBase.BackColor = _focushighlightcolor
            End If
        End Sub

        Private Sub SideLabelledTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
            If e.KeyChar <> Chr(Keys.Back) Then
                If NumericOnly Then
                    If Not _numerickeys.Contains(e.KeyChar.ToString) Then : e.KeyChar = String.Empty
                    Else
                        If Not Char.IsNumber(e.KeyChar) Then
                            If e.KeyChar.ToString = "-" Then
                                If Not String.IsNullOrEmpty(Text.Trim) Then e.KeyChar = String.Empty
                            Else
                                If Text.Contains(e.KeyChar.ToString) Then e.KeyChar = String.Empty
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        Private Sub SideLabelledTextBox_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LocationChanged
            UpdateRequiredIndicator()
        End Sub

        Private Sub SideLabelledTextBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
            If (Not MyBase.ReadOnly) And _
               MyBase.Enabled Then MyBase.BackColor = Color.FromKnownColor(KnownColor.Window)

            If NumericOnly Then
                If Not String.IsNullOrEmpty(NumberFormat.Trim) And _
                   IsNumeric(Text) Then
                    Dim content As String = Text
                    Text = Format(CDbl(content), NumberFormat)
                End If
            End If
        End Sub


        Private Sub SideLabelledTextBox_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
            For Each ctrl As Control In MyBase.Controls
                If (TypeOf ctrl Is TextBox And ctrl.Name.Contains("txt_")) Or _
                   (TypeOf ctrl Is Label And ctrl.Name.Contains("lbl_")) Or _
                   (TypeOf ctrl Is Label And ctrl.Name.Contains("btnBrowse_")) Then ctrl.Size = New Size(ctrl.Size.Width, MyBase.Size.Height)
            Next
            UpdateRequiredIndicator()
        End Sub

        Private Sub SideLabelledTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.TextChanged
            If NumericOnly And _editingflag Then
                _editingflag = False
                If Not IsNumeric(Text.Trim) And _
                   Not String.IsNullOrEmpty(Text.Trim) Then
                    Dim chars() As Char = Text.Trim.ToCharArray
                    Dim newtext As String = String.Empty

                    For Each c As Char In chars
                        If Char.IsNumber(c) Then newtext &= c.ToString
                    Next

                    If String.IsNullOrEmpty(newtext.Trim) Then : Text = "0"
                    Else : Text = newtext
                    End If
                End If

                If Not String.IsNullOrEmpty(NumberFormat.Trim) And _
                   IsNumeric(Text) Then
                    Dim content As String = Text
                    Text = Format(CDbl(content), NumberFormat)
                End If
                _editingflag = True
            End If
        End Sub

        Private Sub SideLabelledTextBox_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
            If (Not String.IsNullOrEmpty(_sidelabeltext.Trim)) And _
               MyBase.Controls.Count <= 0 Then SideLabelText = _sidelabeltext
            UpdateRequiredIndicator()
        End Sub
#End Region

#Region "Embedded Events"
        Private Sub btnBrowse_ButtonClick(ByVal sender As Object, ByVal e As System.EventArgs)
            RaiseEvent ButtonClick(sender, e)
        End Sub

        Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            If TypeOf sender Is TextBox Then
                MyBase.SelectionStart = CType(sender, TextBox).SelectionStart
                MyBase.ScrollToCaret()
                MyBase.OnClick(e)
            End If
        End Sub

        Private Sub txt_CursorChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            If TypeOf sender Is TextBox Then
                MyBase.SelectionStart = CType(sender, TextBox).SelectionStart
                MyBase.ScrollToCaret()
                MyBase.OnCursorChanged(e)
            End If
        End Sub

        Private Sub txt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
            If TypeOf sender Is TextBox Then
                With CType(sender, TextBox)
                    If (Not .ReadOnly) And _
                        .Enabled Then
                        If _focushighlightenabled Then .BackColor = _focushighlightcolor
                    End If
                End With
                MyBase.OnGotFocus(e)
            End If
        End Sub

        Private Sub txt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
            If e.KeyChar <> Chr(Keys.Back) Then
                If NumericOnly Then
                    If Not _numerickeys.Contains(e.KeyChar.ToString) Then : e.KeyChar = String.Empty
                    Else
                        If Not Char.IsNumber(e.KeyChar) Then
                            If e.KeyChar.ToString = "-" Then
                                If Not String.IsNullOrEmpty(sender.Text.Trim) Then e.KeyChar = String.Empty
                            Else
                                If sender.Text.Contains(e.KeyChar.ToString) Then e.KeyChar = String.Empty
                            End If
                        End If
                    End If
                End If
            End If

            MyBase.OnKeyPress(e)
        End Sub

        Private Sub txt_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
            If TypeOf sender Is TextBox Then
                If NumericOnly Then
                    If Not String.IsNullOrEmpty(NumberFormat.Trim) And _
                       IsNumeric(sender.Text) Then
                        Dim content As String = sender.Text
                        sender.Text = Format(CDbl(content), NumberFormat)
                    End If
                End If

                MyBase.OnLostFocus(e)
            End If
        End Sub

        Private Sub txt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            If NumericOnly And _editingflag Then
                _editingflag = False
                If Not IsNumeric(sender.Text.Trim) And _
                   Not String.IsNullOrEmpty(sender.Text.Trim) Then
                    Dim chars() As Char = sender.Text.Trim.ToCharArray
                    Dim newtext As String = String.Empty

                    For Each c As Char In chars
                        If Char.IsNumber(c) Then newtext &= c.ToString
                    Next

                    If String.IsNullOrEmpty(newtext.Trim) Then : sender.Text = "0"
                    Else : sender.Text = newtext
                    End If
                End If

                If Not String.IsNullOrEmpty(NumberFormat.Trim) And _
                   IsNumeric(sender.Text) Then
                    Dim content As String = sender.Text
                    sender.Text = Format(CDbl(content), NumberFormat)
                End If

                _editingflag = True
            End If

            MyBase.Text = sender.Text
        End Sub
#End Region

#Region "Watermarking"
        Private Sub JoinEvents(ByVal join As Boolean)
            If join Then
                AddHandler (TextChanged), AddressOf WaterMark_Toggle
                AddHandler (LostFocus), AddressOf WaterMark_Toggle
                AddHandler (GotFocus), AddressOf WaterMark_Toggle
                AddHandler (FontChanged), AddressOf WaterMark_FontChanged
            End If
        End Sub

        Private Sub WaterMark_Toggle(ByVal sender As Object, ByVal args As EventArgs)
            If (Me.Text.Length <= 0) And _
               (Not Me.Focused) Then : EnableWaterMark()
            Else : DisableWaterMark()
            End If
        End Sub

        Private Sub WaterMark_FontChanged(ByVal sender As Object, ByVal args As EventArgs)
            If _watermarkenabled Then
                _font = New Font(Font.FontFamily, Font.Size, Font.Style, Font.Unit)
                Refresh()
            End If
        End Sub

        Private Sub EnableWaterMark()
            _font = New Font(Font.FontFamily, Font.Size, Font.Style, Font.Unit)
            Me.SetStyle(ControlStyles.UserPaint, True)
            Refresh()
        End Sub

        Private Sub DisableWaterMark()
            Me.SetStyle(ControlStyles.UserPaint, False)
            If Not _font Is Nothing Then
                Me.Font = New Font(Font.FontFamily, Font.Size, Font.Style, Font.Unit)
            End If
        End Sub

        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()
            WaterMark_Toggle(Nothing, Nothing)
        End Sub

        Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
            If Not (String.IsNullOrEmpty(_watermarktext.Trim)) And _
               _watermarkenabled Then
                Dim drawBrush As SolidBrush = New SolidBrush(_watermarkforecolor)
                e.Graphics.DrawString(_watermarktext, _watermarkfont, drawBrush, New Point(0, 0))
            End If

            Dim rc As New Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y, MyBase.Size.Width, MyBase.Size.Height - 1)
            e.Graphics.DrawRectangle(Pens.DimGray, rc)
            MyBase.OnPaint(e)
        End Sub
#End Region

    End Class
End Namespace
