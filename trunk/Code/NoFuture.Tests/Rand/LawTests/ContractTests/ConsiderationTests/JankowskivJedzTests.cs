using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.ConsiderationTests
{
    /// <summary>
    /// JANKOWSKI v. MONCLOVA-MAUMEE-TOLEDO JOINT ECONOMIC DEVELOPMENT ZONE Court of Appeals of Ohio 185 Ohio App. 3d 568, 924 N.E.2d 932 (2010)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// The doctrine issue here is that you can't bargin for what you are already obligated to do
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class JankowskivJedzTests
    {
        [Test]
        public void JankowskivJedz()
        {
            var testSubject = new ComLawContract<Promise>
            {
                Offer = new OfferRenderCustomaryGovSvc(),
                Acceptance = o => o is OfferRenderCustomaryGovSvc ? new AcceptanceOneThirdOfTaxRev() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp => true,
                    TermsOfAgreement = lp => new HashSet<Term<object>> {new Term<object>("undefined", DBNull.Value)}
                }
            };
            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {
                IsGivenByPromisee = (lp, p) => true,
                IsSoughtByPromisor = (lp, p) => true
            };
            var testResult = testSubject.IsValid(new Jankowski(), new Jedz());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class OfferRenderCustomaryGovSvc : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            var isCopm = (offeror is Jankowski || offeror is Jedz)
                         && (offeree is Jankowski || offeree is Jedz);
            if (isCopm)
            {
                AddReasonEntry("The government is already obligated to render " +
                              "these services, so it cannot be bargined for");
            }
            return false;
        }
    }

    public class AcceptanceOneThirdOfTaxRev : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return (offeror is Jankowski || offeror is Jedz)
                   && (offeree is Jankowski || offeree is Jedz);
        }
    }

    public class Jedz : LegalPerson
    {
        public Jedz() :base("MONCLOVA-MAUMEE-TOLEDO JOINT ECONOMIC DEVELOPMENT ZONE") { }
    }

    public class Jankowski : LegalPerson
    {
        public Jankowski() : base("JANKOWSKI") { }
    }
}
