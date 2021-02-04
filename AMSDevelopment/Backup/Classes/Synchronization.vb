Namespace Common
    ''' <summary>
    ''' Class for method and function synchronization.
    ''' </summary>
    ''' <remarks></remarks>
    Partial Public Class Synchronization
        ''' <summary>
        ''' Waits for the System.Threading.Thread's process to be completed.
        ''' </summary>
        ''' <param name="thread">Thread to wait for.</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub WaitToFinish(ByVal thread As Thread)
            WaitToFinish(thread, Nothing)
        End Sub

        Private Shared Function AreFinished(ByVal syncs() As Object) As Boolean
            Dim fins(syncs.Length - 1) As Boolean : Dim started As Boolean = False
            Dim fin As Boolean = False

            While Not fin
                If Not started Then
                    fin = True : started = True
                End If

                For i As Integer = 0 To syncs.Length - 1
                    fins(i) = True
                    If TypeOf syncs(i) Is IAsyncResult Then : fins(i) = fins(i) And CType(syncs(i), IAsyncResult).IsCompleted
                    ElseIf TypeOf syncs(i) Is Thread Then : fins(i) = fins(i) And (Not CType(syncs(i), Thread).IsAlive)
                    Else : fins(i) = fins(i) And True
                    End If
                Next

                fin = True
                For i As Integer = 0 To syncs.Length - 1
                    fin = fin And fins(i)
                Next
            End While

            Return fin
        End Function

        ''' <summary>
        ''' Waits all of specified asynhronizations to be completed.
        ''' </summary>
        ''' <param name="syncs"></param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub WaitToFinish(ByVal ParamArray syncs() As Object)
            Dim fin As Boolean = False
            While Not fin
                If syncs.Length > 0 Then
                    Dim delToFin As New Func(Of Object(), Boolean)(AddressOf AreFinished)
                    Dim arToFin As IAsyncResult = delToFin.BeginInvoke(syncs, Nothing, delToFin)
                    WaitToFinish(arToFin) : fin = delToFin.EndInvoke(arToFin)
                Else : fin = True
                End If
            End While
        End Sub

        ''' <summary>
        ''' Waits for the System.Threading.Thread's process to be completed.
        ''' </summary>
        ''' <param name="thread">Thread to wait for.</param>
        ''' <param name="progressbar">Progress bar object to show the current running time of the thread.</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub WaitToFinish(ByVal thread As Thread, ByVal progressbar As Object)
            While thread.IsAlive
                If progressbar IsNot Nothing Then
                    If TypeOf progressbar Is System.Windows.Forms.ProgressBar Or _
                              progressbar.GetType.FullName = "DevComponents.DotNetBar.ProgressBarItem" Or _
                              progressbar.GetType.FullName = "DevComponents.DotNetBar.Controls.ProgressBarX" Or _
                              progressbar.GetType.FullName = "C1.Win.C1Ribbon.RibbonProgressBar" Then
                        If progressbar.Value < (progressbar.Maximum * 0.95) Then progressbar.Value += 1
                    End If
                End If
                Threading.Thread.Sleep(1) : Application.DoEvents()
            End While
        End Sub


        ''' <summary>
        ''' Waits for the IAsyncResult Interface's process to be completed.
        ''' </summary>
        ''' <param name="asyncresult">IAsyncResult interface to wait for.</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub WaitToFinish(ByVal asyncresult As IAsyncResult)
            WaitToFinish(asyncresult, Nothing)
        End Sub

        ''' <summary>
        ''' Waits for the IAsyncResult Interface's process to be completed.
        ''' </summary>
        ''' <param name="asyncresult">IAsyncResult interface wait for.</param>
        ''' <param name="progressbar">Progress bar object to show the current running time of the delegate.</param>
        ''' <remarks></remarks>
        Public Overloads Shared Sub WaitToFinish(ByVal asyncresult As IAsyncResult, ByVal progressbar As Object)
            While Not asyncresult.IsCompleted
                If progressbar IsNot Nothing Then
                    If TypeOf progressbar Is System.Windows.Forms.ProgressBar Or _
                              progressbar.GetType.FullName = "DevComponents.DotNetBar.ProgressBarItem" Or _
                              progressbar.GetType.FullName = "DevComponents.DotNetBar.Controls.ProgressBarX" Or _
                              progressbar.GetType.FullName = "C1.Win.C1Ribbon.RibbonProgressBar" Then
                        If progressbar.Value < (progressbar.Maximum * 0.95) Then progressbar.Value += 1
                    End If
                End If
                Thread.Sleep(1) : Application.DoEvents()
            End While
        End Sub

        ''' <summary>
        ''' Maximize a progress bar's value synchronously and hides it afterwards.
        ''' </summary>
        ''' <param name="progressbar"></param>
        ''' <remarks></remarks>
        Public Shared Sub EndProgress(ByVal progressbar As Object)
            If TypeOf progressbar Is System.Windows.Forms.ProgressBar Or _
               progressbar.GetType.FullName = "DevComponents.DotNetBar.ProgressBarItem" Or _
               progressbar.GetType.FullName = "DevComponents.DotNetBar.Controls.ProgressBarX" Or _
               progressbar.GetType.FullName = "C1.Win.C1Ribbon.RibbonProgressBar" Then
                While progressbar.Value < progressbar.Maximum
                    progressbar.Value += 1 : Thread.Sleep(1) : Application.DoEvents()
                End While
                progressbar.Hide()
            End If
        End Sub

#Region "Func"
        ''' <summary>
        ''' Encapsulates a method that has 5 parameters and returns a value of the type specified by the TResult parameter.
        ''' </summary>
        ''' <typeparam name="T1"></typeparam>
        ''' <typeparam name="T2"></typeparam>
        ''' <typeparam name="T3"></typeparam>
        ''' <typeparam name="T4"></typeparam>
        ''' <typeparam name="T5"></typeparam>
        ''' <typeparam name="TResult"></typeparam>
        ''' <param name="arg1"></param>
        ''' <param name="arg2"></param>
        ''' <param name="arg3"></param>
        ''' <param name="arg4"></param>
        ''' <param name="arg5"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Delegate Function Func(Of T1, T2, T3, T4, T5, TResult)(ByVal arg1 As T1, ByVal arg2 As T2, ByVal arg3 As T3, ByVal arg4 As T4, ByVal arg5 As T5) As TResult

        ''' <summary>
        ''' Encapsulates a method that has 6 parameters and returns a value of the type specified by the TResult parameter.
        ''' </summary>
        ''' <typeparam name="T1"></typeparam>
        ''' <typeparam name="T2"></typeparam>
        ''' <typeparam name="T3"></typeparam>
        ''' <typeparam name="T4"></typeparam>
        ''' <typeparam name="T5"></typeparam>
        ''' <typeparam name="T6"></typeparam>
        ''' <typeparam name="TResult"></typeparam>
        ''' <param name="arg1"></param>
        ''' <param name="arg2"></param>
        ''' <param name="arg3"></param>
        ''' <param name="arg4"></param>
        ''' <param name="arg5"></param>
        ''' <param name="arg6"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Delegate Function Func(Of T1, T2, T3, T4, T5, T6, TResult)(ByVal arg1 As T1, ByVal arg2 As T2, ByVal arg3 As T3, ByVal arg4 As T4, ByVal arg5 As T5, ByVal arg6 As T6) As TResult
#End Region

#Region "Action"
        ''' <summary>
        ''' Encapsulates a method that has 5 parameters and does note return any value.
        ''' </summary>
        ''' <typeparam name="T1"></typeparam>
        ''' <typeparam name="T2"></typeparam>
        ''' <typeparam name="T3"></typeparam>
        ''' <typeparam name="T4"></typeparam>
        ''' <typeparam name="T5"></typeparam>
        ''' <param name="arg1"></param>
        ''' <param name="arg2"></param>
        ''' <param name="arg3"></param>
        ''' <param name="arg4"></param>
        ''' <param name="arg5"></param>
        ''' <remarks></remarks>
        Public Delegate Sub Action(Of T1, T2, T3, T4, T5)(ByVal arg1 As T1, ByVal arg2 As T2, ByVal arg3 As T3, ByVal arg4 As T4, ByVal arg5 As T5)

        ''' <summary>
        ''' Encapsulates a method that has 6 parameters and does note return any value.
        ''' </summary>
        ''' <typeparam name="T1"></typeparam>
        ''' <typeparam name="T2"></typeparam>
        ''' <typeparam name="T3"></typeparam>
        ''' <typeparam name="T4"></typeparam>
        ''' <typeparam name="T5"></typeparam>
        ''' <typeparam name="T6"></typeparam>
        ''' <param name="arg1"></param>
        ''' <param name="arg2"></param>
        ''' <param name="arg3"></param>
        ''' <param name="arg4"></param>
        ''' <param name="arg5"></param>
        ''' <param name="arg6"></param>
        ''' <remarks></remarks>
        Public Delegate Sub Action(Of T1, T2, T3, T4, T5, T6)(ByVal arg1 As T1, ByVal arg2 As T2, ByVal arg3 As T3, ByVal arg4 As T4, ByVal arg5 As T5, ByVal arg6 As T6)
#End Region

#Region "Tuple"
        ''' <summary>
        ''' Enacapsulates an array of object with two data types.
        ''' </summary>
        ''' <typeparam name="T1"></typeparam>
        ''' <typeparam name="T2"></typeparam>
        ''' <param name="arg1"></param>
        ''' <param name="arg2"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Tuple(Of T1, T2)(ByVal arg1 As T1, ByVal arg2 As T2) As Object()
            Dim values(1) As Object
            values(0) = arg1 : values(1) = arg2
            Return values
        End Function

        ''' <summary>
        ''' Enacapsulates an array of object with three data types. 
        ''' </summary>
        ''' <typeparam name="T1"></typeparam>
        ''' <typeparam name="T2"></typeparam>
        ''' <typeparam name="T3"></typeparam>
        ''' <param name="arg1"></param>
        ''' <param name="arg2"></param>
        ''' <param name="arg3"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Tuple(Of T1, T2, T3)(ByVal arg1 As T1, ByVal arg2 As T2, ByVal arg3 As T3) As Object()
            Dim values(2) As Object
            values(0) = arg1 : values(1) = arg2 : values(2) = arg3
            Return values
        End Function

        ''' <summary>
        ''' Enacapsulates an array of object with four data types. 
        ''' </summary>
        ''' <typeparam name="T1"></typeparam>
        ''' <typeparam name="T2"></typeparam>
        ''' <typeparam name="T3"></typeparam>
        ''' <typeparam name="T4"></typeparam>
        ''' <param name="arg1"></param>
        ''' <param name="arg2"></param>
        ''' <param name="arg3"></param>
        ''' <param name="arg4"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Tuple(Of T1, T2, T3, T4)(ByVal arg1 As T1, ByVal arg2 As T2, ByVal arg3 As T3, ByVal arg4 As T4) As Object()
            Dim values(3) As Object
            values(0) = arg1 : values(1) = arg2 : values(2) = arg3 : values(3) = arg4
            Return values
        End Function

        ''' <summary>
        ''' Enacapsulates an array of object with five data types. 
        ''' </summary>
        ''' <typeparam name="T1"></typeparam>
        ''' <typeparam name="T2"></typeparam>
        ''' <typeparam name="T3"></typeparam>
        ''' <typeparam name="T4"></typeparam>
        ''' <typeparam name="T5"></typeparam>
        ''' <param name="arg1"></param>
        ''' <param name="arg2"></param>
        ''' <param name="arg3"></param>
        ''' <param name="arg4"></param>
        ''' <param name="arg5"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Tuple(Of T1, T2, T3, T4, T5)(ByVal arg1 As T1, ByVal arg2 As T2, ByVal arg3 As T3, ByVal arg4 As T4, ByVal arg5 As T5) As Object()
            Dim values(4) As Object
            values(0) = arg1 : values(1) = arg2 : values(2) = arg3
            values(3) = arg4 : values(4) = arg5
            Return values
        End Function

        ''' <summary>
        ''' Enacapsulates an array of object with six data types. 
        ''' </summary>
        ''' <typeparam name="T1"></typeparam>
        ''' <typeparam name="T2"></typeparam>
        ''' <typeparam name="T3"></typeparam>
        ''' <typeparam name="T4"></typeparam>
        ''' <typeparam name="T5"></typeparam>
        ''' <typeparam name="T6"></typeparam>
        ''' <param name="arg1"></param>
        ''' <param name="arg2"></param>
        ''' <param name="arg3"></param>
        ''' <param name="arg4"></param>
        ''' <param name="arg5"></param>
        ''' <param name="arg6"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Tuple(Of T1, T2, T3, T4, T5, T6)(ByVal arg1 As T1, ByVal arg2 As T2, ByVal arg3 As T3, ByVal arg4 As T4, ByVal arg5 As T5, ByVal arg6 As T6) As Object()
            Dim values(5) As Object
            values(0) = arg1 : values(1) = arg2 : values(2) = arg3
            values(3) = arg4 : values(4) = arg5 : values(5) = arg6
            Return values
        End Function
#End Region

    End Class

End Namespace