using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Law.Contract.US;
using NoFuture.Law.Contract.US.Defense;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Contract.Tests.DefenseTests
{
    /// <summary>
    /// CRABTREE v. ELIZABETH ARDEN SALES CORP. Court of Appeals of New York 305 N.Y. 48, 110 N.E.2d 551 (1953)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, when agreement is written, but across multiple dox some signed, some not
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class CrabtreevElizabethSalesTests
    {
        [Test]
        public void CrabtreevElizabethSales()
        {
            var testContract = new ComLawContract<Promise>
            {
                Offer = new ElizabethSalesOfferTelephoneMemorandum(),
                Acceptance = o => o is ElizabethSalesOfferTelephoneMemorandum ? new AcceptElizabethOffer() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => GetTerms()
                }
            };

            testContract.Consideration = new Consideration<Promise>(testContract)
            {
                IsGivenByOfferee = (lp, p) => true,
                IsSoughtByOfferor = (lp, p) => true
            };

            var testSubject = new StatuteOfFrauds<Promise>(testContract);
            //it is within the statute
            testSubject.Scope.IsYearsInPerformance = c => true;
            testSubject.IsSufficientWriting = c => c.Offer is ElizabethSalesOfferTelephoneMemorandum;
            //this was the main doctrine point of this case, the signature-part 
            //of the statute was scattered about but its overall intent and thereby assent are obvious
            testSubject.IsSigned = c => true;

            var testResult = testSubject.IsValid(new ElizabethSales(), new Crabtree());

            //the defendant was attempting to avoid contract enforcement by 
            // saying it lacked signature part required by statute of frauds
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
        public static ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("employment", DBNull.Value)
            };
        }
    }

    public class ElizabethSalesOfferTelephoneMemorandum : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is ElizabethSales && offeree is Crabtree;
        }

        public DateTime AtTime => new DateTime(1947,9,2, 18,0,0);
        public string PostalAddress => "681-5th Ave, NY NY";
        public decimal BeginPay => 20000m;
        public decimal SixMonths => 25000m;
        public decimal SixxMonths => 30000m;
        public decimal ExpenseMoneyPerYear = 5000m;
        public string Something => "[2 years to make good]";
    }

    public class AcceptElizabethOffer : ElizabethSalesOfferTelephoneMemorandum { }

    public class Crabtree : LegalPerson, IOfferee
    {
        public Crabtree(): base("Nate Crabtree") {}
    }

    public class ElizabethSales : LegalPerson, IOfferor
    {
        public ElizabethSales(): base("Elizabeth Arden Sales Corporation") {}
    }
}
