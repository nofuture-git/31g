function New-ConstitutionalUnitTest
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Plaintiff,
        [Parameter(Mandatory=$true,position=1)]
        [string] $Defendant,
        [Parameter(Mandatory=$false,position=2)]
        [string] $FileNamePrefix

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
using System.Collections.Generic;
using System.Linq;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Constitutional.Tests
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
    public class ${FileNamePrefix}${safeNamePlaintiff}v${safeNameDefendant}Tests
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

    public class ${safeNameDefendant} : LegalPerson, IDefendant
    {
        public ${safeNameDefendant}(): base("$Defendant") { }
    }
}

"@
        
        $filename = Join-Path (Get-Location).Path  ".\${FileNamePrefix}${safeNamePlaintiff}v${safeNameDefendant}Tests.cs"
        [System.IO.File]::WriteAllText($filename, $someCode, [System.Text.Encoding]::UTF8)
    }
}

$testPropertyDll = (Resolve-Path (".\bin\Debug\NoFuture.Law.Constitutional.Tests.dll")).Path
$nunit = (Resolve-Path ("..\..\..\..\packages\NUnit.ConsoleRunner.3.10.0\tools\nunit3-console.exe"))

function Test-NfRandLawConstitutionalMethod($MethodName){
    Invoke-Expression "$nunit $testPropertyDll --where `"method == $MethodName`""
}

function Test-NfRandLawConstitutional(){
    Invoke-Expression "$nunit $testPropertyDll"
}