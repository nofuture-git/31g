try{
if(-not [NoFuture.MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.MyFunctions]::FunctionFiles.Add("Trace-StoredProcedure",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-AllPdbData",$MyInvocation.MyCommand)
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

<#
    .synopsis
    Body of function includes examples and notes
    of using the Windows Performance Counters.
    
    .Description
    For view only.
    
    .outputs
    null
    
#>
function Counter-Examples
{
    [CmdletBinding()]
    Param
    (
    )
    Process
    {
        #add this to app/web.config
        $allCountersOn = @'
        <system.serviceModel>
         <diagnostics performanceCounters="All" />
        </system.serviceModel>   
'@
        #http://msdn.microsoft.com/en-us/library/w8f5kw2e(v=vs.100).aspx
        #total number of request
        $totalRequest = Get-Counter -Counter ("\\{0}\W3SVC_W3WP(*)\Total HTTP Requests Served" -f $env:COMPUTERNAME)
        
        #by web application, including the cached responses from http.sys
        $allRequest = Get-Counter -Counter ("\\{0}\HTTP Service Url Groups(*)\AllRequests" -f $env:COMPUTERNAME)
        #use this to map the group url hex ID to an application 
        $file_lines = &"c:\windows\system32\netsh.exe" http show servicestate
        
		#http://technet.microsoft.com/en-us/library/ee624046%28v=ws.10%29.aspx
		#perform a network trace
		&"c:\windows\system32\netsh.exe" trace start scenario=directaccess capture=yes report=yes
		#reproduce issue
		&"c:\windows\system32\netsh.exe" trace stop
		
        #request from the web application
        $eipWebRequest = Get-Counter -Counter "\\localhost\ASP.NET Apps v4.0.30319(_lm_w3svc_1_root_eip)\Anonymous Requests"
        $eipSvcRequest = Get-Counter -Counter "\\localhost\ASP.NET Apps v4.0.30319(_lm_w3svc_1_root_eipservices)\Anonymous Requests"
        
        #request from the app services layer
        $serviceMethod = "GetAnnouncements"
        $serviceName = "WidgetService"
        $serviceRequest = Get-Counter -Counter ("\\{0}\ServiceModelOperation 4.0.0.0(*{1}*{2}.svc)\Calls" -f $env:COMPUTERNAME,$serviceMethod.SubString(0,10),$serviceName)
        
        #request from the app service layer by method
        $serviceRequestByMethod = Get-Counter -Counter ("\\{0}\ServiceModelService 4.0.0.0(*{1}.svc)\Calls" -f $env:COMPUTERNAME,$serviceName)
        
        #.NET data http://msdn.microsoft.com/en-us/library/kfhcywhs(v=vs.100)
        
        $w3wp = @{
                                    #like the name says, throw 20, you'll see 20
                  ".NET CLR Exceptions" = @(
                                    "# of Exceps Thrown"
                                    );
                                    #CCW's (COM callable wrappers) are native objects having a refernce to a managed one
                                    #stubs are for runtime arg passing between managed and unmanaged
                  ".NET CLR Interop" = @(
                                    "# of CCWs",
                                    "# of Stubs"
                                    );
                                    #should expect this to rise then level off the longer the app is left running
                                    #from http://msdn.microsoft.com/en-us/library/k5532s8a(v=vs.100).aspx
                                    #[JIT compilation takes into account the possibility that some code might 
                                    # never be called during execution. Instead of using time and memory to convert 
                                    # all the MSIL in a PE file to native code, it converts the MSIL as needed 
                                    # during execution and stores the resulting native code in memory so that it 
                                    # is accessible for subsequent calls in the context of that process.]
                  ".NET CLR Jit" = @(
                                    "# of Methods JITted"
                                    );
                                    #just like it says
                  ".NET CLR Loading" = @(
                                    "Current Classes Loaded",
                                    "Current Assemblies"
                                    );
                                    
                                    #logical threads are owned by the managed runtime
                                    #physical threads are the underlying native one's the managed one depend on
                                    #recognized threads are only the ones that came through the main
                                    #thread Factory generated threads are not recognized
                  ".NET CLR LocksAndThreads" = @(
                                    "Total # of Contentions",
                                    "# of current logical Threads",
                                    "# of current physical Threads",
                                    "# of current recognized threads"
                                    );
                                    #this won't be updated until a GC.Collect() gets called
                  ".NET CLR Memory" = @(
                                    "# of Pinned Objects",
                                    "Large Object Heap size",
                                    "Gen 0 heap size",
                                    "Gen 1 heap size",
                                    "Gen 2 heap size"
                                    );
                                    #these don't have anything to do with the IPrincipal crap
                  ".NET CLR Security" = @(
                                    "% Time in RT checks",
                                    "Total Runtime Checks"
                                    );
                                    #this has instances but its always just 1 or 2
                  ".NET Data Provider for SqlServer" = @(
                                    "numberofpooledconnections",
                                    "numberofactiveconnectionpools"
                                    )}
        
        

        $w3wp = Get-Process -Name "w3wp"
        
        $w3wp.Threads | ? {$_.TotalProcessorTime -ne '00:00:00'} | Sort-Object -Property TotalProcessorTime
    
        $counters = @("HTTP Service","HTTP Service Url Groups","HTTP Service Request Queues",
                      "APP_POOL_WAS","W3SVC_W3WP",".NET Data Provider for SqlServer",
                      ".NET CLR Loading","ASP.NET v4.0.30319","ASP.NET Apps v4.0.30319",
                      "ASP.NET State Service","ServiceModelEndpoint 4.0.0.0","ServiceModelOperation 4.0.0.0",
                      "ServiceModelService 4.0.0.0","Windows Workflow Foundation",
                      "WF (System.Workflow) 4.0.0.0",".NET CLR Data","Web Service")
        
        #example for getting all SQL Server connection counters
        (Get-Counter -ListSet ".NET Data Provider for SqlServer") | % {Get-Counter -Counter $_.Counter}
    }
}
