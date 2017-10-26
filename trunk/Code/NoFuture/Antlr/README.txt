You can re-generate any of the .cs files by invoking the included 
GenAntlrV4CSharpCodeFile from within Powershell and pass in just the 
name of one of the .g4 files included in the .\Grammers folder of this
project.

EXAMPLE single .g4 file
.\GenAntlrV4CSharpCodeFile.ps1 -GrammerFileName DotNetIlTypeName.g4

EXAMPLE lexer, parser split
.\GenAntlrV4CSharpCodeFile.ps1 -GrammerFileName HTMLLexer.g4
.\GenAntlrV4CSharpCodeFile.ps1 -GrammerFileName HTMLParser.g4
