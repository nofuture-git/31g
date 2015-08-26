#Powershell.exe -executionpolicy remotesigned -File  $(ProjectDir)\PreBuildScript.ps1 $(ProjectDir)

$workingDir = $args[0]

Write-Host "Pre-Build Script: Arg received as $workingDir"

$getGrammer = (Join-Path $workingDir "Grammers")

Write-Host "Pre-Build Script: Looking for .g4 files in $getGrammer"

if(-not(Test-Path $getGrammer)){
    Write-Host "Pre-Build Script: $getGrammer path does not exist"
    break;
}

$grammerNames = ( ls -Path $getGrammer | ? {$_.Extension -eq ".g4"} | % {
    $_.Name.Replace("Lexer.g4",[string]::Empty).Replace("Parser.g4",[string]::Empty)
} | Sort-Object -Unique)

if($grammerNames -eq $null -or $grammerNames.Length -le 0){
    Write-Host "Pre-Build Script: no grammer files located"
    break;
}

$matchName = "({0}.*?\.cs)" -f  [string]::Join(".*?\.cs)|(",$grammerNames)

Write-Host "Pre-Build Script: Looking for any files whose name matches $matchName"

ls -Path $workingDir | ? {$_.Name -match $matchName } | % {
    $csFileFullName = $_.FullName
    Write-Host "Pre-Build Script: Removing $csFileFullName"
    Remove-Item -Path $csFileFullName -Force
}