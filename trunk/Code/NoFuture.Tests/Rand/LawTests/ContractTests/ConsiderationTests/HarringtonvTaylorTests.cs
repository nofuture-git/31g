using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.ConsiderationTests
{
    /// <summary>
    /// HARRINGTON v. TAYLOR Supreme Court of North Carolina 225 N.C. 690, 36 S.E.2d 227 (1945)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, again, court cannot enforce a donative 
    /// promise - even when promisor has their very life saved.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class HarringtonvTaylorTests
    {
        [Test]
        public void HarringtonvTaylor()
        {
            var testSubject = new ComLawContract<LegalConcept>
            {
                Offer = new OfferSavedTaylorsLife(),
                Acceptance = o => o is OfferSavedTaylorsLife ? new AcceptanceWillPayDamage() : null,
            };
            testSubject.Consideration = new Consideration<LegalConcept>(testSubject)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };
            var testResult = testSubject.IsValid(new Harrington(), new Taylor());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferSavedTaylorsLife : Performance
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return true;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class AcceptanceWillPayDamage : DonativePromise
    {

    }

    public class Harrington : LegalPerson
    {
        public Harrington() : base(" ") { }
    }

    public class Taylor : LegalPerson
    {
        public Taylor(): base("") { }
    }
}
