$currentDirectory = Split-Path $PSCommandPath -Parent
$dependencies = @(
 "NoFuture.Rand.Law.dll", 
 "NoFuture.Rand.Law.Property.dll")

$dependencies | ? {Test-Path (Join-Path $currentDirectory $_) } | % {
    [System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes((Join-Path $currentDirectory $_)))
}

$truePredicate = [NoFuture.Rand.Law.US.ExtensionMethods]::TruePredicateFx
$falsePredicate = [NoFuture.Rand.Law.US.ExtensionMethods]::FalsePredicateFx

<#
    .SYNOPSIS
    Gets a new instance of PropertyInterestFactory
    
    .DESCRIPTION
    Gets a new instance of PropertyInterestFactory for 
    some RealProperty named PropertyName
    
    .PARAMETER PropertyName
    The name of the RealProperty

    .EXAMPLE
    PS C:\> $person = New-Object NoFuture.Rand.Law.LegalPerson("some person")
    PS C:\> $factory = Get-PropertyInterestFactory -PropertyName "some land"
    PS C:\> $factory = $factory.IsPresentInterestPossibleInfinite($truePredicate, $person)
    PS C:\> $factory = $factory.IsPresentInterestDefinitelyInfinite($falsePredicate, $person)
    PS C:\> $factory = $factory.IsFutureInterestInGrantor($falsePredicate, $person)
    PS C:\> $factory = $factory.IsVestOwnershipAutomatic($truePredicate, $person)
    PS C:\> $interest = $factory.GetValue()

    .OUTPUTS
    NoFuture.Rand.Law.Property.US.FormsOf.InTerra.PropertyInterestFactory
    
#>
function Get-PropertyInterestFactory
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $PropertyName
    )
    Process
    {
        $property = New-Object NoFuture.Rand.Law.Property.US.FormsOf.InTerra.RealProperty($PropertyName)
        return New-Object NoFuture.Rand.Law.Property.US.FormsOf.InTerra.PropertyInterestFactory($property, [NoFuture.Rand.Law.US.ExtensionMethods]::FirstOne)
    }
}

<#
    .SYNOPSIS
    Prompts user with questions to determine the property interest
    
    .DESCRIPTION
    Performs a REPL set of question to determine the property interest
    
    .PARAMETER PropertyName
    The name of the RealProperty

    .PARAMETER PersonName
    The name of the LegalPerson

    .EXAMPLE
    PS C:\> $myPropertyInterest = Invoke-PropertyInterestPrompt `
	                              -PropertyName "such-and-such parcel of land" `
								  -PersonName "Cecil B Moneybags"
    Is Present Interest Possible Infinite? (Y/N) : Y
    Is Present Interest Definitely Infinite? (Y/N) : N
    Is Future Interest In Grantor? (Y/N) : Y
    Fee Simple Subject2Executory Interest
	PS C:\>

    .OUTPUTS
    NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests.ILandPropertyInterest
    
#>
function Invoke-PropertyInterestPrompt
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $PropertyName,
        [Parameter(Mandatory=$true,position=1)]
        [string] $PersonName

    )
    Process
    {
        $person = New-Object NoFuture.Rand.Law.LegalPerson($PersonName)

        $factory = Get-PropertyInterestFactory -PropertyName $PropertyName

        while($factory.IsEnd -eq $false){

            $questionMethod = $factory | Get-Member | ? {$_.Name -like "Is*" -and $_.MemberType -eq "Method"}

            $questionMethodName = $questionMethod.Name

            $questionPrompt = [NoFuture.Rand.Law.VocaBase]::TransformCaseToSeparator($questionMethodName, [char]" ")

            $answer = Read-Host -Prompt ("{0}? (Y/N) " -f $questionPrompt) 

            if([string]::Equals($answer, "y", [System.StringComparison]::OrdinalIgnoreCase)){
                $predicate = $truePredicate
            }
            else{
                $predicate = $falsePredicate
            }

            $factory = $factory.$questionMethodName($predicate, $person)
        }

        $interest = $factory.GetValue()

        Write-Host ([NoFuture.Rand.Law.VocaBase]::TransformCaseToSeparator($interest.GetType().Name, [char]" ")) -ForegroundColor Yellow

        return $interest

    }
}