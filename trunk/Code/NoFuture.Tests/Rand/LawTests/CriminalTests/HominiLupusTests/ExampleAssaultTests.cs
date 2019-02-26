﻿using System;
using NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HominiLupusTests
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
                    IsByThreatOfForce = lp => ((lp as DianaPistolEg)?.IsAimPistol ?? false)
                                              && ((DianaPistolEg)lp).IsSheCockGunHammer,
                    IsPresentAbility = lp => lp is DianaPistolEg
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
                    IsByThreatOfForce = lp => ((lp as DianaPistolEg)?.IsAimPistol ?? false)
                                              && ((DianaPistolEg)lp).IsSheCockGunHammer,
                    IsPresentAbility = lp => lp is DianaPistolEg
                },
                MensRea = new DeadlyWeapon("pistol", new GeneralIntent())
            };

            var testResult = testCrime.IsValid(new DianaPistolEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class DianaPistolEg : LegalPerson
    {
        public DianaPistolEg() : base("DIANA PISTOL") {}
        public bool IsAimPistol { get; set; } = true;
        public bool IsPullTrigger { get; set; } = true;
        public bool IsPistolLoaded { get; set; } = true;
        public bool IsSheCockGunHammer { get; set; } = true;
    }
}