using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Semiosis;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.LitigateTests
{
    /// <summary>
    /// MASTERSON v. SINE Supreme Court of California 68 Cal. 2d 222, 436 P.2d 561, 65 Cal.Rptr. 545 (1968)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the application of parol evidence rule various alot
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MastersonvSineTests
    {
        [Test]
        public void MastersonvSine()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferSellRanch(),
                Acceptance = o => o is OfferSellRanch ? new AcceptanceSellRanch() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Masterson _:
                                return ((Masterson)lp).GetTerms();
                            case Sine _:
                                return ((Sine)lp).GetTerms();
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

            var testSubject = new ParolEvidenceRule<Promise>(testContract)
            {
                IsCollateralInForm = t => t.Name == "repurchase option",
                IsNotContradictWritten = t => t.Name == "repurchase option",
                IsNotExpectedWritten = t => t.Name == "repurchase option",
            };

            var testResult = testSubject.IsValid(new Masterson(), new Sine());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferSellRanch : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Masterson || offeror is Sine)
                   && (offeree is Masterson || offeree is Sine);
        }
    }

    public class AcceptanceSellRanch : OfferSellRanch { }

    public class Masterson : LegalPerson
    {
        public Masterson(): base("Dallas Masterson") { }

        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("sell ranch", 0, new WrittenTerm()),
                new ContractTerm<object>("repurchase option", 1, new OralTerm())
            };
        }
    }

    public class Sine : LegalPerson
    {
        public Sine(): base("Lu Sine") { }

        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("sell ranch", 0, new WrittenTerm()),
            };
        }

    }
}
