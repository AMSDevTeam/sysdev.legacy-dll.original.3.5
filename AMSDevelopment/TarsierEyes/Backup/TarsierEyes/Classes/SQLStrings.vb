Namespace Common

    ''' <summary>
    ''' Common strings to SQL-qualified strings.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SQLStrings
        ''' <summary>
        ''' Converts numeric value at floating point to its SQL qualified string representation.
        ''' </summary>
        ''' <param name="NumericValue">Numeric value ro convert.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ToSqlValidString(ByVal NumericValue As Double) As String
            Return Format(NumericValue, "F2")
        End Function

        ''' <summary>
        ''' Converts numeric value at floating point to its SQL qualified string representation.
        ''' </summary>
        ''' <param name="NumericValue">Numeric value ro convert.</param>
        ''' <param name="DecimalPlaces">Number of decimal places.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ToSqlValidString(ByVal NumericValue As Double, ByVal DecimalPlaces As Integer) As String
            Return Format(NumericValue, "F" & DecimalPlaces.ToString)
        End Function

        ''' <summary>
        ''' Converts string value to its SQL qualified string representation.
        ''' </summary>
        ''' <param name="StringValue">String value to convert.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ToSqlValidString(ByVal StringValue As String) As String
            Return StringValue.Trim.Replace("'", "''").Replace("\", "\\")
        End Function

        ''' <summary>
        ''' Converts date value to its SQL qualified date-string representation.
        ''' </summary>
        ''' <param name="DateValue">Date value to convert.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ToSqlValidString(ByVal DateValue As Date) As String
            Return Format(DateValue, "yyyy-MM-dd")
        End Function

        ''' <summary>
        ''' Converts string value to its SQL qualified string representation.
        ''' </summary>
        ''' <param name="StringValue">String value to convert.</param>
        ''' <param name="IsRowFilter">Determines if output will be used as a DataTable RowFilter qualified string.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ToSqlValidString(ByVal StringValue As String, ByVal IsRowFilter As Boolean) As String
            Dim ValidString As String = String.Empty

            If IsRowFilter = True Then
                ValidString = StringValue.Trim.Replace("'", "''").Replace("\", "\\").Replace("[", "[[").Replace("]", "]]").Replace("[[", "[[]").Replace("]]", "[]]").Replace("*", "*]").Replace("*", "[*").Replace("%", "%]").Replace("%", "[%")
            Else
                ValidString = StringValue.Trim.Replace("'", "''").Replace("\", "\\")
            End If

            Return ValidString
        End Function

        ''' <summary>
        ''' Converts date value to its SQL qualified date-string representation.
        ''' </summary>
        ''' <param name="DateValue">Date value to convert.</param>
        ''' <param name="WithHours">Determnies if output string shall represent the time together with the date.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ToSqlValidString(ByVal DateValue As Date, ByVal WithHours As Boolean) As String
            Return Format(DateValue, "yyyy-MM-dd" & IIf(WithHours = True, " HH:mm:ss", ""))
        End Function
    End Class

End Namespace

