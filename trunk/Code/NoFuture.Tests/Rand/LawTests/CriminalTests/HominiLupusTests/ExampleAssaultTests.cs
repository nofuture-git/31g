using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.AgainstPersons;
using NoFuture.Law.Criminal.US.Elements.Intent;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.HominiLupusTests
{
    [TestFixture]
    public class ExampleAssaultTests
    {
        [Test]
        public void ExampleAttemptedBatteryAssault()
        {
            var testCrime = new Felony
            {
                ActusReus = new AttemptedBattery
                {
                    IsProbableDesistance = lp => ((lp as DianaPistolEg)?.IsAimPistol ?? false)
                                                 && ((DianaPistolEg) lp).IsPullTrigger,
                    IsPresentAbility = lp => (lp as DianaPistolEg)?.IsPistolLoaded ?? false
                },
                MensRea = new DeadlyWeapon("loaded pistol")
            };

            var testResult = testCrime.IsValid(new DianaPistolEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestNegligentAttemptBattery()
        {
            var testCrime = new Felony
            {
                ActusReus = new AttemptedBattery
                {
                    IsProbableDesistance = lp => ((lp as DianaPistolEg)?.IsAimPistol ?? false)
                                                 && ((DianaPistolEg)lp).IsPullTrigger,
                    IsPresentAbility = lp => (lp as DianaPistolEg)?.IsPistolLoaded ?? false
                },
                MensRea = new DeadlyWeapon("loaded pistol", new Negligently())
            };

            var testResult = testCrime.IsValid(new DianaPistolEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestThreatenedBatteryAssault()
        {
            var testCrime = new Felony
            {
                ActusReus = new ThreatenedBattery
                {
                    IsByThreatOfViolence = lp => ((lp as DianaPistolEg)?.IsAimPistol ?? false)
                                              && ((DianaPistolEg)lp).IsSheCockGunHammer,
                    IsApparentAbility = lp => lp is DianaPistolEg,
                    Imminence = new Imminence { IsImmediatePresent = ts => true}
                },
                MensRea = new DeadlyWeapon("pistol", new Purposely())
            };

            var testResult = testCrime.IsValid(new DianaPistolEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestThreatenedBatteryAssault_GeneralIntent()
        {
            var testCrime = new Felony
            {
                ActusReus = new ThreatenedBattery
                {
                    IsByThreatOfViolence = lp => ((lp as DianaPistolEg)?.IsAimPistol ?? false)
                                              && ((DianaPistolEg)lp).IsSheCockGunHammer,
                    IsApparentAbility = lp => lp is DianaPistolEg,
                    Imminence = new Imminence { IsImmediatePresent = ts => true }
                },
                MensRea = new DeadlyWeapon("pistol", new GeneralIntent())
            };

            var testResult = testCrime.IsValid(new DianaPistolEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class DianaPistolEg : LegalPerson, IDefendant
    {
        public DianaPistolEg() : base("DIANA PISTOL") {}
        public bool IsAimPistol { get; set; } = true;
        public bool IsPullTrigger { get; set; } = true;
        public bool IsPistolLoaded { get; set; } = true;
        public bool IsSheCockGunHammer { get; set; } = true;
    }
}
