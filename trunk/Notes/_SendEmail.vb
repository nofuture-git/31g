Imports Microsoft.VisualBasic
Imports System.Net

Public Class SendEmail
    Public Shared Sub Main()
        Dim emailService As System.Net.Mail.SmtpClient
        Dim mailCurrent As New System.Net.Mail.MailMessage
        Dim myFrom As System.Net.Mail.MailAddress
        emailService = New System.Net.Mail.SmtpClient("172.16.1.254")

        Try
            myFrom = New System.Net.Mail.MailAddress("AsmallTest@covansys.com")
            mailCurrent.To.Add("bstarrett@covansys.com")
            mailCurrent.From = myFrom
            mailCurrent.Body = "a nice body"
            mailCurrent.Subject = "an old subject"
            emailService.Send(mailCurrent)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally

        End Try

    End Sub
End Class
