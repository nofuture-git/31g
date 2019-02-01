using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.RemedyTests
{
    /// <summary>
    /// PEEVYHOUSE v. GARLAND COAL & MINING CO. Supreme Court of Oklahoma 382 P.2d 109 (Okla. 1962)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, concept of performance-rule compared to value-rule.  When doing something 
    /// cost much, much more than the value such a performance will add.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class PeevyhousevGarlandCoalTests
    {
        [Test]
        public void PeevyhousevGarlandCoal()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferLeaseLandForMining(),
                Acceptance = o => o is OfferLeaseLandForMining ? new AcceptanctLeaseLandForMining() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Peevyhouse _:
                                return ((Peevyhouse)lp).GetTerms();
                            case GarlandCoal _:
                                return ((GarlandCoal)lp).GetTerms();
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

            var testResult = testContract.IsValid(new Peevyhouse(), new GarlandCoal());
            Assert.IsTrue(testResult);

            var testSubject = new Expectation<Promise>(testContract)
            {
                CalcLossToInjured = lp => (lp as Peevyhouse)?.DiminutionOfValue ?? 0m,
            };
            testResult = testSubject.IsValid(new Peevyhouse(), new GarlandCoal());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferLeaseLandForMining : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Peevyhouse || offeror is GarlandCoal)
                   && (offeree is Peevyhouse || offeree is GarlandCoal);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferLeaseLandForMining;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctLeaseLandForMining : OfferLeaseLandForMining
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctLeaseLandForMining;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Peevyhouse : LegalPerson
    {
        public Peevyhouse(): base("WILLIE AND LUCILLE PEEVYHOUSE") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("strip-mining", DBNull.Value),
            };
        }

        public decimal CostOfPerformance => 25000m;
        public decimal VerdictForPlaintiffs => 5000m;
        public decimal DiminutionOfValue => 300m;
    }

    public class GarlandCoal : LegalPerson
    {
        public GarlandCoal(): base("GARLAND COAL & MINING CO.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("strip-mining", DBNull.Value),
            };
        }
    }
}
