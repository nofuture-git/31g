using System;
using NoFuture.Rand.Law.Procedure.Civil.US.Judgment;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Courts;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests.JudgmentTests
{
    [TestFixture]
    public class ExampleTestNewTrial
    {
        [Test]
        public void TestNewTrialIsValid00()
        {
            var testSubject = new NewTrial
            {
                Court = new StateCourt("TN"),
                GetSubjectPerson = lps => lps.Plaintiff(),
                GetAssertion = lp => new ExampleCauseForAction(),
                IsCommittedProceduralError = lp => lp is ExampleCourtJudge
            };
            var testResult =
                testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant(), new ExampleCourtJudge());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }

        [Test]
        public void TestNewTrialIsValid01()
        {
            var testSubject = new NewTrial
            {
                Court = new StateCourt("TN"),
                GetSubjectPerson = lps => lps.Plaintiff(),
                GetAssertion = lp => new ExampleCauseForAction(),
                IsJurySeriouslyErroneousResult = lc => lc is ExampleCauseForAction
            };
            var testResult =
                testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant(), new ExampleCourtJudge());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }

        [Test]
        public void TestNewTrialIsValid02()
        {
            var testSubject = new NewTrial
            {
                Court = new StateCourt("TN"),
                GetSubjectPerson = lps => lps.Plaintiff(),
                GetAssertion = lp => new ExampleCauseForAction(),
                IsNeededToPreventInjustice = lc => lc is ExampleCauseForAction
            };
            var testResult =
                testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant(), new ExampleCourtJudge());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }
    }

    public class ExampleCourtJudge : LegalPerson, ICourtOfficial
    {
        public ExampleCourtJudge(): base("Judge M.B. Judjinbad") { }
    }
}
