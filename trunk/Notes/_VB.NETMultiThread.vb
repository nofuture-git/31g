Imports System.Threading
Public Class myTestController
    Dim WithEvents spn As New myTestSpinner
    Dim itr As New myTestIterator
    Dim lock As New Object
    Dim mylilDelegate As New myDelegate(AddressOf mySpinSub)


    Public Sub mySpinSub()
        spn.myTestSpin()
    End Sub

    Public Sub makeASpin()
        Try

            Dim StartThread As IAsyncResult
            StartThread = mylilDelegate.BeginInvoke(AddressOf aCallBack, mylilDelegate)
        Catch ex As Exception

        End Try
    End Sub

    Public Delegate Sub myDelegate()

    Public Sub handleThatEvent(ByVal name As String) Handles spn.myEvent
        SyncLock lock
            Console.WriteLine(name)
        End SyncLock
    End Sub

    Public Sub aCallBack(ByVal z As IAsyncResult)
        Console.WriteLine("Back")
        mylilDelegate.EndInvoke(z) 'critical call - must call if used BeginInvoke or result memory loss
    End Sub

End Class

Public Class myTestSpinner
    Public myCol As New Collection
    Public Event myEvent(ByVal spinname As String)

    Public Sub myTestSpin()
        Try
            Console.WriteLine("a test spin")
            RaiseEvent myEvent("I zombie")

        Catch ex As Exception
            RaiseEvent myEvent("I zombie")

        End Try
    End Sub
End Class

