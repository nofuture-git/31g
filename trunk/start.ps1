#start load
if($Global:nfLoadedFromStart){break;}
Write-Progress -activity "Loading..." -status "no future" -PercentComplete 1
clear
function Get-MyFunctions(){
    [NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles
}
#most important of all globals
$Global:mypshome = Get-Location
Set-Location $global:mypshome
$occultum = ".\__occultum"

function prompt {
    Write-Host ("{0}@{1} {2}" -f [System.Security.Principal.WindowsIdentity]::GetCurrent().Name, [System.Environment]::MachineName, $(pwd).Path) -ForegroundColor Green
    return "> "
}

$host.UI.RawUi.WindowTitle = "NoFuture"

#load all global session variables
Write-Progress -activity "Loading..." -status "location set" -PercentComplete 6

$noFutureBin = (Join-Path $global:mypshome "bin")

#copy my assemblies

 (ls (Join-Path $global:mypshome "Code\NoFuture\bin") -File ) | ? {$_.Name -notlike "*.vhost.*"} | % {
    $flName = $_.Name
    [System.IO.File]::WriteAllBytes([System.IO.Path]::Combine($noFutureBin, $flName), [System.IO.File]::ReadAllBytes($_.FullName))
 }
 (ls (Join-Path $global:mypshome "Code\NoFuture\bin\Data\Source") ) | % {
    cp -Path $_.FullName -Destination (Join-Path $noFutureBin "Data\Source") -Force -ErrorAction SilentlyContinue
 }
 
 (ls (Join-Path $global:mypshome "Code\NoFuture\bin\Templates") ) | % {
    cp -Path $_.FullName -Destination (Join-Path $noFutureBin "Templates") -Force -ErrorAction SilentlyContinue
 }
 
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
    "Antlr4.Runtime, Version=4.5.0.0, Culture=neutral, PublicKeyToken=09abb75b9ed49849" = (Join-Path $noFutureBin "Antlr4.Runtime.dll");
    "NoFuture.Antlr, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Antlr.dll");
    "Jurassic, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "Jurassic.dll");
	"NoFuture.Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Shared.dll");
    "NoFuture.Encryption, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Encryption.dll");
    "NoFuture.Util, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.dll");
    "NoFuture.Util.Pos, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.Pos.dll");
    "NoFuture.Read, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Read.dll");
	"NoFuture.Tokens, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Tokens.dll");
    "NoFuture.Rand, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.dll");
    "NoFuture.Rand.Data.Source, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Rand.Data.Source.dll");
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
    "Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed" = (Join-Path $noFutureBin "Newtonsoft.Json.dll");
    "itextsharp, Version=5.5.9.0, Culture=neutral, PublicKeyToken=8354ae6d2174ddca" = (Join-Path $noFutureBin "itextsharp.dll");
    "HtmlAgilityPack, Version=2.0.20.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a" = (Join-Path $noFutureBin "HtmlAgilityPack.dll");
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

#=====================================
#intialize paths from nfConfig.cfg.xml
$dnd = [NoFuture.Shared.NfConfig]::Init(([NoFuture.Shared.NfConfig]::FindNfConfigFile($noFutureBin)))
#=====================================

. .\globals.ps1
Write-Progress -activity "Loading..." -status "globals have been loaded" -PercentComplete 8

#load all powershell source-code files
Write-Progress -activity "Loading..." -status ".NET assemblies been loaded" -PercentComplete 12
Import-Module .\console.ps1 -Force
Write-Progress -activity "Loading..." -status "console.ps1 loaded" -PercentComplete 33
Import-Module .\regex.ps1
Write-Progress -activity "Loading..." -status "regex.ps1 loaded" -PercentComplete 37
Import-Module .\format.ps1
Write-Progress -activity "Loading..." -status "format.ps1 loaded" -PercentComplete 39
Import-Module .\print.ps1
Write-Progress -activity "Loading..." -status "print.ps1 loaded" -PercentComplete 42
Import-Module .\tokens.ps1
Write-Progress -activity "Loading..." -status "tokens.ps1 loaded" -PercentComplete 46
Import-Module .\trace.ps1
Write-Progress -activity "Loading..." -status "trace.ps1 loaded" -PercentComplete 49
Import-Module .\util.ps1
Write-Progress -activity "Loading..." -status "util.ps1 loaded" -PercentComplete 51
Import-Module .\net.ps1
Write-Progress -activity "Loading..." -status "net.ps1 loaded" -PercentComplete 54
Import-Module .\crypto.ps1
Write-Progress -activity "Loading..." -status "crypto.ps1 loaded" -PercentComplete 57
Import-Module .\outlook.ps1
Write-Progress -activity "Loading..." -status "outlook.ps1 loaded" -PercentComplete 60
Import-Module .\random.ps1
Write-Progress -activity "Loading..." -status "random.ps1 loaded" -PercentComplete 63
Import-Module .\wsdl.ps1
Write-Progress -activity "Loading..." -status "wsdl.ps1 loaded" -PercentComplete 66
Import-Module .\debug.ps1
Write-Progress -activity "Loading..." -status "debug.ps1 loaded" -PercentComplete 69
Import-Module .\bin.ps1
Write-Progress -activity "Loading..." -status "bin.ps1 loaded" -PercentComplete 72
Import-Module .\gen.ps1
Write-Progress -activity "Loading..." -status "gen.ps1 loaded" -PercentComplete 74

Import-Module .\sql.ps1
Import-Module .\hbm.ps1
Write-Progress -activity "Loading..." -status "sql.ps1 loaded" -PercentComplete 85

if(Test-Path $occultum)
{
    Write-Progress -activity "Loading..." -status "$occultum loaded" -PercentComplete 80
	. ".\$occultum\hiddenStart.ps1"
}
#instantiate global transparent window, print all loaded funtions to it
Write-Progress -activity "Loading..." -status "setting global variables" -PercentComplete 89

#create any required temp folders
@(
[NoFuture.Shared.NfConfig+TempDirectories]::Root,
[NoFuture.Shared.NfConfig+TempDirectories]::Sql,
[NoFuture.Shared.NfConfig+TempDirectories]::StoredProx,
[NoFuture.Shared.NfConfig+TempDirectories]::Code,
[NoFuture.Shared.NfConfig+TempDirectories]::Debug,
[NoFuture.Shared.NfConfig+TempDirectories]::Hbm,
[NoFuture.Shared.NfConfig+TempDirectories]::Calendar,
[NoFuture.Shared.NfConfig+TempDirectories]::JavaSrc,
[NoFuture.Shared.NfConfig+TempDirectories]::JavaBuild,
[NoFuture.Shared.NfConfig+TempDirectories]::JavaDist,
[NoFuture.Shared.NfConfig+TempDirectories]::JavaArchive,
[NoFuture.Shared.NfConfig+TempDirectories]::Graph,
[NoFuture.Shared.NfConfig+TempDirectories]::SvcUtil,
[NoFuture.Shared.NfConfig+TempDirectories]::Wsdl,
[NoFuture.Shared.NfConfig+TempDirectories]::HttpAppDomain,
[NoFuture.Shared.NfConfig+TempDirectories]::Audio,
[NoFuture.Shared.NfConfig+TempDirectories]::TsvCsv) | ? {-not (Test-Path $_) } | % {$dnd = mkdir $_}

#set dir structure for http App Domain
$httpAppDomain = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::HttpAppDomain) "bin")
if(-not(Test-Path $httpAppDomain)){ $dnd = mkdir $httpAppDomain }
@("NoFuture.Domain.Carriage.exe",
  "NoFuture.Domain.Carriage.exe.config",
  "NoFuture.Domain.Carriage.pdb",
  "NoFuture.Domain.Engine.dll",
  "NoFuture.Domain.Engine.pdb",
  "NoFuture.Shared.dll",
  "NoFuture.Shared.pdb",
  "NoFuture.Util.dll",
  "NoFuture.Util.pdb") | % {
    Copy-Item -Path (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) $_) -Destination $httpAppDomain
  }

if(([NoFuture.Shared.NfConfig]::Favicon) -ne $null -and (Test-Path ([NoFuture.Shared.NfConfig]::Favicon)))
{
    cp -Path ([NoFuture.Shared.NfConfig]::Favicon) -Destination ([NoFuture.Shared.NfConfig+TempDirectories]::HttpAppDomain) -Force
}


#environment variables for JAVA
if($env:JAVA_HOME -eq $null -or $env:JAVA_HOME -ne ([NoFuture.Shared.NfConfig+BinDirectories]::JavaRoot))
{
    [System.Environment]::SetEnvironmentVariable("JAVA_HOME",([NoFuture.Shared.NfConfig+BinDirectories]::JavaRoot),"Machine")
}

$javaClassPath = ("{0}\*" -f ([NoFuture.Shared.NfConfig+TempDirectories]::JavaDist))

if($env:CLASSPATH -ne $javaClassPath)
{
    [System.Environment]::SetEnvironmentVariable("CLASSPATH", $javaClassPath,"Machine")
}

#add our Java bin to the PATH
$javaPathVarEntry = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::JavaRoot) "bin")
if((Test-Path $javaPathVarEntry) -and -not($env:Path.Contains($javaPathVarEntry))){
    $pathParts = $env:Path.Split(";")
    $pathParts += $javaPathVarEntry
    [System.Environment]::SetEnvironmentVariable("Path", ([string]::Join(";",$pathParts)),"Machine")
}

#add our NoFuture bin to the PATH
if($env:Path.Split(";") -notcontains $noFutureBin){
    $pathParts = $env:Path.Split(";")
    $pathParts += $noFutureBin
    [System.Environment]::SetEnvironmentVariable("Path", ([string]::Join(";",$pathParts)),"Machine")
}

Write-Progress -activity "Loading..." -status "dependent paths created" -PercentComplete 93

#have the current app domain exec NoFuture.FxPointers.ResolveAssemblyEventHandler whenever it encounters a missing assembly
[NoFuture.Util.FxPointers]::AddResolveAsmEventHandlerToDomain()

[NoFuture.Util.FxPointers]::AddAcceptAllCerts()


#remove any old temp data from previous sessions
Write-Progress -Activity "removing old AppData files" -Status "OK" -PercentComplete 96
ls -Path ([NoFuture.Shared.NfConfig+TempDirectories]::AppData) -Filter *.* | % {
    Remove-Item -Path $_.FullName -Force
}

Write-Progress -activity "Done" -status "complete" -PercentComplete 100
Write-HostAsciiArt "[\]"
Write-Host "=================================" -ForegroundColor DarkRed
Get-MssqlSettings #print it upon load

$Global:nfLoadedFromStart = $true