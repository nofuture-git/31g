using System;
using System.Linq;
using NoFuture.Rand.Law.Procedure.Criminal.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Criminal.Tests
{
    [TestFixture]
    public class ExampleProbableCauseTests
    {
        [Test]
        public void TestProbableCauseIsValid00()
        {
            var testSubject = new ProbableCause
            {
                IsReasonableConcludeCriminalActivity = lp => lp is ExampleLawEnforcement
            };

            var testResult = testSubject.IsValid(new ExampleSuspect(), new ExampleLawEnforcement());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }

        [Test]
        public void TestProbableCauseIsValid01()
        {
            var testSubject = new ProbableCause
            {
                GetInformationSource = lps => lps.FirstOrDefault(lp => lp is IInformant),
                IsReasonableConcludeCriminalActivity = lp => lp is ExampleInformant
            };

            var testResult = testSubject.IsValid(
                new ExampleSuspect(),
                new ExampleInformant {IsInformationSufficientlyCredible = true, IsPersonSufficientlyCredible = true},
                new ExampleLawEnforcement());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class ExampleSuspect : LegalPerson, ISuspect
    {
        public ExampleSuspect() :base("Shaddy McShadison") { }
    }

    public class ExampleInformant : LegalPerson, IInformant
    {
        public ExampleInformant():base("Info Tweeker") { }

        public bool IsPersonSufficientlyCredible { get; set; }
        public bool IsInformationSufficientlyCredible { get; set; }
    }
}
