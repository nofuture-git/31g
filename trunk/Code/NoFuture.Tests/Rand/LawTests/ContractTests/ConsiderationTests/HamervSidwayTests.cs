using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.ConsiderationTests
{
    /// <summary>
    /// HAMER v. SIDWAY Court of Appeals of New York 79 Sickels 538, 124 N.Y. 538, 27 N.E. 256 (1891)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, 
    /// [Abstinence from the use [...] was held to furnish a good consideration for a promissory note]
    /// Schnell v Nell only one side of the bargin had value.
    /// Here, both sides of the bargin are considered to have value.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class HamervSidwayTests
    {
        [Test]
        public void HamervSidway()
        {
            var testSubject = new ComLawContract<Promise>
            {
                Offer = new OfferPay5000On21stBday(),
                Acceptance = o => o is OfferPay5000On21stBday ? new AcceptanceToRefrainFromVice() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => new HashSet<Term<object>> { new Term<object>("undefined", DBNull.Value) }
                }
            };

            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {
                IsSoughtByPromisor = (lp, p) => true,
                IsGivenByPromisee = (lp, p) => true
            };

            var testResult = testSubject.IsValid(new Hamer(), new Sidway());
            Console.WriteLine(testResult);
            Console.WriteLine(testSubject.ToString());

        }
    }

    public class OfferPay5000On21stBday : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Hamer || offeree is Hamer)
                   && (offeree is Sidway || offeror is Sidway);
        }
    }

    public class AcceptanceToRefrainFromVice : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return (offeror is Hamer || offeree is Hamer)
                   && (offeree is Sidway || offeror is Sidway);
        }
    }

    public class Hamer : VocaBase, ILegalPerson
    {
        public Hamer() : base("HAMER") { }
    }

    public class Sidway : VocaBase, ILegalPerson
    {
        public Sidway() : base("SIDWAY") { }
    }
}
