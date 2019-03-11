using System;
using NoFuture.Rand.Law.Contract.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.OffersTests
{
    /// <summary>
    /// PETTERSON v. PATTBERG Court of Appeals of New York 248 N.Y. 86, 161 N.E. 428 (1928)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// The doctrine point of this one is that there was no offer since it was revoked.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class PettersonvPattbergTests
    {
        [Test]
        public void PettersonvPattberg()
        {
            var testSubject = new ComLawContract<Performance>
            {
                Offer = new OfferEarlyMortgagePayment(),
                //this is specific performance therefore any offer may be revoked up-until the last moment
                Acceptance = o => o is OfferEarlyMortgagePayment ? new AcceptanceMoneyTendered() :  null,
            };

            testSubject.Consideration = new Consideration<Performance>(testSubject)
            {
                IsSoughtByOfferor = (lp, p) => lp is Petterson && p is AcceptanceMoneyTendered,
                IsGivenByOfferee = (lp, p) => lp is Pattberg && p is OfferEarlyMortgagePayment
            };

            var testResult = testSubject.IsValid(new Petterson(), new Pattberg());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferEarlyMortgagePayment : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            AddReasonEntry("This offer got revoked when the mortgage was sold to another party");
            return false;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class AcceptanceMoneyTendered : Performance
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class Petterson : LegalPerson
    {
        public Petterson() : base("PETTERSON") { }
    }

    public class Pattberg : LegalPerson
    {
        public Pattberg() : base("PATTBERG") { }
    }
}
