

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

[NoFuture.X64Tools]::SvcUtil = (Join-Path ([NoFuture.BinDirectories]::X64Root) "v4.0\svcUtil.exe")
[NoFuture.X64Tools]::Cdb = (Join-Path ([NoFuture.BinDirectories]::X64Root) "debug\cdb.exe")
[NoFuture.X64Tools]::TList = (Join-Path ([NoFuture.BinDirectories]::X64Root) "debug\tlist.exe")
[NoFuture.X64Tools]::SymChk = (Join-Path ([NoFuture.BinDirectories]::X64Root) "debug\symChk.exe")
[NoFuture.X64Tools]::Depends = (Join-Path ([NoFuture.BinDirectories]::X64Root) "depends\depends.exe")
[NoFuture.X64Tools]::Dumpbin = (Join-Path ([NoFuture.BinDirectories]::X64Root) "vs10\dumpbin.exe")
[NoFuture.X64Tools]::Ildasm = (Join-Path ([NoFuture.BinDirectories]::X64Root) "v4.0\ildasm.exe")
[NoFuture.X64Tools]::SqlCmd = (Join-Path ([NoFuture.BinDirectories]::X64Root) "sqlcmd\SQLCMD.EXE")
[NoFuture.X64Tools]::Wsdl = (Join-Path ([NoFuture.BinDirectories]::X64Root) "v4.0\wsdl.exe")
[NoFuture.X64Tools]::Mdbg = (Join-Path ([NoFuture.BinDirectories]::X64Root) "v4.0\Mdbg.exe")
[NoFuture.X64Tools]::ClrVer = (Join-Path ([NoFuture.BinDirectories]::X64Root) "v4.0\clrver.exe")
[NoFuture.X64Tools]::XsdExe = (Join-Path ([NoFuture.BinDirectories]::X64Root) "v4.0\xsd.exe")

[NoFuture.X86Tools]::Cdb = (Join-Path ([NoFuture.BinDirectories]::X86Root) "debug\cdb.exe")
[NoFuture.X86Tools]::Depends = (Join-Path ([NoFuture.BinDirectories]::X86Root) "depends\depends.exe")
[NoFuture.X86Tools]::Dumpbin = (Join-Path ([NoFuture.BinDirectories]::X86Root) "vs10\dumpbin.exe")
[NoFuture.X86Tools]::Ildasm = (Join-Path ([NoFuture.BinDirectories]::X86Root) "v4.0\ildasm.exe")
[NoFuture.X86Tools]::SqlMetal = (Join-Path ([NoFuture.BinDirectories]::X86Root) "sqlmetal.exe")
[NoFuture.X86Tools]::SvcUtil = (Join-Path ([NoFuture.BinDirectories]::X86Root) "v4.0\svcutil.exe")
[NoFuture.X86Tools]::TextTransform = (Join-Path ([NoFuture.BinDirectories]::X86Root) "TextTransform.exe")
[NoFuture.X86Tools]::Wsdl = (Join-Path ([NoFuture.BinDirectories]::X86Root) "v4.0\wsdl.exe")
[NoFuture.X86Tools]::DotExe = (Join-Path ([NoFuture.BinDirectories]::Root) "graphviz-2.38\bin\dot.exe")
[NoFuture.X86Tools]::NsLookupPort = 799

[NoFuture.JavaTools]::Javac = (Join-Path ([NoFuture.BinDirectories]::JavaRoot) "bin\javac.exe")
[NoFuture.JavaTools]::Java = (Join-Path ([NoFuture.BinDirectories]::JavaRoot) "bin\java.exe")
[NoFuture.JavaTools]::JavaDoc = (Join-Path ([NoFuture.BinDirectories]::JavaRoot) "bin\javadoc.exe")
[NoFuture.JavaTools]::JavaRtJar = (Join-Path ([NoFuture.BinDirectories]::JavaRoot) "jre\lib\rt.jar")
[NoFuture.JavaTools]::Jar = (Join-Path ([NoFuture.BinDirectories]::JavaRoot) "bin\jar.exe")
[NoFuture.JavaTools]::JRunScript = (Join-Path ([NoFuture.BinDirectories]::JavaRoot) "bin\jrunscript.exe")
[NoFuture.JavaTools]::Antlr = (Join-Path ([NoFuture.BinDirectories]::Root) "antlr-4.1-complete.jar")
[NoFuture.JavaTools]::JrePort = 780
[NoFuture.JavaTools]::StanfordPostTagger = (Join-Path ([NoFuture.BinDirectories]::Root) "stanford-postagger-2015-04-20\stanford-postagger.jar");
[NoFuture.JavaTools]::StanfordPostTaggerModels = (Join-Path ([NoFuture.BinDirectories]::Root) "stanford-postagger-2015-04-20\models\");

[NoFuture.CustomTools]::Dia2Dump = (Join-Path ([NoFuture.BinDirectories]::Root) "Dia2Dump.exe")
[NoFuture.CustomTools]::HostProc = (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Host.Proc.dll")
[NoFuture.CustomTools]::InvokeGetCgType = (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Gen.InvokeGetCgOfType.exe")
[NoFuture.CustomTools]::InvokeGraphViz = (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Gen.InvokeGraphViz.exe")
[NoFuture.CustomTools]::InvokeAssemblyAnalysis = (Join-Path ([NoFuture.BinDirectories]::Root) "NoFuture.Util.Gia.InvokeAssemblyAnalysis.exe")
[NoFuture.CustomTools]::RunTransparent = (Join-Path $global:mypshome "runTransparent.ps1")
[NoFuture.CustomTools]::CodeBase = (Join-Path $global:mypshome ".\Code\NoFuture")
if(Test-Path (Join-Path $global:mypshome "favicon.ico"))
{
    [NoFuture.CustomTools]::Favicon = (Join-Path $global:mypshome "favicon.ico")
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