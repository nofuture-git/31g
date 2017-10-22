Write-Host "Begin NoFuture build script" -ForegroundColor Yellow
$currentMsBuildVer = "14"
$targetDotNet = ".NET Framework 4.5"
$installedDotNetFrameworks = @()
$errors = @()

#get .NET versions
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
}
Get-ChildItem "hklm:SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\" | Get-ItemPropertyValue -Name Release | % {$installedDotNetFrameworks += $dotNet2ReleaseNum[$_]}

Write-Host "The following .NET framework version (4.5 and up) where found." -ForegroundColor Yellow
$installedDotNetFrameworks | % {
    Write-Host "`t$_" -ForegroundColor Gray
}

$isTargetDotNetFrameworkInstalled = $installedDotNetFrameworks.Count -gt 0
if(-not $isTargetDotNetFrameworkInstalled){
    $errors += ".NET Frameword version $targetDotNet is not installed."
}

#get list of all programs as they appear in classic "Add\Remove Programs"
Write-Host "Getting list of installed programs on this box." -ForegroundColor Yellow
$installedPrograms = Get-WmiObject -Class Win32_Product

#test for MSBuild
Write-Host "Testing for MSBuild ver. $currentMsBuildVer.0 at typical install location." -ForegroundColor Yellow
$msBuild = "C:\Program Files (x86)\MSBuild\$currentMsBuildVer.0\Bin\MSBuild.exe"
$isMsBuildInstalled = Test-Path $msBuild

if(-not $isMsBuildInstalled){
    $errors += "MSBuild ver. $currentMsBuildVer.0 was not found."
}

#test for Java and JAVA_HOME -required for NoFuture.Antlr
Write-Host "Testing for Java install (version 7 or higher)." -ForegroundColor Yellow
$isJavaInstalled = $installedPrograms | ? {$_.Name -match "Java\(TM\) SE Development Kit [7-9]" -or `
                                             $_.Name -match "Java [7-9]" }

Write-Host "Testing if the JAVA_HOME environment variable is set to a valid location." -ForegroundColor Yellow
$javaHomeAssigned = [System.Environment]::GetEnvironmentVariable("JAVA_HOME","Machine")
$isJavahomeAssigned = $javaHomeAssigned -ne $null -and (Test-Path $javaHomeAssigned)

Write-Host "Testing if the CLASSPATH environment variable is set to a valid location." -ForegroundColor Yellow
$javaCp = [System.Environment]::GetEnvironmentVariable("CLASSPATH","Machine")
$isJavaCpAssigned = $javaCp -ne $null -and (Test-Path $javaCp)

Write-Host "Testing if Java's \bin folder is present in the PATH environment variable." -ForegroundColor Yellow
$isJavaInPath = ($env:Path.Split(";") | ? {$_ -like "*\java\bin*"}).Length -gt 0

if(-not $isJavaInstalled){
    $errors += "Java ver. 7, 8, nor 9 were found."
}
if(-not $isJavahomeAssigned){
    $errors += "JAVA_HOME environment variable is not set."
}
if(-not $isJavaCpAssigned){
    $errors += "Java's CLASSPATH environment variable is not set."
}
if(-not $isJavaInPath){
    $errors += "Java's \bin folder is not added to the global PATH environment variable."
}

if(-not $isJavaInstalled -or -not $isJavahomeAssigned-or -not $isJavaCpAssigned -or -not $isJavaInPath){
    $errors += "`t`tThis is required to build NoFuture.Antlr (used to parse files and IL tokens)."
    $errors += "`t`tThe rest of the NoFuture projects will work without this, but runtime errors are possible."
}

#test for VC redist - required for DIA_SDK (pdb file parser)
Write-Host "Testing for Microsoft Visual C++ (version $currentMsBuildVer) Redist. install." -ForegroundColor Yellow
$isMsVc14Installed = $installedPrograms | ? {$_.Name -match "Microsoft Visual C\+\+ [2][0-9]{3} x(32|64) [^0-9]* $currentMsBuildVer\."}
if(-not $isMsVc14Installed){
    $errors += "Microsoft Visual C++ ... $currentMsBuildVer was not found."
    $errors += "`t`tThis is required to build the 'DIA SDK' VC++ project (a .pdb file parser)."
    $errors += "`t`tHowever, you may have no need of parsing .pdb files so it may be skipped."
}

Write-Host "----" -ForegroundColor DarkGray

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

#Push-Location .\NoFuture\Antlr

#& "$msBuild" NoFuture.Antlr.sln /t:build /tv:14.0 /m:8 /p:platform="any cpu" /p:configuration=debug /p:buildinparallel=true /p:CreateHardLinksForCopyFilesToOutputDirectoryIfPossible=true /p:CreateHardLinksForCopyAdditionalFilesIfPossible=true /p:CreateHardLinksForCopyLocalIfPossible=true /p:CreateHardLinksForPublishFilesIfPossible=true

#Pop-Location

#& "$msBuild" NoFuture.sln /t:build /tv:14.0 /m:8 /p:platform="any cpu" /p:configuration=debug /p:buildinparallel=true /p:CreateHardLinksForCopyFilesToOutputDirectoryIfPossible=true /p:CreateHardLinksForCopyAdditionalFilesIfPossible=true /p:CreateHardLinksForCopyLocalIfPossible=true /p:CreateHardLinksForPublishFilesIfPossible=true

$r = Read-Host "press any key to exit..."