using System;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.Contract.US.Ucc;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.UccTests
{
    /// <summary>
    /// GARED HOLDINGS, LLC v. BEST BOLT PRODUCTS, INC. Court of Appeals of Indiana. 991 N.E.2d 1005 (Ind.Ct.App. 2013)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, what makes one a merchant can be subtle 
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class GaredHoldingsvBestBoltTests
    {
        [Test]
        public void GaredHoldingsvBestBolt()
        {
            var testSubject00 = new BestBolt();
            var testResult00 = testSubject00.IsSkilledOrKnowledgeableOf(new Pulleys());

            //this is what the trial court would have to deciede 
            var testSubject01 = new Merchantable(new Pulleys())
            {
                IsPassWithoutObjection = true,
                IsFairAvgQuality = true, //would fail this as well
                IsFit4OrdinaryPurpose = true, //would fail this I think
                IsWithinPermittedVariations = true,
                IsPackagedAndLabeled = true
            };
            var testResult01 = testSubject01.IsValid(null, null);
            Assert.IsTrue(testResult01);
            Assert.IsTrue(testResult00);

        }
    }

    public class BoltsNutsScrewsAndOtherHardwareItems : Goods { }

    public class Pulleys : Goods { }

    public class GaredHoldings : LegalPerson
    {
        public GaredHoldings() : base("GARED HOLDINGS, LLC") { }
    }

    public class BestBolt : Merchant, IOfferor
    {
        public BestBolt() : base("BEST BOLT PRODUCTS, INC.") { }

        public override Predicate<Goods> IsSkilledOrKnowledgeableOf { get; set; } = goods =>
        {
            if (goods is BoltsNutsScrewsAndOtherHardwareItems)
            {
                goods.AddReasonEntry("bolts, nuts, screws and other hardware " +
                               $"items is the core business of {nameof(BestBolt)}");
                return true;
            }

            if (goods is Pulleys)
            {
                goods.AddReasonEntry($"the court found that {nameof(BestBolt)} is" +
                               " a merchant of pulleys.");
                return true;
            }

            return false;
        };

    }
}
