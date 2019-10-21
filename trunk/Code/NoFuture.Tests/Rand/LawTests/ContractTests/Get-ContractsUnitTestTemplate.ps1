function Get-ContractsUnitTestTemplate
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
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.BreachTests
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
            var testContract = new ComLawContract<Promise>
            {
                Offer = new Offer_RenameMe(),
                Acceptance = o => o is Offer_RenameMe ? new Acceptanct_RenameMe() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case ${safeNamePlaintiff} _:
                                return ((${safeNamePlaintiff})lp).GetTerms();
                            case ${safeNameDefendant} _:
                                return ((${safeNameDefendant})lp).GetTerms();
                            default:
                                return null;
                        }
                    }
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };
        }
    }

    public class Offer_RenameMe : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is ${safeNamePlaintiff} || offeror is ${safeNameDefendant})
                   && (offeree is ${safeNamePlaintiff} || offeree is ${safeNameDefendant});
        }

        public override bool Equals(object obj)
        {
            var o = obj as Offer_RenameMe;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Acceptanct_RenameMe : Offer_RenameMe
    {
        public override bool Equals(object obj)
        {
            var o = obj as Acceptanct_RenameMe;
            if (o == null)
                return false;
            return true;
        }
    }

    public class ${safeNamePlaintiff} : LegalPerson, IOfferor
    {
        public ${safeNamePlaintiff}(): base("") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }

    public class ${safeNameDefendant} : LegalPerson, IOfferee
    {
        public ${safeNameDefendant}(): base("") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }
}

"@
        $filename = Join-Path (Get-Location).Path  ".\${safeNamePlaintiff}v${safeNameDefendant}Tests.cs"
        [System.IO.File]::WriteAllText($filename, $someCode, [System.Text.Encoding]::UTF8)
    }
}

$testContractDll = (Resolve-Path (".\bin\Debug\NoFuture.Rand.Law.Contract.Tests.dll")).Path
$nunit = (Resolve-Path ("..\..\..\..\packages\NUnit.ConsoleRunner.3.10.0\tools\nunit3-console.exe"))

function Test-NfRandLawContractMethod($MethodName){
    Invoke-Expression "$nunit $testContractDll --where `"method == $MethodName`""
}

function Test-NfRandLawContract(){
    Invoke-Expression "$nunit $testContractDll"
}