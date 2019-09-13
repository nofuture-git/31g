$myScriptLocation = Split-Path $PSCommandPath -Parent

$antlrCsharpJar =  Resolve-Path (Join-Path $myScriptLocation "..\antlr4-csharp-4.6.6-complete.jar")

$namespace = "NoFuture.Antlr.HTML"

@("HTMLLexer.g4", "HTMLParser.g4") | % {
    $grammerFileFullName = Resolve-Path (Join-Path $myScriptLocation $_)

    Invoke-Expression -Command "java -jar $antlrCsharpJar -listener -Dlanguage=CSharp_v4_5 -o $myScriptLocation -package $namespace $grammerFileFullName"
}

