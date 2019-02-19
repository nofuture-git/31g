﻿using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests.DefenseOfSelfTests
{
    [TestFixture]
    public class ExampleAppropriateDeadlyForceTests
    {
        [Test]
        public void ExampleAppropriateDeadlyForce()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is WandaEg,
                    IsAction = lp => lp is WandaEg
                },
                MensRea = new Knowingly
                {
                    IsKnowledgeOfWrongdoing = lp => lp is WandaEg,
                    IsIntentOnWrongdoing = lp => lp is WandaEg
                }
            };

            var testResult = testCrime.IsValid(new WandaEg());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfSelf(testCrime)
            {
                IsReasonableFearOfInjuryOrDeath = lp => true,
                Imminence = new Imminence(testCrime)
                {
                    GetResponseTime = lp => lp is WandaEg ? Imminence.NormalReactionTimeToDanger : TimeSpan.Zero
                },
                Provacation = new Provacation(testCrime)
                {
                    IsInitiatorOfAttack = lp => lp is NicholasEg
                },
                Proportionality = new Proportionality<ITermCategory>(testCrime)
                {
                    //deadly force is appropriate to serious bodily harm
                    GetChoice = lp =>
                    {
                        if (lp is WandaEg)
                            return new DeadlyForce();
                        if (lp is NicholasEg)
                            return new SeriousBodilyInjury();
                        return null;
                    }
                }
            };
            testResult = testSubject.IsValid(new WandaEg(), new NicholasEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class NicholasEg : LegalPerson
    {
        public NicholasEg() :base("NICHOLAS") {}
    }

    public class WandaEg : LegalPerson
    {
        public WandaEg() : base("WANDA") {}
    }
}
