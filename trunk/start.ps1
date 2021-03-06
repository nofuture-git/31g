<#
    .SYNOPSIS
    Will update the Environment Variable PATH with NoFuture values
    
    .DESCRIPTION
    Will set the CLASSPATH, JAVA_HOME and PATH values to the
    paths found in the nfConfig.cfg.xml
    
    .PARAMETER WhatIf
    Optional switch to determine what would change on this machine
    if the cmdlet was run.

#>
function Set-NoFutureEnvPathValues
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false)]
        [switch] $WhatIf
    )
    Process
    {
        try{
            #this will blow out with a type loader exception
            if([NoFuture.Shared.Cfg.NfConfig] -eq $null){ }
        }catch{
            Write-Host ("The NoFuture custom configuration file has not been loaded into this AppDomain (PowerShell)" + `
                        "See the type 'NoFuture.Shared.Cfg.NfConfig' in 'NoFuture.Shared.Cfg.dll'.") -ForegroundColor Yellow
            break;
        }

        #environment variables for JAVA
        if($env:JAVA_HOME -eq $null -or $env:JAVA_HOME -ne ([NoFuture.Shared.Cfg.NfConfig+BinDirectories]::JavaRoot))
        {
            if($WhatIf){
                Write-Host ("What if: Would set JAVA_HOME to '{0}'" -f 
                            ([NoFuture.Shared.Cfg.NfConfig+BinDirectories]::JavaRoot)) -ForegroundColor Yellow
            }
            else{
                [System.Environment]::SetEnvironmentVariable("JAVA_HOME",([NoFuture.Shared.Cfg.NfConfig+BinDirectories]::JavaRoot),"Machine")
            }
        }

        $javaClassPath = ("{0}\*" -f ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::JavaDist))

        if($env:CLASSPATH -ne $javaClassPath)
        {
            if($WhatIf){
                Write-Host ("What if: Would set CLASSPATH to '$javaClassPath'") -ForegroundColor Yellow
            }
            else{
                [System.Environment]::SetEnvironmentVariable("CLASSPATH", $javaClassPath,"Machine")
            }
        }

        $nfPythonPath = [NoFuture.Shared.Cfg.NfConfig+PythonTools]::NfPythonPath

        if( -not ([string]::IsNullOrWhiteSpace($nfPythonPath)) -and (Test-Path $nfPythonPath)){
            if($env:PYTHONPATH -eq $null){
                if($WhatIf){
                    Write-Host ("What if: Would add PYTHONPATH as '$nfPythonPath'") -ForegroundColor Yellow
                }
                else{
                    [System.Environment]::SetEnvironmentVariable("PYTHONPATH", $nfPythonPath,"Machine")
                }
            }
            else{
                $existingPyPath = $env:PYTHONPATH.Split(';')
                $fPath00 = [string]::Join("\", ($nfPythonPath.Split("\") | ? {-not ([string]::IsNullOrWhiteSpace($_))}))
                
                #is nfPythonPath already present on the existing PYTHONPATH env var?
                $isNfPyPathPresent = $false
                :nextPyPath foreach($pyPath in $existingPyPath){

                    $fPath01 = [string]::Join("\", ($pyPath.Split("\") | ? {-not ([string]::IsNullOrWhiteSpace($_))}))

                    if($isNfPyPathPresent -eq $false -and ([string]::Equals($fPath00, $fPath01, [System.StringComparison]::OrdinalIgnoreCase))){
                        $isNfPyPathPresent = $true
                    }
                }

                #when missing add it to the tail
                if(-not $isNfPyPathPresent){
                    $newPyPaths = $env:PYTHONPATH.Split(';')
                    $newPyPaths += $nfPythonPath
                    $newPyPath = [string]::Join(";", $newPyPaths)
                    
                    if($WhatIf){
                        Write-Host ("What if: Would set PYTHONPATH to '$newPyPath'") -ForegroundColor Yellow
                    }
                    else{
                        [System.Environment]::SetEnvironmentVariable("PYTHONPATH", $newPyPath,"Machine")
                    }
                }
            }
        }


        $nfBin = Join-Path $global:mypshome "bin\"
        $javaBin = Join-Path ([NoFuture.Shared.Cfg.NfConfig+BinDirectories]::JavaRoot) "bin\"
        $pyBin = ([NoFuture.Shared.Cfg.NfConfig+BinDirectories]::PythonRoot)
        if(-not ($pyBin.EndsWith("\"))){
            $pyBin += "\"
        }

        $ffmpegBin = (Split-Path ([NoFuture.Shared.Cfg.NfConfig+PythonTools]::Ffmpeg) -Parent)
        if(-not ($ffmpegBin.EndsWith("\"))){
            $ffmpegBin += "\"
        }

        $explicitSetPathVars = @{ }

        if(Test-Path $nfBin){
            $explicitSetPathVars += @{ $nfBin ="NoFuture.Shared.Core.dll"}
        }

        if(Test-Path $javaBin){
            $explicitSetPathVars += @{ $javaBin = "java.exe"}
        }

        if(Test-Path $pyBin){
            $explicitSetPathVars += @{ $pyBin = "python.exe"}
        }

        if(Test-Path $ffmpegBin){
            $explicitSetPathVars += @{ $ffmpegBin = "ffmpeg.exe"}
        }

        #first pass: omit those we intend to assign
        $envPaths = [System.Environment]::GetEnvironmentVariable("Path", "Machine").Split(";")

        $newEnvPaths = @()
        $fullyResolvedPaths = @()
        $counter = 0
        :nextPath foreach($p in $envPaths){

            $counter += 1
            if([string]::IsNullOrWhiteSpace($p)){
                if($WhatIf){
                    Write-Host "What if: Would IGNORE empty path at entry $counter" -ForegroundColor Yellow
                }

                continue nextPath;
            }

            #have the working path full-resolved of any environment var's
            $workingPath = $p
            if($workingPath.Contains("%")){
                $tempPath = ""
                if([NoFuture.Util.Core.NfPath]::TryResolveEnvVar($workingPath, [ref] $tempPath)){
                    $workingPath = $tempPath    
                }
            }

            if(-not (Test-Path $workingPath)){
                if($WhatIf){
                    Write-Host "What if: Would remove NON-EXISTENT directory from PATH '$p' at entry $counter" -ForegroundColor Yellow
                }

                continue nextPath;
            }

            if($fullyResolvedPaths -contains $workingPath){
                if($WhatIf){
                    Write-Host "What if: Would remove DUPLICATE directory from PATH '$p' at entry $counter" -ForegroundColor Yellow
                }
                continue nextPath;
            }

            :nextExplicitSet foreach($nfPath in $explicitSetPathVars.Keys){
                $nfFile = $explicitSetPathVars[$nfPath]

                #don't want ending "\" to make it test as not-equal
                $fPath00 = [string]::Join("\", ($workingPath.Split("\") | ? {-not ([string]::IsNullOrWhiteSpace($_))}))
                $fPath01 = [string]::Join("\", ($nfPath.Split("\") | ? {-not ([string]::IsNullOrWhiteSpace($_))}))

                $isNotNfPath = -not ([string]::Equals($fPath00, $fPath01, [System.StringComparison]::OrdinalIgnoreCase))
                $isNfFile = (Test-Path (Join-Path $workingPath $nfFile))

                if($isNfFile -and $isNotNfPath){
                    
                    if($WhatIF){
                        Write-Host "What if: Would REPLACE '$p' with '$nfPath' at entry $counter" -ForegroundColor Yellow
                    }

                    $p = $nfPath
                }
            }

            #attempt to perserve any paths with env var's in them
            $fullyResolvedPaths += $workingPath
            $newEnvPaths += $p
        }

        #having dealt with duplicates, check if any are new outright
        $explicitSetPathVars.Keys | ? {$newEnvPaths -notcontains $_} | % {
            $missingPath = $_;
            $counter += 1
            if($WhatIf){
                Write-Host "What if: Would ADD '$missingPath' at entry $counter" -ForegroundColor Green
            }
            $newEnvPaths += $missingPath
        }

        #determine if anything actually changed
        if($envPaths.Length -eq $newEnvPaths.Length){
            $anyChanged = 0
            $newEnvPaths | ? {$envPaths -notcontains $_} | % {$anyChanged += 1}
            if($anyChanged -eq 0){
                if($WhatIf){
                    Write-Host "What if: NOTHING would be changed" -ForegroundColor Green
                }
                return;
            }
        }
        if($WhatIf){
            Write-Host ("What if: NEW %PATH% variable (new-line used for clarity, actual is semi-colon):") -ForegroundColor Yellow
            Write-Host ("`t" + ([string]::Join("`n`t",$newEnvPaths))) -ForegroundColor Gray

        }
        else{
            [System.Environment]::SetEnvironmentVariable("Path", ([string]::Join(";",$newEnvPaths)),"Machine")
        }
    }
}

#start load
if($Global:nfLoadedFromStart){break;}
Write-Progress -activity "Loading..." -status "no future" -PercentComplete 1
clear

#most important of all globals
$Global:mypshome = Get-Location
$occultum = ".\__occultum"
$noFutureRoot = "C:\Projects\31g\trunk"
$noFutureBin = (Join-Path $global:mypshome "bin")

#get the list of loaded assemblies at session start
if($global:coreAssemblies -eq $null)
{
    $global:coreAssemblies = ([appdomain]::CurrentDomain.GetAssemblies() | % {$_.FullName}  | Sort-Object -Unique)
}

$host.UI.RawUi.WindowTitle = "NoFuture"

#load all global session variables
Write-Progress -activity "Loading..." -status "location set" -PercentComplete 6

#copy assemblies from code
(ls (Join-Path $noFutureRoot "Code\NoFuture\bin") -File ) | ? {$_.Name -notlike "*.vshost.*"} | % {
    $flName = $_.Name
    [System.IO.File]::WriteAllBytes([System.IO.Path]::Combine($noFutureBin, $flName), [System.IO.File]::ReadAllBytes($_.FullName))
 }

$nfCfgXml = Join-Path $noFutureRoot "Code\NoFuture\bin\nfConfig.cfg.xml"
if(Test-Path $nfCfgXml){
    Copy-Item -Path $nfCfgXml -Destination $noFutureBin -Force
}

 
#load all dependent .NET assemblies
$dotNetAssemblies = @(
    "System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
    "System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
    "System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
    "System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
    "System.Speech, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    )

#dependent assemblies from NoFuture
$binPathAssemblies = @{
    "Antlr4.Runtime, Version=4.6.0.0, Culture=neutral, PublicKeyToken=09abb75b9ed49849" = (Join-Path $noFutureBin "Antlr4.Runtime.dll");
    "NoFuture.Antlr.HTML, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Antlr.HTML.dll");
    "NoFuture.Antlr.DotNetIlTypeName, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Antlr.DotNetIlTypeName.dll");
    "NoFuture.Antlr.CSharp4, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Antlr.CSharp4.dll");
    "Jurassic, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "Jurassic.dll");

    "NoFuture.Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Shared.dll");
    "NoFuture.Shared.Cfg, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Shared.Cfg.dll");
    "NoFuture.Shared.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Shared.Core.dll");
    "NoFuture.Shared.WcfClient, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Shared.WcfClient.dll");
    
    "NoFuture.Encryption, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Encryption.dll");
    
    "NoFuture.Util.Binary, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.Binary.dll");
    "NoFuture.Util.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.Core.dll");
    "NoFuture.Util.Core.Math, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.Core.Math.dll");
    "NoFuture.Util.Core.Math.Matrix, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.Core.Math.Matrix.dll");
    "NoFuture.Tokens.DotNetMeta, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Tokens.DotNetMeta.dll");
    "NoFuture.Tokens.Gia, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Tokens.Gia.dll");
    "NoFuture.Util.NfConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.NfConsole.dll");
    "NoFuture.Util.NfType, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.NfType.dll");
    "NoFuture.Tokens.Pos, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Tokens.Pos.dll");

    "NoFuture.Read, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Read.dll");
    "NoFuture.Tokens, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Tokens.dll");

    "NoFuture.Rand.Com, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.Com.dll");
    "NoFuture.Rand.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.Core.dll");
    "NoFuture.Rand.Sp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.Sp.dll");
    "NoFuture.Rand.Domus, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.Domus.dll");
    "NoFuture.Rand.Opes, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.Opes.dll");
    "NoFuture.Rand.Pneuma, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.Pneuma.dll");
    "NoFuture.Rand.Tele, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.Tele.dll");
    "NoFuture.Rand.Edu, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.Edu.dll");
    "NoFuture.Rand.Geo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.Geo.dll");
    "NoFuture.Rand.Gov, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.Gov.dll");
    "NoFuture.Rand.Org, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.Org.dll");
    "NoFuture.Rand.Exo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.Exo.dll");

    "NoFuture.Host.Proc, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Host.Proc.dll");
    "NoFuture.Sql, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Sql.dll");
    "NoFuture.Timeline, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Timeline.dll");

    "Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed" = (Join-Path $noFutureBin "Newtonsoft.Json.dll");
    "itextsharp, Version=5.5.12.0, Culture=neutral, PublicKeyToken=8354ae6d2174ddca" = (Join-Path $noFutureBin "itextsharp.dll");
    "HtmlAgilityPack, Version=2.0.20.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a" = (Join-Path $noFutureBin "HtmlAgilityPack.dll");
}

#load .NET the assemblies
$dotNetAssemblies | % {
    if($global:coreAssemblies -notcontains $_)
    {
        $stdout = [System.Reflection.Assembly]::Load($_)
    }
}

#load my assemblies
$binPathAssemblies.Keys | % {
    if($global:coreAssemblies -notcontains $_)
    {
        $binDll = $binPathAssemblies.$_
        if(Test-Path $binDll)
        {
            $stdout = [System.Reflection.Assembly]::LoadFile($binDll)
        }
        else
        {
            Write-Host ("'{0}' was not found at '{1}'; some functions will not be available." -f $_, $binDll) -ForegroundColor Yellow
        }
    }
}

#=====================================
#intialize paths from nfConfig.cfg.xml
$dnd = [NoFuture.Shared.Cfg.NfConfig]::Init(([NoFuture.Shared.Cfg.NfConfig]::FindNfConfigFile($noFutureBin)))
#=====================================

function Get-MyFunctions(){
    [NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles
}

#load all powershell source-code files
Write-Progress -activity "Loading..." -status ".NET assemblies been loaded" -PercentComplete 12
Import-Module $noFutureRoot\bin.ps1
Write-Progress -activity "Loading..." -status "bin.ps1 loaded" -PercentComplete 17
Import-Module $noFutureRoot\console.ps1 -Force
Write-Progress -activity "Loading..." -status "console.ps1 loaded" -PercentComplete 22
Import-Module $noFutureRoot\crypto.ps1
Write-Progress -activity "Loading..." -status "crypto.ps1 loaded" -PercentComplete 30
Import-Module $noFutureRoot\net.ps1
Write-Progress -activity "Loading..." -status "net.ps1 loaded" -PercentComplete 39
Import-Module $noFutureRoot\random.ps1
Write-Progress -activity "Loading..." -status "random.ps1 loaded" -PercentComplete 48
Import-Module $noFutureRoot\sql.ps1
Write-Progress -activity "Loading..." -status "sql.ps1 loaded" -PercentComplete 54
Import-Module $noFutureRoot\util.ps1
Write-Progress -activity "Loading..." -status "util.ps1 loaded" -PercentComplete 59
Import-Module $noFutureRoot\wsdl.ps1
Write-Progress -activity "Loading..." -status "wsdl.ps1 loaded" -PercentComplete 63

if(Test-Path $occultum)
{
    Write-Progress -activity "Loading..." -status "$occultum loaded" -PercentComplete 68
    . ".\$occultum\hiddenStart.ps1"
}
Write-Progress -activity "Loading..." -status "checking for temp dir's" -PercentComplete 72

#create any temp folders defined in the nfCfg
@(
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Root,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Sql,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::StoredProx,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Code,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Debug,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Hbm,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::JavaSrc,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::JavaBuild,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::JavaDist,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::JavaArchive,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Graph,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::SvcUtil,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Wsdl,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Audio) | ? {-not ([string]::IsNullOrWhiteSpace($_)) -and -not (Test-Path $_) } | % {$dnd = mkdir $_}

Write-Progress -activity "Loading..." -status "checking environment variables" -PercentComplete 81
Set-NoFutureEnvPathValues

#copy Dia2Dump
Write-Progress -activity "Loading..." -status "checking for copy of Dia2Dump.exe" -PercentComplete 89
$dia2DumpDir = Join-Path $noFutureRoot "Code\DIA SDK"
@("Dia2Dump.exe", "Dia2Dump.ilk", "Dia2Dump.pdb") | % {
    $dia2DumpSrc = Join-Path $dia2DumpDir $_ 
    $dia2DumpDest = Join-Path $noFutureBin $_
    if((Test-Path $dia2DumpSrc) -and (-not (Test-Path $dia2DumpDest))){
        Copy-Item -Path $dia2DumpSrc -Destination $dia2DumpDest -Force
    }
}

#have the current app domain exec NoFuture FxPointers.ResolveAssemblyEventHandler whenever it encounters a missing assembly
[NoFuture.Util.Binary.FxPointers]::AddResolveAsmEventHandlerToDomain()

[NoFuture.Util.Binary.FxPointers]::AddAcceptAllCerts()

Write-Progress -activity "Done" -status "complete" -PercentComplete 100
Write-HostAsciiArt "[\]"
Write-Host "=================================" -ForegroundColor DarkRed
Get-MssqlSettings #print it upon load

$Global:nfLoadedFromStart = $true