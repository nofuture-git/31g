Param (
    [string]$GrammerFileName
)

$myScriptLocation = Split-Path $PSCommandPath -Parent

$myGrammerFileDir = Resolve-Path ( Join-Path $myScriptLocation ".\Grammers")

$antlrCsharpJar =  Resolve-Path ( Join-Path $myScriptLocation ".\antlr4-csharp-4.6.4-complete.jar")
$grammerFileFullName = Resolve-Path ( Join-Path $myGrammerFileDir ".\$GrammerFileName")

$outputPath = $myScriptLocation

$namespace = "NoFuture.Antlr.Grammers"

Invoke-Expression -Command "java -jar $antlrCsharpJar -listener -Dlanguage=CSharp_v4_5 -o $myScriptLocation -package $namespace $grammerFileFullName"