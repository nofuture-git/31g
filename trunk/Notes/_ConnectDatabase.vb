'Notes Connecting a database in VB.NET app

'Add app.config file
'1. VS 2005 > Solution Explorer > highlight solution's name 
'2. Right Click white space > Add > New Item
'3. Application Configuration File
'4. Remove > system.diagnostics Node
'5. Insert > appSettings node w/i configuration
'6. Insert > add key > specify key > specify value

Imports Microsoft.VisualBasic
Imports System.Configuration
Imports System.Data
Imports System.Data.OleDb

Public Class ConnectDatabase
    Public Shared Sub Main()
        'Dim myServer As String=ConfigurationManager.AppSettings.Item("myServerKeyItem")
        Dim myServer As String = _
                "Provider=MSDAORA.1;User ID=common; Password=common;Data Source=BNKC01D"
        Dim myConnection As New OleDb.OleDbConnection(myServer)
        Dim myCmd As OleDb.OleDbCommand
        Dim myReader As OleDbDataReader
        Dim mySQL As String
        Dim myFieldCount As Integer
        Dim i As Integer

        Try
            mySQL = "select * from common.wicprogramfile"
            myConnection.Open()
            myCmd = myConnection.CreateCommand()
            myCmd.CommandText = mySQL
            myReader = myCmd.ExecuteReader()
            myFieldCount = myReader.FieldCount

            While myReader.Read()
                For i = 0 To myFieldCount - 1
                    Console.Write(myReader(i).ToString() & vbTab)
                Next
                Console.WriteLine()
            End While
        Catch ex As Exception
        Finally
            myConnection.Close()
        End Try
    End Sub

End Class
