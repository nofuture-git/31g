﻿using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests.DefenseOfSelfTests
{
    [TestFixture]
    public class ExampleBatteredWifeDefenseTests
    {
        [Test]
        public void ExampleBatteredWifeDefense()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is VeronicaEg,
                    IsAction = lp => lp is VeronicaEg
                },
                MensRea = new MaliceAforethought
                {
                    IsIntentOnWrongdoing = lp => lp is VeronicaEg,
                    IsKnowledgeOfWrongdoing = lp => lp is VeronicaEg
                }
            };
            var testResult = testCrime.IsValid(new VeronicaEg());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfSelf(testCrime)
            {
                IsReasonableFearOfInjuryOrDeath = lp => true,
                Imminence = new BatteredWomanSyndrome(testCrime),
                Proportionality = new Proportionality<ITermCategory>(testCrime)
                {
                    GetChoice = lp => new DeadlyForce(),
                },
                Provacation = new Provacation(testCrime)
                {
                    IsInitiatorOfAttack = lp => lp is SpikeEg
                }
            };

            testResult = testSubject.IsValid(new VeronicaEg(), new SpikeEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class SpikeEg : LegalPerson
    {
        public SpikeEg() : base("SPIKE") { }
    }

    public class VeronicaEg : LegalPerson
    {
        public VeronicaEg() : base("VERONICA") { }
    }
}