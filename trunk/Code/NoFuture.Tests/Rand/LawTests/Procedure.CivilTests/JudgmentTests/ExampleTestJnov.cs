using System;
using NoFuture.Rand.Law.Procedure.Civil.US.Judgment;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Courts;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests.JudgmentTests
{
    [TestFixture]
    public class ExampleTestJnov
    {

        [Test]
        public void TestJnovIsValid()
        {
            var testSubject = new JudgmentNotwithstandingVerdict
            {
                Court = new StateCourt("LA"),
                GetSubjectPerson = lps => lps.Plaintiff(),
                GetAssertion = lp => new ExampleCauseForAction(),
                IsCaseWeakBeyondReason = lp => lp is ExampleCauseForAction,
                IsMadeMotionPriorToVerdict = lp => false
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);

            testSubject.IsMadeMotionPriorToVerdict = lp => lp is ExamplePlaintiff;
            testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Assert.IsTrue(testResult);

        }
    }
}
