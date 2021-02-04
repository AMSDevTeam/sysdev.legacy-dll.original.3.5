Imports DevComponents.Editors.DateTimeAdv
Imports DevComponents.DotNetBar.Controls
Imports C1.Win.C1Input
Imports C1.Win.C1List
Imports DevComponents.Editors
Imports MetroFramework.Controls

Public Class Settings
    Shared _validators As New Components.ValidatorCollection

    ''' <summary>
    ''' Gets the list of the application's validator controls.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property Validators As Components.ValidatorCollection
        Get
            Return _validators
        End Get
    End Property

    Shared _connection As String
    Public Shared Property ConnectionString() As String
        Get
            Return _connection
        End Get
        Set(ByVal value As String)
            _connection = value
        End Set
    End Property


    Public Shared Sub MarkControlAsEdited(ByVal title As MetroFramework.Controls.MetroLabel)
        If Not title.Text.Trim.EndsWith("*") Then title.Text &= " *"
    End Sub

    Public Shared Sub ErrorMessenger(ByVal Message As String)

    End Sub

    Public Shared Sub BindControlsToTable(ByVal Table As DataTable, ByVal Container As Object)
        For Each ctrl As Object In Container.controls
            If IsNothing(ctrl.AccessibleName) Then Continue For
            If ctrl.AccessibleName <> "" And Table.Columns.Contains(ctrl.AccessibleName) Then
                Dim col As DataColumn = Table.Columns(ctrl.AccessibleName)
                If ctrl IsNot Nothing Then
                    With Table.Rows(0)
                        If col.DataType Is GetType(Date) Then
                            If IsDate(.Item(col.ColumnName)) Then
                                If TypeOf ctrl Is DateTimeInput Then : CType(ctrl, DateTimeInput).Value = CDate(.Item(col.ColumnName))
                                ElseIf TypeOf ctrl Is DateTimePicker Then : CType(ctrl, DateTimePicker).Value = CDate(.Item(col.ColumnName))
                                Else : ctrl.Text = Format(.Item(col.ColumnName), "yyyy-MM-dd")
                                End If
                            Else
                                If TypeOf ctrl Is DateTimeInput Then
                                    CType(ctrl, DateTimeInput).AllowEmptyState = True : CType(ctrl, DateTimeInput).IsEmpty = True
                                ElseIf TypeOf ctrl Is DateTimePicker Then : CType(ctrl, DateTimePicker).Value = #1/1/1900#
                                Else : ctrl.Text = String.Empty
                                End If
                            End If
                        Else
                            If col.DataType Is GetType(Int16) Or col.DataType Is GetType(Int32) Or col.DataType Is GetType(Int64) Or IsNumeric(.Item(col.ColumnName)) Then
                                If TypeOf ctrl Is CheckBoxX Then : CType(ctrl, CheckBoxX).Checked = CBool(.Item(col.ColumnName))
                                ElseIf TypeOf ctrl Is CheckBox Then : CType(ctrl, CheckBox).Checked = CBool(.Item(col.ColumnName))
                                ElseIf TypeOf ctrl Is ComboBox Or
                                       TypeOf ctrl Is ComboBoxEx Or
                                       TypeOf ctrl Is C1Combo Or
                                       ctrl.GetType().BaseType Is GetType(C1DropDownControl) Then
                                    Dim combo As Object = ctrl
                                    If combo.DataSource IsNot Nothing Then
                                        combo.SelectedValue = .Item(col.ColumnName)
                                        Dim dt As DataTable = combo.DataSource
                                        Dim _display As String = combo.DisplayMember
                                        If Not dt.Columns.Contains(_display) Then _display = dt.Columns(0).ColumnName
                                        Dim _value As String = combo.ValueMember
                                        If Not dt.Columns.Contains(_value) Then _value = dt.Columns(0).ColumnName
                                        If Not IsDBNull(.Item(col.ColumnName)) Then
                                            Dim dr() As DataRow = dt.Select("`" & _value & "` = " & .Item(col.ColumnName))
                                            If dr.Length > 0 Then combo.Text = dr(0).Item(_display)
                                        End If
                                    Else : ctrl.Text = .Item(col.ColumnName)
                                    End If
                                ElseIf TypeOf ctrl Is NumericUpDown Or
                                       TypeOf ctrl Is IntegerInput Or
                                       TypeOf ctrl Is DoubleInput Then
                                    Dim control As Object = ctrl
                                    control.Value = .Item(col.ColumnName)
                                Else
                                    Select Case ctrl.GetType.Name
                                        Case GetType(TextBoxX).Name
                                            If Table.Columns(col.ColumnName).MaxLength > -1 Then DirectCast(ctrl, TextBoxX).MaxLength = Table.Columns(col.ColumnName).MaxLength
                                        Case GetType(MetroTextBox).Name
                                            If Table.Columns.Contains(col.ColumnName) Then
                                                If Table.Columns(col.ColumnName).MaxLength > -1 Then DirectCast(ctrl, MetroTextBox).MaxLength = Table.Columns(col.ColumnName).MaxLength
                                            End If
                                    End Select

                                    If Not IsDBNull(.Item(col.ColumnName)) Then
                                        ctrl.Text = .Item(col.ColumnName)
                                    Else
                                        ctrl.Text = ""
                                    End If
                                End If
                            Else
                                If TypeOf ctrl Is PictureBox Then
                                    If Not IsDBNull(.Item(col.ColumnName)) Then CType(ctrl, PictureBox).Image = TarsierEyes.Common.Simple.ByteArrayToImage(.Item(col.ColumnName))

                                ElseIf TypeOf ctrl Is ComboBox Or
                                       TypeOf ctrl Is ComboBoxEx Or
                                       TypeOf ctrl Is C1Combo Or
                                       ctrl.GetType().BaseType Is GetType(C1DropDownControl) Then
                                    Dim combo As Object = ctrl
                                    If combo.DataSource IsNot Nothing Then : combo.SelectedValue = .Item(col.ColumnName)
                                    Else
                                        If Not IsDBNull(.Item(col.ColumnName)) Then
                                            ctrl.Text = .Item(col.ColumnName)
                                        Else
                                            ctrl.Text = ""
                                        End If
                                    End If
                                Else
                                    Select Case ctrl.GetType.Name
                                        Case GetType(TextBoxX).Name
                                            If Table.Columns(col.ColumnName).MaxLength > -1 Then DirectCast(ctrl, TextBoxX).MaxLength = Table.Columns(col.ColumnName).MaxLength
                                        Case GetType(MetroTextBox).Name
                                            If Table.Columns.Contains(col.ColumnName) Then
                                                If Table.Columns(col.ColumnName).MaxLength > -1 Then DirectCast(ctrl, MetroTextBox).MaxLength = Table.Columns(col.ColumnName).MaxLength
                                            End If
                                    End Select

                                    If Not IsDBNull(.Item(col.ColumnName)) Then
                                        ctrl.Text = .Item(col.ColumnName)
                                    Else
                                        ctrl.Text = ""
                                    End If
                                End If
                            End If
                        End If
                    End With
                End If
            End If
        Next
    End Sub
    Public Shared Sub SaveValueToTable(ByVal Row As DataRow, ByVal Container As Object)
        For Each ctrl As Object In Container.controls
            If IsNothing(ctrl.AccessibleName) Then Continue For

            If ctrl.AccessibleName <> "" And Row.Table.Columns.Contains(ctrl.AccessibleName) Then
                Dim col As DataColumn = Row.Table.Columns(ctrl.AccessibleName)
                If ctrl IsNot Nothing Then
                    With Row
                        If col.DataType Is GetType(Date) Then
                            If TypeOf ctrl Is DateTimeInput Then
                                If Not CType(ctrl, DateTimeInput).IsEmpty Then
                                    .Item(col.ColumnName) = Format(CType(ctrl, DateTimeInput).Value, "yyyy-MM-dd")
                                End If
                            ElseIf TypeOf ctrl Is DateTimePicker Then
                                If CType(ctrl, DateTimePicker).Value <> #1/1/1900# Then
                                    .Item(col.ColumnName) = Format(CType(ctrl, DateTimePicker).Value, "yyyy-MM-dd")
                                End If
                            Else
                                If IsDate(ctrl.Text) Then
                                    .Item(col.ColumnName) = Format(ctrl.Text, "yyyy-MM-dd")
                                End If
                            End If
                        Else
                            If col.DataType Is GetType(Int16) Or col.DataType Is GetType(Int32) Or col.DataType Is GetType(Int64) Or IsNumeric(.Item(col.ColumnName)) Then
                                If TypeOf ctrl Is CheckBoxX Then : CType(ctrl, CheckBoxX).Checked = CBool(.Item(col.ColumnName))
                                ElseIf TypeOf ctrl Is CheckBox Then : CType(ctrl, CheckBox).Checked = CBool(.Item(col.ColumnName))
                                ElseIf TypeOf ctrl Is ComboBox Or
                                       TypeOf ctrl Is ComboBoxEx Or
                                       TypeOf ctrl Is C1Combo Or
                                       ctrl.GetType().BaseType Is GetType(C1DropDownControl) Then
                                    Dim combo As Object = ctrl
                                    If combo.DataSource IsNot Nothing Then
                                        .Item(col.ColumnName) = combo.SelectedValue
                                    Else : .Item(col.ColumnName) = ctrl.Text
                                    End If
                                ElseIf TypeOf ctrl Is NumericUpDown Or
                                       TypeOf ctrl Is IntegerInput Or
                                       TypeOf ctrl Is DoubleInput Then
                                    Dim control As Object = ctrl
                                    .Item(col.ColumnName) = control.Value
                                Else
                                    .Item(col.ColumnName) = ctrl.Text
                                End If
                            Else
                                If TypeOf ctrl Is PictureBox Then
                                    .Item(col.ColumnName) = TarsierEyes.Common.Simple.ImageToByteArray(CType(ctrl, PictureBox).Image)
                                ElseIf TypeOf ctrl Is ComboBox Or
                                       TypeOf ctrl Is ComboBoxEx Or
                                       TypeOf ctrl Is C1Combo Or
                                       ctrl.GetType().BaseType Is GetType(C1DropDownControl) Then
                                    Dim combo As Object = ctrl
                                    If combo.DataSource IsNot Nothing Then
                                        .Item(col.ColumnName) = combo.SelectedValue
                                    Else
                                        .Item(col.ColumnName) = ctrl.Text
                                    End If
                                Else
                                    .Item(col.ColumnName) = ctrl.Text
                                End If
                            End If
                        End If
                    End With
                End If
            End If
        Next
    End Sub
End Class
