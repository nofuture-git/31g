using NoFuture.Law.Procedure.Civil.US.Judgment;
using NoFuture.Law.US;
using NoFuture.Law.US.Courts;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Civil.Tests.JudgmentTests
{
    [TestFixture]
    public class ExampleTestJudgmentAsMatterOfLaw
    {
        [Test]
        public void TestJudgmentAsMatterOfLawIsValid()
        {
            var testSubject = new JudgmentAsMatterOfLaw()
            {
                GetSubjectPerson = lps => lps.Plaintiff(),
                Court = new StateCourt("MS"),
                GetAssertion = lp => new ExampleCauseForAction(),
                IsCaseWeakBeyondReason = lc => lc is ExampleCauseForAction
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Assert.IsTrue(testResult);
        }
    }
}
