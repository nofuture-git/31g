try{
if(-not [NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-T4TextTemplate",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("tt",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Set-JsonFormated",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
Set-Alias tt Get-ImmediateT4Text

<#
    .SYNOPSIS
    Formats a JSON file.
    
    .DESCRIPTION
    Reads then writes a JSON file right back down upon itself making only 
    a change to the format (i.e. pretty-printed).
    
    .PARAMETER FullName
    A full path to a JSON file
    
    .PARAMETER SetDblToSingleQuote
    Optional, will cause all single quotes to be escaped and then have all
    double quotes replaced with single quotes.

    .OUTPUTS
    FullName
#>
function Set-JsonFormated
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0,HelpMessage="Path to a JSON file")]
        [string] $FullName,
        [Parameter(Mandatory=$true,position=0,HelpMessage="Optional switch to have double-quotes converted to single quotes")]
        [switch] $SetDblToSingleQuote
    )
    Process
    {
        if(-not (Test-Path $FullName)){
            return $FullName
        }

        $boolSwitch = $false
        if($SetDblToSingleQuote){
            $boolSwitch = $true
        }

        $readText = [System.IO.File]::ReadAllText($FullName)
        $writeText = [NoFuture.Util.Core.Etc]::FormatJson($readText,$boolSwitch)
        [System.IO.File]::WriteAllText($FullName, $writeText)

        return $FullName
    }
    
}

<#
    .synopsis
    Performs an immediate transformation of a T4 Text string-literal
    
    .Description
    Passes the Input directly into the TextTransform.exe, gets the output
    and returns it.  If no output is created the cmdlet will 
    return 'no results'.
    
    .parameter Input
    A valid T4 string-literal.
    
    .outputs
    string
#>
function Get-ImmediateT4Text
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0,HelpMessage="A string to transform")]
        [string] $InputText
    )
    Process
    {
        if(-not (Test-Path ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Text))){mkdir ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Text)}
        
        $t4Temp = ([NoFuture.Shared.Cfg.NfConfig+TempFiles]::T4Template)
        $t4Out = (Join-Path ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Text) "t4Temp.txt")
        $InputText > $t4Temp
        Write-Host $t4Temp 
        
        Get-T4TextTemplate -InputFile $t4Temp -OutputFile $t4Out
        
        if(Test-Path $t4Out){return (Get-Content -Path $t4Out)}
        return "no results"
    }
    
}

<#
    .SYNOPSIS
    Encapsulates the TextTransform.exe
    
    .DESCRIPTION
    Directly calls the TextTransform.exe matching the cmdlet's 
    args to the exe's args
    
    .PARAMETER InputFile
    This command runs a transform using this text template.
    
    .PARAMETER OutputFile
    The file to write the output of the transform to.

    .PARAMETER $ParamNameValues
    An hashtable of parameter names to values.

    .LINK
    https://msdn.microsoft.com/en-us/library/bb126245.aspx

    .OUTPUTS
    null
#>
function Get-T4TextTemplate
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $InputFile,
        [Parameter(Mandatory=$true,position=1)]
        [string] $OutputFile,
        [Parameter(Mandatory=$false,position=2)]
        [Hashtable] $ParamNameValues
    )
    Process
    {
        #test TestTransform.exe is installed
        if(-not(Test-Path ([NoFuture.Shared.Cfg.NfConfig+X86]::TextTransform)))
        {
            throw ("The Microsoft tool named 'TextTransform.exe' is required and was expected at '{0}'" -f ([NoFuture.Shared.Cfg.NfConfig+X86]::TextTransform))
        }
    
        #set a flag when parameter directive are being used
        $parametersWereSet = $false
        
        $paramSwitch = New-Object System.Text.StringBuilder
        if($ParamNameValues -ne $null){
            
            $ParamNameValues.Keys | % {
                $param = $_
                $value = $ParamNameValues[$param]
                $dnd = $paramSwitch.Append("-a !!")
                $dnd = $paramSwitch.Append($param)
                $dnd = $paramSwitch.Append("!")
                $dnd = $paramSwitch.Append($value)
                $dnd = $paramSwitch.Append(" ")
            }
            
            $parametersWereSet = $true
        }
        
        
        #draft the command string
        if($parametersWereSet)
        {
            $cmd = ("& {0} -out `"{1}`" {2}`"{3}`"" -f ([NoFuture.Shared.Cfg.NfConfig+X86]::TextTransform),$OutputFile,$paramSwitch,$InputFile)
        }
        else
        {
            $cmd = ("& {0} -out `"{1}`" `"{2}`"" -f ([NoFuture.Shared.Cfg.NfConfig+X86]::TextTransform),,$OutputFile,$InputFile)        
        }
        
        #send a copy of the command to the host
        Write-Host $cmd
        
        #invoke TextTransform.exe
        $donotdisplay = (Invoke-Expression -Command $cmd)
    }
}

