try{
if(-not [NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Trace-StoredProcedure",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Get-AllPdbData",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("New-AssemblyAnalysis",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
<#
    .synopsis
    Recurse trace of a stored procedures.
    
    .Description
    Performs a recursive trace of a list of stored procedure names.
    When another stored procedure is found to be calling one of the 
    procedures in the list it is, in turn, added to the list and the cycle
    is repeated. 
    
    The recurse continues until no more calls are found for any procedure
    in the list.
    
    The final list is then returned.
    
    Depends on the sql.ps1 script being in scope.
    
    .parameter StoredProcedures
    An array of stored procedure names.
    
    .example
    C:\PS>Trace-StoredProcedure -Names @("p_storedproc1","p_storedProc2")
    
    .example
    C:\PS>Trace-StoredProcedure ,"stored_proc"
    
    .outputs
    null
    
#>
function Trace-StoredProcedure
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [array] $Names
    )
    Process
    {
        $procList = $Names
        $localList = @()
        if($procList.GetType() -ne @().GetType()){ $val = $procList; $procList = @(); $procList += $val }
        $procList | % {
            Write-Progress -Activity $_ -Status "OK"
            $result = $null
            $proc = $_
            $results = (pss ('\W{0}\W' -f $_))
            if($results -ne $null){
                $results |? {$_ -ne $null -and $_.Line -notmatch "\WPROCEDURE\W"}| % {
                    $path = $_.Path.Replace((Join-Path $mypshome "\temp\procs\"),"").Replace(".sql","")
                    if($procList -notcontains $path){
                        $localList += $path
                    }#end add to recursive list
                }#end foreach results
            }#end if results not null
        }#end foreach w/i list
        if($locallist.Count -gt 0){
            $procList += (Trace-StoredProcedure $locallist)
        }
        return ($procList | Sort-Object -Unique)
    }
}
<#
    .SYNOPSIS
    Reads in std output from dia2dump into a dynamic psobject.
   
    .DESCRIPTION
    Given a valid assembly and the custom tool Dia2Dump, the
    cmdlet will get the pdb data for the given assembly (testing
    first that both the dll and pdb files are present and
    side-by-side).

    If all cmdlet switches are false then then the cmdlet will
    return everything.

    All the heavy lifting is done by the Dia2Dump and this 
    cmdlet simply takes its output and converts it from JSON 
    into a dynamic psobject.
    
    While Dia2Dump appears to be fully capable of getting 
    pdb data from WIN32, COM, and other native Windows assemblies,
    this cmdlet only focuses on what it produces for .NET (Managed)
    assemblies. 
    
    .PARAMETER AssemblyName
    The full path and name of an assembly on disk.

    .PARAMETER Modules
    The '-m' switch for Dia2Dump.

    .PARAMETER Globals
    The '-g' switch for Dia2Dump.

    .PARAMETER Files
    The '-f' switch for Dia2Dump.

    .PARAMETER Lines
    The '-l' switch for Dia2Dump.

    .PARAMETER Symbols
    The '-s' switch for Dia2Dump.

    .PARAMETER Sections
    The '-c' switch for Dia2Dump.
    
    .OUTPUTS
    psobject
#>
function Get-AllPdbData
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $AssemblyPath,
        [Parameter(Mandatory=$false,position=1)]
        [switch] $Modules,
        [Parameter(Mandatory=$false,position=2)]
        [switch] $Globals,
        [Parameter(Mandatory=$false,position=3)]
        [switch] $Files,
        [Parameter(Mandatory=$false,position=4)]
        [switch] $Lines,
        [Parameter(Mandatory=$false,position=5)]
        [switch] $Symbols,
        [Parameter(Mandatory=$false,position=6)]
        [switch] $Sections

    )
    Process
    {
        if(-not(Test-Path $AssemblyPath)) {throw "No dll was found at '$AssemblyPath'."}
        $pdbData = New-Object NoFuture.Shared.DiaSdk.DumpAll
        $invocationDia2Dump = New-Object NoFuture.Gen.InvokeDia2Dump.GetPdbData($AssemblyPath);
        $atLeastOneSwitch = $false;

        if($Modules){
            Write-Progress -Activity ("Getting Pdb Modules for '$AssemblyPath'.") -Status "OK" -PercentComplete 16
            $atLeastOneSwitch = $true
            $pdbData.Modules = $invocationDia2Dump.DumpAllModulesToFile();
        }

        if($Globals){
            Write-Progress -Activity ("Getting Pdb Globals for '$AssemblyPath'.") -Status "OK" -PercentComplete 32
            $atLeastOneSwitch = $true
            $pdbData.Globals = $invocationDia2Dump.DumpAllGlobalsToFile();
        }

        if($Files){
            Write-Progress -Activity ("Getting Pdb Files for '$AssemblyPath'.") -Status "OK" -PercentComplete 48
            $atLeastOneSwitch = $true
            $pdbData.Files = $invocationDia2Dump.DumpAllFilesToFile();
        }

        if($Lines){
            Write-Progress -Activity ("Getting Pdb Lines for '$AssemblyPath'.") -Status "OK" -PercentComplete 64
            $atLeastOneSwitch = $true
            $pdbData.Lines = $invocationDia2Dump.DumpAllLinesToFile();
        }

        if($Symbols){
            Write-Progress -Activity ("Getting Pdb Symbols for '$AssemblyPath'.") -Status "OK" -PercentComplete 80
            $atLeastOneSwitch = $true
            $pdbData.Symbols = $invocationDia2Dump.DumpAllSymbolsToFile();
        }

        if($Sections){
            Write-Progress -Activity ("Getting Pdb Sections for '$AssemblyPath'.") -Status "OK" -PercentComplete 96
            $atLeastOneSwitch = $true
            $pdbData.Sections = $invocationDia2Dump.DumpAllSectionsToFile();
        }

        if(-not $atLeastOneSwitch){
            #if no switches then return them all
            return Get-AllPdbData -AssemblyPath $AssemblyPath -Modules -Globals -Files -Lines -Symbols -Sections
        }
        else{
            return $pdbData
        }
    }

}#end Get-AllPdbData


<#
    .SYNOPSIS
    Creates a new instance of AssemblyAnalysis.

    .DESCRIPTION
    Creates a new instance of NoFuture.Util.Gia.AssemblyAnalysis
    and registers an event handler.

    .PARAMETER AssemblyPath
    Path to a .NET assembly.

    .OUTPUTS
    NoFuture.Util.Gia.AssemblyAnalysis
#>
function New-AssemblyAnalysis
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $AssemblyPath

    )
    Process
    {
        if(-not (Test-Path $AssemblyPath)){
            throw "bad path or file name"
        }

        $asmAnl = New-Object NoFuture.Util.Gia.AssemblyAnalysis($AssemblyPath,$false)

        #$psEventRegister = Register-ObjectEvent -InputObject $asmAnl -EventName "ProgressReporter" -Action {
        #    Write-Progress -Activity $event.SourceArgs.Activity -Status $event.SourceArgs.Status -PercentComplete $event.SourceArgs.ProgressCounter            
        #}

        $asmAnl | Add-Member -MemberType NoteProperty -Name "PsEventRegistration" -Value $psEventRegister

        return $asmAnl
    }
}

