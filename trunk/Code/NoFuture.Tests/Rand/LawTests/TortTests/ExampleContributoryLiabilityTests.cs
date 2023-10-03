using System;
using NoFuture.Law.Tort.US.Elements;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Tort.Tests
{
    [TestFixture]
    public class ExampleContributoryLiabilityTests
    {
        [Test]
        public void TestContributoryLiabilityIsValid()
        {
            var test = new ContributoryLiability(ExtensionMethods.Defendant)
            {
                IsAction = lp => lp is SomeDefendant,
                IsVoluntary = lp => lp is SomeDefendant,
                IsMutuallyBeneficialRelationship = (l1, l2) =>
                    l1 is Some3rdParty && l2 is SomeDefendant || l1 is SomeDefendant && l2 is Some3rdParty,
                IsEncourageOthersAct = lp => lp is SomeDefendant
            };

            var testResult = test.IsValid(new SomePlaintiff(), new SomeDefendant(), new Some3rdParty());
            Assert.IsTrue(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class Some3rdParty : LegalPerson, IThirdParty
    {
        public Some3rdParty() : base("Sonny T. Party III") { }
    }
}
