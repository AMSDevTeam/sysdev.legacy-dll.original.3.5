Namespace Common
    ''' <summary>
    ''' Class for converting numbers / amounts to its corresponding english word representations.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Amounts
        Implements IDisposable

#Region "Sub New"
        ''' <summary>
        ''' Creates a new instance of TarsierEyes.Common.Amounts.
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            _Amount = 0 : _Currency = "USD"
        End Sub

        ''' <summary>
        ''' Creates a new instance of TarsierEyes.Common.Amounts.
        ''' </summary>
        ''' <param name="amount">Amount to convert</param>
        ''' <remarks></remarks>
        Sub New(ByVal amount As Decimal)
            _Amount = amount : _Currency = "USD"
        End Sub

        ''' <summary>
        ''' Creates a new instance of TarsierEyes.Common.Amounts.
        ''' </summary>
        ''' <param name="amount">Amount to convert</param>
        ''' <param name="currency">Suffixing currency</param>
        ''' <remarks></remarks>
        Sub New(ByVal amount As Decimal, ByVal currency As String)
            _Amount = amount : _Currency = currency
        End Sub
#End Region

#Region "Properties"
        Dim _amount As Decimal = 0
        ''' <summary>
        ''' Gets or sets amount to be converted.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Amount() As Decimal
            Get
                Return _amount
            End Get
            Set(ByVal value As Decimal)
                _amount = value
            End Set
        End Property

        Dim _Currency As String = "USD"

        ''' <summary>
        ''' Gets or sets suffixing currency for the word representation.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Currency() As String
            Get
                Return _Currency
            End Get
            Set(ByVal value As String)
                _Currency = value
            End Set
        End Property
#End Region

#Region "Internal Functions"
        Private Function HundredsToWords(ByVal NumberToConvert As Integer) As String
            Dim sNumber As String = ""

            Select Case NumberToConvert \ 100
                Case 1 : sNumber &= "One"
                Case 2 : sNumber &= "Two"
                Case 3 : sNumber &= "Three"
                Case 4 : sNumber &= "Four"
                Case 5 : sNumber &= "Five"
                Case 6 : sNumber &= "Six"
                Case 7 : sNumber &= "Seven"
                Case 8 : sNumber &= "Eight"
                Case 9 : sNumber &= "Nine"
            End Select

            sNumber &= IIf(sNumber.Trim = String.Empty, "", " Hundred")

            Return sNumber
        End Function

        Private Function TensToWords(ByVal NumberToConvert As Integer) As String
            Dim sNumber As String = ""

            Select Case NumberToConvert \ 10
                Case 1
                    Select Case NumberToConvert
                        Case 10 : sNumber &= "Ten"
                        Case 11 : sNumber &= "Eleven"
                        Case 12 : sNumber &= "Twelve"
                        Case 13 : sNumber &= "Thirteen"
                        Case 14 : sNumber &= "Fourteen"
                        Case 15 : sNumber &= "Fifteen"
                        Case 16 : sNumber &= "Sixteen"
                        Case 17 : sNumber &= "Seventeen"
                        Case 18 : sNumber &= "Eighteen"
                        Case 19 : sNumber &= "Nineteen"
                    End Select

                Case 2 : sNumber &= "Twenty"
                Case 3 : sNumber &= "Thirty"
                Case 4 : sNumber &= "Fourty"
                Case 5 : sNumber &= "Fifty"
                Case 6 : sNumber &= "Sixty"
                Case 7 : sNumber &= "Seventy"
                Case 8 : sNumber &= "Eighty"
                Case 9 : sNumber &= "Ninety"
            End Select

            Return sNumber
        End Function

        Private Function OnesToWords(ByVal NumberToConvert As Integer) As String
            Dim sNumber As String = ""

            Select Case NumberToConvert
                Case 1 : sNumber &= "One"
                Case 2 : sNumber &= "Two"
                Case 3 : sNumber &= "Three"
                Case 4 : sNumber &= "Four"
                Case 5 : sNumber &= "Five"
                Case 6 : sNumber &= "Six"
                Case 7 : sNumber &= "Seven"
                Case 8 : sNumber &= "Eight"
                Case 9 : sNumber &= "Nine"
            End Select

            Return sNumber
        End Function

        Private Function NumberToWords(ByVal NumberToConvert As Double) As String
            Dim sNumber As String = IIf(NumberToConvert = 1000000000000000, "1000000000000000", NumberToConvert.ToString)
            Dim sWords As String = ""

            Dim DigitCount As Integer = sNumber.Length \ 3

            If (DigitCount > 16) Or (NumberToConvert > 1000000000000000) Then
                Return "Number was too long"
                Exit Function
            Else
                Dim sNumbers(DigitCount) As Char
                sNumbers = sNumber.ToCharArray

                Dim iSeries As Integer = 0

                For iCtr As Integer = 0 To sNumbers.Length - 1
                    iSeries = (sNumbers.Length - 1) - iCtr

                    Select Case iSeries
                        Case 15
                            sWords &= OnesToWords(CInt(sNumbers(iCtr).ToString)).Trim & " Quadrilion "

                        Case 14
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty
                            sWords = sWords.Trim & " " & HundredsToWords(CInt(sNumbers(iCtr).ToString & sNumbers(iCtr + 1).ToString & sNumbers(iCtr + 2).ToString)) & " "

                        Case 13
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty
                            sWords = sWords.Trim & " " & TensToWords(CInt(sNumbers(iCtr).ToString & sNumbers(iCtr + 1).ToString)).Trim & " "

                        Case 12
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty
                            If sNumber.Length = 13 Then
                                sWords = sWords.Trim & " " & OnesToWords(CInt(sNumbers(iCtr).ToString)).Trim & " Trillion "
                            Else
                                sWords = sWords.Trim & " " & IIf(CInt(sNumbers(iCtr - 1).ToString) <> 1, OnesToWords(CInt(sNumbers(iCtr).ToString)), "").ToString.Trim & " "
                                If sNumber.Length = 14 Then
                                    If CInt(sNumbers(iCtr - 1).ToString) <> 0 Then sWords = sWords.Trim & " " & "Trillion "
                                Else
                                    If CInt(sNumbers(iCtr - 2).ToString & sNumbers(iCtr - 1).ToString & sNumbers(iCtr).ToString) <> 0 Then sWords = sWords.Trim & " " & "Trillion "
                                End If
                            End If

                        Case 11
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty
                            sWords = sWords.Trim & " " & HundredsToWords(CInt(sNumbers(iCtr).ToString & sNumbers(iCtr + 1).ToString & sNumbers(iCtr + 2).ToString)).Trim & " "

                        Case 10
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty
                            sWords = sWords.Trim & " " & TensToWords(CInt(sNumbers(iCtr).ToString & sNumbers(iCtr + 1).ToString)).Trim & " "

                        Case 9
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty
                            If sNumber.Length = 10 Then
                                sWords = sWords.Trim & " " & OnesToWords(CInt(sNumbers(iCtr).ToString)).Trim & " Billion "
                            Else
                                sWords = sWords.Trim & " " & IIf(CInt(sNumbers(iCtr - 1).ToString) <> 1, OnesToWords(CInt(sNumbers(iCtr).ToString)), "") & " "
                                If sNumber.Length = 11 Then
                                    If CInt(sNumbers(iCtr - 1).ToString) <> 0 Then sWords = sWords.Trim & " " & "Billion "
                                Else
                                    If CInt(sNumbers(iCtr - 2).ToString & sNumbers(iCtr - 1).ToString & sNumbers(iCtr).ToString) <> 0 Then sWords = sWords.Trim & " " & "Billion "
                                End If
                            End If

                        Case 8
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty
                            sWords = sWords.Trim & " " & HundredsToWords(CInt(sNumbers(iCtr).ToString & sNumbers(iCtr + 1).ToString & sNumbers(iCtr + 2).ToString)).Trim & " "

                        Case 7
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty
                            sWords = sWords.Trim & " " & TensToWords(CInt(sNumbers(iCtr).ToString & sNumbers(iCtr + 1).ToString)).Trim & " "

                        Case 6
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty
                            If sNumber.Length = 7 Then
                                sWords = sWords.Trim & " " & OnesToWords(CInt(sNumbers(iCtr).ToString)).Trim & " Million "
                            Else
                                sWords = sWords.Trim & " " & IIf(CInt(sNumbers(iCtr - 1).ToString) <> 1, OnesToWords(CInt(sNumbers(iCtr).ToString)), "").ToString.Trim & " "
                                If sNumber.Length = 8 Then
                                    If CInt(sNumbers(iCtr - 1).ToString) <> 0 Then sWords = sWords.Trim & " " & "Million "
                                Else
                                    If CInt(sNumbers(iCtr - 2).ToString & sNumbers(iCtr - 1).ToString & sNumbers(iCtr).ToString) <> 0 Then sWords = sWords.Trim & " " & "Million "
                                End If
                            End If

                        Case 5
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty
                            sWords = sWords.Trim & " " & HundredsToWords(CInt(sNumbers(iCtr).ToString & sNumbers(iCtr + 1).ToString & sNumbers(iCtr + 2).ToString)).Trim & " "

                        Case 4
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty
                            sWords = sWords.Trim & " " & TensToWords(CInt(sNumbers(iCtr).ToString & sNumbers(iCtr + 1).ToString)).Trim & " "

                        Case 3
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty

                            If sNumber.Length = 4 Then
                                sWords = sWords.Trim & " " & OnesToWords(CInt(sNumbers(iCtr).ToString)).Trim & " Thousand "
                            Else
                                sWords = sWords.Trim & " " & IIf(CInt(sNumbers(iCtr - 1).ToString) <> 1, OnesToWords(CInt(sNumbers(iCtr).ToString)), "") & " "
                                If sNumber.Length = 5 Then
                                    If CInt(sNumbers(iCtr - 1).ToString) <> 0 Then sWords = sWords.Trim & " " & "Thousand "
                                Else
                                    If CInt(sNumbers(iCtr - 2).ToString & sNumbers(iCtr - 1).ToString & sNumbers(iCtr).ToString) <> 0 Then sWords = sWords.Trim & " " & "Thousand "
                                End If
                            End If

                        Case 2
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty
                            sWords = sWords.Trim & " " & HundredsToWords(CInt(sNumbers(iCtr).ToString & sNumbers(iCtr + 1).ToString & sNumbers(iCtr + 2).ToString)).Trim & " "

                        Case 1
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty
                            sWords = sWords.Trim & " " & TensToWords(CInt(sNumbers(iCtr).ToString & sNumbers(iCtr + 1).ToString)).Trim & " "

                        Case 0
                            sWords = sWords.Trim & " "
                            If String.IsNullOrEmpty(sWords.Trim) Then sWords = String.Empty

                            If sNumber.Length = 1 Then
                                sWords = sWords.Trim & " " & OnesToWords(CInt(sNumbers(iCtr).ToString)).Trim
                            Else
                                sWords = sWords.Trim & " " & IIf(CInt(sNumbers(iCtr - 1).ToString) <> 1, OnesToWords(CInt(sNumbers(iCtr).ToString)), "").ToString.Trim
                            End If

                    End Select
                Next
            End If

            sWords = LTrim(sWords).Trim

            Return sWords
        End Function

        ''' <summary>
        ''' Gets the english word representation of the specified amount.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String
            Dim sAmount As String = String.Empty

            sAmount = NumberToWords(_amount - (_amount Mod 1)) & IIf(CInt((_amount Mod 1) * 100) > 0, IIf(String.IsNullOrEmpty(NumberToWords(_amount - (_amount Mod 1))) = False, " and ", String.Empty) & CInt((_amount Mod 1) * 100) & " / 100", "")
            sAmount &= IIf(Not String.IsNullOrEmpty(sAmount.Trim), IIf(Not String.IsNullOrEmpty(_Currency.Trim), " " & _Currency, String.Empty), String.Empty)

            Return sAmount
        End Function
#End Region

#Region "Shared Functions"
        ''' <summary>
        ''' Calls the Amounts class' ToString overriden function to convert numeric value to its english word representation.
        ''' </summary>
        ''' <param name="amount">Amount to represent.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function AmountToWords(ByVal amount As Double) As String
            Dim aw As New Amounts(amount)
            Dim words As String = aw.ToString
            aw.Dispose()

            Return words
        End Function

        ''' <summary>
        ''' Calls the Amounts class' ToString overriden function to convert numeric value to its english word representation.
        ''' </summary>
        ''' <param name="amount">Amount to represent.</param>
        ''' <param name="currency">Suffixing currency.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function AmountToWords(ByVal amount As Double, ByVal currency As String) As String
            Dim aw As New Amounts(amount, currency)
            Dim words As String = aw.ToString
            aw.Dispose()

            Return words
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
                    Simple.RefreshAndManageCurrentProcess()
                    ' TODO: free other state (managed objects).
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

End Namespace

