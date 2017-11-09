try{
if(-not [NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("npp",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("Write-COM",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("Write-COMRemote",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("Write-FileDifferences",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("Write-Wmi",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
#=================================================

Set-Alias npp Get-NotepadPlusPlus


<#
    .synopsis
    Opens Notepad++
    
    .Description
    Opens Notepad++ for a given file or for an entire
    directory in which the file's extensions therein are
    found in the global code extension list.  A file's 
    syntax will also be resolved to have Notepad++ pretty
    print it with syntax highlighting.
    
    .parameter Path
    A file or directory.
    
    .parameter Line
    The line number at which Notepad++ should open to.
    
    .example
    C:\PS>Get-NotepadPlusPlus -Path "C:\MyCodeDirectory"
    
    .example
    C:\PS>Get-NotepadPlusPlus -Path "C:\MyCodeDirectory\scripts\myJavascript.js" -Line 243
    
    .example
    C:\PS>Get-NotepadPlusPlus C:\MyCodeDirectory\scripts\myJavascript.js 243
    
    .outputs
    null
    
#>
function Get-NotepadPlusPlus
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [AllowEmptyString()]
        [string] $Path,
        [Parameter(Mandatory=$false,position=1)]
        [int] $Line
    )
    Process
    {

        if($Path -eq $null -or $Path -eq ""){Invoke-Expression -Command "& 'C:\Program Files\Notepad++\notepad++.exe'"; break;}

        if((Get-Item $Path).PSIsContainer){
            ls -Path $Path | ? {(-not $_.PSIsContainer) -and ($global:codeExtensions -contains $_.Extension) } | % {
                $ext = Get-NppSyntax $_.FullName
	            Invoke-Expression -Command ("& 'C:\Program Files\Notepad++\notepad++.exe' -n{0} -l{1} '{2}'" -f 0,$ext,$_.FullName)
            }
        }
        else{
            $ext = Get-NppSyntax $Path
	        Invoke-Expression -Command ("& 'C:\Program Files\Notepad++\notepad++.exe' -n{0} -l{1} '{2}'" -f $Line,$ext,$Path)
        }
    }
}


function Get-NppSyntax($pathFile)
{
    $xmlExtentsion = @(
        "aspx","asax","ascx","asmx","config","csproj",
        "vbproj","sln","rpx","xslt","xsd","webinfo",
        "wsdl","sitemap","dtsx","database","user",
        "edmx","fcc","pdm","pp","svc","testsettings",
        "ps1xml","tt")

    $ext = [System.IO.Path]::GetExtension($pathFile)
    if($ext -eq $null){
        return "txt"
    }
    $ext = $ext.Replace(".","")
    if($ext -eq "ps1"){
        $ext = "powershell"
    }
    elseif($ext -eq "js"){
        $ext = "javascript"
    }
    elseif(@("bas","cls","frm") -contains $ext){
        $ext = "vb"
    }
    elseif($ext -eq "h"){
        $ext = "cpp"
    }
    elseif(@($xmlExtentsion) -contains $ext){
        $ext = "xml"
    }
    elseif($ext -eq "vbp"){
        $ext = "ini"
    }
	elseif($ext -eq "cs"){
		#npp know the cs as c-sharp so no translation is needed but the 
		#appropriate regex still needs to be recorded
	}
	
    return $ext
    
}


<#
    .synopsis
    Prints all COM objects installed on the local machine.
    
    .Description
    Prints all the COM objects installed on the local machine.
    The output tends to be very very long.
	Installed COM types are listed in the registry under HKCR\TypeLib
    
#>
function Write-COM
{
    [CmdletBinding()]
    Param()
    Process
    {
    	ls HKLM:\Software\Classes -ea 0| ? {$_.PSChildName -match '^\w+\.\w+$' -and (Get-ItemProperty "$($_.PSPath)\CLSID" -ea 0)} | % { $_.PSChildName }
    }
}

<#
    .SYNOPSIS
    Prints all WMI namespaces.
    
    .DESCRIPTION
    Prints all the WMI namespace on this box followed 
    by a list of the providers under that namespace.
    
    .PARAMETER Namespace
    The WMI namespace from which to recursively
    start the print.  The root is "root".
    
    .EXAMPLE
    C:\PS> Write-WmiNamespaces "root"
    
#>
function Write-Wmi
{
    [CmdletBinding()]
    Param 
    (
    )
    Process
    {
        $proceed = Read-Host "This takes a very long time to run - proceed [y | n]"
        if($proceed.ToLower() -ne "y") {break;}
        $wmiOutputFile = ([NoFuture.Shared.Core.NfConfig+TempFiles]::Wmi)
        "root" > $wmiOutputFile
        Get-WmiNamespace "root" $wmiOutputFile

    }
}
function Get-WmiNamespace
{
    Param
    (
        [string]$Name,
        [string]$wmiOutput
    )
    Process
    {
        Get-WmiObject -Namespace $Name -Class "__NAMESPACE" | % {
            
            $ns = "$name\" + $_.Name
            Write-Progress -Activity ("Namespace: {0}" -f $ns) -Status "working..."
            "`nNamespace: $ns"  >> $wmiOutput
            Get-WmiProviderClass $ns $wmiOutput
            Get-WmiNamespace $ns $wmiOutput
        }
    }
}

function Get-WmiProviderClass
{
    Param
    (
        [string]$namespace,
        [string]$wmiOutput

    )
    Process
    {
        Get-WmiObject -Namespace $namespace -Class __Win32Provider | % {
            $provider = $_.Name
            Write-Progress -Activity ("Namespace: {0} Provider: {1}" -f $namespace,$provider) -Status "working..."
            "Provider : $provider" >> $wmiOutput

            $refs = Get-WmiObject -Namespace $namespace -Query "REFERENCES OF {__Win32Provider.Name='$provider'}"

            foreach($ref in $refs)
            {
                $type = $ref.__CLASS
                Write-Progress -Activity ("Namespace: {0} Provider: {1} Registration: {2}" -f $namespace,$provider,$type) -Status "working..."
                " Registration: $type" >> $wmiOutput

                switch ($type) 
                {
                    "__PropertyProviderRegistration" {
                        "  does not have classes" >> $wmiOutput
                        break
                    }

                    "__ClassProviderRegistration" {
                        "  only provides class definitions" >> $wmiOutput
                        break
                    }

                    "__EventConsumerProviderRegistration" {
                        "  uses these classess" >> $wmiOutput
                        "    $($ref.ConsumerClassNames)" >> $wmiOutput
                        break
                    }

                    "__EventProviderRegistration" {
                        "  queries these classes:"  >> $wmiOutput
                        foreach($query in $ref.EventQueryList) {
                            $a = $query -split " "
                            "    $($a[($a.Length-1)])" >> $wmiOutput
                        }#end foreach query
                        break
                    }

                    default {
                        "  supplies these classes:" >> $wmiOutput
                        Get-WmiObject -Namespace $namespace -List -Amended | % {
                            if($_.Qualifiers["provider"].Value -eq "$provider")
                            {
                                "    $($_.Name)" >> $wmiOutput
                            }
                        }#end foreach amended 
                    }
                }#end switch
            }#end foreach refs

        }#end foreach Win32Provider
    }

}

<#
    .synopsis
    Prints all COM objects on a remote machine.
    
    .Description
    Prints all COM objects on a remote machine.
    The output tends to be very, very long.
    
    .parameter RemoteMachine
    The machine name or the remote box.  The user must have permissions.
    
    .EXAMPLE
    C:\PS>Wrote-COMRemote VNCMACHINE_2VX
    
#>
function Write-COMRemote
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $RemoteMachine
    )
    Process
    {
        $thehklm = [microsoft.win32.registrykey]::openremotebasekey("LOCALMACHINE",$RemoteMachine)
        $theComObjs = $thehklm.opensubkey("SOFTWARE\Classes")
        $theComObjs.GetSubKeyNames() | ? {$_.ToString() -match '^\w+\.\w+$' } > ("{0}ComInstalls.txt" -f $RemoteMachine)
    }    
}

<#
    .synopsis
    Print differences between two directories.
    
    .Description
    The Benchmark directory is searched and each file
    therein is checked for in the Target including child
    directory files.  Return a simple hash table having the 
    file's fullname as key and the words 'MISSING' or 'DIFFER'.
    Files that are equal are not included
        
    .parameter $BenchmarkPath
    The path from which all comparasions manifest.
    
    .parameter $TargetPath
    The target directory for comparing against
    
    .example
    C:\PS> Write-FileDifferences -BenchmarkPath C:\projects\mycode\myproject -TargetPath C:\Inetpub\wwwroot\myproject 
    
    .outputs
    Hashtable
    
#>
function Write-FileDifferences
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $BenchmarkPath,
        [Parameter(Mandatory=$true,position=1)]
        [string] $TargetPath,
        [Parameter(Mandatory=$false,position=2)]
        [switch] $IgnoreNewLineDiff

    )
    Process
    {
    
    Write-Progress -Activity "starting" -Status "OK"

    #confirm user input is a directory 
    if(-not (Get-Item $BenchmarkPath).PSIsContainer)
    {
        $BenchmarkPath = (Split-Path -Path $BenchmarkPath -Parent)
    }
    if(-not (Get-Item $TargetPath).PSIsContainer)
    {
        $TargetPath = (Split-Path -Path $TargetPath -Parent)
    }

   
    #append the path separator to the end for consistancy
    if(-not $BenchmarkPath.EndsWith([System.IO.Path]::DirectorySeparatorChar))
    {
        $BenchmarkPath += [System.IO.Path]::DirectorySeparatorChar
    }
    
    if(-not $TargetPath.EndsWith([System.IO.Path]::DirectorySeparatorChar))
    {
        $TargetPath += [System.IO.Path]::DirectorySeparatorChar
    }

    if($IgnoreNewLineDiff){
        #break paths into copy parts
        $fileDiffTemp = ([NoFuture.Shared.Core.NfConfig+TempDirectories]::Root) 
        if([string]::IsNullOrWhiteSpace($fileDiffTemp)){
            $fileDiffTemp = $env:TEMP
        }

        $bfn = (Split-Path $BenchmarkPath -Leaf)
        $tfn = (Split-Path $TargetPath -Leaf)

        Write-Progress -Activity "creating copies of both paths to " -Status "OK"
        #benchmark temp path 
        $tempBenchmark = (Join-Path $fileDiffTemp $bfn)
        $tempTarget = (Join-Path $fileDiffTemp $tfn)

        #simply don't handle situation where making copies is going to overwrite each other
        if($bfn -eq $tfn){
            Write-Host "to ignore new line chars copies need to be made of '$bfn' and '$tfn'" -ForegroundColor Yellow
            Write-Host "rename one of the two and try again." -ForegroundColor Yellow
            break;
        }

        #test if the, prehaps, has been run before
        if(Test-Path $tempBenchmark){
            $doNotDisplay = Remove-Item $tempBenchmark -Force -Recurse
        }
        if(Test-Path $tempTarget){
            $doNotDisplay = Remove-Item $tempTarget -Force -Recurse
        }

        #make full copies of the whole directories
        $doNotDisplay = Copy-Item -Path $BenchmarkPath -Destination $fileDiffTemp -Recurse -Force
        $doNotDisplay = Copy-Item -Path $TargetPath -Destination $fileDiffTemp -Recurse -Force

        #test that the copies are present
        if(-not(Test-Path $tempBenchmark)){
            Write-Host "the copied directory of '$bfn' is missing, was expected it at '$tempBenchmark'"
            break;
        }
        if(-not(Test-Path $tempTarget)){
            Write-Host "the copied directory of '$tfn' is missing, was expected it at '$tempTarget'"
            break;
        }

        #replace all new line chars
        $lf = (New-Object System.String(@([char]0x0A)))
        $crLf = (New-Object System.String(@([char]0x0D, [char]0x0A)))

        Replace-Content -FindText $crLf -ReplaceWith $lf -Path $tempBenchmark
        Replace-Content -FindText $crLf -ReplaceWith $lf -Path $tempTarget

        #reassign parameters and continue
        $BenchmarkPath = $tempBenchmark
        $TargetPath = $tempTarget
    }

    #set this explictly to an array
    $benchmarkDirs = @()

    #get directories - expect all to start with path sep.
    Write-Progress -Activity "getting all benchmark directories" -Status "OK"
    $benchmarkDirs = (ls -Path $BenchmarkPath -Exclude *.* -Recurse | % {$_.FullName.Replace($BenchmarkPath,"")})
    #add self
    $benchmarkDirs += [System.IO.Path]::DirectorySeparatorChar
    
    #set aside a hash table to return
    $differences = @{}
    
    
    #go through each directory including self
    $benchmarkDirs | % {
        $progressPath00 = $_;
        #join the base directories user entered with a child-directory/self
        $currentTargetPath = (Join-Path $TargetPath $_)
        $currentBenchPath = (Join-Path $BenchmarkPath $_)
        
        #test the benchmark directory exist
        if(Test-Path $currentTargetPath){
            
            #iterate each file in the given Benchmark directory
            ls -Path $currentBenchPath | ? {(-not $_.PSIsContainer)} | % {

                #get the full path names of benchmark and target files
                $currentBenchFullName = $_.FullName
                $currentTargetFullName = (Join-Path $currentTargetPath $_.Name)
                
                Write-Progress -Activity "$progressPath00 $currentTargetFullName" -Status "OK"

                #determine if target file is present
                if(Test-Path $currentTargetFullName)
                {
                
                    #determine if files differ
                    $BenchFileHash = $(Get-Md5CheckSum -Path $currentBenchFullName)
                    $TargetFileHash = $(Get-Md5CheckSum -Path $currentTargetFullName)
                    
                    #these files differ in some capacity
                    if($BenchFileHash -ne $TargetFileHash)
                    {
                        $differences.Add($currentBenchFullName.Replace($BenchmarkPath,""),"DIFFER")
                    }
                }#end Target File exist
                else{#the directory is present but this file is missing
                    $differences.Add($currentBenchFullName.Replace($BenchmarkPath,""),"MISSING")
                }
            
            }#end currentBenchPath
        }
        else{#the entire directory is missing
           ls -Path $currentBenchPath | ? { (-not $_.PSIsContainer) } | % { $differences.Add($_.FullName.Replace($BenchmarkPath,""),"MISSING") }
        }#end TestPath
    }#end benchmarkDirs
    
    return $differences
	
	}#end Process
}

