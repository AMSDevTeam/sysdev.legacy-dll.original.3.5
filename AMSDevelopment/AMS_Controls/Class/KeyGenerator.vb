#Region "Imports"
Imports System.Text
#End Region

''' <summary>
''' Random key generator for script authorization.
''' </summary>
''' <remarks></remarks>
Public Class KeyGenerator

#Region "Properties"
    Dim _keyletters As String = String.Empty
    Dim _keynumbers As String = String.Empty
    Dim _keychars As Integer = 0

    ''' <summary>
    ''' Sets the pattern alphabets for the key.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property KeyLetters() As String
        Set(ByVal value As String)
            _keyletters = value
        End Set
    End Property

    ''' <summary>
    ''' Sets the pattern numeric values for the key.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property KeyNumbers() As String
        Set(ByVal value As String)
            _keynumbers = value
        End Set
    End Property

    ''' <summary>
    ''' Sets the number of key characters.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property KeyChars() As Integer
        Set(ByVal value As Integer)
            _keychars = value
        End Set
    End Property
#End Region

    ''' <summary>
    ''' Generates the key using the specified key patterns.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Generate() As String
        Dim sb As New StringBuilder
     
        Dim _lettersarray() As Char = _keyletters.ToCharArray
        Dim _numbersarray() As Char = _keynumbers.ToCharArray

        For ikey = 1 To _keychars
            Randomize()
            Dim random1 As Single = Rnd() : Dim arrIndex As Int16 = -1

            If (CType(random1 * 111, Integer)) Mod 2 = 0 Then
                Do While arrIndex < 0
                    arrIndex = _
                     Convert.ToInt16(_lettersarray.GetUpperBound(0) * random1)
                Loop

                Dim randomletter As String = _lettersarray(arrIndex)

                If (CType(arrIndex * random1 * 99, Integer)) Mod 2 <> 0 Then
                    randomletter = _lettersarray(arrIndex).ToString
                    randomletter = randomletter.ToUpper
                End If

                sb.Append(randomletter)
            Else
                Do While arrIndex < 0
                    arrIndex = _
                      Convert.ToInt16(_numbersarray.GetUpperBound(0) * random1)
                Loop
                sb.Append(_numbersarray(arrIndex))
            End If
        Next

        Return sb.ToString
    End Function
End Class
