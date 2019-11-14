using System;
using NoFuture.Rand.Law.Procedure.Civil.US.Pleadings;
using NoFuture.Rand.Law.US.Courts;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests
{
    [TestFixture]
    public class ExampleTestCounterclaim
    {
        [Test]
        public void TestCounterclaimIsValid()
        {
            var testSubject = new Counterclaim()
            {
                CausesOfAction = new ExampleCauseForAction(),
                Court = new StateCourt("KY"),
                RequestedRelief = new ExampleRequestRelief(),
                IsSigned = lp => true,
                OppositionCausesOfAction = new ExampleOppositionCausesOfAction(),
                IsSameTransactionOrOccurrence = (lc0,lc1) => lc0 is ExampleCauseForAction && lc1 is ExampleCauseForAction,
                IsSameQuestionOfLawOrFact = (lc0, lc1) => lc0 is ExampleCauseForAction && lc1 is ExampleCauseForAction,
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class ExampleOppositionCausesOfAction : ExampleCauseForAction { }


}
