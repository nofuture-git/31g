using System;
using System.Linq;
using NoFuture.Law.Procedure.Criminal.US.Intrusions;
using NoFuture.Law.Procedure.Criminal.US.Warrants;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleBodilyIntrusionTests
    {
        [Test]
        public void TestBodilyIntrusionIsValid00()
        {
            var testSubject = new BodilyIntrusion
            {
                ProbableCause = new ExampleExigentCircumstances(),
            };

            var testResult = testSubject.IsValid(new ExampleLawEnforcement(), new ExampleSuspect());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestBodilyIntrusionIsValid01()
        {
            var testSubject = new BodilyIntrusion
            {
                Warrant = new ArrestWarrant
                {
                    GetObjectiveOfSearch = () => new ExampleSuspect(),
                    IsObjectiveDescribedWithParticularity = lp => lp is ExampleSuspect,
                    GetIssuerOfWarrant = lps => lps.FirstOrDefault(lp => lp is ExampleJudge),
                    IsIssuerCapableDetermineProbableCause = lp => lp is ExampleJudge,
                    IsIssuerNeutralAndDetached = lp => lp is ExampleJudge,
                    ProbableCause = new ExampleProbableCause()
                }
            };

            var testResult = testSubject.IsValid(new ExampleLawEnforcement(), new ExampleSuspect(), new ExampleJudge());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestBodilyIntrusionIsValid02()
        {
            var testSubject = new BodilyIntrusion
            {
                IsShockingToConscience = lp => lp is ExampleSuspect,
                Warrant = new ArrestWarrant
                {
                    GetObjectiveOfSearch = () => new ExampleSuspect(),
                    IsObjectiveDescribedWithParticularity = lp => lp is ExampleSuspect,
                    GetIssuerOfWarrant = lps => lps.FirstOrDefault(lp => lp is ExampleJudge),
                    IsIssuerCapableDetermineProbableCause = lp => lp is ExampleJudge,
                    IsIssuerNeutralAndDetached = lp => lp is ExampleJudge,
                    ProbableCause = new ExampleProbableCause()
                }
            };

            var testResult = testSubject.IsValid(new ExampleLawEnforcement(), new ExampleSuspect(), new ExampleJudge());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }
}
