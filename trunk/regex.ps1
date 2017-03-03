try{
if(-not [NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Select-RecursiveRegex",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("ConvertTo-EscRegex",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Get-RegexCatalog",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Select-AllRecursiveRegex",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
<#
    .synopsis
    Find the most primitive regex match therein.
    
    .Description
    Recursively matches a pattern on a given input until no 
    match can be found returning the last one as the output.
    
    .parameter Value
    The string in which the pattern will be applied recursively.
    
    .parameter Pattern
    The regex pattern. It must have at least one back reference
    for reduction to be applicable.
    
    .example
    C:\PS> Select-RecursiveRegex -Value "1 and 3 then some 2 but even more than that 9" -Pattern '[0-9]([0-9a-z\s]+)[0-9]'
     then some 
    
    .outputs
    String
    
#>
function Select-RecursiveRegex
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [AllowEmptyString()]
        [string] $Value,
        [Parameter(Mandatory=$true,position=1)]
        [string] $Pattern
    )
    Process
    {
        #eliminate matches within matches - find most primitive match therein
        if($Value -cmatch $Pattern){
            #check for recurse blowing out the stack
            if($Value -ne $matches[1]){return Select-RecursiveRegex $matches[1] $Pattern}else{return $Value}
        }
        else{
            return $Value
        }
    }
}


<#
    .synopsis
    Allocates matches from a given string into an array
    
    .Description
    Given the string the function will find all simple matches 
    (matches which do not contain a match themself). And return 
    all simple matches in an array. 
    
    .parameter Value
    The string in which the pattern will be applied recursively.
    
    .parameter Pattern
    The regex pattern. It must have at least one back reference
    for reduction to be applicable.
    
    .parameter ReverseOrder
    Set to true to recieve the returned array ordered by last match 
    to first, leave-off or set to true to recieve first-match to last.
    
    .example
    C:\PS> Select-AllRecursiveRegex -Value "1 and 3 then some 2 but even more than that 9" -Pattern '[0-9]([0-9a-z\s]+)[0-9]'
     then some 
     but even more than that
    
    .outputs
    Array
    
#>
function Select-AllRecursiveRegex
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [AllowEmptyString()]
        [string] $Value,
        [Parameter(Mandatory=$true,position=1)]
        [string] $Pattern,
        [Parameter(Mandatory=$false,position=2)]
        [bool] $ReverseOrder
        
    )
    Process
    {
        #begin the results array
        $approtion = @()
        
        #find the match 
        $resultingMatch = (Select-RecursiveRegex -Value $Value -Pattern $Pattern)
               
        #forms the 
        if($Value -eq "" -or $resultingMatch -eq $Value)
        {
            $approtion += $resultingMatch
            return $approtion 
        }
        else
        {
            if (-not $ReverseOrder) 
            {
                $approtion += $resultingMatch
            }
            $approtion += (Select-AllRecursiveRegex -Value ($Value.SubString($Value.IndexOf($resultingMatch) + $resultingMatch.Length)) -Pattern $Pattern -ReverseOrder $ReverseOrder)
            if ($ReverseOrder)
            {
                $approtion += $resultingMatch
            }
            return $approtion
        }
        
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
        [string] $StringLiteral
    )
    Process
    {

        return [NoFuture.Util.Etc]::EscapeString($StringLiteral,[NoFuture.Shared.EscapeStringType]::REGEX)
    }
}


<#
    .SYNOPSIS
    Gets new instance of NoFuture.Shared.RegexCatalog.
    
    .DESCRIPTION
    The PsObject returned is named "RegexCatalog" and the note
    properties are regex patterns are whose name is obvious to 
    thier intent.
    
    .OUTPUTS
    NoFuture.Shared.RegexCatalog
    
#>
function Get-RegexCatalog
{
    [CmdletBinding()]
    Param()
    Process
    {
        return New-Object NoFuture.Shared.RegexCatalog
    }
}