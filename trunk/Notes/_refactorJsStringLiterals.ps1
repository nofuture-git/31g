
<#
    .synopsis
    Finds and replaces any instance of a string literal with a constant.
    
    .Description
    Given a file every instance of a string literal will be replaced with 
    an ALL-CAPS constant whose name is similar to the string value itself.
    The return from the cmdlet is the declarations of the given string literals.
    
    .parameter Path
    The Path to a source-code file whose string literals are to be replaced.
    
    .example
    C:\PS>Refactor-StringLiterals -Path "C:\Projects\MySolution\MyWebsite\scripts\myJavascript.js"
         
    .outputs
    Array
    
#>
function Refactor-JsStringLiterals
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [AllowEmptyString()]
        [string] $Path
    )
    Process
    {
    #test input
    if(-not(Test-Path -Path $Path)) {break;}
    
    #returnable array
    $declarations = @()
    
    #get an array of all the string literals found in the file
    $stringLiterals = GetStringLiterals $Path
    if($stringLiterals -eq $null -or $stringLiterals.Length -eq 0){return $declarations}
    
    #modify contents of Path arg
    OverwriteStringLiterals $stringLiterals $Path
    
    #get declarations of new string contants
    $stringLiterals | ? {$_ -ne $null} | % { 
        $stringConstants = GetReplaceText $_
        $declarations += GetHostOutputString $stringConstants $_
        }
    
    #continue to recurse so long as string literals are found
    $stringLiterals = GetStringLiterals $Path
    if($stringLiterals -ne $null -and $stringLiterals.Length -ne 0)
    {
        $declarations += Refactor-JsStringLiterals $Path
    }

    #when stack finally unwinds         
    $declarations = ($declarations | Sort-Object -Unique)
    
    return $declarations
    }
}

#control over setting content of target file
function OverwriteStringLiterals($stringLiterals, $Path)
{
    #for each string literal overwrite the content of the file
    $stringLiterals | % { 
       $stringLiteral = $_
       ls -Recurse -Path $Path | ? {-not $_.PSIsContainer} | % {
            if($stringLiteral -ne $null)
            {
                $FindText = $stringLiteral
                $ReplaceWith = (GetPropertyCall $Path $stringLiteral)
        		Write-Progress -activity $("Replace : '{0}' with {1}." -f $FindText,$ReplaceWith) -status $("Location : '{0}'." -f $_.FullName)

                #again get file in memory
                (Get-Content -Path $_.FullName) | % {

                  if($_.Contains(("{0}" -f $FindText)))
                  {
                    $_.Replace(("{0}" -f $FindText),$ReplaceWith)
                  }
                  else
                  {
                    $_
                  }
                } | Set-Content $_.FullName
            }#end string literal is not null
        }#end ls Path
        
    }#end foreach string literal
}

#search an return all matching string literals in the target file
function GetStringLiterals($Path)
{
    $regexCatalog = Get-RegexCatalog

    #get file as array of lines in memory
    $fileContent = (Get-Content -Path $Path)
    
    $stringLiterals = @()
    
    #get every matching instance of a string-literal unless its enclosing markup 
    $fileContent | % { 
        if((-not($_ -match $regexCatalog.EmbeddedHtml)) -and ($_ -match $regexCatalog.StringLiteral)) 
        {
            #get the dbl-quote match if present, otherwise single-quote match
            if($matches[2] -eq $null)
            {
                $stringLiteral = $matches[4]
            }
            else
            {
                $stringLiteral = $matches[2]
            }
            
            #validate the string literal prior to adding
            if(ValidStringLiteral $stringLiteral)
            {
                $stringLiterals += $stringLiteral
            }#end valid string literal
        }#end line is match 
    }#end foreach file in Path
    
   
    $stringLiterals = ($stringLiterals | Sort-Object -Unique)
    return $stringLiterals

}

#control over transformation of string literals to constant names
function GetReplaceText($line)
{
    #make as uppercase and strip quotes off
    $line = $line.ToUpper()
    $line = $line.Substring(1,$line.Length-2)
    
    #if name begins with a number
    if($line -ne $null -and ($line.ToCharArray()[0] -match '[0-9]' ))
    {
        $line = ("NUM_{0}" -f $line) 
    }
    
    #replace any other chars as needed
    $charReplacementHash.Keys | % {
        $line = $line.Replace($_,$charReplacementHash.$_)
    }
    return $line
}

#control of the string literal declarations
function GetHostOutputString($stringConstant, $stringLiteral)
{
    $stringLiteral = StandardizeStringLiteral $stringLiteral
    return ("this.{0} = {1};" -f $stringConstant, $stringLiteral)
}

#control of the string literal invocations
function GetPropertyCall($Path, $stringLiteral)
{
    $namespace = ("_{0}" -f [System.IO.Path]::GetFileNameWithoutExtension($Path))
    return ("{0}.{1}" -f $namespace,(GetReplaceText $stringLiteral))
}

#control over the form of the string literal's declaration
function StandardizeStringLiteral($stringLiteral)
{
    #replace stringliterals in single quotes with double quotes
    if($stringLiteral.StartsWith("'") -and $stringLiteral.EndsWith("'"))
    {
        $len = $stringLiteral.Length
        $stringLiteral = ("`"{0}`"" -f $stringLiteral.Substring(1,$len-2))
    }
    return $stringLiteral

}

#last control over targeted string literal value 
function ValidStringLiteral($stringLiteral)
{
    return ($stringLiteral -ne $null)
}

$charReplacementHash = @{
"." = "_DOT_";
" " = "_SP_";
"-" = "_";
"#" = "POUND_";
"[" = "_OPEN_BRACE_";
"]" = "_CLS_BRACE_";
"?" = "_";
"\" = "_";
"/" = "_";
"," = "_";
"=" = "_EQ_";
"'" = "_SINGLE_QUOTE_";
"`"" = "_DBL_QUOTE_";
":" = "_COLON_"}
