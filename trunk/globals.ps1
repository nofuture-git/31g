

#get the list of loaded assemblies at session start
if($global:coreAssemblies -eq $null)
{
    $global:coreAssemblies = ([appdomain]::CurrentDomain.GetAssemblies() | % {$_.FullName}  | Sort-Object -Unique)
}

#hidden console proc of nslookup.exe in interactive mode
$Global:nslookup = $null

#keep this in memory
$Global:hostFile = (Join-Path $env:SystemRoot "System32\drivers\etc\hosts")
#each script file loads its own cmdlets into this List
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Clear()

#easier to access as a ps variable than a .NET static
$Global:AsmSearchDirs = [NoFuture.Shared.Core.NfConfig]::AssemblySearchPaths

#.NET global variables
$Global:gac = "C:\Windows\Microsoft.NET\assembly\GAC_MSIL"

$Global:net11 = "v1.1.4322"
$Global:net20 = "v2.0.50727"
$Global:net35 = "v3.5"
$Global:net40 = "v4.0.30319"

$Global:net11Path = (Join-Path "C:\WINDOWS\Microsoft.NET\Framework" $global:net11)
$Global:net20Path = (Join-Path "C:\WINDOWS\Microsoft.NET\Framework" $global:net20)
$Global:net35Path = (Join-Path "C:\WINDOWS\Microsoft.NET\Framework" $global:net35)
$Global:net40Path = (Join-Path "C:\WINDOWS\Microsoft.NET\Framework" $global:net40)

$Global:cscExe = "csc.exe"
$Global:vbcExe = "vbc.exe"
$Global:aspnetCompilerExe = "aspnet_compiler.exe"

#perm. global arrays
$Global:codeExtensions = [NoFuture.Shared.Core.NfConfig]::CodeFileExtensions | % { "*.{0}" -f $_}

$Global:configExtension = [NoFuture.Shared.Core.NfConfig]::ConfigFileExtensions | % { "*.{0}" -f $_}

$Global:excludeExtensions = [NoFuture.Shared.Core.NfConfig]::BinaryFileExtensions | % { "*.{0}" -f $_}