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
                IsGivenByOfferee = (lp, p) => true,
                IsSoughtByOfferor = (lp, p) => true
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
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
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

    public class Sullivan : LegalPerson, IOfferor
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

    public class Oconnor : LegalPerson, IOfferee
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
