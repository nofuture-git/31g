$GrammerFileName = "DotNetIlTypeName.g4"

$myScriptLocation = Split-Path $PSCommandPath -Parent

$antlrCsharpJar =  Resolve-Path (Join-Path $myScriptLocation "..\antlr4-csharp-4.6.4-complete.jar")

$namespace = "NoFuture.Antlr.DotNetIlTypeName"

$grammerFileFullName = Resolve-Path (Join-Path $myScriptLocation $GrammerFileName)

Invoke-Expression -Command "java -jar $antlrCsharpJar -listener -Dlanguage=CSharp_v4_5 -o $myScriptLocation -package $namespace $grammerFileFullName"