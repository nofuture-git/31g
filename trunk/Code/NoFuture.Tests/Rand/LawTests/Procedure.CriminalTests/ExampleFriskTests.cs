using System;
using NoFuture.Rand.Law.Procedure.Criminal.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleFriskTests
    {
        [Test]
        public void TestFriskIsValid00()
        {
            var testSubject = new Frisk
            {
                IsBeliefFreeToGo = lp => false,
                GetDetainedTimespan = lp => new TimeSpan(0, 15, 0),
                GetRequiredInvestigateTimespan = lp => new TimeSpan(0, 20, 0)
            };

            var testResult = testSubject.IsValid(new ExampleSuspect(), new ExampleLawEnforcement());
            Assert.IsFalse(testResult);
            Console.WriteLine(testSubject.ToString());

        }

        [Test]
        public void TestFriskIsValid01()
        {
            var testSubject = new Frisk
            {
                IsBeliefFreeToGo = lp => false,
                GetDetainedTimespan = lp => new TimeSpan(0, 15, 0),
                GetRequiredInvestigateTimespan = lp => new TimeSpan(0, 20, 0),
                IsBeliefArmedAndDangerous = (lp1, lp2) => 
                    lp1 is ExampleSuspect && lp2 is ExampleLawEnforcement
                    ||
                    lp1 is ExampleLawEnforcement && lp2 is ExampleSuspect
            };

            var testResult = testSubject.IsValid(new ExampleSuspect(), new ExampleLawEnforcement());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());

        }
    }
}
