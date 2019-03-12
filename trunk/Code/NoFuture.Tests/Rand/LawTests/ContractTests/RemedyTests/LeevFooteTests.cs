using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Remedy.MoneyDmg;
using NoFuture.Rand.Law.Contract.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.RemedyTests
{
    /// <summary>
    /// LEE v. FOOTE District of Columbia Court of Appeals 481 A.2d 484 (D.C. 1984)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, calc of some performance value when it was the consideration of a contract
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class LeevFooteTests
    {
        [Test]
        public void LeevFoote()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferCarpentry(),
                Acceptance = o => o is OfferCarpentry ? new AcceptanctDoPlumingWork() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Lee _:
                                return ((Lee)lp).GetTerms();
                            case Foote _:
                                return ((Foote)lp).GetTerms();
                            default:
                                return null;
                        }
                    }
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByOfferee = (lp, p) => true,
                IsSoughtByOfferor = (lp, p) => true
            };

            var testResult = testContract.IsValid(new Lee(), new Foote());
            Assert.IsTrue(testResult);

            var testSubject = new Restitution<Promise>(testContract)
            {
                CalcUnjustGain = lp => lp is Lee ? 1m : 0m,
            };

            testResult = testSubject.IsValid(new Lee(), new Foote());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferCarpentry : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Lee || offeror is Foote)
                   && (offeree is Lee || offeree is Foote);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferCarpentry;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctDoPlumingWork : OfferCarpentry
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctDoPlumingWork;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Lee : LegalPerson, IOfferor
    {
        public Lee(): base("") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }

    public class Foote : LegalPerson, IOfferee
    {
        public Foote(): base("") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("", DBNull.Value),
            };
        }
    }
}
