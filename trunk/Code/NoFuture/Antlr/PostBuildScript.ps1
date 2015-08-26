#Powershell.exe -executionpolicy remotesigned -File  $(ProjectDir)\PostBuildScript.ps1 $(ProjectDir)

$workingDir = $args[0]

Write-Host "Post-Build Script: Arg received as $workingDir"

$getGrammer = (Join-Path $workingDir "Grammers")

Write-Host "Post-Build Script: Looking for .g4 files in $getGrammer"

if(-not(Test-Path $getGrammer)){
    Write-Host "Post-Build Script: $getGrammer path does not exist"
    break;
}

$grammerNames = ( ls -Path $getGrammer | ? {$_.Extension -eq ".g4"} | % {
    $_.Name.Replace("Lexer.g4",[string]::Empty).Replace("Parser.g4",[string]::Empty)
} | Sort-Object -Unique)

if($grammerNames -eq $null -or $grammerNames.Length -le 0){
    Write-Host "Post-Build Script: no grammer files located"
    break;
}

$matchName = "({0}.*?\.cs)" -f  [string]::Join(".*?\.cs)|(",$grammerNames)

Write-Host "Post-Build Script: Looking for any files whose name matches $matchName"

$srcDir = (Join-Path $workingDir "obj\Debug")

if(-not(Test-Path $srcDir)){
    Write-Host "Post-Build Script: Expected to find .cs files at $srcDir"
    break;
}

ls -Path $srcDir | ? {$_.Name -match $matchName } | % {
    $csFileFullName = $_.FullName
    Write-Host "Moving $csFileFullName"
    Move-Item  -Path $_.FullName -Destination $workingDir -Force
}