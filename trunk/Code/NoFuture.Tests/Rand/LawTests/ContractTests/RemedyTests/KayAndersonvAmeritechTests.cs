using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Remedy.MoneyDmg;
using NoFuture.Rand.Law.Contract.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.RemedyTests
{
    /// <summary>
    /// KAY & ANDERSON, S.C. v. AMERITECH PUBLISHING, INC. Court of Appeals of Wisconsin 2005 Wisc.App.LEXIS 216
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, what is reasonable in being certian, past performance is admissible
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class KayAndersonvAmeritechTests
    {
        [Test]
        public void KayAndersonvAmeritech()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferYellowPagesListing(),
                Acceptance = o => o is OfferYellowPagesListing ? new AcceptanctYellowPagesListing() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case KayAnderson _:
                                return ((KayAnderson)lp).GetTerms();
                            case Ameritech _:
                                return ((Ameritech)lp).GetTerms();
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

            var testResult = testContract.IsValid(new KayAnderson(), new Ameritech());
            Assert.IsTrue(testResult);

            var testSubject = new Expectation<Promise>(testContract)
            {
                CalcLossToInjured = lp => lp is KayAnderson ? 183000m : 0m
            };

            //the defendant wanted something like this - they failed
            testSubject.Limits.CalcUncertianty = lp => lp is KayAnderson ? 165000 : 0m;

            testResult = testSubject.IsValid(new KayAnderson(), new Ameritech());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferYellowPagesListing : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is KayAnderson || offeror is Ameritech)
                   && (offeree is KayAnderson || offeree is Ameritech);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferYellowPagesListing;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctYellowPagesListing : OfferYellowPagesListing
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctYellowPagesListing;
            if (o == null)
                return false;
            return true;
        }
    }

    public class KayAnderson : LegalPerson
    {
        public KayAnderson(): base("KAY & ANDERSON, S.C.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("yellow pages", DBNull.Value),
            };
        }
    }

    public class Ameritech : LegalPerson
    {
        public Ameritech(): base("AMERITECH PUBLISHING, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("yellow pages", DBNull.Value),
            };
        }
    }
}
