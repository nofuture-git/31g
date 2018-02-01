Param (
    [string]$GrammerFileName
)

$myScriptLocation = Split-Path $PSCommandPath -Parent

$myGrammerFileDir = Resolve-Path ( Join-Path $myScriptLocation ".\Grammers")

$antlrCsharpJar =  Resolve-Path ( Join-Path $myScriptLocation ".\antlr4-csharp-4.6.4-complete.jar")

$grammerFileFullName = $GrammerFileName

if(-not ([System.IO.Path]::IsPathRooted($GrammerFileName))){
    $grammerFileFullName = Resolve-Path ( Join-Path $myGrammerFileDir ".\$GrammerFileName")
}

$grammerFileDirName = [System.IO.Path]::GetFileNameWithoutExtension($grammerFileFullName)
$grammerFileDirName = $grammerFileDirName.Replace("Parser","").Replace("Lexer","")

$outputPath = $myScriptLocation
$outputPath = Join-Path $outputPath $grammerFileDirName

if(-not (Test-Path $outputPath)){
    $dnd = mkdir $outputPath -Force
}

$namespace = "NoFuture.Antlr.Grammers.$grammerFileDirName"

Invoke-Expression -Command "java -jar $antlrCsharpJar -listener -Dlanguage=CSharp_v4_5 -o $outputPath -package $namespace $grammerFileFullName"