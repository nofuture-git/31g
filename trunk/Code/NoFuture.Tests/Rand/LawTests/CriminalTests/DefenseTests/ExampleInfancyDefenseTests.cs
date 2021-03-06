﻿using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense.Excuse;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests
{
    [TestFixture]
    public class ExampleInfancyDefenseTests
    {
        [Test]
        public void ExampleInfancyDefense()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is MarioEg,
                    IsVoluntary = lp => lp is MarioEg,
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is MarioEg,
                }
            };

            var testResult = testCrime.IsValid(new MarioEg());
            Assert.IsTrue(testResult);

            var testSubject = new Infancy();

            testResult = testSubject.IsValid(new MarioEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class MarioEg : LegalPerson, IDefendant, IChild
    {
        public MarioEg() : base("MARIO CANDY") { }
    }
}
