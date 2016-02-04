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

   .LINK
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
        $tListPath = [NoFuture.Tools.X64]::TList
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
        if(-not (Test-Path ([NoFuture.Tools.X64]::Mdbg))){throw ("Mdbg is not at the expected location of {0}" -f ([NoFuture.Tools.X64]::Mdbg))}

        #start the proc async
        $mdbgProc =  Start-ProcessRedirect -ExePath ([NoFuture.Tools.X64]::Mdbg)

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
        $hasSymbolPath = [System.Environment]::GetEnvironmentVariable("_NT_SYMBOL_PATH");
        if([string]::IsNullOrWhiteSpace($hasSymbolPath)){
            Write-Host "You first need to set the environment variable for '_NT_SYMBOL_PATH' - see Get-Symbols cmdlet"
            break;
        }
        
        #resolve path to command-line debugger
        if($Use64bit)
        {
            $cdb = [NoFuture.Tools.X64]::Cdb
        }
        else
        {
            $cdb = [NoFuture.Tools.X86]::Cdb
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
        $clrVerPath = [NoFuture.Tools.X64]::ClrVer
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
    Param
    ( 
        [Parameter(Mandatory=$false,Position=0)]
        [string] $GetSymbolsForPath
    )
    Process
    {
        
        $symSrvExe = [NoFuture.Tools.X64]::SymChk

        if([string]::IsNullOrWhiteSpace($symSrvExe) -or -not (Test-Path $symSrvExe)){
            Write-Host "The global NoFuture.Tools.X64.SymChk is unassigned."
            break;
        }

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

        if([string]::IsNullOrWhiteSpace($GetSymbolsForPath) -or -not (Test-Path $GetSymbolsForPath))
        {
            return;
        }

        Write-Progress -Activity "Adding symbols for $GetSymbolsForPath" -Status "OK" -PercentComplete 42
        iex  "& $symSrvExe /r $GetSymbolsForPath"

        Write-Progress -Activity "Done" -Status "OK" -PercentComplete 99
    }

}
