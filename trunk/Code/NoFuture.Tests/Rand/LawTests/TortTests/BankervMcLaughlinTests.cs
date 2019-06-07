using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.Tort.US.Elements.ReasonableCare;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Banker v. McLaughlin, 146 Tex. 434 (1948)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, children are considered invitees when something is attractive on private property
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class BankervMcLaughlinTests
    {
        [Test]
        public void BankervMcLaughlin()
        {
            var property = new LegalProperty("some hole")
            {
                EntitledTo = new Banker(),
                InPossessionOf = new Banker()
            };

            var test = new OfLandOwner(ExtensionMethods.Tortfeasor)
            {
                SubjectProperty = property,
                Consent = new Consent(ExtensionMethods.Tortfeasor)
                {
                    IsCapableThereof = lp => true,
                    IsApprovalExpressed = lp => false
                },
                IsAttractiveToChildren = p => p.Equals(property)
            };

            var testResult = test.IsValid(new Banker(), new McLaughlin());
            Console.WriteLine(test.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Banker : LegalPerson, ITortfeasor
    {
        public Banker(): base("Banker") { }
    }

    public class McLaughlin : LegalPerson, IPlaintiff, IChild, IVictim
    {
        public McLaughlin(): base("McLaughlin") { }
    }

    
}
