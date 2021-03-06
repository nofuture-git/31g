﻿using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstGov;
using NoFuture.Rand.Law.Criminal.US.Elements.AttendantCircumstances;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.AgainstGovTests
{
    [TestFixture]
    public class ExampleSabotageTests
    {
        [Test]
        public void TestSabotage()
        {
            var testCrime = new Felony
            {
                ActusReus = new Sabotage("computer system")
                {
                    IsDamagerOf = lp => lp is MoSabotageEg,
                    IsDefenseProperty = true,
                    IsEntitledTo = lp => lp is UsDeptDefense,
                    IsInPossessionOf = lp => lp is UsDeptDefense
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is MoSabotageEg
                },
                AttendantCircumstances = { new NationalEmergency()}
            };

            var testResult = testCrime.IsValid(new MoSabotageEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class UsDeptDefense : Government
    {
        public UsDeptDefense() : base("US Department of Defense") { }
       
    }

    public class MoSabotageEg : MoDisgruntledEg, IDefendant
    {
        public MoSabotageEg() : base("MO SABOTAGE") { }
    }
}
