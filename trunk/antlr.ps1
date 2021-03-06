$myScriptLocation = Split-Path $PSCommandPath -Parent
$dependencies = @{
    "NoFuture.Shared.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Shared.Core.dll");
    "NoFuture.Shared.Cfg, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Shared.Cfg.dll");
}

$loadedAsms = ([appdomain]::CurrentDomain.GetAssemblies() | % {$_.FullName}  | Sort-Object -Unique)
$dependencies.Keys | % {
    if($loadedAsms -notcontains $_)
    {
        $binDll = $dependencies.$_
        if(Test-Path $binDll)
        {
            $stdout = [System.Reflection.Assembly]::LoadFile($binDll)
        }
        else
        {
            Write-Host ("'{0}' was not found at '{1}'; some functions will not be available." -f $_, $binDll) -ForegroundColor Yellow
        }
    }
}


#=====================================
#intialize paths from nfConfig.cfg.xml
$dnd = [NoFuture.Shared.Cfg.NfConfig]::Init(([NoFuture.Shared.Cfg.NfConfig]::FindNfConfigFile($noFutureBin)))
#=====================================

<#
    .SYNOPSIS
    Call java on the antlr-4.7.1-complete.jar with the given arg.
    
    .DESCRIPTION
    Concise manner to have the ANTLR files generated given the 
    arg.

    All .g4 files must start with a header, like:

    grammer MyGrammer

    where 'MyGrammer' is a place holder.  Furthermore, the file
    must then be named as .\MyGrammer.g4

    Concerning generated files:

    MyGrammerParser.java, extends the org.antlr.runtime.Parser class
     and contains a method for each rule from the .g4 file.  Within the 
     .g4 file, Parser rules start with lowercase letters

    MyGrammerLexer.java, extends the org.antlr.runtime.Lexer class
     and contains lexical rules for the values from the .g4 file  Within
     the .g4 file, Lexer rules start with uppercase letters and are typically
     all upper case.

    MyGrammer.tokens, each token defined in the .g4 file receives a token-type-number
     which is used as grammers become compounded to delimit one token from another.
    
    MyGrammerListener.java, extends the org.antlr.v4.runtime.tree.ParseTreeListener 
     futhermore, a base implementation is generated with like name.  These are for 
     pluging into events fired by the Parser.
    
    .PARAMETER Path
    A Path to an ANTLR .g4 file.

    .PARAMETER Visitor
    Switch passed to ANTLR to generate additional java files:

    MyGrammerVisitor.java, extends org.antlr.v4.runtime.tree.ParseTreeVisitor
    
    MyGrammerBaseVisitor.java, abstract implementation of 
      MyGrammerVisitor interface 
    
    .EXAMPLE
    C:\PS>
#>
function Invoke-Antlr
{
	[CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [string] $Path,

        [Parameter(Mandatory=$false,position=1)]
        [switch] $Visitor
    )
    Process
    {
        $binCopy = [NoFuture.Shared.Cfg.NfConfig+JavaTools]::Antlr
        $antlr = Join-Path $env:CLASSPATH.Replace("*","") ([System.IO.Path]::GetFileName($binCopy))

        #get a copy prior to any modifications
        $originalPath = $Path

        #test for antlr jar file at CLASSPATH
        if(-not (Test-Path $antlr)){
            
            if(-not (Test-Path $binCopy)){
                Write-Host "antlr not found at CLASSPATH nor at $binCopy"
                break;
            }
            Copy-Item -Path $binCopy -Destination $env:CLASSPATH.Replace("*","") -Force
        }

        $expr = "java -jar {0}" -f $antlr

        if(Test-Path $Path){
            $argPath = (Resolve-Path $Path)
            $Path = $argPath.Path
        }

        #need to call antlr first on the lexer .g4 file
        $Path2 = Test-GrammerFileRequiresTokensFile $Path
        if($Path2 -ne $null){
            $expr2 = "{0} {1}" -f $expr, $Path2
        
            ("[{0:yyyyMMdd-HHmmss}]{1}" -f $(Get-Date),$expr2) >> (Join-Path (Split-Path $originalPath -Parent) "cmd.log")

            Invoke-Expression -Command $expr2
        }

        if($Visitor){
            $Path = ("-visitor {0}" -f $Path)
        }
        
        $expr = "{0} {1}" -f $expr, $Path
        
        ("[{0:yyyyMMdd-HHmmss}]{1}" -f $(Get-Date),$expr) >> (Join-Path (Split-Path $originalPath -Parent) "cmd.log")

        Invoke-Expression -Command $expr
    }
}#end Invoke-Antlr


<#
    .SYNOPSIS
    Calls Invoke-Antlr having copied the Path to JavaSrc
    
    .DESCRIPTION
    Intended to be used in-scope of the whole NoFuture namespace
    functionality.

#>
function Invoke-AntlrToJavaSrc
{
	[CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Path,

        [Parameter(Mandatory=$false,position=1)]
        [switch] $Visitor
    )
    Process
    {
        $ms = [NoFuture.Shared.Cfg.NfConfig]::ThreadSleepTime
        $pushFlag = $false
        $javaSrc = ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::JavaSrc)
        $antlrFilePath = ([NoFuture.Shared.Cfg.NfConfig+JavaTools]::Antlr)

        $antlrFile = [System.IO.Path]::GetFileName($antlrFilePath)
        $antlr = Join-Path $env:CLASSPATH.Replace("*","") $antlrFile

        #resolve the Path arg to fully qualified path
        if(Test-Path $Path){
            $argPath = (Resolve-Path $Path)
            $Path = $argPath.Path
        }

        #validate the file has expected extension
        if([System.IO.Path]::GetExtension($Path) -ne ".g4"){
            Write-Host "'$antlrFile' requires .g4 extension"
            break;
        }

        #check if the grammer file splits tokens from parser rules
        $Path2 = Test-GrammerFileRequiresTokensFile $Path

        #copy .g4 file to JavaSrc and push location to it
        cp -Path $Path -Destination $javaSrc -Force
        if($Path2 -ne $null){
            cp -Path $Path2 -Destination $javaSrc -Force
        }
        Push-Location $javaSrc
        $pushFlag = $true

        #test everything is where is supposed to be
        $tempPath = (Join-Path $javaSrc ([System.IO.Path]::GetFileName($Path)))
        [System.Threading.Thread]::Sleep($ms)

        #if the copy to JavaSrc didn't work for some reason, pop location and carry on
        if(-not (Test-Path $tempPath)){
            Pop-Location
            $pushFlag = $false
        }
        else{
            $Path = $tempPath
        }

        if($Visitor){
            Invoke-Antlr -Path $Path -Visitor
        }
        else{
            Invoke-Antlr -Path $Path
        }
        
        if($pushFlag){
            Pop-Location
        }

    }
}#end Invoke-AntlrToJavaSrc

<#
    .SYNOPSIS
    Drafts a command string to be run from powershell.exe (does not work in the ISE).
    
    .DESCRIPTION
    Generates a full command to be run in powershell.exe to invoke the 
    org.antlr.v4.Tool object.

    To use call it with whatever command is to be passed to the Tool, 
    Ctrl+Shift+P to open a new powershell console, change to the 
    NoFuture.Shared.Core.NfConfig.TempDirectories.JavaDist directory, copy/paste 
	the command and press Enter.
    
    .PARAMETER Arg
    The command to be passed to the org.antlr.v4.Tool object itself
    
    .EXAMPLE
    PS C:\Projects\31g\trunk> cd .\temp\code\java\dist
    PS C:\Projects\31g\trunk\temp\code\java\dist>Invoke-Grun -Arg "ArrayInit init -tokens"
    {99, 3, 451}
    ^Z
    [@0,0:0='{',<1>,1:0]
    [@1,1:2='99',<4>,1:1]
    [@2,3:3=',',<2>,1:3]
    [@3,5:5='3',<4>,1:5]
    [@4,6:6=',',<2>,1:6]
    [@5,8:10='451',<4>,1:8]
    [@6,11:11='}',<3>,1:11]
    [@7,14:13='<EOF>',<-1>,2:0]
    PS C:\Projects\31g\trunk\temp\code\java\dist>

    .EXAMPLE
    PS C:\> Invoke-AntlrToJavaSrc -Path ".\temp\code\java\myAntlr\VbScript.g4"
    PS C:\> Invoke-JavaCompiler -TypeName "VbScript*"
    PS C:\> Invoke-Grun -Arg "VbScript vbBlock -tokens"
    <% dim myVar %>
    ^Z
    [@0,0:1='<%',<1>,1:0]
    [@1,3:6='dim ',<5>,1:3]
    [@2,7:11='myVar',<4>,1:7]
    [@3,13:14='%>',<2>,1:13]
    [@4,15:16='\r\n',<6>,1:15]
    [@5,17:16='<EOF>',<-1>,2:17]
    PS C:\> 

    .EXAMPLE
    PS C:\> Invoke-AntlrToJavaSrc -Path "C:\Projects\31g\trunk\Code\ANTLRGrammer\grammars-v4-master\html\HTMLParser.g4" #handle compilation of HTMLLexer.g4 internally
    PS C:\> Invoke-JavaCompiler -TypeName "HTML*" #note: wildcard catches both HTMLParser* and HTMLLexer*
    PS C:\> Invoke-Grun -Arg "HTML htmlDocument -gui C:\Temp\MyHtml.html"


#>
function Invoke-Grun
{
	[CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [string] $Arg
    )
    Process
    {
        $javaSrc = [NoFuture.Shared.Cfg.NfConfig+TempDirectories]::JavaSrc 
        $javaRoot = (Split-Path $javaSrc -Parent)
        $grunCmd = ('CMD.EXE /C java -cp .`;{0} org.antlr.v4.gui.TestRig {1}' -f $env:CLASSPATH, $Arg)
        $grunCmd >> (Join-Path $javaRoot "cmd.log")
        Invoke-Expression -Command $grunCmd
    }
}#end Invoke-Grun


function Test-GrammerFileRequiresTokensFile($Path){
#check if the grammer file splits tokens from parser rules
$tokenDependency =  Select-String -Pattern "options.*?tokenVocab\=(.*?)\}" -Path $Path
if ($tokenDependency -ne $null){
    $tokenMatches =  $tokenDependency.Matches[0]
    if($tokenMatches -ne $null){
        $tokenMatchGroups =  $tokenMatches.Groups
        if($tokenMatchGroups -ne $null -and $tokenMatchGroups.Count -gt 0){
            $tokenFile = $tokenMatchGroups[1].Value
            $tokenFile = (Join-Path (Split-Path $Path -Parent) ($tokenFile.Replace(";","").Trim() + ".g4"))
            if(Test-Path $tokenFile){
                return $tokenFile
            }
        }
    }
}
}