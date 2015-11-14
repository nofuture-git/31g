#start load
Write-Progress -activity "Loading..." -status "no future" -PercentComplete 1
clear
function Get-MyFunctions(){
    [NoFuture.MyFunctions]::FunctionFiles
}
#most important of all globals
$global:mypshome = Get-Location
Set-Location $global:mypshome
$employer = ".\__quest"

#load all global session variables
Write-Progress -activity "Loading..." -status "location set" -PercentComplete 6

$noFutureBin = (Join-Path $global:mypshome "bin")

#copy my assemblies
 cp -Path (Join-Path $global:mypshome "Code\NoFuture\bin\*")  -Destination $noFutureBin -Force -Recurse

#load all dependent .NET assemblies
$dotNetAssemblies = @(
    "System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
    "System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
    "System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
    "System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
    "System.Speech, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    )

$binPathAssemblies = @{
    "WatiN.Core, Version=2.1.0.1196, Culture=neutral, PublicKeyToken=db7cfd3acb5ad44e" = (Join-Path $noFutureBin "WatiN.Core.dll");
    "Antlr4.Runtime.net40, Version=4.3.0.0, Culture=neutral, PublicKeyToken=eb42632606e9261f" = (Join-Path $noFutureBin "Antlr4.Runtime.net40.dll");
    "NoFuture.Read.Ini, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Read.Ini.dll");
    "NoFuture.Antlr, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Antlr.dll");
    "Jurassic, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "Jurassic.dll");
	"NoFuture.Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Shared.dll");
    "NoFuture.Util, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.dll");
    "NoFuture.Util.Pos, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.Pos.dll");
    "NoFuture.Read, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Read.dll");
	"NoFuture.Tokens, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Tokens.dll");
    "NoFuture.Rand, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.dll");
    "NoFuture.Shared.WcfClient, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Shared.WcfClient.dll");
    "NoFuture.Host.Proc, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Host.Proc.dll");
    "NoFuture.Hbm, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Hbm.dll");
    "NoFuture.Hbm.Sid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Hbm.Sid.dll");
    "NoFuture.Gen, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Gen.dll");
    "NoFuture.Gen.InvokeDia2Dump, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Gen.InvokeDia2Dump.dll");
	"NoFuture.Sql, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Sql.dll");
	"NoFuture.Timeline, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Timeline.dll");
    "Iesi.Collections, Version=4.0.0.0, Culture=neutral, PublicKeyToken=aa95f207798dfdb4" = (Join-Path $noFutureBin "Iesi.Collections.dll");
    "NHibernate, Version=4.0.0.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4" = (Join-Path $noFutureBin "NHibernate.dll");
    "antlr.runtime, Version=2.7.6.2, Culture=neutral, PublicKeyToken=1790ba318ebc5d56" = (Join-Path $noFutureBin "antlr.runtime.dll");
    "DDay.Collections, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "DDay.Collections.dll");
    "DDay.iCal, Version=1.0.2.575, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "DDay.iCal.dll");
    "AutoMapper, Version=3.3.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005" = (Join-Path $noFutureBin "AutoMapper.dll");
    "AutoMapper.Net4, Version=3.3.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005" = (Join-Path $noFutureBin "AutoMapper.Net4.dll");
    "Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed" = (Join-Path $noFutureBin "Newtonsoft.Json.dll");
}

$dotNetAssemblies | % {
    if($global:coreAssemblies -notcontains $_)
    {
        $stdout = [System.Reflection.Assembly]::Load($_)
    }
}

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
. .\globals.ps1
Write-Progress -activity "Loading..." -status "globals have been loaded" -PercentComplete 8

#load all powershell source-code files
Write-Progress -activity "Loading..." -status ".NET assemblies been loaded" -PercentComplete 12
. .\console.ps1
Write-Progress -activity "Loading..." -status "console.ps1 loaded" -PercentComplete 33
. .\regex.ps1
Write-Progress -activity "Loading..." -status "regex.ps1 loaded" -PercentComplete 37
. .\format.ps1
Write-Progress -activity "Loading..." -status "format.ps1 loaded" -PercentComplete 39
. .\print.ps1
Write-Progress -activity "Loading..." -status "print.ps1 loaded" -PercentComplete 42
. .\tokens.ps1
Write-Progress -activity "Loading..." -status "tokens.ps1 loaded" -PercentComplete 46
. .\trace.ps1
Write-Progress -activity "Loading..." -status "trace.ps1 loaded" -PercentComplete 49
. .\util.ps1
Write-Progress -activity "Loading..." -status "util.ps1 loaded" -PercentComplete 51
. .\net.ps1
Write-Progress -activity "Loading..." -status "net.ps1 loaded" -PercentComplete 54
. .\crypto.ps1
Write-Progress -activity "Loading..." -status "crypto.ps1 loaded" -PercentComplete 57
. .\outlook.ps1
Write-Progress -activity "Loading..." -status "outlook.ps1 loaded" -PercentComplete 60
. .\random.ps1
Write-Progress -activity "Loading..." -status "random.ps1 loaded" -PercentComplete 63
. .\wsdl.ps1
Write-Progress -activity "Loading..." -status "wsdl.ps1 loaded" -PercentComplete 66
. .\debug.ps1
Write-Progress -activity "Loading..." -status "debug.ps1 loaded" -PercentComplete 69
. .\bin.ps1
Write-Progress -activity "Loading..." -status "bin.ps1 loaded" -PercentComplete 72
. .\gen.ps1
Write-Progress -activity "Loading..." -status "gen.ps1 loaded" -PercentComplete 74

. .\sql.ps1
. .\hbm.ps1
Write-Progress -activity "Loading..." -status "sql.ps1 loaded" -PercentComplete 85

if(Test-Path $employer)
{
    Write-Progress -activity "Loading..." -status "$employer loaded" -PercentComplete 80
	. .\__quest\employerStart.ps1
}
#instantiate global transparent window, print all loaded funtions to it
Write-Progress -activity "Loading..." -status "setting global variables" -PercentComplete 89

#create any required temp folders
if(-not(Test-Path ([NoFuture.TempDirectories]::Root))){$dnd = mkdir ([NoFuture.TempDirectories]::Root)}
if(-not(Test-Path ([NoFuture.TempDirectories]::Sql))){$dnd = mkdir ([NoFuture.TempDirectories]::Sql)}
if(-not(Test-Path ([NoFuture.TempDirectories]::StoredProcedures))){$dnd = mkdir ([NoFuture.TempDirectories]::StoredProcedures)}
if(-not(Test-Path ([NoFuture.TempDirectories]::Code))){$dnd = mkdir ([NoFuture.TempDirectories]::Code)}
if(-not(Test-Path ([NoFuture.TempDirectories]::Debug))){$dnd = mkdir ([NoFuture.TempDirectories]::Debug)}
if(-not(Test-Path ([NoFuture.TempDirectories]::Hbm))){$dnd = mkdir ([NoFuture.TempDirectories]::Hbm)}
if(-not(Test-Path ([NoFuture.TempDirectories]::Calendar))){$dnd = mkdir ([NoFuture.TempDirectories]::Calendar)}
if(-not(Test-Path ([NoFuture.TempDirectories]::JavaSrc))){$dnd = mkdir ([NoFuture.TempDirectories]::JavaSrc)}
if(-not(Test-Path ([NoFuture.TempDirectories]::JavaBuild))){$dnd = mkdir ([NoFuture.TempDirectories]::JavaBuild)}
if(-not(Test-Path ([NoFuture.TempDirectories]::JavaDist))){$dnd = mkdir ([NoFuture.TempDirectories]::JavaDist)}
if(-not(Test-Path ([NoFuture.TempDirectories]::JavaArchive))){$dnd = mkdir ([NoFuture.TempDirectories]::JavaArchive)}
if(-not(Test-Path ([NoFuture.TempDirectories]::Binary))){$dnd = mkdir ([NoFuture.TempDirectories]::Binary)}
if(-not(Test-Path ([NoFuture.TempDirectories]::Graph))){$dnd = mkdir ([NoFuture.TempDirectories]::Graph)}
if(-not(Test-Path ([NoFuture.TempDirectories]::SvcUtil))){$dnd = mkdir ([NoFuture.TempDirectories]::SvcUtil)}
if(-not(Test-Path ([NoFuture.TempDirectories]::Wsdl))){$dnd = mkdir ([NoFuture.TempDirectories]::Wsdl)}
if(-not(Test-Path ([NoFuture.TempDirectories]::HttpAppDomain))){$dnd = mkdir ([NoFuture.TempDirectories]::HttpAppDomain)}

#set dir structure for http App Domain
$httpAppDomain = (Join-Path ([NoFuture.TempDirectories]::HttpAppDomain) "bin")
if(-not(Test-Path $httpAppDomain)){ $dnd = mkdir $httpAppDomain }
Copy-Item -Path (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Domain.Carriage.exe") -Destination $httpAppDomain
Copy-Item -Path (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Domain.Carriage.exe.config") -Destination $httpAppDomain
Copy-Item -Path (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Domain.Carriage.pdb") -Destination $httpAppDomain
Copy-Item -Path (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Domain.Engine.dll") -Destination $httpAppDomain
Copy-Item -Path (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Domain.Engine.pdb") -Destination $httpAppDomain
Copy-Item -Path (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Shared.dll") -Destination $httpAppDomain
Copy-Item -Path (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Shared.pdb") -Destination $httpAppDomain
Copy-Item -Path (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Util.dll") -Destination $httpAppDomain
Copy-Item -Path (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Util.pdb") -Destination $httpAppDomain
if(([NoFuture.Tools.CustomTools]::Favicon) -ne $null -and (Test-Path ([NoFuture.Tools.CustomTools]::Favicon)))
{
    cp -Path ([NoFuture.Tools.CustomTools]::Favicon) -Destination ([NoFuture.TempDirectories]::HttpAppDomain) -Force
}

#copy antlr to CLASSPATH location
if(-not(Test-Path ([NoFuture.Tools.JavaTools]::Antlr))){

    $antlrDestPath = (Join-Path ([NoFuture.TempDirectories]::JavaDist) ([System.IO.Path]::GetFileName([NoFuture.Tools.JavaTools]::Antlr)))
    if(-not (Test-Path $antlrDestPath)){
        cp -Path ([NoFuture.Tools.JavaTools]::Antlr) -Destination $antlrDestPath
    }
}

#environment variables for JAVA
if($env:JAVA_HOME -eq $null -or $env:JAVA_HOME -ne ([NoFuture.BinDirectories]::JavaRoot))
{
    [System.Environment]::SetEnvironmentVariable("JAVA_HOME",([NoFuture.BinDirectories]::JavaRoot),"Machine")
}

$javaClassPath = ("{0}\*" -f ([NoFuture.TempDirectories]::JavaDist))

if($env:CLASSPATH -ne $javaClassPath)
{
    [System.Environment]::SetEnvironmentVariable("CLASSPATH", $javaClassPath,"Machine")
}

$javaPathVarEntry = (Join-Path ([NoFuture.BinDirectories]::JavaRoot) "bin")
if((Test-Path $javaPathVarEntry) -and -not($env:Path.Contains($javaPathVarEntry))){
    $newPathVar = $env:Path
    if(-not($newPathVar.EndsWith(";"))){
        $newPathVar += ";"
    }
    $newPathVar += $javaPathVarEntry
    [System.Environment]::SetEnvironmentVariable("Path", $newPathVar,"Machine")
}

Write-Progress -activity "Loading..." -status "dependent paths created" -PercentComplete 93

#backup passwords if present
#if((Test-Path $global:KeePassPasswordDatabase)){
#    cp -Path $global:KeePassPasswordDatabase -Destination $global:mypshome -Force
#}

#put copy, if needed, of go.ps1 in user's root for independent ps sessions on clear aero 
$goPs = (Join-Path $env:USERPROFILE "go.ps1")
if(-not (Test-Path $goPs)){
    $srcCtrlGoPs = (Join-Path $global:mypshome "go.ps1")
    if(Test-Path $srcCtrlGoPs){
        cp -Path $srcCtrlGoPs -Destination $goPs -Force
    }
}

#have the current app domain exec NoFuture.FxPointers.ResolveAssemblyEventHandler whenever it encounters a missing assembly
[NoFuture.Util.FxPointers]::AddResolveAsmEventHandlerToDomain()

#set global switches
[NoFuture.Globals.Switches]::SqlFiltersOff = $true

Write-Progress -activity "Done" -status "complete" -PercentComplete 100
Write-HostAsciiArt "[\]"
Write-Host "=================================" -ForegroundColor DarkRed
Mssql-Settings #print it upon load

