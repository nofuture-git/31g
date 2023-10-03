using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Law.Contract.US;
using NoFuture.Law.Contract.US.Remedy.MoneyDmg;
using NoFuture.Law.Contract.US.Terms;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Contract.Tests.RemedyTests
{
    /// <summary>
    /// SOUTHERN ILLINOIS RIVERBOAT/CASINO CRUISES, INC. v.HNEDAK BOBO GROUP, INC. United States District Court for the Southern District of Illinois 2007 U.S.Dist.LEXIS 53776 (No. 03-CV-4215-JPG)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, what is foreseeable is true even when neither party actually does so
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class HarrahsvHnedakBoboTests
    {
        [Test]
        public void HarrahsvHnedakBobo()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferDesignDockingFacility(),
                Acceptance = o => o is OfferDesignDockingFacility ? new AcceptanctDesignDockingFacility() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Harrahs _:
                                return ((Harrahs)lp).GetTerms();
                            case HnedakBobo _:
                                return ((HnedakBobo)lp).GetTerms();
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

            var testResult = testContract.IsValid(new Harrahs(), new HnedakBobo());
            Assert.IsTrue(testResult);

            var testSubject = new Expectation<Promise>(testContract)
            {
                CalcLossToInjured = lp => 100m
            };

            //this is what HBG wanted summary judgement of
            testSubject.Limits.CalcUnforeseeable = lp => 100m;

            testResult = testSubject.IsValid(new Harrahs(), new HnedakBobo());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);

        }
    }

    public class OfferDesignDockingFacility : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Harrahs || offeror is HnedakBobo)
                   && (offeree is Harrahs || offeree is HnedakBobo);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferDesignDockingFacility;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctDesignDockingFacility : OfferDesignDockingFacility
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctDesignDockingFacility;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Harrahs : LegalPerson, IOfferor
    {
        public Harrahs(): base("SOUTHERN ILLINOIS RIVERBOAT/CASINO CRUISES, INC.") { }

        public decimal DamagesFromFireWatch => 100m;

        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("docking facility", DBNull.Value),
                new ContractTerm<object>("ingress", -1),
                new ContractTerm<object>("egress", 1),
            };
        }
    }

    public class HnedakBobo : LegalPerson, IOfferee
    {
        public HnedakBobo() : base("HNEDAK BOBO GROUP, INC.")
        {
            
        }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("docking facility", DBNull.Value),
                new ContractTerm<object>("ingress", -1),
                new ContractTerm<object>("egress", 1),
            };
        }
    }
}
