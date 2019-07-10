using System;
using NoFuture.Rand.Law.Tort.US.IntentionalTort;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tort.Tests
{
    [TestFixture]
    public class ExampleUnjustEnrichmentTests
    {
        [Test]
        public void TestUnjustEnrichmentIsValid()
        {
            var test = new UnjustEnrichment(ExtensionMethods.Tortfeasor)
            {
                IsEnriched = lp => lp is SomeDefendant,
                IsImpoverished = lp => lp is SomePlaintiff,
                IsOtherwiseWithoutRemedyAtLaw = lp => lp is SomePlaintiff,
                LegalCause = new ProximateCause(ExtensionMethods.Tortfeasor)
                {
                    IsForeseeable = lp => lp is SomeDefendant,
                    IsDirectCause = lp => lp is SomeDefendant
                }
            };

            var testResult = test.IsValid(new SomePlaintiff(), new SomeDefendant());
            Assert.IsTrue(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class SomePlaintiff : LegalPerson, IPlaintiff
    {
        public SomePlaintiff() : base("Some P. Laintiff") { }
    }

    public class SomeDefendant : LegalPerson, ITortfeasor
    {
        public SomeDefendant() : base("Billy B Defendant") { }
    }
}
