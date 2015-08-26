Public Class OldSchoolDelegate
    'This is like:  typedef bool (*myDelegate)(); -or- bool (*myDelegate)();
    Delegate Function myDelegate() As Boolean
    'This is like:  myDelegate myDelRef; //specific to using the typedef
    Protected Friend myDelRef As myDelegate
    Delegate Function myComplexDelegate(ByVal delParam1 As String, ByVal delParam2 As String()) As String
    Protected Friend myComplexDelRef As myComplexDelegate
    Public Sub EncloseTheAction()
        'This is like: myDelRef = FunctionBeingPointedTo; -or- myDelegate = FunctionBeingPointedTo;
        myDelRef = New myDelegate(AddressOf FunctionBeingPointedTo)
        Dim aReturnValue As Boolean
        'This is like: aReturnValue = myDelRef(); -or- aReturnValue = (*myDelegate)();
        aReturnValue = myDelRef.Invoke()

        Dim complexReturnType As String
        myComplexDelRef = New myComplexDelegate(AddressOf ComplexFunctionBeingPointedTo)
        complexReturnType = myComplexDelRef.Invoke("param1", New String() {"param2", "param3"})

    End Sub
    Public Function FunctionBeingPointedTo() As Boolean
        Return True
    End Function
    Public Function ComplexFunctionBeingPointedTo(ByVal Param1 As String, ByVal Param2 As String()) As String
        Return "whatever"
    End Function
End Class
Public Class NewLamda
    Public Sub EncloseTheAction()
        Dim SimpleLamda As Func(Of Boolean) = AddressOf FunctionBeingPointedTo
        Dim aReturnType As Boolean
        'simple type where delegate is skipped
        aReturnType = SimpleLamda()
        'simpler type where delegate type (function pointer) is not named at all
        aReturnType = (Function() (FunctionBeingPointedTo()))()
        'simplest type where function pointer and the function itself are all inline.
        aReturnType = (Function() (True))()

        Dim ComplexLamda As Func(Of String, String(), String) = AddressOf ComplexFunctionBeingPointedTo
        Dim complexReturnType As String
        complexReturnType = ComplexLamda("param1", New String() {"param2", "param3"})
        complexReturnType = (Function() (ComplexFunctionBeingPointedTo("param1", New String() {"param2", "param3"})))()
        complexReturnType = (Function(Param1 As String, Param2 As String()) "whatever")("param1", New String() {"param2", "param3"})
    End Sub
    Public Function FunctionBeingPointedTo() As Boolean
        Return False
    End Function
    Public Function ComplexFunctionBeingPointedTo(ByVal Param1 As String, ByVal Param2 As String()) As String
        Return "whatever"
    End Function
End Class
