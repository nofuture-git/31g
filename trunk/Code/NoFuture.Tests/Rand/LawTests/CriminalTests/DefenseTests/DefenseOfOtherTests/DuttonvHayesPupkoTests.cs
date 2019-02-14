using System;
using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Defense;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests.DefenseOfOtherTests
{
    [TestFixture]
    public class DuttonvHayesPupkoTests
    {
        [Test]
        public void DuttonvHayesPupko()
        {
            var testSue = new Misdemeanor
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => true,
                    IsVoluntary = lp => true
                },
                MensRea = StrictLiability.Value,
                OtherParties = () => new ILegalPerson[]{new Dutton(), new HayesPupko(), }
            };

            var testResult = testSue.IsValid(new Dutton());
            Assert.IsTrue(testResult);

            var testSubject = new PolicePower(testSue)
            {
                IsAgentOfTheState = lp => lp is Dutton,
                IsReasonableUseOfForce = lp => false
            };

            testResult = testSubject.IsValid(new Dutton());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Dutton : LegalPerson
    {
        public Dutton() : base("DERRICK DUTTON") { }
    }

    public class HayesPupko : LegalPerson
    {
        public HayesPupko() : base("SHERYL HAYES-PUPKO") {}
    }
}
