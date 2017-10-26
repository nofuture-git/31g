Write-Host "Begin NoFuture build script" -ForegroundColor Yellow
$currentMsBuildVer = "14"
$targetDotNet = ".NET Framework 4.7"
$installedDotNetFrameworks = @()
$errors = @()

#get list of all programs as they appear in classic "Add\Remove Programs"
Write-Host "Getting list of installed programs on this box (this may take awhile)." -ForegroundColor Yellow
$installedPrograms = Get-WmiObject -Class Win32_Product

#===================================
#TEST FOR .NET FRAMEWORK
Write-Host (New-Object System.String('-',80)) -ForegroundColor Green
Write-Host "Getting installed version of Microsoft.NET." -ForegroundColor Yellow
#https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed#ps_a
$dotNet2ReleaseNum = @{
378389 = ".NET Framework 4.5";
378675 = ".NET Framework 4.5.1";
379893 = ".NET Framework 4.5.2";
393295 = ".NET Framework 4.6";
394254 = ".NET Framework 4.6.1";
394802 = ".NET Framework 4.6.2";
460798 = ".NET Framework 4.7";
461308 = ".NET Framework 4.7.1";
461310 = ".NET Framework 4.7.1";
}
Get-ChildItem "hklm:SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\" | Get-ItemProperty -Name Release | % {
 $installedDotNetFrameworks += $dotNet2ReleaseNum[$_.Release]
}

Write-Host "The following .NET framework version (4.5 and up) where found." -ForegroundColor Yellow
$installedDotNetFrameworks | % {
    Write-Host "`t$_" -ForegroundColor Gray
}

$isTargetDotNetFrameworkInstalled = $installedDotNetFrameworks.Count -gt 0
if(-not $isTargetDotNetFrameworkInstalled){
    $errors += ".NET Framework version $targetDotNet is not installed. [REQUIRED]"
}
#===================================


#===================================
#TEST FOR MSBUILD (pre VS 2017)
Write-Host (New-Object System.String('-',80)) -ForegroundColor Green
Write-Host "Testing for MSBuild ver. $currentMsBuildVer.0 at typical install location." -ForegroundColor Yellow
$msBuild = "C:\Program Files (x86)\MSBuild\$currentMsBuildVer.0\Bin\MSBuild.exe"
$isMsBuildInstalled = Test-Path $msBuild

if(-not $isMsBuildInstalled){
    $errors += "MSBuild ver. $currentMsBuildVer.0 was not found. [REQUIRED]"
}
else{
    Write-Host "MSBuild.exe found at:" -ForegroundColor Yellow
    Write-Host ("`t$msBuild") -ForegroundColor Gray
}
#===================================

#===================================
#TEST FOR .NET CORE
Write-Host (New-Object System.String('-',80)) -ForegroundColor Green
Write-Host "Testing for .NET Core install" -ForegroundColor Yellow
$isDotNetCoreInstalled = $installedPrograms | ? {$_.Name -like "Microsoft .NET Core SDK*"}

if(-not $isDotNetCoreInstalled){
    $errors += "The Microsoft .NET Core SDK was not found. [NOT REQUIRED]"
}
else{
    Write-Host "The Microsoft .NET Core SDK was found at:" -ForegroundColor Yellow
    $isDotNetCoreInstalled | % {
        Write-Host ("`t{0}" -f $_.Name ) -ForegroundColor Gray
    }
}
#===================================

#===================================
#TEST FOR JAVA
Write-Host (New-Object System.String('-',80)) -ForegroundColor Green
Write-Host "Testing for Java install (version 7 or higher)." -ForegroundColor Yellow
$isJavaInstalled = $installedPrograms | ? {$_.Name -match "Java\(TM\) SE Development Kit [7-9]" -or `
                                             $_.Name -match "Java [7-9]" }
$javaHomeAssigned = [System.Environment]::GetEnvironmentVariable("JAVA_HOME")
$isJavahomeAssigned = $javaHomeAssigned -ne $null -and (Test-Path $javaHomeAssigned)
$isJavaInPath = $env:Path.Split(";") | ? {Test-Path (Join-Path $_ "java.exe")}

if(-not $isJavaInstalled){
    $errors += "Java ver. 7, 8, nor 9 were found.  [NOT REQUIRED]"
}
else{
    Write-Host "The following Java Versions were found:" -ForegroundColor Yellow
    $isJavaInstalled | % {
        Write-Host ("`t{0}" -f $_.Name ) -ForegroundColor Gray
    }
}
if(-not $isJavahomeAssigned){
    $errors += "JAVA_HOME environment variable is not set.  [NOT REQUIRED]"
}
else{
    Write-Host "JAVA_HOME environment variable assigned to:" -ForegroundColor Yellow
    Write-Host ("`t$javaHomeAssigned") -ForegroundColor Gray
}
if(-not $isJavaInPath){
    $errors += "java.exe was not found in the global PATH environment variable.  [NOT REQUIRED]"
}
else{
    Write-Host "The following java.exe(s) were found in PATH environment variable:" -ForegroundColor Yellow
    $isJavaInPath | % {
        Write-Host "`t$_" -ForegroundColor Gray
    }
}
#=================================

#===================================
#TEST FOR C++
Write-Host (New-Object System.String('-',80)) -ForegroundColor Green
Write-Host "Testing for C++ Redist. install." -ForegroundColor Yellow
$isMsVc14Installed = $installedPrograms | ? {$_.Name -match "Microsoft Visual C\+\+ [2][0-9]{3} x(32|64) (Minimum|Additional) Runtime [^0-9]*"}
if(-not $isMsVc14Installed){
    $errors += "Microsoft Visual C++ [...] Runtime $currentMsBuildVer was not found. [NOT REQUIRED]"
}
else{
    Write-Host "The following C++ Runtimes were found" -ForegroundColor Yellow
    $isMsVc14Installed | % {
        Write-Host ("`t{0}" -f $_.Name ) -ForegroundColor Gray
    }
}
#===================================

#===================================
#TEST FOR PYTHON
Write-Host (New-Object System.String('-',80)) -ForegroundColor Green
Write-Host "Testing for Python environments" -ForegroundColor Yellow
$isPyPathSet = [System.Environment]::GetEnvironmentVariable("PYTHONPATH")
$isPyHomeSet = [System.Environment]::GetEnvironmentVariable("PYTHONHOME")
$isPythonInstalled = $installedPrograms | ? {$_.Name -match "Python [0-9\.]+ (Executables|Core|Standard)"}
$isPythonInPath = $env:Path.Split(";") | ? {Test-Path (Join-Path $_ "python.exe")}
if(-not $isPyPathSet){
    $errors += "PYTHONPATH environment variable is not set. [NOT REQUIRED]"
}
else{
    Write-Host "PYTHONPATH environment variable assigned to:" -ForegroundColor Yellow
    Write-Host ("`t$isPyPathSet") -ForegroundColor Gray
}
if(-not $isPyHomeSet){
    $errors += "PYTHONHOME environment variable is not set. [NOT REQUIRED]"
}
else{
    Write-Host "PYTHONHOME environment variable assigned to:" -ForegroundColor Yellow
    Write-Host ("`t$isPyHomeSet") -ForegroundColor Gray
}
if(-not $isPythonInstalled){
    $errors += "No Python environments were found. [NOT REQUIRED]"
}
else{
    Write-Host "The following Python environment(s) were found:" -ForegroundColor Yellow
    $isPythonInstalled | % {
        Write-Host ("`t{0}" -f $_.Name ) -ForegroundColor Gray
    }
}
if(-not $isPythonInPath){
    $errors += "python.exe was not found in the global PATH environment variable.  [NOT REQUIRED]"
}
else{
    Write-Host "The following python.exe(s) were found in PATH environment variable:" -ForegroundColor Yellow
    $isPythonInPath | % {
        Write-Host "`t$_" -ForegroundColor Gray
    }
}
#===================================

Write-Host (New-Object System.String('-',80)) -ForegroundColor DarkGray

foreach($err in $errors){
    Write-Host $err -ForegroundColor Cyan
}

if($errors.Length -gt 0){
    $yon = Write-Host "Errors were detected (see above), the build script may run with errors resulting in a partial build." -ForegroundColor Magenta
    $yon = Read-Host "Do you wish to continue? [Y|N]"
    if($yon -ne "Y"){
        break;
    }
}

#TODO - MSBuild bound to Visual Studio now, to build with .NET Core is a lot of changes
& "$msBuild" NoFuture.sln /t:build /tv:14.0 /m:8 /p:platform="any cpu" /p:configuration=debug /p:buildinparallel=true /p:CreateHardLinksForCopyFilesToOutputDirectoryIfPossible=true /p:CreateHardLinksForCopyAdditionalFilesIfPossible=true /p:CreateHardLinksForCopyLocalIfPossible=true /p:CreateHardLinksForPublishFilesIfPossible=true

$r = Read-Host "press any key to exit..."