<#
Notes WMI on Powershell

PowerShell and WMI
By: Richard Siddaway; Foreword by Ed Wilson
Publisher: Manning Publications
Pub. Date: May 07, 2012
Print ISBN-10: 1-61729-011-4
Print ISBN-13: 978-1-61729-011-4Pages in Print Edition: 552
#>

<#
WMI structure
 - is an extension of 
   Distributed Management Task Force's Common
    Information Model (DMTF and CIM for short, respectively)
 - WMI is based on COM, not .NET
 - starts with MOF files at
 C:\Windows\System32\wbem
 - compiles into *.dll which are the providers
 - providers are registered with windows in 
 C:\Windows\System32\wbem\Repository

 - WMI is accessable remotely and this is a main feature
  - Get-WmiObject -ComputerName swithc takes an array of strings for such an effort
  - admin priv.'s are pretty much required for WMI
  - WMI remoting doesn't require Ps remoting
  - 'Winmgmt' service must be running for WMI access locally
  - Ps v3's WSMAN and CIM cmdlets require WinRM to be running on the remote box
  - Get-Credential cmdlet's results may be used as the value of the -Credential switch
#>

<#
Providers
 - not used directly
 - providers are divided into five types
  Type        Registration Type
  ----        -----------------
  Class       __ClassProviderRegistration
  Event       __EventProviderRegistration
  Event
   Consumer   __EventConsumerProviderRegistration
  Instance    __InstanceProviderRegistration
  Method      __MethodProviderRegistration
  Property    __PropertyProviderRegistration
 
 - Providers install classes into Namespaces, 
   which is the working part of WMI
#>
#prints all providers
Get-WmiObject -Class __Win32Provider | select name

<#
Namespaces
 - logical division of classes
 - powershell's default root namespace
  root\cimv2
 - the 'name' column of listed namespaces is 
   a class
 - namespaces contain providers 
  - these providers are __Win32Provider types
  - for example getting providers of a namespace looks as
  Get-WmiObject -Namespace "queryNamespace" -Class __Win32Provider

#>
#list WMI Namespaces
Get-WmiObject -Namespace 'root\cimv2' -List "__*"

<#
Registration
 - a provider registers itself with WMI within a 
   namespace
 - a provider can register events which can then be subscribed to
#>
#get a list of root level classes that handle registration of providers
# into a WMI namespace
Get-WmiObject -Namespace 'root\cimv2' -List "__*Registration"

#gets a sample, note in the output there is a __CLASS which is one of the reg.
# provider types and there is a 'provider' as well
Get-WmiObject -Namespace 'root\cimv2' -List "__*Registration" | % {
    Get-WmiObject -Namespace 'root\cimv2' -Class ($_.Name) | select -First 1}

#get a list of registered events
Get-WmiObject -Namespace 'root\cimv2' -Class __EventProviderRegistration | % {
    "`n";$_ | Format-Table EventQueryList, provider -Wrap}


<#
Classes
 - logically the lowest level of the WMI hierarchy
 - most important component since they facilitate actual tasks
 - represents an item on a system
  - optical drive, NIC, the OS, Hotfix, installed software
 - CIM defines three logical groups of classes
  - Core, represent that which applies to all areas
  - Common, apply to specific management areas
  - Extended, technology specific (e.g. IIS, DNS)
 - WMI classes are divided into two general groups
  - 1.) System classes that are present in each namespace
  - 2.) 'all the rest', being core CIM and extended Microsoft classes
 - Each WMI namespace has an instance of each of the System classes
 - the programmatic link between WMI providers and WMI classes 
   through WQL syntax, namely

   Get-WmiObject -Namespace "myTargetNamespace" 
    -Query "REFERENCES OF {__Win32Provider.Name='myTargetProvider'}" | % {
        $_.__CLASS
    }
#>

<#
Qualifiers
 - hidden properties concerning a WMI class
 - getting them slows performance
 - the 'Amended' switch is required on the Get-WmiObject cmdlet
  - having added this switch the info is contained in the 'Qualifiers'
    property
 - the key property is needed for use of WMI accelerator [wmi]
#>
#example of getting the Description property on networkadapter classes
Get-WmiObject -List Win32*networkadapter* | % {
    "`n$($_.Name)"
    ((Get-WmiObject -List $($_.Name) -Amended).Qualifiers | ? {
        $_.Name -eq "Description"}).Value
}

<#
Running WMI Methods 
 - Get-WmiObject will get the reference to a WMI class 
 - Invoke-WmiMethod is needed to enact methods on that class
  - this differs from the standard .NET 'dot-invoke'
#>
#this example gets the WMI eventlog class and invokes its 
# BackupEventLog method
Get-WmiObject -Class Win32_NTEventLogFile -Filter "LogFileName = 'Scripts'" | 
 Invoke-WmiMethod -Name BackupEventLog -ArgumentList (Join-Path $mypshome "temp\myLog.evt")


<#
WMI Query Language (WQL)
 - like SQL less UPDATE, DELETE, REPLACE and JOIN
 - general syntax as
 SELECT <property list>
 FROM <WMI class name>
 WHERE <one or more conditions>

 - WQL Logical Operators
 AND
 OR
 TRUE evals to -1
 FALSE evals to 0

 - WQL Comparison Operators (obvious)
 = 
 <
 >
 <=
 >=
 != or <>
 IS NULL
 LIKE

 - WQL Wildcard Characters
 _ (underscore) as any single character (PS\DOS eqiv. ?)
 % Zero or more characters (PS\DOS equiv. *)
 [a=z] any one character between 'a' and 'z'
 [abc] any one character being 'a', 'b' or 'c'
 [^a=z] 
 [^abc] are also valid for negation

 - WQL not otherwise specified
 ISA for id'ing by WMI type (class)
 *   used in SELECT statement meaning the same as it does in SQL


#>
#basic example
Get-WmiObject -Query "SELECT Name, Threadcount, UserModetime FROM Win32_Process WHERE Name = 'Powershell.exe'"
#likewise
Get-WmiObject -Class Win32_Process -Filter "Name='Powershell.exe'" -Property Name,Threadcount,UserModetime
#furthermore
Get-WmiObject -Class Win32_Process -Filter "Name='PowerShell.exe'" | Format-List Name, Threadcount, UserModetime

#logical operator example
Get-WmiObject -Query "SELECT * FROM Win32_Process WHERE Name ='PowerShell.exe' AND Handle='6036'"

#comparison operators - first one is better performance
Get-WmiObject -Query "SELECT Name, HandleCount FROM Win32_Process WHERE HandleCount>=550" | Format-Table Name, HandleCount -AutoSize
#likewise - second in performance
Get-WmiObject -Class Win32_Process -Filter "HandleCount>=550" | Format-Table Name, HandleCount -AutoSize
#futhermore - worst performance
Get-WmiObject -Class Win32_Process | ? {$_.HandleCount -ge 550} | Format-Table Name, HandleCount -AutoSize
#use of WQL NULL operator
Get-WmiObject -Query "SELECt * FROM Win32_CDRomDrive WHERE VolumeName IS NULL"

#WMI wildcard examples -case insensitive
Get-WmiObject -Class Win32_Process -Filter "Name LIKE 'p_w%'"
#in ps script 
Get-WmiObject -Class Win32_Process | ? {$_.Name -like 'P?W*'}

<#
WMI References and Associations
 - references are as the sound, other classes with a HAS-A property to the target
 - associators is the idea of an end-point in an object graph of references

 - Specific keywords in the WHERE statement pertinent to REFERENCES OF and ASSOCIATORS OF
 ClassDefsOnly           (returns the definition (i.e. type) instead of the instance)
 RequiredQualifier       (a kind-of filter concerning those ref's\assoc's must possess)
 ResultClass             (for references only, restricts to a single type)
 AssocClass              (for associations only, restricts to a single type)
 RequiredAssocQualifier  (for associations only, enhanced form of the latter) 
#>
#gets enabled Network adapters 
Get-WmiObject -Class Win32_NetworkAdapter | ? {$_.NetEnabled} | select name, deviceid
#references example
# the curly-braces define the WMI instance for which you need references
Get-WmiObject -Query "REFERENCES OF {Win32_NetworkAdapter.DeviceId='7'}" | % { "";$_.__CLASS;$($_.__PATH -split ",") }
#clearer results
Get-WmiObject -Query "REFERENCES OF {Win32_NetworkAdapter.DeviceId='7'} WHERE ClassDefsOnly" | Format-Table Name, Properties -AutoSize

#associators example
Get-WmiObject -Query "ASSOCIATORS OF {Win32_NetworkAdapter.DeviceId='7'} WHERE ClassDefsOnly" | select name

<#
WMI Events
 - WMI, .NET and Powershell herself are all registerable events
 - there is a cmdlet for each
 Register-EngineEvent for powershell
 Register-ObjectEvent for .NET
 Register-WmiEvent for WMI

 - the event object handed to the event handler has properties such as:
  $e.TimeGenerated
  $e.SourceIdentifier 
  $e.SourceEventArgs.NewEvent.TargetInstance
  $e.SourceEventArgs.NewEvent.PreviousInstance.Name

 - Basic WMI events are applicable to Registry, File system and processes
 __InstanceCreationEvent
 __InstanceDeletionEvent

 - Process specific events - these are used in a WQL FROM query same as the latter
 Win32_ProcessStartTrace
 Win32_ProcessStopTrace

 - File System specific, these are filtered in the WQL WHERE statement with a ISA test for:
 CIM_DirectoryContainsFile  (for use in file creation and deletion)
 CIM_DataFile               (for use in file modification, this monitors only on specific files)

 - Registry specific events, these are used in the WQL FROM part of the query
 RegistryKeyChangeEvent    (monitors on a single key not its subkeys)
 RegistryTreeChangeEvent   (monitors on a key and its subkeys)
 RegistryValueChangeEvent  (a single value of a specific key)
  
  ** HKCR & HKCU aren't applicable

#>
#register WMI event example
$myProc = "devenv.exe"
$wmiQry = (@"
SELECT * 
FROM __InstanceCreationEvent WITHIN 5 
WHERE TargetInstance ISA 'Win32_Process'
 AND TargetInstance.Name = '{0}'
"@ -f $myProc)

Register-WmiEvent -Query $wmiQry -SourceIdentifier $myProc -Action {
    Write-Host "Visual Studio started"
}

#file deleted or created WQL example
$addRmFilesQry = @"
SELECT * 
FROM __InstanceCreationEvent WITHIN 5
WHERE TargetInstance ISA 'CIM_DirectoryContainsFile' 
"@

#specific file modified WQL example
$myFileChanged = @"
SELECT * 
FROM __InstanceCreationEvent WITHIN 5
WHERE TargetInstance ISA 'CIM_DataFile' 
 AND TargetInstance.Name = 'C:\\Projects\\MyFile.txt'
"@

#registry tree changed (key and all subkeys) WQL example
$regTreeQry = @"
SELECT *
FROM RegistryTreeChangeEvent
WHERE Hive = 'HKEY_LOCAL_MACHINE' 
 AND RootPath = 'SOFTWARE\\PAW'
"@

#registry key's value changed WQL example
$regValQry = @"
SELECT *
FROM RegistryValueChangeEvent
WHERE Hive = 'HKEY_LOCAL_MACHINE' 
 AND KeyPath = 'SOFTWARE\\PAW'
 AND ValueName = 'somevalue'
"@

<#
Network general notes
#>
#get local machine's mapped drives
Get-WmiObject -Class Win32_NetworkConnection 

#get local machines network adapters - includes MAC address
Get-WmiObject -Class Win32_NetworkAdapter 

#get local machines network IPs, Default Gateway & DNSDomain
Get-WmiObject -Class Win32_NetworkAdapterConfiguration 

<#
IIS
 - not really WMI related
 - run 
 Import-Module WebAdministration
 
 - then check out the commands returned from 
 Get-Command -Module WebAdministration
#>

