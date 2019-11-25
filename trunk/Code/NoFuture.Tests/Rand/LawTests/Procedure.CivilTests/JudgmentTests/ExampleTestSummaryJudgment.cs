using System;
using NoFuture.Rand.Law.Procedure.Civil.US.Judgment;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Courts;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests.JudgmentTests
{
    [TestFixture]
    public class ExampleTestSummaryJudgment
    {
        [Test]
        public void TestSummaryJudgmentIsValid()
        {
            var testSubject = new SummaryJudgment
            {
                GetSubjectPerson = lps => lps.Plaintiff(),
                Court = new StateCourt("UT"),
                GetAssertion = lp => new ExampleCauseForAction(),
                IsEssentialElement = lc => lc is ExampleCauseForAction,
                RequiredTruthValue = lc => false,
                ActualTruthValue = lc => false,
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }
}
