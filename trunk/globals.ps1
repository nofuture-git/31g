

#get the list of loaded assemblies at session start
if($global:coreAssemblies -eq $null)
{
    $global:coreAssemblies = ([appdomain]::CurrentDomain.GetAssemblies() | % {$_.FullName}  | Sort-Object -Unique)
}

#$global:KeePassPasswordDatabase = "G:\Passwords.kdbx"

#session has a private aero console window - loaded only once
if($global:aeroWindow -ne $null)
{ Write-Host "transparent window is loaded" -ForegroundColor Yellow }

#the private hidden console proc interacted by StdIn/Out which host a JRE
$Global:jrunscript = $null

#hidden console proc of nslookup.exe in interactive mode
$Global:nslookup = $null

#this is the hash used by console.ps1::Start-ProcessRedirect
$Global:redirectProcs = @{}

#keep this in memory
$Global:hostFile = (Join-Path $env:SystemRoot "System32\drivers\etc\host")

[NoFuture.BinDirectories]::Root = (Join-Path $global:mypshome "bin")

#each script file loads its own cmdlets into this List
[NoFuture.MyFunctions]::FunctionFiles.Clear()

$net461SdkTools = ("${env:ProgramFiles(x86)}\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools")

#assign NoFuture properties - defined in NoFuture.ps1
[NoFuture.TempDirectories]::Root = (Join-Path $global:mypshome "temp")
[NoFuture.TempDirectories]::Sql = (Join-Path ([NoFuture.TempDirectories]::Root) "sql");
[NoFuture.TempDirectories]::StoredProcedures = (Join-Path ([NoFuture.TempDirectories]::Root) "procs");
[NoFuture.TempDirectories]::Code = (Join-Path ([NoFuture.TempDirectories]::Root) "code")
[NoFuture.TempDirectories]::Text = (Join-Path ([NoFuture.TempDirectories]::Root) "text")
[NoFuture.TempDirectories]::Debug = (Join-Path ([NoFuture.TempDirectories]::Root) "debug")
[NoFuture.TempDirectories]::Graph = (Join-Path ([NoFuture.TempDirectories]::Root) "graph")
[NoFuture.TempDirectories]::SvcUtil = (Join-Path ([NoFuture.TempDirectories]::Code) "svcUtil")
[NoFuture.TempDirectories]::Wsdl = (Join-Path ([NoFuture.TempDirectories]::Code) "wsdl")
[NoFuture.TempDirectories]::Hbm = (Join-Path ([NoFuture.TempDirectories]::Code) "hbm")
[NoFuture.TempDirectories]::Binary = (Join-Path ([NoFuture.TempDirectories]::Code) "tempBin")
[NoFuture.TempDirectories]::JavaSrc = (Join-Path ([NoFuture.TempDirectories]::Code) "java\src")
[NoFuture.TempDirectories]::JavaBuild = (Join-Path ([NoFuture.TempDirectories]::Code) "java\build")
[NoFuture.TempDirectories]::JavaDist = (Join-Path ([NoFuture.TempDirectories]::Code) "java\dist")
[NoFuture.TempDirectories]::JavaArchive = (Join-Path ([NoFuture.TempDirectories]::Code) "java\archive")
[NoFuture.TempDirectories]::Calendar = (Join-Path ([NoFuture.TempDirectories]::Root) "Calendar")
[NoFuture.TempDirectories]::HttpAppDomain = (Join-Path ([NoFuture.TempDirectories]::Root) "httpAppDomain")
[NoFuture.TempDirectories]::Audio = (Join-Path ([NoFuture.TempDirectories]::Root) "audio")

[NoFuture.TempFiles]::JavaScript = (Join-Path ([NoFuture.TempDirectories]::Root) "temp.js")
[NoFuture.TempFiles]::Html = (Join-Path ([NoFuture.TempDirectories]::Root) "temp.html")
[NoFuture.TempFiles]::Csv = (Join-Path ([NoFuture.TempDirectories]::Root) "temp.csv")
[NoFuture.TempFiles]::StdOut = (Join-Path ([NoFuture.TempDirectories]::Root) "stdout.txt")
[NoFuture.TempFiles]::T4Template = (Join-Path ([NoFuture.TempDirectories]::Root) "t4Temp.tt")
[NoFuture.TempFiles]::NetStat = (Join-Path ([NoFuture.TempDirectories]::Root) "netstat.txt")
[NoFuture.TempFiles]::Wmi = (Join-Path ([NoFuture.TempDirectories]::Text) "wmi.txt")

[NoFuture.BinDirectories]::X64Root = (Join-Path ([NoFuture.BinDirectories]::Root) "amd64")
[NoFuture.BinDirectories]::X86Root = (Join-Path ([NoFuture.BinDirectories]::Root) "x86")
[NoFuture.BinDirectories]::JavaRoot = (Join-Path ([NoFuture.BinDirectories]::Root) "java")
[NoFuture.BinDirectories]::T4Templates = (Join-Path ([NoFuture.BinDirectories]::Root) "Templates")
[NoFuture.BinDirectories]::PhpRoot = (Join-Path ([NoFuture.BinDirectories]::Root) "php")

[NoFuture.Tools.X64]::SvcUtil = (Join-Path ("$net461SdkTools\x64") "svcUtil.exe")
[NoFuture.Tools.X64]::Cdb = (Join-Path ([NoFuture.BinDirectories]::X64Root) "debug\cdb.exe")
[NoFuture.Tools.X64]::TList = (Join-Path ([NoFuture.BinDirectories]::X64Root) "debug\tlist.exe")
[NoFuture.Tools.X64]::SymChk = (Join-Path ([NoFuture.BinDirectories]::X64Root) "debug\symChk.exe")
[NoFuture.Tools.X64]::Depends = (Join-Path ([NoFuture.BinDirectories]::X64Root) "depends\depends.exe")
[NoFuture.Tools.X64]::Dumpbin = (Join-Path ([NoFuture.BinDirectories]::X64Root) "vs10\dumpbin.exe")
[NoFuture.Tools.X64]::Ildasm = (Join-Path ("$net461SdkTools\x64") "ildasm.exe")
[NoFuture.Tools.X64]::SqlCmd = (Join-Path ([NoFuture.BinDirectories]::X64Root) "sqlcmd\SQLCMD.EXE")
[NoFuture.Tools.X64]::Wsdl = (Join-Path ("$net461SdkTools\x64") "wsdl.exe")
[NoFuture.Tools.X64]::Mdbg = (Join-Path ([NoFuture.BinDirectories]::X64Root) "v4.0\Mdbg.exe")
[NoFuture.Tools.X64]::ClrVer = (Join-Path ("$net461SdkTools\x64") "clrver.exe")
[NoFuture.Tools.X64]::XsdExe = (Join-Path ("$net461SdkTools\x64") "xsd.exe")

[NoFuture.Tools.X86]::Cdb = (Join-Path ([NoFuture.BinDirectories]::X86Root) "debug\cdb.exe")
[NoFuture.Tools.X86]::Depends = (Join-Path ([NoFuture.BinDirectories]::X86Root) "depends\depends.exe")
[NoFuture.Tools.X86]::Dumpbin = (Join-Path ([NoFuture.BinDirectories]::X86Root) "vs10\dumpbin.exe")
[NoFuture.Tools.X86]::Ildasm = (Join-Path $net461SdkTools "ildasm.exe")
[NoFuture.Tools.X86]::SqlMetal = (Join-Path $net461SdkTools "sqlmetal.exe")
[NoFuture.Tools.X86]::SvcUtil = (Join-Path $net461SdkTools "svcUtil.exe")
[NoFuture.Tools.X86]::TextTransform = (Join-Path ([NoFuture.BinDirectories]::X86Root) "TextTransform.exe")
[NoFuture.Tools.X86]::Wsdl = (Join-Path $net461SdkTools "wsdl.exe")
[NoFuture.Tools.X86]::DotExe = (Join-Path ([NoFuture.BinDirectories]::Root) "graphviz-2.38\bin\dot.exe")
[NoFuture.Tools.X86]::NsLookupPort = 799

[NoFuture.Tools.JavaTools]::Javac = (Join-Path ([NoFuture.BinDirectories]::JavaRoot) "bin\javac.exe")
[NoFuture.Tools.JavaTools]::Java = (Join-Path ([NoFuture.BinDirectories]::JavaRoot) "bin\java.exe")
[NoFuture.Tools.JavaTools]::JavaDoc = (Join-Path ([NoFuture.BinDirectories]::JavaRoot) "bin\javadoc.exe")
[NoFuture.Tools.JavaTools]::JavaRtJar = (Join-Path ([NoFuture.BinDirectories]::JavaRoot) "jre\lib\rt.jar")
[NoFuture.Tools.JavaTools]::Jar = (Join-Path ([NoFuture.BinDirectories]::JavaRoot) "bin\jar.exe")
[NoFuture.Tools.JavaTools]::JRunScript = (Join-Path ([NoFuture.BinDirectories]::JavaRoot) "bin\jrunscript.exe")
[NoFuture.Tools.JavaTools]::Antlr = (Join-Path ([NoFuture.BinDirectories]::Root) "antlr-4.1-complete.jar")
[NoFuture.Tools.JavaTools]::JrePort = 780
[NoFuture.Tools.JavaTools]::StanfordPostTagger = (Join-Path ([NoFuture.BinDirectories]::Root) "stanford-postagger-2015-04-20\stanford-postagger.jar");
[NoFuture.Tools.JavaTools]::StanfordPostTaggerModels = (Join-Path ([NoFuture.BinDirectories]::Root) "stanford-postagger-2015-04-20\models\");

[NoFuture.Tools.CustomTools]::Dia2Dump = (Join-Path ([NoFuture.BinDirectories]::Root) "Dia2Dump.exe")
[NoFuture.Tools.CustomTools]::HostProc = (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Host.Proc.dll")
[NoFuture.Tools.CustomTools]::InvokeGetCgType = (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Gen.InvokeGetCgOfType.exe")
[NoFuture.Tools.CustomTools]::InvokeGraphViz = (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Gen.InvokeGraphViz.exe")
[NoFuture.Tools.CustomTools]::InvokeAssemblyAnalysis = (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Util.Gia.InvokeAssemblyAnalysis.exe")
[NoFuture.Tools.CustomTools]::RunTransparent = (Join-Path $global:mypshome "runTransparent.ps1")
[NoFuture.Tools.CustomTools]::CodeBase = (Join-Path $global:mypshome ".\Code\NoFuture")

[NoFuture.Tools.BinTools]::Ffmpeg = (Join-Path ([NoFuture.BinDirectories]::Root) "ffmpeg.exe")
[NoFuture.Tools.BinTools]::YoutubeDl = (Join-Path ([NoFuture.BinDirectories]::Root) "youtube-dl.exe")

if(Test-Path (Join-Path $global:mypshome "favicon.ico"))
{
    [NoFuture.Tools.CustomTools]::Favicon = (Join-Path $global:mypshome "favicon.ico")
}
#.NET global variables
$global:gac = "C:\Windows\Microsoft.NET\assembly\GAC_MSIL"

$global:net11 = "v1.1.4322"
$global:net20 = "v2.0.50727"
$global:net35 = "v3.5"
$global:net40 = "v4.0.30319"

$global:net11Path = (Join-Path "C:\WINDOWS\Microsoft.NET\Framework" $global:net11)
$global:net20Path = (Join-Path "C:\WINDOWS\Microsoft.NET\Framework" $global:net20)
$global:net35Path = (Join-Path "C:\WINDOWS\Microsoft.NET\Framework" $global:net35)
$global:net40Path = (Join-Path "C:\WINDOWS\Microsoft.NET\Framework" $global:net40)

$global:cscExe = "csc.exe"
$global:vbcExe = "vbc.exe"
$global:aspnetCompilerExe = "aspnet_compiler.exe"

#perm. global arrays
$global:codeExtensions = [NoFuture.Shared.Constants]::CodeExtensions | % { "*.{0}" -f $_}

$global:configExtension = [NoFuture.Shared.Constants]::ConfigExtensions | % { "*.{0}" -f $_}

$global:excludeExtensions = [NoFuture.Shared.Constants]::BinaryExtensions | % { "*.{0}" -f $_}