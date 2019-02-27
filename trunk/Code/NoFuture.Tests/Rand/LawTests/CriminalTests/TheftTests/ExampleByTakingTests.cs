﻿using System;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft;
using NoFuture.Rand.Law.Criminal.Homicide.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.TheftTests
{
    [TestFixture]
    public class ExampleByTakingTests
    {
        [Test]
        public void ExampleFiveFingerTheft()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new ByTaking
                {
                    AmountOfTheft = 1.25m,
                    SubjectOfTheft = new ChewingGum(),
                    IsTakenUnlawful = lp => lp is JeremyTheifEg,
                    IsToBenefitUnentitled = lp => lp is JeremyTheifEg
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is JeremyTheifEg
                }
            };
            var testResult = testCrime.IsValid(new JeremyTheifEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleEmbezzlementTheft()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new ByTaking
                {
                    SubjectOfTheft =  new LegalProperty("payment for gas"),
                    IsControlOverUnlawful = lp => lp is JeremyTheifEg,
                    IsToBenefitUnentitled = lp => lp is JeremyTheifEg
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is JeremyTheifEg
                }
            };
            var testResult = testCrime.IsValid(new JeremyTheifEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleInvalidTheftWhenOwner()
        {
            var jermey = new JeremyTheifEg();
            var property = new ChewingGum {BelongsTo = jermey};
            var testCrime = new Misdemeanor
            {
                ActusReus = new ByTaking
                {
                    AmountOfTheft = 1.25m,
                    SubjectOfTheft = property,
                    IsTakenUnlawful = lp => lp is JeremyTheifEg,
                    IsToBenefitUnentitled = lp => lp is JeremyTheifEg
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is JeremyTheifEg
                }
            };
            var testResult = testCrime.IsValid(new JeremyTheifEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }

        [Test]
        public void ExampleVictimConsentGiven()
        {
            var cody = new CodyFriendEg();
            var property = new ChewingGum {BelongsTo = cody};

            var testCrime = new Misdemeanor
            {
                ActusReus = new ByTaking
                {
                    SubjectOfTheft = property,
                    IsTakenUnlawful = lp => lp is JeremyTheifEg,
                    IsToDepriveEntitled = lp => lp is JeremyTheifEg,
                    Consent = new Consent
                    {
                        IsCapableThereof = lp => true,
                        //Cody said it was ok to take the property 
                        IsDenialExpressed = lp => !(lp is CodyFriendEg)
                    }
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is JeremyTheifEg
                },
            };
            var testResult = testCrime.IsValid(new JeremyTheifEg(), new CodyFriendEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class ChewingGum : LegalProperty
    {

    }

    public class JeremyTheifEg : LegalPerson
    {
        public JeremyTheifEg() : base("JEREMY THEIF") {}
    }

    public class CodyFriendEg : Victim
    {
        public CodyFriendEg() : base("CODY FRIEND") { }
    }
}
