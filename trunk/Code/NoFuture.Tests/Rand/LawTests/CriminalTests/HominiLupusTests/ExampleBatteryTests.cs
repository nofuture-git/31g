using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.AgainstPersons;
using NoFuture.Law.Criminal.US.Elements.AttendantCircumstances;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.HominiLupusTests
{
    [TestFixture]
    public class ExampleBatteryTests
    {
        [Test]
        public void ExampleBattery()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new Battery
                {
                    IsByViolence = lp => lp is HarrietIncestEg
                },
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is HarrietIncestEg
                }
            };

            var testResult = testCrime.IsValid(new HarrietIncestEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleMutualCombat()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new Battery
                {
                    IsByViolence = lp => lp is Combatent00,
                    Consent = new VictimConsent
                    {
                        IsCapableThereof = lp => true,
                        IsApprovalExpressed = lp => true,
                    }
                },
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is Combatent00
                },
            };
            var testResult = testCrime.IsValid(new Combatent00(), new Combatent01());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Combatent00 : LegalPerson, IDefendant
    {
        public Combatent00() : base("COMBATENT 00") { }
    }
    public class Combatent01 : LegalPerson, IVictim
    {
        public Combatent01() : base("COMBATENT 01") { }
    }
}
