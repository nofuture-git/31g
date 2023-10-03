using System;
using NUnit.Framework;
using NoFuture.Law.US.Persons;
using NoFuture.Law.Tort.US.Elements;
using NoFuture.Law.Tort.US.ReasonableCare;
using NoFuture.Law.Tort.US.Terms;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.Tests
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
                IsEntitledTo = lp => lp is Banker,
                IsInPossessionOf = lp => lp is Banker
            };

            var test = new OfLandOwner(ExtensionMethods.Tortfeasor)
            {
                SubjectProperty = property,
                Consent = new Consent(ExtensionMethods.Tortfeasor)
                {
                    IsCapableThereof = lp => true,
                    IsApprovalExpressed = lp => false
                },
                AttractiveNuisance = new AttractiveNuisanceTerm(ExtensionMethods.Tortfeasor)
                {
                    IsLocatedWhereChildrenLikelyAre = p => p.Equals(property),
                    IsArtificialCondition = p => p.Equals(property),
                    IsDangerOutweighUse = p => p.Equals(property),
                    IsDangerToChildren = p => p.Equals(property),
                    IsOwnerFailMitigateDanger = lp => true,
                }
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
