try{
if(-not [NoFuture.MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.MyFunctions]::FunctionFiles.Add("List-Process",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-Sos",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-Mdbg",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-ProcessClrVersion",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-Symbols",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}

<#
	.SYNOPSIS
	Calls the 'tlist.exe' with the given args.
	
	.DESCRIPTION
	Calls the 'tlist.exe' with the given args.

    .PARAMETER PrintTaskTree
    Print Task Tree

   .PARAMETER ProcessId
    List module information for this task.

   .PARAMETER ProcessName
    Returns the PID of the process specified or -1
    if the specified process doesn't exist.  If there
    are multiple instances of the process running only
    the instance with the first PID value is returned.

   .PARAMETER SearchStringPattern
    The pattern can be a complete task
    name or a regular expression pattern
    to use as a match.  Tlist matches the
    supplied pattern against the task names
    and the window titles.

   .PARAMETER ModuleNamePattern
    Lists all tasks that have DLL modules loaded
    in them that match the given pattern name
 
   .PARAMETER ShowCommandLine
    Show command lines for each process
   
   .PARAMETER ShowSessionId
    Show session IDs for each process
   
   .PARAMETER ShowGroupAffinity
    Show group affinity for each process (Win7+)
   
   .PARAMETER ShowMTSpackages
    Show MTS packages active in each process.
   
   .PARAMETER ShowActiveServices
    Show services active in each process.
   
   .PARAMETER ShowAllInformation
    Show all process information
   
   .PARAMETER ShowWow64Info
    Show Wow64 process information

   .LINKS
   http://msdn.microsoft.com/en-us/library/windows/hardware/ff558903%28v=vs.85%29.aspx
#>
function List-Process
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,Position=0)]
        [int] $ProcessId,
        [Parameter(Mandatory=$false,Position=1)]
        [string] $ProcessName,
        [Parameter(Mandatory=$false,Position=2)]
        [string] $SearchStringPattern,
        [Parameter(Mandatory=$false,Position=3)]
        [string] $ModuleNamePattern,
        [Parameter(Mandatory=$false,Position=4)]
        [switch] $PrintTaskTree,
        [Parameter(Mandatory=$false,Position=5)]
        [switch] $ShowCommandLine,
        [Parameter(Mandatory=$false,Position=6)]
        [switch] $ShowSessionId,
        [Parameter(Mandatory=$false,Position=7)]
        [switch] $ShowGroupAffinity,
        [Parameter(Mandatory=$false,Position=8)]
        [switch] $ShowMTSpackages,
        [Parameter(Mandatory=$false,Position=9)]
        [switch] $ShowActiveServices,
        [Parameter(Mandatory=$false,Position=10)]
        [switch] $ShowWow64Info
    )
    Process
    {
        $tListPath = [NoFuture.X64Tools]::TList
        $cmd = ("& {0}" -f $tListPath)
$cmdLineHelp = @'
Copyright (c) Microsoft Corporation. All rights reserved.
usage: TLIST <<-m <pattern>> | <-t> | <pid> | <pattern> | <-p <processname>>> | <-k> | <-s>
           [options]:
           -t
              Print Task Tree
           <pid>
              List module information for this task.
           <pattern>
              The pattern can be a complete task
              name or a regular expression pattern
              to use as a match.  Tlist matches the
              supplied pattern against the task names
              and the window titles.
           -c
              Show command lines for each process
           -e
              Show session IDs for each process
           -g
              Show group affinity for each process (Win7+)
           -k
              Show MTS packages active in each process.
           -m <pattern>
              Lists all tasks that have DLL modules loaded
              in them that match the given pattern name
           -s
              Show services active in each process.
           -p <processname>
              Returns the PID of the process specified or -1
              if the specified process doesn't exist.  If there
              are multiple instances of the process running only
              the instance with the first PID value is returned.
           -v
              Show all process information
           -w
              Show Wow64 process information
'@


        #determine manner of call to tlist.exe
        if($ProcessId -ne 0)
        {
            $cmd = ("{0} {1}" -f $cmd,$ProcessId)
        }
        elseif(-not [System.String]::IsNullOrWhiteSpace($ProcessName))
        {
            $cmd = ("{0} {1} '{2}'" -f $cmd,"-p",$ProcessName)
        }
        elseif(-not [System.String]::IsNullOrWhiteSpace($SearchStringPattern))
        {
            $cmd = ("{0} '{1}'" -f $cmd,$SearchStringPattern)
        }
        elseif(-not [System.String]::IsNullOrWhiteSpace($ModuleNamePattern))
        {
            $cmd = ("{0} {1} '{2}'" -f $cmd,"-m",$ModuleNamePattern)
        }
        
        #handle switchs
        if($ShowCommandLine)
        {
            $cmd = ("{0} {1}" -f $cmd,"-c")
        }

        if($PrintTaskTree)
        {
            $cmd = ("{0} {1}" -f $cmd,"-t")
        }

        if($ShowSessionId)
        {
            $cmd = ("{0} {1}" -f $cmd,"-e")
        }

        if($ShowGroupAffinity)
        {
            $cmd = ("{0} {1}" -f $cmd,"-g")
        }

        if($ShowMTSpackages)
        {
            $cmd = ("{0} {1}" -f $cmd,"-k")
        }

        if($ShowActiveServices)
        {
            $cmd = ("{0} {1}" -f $cmd,"-t")
        }

        if($ShowWow64Info)
        {
            $cmd = ("{0} {1}" -f $cmd,"-w")
        }

        if($Verbose)
        {
            $cmd = ("{0} {1}" -f $cmd,"-v")
        }

        #TODO parse this out into a psobject
        $tlistOutput = Invoke-Expression -Command $cmd

        return $tlistOutput
    }
}

<#
    .SYNOPSIS
    Invokes 'Mdbg.exe', sends each command and returns
    the running process with additional PS methods.

    .DESCRIPTION
	Process is dependent on creation of a seperate process
    to run the Mdbg.exe and send each command over the standard input.

    A check is made against the PID submitted to validate its a running, local process.

    Start-ProcessRedirect is used meaning the actually Mdbg.exe is hidden
    an aero console window acts as the output, the caller receives a ref to the 
    running Mdbg proc and, in addition, the output is stored to a PS Property 
    named 'DataLines'.  
    
    Furthermore, the Mdbg.exe is expected as the global location of x64tools.Mdbg

    Last, the caller needs to call the dynamic method 'Stop' RATHER THAN calling Close
    or Dispose.

    .PARAMETER ProcessId
    The running, local process upon which to attach Mdbg.

    .PARAMETER MdbgCommands
    An array of valid Mdbg commands, see the enclosed link for more info.  
    Do not include a call to 'a <PID>' since that is performed by the script 
    itself.  Furthermore, this array is optional and the caller may execute
    as many commands as needed on the returned Process instance.

    .EXAMPLE
	$myMdbgProc = Get-Mdbg -ProcessId  1792 -MdbgCommands @("a ","l appdomains","detach")

    this example will print the AppDomains loaded into target process 1792

    .EXAMPLE
    $myMdbgProc = Get-Mdbg -ProcessId 2588 -MdbgCommands @("w -c all 2","detach")

    this example will print the stack-trace for all frames 
    pertinent to managed thread #2 on process id'ed as 2588

    .EXAMPLE
    $myMdbgProc = Get-Mdbg -ProcessId 2588 -MdbgCommands @("thread","detach") 

    this example will print all managed threads current 
    to process id'ed as 2588, this output will, in turn, be handled
    by the function MyMdbgHandler

    .RETURNS
    System.Diagnostics.Process

    .LINK
    http://msdn.microsoft.com/en-us/library/ms229861.aspx
#>
function Get-Mdbg
{
	[CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [int] $ProcessId,
        [Parameter(Mandatory=$false,position=1)]
        [array] $MdbgCommands

    )
    Process
    {
        #validate inputs
        #if($MdbgCommands.Length -eq 0) {Write-Host "no commands entered..."; break;}
        if(-not (Test-Path ([NoFuture.X64Tools]::Mdbg))){throw ("Mdbg is not at the expected location of {0}" -f ([NoFuture.X64Tools]::Mdbg))}

        #start the proc async
        $mdbgProc =  Start-ProcessRedirect -ExePath ([NoFuture.X64Tools]::Mdbg)

        #attach to the target
        $mdbgProc.StandardInput.WriteLine(("a {0}" -f $ProcessId))

        #run the commands if any
        $MdbgCommands | % {
            $mdbgProc.StandardInput.WriteLine($_)
        }

        #helper method
        $mdbgProc | Add-Member ScriptMethod RunCmd {
            $this.StandardInput.WriteLine($args[0])
        }

        #return the running process 
        return $mdbgProc
    }
}

<#
    .SYNOPSIS
    Calls Cdb.Exe targeting the given process, loads sos.dll, saves stdout to 
    the dynamic 'DataLines' property and, in addition, displays stdout to an independent
    console.
    
    .DESCRIPTION
    Cdb.exe is expected at the global variable $global:x[64|86]tools.Cdb.  
    
    Receiving an 0x00..BB error indicates the cmdlet should be run again having
    the Use64bit switch opposite whatever it was when the error was received.

    SOS.dll allows for a special access into the .NET runtime. Unlike type 
    definitions, which may be inspected by Reflection, SOS.dll allows for 
    inspection of live threads, object instances (including their size) and 
    memory layout.

    Last, the caller needs to call the dynamic method 'Stop' RATHER THAN calling Close
    or Dispose.

    (See the enclosed links for detaching, breaking, continuing and quiting debug.)
         
    .PARAMETER ProcessId
    The PID of a running, local process; this is the target of the cdb/sos commands.
    
    .PARAMETER CdbCommands
    Any valid commands which could be used by cdb or sos.dll.  This should not include
    any attach commands nor 'loadby sos clr' since both are handled internaly by the 
    script.
    
    .PARAMETER Use64bit
    Indicates to use the 64-bit Cdb.Exe version.

    .EXAMPLE 
    PS C:\> $w3wp = Get-Sos -ProcessId 10192 -Use64bit               #attach to a process
    PS C:\> $w3wp.RunCmd("!DumpHeap -type NhSessionFactory")         #need to move from Type paradigm to addressOf
    PS C:\> $dataContextMT = $w3wp.Data[266].Split(" ")[1].Trim()    #get a Method Table address from output
    PS C:\> $w3wp.RunCmd(("!DumpMT -MD {0}" -f $dataContextMT))      #get contents of Method Table
    PS C:\> $md = $w3wp.Data[370].Split(" ")[1].Trim()               #scope in on a Method Descriptor
    PS C:\> $w3wp.RunCmd(("!BPMD -md {0}" -f $md))                   #apply a break point at the scoped-in MD
    PS C:\> $w3wp.RunCmd("g")                                        #set the proc back into motion
    PS C:\> #[debugger breaks]
    PS C:\> $w3wp.RunCmd("~#e !CLRStack -l")                         #print all the local variables on the stack of the arrested thread
    PS C:\> $w3wp.RunCmd("~#e !CLRStack -p")                         #same, only all parameters instead
    PS C:\> $usernameAddr = $w3wp.Data[622].Split("=")[1].Trim()     #find a local target
    PS C:\> $w3wp.RunCmd(("dc {0}" -f $usernameAddr))                #print its value
    PS C:\> $httpContextAddr = $w3wp.Data[753].Split("=")[1].Trim()  #poke around at some other objects
    PS C:\> $w3wp.RunCmd(("!DumpObj {0}" -f $httpContextAddr))
    PS C:\> $requestorAddr = $w3wp.Data[638].Split("=")[1].Trim()
    PS C:\> $w3wp.RunCmd(("!DumpObj {0}" -f $requestorAddr))
    PS C:\> $w3wp.RunCmd("!EEVersion") #.NET version info
    PS C:\> $w3wp.RunCmd("!BPMD -clearall")                          #clear the break points
    PS C:\> $w3wp.RunCmd("qd")                                       #quit debugging
    PS C:\> $w3wp.Stop()                                             #close down the cmdlet

    .LINK
    For more on SOS.dll 
    http://msdn.microsoft.com/en-us/library/bb190764.aspx
    
    For more on Cdb.exe 
    http://msdn.microsoft.com/en-us/library/windows/hardware/ff539058(v=vs.85).aspx
    
    Info on 32 vs 64 bit debugging 
    http://msdn.microsoft.com/en-us/library/windows/hardware/ff539099(v=vs.85).aspx
    
    Info on Cdb meta commands
    http://msdn.microsoft.com/en-us/library/windows/hardware/ff552199(v=vs.85).aspx

    .OUTPUTS
    System.Diagnostics.Process
    
#>
function Get-Sos
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [int] $ProcessId,
        [Parameter(Mandatory=$false,position=1)]
        [array] $CdbCommands,
        [Parameter(Mandatory=$false,position=2)]
        [switch] $Use64bit
    )
    Process
    {
        
        #resolve path to command-line debugger
        if($Use64bit)
        {
            $cdb = [NoFuture.X64Tools]::Cdb
        }
        else
        {
            $cdb = [NoFuture.X86Tools]::Cdb
        }

        #validate the input
        if(-not (Test-Path $cdb)){throw ("cdb.exe not found at '{0}'" -f $cdb);}
        $targetProc = Get-Process -Id $ProcessId
        if($targetProc -eq $null) {throw ("no matching process for PID {0}" -f $ProcessId)}
 
        #-noinh disables handle inheritance for created processes
        #-c "<command>" executes the given debugger command at the first debugger prompt
        $cdbArgs = ("-p {0} -noinh -c `"{1}`"" -f $targetProc.ID, ".loadby sos mscorwks; .loadby sos clr")

        #start cdb.exe with sos.dll loaded (Start-ProcessRedirect wires up StdOut to the dynamic mehtods)           
        $cdbProc = Start-ProcessRedirect -ExePath $cdb -StartInfoArgs $cdbArgs

        #run any commands caller might have specified
        $CdbCommands | % {
            $cdbProc.StandardInput.WriteLine($_)    
        }

        #helper method
        $cdbProc | Add-Member ScriptMethod RunCmd {
            $this.StandardInput.WriteLine($args[0])
        }

        #return the running proc of cdb.exe
        return $cdbProc
    }
}

<#
    .SYNOPSIS
    Calls 'clrver.exe' with the given args.

    .DESCRIPTION
    Calls 'clrver.exe' which may be used to determine 
    the .NET runtime version for a currently active 
    process.  See /? on the exe for more.
    If called with no options, clrver will 
    display all installed CLR versions.

    .PARAMETER ProcessId
    Integer id of a running process on this host machine.

    .PARAMETER ShowAll 
    Display all processes and their respective runtime.

    .EXAMPLE
    $MyOutput = Get-ProcessClrVersion -ProcessId 2356

    .EXAMPLE
    $MyOutput = Get-ProcessClrVersion -ShowAll

    .OUTPUT
    Array
#>
function Get-ProcessClrVersion
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,Position=0)]
        [int] $ProcessId,
        [Parameter(Mandatory=$false,Position=1)]
        [switch] $ShowAll
    )
    Process
    {
        $clrVerPath = [NoFuture.X64Tools]::ClrVer
        $cmd = ("& {0}" -f $clrVerPath)
        if($ProcessId -gt 0)
        {
            $cmd = ("{0} {1}" -f $cmd,$ProcessId)
        }
        elseif($ShowAll)
        {
            $cmd = ("{0} {1}" -f $cmd,"-all")
        }

        Invoke-Expression -Command $cmd

    }
}


<#
    .SYNOPSIS
    Calls 'symChk.exe' for highest .NET and SSMS versions.

    .DESCRIPTION
    Sets the _NT_SYMBOL_PATH environment variable to 
     "symsrv*symsrv.dll*c:\symbols*http://msdl.microsoft.com/download/symbols"
    at the Machine level.  Then calls SymChk.exe on the highest
    installed version of .NET present on the box, the highest version
    of Sql Server Management Studio and, lastly, gets the symbols for the
    native Win32 dlls present in WINDOWS32 directory.

    .OUTPUT
    null
#>
function Get-Symbols
{
    [CmdletBinding()]
    Param( )
    Process
    {
        
        $symSrvExe = [NoFuture.X64Tools]::SymChk

        $localSymbolsFolder = [NoFuture.Shared.Constants]::SymbolsPath

        if(-not(Test-Path -Path $localSymbolsFolder))
        {
            #create if not present
            mkdir $localSymbolsFolder -Force
        }

        #add environment variable used to resolve PDB (symbol) files
        if($env:_NT_SYMBOL_PATH -eq $null)
        {
            [Environment]::SetEnvironmentVariable("_NT_SYMBOL_PATH","symsrv*symsrv.dll*$localSymbolsFolder*http://msdl.microsoft.com/download/symbols","Machine")
        }

        Write-Progress -Activity "Inspecting this machine" -Status "OK" -PercentComplete 16

        #set array of possiable path resolutions
        $dotNetPaths = @("C:\WINDOWS\Microsoft.NET\Framework","C:\Windows\Microsoft.NET\Framework64")
        $dotNetVersions = @("v4.0.30319","v3.5","v3.0","v2.0.50727","v1.1.4322","v1.0.3705")
        $ssmsVersions = @("120","110","100","90","80")
        $ssmsPath = "C:\Program Files (x86)\Microsoft SQL Server"

        #setup a psobject to hold various info together
        $symSrv = [psobject] "PsSymSrv"
        $symSrv | Add-Member NoteProperty Arch $env:PROCESSOR_ARCHITECTURE
        $symSrv | Add-Member NoteProperty SymbolsPath $localSymbolsFolder
        $symSrv | Add-Member -MemberType ScriptMethod DotNetPath {
            if($this.Arch -eq "AMD64" -and (Test-Path ($dotNetPaths[1])))
            {
                return $dotNetPaths[1]
            }
            elseif(Test-Path ($dotNetPaths[0]))
            {
                return $dotNetPaths[0]
            }
            else
            {
                return $null;
            }
    
        }
        $symSrv | Add-Member -MemberType ScriptMethod HighestDotNetPath {
            $dotNetVersions | % {
                $testPath = (Join-Path $this.DotNetPath $_)
                if(Test-Path $testPath)
                {
                    return $testPath
                }
            }
        }
        $symSrv | Add-Member -MemberType ScriptMethod HighestSsmsPath {
            $ssmsVersions | % {
                $testPath = (Join-Path $ssmsPath $_)
                if(Test-Path $testPath)
                {
                    return $testPath
                }
            }
        }

        Write-Progress -Activity "Adding symbols for SSMS" -Status "OK" -PercentComplete 42
        if($symSrv.HighestSsmsPath -ne $null -and (Test-Path $symSrv.HighestSsmsPath))
        {
            . $symSrvExe /r $symSrv.HighestSsmsPath
        }
        Write-Progress -Activity "Adding symbols for .NET" -Status "OK" -PercentComplete 66
        if($symSrv.HighestDotNetPath -ne $null -and (Test-Path $symSrv.HighestDotNetPath))
        {
            . $symSrvExe /r $symSrv.HighestDotNetPath
        }
        Write-Progress -Activity "Adding symbols for Win32" -Status "OK" -PercentComplete 86
        if(Test-Path (Join-Path $env:SystemRoot "system32"))
        {
            . $symSrvExe /r (Join-Path $env:SystemRoot "system32")
        }

        Write-Progress -Activity "Done" -Status "OK" -PercentComplete 99
    }

}


<#
Examples, using cmd line tools

SymChk command-line options
http://msdn.microsoft.com/en-us/library/windows/hardware/ff558845(v=vs.85).aspx
 
 symchk /r C:\Windows\Microsoft.NET\Framework64\v4.0.30319 /s SRV*c:\symbols\*http://msdl.microsoft.com/download/symbols
 
 symsrv*symsrv.dll*C:\Projects\Symbols\MicrosoftPublicSymbols*http://msdl.microsoft.com/download/symbols
 
 lm v clr
 
 .load C:\Windows\Microsoft.NET\Framework64\v4.0.30319\sos.dll
 !wow64exts.sw 
 
 HKLM\SOFTWARE\Wow6432Node 
 
 Entries here show up in VS > References window
 HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx
'@
#>
#netsh trace show CaptureFilterHelp
<#
  Capture Filters: 
	Capture filters are only supported when capture is explicitly 
	enabled with capture=yes. Supported capture filters are: 

	CaptureInterface=<interface name or GUID> 
	 Enables packet capture for the specified interface name or GUID. Use 
	 'netsh trace show interfaces' to list available interfaces. 
	e.g. CaptureInterface={716A7812-4AEE-4545-9D00-C10EFD223551} 
	e.g. CaptureInterface=!{716A7812-4AEE-4545-9D00-C10EFD223551} 
	e.g. CaptureInterface="Local Area Connection" 

	Ethernet.Address=<MAC address> 
	 Matches the specified filter against both source and destination 
	 MAC addresses. 
	e.g. Ethernet.Address=00-0D-56-1F-73-64 

	Ethernet.SourceAddress=<MAC address> 
	 Matches the specified filter against source MAC addresses. 
	e.g. Ethernet.SourceAddress=00-0D-56-1F-73-64 

	Ethernet.DestinationAddress=<MAC address> 
	 Matches the specified filter against destination MAC addresses. 
	e.g. Ethernet.DestinationAddress=00-0D-56-1F-73-64 

	Ethernet.Type=<ethertype> 
	 Matches the specified filter against the MAC ethertype. 
	e.g. Ethernet.Type=IPv4 
	e.g. Ethernet.Type=NOT(0x86DD) 
	e.g. Ethernet.Type=(IPv4,IPv6) 

	Wifi.Type=<Management|Data> 
	 Matches the specified filter against the Wifi type. Allowed values 
	 are 'Management' and 'Data'. If not specified, the Wifi.Type filter 
	 is not applied. 
	 Note: This capture filter does not support ranges, lists or negation. 
	e.g. Wifi.Type=Management 

	Protocol=<protocol> 
	 Matches the specified filter against the IP protocol. 
	e.g. Protocol=6 
	e.g. Protocol=!(TCP,UDP) 
	e.g. Protocol=(4-10) 

	IPv4.Address=<IPv4 address> 
	 Matches the specified filter against both source and destination 
	 IPv4 addresses. 
	e.g. IPv4.Address=157.59.136.1 
	e.g. IPv4.Address=!(157.59.136.1) 
	e.g. IPv4.Address=(157.59.136.1,157.59.136.11) 

	IPv4.SourceAddress=<IPv4 address> 
	 Matches the specified filter against source IPv4 addresses. 
	e.g. IPv4.SourceAddress=157.59.136.1 

	IPv4.DestinationAddress=<IPv4 address> 
	 Matches the specified filter against destination IPv4 addresses. 
	e.g. IPv4.DestinationAddress=157.59.136.1 

	IPv6.Address=<IPv6 address> 
	 Matches the specified filter against both source and destination 
	 IPv6 addresses. 
	e.g. IPv6.Address=fe80::5038:3c4:35de:f4c3\%8 
	e.g. IPv6.Address=!(fe80::5038:3c4:35de:f4c3\%8) 

	IPv6.SourceAddress=<IPv6 address> 
	 Matches the specified filter against source IPv6 addresses. 
	e.g. IPv6.SourceAddress=fe80::5038:3c4:35de:f4c3\%8 

	IPv6.DestinationAddress=<IPv6 address> 
	 Matches the specified filter against destination IPv6 addresses. 
	e.g. IPv6.DestinationAddress=fe80::5038:3c4:35de:f4c3\%8 

	CustomMac=<type(offset,value)> 
	 Matches the specified filter against the value at the specified 
	 offset starting with the MAC header. 
	 Note: This capture filter does not support ranges, lists or negation. 
	e.g. CustomMac=UINT8(0x1,0x23)
	e.g. CustomMac=ASCIISTRING(3,test)
	e.g. CustomMac=UNICODESTRING(2,test)

	CustomIp=<type(offset,value)> 
	 Matches the specified filter against the value at the specified 
	 offset starting with the IP header. 
	 Note: This capture filter does not support ranges, lists or negation. 
	e.g. CustomIp=UINT16(4,0x3201)
	e.g. CustomIp=UINT32(0x2,18932)

	CaptureMultiLayer=<yes|no> 
	 Enables multi-layer packet capture. 
	 Note: This capture filter does not support ranges, lists or negation. 

	PacketTruncateBytes=<value> 
	 Captures only the the specified number of bytes of each packet. 
	 Note: This capture filter does not support ranges, lists or negation. 
	e.g. PacketTruncateBytes=40 

Note: 
	Multiple filters may be used together. However the same filter may 
	not be repeated. 
	e.g. 'netsh trace start capture=yes Ethernet.Type=IPv4 
	      IPv4.Address=157.59.136.1' 
 
	Filters need to be explicitly stated when required. If a filter is 
	not specified, it is treated as "don't-care". 
	 e.g. 'netsh trace start capture=yes IPv4.SourceAddress=157.59.136.1' 
	      This will capture IPv4 packets only from 157.59.136.1, and it 
	      will also capture packets with non-IPv4 Ethernet Types, since 
	      the Ethernet.Type filter is not explicitly specified. 
	 e.g. 'netsh trace start capture=yes IPv4.SourceAddress=157.59.136.1 
	       Ethernet.Type=IPv4' 
	      This will capture IPv4 packets only from 157.59.136.1. Packets 
	      with other Ethernet Types will be discarded since an explicit 
	      filter has been specified. 
 
	Capture filters support ranges, lists and negation (unless stated 
	otherwise). 
	 e.g. Range: 'netsh trace start capture=yes Ethernet.Type=IPv4 
	              Protocol=(4-10)' 
	      This will capture IPv4 packets with protocols between 4 and 10 
	      inclusive. 
	 e.g. List: 'netsh trace start capture=yes Ethernet.Type=(IPv4,IPv6)' 
	      This will capture only IPv4 and IPv6 packets. 
	 e.g. Negation: 'netsh trace start capture=yes Ethernet.Type=!IPv4' 
	      This will capture all non-IPv4 packets. 
 
	Negation may be combined with lists in some cases. 
	 e.g. 'netsh trace start capture=yes Ethernet.Type=!(IPv4,IPv6)' 
	       This will capture all non-IPv4 and non-IPv6 packets. 
 
	'NOT' can be used instead of '!' to indicate negation. This requires 
	parentheses to be present around the values to be negated. 
	 e.g. 'netsh trace start capture=yes Ethernet.Type=NOT(IPv4)' 


#>

<#
netsh trace start /?

start
  Starts tracing.

  Usage: trace start [[scenario=]<scenario1,scenario2>] 
	[[globalKeywords=]keywords] [[globalLevel=]level]
	[[capture=]yes|no] [[report=]yes|no]
	[[persistent=]yes|no] [[traceFile=]path\filename] 
	[[maxSize=]filemaxsize] [[fileMode=]single|circular|append] 
	[[overwrite=]yes|no] [[correlation=]yes|no|disabled] [capturefilters] 
	[[provider=]providerIdOrName] [[keywords=]keywordMaskOrSet]  
	[[level=]level] [[provider=]provider2IdOrName] 
	[[keywords=]keyword2MaskOrSet] [[level=]level2] ... 
#>
#netsh trace start InternetClient provider=Microsoft-Windows-TCPIP level=4 keywords=ut:ReceivePath,ut:SendPath

#netsh trace stop

#netsh trace convert C:\Users\brian.s.starrett\AppData\Local\Temp\NetTraces\NetTrace.etl dump=XML report=yes overwrite=yes