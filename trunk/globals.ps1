

#get the list of loaded assemblies at session start
if($global:coreAssemblies -eq $null)
{
    $global:coreAssemblies = ([appdomain]::CurrentDomain.GetAssemblies() | % {$_.FullName}  | Sort-Object -Unique)
}

#hidden console proc of nslookup.exe in interactive mode
$Global:nslookup = $null

#keep this in memory
$Global:hostFile = (Join-Path $env:SystemRoot "System32\drivers\etc\host")

[NoFuture.Shared.NfConfig+BinDirectories]::Root = (Join-Path $global:mypshome "bin")

#each script file loads its own cmdlets into this List
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Clear()

$net461SdkTools = ("${env:ProgramFiles(x86)}\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools")

#assign NoFuture properties - defined in NoFuture.ps1
[NoFuture.Shared.NfConfig+TempDirectories]::Root = (Join-Path $global:mypshome "temp")
[NoFuture.Shared.NfConfig+TempDirectories]::Sql = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "sql");
[NoFuture.Shared.NfConfig+TempDirectories]::StoredProcedures = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "procs");
[NoFuture.Shared.NfConfig+TempDirectories]::Code = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "code")
[NoFuture.Shared.NfConfig+TempDirectories]::Text = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "text")
[NoFuture.Shared.NfConfig+TempDirectories]::Debug = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "debug")
[NoFuture.Shared.NfConfig+TempDirectories]::Graph = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "graph")
[NoFuture.Shared.NfConfig+TempDirectories]::SvcUtil = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Code) "svcUtil")
[NoFuture.Shared.NfConfig+TempDirectories]::Wsdl = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Code) "wsdl")
[NoFuture.Shared.NfConfig+TempDirectories]::Hbm = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Code) "hbm")
[NoFuture.Shared.NfConfig+TempDirectories]::Binary = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Code) "tempBin")
[NoFuture.Shared.NfConfig+TempDirectories]::JavaSrc = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Code) "java\src")
[NoFuture.Shared.NfConfig+TempDirectories]::JavaBuild = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Code) "java\build")
[NoFuture.Shared.NfConfig+TempDirectories]::JavaDist = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Code) "java\dist")
[NoFuture.Shared.NfConfig+TempDirectories]::JavaArchive = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Code) "java\archive")
[NoFuture.Shared.NfConfig+TempDirectories]::Calendar = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "Calendar")
[NoFuture.Shared.NfConfig+TempDirectories]::HttpAppDomain = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "httpAppDomain")
[NoFuture.Shared.NfConfig+TempDirectories]::Audio = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "audio")
[NoFuture.Shared.NfConfig+TempDirectories]::TsvCsv = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "tsvCsv")

[NoFuture.Shared.NfConfig+TempFiles]::JavaScript = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "temp.js")
[NoFuture.Shared.NfConfig+TempFiles]::Html = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "temp.html")
[NoFuture.Shared.NfConfig+TempFiles]::Csv = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "temp.csv")
[NoFuture.Shared.NfConfig+TempFiles]::StdOut = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "stdout.txt")
[NoFuture.Shared.NfConfig+TempFiles]::T4Template = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "t4Temp.tt")
[NoFuture.Shared.NfConfig+TempFiles]::NetStat = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Root) "netstat.txt")
[NoFuture.Shared.NfConfig+TempFiles]::Wmi = (Join-Path ([NoFuture.Shared.NfConfig+TempDirectories]::Text) "wmi.txt")

[NoFuture.Shared.NfConfig+BinDirectories]::X64Root = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "amd64")
[NoFuture.Shared.NfConfig+BinDirectories]::X86Root = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "x86")
[NoFuture.Shared.NfConfig+BinDirectories]::JavaRoot = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "java")
[NoFuture.Shared.NfConfig+BinDirectories]::T4Templates = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "Templates")
[NoFuture.Shared.NfConfig+BinDirectories]::PhpRoot = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "php")
[NoFuture.Shared.NfConfig+BinDirectories]::DataRoot = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "Data\Source")

[NoFuture.Shared.NfConfig+X64]::SvcUtil = (Join-Path ("$net461SdkTools\x64") "svcUtil.exe")
[NoFuture.Shared.NfConfig+X64]::Cdb = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::X64Root) "debug\cdb.exe")
[NoFuture.Shared.NfConfig+X64]::TList = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::X64Root) "debug\tlist.exe")
[NoFuture.Shared.NfConfig+X64]::SymChk = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::X64Root) "debug\symChk.exe")
[NoFuture.Shared.NfConfig+X64]::Depends = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::X64Root) "depends\depends.exe")
[NoFuture.Shared.NfConfig+X64]::Dumpbin = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::X64Root) "vs10\dumpbin.exe")
[NoFuture.Shared.NfConfig+X64]::Ildasm = (Join-Path ("$net461SdkTools\x64") "ildasm.exe")
[NoFuture.Shared.NfConfig+X64]::SqlCmd = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::X64Root) "sqlcmd\SQLCMD.EXE")
[NoFuture.Shared.NfConfig+X64]::Wsdl = (Join-Path ("$net461SdkTools\x64") "wsdl.exe")
[NoFuture.Shared.NfConfig+X64]::Mdbg = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::X64Root) "v4.0\Mdbg.exe")
[NoFuture.Shared.NfConfig+X64]::ClrVer = (Join-Path ("$net461SdkTools\x64") "clrver.exe")
[NoFuture.Shared.NfConfig+X64]::XsdExe = (Join-Path ("$net461SdkTools\x64") "xsd.exe")

[NoFuture.Shared.NfConfig+X86]::Cdb = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::X86Root) "debug\cdb.exe")
[NoFuture.Shared.NfConfig+X86]::Depends = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::X86Root) "depends\depends.exe")
[NoFuture.Shared.NfConfig+X86]::Dumpbin = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::X86Root) "vs10\dumpbin.exe")
[NoFuture.Shared.NfConfig+X86]::Ildasm = (Join-Path $net461SdkTools "ildasm.exe")
[NoFuture.Shared.NfConfig+X86]::SqlMetal = (Join-Path $net461SdkTools "sqlmetal.exe")
[NoFuture.Shared.NfConfig+X86]::SvcUtil = (Join-Path $net461SdkTools "svcUtil.exe")
[NoFuture.Shared.NfConfig+X86]::TextTransform = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::X86Root) "TextTransform.exe")
[NoFuture.Shared.NfConfig+X86]::Wsdl = (Join-Path $net461SdkTools "wsdl.exe")
[NoFuture.Shared.NfConfig+X86]::DotExe = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "graphviz-2.38\bin\dot.exe")
[NoFuture.Shared.NfConfig+NfDefaultPorts]::NsLookupPort = 799

[NoFuture.Shared.NfConfig+JavaTools]::Javac = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::JavaRoot) "bin\javac.exe")
[NoFuture.Shared.NfConfig+JavaTools]::Java = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::JavaRoot) "bin\java.exe")
[NoFuture.Shared.NfConfig+JavaTools]::JavaDoc = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::JavaRoot) "bin\javadoc.exe")
[NoFuture.Shared.NfConfig+JavaTools]::JavaRtJar = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::JavaRoot) "jre\lib\rt.jar")
[NoFuture.Shared.NfConfig+JavaTools]::Jar = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::JavaRoot) "bin\jar.exe")
[NoFuture.Shared.NfConfig+JavaTools]::JRunScript = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::JavaRoot) "bin\jrunscript.exe")
[NoFuture.Shared.NfConfig+JavaTools]::Antlr = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "antlr-4.1-complete.jar")
[NoFuture.Shared.NfConfig+JavaTools]::StanfordPostTagger = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "stanford-postagger-2015-12-09\stanford-postagger.jar");
[NoFuture.Shared.NfConfig+JavaTools]::StanfordPostTaggerModels = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "stanford-postagger-2015-12-09\models\");

[NoFuture.Shared.NfConfig+CustomTools]::Dia2Dump = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "Dia2Dump.exe")
[NoFuture.Shared.NfConfig+CustomTools]::InvokeGetCgType = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "NoFuture.Gen.InvokeGetCgOfType.exe")
[NoFuture.Shared.NfConfig+CustomTools]::InvokeGraphViz = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "NoFuture.Gen.InvokeGraphViz.exe")
[NoFuture.Shared.NfConfig+CustomTools]::InvokeAssemblyAnalysis = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "NoFuture.Util.Gia.InvokeAssemblyAnalysis.exe")
[NoFuture.Shared.NfConfig+CustomTools]::InvokeFlatten = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "NoFuture.Util.Gia.InvokeFlatten.exe")
[NoFuture.Shared.NfConfig+CustomTools]::UtilPosHost = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "NoFuture.Util.Pos.Host.exe")
[NoFuture.Shared.NfConfig+CustomTools]::InvokeDpx = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "NoFuture.Util.Binary.InvokeDpx.exe")
[NoFuture.Shared.NfConfig+CustomTools]::InvokeNfTypeName = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "NoFuture.Tokens.InvokeNfTypeName.exe")

[NoFuture.Shared.NfConfig+BinTools]::Ffmpeg = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "ffmpeg.exe")
[NoFuture.Shared.NfConfig+BinTools]::YoutubeDl = (Join-Path ([NoFuture.Shared.NfConfig+BinDirectories]::Root) "youtube-dl.exe")


[NoFuture.Shared.NfConfig+CustomTools]::CodeBase = (Join-Path $global:mypshome ".\Code\NoFuture")

if(Test-Path (Join-Path $global:mypshome "favicon.ico"))
{
    [NoFuture.Shared.NfConfig+CustomTools]::Favicon = (Join-Path $global:mypshome "favicon.ico")
}

#easier to access as a ps variable than a .NET static
$global:AsmSearchDirs = [NoFuture.Shared.NfConfig]::AssemblySearchPaths

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
$global:codeExtensions = [NoFuture.Shared.NfConfig]::CodeExtensions | % { "*.{0}" -f $_}

$global:configExtension = [NoFuture.Shared.NfConfig]::ConfigExtensions | % { "*.{0}" -f $_}

$global:excludeExtensions = [NoFuture.Shared.NfConfig]::BinaryExtensions | % { "*.{0}" -f $_}