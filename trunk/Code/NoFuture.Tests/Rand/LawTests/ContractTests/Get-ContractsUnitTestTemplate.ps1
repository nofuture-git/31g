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
    public class ${Plaintiff}v${Defendant}Tests
    {
        [Test]
        public void ${Plaintiff}v${Defendant}()
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
                            case ${Plaintiff} _:
                                return ((${Plaintiff})lp).GetTerms();
                            case ${Defendant} _:
                                return ((${Defendant})lp).GetTerms();
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
            return (offeror is ${Plaintiff} || offeror is ${Defendant})
                   && (offeree is ${Plaintiff} || offeree is ${Defendant});
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

    public class ${Plaintiff} : LegalPerson, IOfferor
    {
        public ${Plaintiff}(): base("") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }

    public class ${Defendant} : LegalPerson, IOfferee
    {
        public ${Defendant}(): base("") { }
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
        $filename = Join-Path (Get-Location).Path  ".\${Plaintiff}v${Defendant}Tests.cs"
        [System.IO.File]::WriteAllText($filename, $someCode, [System.Text.Encoding]::UTF8)
    }
}