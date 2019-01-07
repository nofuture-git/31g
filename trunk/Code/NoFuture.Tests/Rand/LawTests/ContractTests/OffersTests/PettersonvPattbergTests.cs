using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.OffersTests
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
            var testSubject = new UnilateralContract
            {
                Offer = new OfferEarlyMortgagePayment(),
                //this is specific performance therefore any offer may be revoked up-until the last moment
                Acceptance = o => o is OfferEarlyMortgagePayment ? new AcceptanceMoneyTendered() :  null,
            };

            testSubject.Consideration = new Consideration<Performance>(testSubject)
            {
                IsSoughtByPromisor = (lp, p) => lp is Petterson && p is AcceptanceMoneyTendered,
                IsGivenByPromisee = (lp, p) => lp is Pattberg && p is OfferEarlyMortgagePayment
            };

            var testResult = testSubject.IsValid(new Petterson(), new Pattberg());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferEarlyMortgagePayment : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            AddAuditEntry("This offer got revoked when the mortgage was sold to another party");
            return false;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class AcceptanceMoneyTendered : Performance
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return true;
        }

        public override bool IsEnforceableInCourt => true;
    }

    public class Petterson : VocaBase, ILegalPerson
    {
        public Petterson() : base("PETTERSON") { }
    }

    public class Pattberg : VocaBase, ILegalPerson
    {
        public Pattberg() : base("PATTBERG") { }
    }
}
