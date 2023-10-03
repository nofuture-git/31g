using System;
using NoFuture.Law.Procedure.Civil.US.Pleadings;
using NoFuture.Law.US.Courts;
using NoFuture.Law.US.Persons;
using NUnit.Framework;


namespace NoFuture.Law.Procedure.Civil.Tests
{
    [TestFixture]
    class ExampleTestSummons
    {
        [Test]
        public void TestSummonsIsValid()
        {
            var testSubject = new Summons
            {
                Court = new FederalCourt("district"),
                IsSigned = lp => true,
                GetAssertion = lp => new ExampleCauseForAction(),
                GetDateOfAppearance = lp => DateTime.Today.AddDays(30),
            };

            var testResult = testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }
    }
}
