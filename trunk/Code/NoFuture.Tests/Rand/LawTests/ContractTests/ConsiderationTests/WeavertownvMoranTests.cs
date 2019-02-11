using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.LawTests.ContractTests.ConsiderationTests
{
    /// <summary>
    /// WEAVERTOWN TRANSPORT LEASING, INC. v. MORAN Superior Court of Pennsylvania 834 A.2d 1169 (Pa.Super.Ct. 2003)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, seems to be that understanding the side of a bargin in consideration can be 
    /// tricky.  Appeal court found the arrangement as "gratuitous" so its without consideration.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class WeavertownvMoranTests
    {
        [Test]
        public void WeavertownvMoran()
        {
            var testSubject = new ComLawContract<Promise>
            {
                Offer = new OfferSellTicketLicx(),
                Acceptance = o => o is OfferSellTicketLicx ? new AcceptanceBuyTicketLicx() : null,
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

            var testResult = testSubject.IsValid(new Weavertown(), new Moran());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferSellTicketLicx : DonativePromise
    {
    }

    public class AcceptanceBuyTicketLicx : Promise
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return offeror is Moran && offeree is Weavertown;
        }
    }

    public class Weavertown : LegalPerson
    {
        public Weavertown() : base("WEAVERTOWN TRANSPORT LEASING, INC.") { }
    }

    public class Moran : LegalPerson
    {
        public Moran(): base("Daniel Moran") { }
    }
}
