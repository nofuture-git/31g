using System;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.ConsiderationTests
{
    /// <summary>
    /// RIDGE RUNNER FORESTRY v. VENEMAN United States Court of Appeals for the Federal Circuit 287 F.3d 1058 (Fed.Cir. 2002)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue: of promises that, "promisor reserves 
    /// a choice of alternative performances" is not consideration.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class RidgeRunnerForestryvVenemanTests
    {
        [Test]
        public void RidgeRunnerForestryvVeneman()
        {
            var testSubject = new ComLawContract<DonativePromise>()
            {
                Offer = new OfferRequestForQuotations(),
                Acceptance = o => o is OfferRequestForQuotations ? new AcceptanceInteragencyRental() : null,
            };
            testSubject.Consideration = new Consideration<DonativePromise>(testSubject)
            {
                IsSoughtByOfferor = (lp, p) => lp is RidgeRunnerForestry && p is AcceptanceInteragencyRental,
                IsGivenByOfferee = (lp, p) => lp is Veneman && p is OfferRequestForQuotations
            };
            var testResult = testSubject.IsValid(new RidgeRunnerForestry(), new Veneman());
            Console.WriteLine(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferRequestForQuotations : DonativePromise
    {

    }

    public class AcceptanceInteragencyRental : DonativePromise
    {

    }

    public class RidgeRunnerForestry : LegalPerson, IOfferor
    {
        public RidgeRunnerForestry() : base("RIDGE RUNNER FORESTRY") { }
    }

    public class Veneman : LegalPerson, IOfferee
    {
        public Veneman() : base("VENEMAN") { }
    }
}
