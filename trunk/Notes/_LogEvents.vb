'Notes Adding a log to a VB.NET app

'Add app.config file
'1. VS 2005 > Solution Explorer > highlight solution's name 
'2. Right Click white space > Add > New Item
'3. Application Configuration File
'4. Remove > system.diagnostics Node
'5. Insert > appSettings node w/i configuration
'6. Insert > add key > specify key > specify value
'7. Add System.Diagnostics as a reference
Imports System.Diagnostics


Module Module1

    Sub Main()

        Dim myEventLog As EventLog 'declare your log
        myEventLog = New EventLog() ' instantiate it 
        If Not EventLog.SourceExists(ConfigurationManager.AppSettings. _
                          Item("ItemKey")) Then 'does the source exist?
            EventLog.CreateEventSource(ConfigurationManager.AppSettings. _
                          Item("ItemKey"), _
                          ConfigurationManager.AppSettings.Item("ItemKey"))
        End If
        SyncLock myEventLog 'lock it, avoid race-conditions
            myEventLog.Source() = ConfigurationManager.AppSettings. _
                        Item("ItemKey") 'where is the entry coming from?
            myEventLog.WriteEntry("another" & vbCrLf & "line") 'write something
        End SyncLock

    End Sub

End Module
