using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.RemedyTests
{
    /// <summary>
    ///UNITED STATES ex rel. COASTAL STEEL ERECTORS, INC. v. ALGERNON BLAIR, INC. United States Court of Appeals for the Fourth Circuit 479 F.2d 638 (4th Cir. 1973)    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, restitution as re-payment for work done
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class CoastalSteelvAlgernonTests
    {
        [Test]
        public void CoastalSteelvAlgernon()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferSubContractSteelWork(),
                Acceptance = o => o is OfferSubContractSteelWork ? new AcceptanctSubContractSteelWork() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case CoastalSteel _:
                                return ((CoastalSteel)lp).GetTerms();
                            case Algernon _:
                                return ((Algernon)lp).GetTerms();
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

            var testResult = testContract.IsValid(new CoastalSteel(), new Algernon());
            Assert.IsTrue(testResult);

            var testSubject = new Restitution<Promise>(testContract)
            {
                CalcUnjustGain = lp => lp is CoastalSteel ? 256m : 0m,
            };

            testResult = testSubject.IsValid(new CoastalSteel(), new Algernon());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferSubContractSteelWork : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is CoastalSteel || offeror is Algernon)
                   && (offeree is CoastalSteel || offeree is Algernon);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferSubContractSteelWork;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctSubContractSteelWork : OfferSubContractSteelWork
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctSubContractSteelWork;
            if (o == null)
                return false;
            return true;
        }
    }

    public class CoastalSteel : LegalPerson
    {
        public CoastalSteel(): base("COASTAL STEEL ERECTORS, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("contract", DBNull.Value),
            };
        }
    }

    public class Algernon : LegalPerson
    {
        public Algernon(): base("ALGERNON BLAIR, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("contract", DBNull.Value),
            };
        }
    }
}
