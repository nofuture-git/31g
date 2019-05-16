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
$someCode = @"
using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

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
    public class ${Plaintiff}v${Defendant}Tests
    {
        [Test]
        public void ${Plaintiff}v${Defendant}()
        {

        }
    }

    public class ${Plaintiff} : LegalPerson, IPlaintiff
    {
        public ${Plaintiff}(): base("") { }
    }

    public class ${Defendant} : LegalPerson, ITortfeasor
    {
        public ${Defendant}(): base("") { }
    }
}

"@
        $filename = Join-Path (Get-Location).Path  ".\${Plaintiff}v${Defendant}Tests.cs"
        [System.IO.File]::WriteAllText($filename, $someCode, [System.Text.Encoding]::UTF8)
    }
}

$testDll = (Resolve-Path (".\bin\Debug\NoFuture.Rand.Law.Tort.Tests.dll")).Path
$nunit = (Resolve-Path ("..\..\..\..\packages\NUnit.ConsoleRunner.3.9.0\tools\nunit3-console.exe"))