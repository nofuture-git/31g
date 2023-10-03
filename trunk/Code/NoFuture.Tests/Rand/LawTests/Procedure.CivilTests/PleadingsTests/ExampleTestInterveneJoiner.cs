using System;
using NoFuture.Law.Procedure.Civil.US.Pleadings;
using NoFuture.Law.US.Courts;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Procedure.Civil.Tests
{
    [TestFixture]
    public class ExampleTestInterveneJoiner
    {
        [Test]
        public void TestInterveneJoinerIsValid()
        {
            var testSubject = new InterveneJoiner
            {
                GetAssertion = lp => new ExampleCauseForAction(),
                Court = new StateCourt("WY"),
                IsStatueAuthorizedRight = lp => lp is ExampleAbsentee
            };

            var testResult =
                testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant(), new ExampleThirdParty());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
            testSubject.ClearReasons();

            testResult =
                testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant(), new ExampleAbsentee());
            Assert.IsTrue(testResult);

            testSubject.IsFeasible = lp => false;
            testSubject.IsStatueAuthorizedRight = lp => false;
            testResult =
                testSubject.IsValid(new ExamplePlaintiff(), new ExampleDefendant(), new ExampleAbsentee{IsIndispensable = true});
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
            testSubject.IsFeasible = lp => true;
            testSubject.IsStatueAuthorizedRight = lp => lp is ExampleAbsentee;

        }
    }

    public class ExampleAbsentee : LegalPerson, IAbsentee
    {
        public ExampleAbsentee() : base("Absent Mr T") { }

        public bool IsIndispensable { get; set; }
    }
}
