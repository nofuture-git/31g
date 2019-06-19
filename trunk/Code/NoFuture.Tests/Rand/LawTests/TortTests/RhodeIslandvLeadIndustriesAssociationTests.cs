using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.IntentionalTort;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Rhode Island v. Lead Industries Association, 951 A.2d 428 (R.I. 2008)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, limits the fuzzy meaning of safety, health, peace, etc. to the actual shared things of air, water and rights of way
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class RhodeIslandvLeadIndustriesAssociationTests
    {
        [Test]
        public void RhodeIslandvLeadIndustriesAssociation()
        {
            var test = new PublicNuisance(ExtensionMethods.Tortfeasor)
            {
                IsUnreasonableInterference = lp => lp is LeadIndustriesAssociation,
                IsRightCommonToPublic = lp => false,
            };
            var testResult = test.IsValid(new RhodeIsland(), new LeadIndustriesAssociation());
            Assert.IsFalse(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class RhodeIsland : LegalPerson, IPlaintiff, IGovernment
    {
        public RhodeIsland(): base("Rhode Island") { }
    }

    public class LeadIndustriesAssociation : LegalPerson, ITortfeasor
    {
        public LeadIndustriesAssociation(): base("Lead Industries Association") { }
    }
}
