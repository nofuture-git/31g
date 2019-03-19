using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Excuse;
using NoFuture.Rand.Law.Contract.US.Terms;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.ExcuseTests
{
    /// <summary>
    /// U.S. BANCORP EQUIPMENT FINANCE, INC. v.AMERIQUEST HOLDINGS LLC United States District Court for the District of Minnesota 2004 U.S.Dist.LEXIS 24709, 55 U.C.C.Rep.Serv. 2d (Callaghan) 423 (2004)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, impracticability is not just economic hardship, even when the same event was used in another case - its not the same thing
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class UsBancorpvAmeriquestTests
    {
        [Test]
        public void UsBancorpvAmeriquest()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferFinanceAirplanes(),
                Acceptance = o => o is OfferFinanceAirplanes ? new AcceptanctFinanceAirplanes() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case UsBancorp _:
                                return ((UsBancorp)lp).GetTerms();
                            case Ameriquest _:
                                return ((Ameriquest)lp).GetTerms();
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

            var testResult = testContract.IsValid(new UsBancorp(), new Ameriquest());
            Assert.IsTrue(testResult);

            var testSubject = new ImpracticabilityOfPerformance<Promise>(testContract)
            {
                IsBasicAssumptionGone = lp => false,
                IsAtFault = lp => false,
                IsContraryInForm = lp => false
            };

            testResult = testSubject.IsValid(new UsBancorp(), new Ameriquest());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class OfferFinanceAirplanes : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is UsBancorp || offeror is Ameriquest)
                   && (offeree is UsBancorp || offeree is Ameriquest);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferFinanceAirplanes;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctFinanceAirplanes : OfferFinanceAirplanes
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctFinanceAirplanes;
            if (o == null)
                return false;
            return true;
        }
    }

    public class UsBancorp : LegalPerson, IOfferor
    {
        public UsBancorp(): base("U.S. BANCORP EQUIPMENT FINANCE, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("MD-82 aircraft", DBNull.Value),
                new ContractTerm<object>("Boeing 737", 2),
            };
        }
    }

    public class Ameriquest : LegalPerson, IOfferee
    {
        public Ameriquest(): base("AMERIQUEST HOLDINGS LL") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("MD-82 aircraft", DBNull.Value),
                new ContractTerm<object>("Boeing 737", 2),
            };
        }
    }
}
