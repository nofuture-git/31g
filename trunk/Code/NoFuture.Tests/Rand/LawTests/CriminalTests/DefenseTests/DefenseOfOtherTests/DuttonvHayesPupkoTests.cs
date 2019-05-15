using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests.DefenseOfOtherTests
{
    /// <summary>
    /// Dutton v. Hayes-Pupko, No. 03-06-00438 (2008).
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the use of police power must be reasonable
    /// ]]>
    /// </remarks>
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
                MensRea = StrictLiability.Value
            };

            var testResult = testSue.IsValid(new Dutton());
            Assert.IsTrue(testResult);

            var testSubject = new PolicePower
            {
                IsAgentOfTheState = lp => lp is Dutton,
                IsReasonableUseOfForce = lp => false
            };

            testResult = testSubject.IsValid(new Dutton(), new HayesPupko());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Dutton : LegalPerson, IDefendant
    {
        public Dutton() : base("DERRICK DUTTON") { }
    }

    public class HayesPupko : LegalPerson, IVictim
    {
        public HayesPupko() : base("SHERYL HAYES-PUPKO") {}
    }
}
