using System;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.ConsiderationTests
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
                IsGivenByOfferee = (lp, p) => true,
                IsSoughtByOfferor = (lp, p) => true
            };
            var testResult = testSubject.IsValid(new Harrington(), new Taylor());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferSavedTaylorsLife : Performance
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class AcceptanceWillPayDamage : DonativePromise
    {

    }

    public class Harrington : LegalPerson, IOfferor
    {
        public Harrington() : base(" ") { }
    }

    public class Taylor : LegalPerson, IOfferee
    {
        public Taylor(): base("") { }
    }
}
