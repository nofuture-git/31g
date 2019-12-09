using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Procedure.Criminal.US.Intrusions;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExamplePlainViewSeizureTests
    {
        [Test]
        public void TestPlainViewSeizureIsValid00()
        {
            var testSubject = new PlainViewSeizure
            {
                OriginalIntrusion = new Frisk
                {
                    IsBeliefFreeToGo = lp => false,
                    GetDetainedTimespan = lp => new TimeSpan(0, 15, 0),
                    GetRequiredInvestigateTimespan = lp => new TimeSpan(0, 20, 0),
                    IsBeliefArmedAndDangerous = (lp1, lp2) =>
                        lp1 is ExampleSuspect && lp2 is ExampleLawEnforcement
                        ||
                        lp1 is ExampleLawEnforcement && lp2 is ExampleSuspect
                },
              GetObjectOfSeizure = lp => lp is ExampleSuspect ? new ExampleContraband() : null,
              IsObservedInPermissibleScope = lp => lp is ExampleLawEnforcement,
              IsPlainlyApparentClearlyIllegal = i => i is ExampleContraband
            };

            var testResult = testSubject.IsValid(new ExampleSuspect(), new ExampleLawEnforcement());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class ExampleContraband : VocaBase
    {
        public ExampleContraband(): base("Mare'ij'ah'wana") { }
    }
}
