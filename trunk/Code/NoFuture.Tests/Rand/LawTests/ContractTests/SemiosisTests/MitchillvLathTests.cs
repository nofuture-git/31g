using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NoFuture.Rand.Law.US.Contracts.Semiosis;
using NoFuture.Rand.Law.US.Contracts.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.SemiosisTests
{
    /// <summary>
    /// MITCHILL v. LATH Court of Appeals of New York 247 N.Y. 377, 160 N.E. 646 (1928)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the objective test of Parol Evidence Rule
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MitchillvLathTests
    {
        [Test]
        public void MitchillvLath()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferPurchaseFarmRemIceHouse(),
                Acceptance = o => o is OfferPurchaseFarmRemIceHouse ? new AcceptancePurchaseFarmRemIceHouse() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Mitchill _:
                                return ((Mitchill) lp).GetTerms();
                            case Lath _:
                                return ((Lath) lp).GetTerms();
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
            var additonalTerms = testContract.Assent.GetAdditionalTerms(new Mitchill(), new Lath());

            Assert.IsNotNull(additonalTerms);
            Assert.AreEqual(1, additonalTerms.Count);
            Assert.AreEqual("remove ice house", additonalTerms.First().Name);

            var testSubject = new ParolEvidenceRule<Promise>(testContract)
            {
                IsCollateralInForm = t => t.Name == "remove ice house",
                IsNotContradictWritten = t => t.Name == "remove ice house",
                //the court finds "remove ice house" is closely related and should have been 
                // present in the contract
                IsNotExpectedWritten = t => false
            };

            var testResult = testSubject.IsValid(new Mitchill(), new Lath());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class OfferPurchaseFarmRemIceHouse : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Mitchill || offeror is Lath)
                   && (offeree is Mitchill || offeree is Lath);
        }
    }

    public class AcceptancePurchaseFarmRemIceHouse : OfferPurchaseFarmRemIceHouse { }

    public class Mitchill : LegalPerson
    {
        public ISet<Term<object>> GetTerms()
        {
            var terms = new HashSet<Term<object>>
            {
                new ContractTerm<object>("purchase farm", 1, new WrittenTerm()),
                new ContractTerm<object>("remove ice house", 2, new OralTerm())
            };
            return terms;
        }
    }

    public class Lath : LegalPerson
    {
        public ISet<Term<object>> GetTerms()
        {
            var terms = new HashSet<Term<object>>
            {
                new ContractTerm<object>("purchase farm", 1, new WrittenTerm()),
            };
            return terms;
        }
    }
}
