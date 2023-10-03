using System;
using NoFuture.Law.Tort.US.Elements;
using NoFuture.Law.US;
using NUnit.Framework;

namespace NoFuture.Law.Tort.Tests
{
    [TestFixture]
    public class ExampleVicariousLiabilityTests
    {
        [Test]
        public void TestVicariousLiabilityIsValid()
        {
            var test = new VicariousLiability(ExtensionMethods.Defendant)
            {
                IsAction = lp => lp is SomeDefendant,
                IsVoluntary = lp => lp is SomeDefendant,
                IsMutuallyBeneficialRelationship = (l1, l2) =>
                    l1 is Some3rdParty && l2 is SomeDefendant || l1 is SomeDefendant && l2 is Some3rdParty,
                IsAttempt2LimitOthersAct = lp => false
            };
            var testResult = test.IsValid(new SomePlaintiff(), new SomeDefendant(), new Some3rdParty());
            Assert.IsTrue(testResult);
            Console.WriteLine(test.ToString());
        }
    }
}
