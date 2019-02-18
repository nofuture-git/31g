using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.EstoppelTests
{
    /// <summary>
    /// HOFFMAN v. RED OWL STORES, INC. Supreme Court of Wisconsin 26 Wis. 2d 683, 133 N.W.2d 267 (1965)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, consideration substitute of promissory estoppel 
    /// applicable to a promise that is not donative
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class HoffmanvRedOwlStoresTests
    {
        [Test]
        public void HoffmanvRedOwlStores()
        {
            var testSubject = new ComLawContract<Promise>()
            {
                Offer = new OfferRedOwlFranchise(),
                Acceptance = o => o is OfferRedOwlFranchise ? new AcceptanceRedOwlFranchise() : null,
            };
            testSubject.Consideration = new PromissoryEstoppel<Promise>(testSubject)
            {
                IsOffereeDependedOnPromise = lp => lp is Hoffman,
                IsOffereePositionWorse = lp => lp is Hoffman
            };

            var testResult = testSubject.IsValid(new RedOwlStores(), new Hoffman());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferRedOwlFranchise : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is RedOwlStores && offeree != null;
        }

        public decimal ExpectedPrice => 18000m;
        public decimal ActualPrice = 24300m;
    }

    public class AcceptanceRedOwlFranchise : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is RedOwlStores && offeree is Hoffman;
        }
    }

    public class Hoffman : LegalPerson
    {
        public Hoffman() : base("Joseph Hoffman") { }
    }

    public class RedOwlStores : LegalPerson
    {
        public RedOwlStores() :base("Red Owl Stores") { }
    }
}
