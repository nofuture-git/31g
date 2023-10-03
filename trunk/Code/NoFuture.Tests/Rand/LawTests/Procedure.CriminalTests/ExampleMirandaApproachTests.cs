using System;
using NoFuture.Law.Procedure.Criminal.US.Interrogations;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Criminal.Tests
{
    public class ExampleMirandaApproachTests
    {
        [Test]
        public void TestMirandaApproachIsValid00()
        {
            var testSubject = new MirandaApproach
            {
                IsInCoercivePressureEnvironment = lp => lp is ExampleSuspect,
                IsIncriminatoryQuestioning = lp => lp is ExampleSuspect, 
                IsToldRightToRemainSilent = lp => lp is ExampleSuspect,
                IsToldRightToAttorney = lp => lp is ExampleSuspect
            };

            var testResult = testSubject.IsValid(new ExampleLawEnforcement(), new ExampleSuspect());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestMirandaApproachIsValid01()
        {
            var testSubject = new MirandaApproach
            {
                IsInCoercivePressureEnvironment = lp => lp is ExampleSuspect,
                //is voluntary
                IsIncriminatoryQuestioning = lp => false,
                IsToldRightToRemainSilent = lp => lp is ExampleSuspect,
                IsToldRightToAttorney = lp => lp is ExampleSuspect
            };

            var testResult = testSubject.IsValid(new ExampleLawEnforcement(), new ExampleSuspect());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }
}
