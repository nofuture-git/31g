using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Procedure.Criminal.US.Challenges;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleExclusionaryRuleTests
    {
        [Test]
        public void TestExclusionaryRuleIsValid00()
        {
            var testSubject = new ExclusionaryRule<IVoca>
            {
                GetEvidence = lp => lp is ExampleLawEnforcement ? new ExampleContraband() : null,
                IsObtainedThroughUnlawfulMeans = v => false,
                DerivativeExclusion = new DerivativeExclusionaryRule<IVoca>
                {
                    IsDerivedFromUnlawfulSource = v => true,
                    IsInterveningEventsAttenuated = v => v is ExampleContraband
                }
            };

            var testResult = testSubject.IsValid(new ExampleSuspect(), new ExampleLawEnforcement());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);

        }
    }
}
