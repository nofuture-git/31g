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
    /// UNITED STATES NAVAL INSTITUTE v. CHARTER COMMUNICATIONS, INC. United States Court of Appeals for the Second Circuit 936 F.2d 692 (2d Cir. 1991)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, manner of calc of loss in expectation
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class UsNavalInstitutevCharterCommTests
    {
        [Test]
        public void UsNavalInstitutevCharterComm()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferSellPaperBack(),
                Acceptance = o => o is OfferSellPaperBack ? new AcceptanctSellPaperBack() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case UsNavalInstitute _:
                                return ((UsNavalInstitute)lp).GetTerms();
                            case CharterComm _:
                                return ((CharterComm)lp).GetTerms();
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
            var testResult = testContract.IsValid(new UsNavalInstitute(), new CharterComm());
            Assert.IsTrue(testResult);

            var testSubject = new Expectation<Promise>(testContract)
            {
                CalcLossToInjured = lp => lp is UsNavalInstitute ? 15000m : 0m
            };

            testResult = testSubject.IsValid(new UsNavalInstitute(), new CharterComm());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class OfferSellPaperBack : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is UsNavalInstitute || offeror is CharterComm)
                   && (offeree is UsNavalInstitute || offeree is CharterComm);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferSellPaperBack;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctSellPaperBack : OfferSellPaperBack
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctSellPaperBack;
            if (o == null)
                return false;
            return true;
        }
    }

    public class UsNavalInstitute : LegalPerson, IOfferor
    {
        public UsNavalInstitute(): base("UNITED STATES NAVAL INSTITUTE") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("hardcover", DBNull.Value),
                new ContractTerm<object>("paperback", 2),
            };
        }
    }

    public class CharterComm : LegalPerson, IOfferee
    {
        public CharterComm(): base("CHARTER COMMUNICATIONS, INC.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("hardcover", DBNull.Value),
                new ContractTerm<object>("paperback", 2),
            };
        }
    }
}
