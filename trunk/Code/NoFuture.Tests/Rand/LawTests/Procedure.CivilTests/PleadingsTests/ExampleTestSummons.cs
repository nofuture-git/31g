using System;
using NoFuture.Rand.Law.Procedure.Civil.US.Pleadings;
using NoFuture.Rand.Law.US.Courts;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;


namespace NoFuture.Rand.Law.Procedure.Civil.Tests
{
    [TestFixture]
    class ExampleTestSummons
    {
        [Test]
        public void TestSummonsIsValid()
        {
            var testSubject = new Summons
            {
                Court = new FederalCourt("district"), IsSigned = lp => true,
                GetCausesOfAction = lp => new ExampleCauseForAction(),
                GetDateOfAppearance = lp => DateTime.Today.AddDays(30),
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }
    }
}
