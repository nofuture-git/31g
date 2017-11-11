try{
if(-not [NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("List-Process",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("Get-ProcessClrVersion",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("Get-Symbols",$MyInvocation.MyCommand)
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
        $tListPath = [NoFuture.Shared.Core.NfConfig+X64]::TList
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
        $clrVerPath = [NoFuture.Shared.Core.NfConfig+X64]::ClrVer
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
        
        $symSrvExe = [NoFuture.Shared.Core.NfConfig+X64]::SymChk

        if([string]::IsNullOrWhiteSpace($symSrvExe) -or -not (Test-Path $symSrvExe)){
            Write-Host "The global NoFuture.Shared.Core.NfConfig+X64.SymChk is unassigned."
            break;
        }

        $localSymbolsFolder = [NoFuture.Shared.Core.NfConfig]::SymbolsPath

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
