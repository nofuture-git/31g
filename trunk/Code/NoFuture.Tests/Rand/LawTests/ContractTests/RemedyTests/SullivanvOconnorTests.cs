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
    /// SULLIVAN v. O’CONNOR Supreme Judicial Court of Massachusetts 363 Mass. 579, 296 N.E.2d 183 (1973)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class SullivanvOconnorTests
    {
        [Test]
        public void SullivanvOconnor()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferNoseJob(),
                Acceptance = o => o is OfferNoseJob ? new AcceptanctNoseJob() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Sullivan _:
                                return ((Sullivan)lp).GetTerms();
                            case Oconnor _:
                                return ((Oconnor)lp).GetTerms();
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
            var testResult = testContract.IsValid(new Sullivan(), new Oconnor());
            Assert.IsTrue(testResult);

            var testSubject = new Reliance<Promise>(testContract)
            {
                CalcPrepExpenditures = lp => lp is Sullivan ? 2m : 0m,
                CalcLossAvoided = lp => lp is Sullivan ? 1m : 0m,
            };
            testResult = testSubject.IsValid(new Sullivan(), new Oconnor());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferNoseJob : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Sullivan || offeror is Oconnor)
                   && (offeree is Sullivan || offeree is Oconnor);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferNoseJob;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctNoseJob : OfferNoseJob
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctNoseJob;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Sullivan : LegalPerson
    {
        public Sullivan(): base("SULLIVAN") { }

        public decimal JuryVerdict => 13500m;

        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("operation", DBNull.Value),
            };
        }
    }

    public class Oconnor : LegalPerson
    {
        public Oconnor(): base("O’CONNOR") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("operation", DBNull.Value),
            };
        }
    }
}
