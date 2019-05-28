using System;
using NoFuture.Rand.Law.Tort.US.Defense;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Gortarez v. Smitty’s Super Valu, 680 P.2d 807 (Ariz. 1984)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue: shopkeeper's privilege
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class GortarezvSmittysSuperValuTests
    {
        [Test]
        public void GortarezvSmittysSuperValu()
        {
            var test = new ShopkeeperPrivilege
            {
                IsReasonableCause = lp => !(lp is SmittysSuperValu),
                IsReasonableManner = lp => !(lp is SmittysSuperValu),
                IsReasonableTime = lp => true,
                SubjectProperty = new AirFreshener(),
                Consent =  new Consent(ExtensionMethods.Tortfeasor)
                {
                    IsCapableThereof = lp => lp is SmittysSuperValu,
                    IsApprovalExpressed = lp => false
                }
            };
            var testResult = test.IsValid(new SmittysSuperValu(), new Gortarez());
            Console.WriteLine(test.ToString());
            Assert.IsFalse(testResult);

        }
    }

    public class AirFreshener : PersonalProperty
    {
        public override decimal? PropertyValue { get; set; } = 0.56m;
        public override ILegalPerson EntitledTo { get; set; } = new SmittysSuperValu();
        public override ILegalPerson InPossessionOf { get; set; } = new SmittysSuperValu();
    }

    public class Gortarez : LegalPerson, IPlaintiff
    {
        public Gortarez(): base("") { }
    }

    public class SmittysSuperValu : LegalPerson, IMerchant<ILegalConcept>, ITortfeasor, IVictim
    {
        public SmittysSuperValu(): base("") { }
        public Predicate<ILegalConcept> IsSkilledOrKnowledgeableOf { get; set; } = g => true;
    }
}
