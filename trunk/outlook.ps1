try{
if(-not [NoFuture.MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-OutlookEmail",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Export-OutlookAppointments", $MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("ImportRazrV3-OutlookCalendarEntry", $MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("AddRazrV3-CalendarEntry", $MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("SendGsmMT-Sms", $MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("ReadGsmMT-Sms", $MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("ListGsmMT-Sms", $MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("ListGsmMt-PhoneBook", $MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("AddGsmMt-PhoneBookEntry", $MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}

#----
# OUTLOOK
#----
<#
    .synopsis
    Gets email from the local inbox and saves them to disk as XML.
    
    .Description
    Using the COM Outlook object this function will parse the inbox 
    of the local outlook account and save the values of the applied 
    filter emails to the specified disk-drive locations.  This will
    include any attachments present therein.
    
    Emails get saved to a child directory of the Path parameter having
    the directory name as a Guid.  The attachments get saved within this
    guid directory by the name as in the email.  The content
    of the email is saved to an xml file therein as well.
    
    Expect to receive a popup warning from Outlook advising that something
    is attempting to programmatically access your inbox.  Set the time from 
    the given drop-down within the popup and click OK.
    
    .parameter Filter
    The email property to filter on.
    
    .parameter Pattern
    The value of the filter.
    
    .parameter Path
    The path location in which to place the XML email output and attachments.
    
    .example
    C:\PS>Get-OutlookEmail -Filter 'From' -Pattern 'anEmailAddr@SomeCompany.net' -Path 'C:\My Emails\'
    
    .example
    C:\PS>Get-OutlookEmail "Subject" "Please Respond to my email." "C:\My Emails"
    
    .outputs
    null
    
    
#>
function Get-OutlookEmail
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Filter,
        [Parameter(Mandatory=$true,position=1)]
        [string] $Pattern,
        [Parameter(Mandatory=$true,position=2)]
        [string] $Path
    )
    Process
    {
        #set array for guids to return
        $rtrn = @()
        
        #com obj prop isn't actually 'from' - its 'sender-name' so rewrite as needed
        if($Filter.ToLower() -eq "from"){$Filter = "sender-name"}
        
        #get connection to Outlook through the COM object
        $outlookObject = New-Object -ComObject Outlook.Application
        $mapi = $outlookObject.GetNameSpace("MAPI")
        $myinbox = $mapi.GetDefaultFolder('olFolderInbox')
        
        $totalItems = $myinbox.Items.Count
        
        #iterate all the items in the inbox filtering by the given filter and value
        $myinbox.Items | ? {Write-Progress -Activity ("'{0}'" -f $_.($Filter)) -status "Searching..." -PercentComplete ([NoFuture.Util.Etc]::CalcProgressCounter(($i++),$totalItems)); $_.($Filter) -match $Pattern} | % {
        
            #for the located emails set aside a unique directory to store the values and attachments
            $id = [System.Guid]::NewGuid().ToString()
            $itemPath = (Join-Path (Resolve-Path $Path) $id)
            $disregard = mkdir -Path $itemPath
            [System.Threading.Thread]::Sleep(1000);
            
            #get the xml that will contain the results
            $outlookEmail = New-Object System.Xml.Linq.XDocument
            $outlookEmail.Add((New-Object System.Xml.Linq.XElement([System.Xml.Linq.XName]::Get("outlook-email"))))
            $rootNode = $outlookEmail.FirstNode
            
            #append the pertinent data 
            $rootNode.Add((New-Object System.Xml.Linq.XElement([System.Xml.Linq.XName]::Get("sent-on"),$_.SentOn)))
            $rootNode.Add((New-Object System.Xml.Linq.XElement([System.Xml.Linq.XName]::Get("body"),(New-Object System.Xml.Linq.XCdata($_.Body)))))
            $rootNode.Add((New-Object System.Xml.Linq.XElement([System.Xml.Linq.XName]::Get("size"),$_.Size)))
            $rootNode.Add((New-Object System.Xml.Linq.XElement([System.Xml.Linq.XName]::Get("subject"),$_.Subject)))
            $rootNode.Add((New-Object System.Xml.Linq.XElement([System.Xml.Linq.XName]::Get("bcc"),$_.BCC)))
            $rootNode.Add((New-Object System.Xml.Linq.XElement([System.Xml.Linq.XName]::Get("cc"),$_.CC)))
            $rootNode.Add((New-Object System.Xml.Linq.XElement([System.Xml.Linq.XName]::Get("to"),$_.To)))
            $rootNode.Add((New-Object System.Xml.Linq.XElement([System.Xml.Linq.XName]::Get("sender-name"),$_.SenderName)))
            
            #append the attachments - saving them to file as encountered
            $attachments = (New-Object System.Xml.Linq.XElement([System.Xml.Linq.XName]::Get("attachments")))
            $_.Attachments | % {
                $fullName = (Join-Path $itemPath $_.FileName)
                $_.SaveAsFile($fullName)
                $attachments.Add((New-Object System.Xml.Linq.XElement([System.Xml.Linq.XName]::Get("attachment"),$fullName)))
            }
            $rootNode.Add($attachments)
            
            #clean up the filename 
            $outputXmlName = ("{0:yyyyMMddhhmm}_{1}.xml" -f [System.Convert]::ToDateTime($_.SentOn),$_.SenderName)
            [System.IO.Path]::GetInvalidFileNameChars() | % {
                $outputXmlName = $outputXmlName.Replace($_.ToString(),"")
            }
            
            #save the email document with some clues as to its origin
            $rootNode.Save((Join-Path $itemPath $outputXmlName))
            
            #add the folder id to the returnable collection results
            $rtrn += $id
        }
        
        #hand the guid's back to the calling assembly
        return $rtrn
    }

}#end Get-OutlookEmail

<#
    .SYNOPSIS
    Gets email from the local inbox and saves them to disk as XML.
    
    .DESCRIPTION
    Using the COM Outlook object this function will parse the calendar 
    of the local outlook account and saves the entries into the standard 
    iCal format.

    Expect to receive a popup warning from Outlook advising that something
    is attempting to programmatically access your calendar.  Set the time from 
    the given drop-down within the popup and click OK.
    
    .PARAMETER StartingFrom
    The date the export will begin from.
    
    .PARAMETER OutTo
    The date the export will go out to.
    
    .PARAMETER Path
    The to a folder in which to store the iCal output file, the cmdlet 
    will create a file name and therefore one should not be specified in 
    the path.
    
    .EXAMPLE

#>
function Export-OutlookAppointments
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [System.DateTime] $StartingFrom,
        [Parameter(Mandatory=$true,position=1)]
        [System.DateTime] $OutTo,
        [Parameter(Mandatory=$false,position=2)]
        [string] $Path
    )
    Process
    {
        if([System.String]::IsNullOrWhiteSpace($Path)){$Path = ([NoFuture.TempDirectories]::Calendar)}

        #if the caller specified a file-path, reassign to just the path
        if([System.IO.Path]::GetExtension($Path) -ne [string]::Empty){
            $Path = (Split-Path $Path -Parent)
        }

        $icsFileName = ("outlook.cal.from{0:yyyyMMdd}.to{1:yyyyMMdd}.ics" -f $StartingFrom,$OutTo)
        $icsFullName = (Join-Path $Path $icsFileName)

        if(Test-Path $icsFullName){rm -Path $icsFullName -Force}

        $outlookObject = New-Object -ComObject Outlook.Application
        $calendar = $outlookObject.Session.GetDefaultFolder('olFolderCalendar')

        #outlook::CalendarSharing
        $outlookCalendarExporter = $calendar.GetCalendarExporter()
        $outlookCalendarExporter.StartDate = $StartingFrom.ToString("g")
        $outlookCalendarExporter.EndDate = $OutTo.ToString("g")
        
        #http://msdn.microsoft.com/en-us/library/office/ff866922.aspx
        $outlookCalendarExporter.CalendarDetail = 1 #w/o this the Summary property is empty

        $outlookCalendarExporter.SaveAsICal($icsFullName)
        return $icsFullName
    }
}#end Export-OutlookAppointments

<#
    .SYNOPSIS
    Adds an appointment to you local Outlook Calendar.
    
    .DESCRIPTION
    Using the COM Outlook object this function will add a new calendar 
    to your Outlook Calendar.

    The subject is always prefixed with a keyword whose value is assigned
    at NoFuture.Shared.Constants.OUTLOOK_APPT_PREFIX followed by a space.

    Expect to receive a popup warning from Outlook advising that something
    is attempting to programmatically access your calendar.  Set the time from 
    the given drop-down within the popup and click OK.
    
    .PARAMETER StartDate
    The start of the appointment to be added.
    
    .PARAMETER EndDate
    The end of the appointment to be added.
    
    .PARAMETER Subject
    
    .PARAMETER Location
    [Optional] 

    .PARAMETER Body
    [Optional] If left as null or whitespace it will
    be assigned to the value of the Subject
    
    .EXAMPLE

#>
function Import-OutlookAppointment
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [System.DateTime] $StartDate,
        [Parameter(Mandatory=$true,position=1)]
        [System.DateTime] $EndDate,
        [Parameter(Mandatory=$true,position=2)]
        [string] $Subject,
        [Parameter(Mandatory=$false,position=3)]
        [string] $Location,
        [Parameter(Mandatory=$false,position=4)]
        [int] $ReminderInMin,
        [Parameter(Mandatory=$false,position=5)]
        [string] $Body
    )
    Process
    {

        $outlookObject = New-Object -ComObject Outlook.Application
        $appt = $outlookObject.CreateItem("olAppointmentItem")
        $appt.Start = $StartDate.ToString("g")
        $appt.End = $EndDate.ToString("g")
        $appt.Subject = "{0} {1}" -f [NoFuture.Shared.Constants]::OUTLOOK_APPT_PREFIX, $Subject
        $appt.Location = $Location
        if([string]::IsNullOrWhiteSpace($Body)){ $Body = $Subject }
        $appt.Body = $Body
        if($ReminderInMin -gt 0){
            $appt.ReminderSet = $true
            $appt.ReminderMinutesBeforeStart = $ReminderInMin
        }

        $appt.Save()
    }
}#end Import-OutlookAppointment

#----
# Motorola RAZR V3
#----
<#
    .SYNOPSIS
    Copies calendar entries from Outlook to RazrV3 phone.
    
    .DESCRIPTION
    Calendar entries are not part of the GSM spec and therefore the
    commands used to sent calendar entries on the Serial Port are 
    specific to RazrV3 phone.

    The cmdlet will attempt to export all calendar entries for the 
    past month to an iCal file stored at NoFuture.TempDirectories.Calendar.
    So expect to get the popup from Outlook that something is attempting 
    to programmatically access it.

    The cmdlet then scans the contents of the iCal file looking for entries
    whose Subject matches.  Finding matches the cmdlet then wires the entries
    over to the RazrV3 device.

    .PARAMETER PortName
    The Port in which the RazrV3 phone is currently connected.
    
    .PARAMETER OutlookSubject
    A string to match on from Outlook Calendar Subject.  This must match
    exactly so cut & paste it.

    .PARAMETER iCalPath
    If you don't want the cmdlet to fetch appointments from outlook then 
    specify a iCal file-path instead and only it will be used in the export
    process.
    
    .EXAMPLE

#>
function ImportRazrV3-OutlookCalendarEntry
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string]$PortName,

        [Parameter(Mandatory=$true,position=1)]
        [string] $OutlookSubject,

        [Parameter(Mandatory=$false,position=2)]
        [string] $iCalPath

    )
    Process
    {
        $ms = [NoFuture.Gsm.Settings]::ThreadSleepMs

        $razrV3Entries = ReadRazrV3-Calendar -PortName $PortName

        if([string]::IsNullOrWhiteSpace($iCalPath)){
            $iCalPath = Export-OutlookAppointments -StartingFrom $(Get-Date) -OutTo ($(Get-Date).AddMonths(1))
        }
        
        if(-not (Test-Path $iCalPath)){
            throw "could not locate an iCal file at '$iCalPath'"
            break;
        }

        $cal = [DDay.iCal.iCalendar]::LoadFromFile($iCalPath)

        $cal.Events | ? {$_.Summary.Trim() -eq $OutlookSubject.Trim()} | % {
            $description = $_.Summary
            $startTime = $_.Start.Value
            $duration = $_.Duration.TotalMinutes

            $outlookEntry = New-Object NoFuture.Gsm.RazrV3.CalendarEntry
            $outlookEntry.Description = $description
            $outlookEntry.EntryDateTime = $startTime
            $outlookEntry.Duration = $duration

            $entryIsPresent = $false
            $razrV3Entries | % {if($outlookEntry.Equals($_) -and (-not($entryIsPresent))){$entryIsPresent = $true}}

            if(-not $entryIsPresent){
                Write-Progress -Activity ("Working Entry {0}: {1} +{2}" -f $outlookEntry.Description,$outlookEntry.EntryDateTime,$outlookEntry.Duration) -Status "OK"
                AddRazrV3-CalendarEntry -PortName $PortName -Description $outlookEntry.Description -EntryDateTime $outlookEntry.EntryDateTime -Duration $outlookEntry.Duration
                [System.Threading.Thread]::Sleep($ms)
            }
        }
    }
}#end ImportRazrV3-OutlookCalendarEntry

<#
    .SYNOPSIS
    Reads the calendar from a connected RazrV3 device.
    
    .DESCRIPTION
    Reads all the entries contained on the device and returns 
    them as a list of 
    System.Collections.ObjectModel.Collection`1[NoFuture.Gsm.RazrV3.CalendarEntry]

    Calendar is not a GSM standard and therefore this is specific to this
    device.

    The connection is on a SerialPort and every time some data is written
    there is a thread wait of NoFuture.Gsm.Settings.ThreadSleepMs
    milliseconds, so if there are a lot of entries the cmdlet may take
    awhile to complete.

    .PARAMETER PortName
    The Port in which the RazrV3 phone is currently connected.
    
    .EXAMPLE

#>
function ReadRazrV3-Calendar
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string]$PortName
    )
    Process
    {
        $entryIndex = 0

        $sp = New-Object System.IO.Ports.SerialPort
        $sp.PortName = $PortName
        $sp.BaudRate = 9600
        $sp.RtsEnable = $true
        $sp.DtrEnable = $true
        $sp.NewLine = ([System.Convert]::ToChar([byte]0xD));
        try{

            $sp.Open()

            $ms = [NoFuture.Gsm.Settings]::ThreadSleepMs

            if(-not (IsGsmMT-ThisPortName -Port $sp)){throw ("the port-name '{0}' is not responding to a standard GSM AT command" -f $PortName)}

            [System.Threading.Thread]::Sleep($ms)

            #block access to calendar in phone
            $entryBlock = $false
            $sp.WriteLine("AT+MDBL=1")

            [System.Threading.Thread]::Sleep($ms)
            $spOut = $sp.ReadExisting()
            if(IsGsmMT-CmdOutputError $spOut){throw "Could not get a lock on the device calendar with command +MDBL=1"}
            $entryBlock = $true

            [System.Threading.Thread]::Sleep($ms)
            $entryIndex = GetRazrV3-NextCalendarIndex -Port $sp
            $entryIndex += 1

            [System.Threading.Thread]::Sleep($ms)
            $sp.WriteLine("AT+MDBR=0,$entryIndex")

            [System.Threading.Thread]::Sleep($ms)
            $spOut = $sp.ReadExisting()
            if(IsGsmMT-CmdOutputError $spOut){throw ("Could not perform a read operation on the calendar up to entry '{0}'." -f $entryIndex)}

            $entries = ReadRazrV3-CalendarEntry $spOut

            [System.Threading.Thread]::Sleep($ms)

            return $entries
        }
        catch [System.Exception]
        {
            Write-Host "An error occured, check the global 'error' variable for more details."

        }
        finally
        {
            #attempt to unblock the calendar if a lock was achieved
            if($entryBlock){
                $sp.WriteLine("AT+MDBL=0")
                [System.Threading.Thread]::Sleep($ms)
            }
            #clean up 
            if($sp -ne $null -and $sp.IsOpen){
                $sp.Close();
                $sp.Dispose()
            }
        }

    }
}#end ReadRazrV3-Calendar

function AddRazrV3-CalendarEntry
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string]$PortName,

        [Parameter(Mandatory=$true,position=1)]
        [string]$Description, 

        [Parameter(Mandatory=$true,position=2)]
        [System.DateTime]$EntryDateTime,

        [Parameter(Mandatory=$false,position=3)]
        [AllowNull()]
        [System.DateTime]$SetAlarm,

        [Parameter(Mandatory=$false,position=4)]
        [int]$Duration,

        [Parameter(Mandatory=$false,position=5)]
        [int]$RepeatAs
    )
    Process
    {
        $entryIndex = 0

        $sp = New-Object System.IO.Ports.SerialPort
        $sp.PortName = $PortName
        $sp.BaudRate = 9600
        $sp.RtsEnable = $true
        $sp.DtrEnable = $true
        $sp.NewLine = ([System.Convert]::ToChar([byte]0xD));
        try{

            $sp.Open()

            $ms = [NoFuture.Gsm.Settings]::ThreadSleepMs

            if(-not (IsGsmMT-ThisPortName -Port $sp)){throw ("the port-name '{0}' is not responding to a standard GSM AT command" -f $PortName)}

            $entryIndex = GetRazrV3-NextCalendarIndex -Port $sp

            if($entryIndex -lt 0){throw "The next calendar memory index could not be resolved for the device, no changes made."; }

            $mdbrParams = (DraftRazrV3-CalendarEntry -Description $Description -EntryDateTime $EntryDateTime -SetAlarm $SetAlarm -Duration $Duration -RepeatAs $RepeatAs)

            #block access to calendar in phone
            $entryBlock = $false
            $sp.WriteLine("AT+MDBL=1")
            [System.Threading.Thread]::Sleep($ms)
            $spOut = $sp.ReadExisting()
            if(IsGsmMT-CmdOutputError $spOut){throw "Could not get a lock on the device calendar with command +MDBL=1"}
            $entryBlock = $true

            [System.Threading.Thread]::Sleep($ms)

            #set index for write
            $sp.WriteLine(("AT+MDBWE={0},0,0" -f $entryIndex))
            [System.Threading.Thread]::Sleep($ms)
            $spOut = $sp.ReadExisting()
            if(IsGsmMT-CmdOutputError $spOut){throw ("Could not gain read-access to calendar entry at index {0}." -f $entryIndex)}

            [System.Threading.Thread]::Sleep($ms)

            #write entry to device
            $sp.WriteLine(("AT+MDBW={0}" -f $mdbrParams))
            [System.Threading.Thread]::Sleep($ms)
            $spOut = $sp.ReadExisting()
            if(IsGsmMT-CmdOutputError $spOut){throw ("Attempted to record `n'{0}'`n to device, but an error was detected.  The state of the entry is unknown." -f $mdbrParams)}

            [System.Threading.Thread]::Sleep($ms)

        
        }
        catch [System.Exception]
        {
            Write-Host "An error occured, check the global 'error' variable for more details."
        }
        finally
        {
            #clean up 
            if($sp -ne $null -and $sp.IsOpen){
                #attempt to unblock the calendar if a lock was achieved
                if($entryBlock){
                    $sp.WriteLine("AT+MDBL=0")
                    [System.Threading.Thread]::Sleep($ms)
                }
                $sp.Close();
                $sp.Dispose()
            }
        }
    }#end Process
}#end AddRazrV3-CalendarEntry

function GetRazrV3-NextCalendarIndex
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [System.IO.Ports.SerialPort]$Port
    )
    Process
    {
        $sp = $Port
        $ms = [NoFuture.Gsm.Settings]::ThreadSleepMs

        #open the port if not open already
        if(-not $sp.IsOpen){$sp.Open();}
        
        #send command to get current number of entries
        $sp.WriteLine("AT+MDBR=?")
        [System.Threading.Thread]::Sleep($ms)
        $spOut = $sp.ReadExisting()

        #test command was digested by port
        if(IsGsmMT-CmdOutputError $spOut){return  -1;}

        #cast returned output to an array
        $mdbrOut = ($spOut.Split("`n") | % {$_.Trim()})

        #find the particular results of the command
        $mdbrOut = ($mdbrOut | ? {$_.StartsWith("+MDBR:")})
        if([string]::IsNullOrWhiteSpace($mdbrOut)){return -1;}

        #further break down results to integer values 
        $mdbrOut = $mdbrOut.Split(":")
        if($mdbrOut -eq $null -or $mdbrOut.Length -le 1){return -1;}
        $mdbrOut = $mdbrOut[1].Split(",")
        if($mdbrOut -eq $null -or $mdbrOut.Length -le 1) {return -1;}

        #get the two particular values of interest
        $entryBlocks = 0
        if(-not ([System.Int32]::TryParse($mdbrOut[0],[ref]$entryBlocks))){return -1;}
        $nextEntry = 0
        if(-not ([System.Int32]::TryParse($mdbrOut[1],[ref]$nextEntry))){return -1;}
        $nextEntry += 1

        #test if everything is working but there is no more room on the device
        if($nextEntry -ge $entryBlocks){throw "no more room for further entries available, clear out some old entries and try again."}

        #return the next index which is free
        return $nextEntry

    }
}#end GetRazrV3-NextCalendarIndex

function DraftRazrV3-CalendarEntry($Description,$EntryDateTime,$SetAlarm,$Duration,$RepeatAs){
    $defaultTime = "00:00"
    $defaultDate = "00-00-2000"
    $defaultDuration = 60

    if($RepeatAs -eq $null){$RepeatAs = 0}

    if($SetAlarm -ne $null){
        $alarmOnFlag =  1
        $alarmTime = ("{0:HH:mm}" -f $SetAlarm)
        $alarmDate = ("{0:MM-dd-yyyy}" -f $SetAlarm)
    }
    else{
        $alarmOnFlag =  0
        $alarmTime = $defaultTime
        $alarmDate = $defaultDate
    }

    $allDayFlag = 1
    $startTime = ("{0:HH:mm}" -f $EntryDateTime)
    $startDate = ("{0:MM-dd-yyyy}" -f $EntryDateTime)

    if($Duration -eq 0){
        $Duration = $defaultDuration
    }
        

    $mdbrParams = ("{0},`"{1}`",{2},{3},`"{4}`",`"{5}`",{6},`"{7}`",`"{8}`",{9}" -f $entryIndex,$Description,$allDayFlag,$alarmOnFlag,$startTime,$startDate,$Duration,$alarmTime,$alarmDate,$RepeatAs)

    return $mdbrParams
} #end DraftRazrV3-CalendarEntry

function ReadRazrV3-CalendarEntry($spOut) {
    if([System.String]::IsNullOrWhiteSpace($spOut)){return $null}

    $lines = ($spOut.Split("`n") | ? {-not([System.String]::IsNullOrWhiteSpace($_)) -and $_.StartsWith("+MDBR")})
    if($lines -eq $null -or $lines.Length -eq 0) {return $null}

    $entries = New-Object 'System.Collections.ObjectModel.Collection`1[NoFuture.Gsm.RazrV3.CalendarEntry]'

    $lines | ? {-not([System.String]::IsNullOrWhiteSpace($_))} | % {
        $line = $_
        $line = $line.Replace("+MDBR: ","")
        $vals = $line.Split(",")
        $entry = New-Object NoFuture.Gsm.RazrV3.CalendarEntry
        $entry.EntryIndex = $vals[0]
        $description = $vals[1]
        $description = $description.Substring(1,($description.Length-2))
        $entry.Description = $vals[1]

        $entryTime = $vals[4]
        $entryDate = $vals[5]
        $entry.EntryDateTime = [System.DateTime]("{0} {1}" -f $entryDate.Replace("`"",""),$entryTime.Replace("`"",""))
        $entry.Duration = $vals[6]

        $entries.Add($entry)
    }

    return $entries

}#end ReadRazrV3-CalendarEntry

#----
# GSM
#----
<#
    .SYNOPSIS
    Gets an array of PhoneBookEntry(s)
    
    .DESCRIPTION
    This cmdlet is built based on functionality as specified in 
    '3GPP TS 27.007 version 8.3.0 Release 8' 
    Section 8.11-12 'Read phonebook entries +CPBR'.

    .PARAMETER PortName
    The Port in which the Mobile Equipment is currently connected.
    
    .PARAMETER PhoneBookCode
    [optional] A valid Phone Book Code as listed in section 8.11 of the above 
    cited document.  If this is left null or empty then the current
    Phone Book of the Mobile Equipment is used.

    .EXAMPLE
#>
function ListGsmMt-PhoneBook
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string]$PortName,

        [Parameter(Mandatory=$false,position=1)]
        [string] $PhoneBookCode
    )
    Process
    {
        $sp = New-Object System.IO.Ports.SerialPort
        $sp.PortName = $PortName
        $sp.BaudRate = 9600
        $sp.RtsEnable = $true
        $sp.DtrEnable = $true
        $sp.NewLine = ([System.Convert]::ToChar([byte]0xD));
        try{

            $sp.Open()

            $ms = [NoFuture.Gsm.Settings]::ThreadSleepMs

            if(-not (IsGsmMT-ThisPortName -Port $sp)){throw ("the port-name '{0}' is not responding to a standard GSM AT command" -f $PortName)}

            [System.Threading.Thread]::Sleep($ms)

            [NoFuture.Gsm.PhoneBook] $atEntryPhoneBook = $null;
            [NoFuture.Gsm.PhoneBook] $assignedPhoneBook = $null;
            $phoneBookEntries = @()

            #set the phone book to the storage device if specified
            if(-not [string]::IsNullOrWhiteSpace($PhoneBookCode)){

                #check if this is a valid phone book
                if(-not ([NoFuture.Gsm.PhoneBook]::IsValidPhoneBookCode($PhoneBookCode))){
                    throw ("'$PhoneBookCode' is not valid, choose a value from NoFuture.Gsm.PhoneBook.")
                }

                #get the original so it may be set back upon exit
                $atEntryPhoneBook =  GetGsmMT-CurrentPhoneBook -Port $sp;

                #get the current
                $assignedPhoneBook = SelectGsmMT-PhoneBook -Port $sp -PhoneBookCode $PhoneBookCode
            }
            else {
                
                #having not specified a PhoneBook, just get current
                $assignedPhoneBook =  GetGsmMT-CurrentPhoneBook -Port $sp;
            }

            #check that the phone book has any entries at all
            if($assignedPhoneBook -ne $null -and $assignedPhoneBook.BlocksUsed -gt 0)
            {
                $sp.WriteLine(("AT+CPBR=1,{0}"-f $assignedPhoneBook.BlocksUsed))

                #only about 20 lines will be written to the buffer per half-a-second
                $contactsWaitTime = ($assignedPhoneBook.BlocksUsed % 20) * 500
                [System.Threading.Thread]::Sleep($contactsWaitTime)

                $spOut = $sp.ReadExisting()

                if((IsGsmMT-CmdOutputError $spOut)){ 
                    throw ("an error occured while trying to list phone book entries '$spOut'"); 
                }

                #parse each returned line, add to array
                $spOut.Split("`n") | ? {-not ([string]::IsNullOrWhiteSpace($_))} | % {
                    [NoFuture.Gsm.PhoneBookEntry] $phBookEntryOut = $null;
                    if([NoFuture.Gsm.PhoneBookEntry]::TryParse($_, [ref] $phBookEntryOut)){
                        
                        $phoneBookEntries += $phBookEntryOut

                    }#end TryParse true

                }#end foreach PhoneBookEntry
            }

            #return array of PhoneBookEntries
            return $phoneBookEntries

        }
        catch [System.Exception]
        {
            Write-Host "An error occured, check the global 'error' variable for more details."
        }
        finally
        {
            if($sp -ne $null -and $sp.IsOpen){

                #if the cmdlet changed the phonebook, set it back
                if([NoFuture.Gsm.Settings]::HaveSetPhoneBook -and $atEntryPhoneBook -ne $null){
                    $atEntryPhoneBook = SelectGsmMT-PhoneBook -Port $sp -PhoneBookCode $atEntryPhoneBook.Storage
                    $sp.WriteLine("AT+CMGF=$atCmdEntry")
                    [System.Threading.Thread]::Sleep($ms)
                }
                #clean up 
                $sp.Close();
                $sp.Dispose()
            }
            [NoFuture.Gsm.Settings]::HaveSetPhoneBook = $false
        }#end try\catch\finally        
    }
}#end ListGsmMt-PhoneBook

<#
    .SYNOPSIS
    Add a new entry to the current Phone Book of the Mobile Equipment
    
    .DESCRIPTION
    Given the PhoneBookEntry, the ToString() method is called whose 
    contents are passed into the Serial Port prefixed with the
    'AT+CPBW=' command.
    No index checking is performed so the caller will need to verify
    the given index is not out-of-bounds.  In addition if the index 
    is already present the contents previously held by that index 
    will be overwritten.

    .PARAMETER PortName
    The Serial Port in which the Mobile Equipment is currently connected.

    .PARAMETER PhoneBookEntry
    Some entry data to be copied over to the Mobile Equipment

    .EXAMPLE
#>
function AddGsmMt-PhoneBookEntry
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string]$PortName,

        [Parameter(Mandatory=$true,position=1)]
        [NoFuture.Gsm.PhoneBookEntry] $PhoneBookEntry
    )
    Process
    {
        $sp = New-Object System.IO.Ports.SerialPort
        $sp.PortName = $PortName
        $sp.BaudRate = 9600
        $sp.RtsEnable = $true
        $sp.DtrEnable = $true
        $sp.NewLine = ([System.Convert]::ToChar([byte]0xD));
        try{

            $sp.Open()

            $ms = [NoFuture.Gsm.Settings]::ThreadSleepMs

            if(-not (IsGsmMT-ThisPortName -Port $sp)){throw ("the port-name '{0}' is not responding to a standard GSM AT command" -f $PortName)}

            #get next index from current phone book
            $currentPhoneBook = GetGsmMT-CurrentPhoneBook -Port $sp

            #assign the Index if caller did not specify one
            $nextIndex = $currentPhoneBook.BlocksUsed + 1
            if($PhoneBookEntry.Index -eq 0){
                $PhoneBookEntry.Index = $nextIndex
            }

            [System.Threading.Thread]::Sleep($ms)

            #TS 27.007 Section 8.14 
            $sp.WriteLine(("AT+CPBW={0}"-f $PhoneBookEntry.ToString()))

            [System.Threading.Thread]::Sleep($ms)

            $spOut = $sp.ReadExisting()

            if((IsGsmMT-CmdOutputError $spOut)){ 
                throw ("an error occured while trying to list phone book entries '$spOut'"); 
            }
            else{
                return $spOut
            }

        }
        catch [System.Exception]
        {
            Write-Host "An error occured, check the global 'error' variable for more details."
        }
        finally
        {
            if($sp -ne $null -and $sp.IsOpen){

                #clean up 
                $sp.Close();
                $sp.Dispose()
            }

        }#end try\catch\finally        
    }
}#end AddGsmMt-PhoneBookEntry

<#
    .SYNOPSIS
    Sends a text message from the command line using the connected Mobile Equipment
    
    .DESCRIPTION
    This cmdlet is built based on functionality as specified in 
    'ETSI TS 27.005 V11.0.0 (2012-10)'. See section 3 'Text Mode'
    It has only ever been tested on my RazrV3 device, which 
    is very old (as cell phones go).

    .PARAMETER PortName
    The Port in which the Mobile Equipment is currently connected.
    
    .PARAMETER PhoneNumber
    A phone number. The cmdlet will attempt to 'parse' the value 
    into the format required by the spec.  SMS short numbers (the one's
    starting with a '*') is untested.

    .PARAMETER Message
    A string message. No attempt is made to validate the characters
    as suitable for SMS (which is octet).

    .EXAMPLE

#>
function SendGsmMT-Sms
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string]$PortName,

        [Parameter(Mandatory=$true,position=1)]
        [string] $PhoneNumber,

        [Parameter(Mandatory=$true,position=2)]
        [string] $Message
    )
    Process
    {
        $sp = New-Object System.IO.Ports.SerialPort
        $sp.PortName = $PortName
        $sp.BaudRate = 9600
        $sp.RtsEnable = $true
        $sp.DtrEnable = $true
        $sp.NewLine = ([System.Convert]::ToChar([byte]0xD));
        try{

            $sp.Open()

            $ms = [NoFuture.Gsm.Settings]::ThreadSleepMs

            if(-not (IsGsmMT-ThisPortName -Port $sp)){throw ("the port-name '{0}' is not responding to a standard GSM AT command" -f $PortName)}

            [System.Threading.Thread]::Sleep($ms)

            #set MT to SMS text Mode
            $atCmdEntry = SetGsmMT-ToSmsTextMode -Port $sp

            #send SMS
            $smsMessage = [NoFuture.Gsm.Utility]::EncodeSmsMessage($PhoneNumber, $Message);

            [System.Threading.Thread]::Sleep($ms)

            $sp.WriteLine($smsMessage);

            #read response from Mobile Equipment
            [System.Threading.Thread]::Sleep($ms)
            $spOut = $sp.ReadExisting()

            if((IsGsmMT-CmdOutputError $spOut)){ 
                throw ("an error occured while trying to send the SMS on the Mobile Equipment '$spOut'"); 
            }
            else{
                Write-Host $spOut
            }
        }
        catch [System.Exception]
        {
            Write-Host "An error occured, check the global 'error' variable for more details."
        }
        finally
        {
            if($sp -ne $null -and $sp.IsOpen){

                #if the cmdlet changed the mode, set it back
                if([NoFuture.Gsm.Settings]::HaveSetTextMode){
                    $sp.WriteLine("AT+CMGF=$atCmdEntry")
                    [System.Threading.Thread]::Sleep($ms)
                }
                #clean up 
                $sp.Close();
                $sp.Dispose()
            }
            [NoFuture.Gsm.Settings]::HaveSetTextMode = $false
        }#end try\catch\finally
    }#end Process
}#end SendGsmMT-Sms

<#
    .SYNOPSIS
    Reads all SMS entries for the given storage type.
    
    .DESCRIPTION
    This cmdlet is built based on functionality as specified in 
    'ETSI TS 27.005 V11.0.0 (2012-10) 3.4.2 List Messages +CMGL'

    .PARAMETER PortName
    The Port in which the Mobile Equipment is currently connected.
    
    .PARAMETER GsmSmsTextMode
    One of constants listed in NoFuture.Gsm.SmsStatusTextMode

    .EXAMPLE

#>
function ListGsmMT-Sms
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string]$PortName,

        [Parameter(Mandatory=$false,position=1)]
        [string] $GsmSmsTextMode
    )
    Process
    {
        if([string]::IsNullOrWhiteSpace($GsmSmsTextMode)){
            $GsmSmsTextMode = [NoFuture.Gsm.SmsStatusTextMode]::RECEIVED_UNREAD;
        }

        #test caller specified values
        if(-not([NoFuture.Gsm.SmsStatusTextMode]::IsValid($GsmSmsTextMode))){
           throw "specified SMS Text Mode is invalid '$GsmSmsTextMode'" 
        }

        $sp = New-Object System.IO.Ports.SerialPort
        $sp.PortName = $PortName
        $sp.BaudRate = 9600
        $sp.RtsEnable = $true
        $sp.DtrEnable = $true
        $sp.NewLine = ([System.Convert]::ToChar([byte]0xD));
        try{

            $sp.Open()

            $ms = [NoFuture.Gsm.Settings]::ThreadSleepMs

            if(-not (IsGsmMT-ThisPortName -Port $sp)){throw ("the port-name '{0}' is not responding to a standard GSM AT command" -f $PortName)}

            [System.Threading.Thread]::Sleep($ms)

            #set MT to SMS text Mode
            $atCmdEntry = SetGsmMT-ToSmsTextMode -Port $sp

            [System.Threading.Thread]::Sleep($ms)

            #get all by type
            $atCmglCmd = "AT+CMGL=`"$GsmSmsTextMode`""
            $sp.WriteLine($atCmglCmd);

            #read response from Mobile Equipment
            [System.Threading.Thread]::Sleep($ms) #expect only about 20 entries per half-second, may need to increase this
            $spOut = $sp.ReadExisting()

            if((IsGsmMT-CmdOutputError $spOut)){ 
                throw ("an error occured while trying to send the SMS on the Mobile Equipment '$spOut'"); 
            }
            else{
                [NoFuture.Gsm.SmsSummary[]]$smsMessages = $null
                if(-not ([NoFuture.Gsm.SmsSummary]::TryParseList($spOut, [ref] $smsMessages))){
                    $smsMessages = $null
                }
            }
        }
        catch [System.Exception]
        {
            Write-Host "An error occured, check the global 'error' variable for more details."
        }
        finally
        {
            if($sp -ne $null -and $sp.IsOpen){

                #if the cmdlet changed the mode, set it back
                if([NoFuture.Gsm.Settings]::HaveSetTextMode){
                    $sp.WriteLine("AT+CMGF=$atCmdEntry")
                    [System.Threading.Thread]::Sleep($ms)
                }
                #clean up 
                $sp.Close();
                $sp.Dispose()
            }
            [NoFuture.Gsm.Settings]::HaveSetTextMode = $false
        }#end try\catch\finally

        return $smsMessages
    }#end Process
}#end ListGsmMT-Sms

<#
    .SYNOPSIS
    Fully reads an SMS entry on the Mobile Equipment whereby it is 
    marked as 'read' on the device.
    
    .DESCRIPTION
    This cmdlet is built based on functionality as specified in 
    'ETSI TS 27.005 V11.0.0 (2012-10) 3.4.3 Read Message +CMGR'

    .PARAMETER PortName
    The Port in which the Mobile Equipment is currently connected.
    
    .PARAMETER SmsIndex
    The index entry for the SMS on the Mobile Equipment.

    .EXAMPLE

#>
function ReadGsmMT-Sms
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string]$PortName,

        [Parameter(Mandatory=$true,position=1)]
        [int] $SmsIndex
    )
    Process
    {
        $sp = New-Object System.IO.Ports.SerialPort
        $sp.PortName = $PortName
        $sp.BaudRate = 9600
        $sp.RtsEnable = $true
        $sp.DtrEnable = $true
        $sp.NewLine = ([System.Convert]::ToChar([byte]0xD));
        try{

            $sp.Open()

            $ms = [NoFuture.Gsm.Settings]::ThreadSleepMs

            if(-not (IsGsmMT-ThisPortName -Port $sp)){throw ("the port-name '{0}' is not responding to a standard GSM AT command" -f $PortName)}

            [System.Threading.Thread]::Sleep($ms)

            #set MT to SMS text Mode
            $atCmdEntry = SetGsmMT-ToSmsTextMode -Port $sp

            #get all by type
            $atCmgrCmd = "AT+CMGR=$SmsIndex"
            $sp.WriteLine($atCmgrCmd);

            #read response from Mobile Equipment
            [System.Threading.Thread]::Sleep($ms)
            $spOut = $sp.ReadExisting()

            if((IsGsmMT-CmdOutputError $spOut)){ 
                throw ("an error occured while trying to send the SMS on the Mobile Equipment '$spOut'"); 
            }
            else{
                return $spOut
            }
        }
        catch [System.Exception]
        {
            Write-Host "An error occured, check the global 'error' variable for more details."
        }
        finally
        {
            if($sp -ne $null -and $sp.IsOpen){

                #if the cmdlet changed the mode, set it back
                if([NoFuture.Gsm.Settings]::HaveSetTextMode){
                    $sp.WriteLine("AT+CMGF=$atCmdEntry")
                    [System.Threading.Thread]::Sleep($ms)
                }
                #clean up 
                $sp.Close();
                $sp.Dispose()
            }
            [NoFuture.Gsm.Settings]::HaveSetTextMode = $false
        }#end try\catch\finally
    }#end Process
}#end ReadGsmMT-Sms

#'ETSI TS 27 005 V11.0.0 (2012-10)' section 3.2.3
function SetGsmMT-ToSmsTextMode
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [System.IO.Ports.SerialPort]$Port
    )
    Process
    {
        $sp = $Port
        $ms = [NoFuture.Gsm.Settings]::ThreadSleepMs

        #check current SMS Mode
        $sp.WriteLine("AT+CMGF?");
        [System.Threading.Thread]::Sleep($ms)
        $spOut = $sp.ReadExisting()

        if((IsGsmMT-CmdOutputError $spOut)) { throw "the Serial Port's response was not as expected '$spOut'"; }

        #switch to Text SMS Mode
        if($spOut -cmatch ([NoFuture.Gsm.Utility]::OK_RESPONSE_REGEX_PATTERN)){
            $sp.WriteLine("AT+CMGF=1")
            [System.Threading.Thread]::Sleep($ms)
            $spOut = $sp.ReadExisting();
            if((IsGsmMT-CmdOutputError $spOut)) { throw "could not set the SMS mode on the Mobile Equipment to Text Mode, '$spOut'"; }

            #set this only when its activly changed by the cmdlet
            [NoFuture.Gsm.Settings]::HaveSetTextMode = $true;
        }

        return $spOut;
    }
}#end SetGsmMT-ToSmsTextMode

function GetGsmMT-CurrentPhoneBook
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [System.IO.Ports.SerialPort]$Port
    )
    Process
    {
        $sp = $Port
        $ms = [NoFuture.Gsm.Settings]::ThreadSleepMs
        [NoFuture.Gsm.PhoneBook] $phBookOut = $null;

        #check current PhoneBook
        $sp.WriteLine("AT+CPBS?");
        [System.Threading.Thread]::Sleep($ms)
        $spOut = $sp.ReadExisting()

        if((IsGsmMT-CmdOutputError $spOut)) { throw "the Serial Port's response was not as expected '$spOut'"; }

        #attempt to parse current phone book into object
        if(-not ([NoFuture.Gsm.PhoneBook]::TryParse($spOut, [ref] $phBookOut))){

            throw "the response of 'AT+CPBS?' is not parsing to a valid GSM PhoneBook '$spOut'";
                 
        }#end AT data parsed to a PhoneBook

        return $phBookOut
    }
}#end GetGsmMT-CurrentPhoneBook

function SelectGsmMT-PhoneBook
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [System.IO.Ports.SerialPort]$Port,

        [Parameter(Mandatory=$true,position=1)]
        [string] $PhoneBookCode
    )
    Process
    {
        #validate the phone book code before bothering to do any of the rest
        if(-not ([NoFuture.Gsm.PhoneBook]::IsValidPhoneBookCode($PhoneBookCode))){
            throw "the phone book code provided is invalid '$PhoneBookCode'."
        }

        $sp = $Port
        $ms = [NoFuture.Gsm.Settings]::ThreadSleepMs
        [NoFuture.Gsm.PhoneBook] $phBookOriginalOut = GetGsmMT-CurrentPhoneBook -Port $Port;

        #if caller supplied same phonebook that MT is already assigned to then simply leave
        if($phBookOriginalOut.Storage -eq $PhoneBookCode){
            [NoFuture.Gsm.Settings]::HaveSetPhoneBook = $false
            return $phBookOriginalOut
        }

        #assign phone book storage 
        $sp.WriteLine("AT+CPBS=`"$PhoneBookCode`"");
        [System.Threading.Thread]::Sleep($ms)
        $spOut = $sp.ReadExisting()

        #check again for error
        if((IsGsmMT-CmdOutputError $spOut)) { 
            throw "upon assignment to phonebook '$PhoneBookCode', the Serial Port's response was not as expected '$spOut'"; 
        }

        #assign global flag
        [NoFuture.Gsm.Settings]::HaveSetPhoneBook = $true

        #get the newly assigned phonebook 
        [NoFuture.Gsm.PhoneBook] $phBookOut = GetGsmMT-CurrentPhoneBook -Port $Port;
        return $phBookOut
    }
}#end SelectGsmMT-PhoneBook

function IsGsmMT-CmdOutputError
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [string]$ReadExistingOutput
    )
    Process
    {
        return [NoFuture.Gsm.Utility]::IsCmdOutputError($ReadExistingOutput)
    }
}#end IsGsmMT-CmdOutputError

function IsGsmMT-ThisPortName
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [System.IO.Ports.SerialPort]$Port
    )
    Process
    {
        $sp = $Port
        $ms = [NoFuture.Gsm.Settings]::ThreadSleepMs

        #open the port if not open already
        if(-not $sp.IsOpen){$sp.Open();}

        #generic and standard GSM AT cmd
        $sp.WriteLine("AT+CGMI");
        [System.Threading.Thread]::Sleep($ms)
        $spOut = $sp.ReadExisting()
        if(IsGsmMT-CmdOutputError $spOut){return $false;}

        return $true;
    }
}#end IsGsmMT-ThisPortName

<#
Notes
#another way to filter outlook calendar
[System.TimeZoneInfo]::ConvertTimeFromUtc($cal.Events[0].Start.Value,[System.TimeZone]::CurrentTimeZone)
$calendar.Items.Restrict(("[Start] >= '{0}' AND [End] =< '{1}'" -f $StartingFrom.ToString("g"),$OutTo.ToString("g"))) | % {
#>