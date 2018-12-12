#start load
if($Global:nfLoadedFromStart){break;}
Write-Progress -activity "Loading..." -status "no future" -PercentComplete 1
clear
function Get-MyFunctions(){
    [NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles
}
#most important of all globals
$Global:mypshome = Get-Location
$occultum = ".\__occultum"

function prompt {
    Write-Host ("{0}@{1} {2}" -f [System.Security.Principal.WindowsIdentity]::GetCurrent().Name, [System.Environment]::MachineName, $(pwd).Path) -ForegroundColor Green
    return "> "
}

#get the list of loaded assemblies at session start
if($global:coreAssemblies -eq $null)
{
    $global:coreAssemblies = ([appdomain]::CurrentDomain.GetAssemblies() | % {$_.FullName}  | Sort-Object -Unique)
}

$host.UI.RawUi.WindowTitle = "NoFuture"

#load all global session variables
Write-Progress -activity "Loading..." -status "location set" -PercentComplete 6

$noFutureBin = (Join-Path $global:mypshome "bin")

#copy my assemblies

 (ls (Join-Path $global:mypshome "Code\NoFuture\bin") -File ) | ? {$_.Name -notlike "*.vshost.*"} | % {
    $flName = $_.Name
    [System.IO.File]::WriteAllBytes([System.IO.Path]::Combine($noFutureBin, $flName), [System.IO.File]::ReadAllBytes($_.FullName))
 }
 
 (ls (Join-Path $global:mypshome "Code\NoFuture\bin\Templates") ) | % {
    $flName = $_.Name
    $nfTemplatesFolder = [System.IO.Path]::Combine($noFutureBin, "Templates")
    if(-not (Test-Path $nfTemplatesFolder)){
        $dnd = mkdir $nfTemplatesFolder
    }
    [System.IO.File]::WriteAllBytes([System.IO.Path]::Combine($nfTemplatesFolder, $flName), [System.IO.File]::ReadAllBytes($_.FullName))
 }

  (ls (Join-Path $global:mypshome "Code\NoFuture\bin\NoFuture.Util.Pos.Host") ) | % {
    $flName = $_.Name
    $nfPosHostFolder = [System.IO.Path]::Combine($noFutureBin, "NoFuture.Util.Pos.Host")
    if(-not (Test-Path $nfPosHostFolder)){
        $dnd = mkdir $nfPosHostFolder
    }
    [System.IO.File]::WriteAllBytes([System.IO.Path]::Combine($nfPosHostFolder, $flName), [System.IO.File]::ReadAllBytes($_.FullName))
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
    "NoFuture.Util.DotNetMeta, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.DotNetMeta.dll");
    "NoFuture.Util.Gia, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.Gia.dll");
    "NoFuture.Util.NfConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.NfConsole.dll");
    "NoFuture.Util.NfType, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.NfType.dll");
    "NoFuture.Util.Pos, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.Pos.dll");

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
$dnd = [NoFuture.Shared.Cfg.NfConfig]::Init(([NoFuture.Shared.Cfg.NfConfig]::FindNfConfigFile($noFutureBin)))
#=====================================


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
Import-Module .\random.ps1
Write-Progress -activity "Loading..." -status "random.ps1 loaded" -PercentComplete 63
Import-Module .\wsdl.ps1
Write-Progress -activity "Loading..." -status "wsdl.ps1 loaded" -PercentComplete 66
Import-Module .\debug.ps1
Write-Progress -activity "Loading..." -status "debug.ps1 loaded" -PercentComplete 69
Import-Module .\bin.ps1
Write-Progress -activity "Loading..." -status "bin.ps1 loaded" -PercentComplete 72

Import-Module .\sql.ps1
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
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Root,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Sql,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::StoredProx,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Code,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Debug,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Hbm,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Calendar,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::JavaSrc,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::JavaBuild,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::JavaDist,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::JavaArchive,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Graph,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::SvcUtil,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Wsdl,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::HttpAppDomain,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Audio,
[NoFuture.Shared.Cfg.NfConfig+TempDirectories]::TsvCsv) | ? {-not (Test-Path $_) } | % {$dnd = mkdir $_}

#set dir structure for http App Domain
$httpAppDomain = (Join-Path ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::HttpAppDomain) "bin")
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
    Copy-Item -Path (Join-Path ([NoFuture.Shared.Cfg.NfConfig+BinDirectories]::Root) $_) -Destination $httpAppDomain
  }

if(([NoFuture.Shared.Cfg.NfConfig]::Favicon) -ne $null -and (Test-Path ([NoFuture.Shared.Cfg.NfConfig]::Favicon)))
{
    cp -Path ([NoFuture.Shared.Cfg.NfConfig]::Favicon) -Destination ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::HttpAppDomain) -Force
}


#environment variables for JAVA
if($env:JAVA_HOME -eq $null -or $env:JAVA_HOME -ne ([NoFuture.Shared.Cfg.NfConfig+BinDirectories]::JavaRoot))
{
    [System.Environment]::SetEnvironmentVariable("JAVA_HOME",([NoFuture.Shared.Cfg.NfConfig+BinDirectories]::JavaRoot),"Machine")
}

$javaClassPath = ("{0}\*" -f ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::JavaDist))

if($env:CLASSPATH -ne $javaClassPath)
{
    [System.Environment]::SetEnvironmentVariable("CLASSPATH", $javaClassPath,"Machine")
}

#add our Java bin to the PATH
$javaPathVarEntry = (Join-Path ([NoFuture.Shared.Cfg.NfConfig+BinDirectories]::JavaRoot) "bin")
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

#copy Dia2Dump
$dia2DumpDir = Join-Path $Global:mypshome "Code\DIA SDK"
@("Dia2Dump.exe", "Dia2Dump.ilk", "Dia2Dump.pdb") | % {
    $dia2DumpSrc = Join-Path $dia2DumpDir $_ 
    $dia2DumpDest = Join-Path $noFutureBin $_
    if((Test-Path $dia2DumpSrc) -and (-not (Test-Path $dia2DumpDest))){
        Copy-Item -Path $dia2DumpSrc -Destination $dia2DumpDest -Force
    }
}

Write-Progress -activity "Loading..." -status "dependent paths created" -PercentComplete 93

#have the current app domain exec NoFuture FxPointers.ResolveAssemblyEventHandler whenever it encounters a missing assembly
[NoFuture.Util.Binary.FxPointers]::AddResolveAsmEventHandlerToDomain()

[NoFuture.Util.Binary.FxPointers]::AddAcceptAllCerts()

Write-Progress -activity "Done" -status "complete" -PercentComplete 100
Write-HostAsciiArt "[\]"
Write-Host "=================================" -ForegroundColor DarkRed
Get-MssqlSettings #print it upon load

$Global:nfLoadedFromStart = $true