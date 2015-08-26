Imports System.IO

Module TextWriter
    Sub Main()
        Dim myWriter As StreamWriter
        
        Try
          myWriter = New StreamWriter("C:\anyFolder\myWriter.txt")
          myWriter.WriteLine("Here is the First Line")
          myWriter.WriteLine("Here is the Second Line")
        Catch ex As Exception
        
        Finally
            myWriter.Close
        End Try
    End Sub
End Module