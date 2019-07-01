function Get-TortUnitTestTemplate
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Plaintiff,
        [Parameter(Mandatory=$true,position=1)]
        [string] $Defendant
    )
    Process
    {
        $safeNamePlaintiff = New-Object System.String(,[char[]]($Plaintiff.ToCharArray() | ? {[char]::IsLetterOrDigit($_)}))
        $safeNameDefendant = New-Object System.String(,[char[]]($Defendant.ToCharArray() | ? {[char]::IsLetterOrDigit($_)}))

        if([char]::IsDigit($safeNamePlaintiff.ToCharArray()[0])){
            $safeNamePlaintiff = "_$safeNamePlaintiff"
        }
        
        if([char]::IsDigit($safeNameDefendant.ToCharArray()[0])){
            $safeNameDefendant = "_$safeNameDefendant"
        }


$someCode = @"
using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.Property.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ${safeNamePlaintiff}v${safeNameDefendant}Tests
    {
        [Test]
        public void ${safeNamePlaintiff}v${safeNameDefendant}()
        {

        }
    }

    public class ${safeNamePlaintiff} : LegalPerson, IPlaintiff
    {
        public ${safeNamePlaintiff}(): base("$Plaintiff") { }
    }

    public class ${safeNameDefendant} : LegalPerson, ITortfeasor
    {
        public ${safeNameDefendant}(): base("$Defendant") { }
    }
}

"@
        $filename = Join-Path (Get-Location).Path  ".\${safeNamePlaintiff}v${safeNameDefendant}Tests.cs"
        [System.IO.File]::WriteAllText($filename, $someCode, [System.Text.Encoding]::UTF8)
    }
}

$testDll = (Resolve-Path (".\bin\Debug\NoFuture.Rand.Law.Tort.Tests.dll")).Path
$nunit = (Resolve-Path ("..\..\..\..\packages\NUnit.ConsoleRunner.3.9.0\tools\nunit3-console.exe"))

function Test-NfRandLawTortMethod($MethodName){
    Invoke-Expression "$nunit $testDll --where `"method == $MethodName`""
}

function Test-NfRandLawTort(){
    Invoke-Expression "$nunit $testDll"
}