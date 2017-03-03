try{
if(-not [NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Get-XDocFrame",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Merge-XDocFrame",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}

<#
    .SYNOPSIS
    Creates and XDocument in which child elements map to token-pair matches.
    
    .DESCRIPTION
    Given a string arguement (or a path to a text file) the function will 
    produce an XDocument where the nesting of child nodes represents 
    the internal context of a matching pair of tokens.  Therefore the output
    will contian nodes and those nodes may contain furtherer child nodes and
    so on down to an internal size that is at least the defined minimum token
    span.
    
    The string input will have all its concurrent occurances of spaces and tabs 
    reduced to a single space or tab.
    
    The result is simply a frame and does not contain any of the original string.
    
    .PARAMETER TokenPairs
    A single token pair is an array with two elements.

    .PARAMETER MinimumStringLength
    Optional, token pairs are only applied to the XDocument frame when the length of the string between
    them is this size or greater.  Defaults to '1'.
    
    .PARAMETER InputString
    Optional, the string upon which the XDocument frame will be derived. If this is 
    omitted then the Path parameter must be assigned,

    .PARAMETER Path
    Optional, full name to a file upon which the XDocument frame will be derieved. 
    The file will be transformed to a single string and that string will be assigned
    to the InputString parameter.  If both an InputString and a Path are given then 
    the InputString will be operated on and the Path will be disgarded.
    
    .EXAMPLE
    C:\PS>$mySqlFileFrame = Get-XDocFrame -TokenPairs @("begin","end") -Path "C:\my sql files\mySqlFile.sql" -MinimumStringLength 5
    
    .EXAMPLE
    C:\PS>myCsFileFrame = Get-XDocFrame -TokenPairs @("{","}") -InputString $myCsCode -MinimumStringLength 9
    
    .OUTPUTS
    System.Xml.Linq.XDocument
    
#>
function Get-XDocFrame
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [System.Array] $TokenPairs,
        [Parameter(Mandatory=$false,position=1)]
        [int]$MinimumStringLength,
        [Parameter(Mandatory=$false,position=2)]
        [string] $InputString,
        [Parameter(Mandatory=$false,position=3)]
        [string] $Path
    )
    Process
    {
        $tokenDefinition = $TokenPairs
        if($MinimumStringLength -eq 0){$MinimumStringLength = 1}
        
        if($Path -ne "" -and $InputString -eq ""){
            if(Test-Path $Path){
                $stringArg = [System.IO.File]::ReadAllText($Path)
            }
            else{
                throw ("'{0}' path not found." -f $Path)
            }
        }
        if($InputString -ne ""){
            $stringArg = $InputString
        }
        if($stringArg -eq $null -or $stringArg.Trim() -eq "")
        {
            throw "The input string arg is empty."
        }
        $sToken = $TokenPairs[0]
        $eToken = $TokenPairs[1]
            
        if(-not($TokenPairs[0] -is [System.String]) -or -not($TokenPairs[1] -is [System.String])){
            throw "Tokens must be in the form of a string."
        }

        if([string]::IsNullOrWhiteSpace($sToken) -or [string]::IsNullOrWhiteSpace($eToken)){
            throw "Tokens cannot be empty nor whitespace."
        }

        if($sToken.Length -gt 1){
            $nfXDoc = New-Object NoFuture.Tokens.XDocFrame($sToken,$eToken) -Property @{MinTokenSpan = $MinimumStringLength}
            
        }
        else{
            $nfXDoc = New-Object NoFuture.Tokens.XDocFrame([char]($sToken[0]),[char]($eToken[0])) -Property @{MinTokenSpan = $MinimumStringLength}
        }
        return $nfXDoc.GetXDocFrame($stringArg);

    }
    
}


<#
    .SYNOPSIS
    Applies the results of Get-XDocFrame to the original content.
    
    .DESCRIPTION
    Returns the XDocument with the inter-string spans applied as text data 
    of each of the XDocument frame nodes.
    
    The input is again distilled of all concurrent occurances of tab and space
    characters.
    
    .PARAMETER XDocFrame
    The output XDocument of Get-XDocFrame.  The functions are tightly coupled.
    
    .PARAMETER InputString
    Optional. the same string from which the XDocFrame was derived. If this is 
    omitted then the Path parameter must be assigned.
    
    .PARAMETER Path
    Optional, full name to a file from which the XDocFram was derieved. 
    The file will be transformed to a single string and that string will be assigned
    to the InputString parameter.  If both an InputString and a Path are given then 
    the InputString will be operated on and the Path will be disgarded.
    
    .EXAMPLE
    C:\PS> Merge-XDocFrame -XDocFrame $(Get-XDocFrame -Path "myCode.cs" -Tokens @("{","}") -MinimumStringLength 9) -Path "myCode.cs"
    
    .EXAMPLE
    C:\PS> Merge-XDocFrame $myXDocRef $myOriginalString
    
    .OUTPUTS
    System.Xml.Linq.XDocument
    
#>
function Merge-XDocFrame
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [System.Xml.Linq.XElement] $XDocFrame,
        [Parameter(Mandatory=$false,position=1)]
        [string] $InputString,
        [Parameter(Mandatory=$false,position=2)]
        [string] $Path

    )
    Process
    {

        if($Path -ne "" -and $InputString -eq ""){
            if(Test-Path $Path){
                $stringArg = [System.IO.File]::ReadAllText($Path)
            }
            else{
                throw ("'{0}' path not found." -f $Path)
            }
        }
        if($InputString -ne ""){
            $stringArg = $InputString
        }
        
        if(($stringArg -eq $null) -or ($stringArg -eq "") -or ($XDocFrame -eq $null))
        {
            throw "The input args are invalid - check the values and try again."
        }

        $nfXDoc = New-Object NoFuture.Tokens.XDocFrame
        
        $nfXDoc.ApplyXDocFrame($XDocFrame, $stringArg)
    }
}
