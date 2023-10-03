using System;
using NoFuture.Law.Procedure.Civil.US.Pleadings;
using NoFuture.Law.US.Courts;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Civil.Tests
{
    [TestFixture]
    public class ExampleTestCounterclaim
    {
        [Test]
        public void TestCounterclaimIsValid()
        {
            var testSubject = new Counterclaim()
            {
                GetAssertion = lp =>
                {
                    if(lp is ExamplePlaintiff)
                        return new ExampleCauseForAction();
                    if(lp is ExampleDefendant)
                        return new ExampleOppositionCausesOfAction();
                    return null;
                },
                Court = new StateCourt("KY"),
                GetRequestedRelief = lp => new ExampleRequestRelief(),
                IsSigned = lp => true,
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
