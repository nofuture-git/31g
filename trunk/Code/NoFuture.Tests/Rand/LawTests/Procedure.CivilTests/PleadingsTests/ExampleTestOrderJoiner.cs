using System;
using NoFuture.Rand.Law.Procedure.Civil.US.Pleadings;
using NoFuture.Rand.Law.US.Courts;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Procedure.Civil.Tests
{
    [TestFixture]
    public class ExampleTestOrderJoiner
    {
        [Test]
        public void TestOrderJoinerIsValid()
        {
            var testSubject = new OrderJoiner
            {
                GetAssertion = lp => new ExampleCauseForAction(),
                Court = new StateCourt("CA"),
                GetRequestedRelief = lp => new ExampleRequestRelief(),
                IsSigned = lp => true,
                IsRequiredForCompleteRelief = lp => lp is ExampleAbsentee
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant(), new ExampleAbsentee());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
            testSubject.ClearReasons();

            testSubject.IsRequiredForCompleteRelief = lp => false;
            testSubject.IsRequiredToProtectOthersExposure = lp => lp is ExampleAbsentee;

            testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant(), new ExampleAbsentee());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
            testSubject.ClearReasons();

            testSubject.IsRequiredToProtectOthersExposure = lp => false;
            testSubject.IsRequiredToAvoidContradictoryObligations = lp => lp is ExampleAbsentee;

            testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant(), new ExampleAbsentee());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
            testSubject.ClearReasons();

            testSubject.IsRequiredToAvoidContradictoryObligations = lp => false;
            testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant(), new ExampleAbsentee());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }
}
