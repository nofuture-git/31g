using System;
using NoFuture.Rand.Law.Procedure.Civil.US.Judgment;
using NoFuture.Rand.Law.US.Courts;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests.JudgmentTests
{
    [TestFixture]
    public class ExampleTestResJudicata
    {
        [Test]
        public void TestResJudicataIsValid()
        {
            var testSubject = new ResJudicata
            {
                GetAssertion = lp => new ExampleCauseForAction(),
                Court = new StateCourt("KS"),
                IsFinalJudgment = lc => true,
                IsJudgmentBasedOnMerits = lc => true,
                IsSameQuestionOfLawOrFact = lc => lc is ExampleCauseForAction,
                IsSamePartyAsPrior = lp => lp is ExamplePlaintiff || lp is ExampleDefendant
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Assert.IsTrue(testResult);
            Console.WriteLine(testSubject.ToString());
        }
    }
}
