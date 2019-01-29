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
    /// CLARK v. WEST Court of Appeals of New York 193 N.Y. 349, 86 N.E. 1 (1908)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, separation of substance of consideration and ancillary conditionals which may be dismissed (waived)
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class ClarkvWestTests
    {
        [Test]
        public void ClarkvWest()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferWriteLawDox(),
                Acceptance = o => o is OfferWriteLawDox ? new AcceptanceWriteLawDox() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Clark _:
                                return ((Clark)lp).GetTerms();
                            case West _:
                                return ((West)lp).GetTerms();
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

            var testSubject = new ConditionsPrecedent<Promise>(testContract)
            {
                IsConditionalTerm = t => t.Name.StartsWith("abstain from"),
                IsNotConditionMet = (t, lp) =>
                {
                    var isParty = lp is West;
                    if (!isParty)
                        return false;
                    var isTerm = t.Name.StartsWith("abstain from");
                    if (!isTerm)
                        return false;

                    //they failed to meet this requirement
                    if (isTerm && isParty)
                        return true;
                    return false;
                }
            };

            var testResult = testSubject.IsValid(new Clark(), new West());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class OfferWriteLawDox : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Clark || offeror is West)
                && (offeree is Clark || offeree is West);
        }
    }
    public class AcceptanceWriteLawDox : OfferWriteLawDox { }

    public class Clark : LegalPerson
    {
        public Clark() : base("WILLIAM LAWRENCE CLARK, JR.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("abstain from the use of intoxicating liquors", DBNull.Value),
            };
        }
    }

    public class West : LegalPerson
    {
        public West() : base("JOHN BRIGGS WEST") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("abstain from the use of intoxicating liquors", DBNull.Value),
            };
        }
    }
}
