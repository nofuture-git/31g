using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Law.Contract.US;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Contract.Tests.ConsiderationTests
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
                IsSoughtByOfferor = (lp, p) => true,
                IsGivenByOfferee = (lp, p) => true
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
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is Moran && offeree is Weavertown;
        }
    }

    public class Weavertown : LegalPerson, IOfferor
    {
        public Weavertown() : base("WEAVERTOWN TRANSPORT LEASING, INC.") { }
    }

    public class Moran : LegalPerson, IOfferee
    {
        public Moran(): base("Daniel Moran") { }
    }
}
