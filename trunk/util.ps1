try{
if(-not [NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("npp",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("ssr",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("c2cb",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("cfcb",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Replace-Content",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Invoke-CscExeSingleFile",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("ConvertTo-EscRegex",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Format-Json",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Format-Xml",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
#=================================================
set-alias ss    select-string
set-alias ssr   Select-StringRecurse
#=================================================

function Set-ClipBoardContent
{
    Param
    (
        [Parameter(Mandatory=$true,ValueFromPipeline=$true)]
        [string] $val
    )
    Process{
        [System.Windows.Forms.Clipboard]::SetText($val)
    }
}
function Get-ClipBoardContent
{
    Param
    (
    )
    Process{
        return [System.Windows.Forms.Clipboard]::GetText()
    }
}

Set-Alias c2cb Set-ClipBoardContent
Set-Alias cfcb Get-ClipBoardContent


<#
    .SYNOPSIS
    Invokes Select-String cmdlet on every file in the directory and child directories thereof.
    
    .DESCRIPTION
    Performs a Select-String cmdlet on each file recursively and will filter to only 
    those files which are considered code (see NoFuture.Shared.Core.NfConfig.CodeFileExtensions)
    
    .PARAMETER Pattern
    The regex pattern that is used for the Select-String cmdlet.
    
    .PARAMETER Path
    Optional directory at which to begin the recursive Select-String, default to
    the working directory

    .PARAMETER CurrentDirOnly
    Optional to limit the Select-String to only text files in this directory

    .PARAMETER IncludeConfig
    Optional to include configuration file types (see NoFuture.Shated.NfConfig.ConfigFileExtensions)
    
    .EXAMPLE
    C:\PS>Select-StringRecurse -Pattern "^an important message.*$" -Path C:\AllTheMessages
    
    .EXAMPLE
    C:\PS>Select-StringRecurse "^.*put this somewhere.*$"  #will search C:\ since that the current dir
    
    .OUTPUTS
    System.Array
    
#>
function Select-StringRecurse
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Pattern,
        [Parameter(Mandatory=$false,position=1)]
        [string] $Path,
        [Parameter(Mandatory=$false,position=2)]
        [switch] $CurrentDirOnly,
        [Parameter(Mandatory=$false,position=3)]
        [switch] $IncludeConfig
    )
    Process
    {
      
      $ssrExt = @()
      $ssrExt += [NoFuture.Shared.Cfg.NfConfig]::CodeFileExtensions | % { "*.{0}" -f $_}
      if ($IncludeConfig)
      {
        $ssrExt += [NoFuture.Shared.Cfg.NfConfig]::ConfigFileExtensions | % { "*.{0}" -f $_}
      }
      
      if($Path -eq $null){$Path = ".\"}  

      Write-Progress -activity $("Regex : `'" + $Pattern + "`'.") -status $("Filter : `'" + $_ + "`'");

      if($CurrentDirOnly){
          ls -Path $Path -File | ? { 
            ($ssrExt -contains ("*{0}" -f $_.Extension))  } | % {
              Write-Progress -activity $("Searching `'" + $_.FullName + "`'.") -status $("Regex : `'" + $Pattern + "`'"); 
              ss -Pattern $Pattern -Path $_.FullName
           }
      }
      else{
          ls -r -Path $Path -File | ? { 
            ($ssrExt -contains ("*{0}" -f $_.Extension)) -and -not ([NoFuture.Util.Core.NfPath]::ContainsExcludeCodeDirectory($_.FullName))  } | % {
              Write-Progress -activity $("Searching `'" + $_.FullName + "`'.") -status $("Regex : `'" + $Pattern + "`'"); 
              ss -Pattern $Pattern -Path $_.FullName
           }
      }
    }
}


<#
    .SYNOPSIS
    Static swap of one string for another in all files in the specified directory and child directories thereof.
    
    .DESCRIPTION
    Performs a simple switch of one string value for another.  The input string is not
    a regex pattern.  The function simply used the powershell -replace operator for every
    line of the given file content.
    
    Regardless if a match is found within a given file the file in-scope with have the same
    content but its last-write time will be changed to now.
    
    .PARAMETER FindText
    The text that is being targeted for replacement.
    
    .PARAMETER ReplaceWith
    The text that will replace the target in the given file.

    .PARAMETER RegexMatchPattern
    Optional regex pattern to apply to reach file name where only
    those which match will be operated upon.
    
    .PARAMETER Path
    The startin directory - every child directory will be affected the same as this one.

    .EXAMPLE
    C:\PS>Replace-Content -FindText "Stand" -ReplaceWith "Sit" -Path C:\AllMyFiles
    
    .EXAMPLE
    C:\PS>Replace-Content "you'll" "all amoung you"  #every file on the C: drive is about to be written
    
    .OUTPUTS
    null
    
#>
function Replace-Content
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $FindText,
        [Parameter(Mandatory=$true,position=1)]
        [string] $ReplaceWith,
        [Parameter(Mandatory=$false,position=2)]
        [string] $Path,
        [Parameter(Mandatory=$false,position=3)]
        [string] $RegexMatchPattern
    )
    Process
    {
        Write-Progress -activity "Finding all target files" -status "Searching" -PercentComplete 1
        $excludeExtensions = [NoFuture.Shared.Cfg.NfConfig]::BinaryFileExtensions | % { "*.{0}" -f $_}

        $targets = (ls -Recurse -Path $Path -Exclude $excludeExtensions -File | % {$_.FullName})

        if(-not([string]::IsNullOrWhiteSpace($RegexMatchPattern))){
            $targets = ($targets | ? {$_ -match $RegexMatchPattern})
        }
        $total = $targets.Length
        $counter = 0;
        :nextTarget foreach($filePath in $targets){
            $counter += 1
            if([NoFuture.Util.Core.NfPath]::ContainsExcludeCodeDirectory($filePath)){continue nextTarget;}

            Write-Progress -activity $("Replace : '{0}' with {1}." -f $FindText,$ReplaceWith) -status $("Location : '{0}'." -f $filePath) `
                           -PercentComplete ([NoFuture.Util.Core.Etc]::CalcProgressCounter($counter, $total))
            $content = [System.IO.File]::ReadAllText($filePath)
            $content = $content.Replace($FindText, $ReplaceWith)
            [System.IO.File]::WriteAllText($filePath, $content, [System.Text.Encoding]::UTF8)
        }
    }
}

<#
    .SYSNOPSIS
    Invokes csc.exe (ver. 4.0) for a single file compiling into a .dll

    .DESCRIPTION
    Utility method to compile a single .cs file into a binary dll
    
    .PARAMETER CodeFile
    Path to a .cs code file.
    
    .PARAMETER References
    An array of paths to referenced binaries.
#>
function Invoke-CscExeSingleFile
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $CodeFile,
        [Parameter(Mandatory=$false,position=1)]
        [array] $ReferencePaths
    )
    Process
    {
        if(-not (Test-Path $CodeFile)){
            Write-Host "No such File at '$CodeFile'" -ForegroundColor Yellow
            break;
        }

        if(-not (Get-Item $CodeFile).Extension -eq ".cs"){
            Write-Host "The '$CodeFile' does not have a .cs extension" -ForegroundColor Yellow
            break;
        }

        $refCmd = ""
        if($ReferencePaths -ne $null -and $ReferencePaths.Length -gt 0){
            $ReferencePaths | ? {Test-Path $_} | % {
                $fp = Resolve-Path $_
                $refCmd += ("/reference:`"{0}`" " -f $fp)
            }
        }

        $CodeFile = (Resolve-Path $CodeFile).Path
        $dir = Split-Path $CodeFile -Parent
        $csFile = Split-Path $CodeFile -Leaf
        $name = [System.IO.Path]::GetFileNameWithoutExtension($CodeFile)
        $cscCompiler = "C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\csc.exe"
        $svcUtilCscCmd = ("& {0} /t:library /debug /nologo /out:'{1}\{2}.dll' {3} '{1}\{2}.cs'" -f $cscCompiler, $dir,$name,$refCmd)
        Write-Host $svcUtilCscCmd -ForegroundColor Yellow
        Invoke-Expression -Command $svcUtilCscCmd
    }
}

<#
    .synopsis
    Returns the input string as string-series of hexidecimal regex escape characters.
    
    .Description
    Escapes every character in the input string to its hexidecimal value in the form 
    of (\x00).  All characters are escaped even if thier literal value is not a 
    reserved regex character.
    
    .parameter StringLiteral
    The string candidate whose value will be transposed to hexidecimal escape sequence.
    
    .example
    C:\PS> ConvertTo-EscRegex '$#%^@#$'
    \x24\x23\x25\x5e\x40\x23\x24
    
    .outputs
    String
    
#>
function ConvertTo-EscRegex
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $StringLiteral,
        [Parameter(Mandatory=$false,position=1)]
        [NoFuture.Shared.Core.EscapeStringType] $EscAs

    )
    Process
    {
        if($EscAs -eq $null){
            $EscAs = [NoFuture.Shared.Core.EscapeStringType]::REGEX
        }

        return [NoFuture.Util.Core.NfString]::EscapeString($StringLiteral,$EscAs)
    }
}


Set-Alias npp Get-NotepadPlusPlus


<#
    .synopsis
    Opens Notepad++
    
    .Description
    Opens Notepad++ for a given file or for an entire
    directory in which the file's extensions therein are
    found in the global code extension list.  A file's 
    syntax will also be resolved to have Notepad++ pretty
    print it with syntax highlighting.
    
    .parameter Path
    A file or directory.
    
    .parameter Line
    The line number at which Notepad++ should open to.
    
    .example
    C:\PS>Get-NotepadPlusPlus -Path "C:\MyCodeDirectory"
    
    .example
    C:\PS>Get-NotepadPlusPlus -Path "C:\MyCodeDirectory\scripts\myJavascript.js" -Line 243
    
    .example
    C:\PS>Get-NotepadPlusPlus C:\MyCodeDirectory\scripts\myJavascript.js 243
    
    .outputs
    null
    
#>
function Get-NotepadPlusPlus
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [AllowEmptyString()]
        [string] $Path,
        [Parameter(Mandatory=$false,position=1)]
        [int] $Line
    )
    Process
    {
        $nppFullPath = "C:\Program Files (x86)\Notepad++\notepad++.exe"
        $codeExtensions = [NoFuture.Shared.Cfg.NfConfig]::CodeFileExtensions | % { "*.{0}" -f $_}

        if($Path -eq $null -or $Path -eq ""){Invoke-Expression -Command "& '$nppFullPath'"; break;}

        if((Get-Item $Path).PSIsContainer){
            Get-ChildItem -Path $Path | ? {(-not $_.PSIsContainer) -and ($codeExtensions -contains $_.Extension) } | % {
                $ext = Get-NppSyntax $_.FullName
	            Invoke-Expression -Command ("& '$nppFullPath' -n{0} -l{1} '{2}'" -f 0,$ext,$_.FullName)
            }
        }
        else{
            $ext = Get-NppSyntax $Path
	        Invoke-Expression -Command ("& '$nppFullPath' -n{0} -l{1} '{2}'" -f $Line,$ext,$Path)
        }
    }
}


function Get-NppSyntax($pathFile)
{
    $xmlExtentsion = @(
        "aspx","asax","ascx","asmx","config","csproj",
        "vbproj","sln","rpx","xslt","xsd","webinfo",
        "wsdl","sitemap","dtsx","database","user",
        "edmx","fcc","pdm","pp","svc","testsettings",
        "ps1xml","tt")

    $ext = [System.IO.Path]::GetExtension($pathFile)
    if($ext -eq $null){
        return "txt"
    }
    $ext = $ext.Replace(".","")
    if($ext -eq "ps1"){
        $ext = "powershell"
    }
    elseif($ext -eq "js"){
        $ext = "javascript"
    }
    elseif(@("bas","cls","frm") -contains $ext){
        $ext = "vb"
    }
    elseif($ext -eq "h"){
        $ext = "cpp"
    }
    elseif(@($xmlExtentsion) -contains $ext){
        $ext = "xml"
    }
    elseif($ext -eq "vbp"){
        $ext = "ini"
    }
	elseif($ext -eq "cs"){
		#npp know the cs as c-sharp so no translation is needed but the 
		#appropriate regex still needs to be recorded
	}
	
    return $ext
    
}

<#
    .SYNOPSIS
    Formats a JSON string.
    
    .DESCRIPTION
    Formats the JSON string input
    
    .PARAMETER 
    The JSON String literal
    
    .PARAMETER SetDblToSingleQuote
    Optional, will cause all single quotes to be escaped and then have all
    double quotes replaced with single quotes.

    .OUTPUTS
    string
#>
function Format-Json
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0,HelpMessage="JSON string literal", ValueFromPipeline = $true)]
        [string] $InputJson,
        [Parameter(Mandatory=$false,position=0,HelpMessage="Optional switch to have double-quotes converted to single quotes")]
        [switch] $SetDblToSingleQuote
    )
    Process
    {
        if([string]::IsNullOrEmpty($InputJson)){
            return;
        }

        $boolSwitch = $false
        if($SetDblToSingleQuote){
            $boolSwitch = $true
        }
        
        return [NoFuture.Util.Core.Etc]::FormatJson($InputJson,$boolSwitch)
    }
}

<#
    .SYNOPSIS
    Formats an XML string.
    
    .DESCRIPTION
    Formats the XML string input
    
    .PARAMETER 
    The XML string literal

    .OUTPUTS
    string
#>
function Format-Xml
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0,HelpMessage="XML string literal", ValueFromPipeline = $true)]
        [string] $InputXml
    )
    Process
    {
        return [NoFuture.Util.Core.Etc]::FormatXml($readText)
    }
}