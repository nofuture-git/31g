using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Remedy.MoneyDmg;
using NoFuture.Rand.Law.Contract.US.Terms;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.RemedyTests
{
    /// <summary>
    /// DOERING EQUIPMENT CO. v. JOHN DEERE CO. Appeals Court of Massachusetts 61 Mass. App. Ct. 850, 815 N.E.2d 234 (Mass. Ct. App. 2004)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, reliance cannot be used as excuse for recovering from a bad deal
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class DoeringvJohnDeereTests
    {
        [Test]
        public void DoeringvJohnDeere()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new OfferActAsDistributor(),
                Acceptance = o => o is OfferActAsDistributor ? new AcceptanctActAsDistributor() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp =>
                    {
                        switch (lp)
                        {
                            case Doering _:
                                return ((Doering)lp).GetTerms();
                            case JohnDeere _:
                                return ((JohnDeere)lp).GetTerms();
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

            var testResult = testContract.IsValid(new Doering(), new JohnDeere());
            Assert.IsTrue(testResult);

            var testSubject = new Reliance<Promise>(testContract)
            {

            };
            testResult = testSubject.IsValid(new Doering(), new JohnDeere());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class OfferActAsDistributor : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Doering || offeror is JohnDeere)
                   && (offeree is Doering || offeree is JohnDeere);
        }

        public override bool Equals(object obj)
        {
            var o = obj as OfferActAsDistributor;
            if (o == null)
                return false;
            return true;
        }
    }

    public class AcceptanctActAsDistributor : OfferActAsDistributor
    {
        public override bool Equals(object obj)
        {
            var o = obj as AcceptanctActAsDistributor;
            if (o == null)
                return false;
            return true;
        }
    }

    public class Doering : LegalPerson, IOfferor
    {
        public Doering(): base("DOERING EQUIPMENT CO.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("distributor", DBNull.Value),
            };
        }
    }

    public class JohnDeere : LegalPerson, IOfferee
    {
        public JohnDeere(): base("JOHN DEERE CO.") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new ContractTerm<object>("distributor", DBNull.Value),
            };
        }
    }
}
