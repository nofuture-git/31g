﻿using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HominiLupusTests
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
                    Consent = new Consent
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

    public class Combatent00 : LegalPerson
    {
        public Combatent00() : base("COMBATENT 00") { }
    }
    public class Combatent01 : Victim
    {
        public Combatent01() : base("COMBATENT 01") { }
    }
}
