using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Defense.ToAssent;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.DefenseTests
{
    /// <summary>
    /// AUSTIN INSTRUMENT, INC. v. LORAL CORP. Court of Appeals of New York 29 N.Y.2d 124, 272 N.E.2d 533, 324 N.Y.S.2d 22 (1971)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, econ. duress using existing duty (lack of consideration) as leverage for duress in further contrax
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class AustinInstrumentvLoralCorpTests
    {
        [Test]
        public void AustinInstrumentvLoralCorp()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferSubcontractorComponents(),
                Acceptance = o => o is OfferSubcontractorComponents ? new AcceptanceSubcontractorComponents() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => GetTerms()
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };

            var testSubject = new ByDuress<Promise>(testContract)
            {
                
                ImproperThreat = new ImproperThreat<Promise>(testContract)
                {
                    //austin all ready had a duty to deliver parts,
                    IsBreachOfGoodFaithDuty = lp => lp is AustinInstruments,
                    IsUnfairTerms = lp => lp is AustinInstruments,
                    IsSignificantViaPriorUnfairDeal = lp => lp is AustinInstruments,
                }
            };

            var testResult = testSubject.IsValid(new LoralCorp(), new AustinInstruments());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("radar components", DBNull.Value)
            };
        }
    }

    public class OfferSubcontractorComponents : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is LoralCorp || offeror is AustinInstruments)
                   && (offeree is LoralCorp || offeree is AustinInstruments);
        }
    }

    public class AcceptanceSubcontractorComponents : OfferSubcontractorComponents { }

    public class AustinInstruments : LegalPerson
    {
        public AustinInstruments() : base("AUSTIN INSTRUMENT, INC.") {}
    }

    public class LoralCorp : LegalPerson
    {
        public LoralCorp() : base("LORAL CORP.") {}
    }
}
