try{
if(-not [NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Get-OutlookEmail",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Export-OutlookAppointments", $MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Send-GsmMTSms", $MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Read-GsmMTSms", $MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("List-GsmMTSms", $MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("List-GsmMtPhoneBook", $MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Add-GsmMtPhoneBookEntry", $MyInvocation.MyCommand)
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
        if([System.String]::IsNullOrWhiteSpace($Path)){$Path = ([NoFuture.Shared.NfConfig+TempDirectories]::Calendar)}

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
function List-GsmMtPhoneBook
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

            if(-not (Test-GsmMTThisPortName -Port $sp)){throw ("the port-name '{0}' is not responding to a standard GSM AT command" -f $PortName)}

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
                $atEntryPhoneBook =  Get-GsmMTCurrentPhoneBook -Port $sp;

                #get the current
                $assignedPhoneBook = Select-GsmMTPhoneBook -Port $sp -PhoneBookCode $PhoneBookCode
            }
            else {
                
                #having not specified a PhoneBook, just get current
                $assignedPhoneBook =  Get-GsmMTCurrentPhoneBook -Port $sp;
            }

            #check that the phone book has any entries at all
            if($assignedPhoneBook -ne $null -and $assignedPhoneBook.BlocksUsed -gt 0)
            {
                $sp.WriteLine(("AT+CPBR=1,{0}"-f $assignedPhoneBook.BlocksUsed))

                #only about 20 lines will be written to the buffer per half-a-second
                $contactsWaitTime = ($assignedPhoneBook.BlocksUsed % 20) * 500
                [System.Threading.Thread]::Sleep($contactsWaitTime)

                $spOut = $sp.ReadExisting()

                if((Test-GsmMTCmdOutputError $spOut)){ 
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
                    $atEntryPhoneBook = Select-GsmMTPhoneBook -Port $sp -PhoneBookCode $atEntryPhoneBook.Storage
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
}#end List-GsmMtPhoneBook

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
function Add-GsmMtPhoneBookEntry
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

            if(-not (Test-GsmMTThisPortName -Port $sp)){throw ("the port-name '{0}' is not responding to a standard GSM AT command" -f $PortName)}

            #get next index from current phone book
            $currentPhoneBook = Get-GsmMTCurrentPhoneBook -Port $sp

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

            if((Test-GsmMTCmdOutputError $spOut)){ 
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
function Send-GsmMTSms
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

            if(-not (Test-GsmMTThisPortName -Port $sp)){throw ("the port-name '{0}' is not responding to a standard GSM AT command" -f $PortName)}

            [System.Threading.Thread]::Sleep($ms)

            #set MT to SMS text Mode
            $atCmdEntry = Set-GsmMTToSmsTextMode -Port $sp

            #send SMS
            $smsMessage = [NoFuture.Gsm.Utility]::EncodeSmsMessage($PhoneNumber, $Message);

            [System.Threading.Thread]::Sleep($ms)

            $sp.WriteLine($smsMessage);

            #read response from Mobile Equipment
            [System.Threading.Thread]::Sleep($ms)
            $spOut = $sp.ReadExisting()

            if((Test-GsmMTCmdOutputError $spOut)){ 
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
}#end Send-GsmMTSms

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
function List-GsmMTSms
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

            if(-not (Test-GsmMTThisPortName -Port $sp)){throw ("the port-name '{0}' is not responding to a standard GSM AT command" -f $PortName)}

            [System.Threading.Thread]::Sleep($ms)

            #set MT to SMS text Mode
            $atCmdEntry = Set-GsmMTToSmsTextMode -Port $sp

            [System.Threading.Thread]::Sleep($ms)

            #get all by type
            $atCmglCmd = "AT+CMGL=`"$GsmSmsTextMode`""
            $sp.WriteLine($atCmglCmd);

            #read response from Mobile Equipment
            [System.Threading.Thread]::Sleep($ms) #expect only about 20 entries per half-second, may need to increase this
            $spOut = $sp.ReadExisting()

            if((Test-GsmMTCmdOutputError $spOut)){ 
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
}#end List-GsmMTSms

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
function Read-GsmMTSms
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

            if(-not (Test-GsmMTThisPortName -Port $sp)){throw ("the port-name '{0}' is not responding to a standard GSM AT command" -f $PortName)}

            [System.Threading.Thread]::Sleep($ms)

            #set MT to SMS text Mode
            $atCmdEntry = Set-GsmMTToSmsTextMode -Port $sp

            #get all by type
            $atCmgrCmd = "AT+CMGR=$SmsIndex"
            $sp.WriteLine($atCmgrCmd);

            #read response from Mobile Equipment
            [System.Threading.Thread]::Sleep($ms)
            $spOut = $sp.ReadExisting()

            if((Test-GsmMTCmdOutputError $spOut)){ 
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
}#end Read-GsmMTSms

#'ETSI TS 27 005 V11.0.0 (2012-10)' section 3.2.3
function Set-GsmMTToSmsTextMode
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

        if((Test-GsmMTCmdOutputError $spOut)) { throw "the Serial Port's response was not as expected '$spOut'"; }

        #switch to Text SMS Mode
        if($spOut -cmatch ([NoFuture.Gsm.Utility]::OK_RESPONSE_REGEX_PATTERN)){
            $sp.WriteLine("AT+CMGF=1")
            [System.Threading.Thread]::Sleep($ms)
            $spOut = $sp.ReadExisting();
            if((Test-GsmMTCmdOutputError $spOut)) { throw "could not set the SMS mode on the Mobile Equipment to Text Mode, '$spOut'"; }

            #set this only when its activly changed by the cmdlet
            [NoFuture.Gsm.Settings]::HaveSetTextMode = $true;
        }

        return $spOut;
    }
}#end Set-GsmMTToSmsTextMode

function Get-GsmMTCurrentPhoneBook
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

        if((Test-GsmMTCmdOutputError $spOut)) { throw "the Serial Port's response was not as expected '$spOut'"; }

        #attempt to parse current phone book into object
        if(-not ([NoFuture.Gsm.PhoneBook]::TryParse($spOut, [ref] $phBookOut))){

            throw "the response of 'AT+CPBS?' is not parsing to a valid GSM PhoneBook '$spOut'";
                 
        }#end AT data parsed to a PhoneBook

        return $phBookOut
    }
}#end Get-GsmMTCurrentPhoneBook

function Select-GsmMTPhoneBook
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
        [NoFuture.Gsm.PhoneBook] $phBookOriginalOut = Get-GsmMTCurrentPhoneBook -Port $Port;

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
        if((Test-GsmMTCmdOutputError $spOut)) { 
            throw "upon assignment to phonebook '$PhoneBookCode', the Serial Port's response was not as expected '$spOut'"; 
        }

        #assign global flag
        [NoFuture.Gsm.Settings]::HaveSetPhoneBook = $true

        #get the newly assigned phonebook 
        [NoFuture.Gsm.PhoneBook] $phBookOut = Get-GsmMTCurrentPhoneBook -Port $Port;
        return $phBookOut
    }
}#end Select-GsmMTPhoneBook

function Test-GsmMTCmdOutputError
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
}#end Test-GsmMTCmdOutputError

function Test-GsmMTThisPortName
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
        if(Test-GsmMTCmdOutputError $spOut){return $false;}

        return $true;
    }
}#end Test-GsmMTThisPortName

<#
Notes
#another way to filter outlook calendar
[System.TimeZoneInfo]::ConvertTimeFromUtc($cal.Events[0].Start.Value,[System.TimeZone]::CurrentTimeZone)
$calendar.Items.Restrict(("[Start] >= '{0}' AND [End] =< '{1}'" -f $StartingFrom.ToString("g"),$OutTo.ToString("g"))) | % {
#>