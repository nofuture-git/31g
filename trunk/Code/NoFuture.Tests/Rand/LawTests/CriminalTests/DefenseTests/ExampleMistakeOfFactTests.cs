using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense.Excuse;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests
{
    [TestFixture]
    public class ExampleMistakeOfFactTests
    {
        [Test]
        public void ExampleMistakeOfFactCorrect()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is MickieEg,
                    IsAction = lp => lp is MickieEg,
                },
                MensRea = new GeneralIntent
                {
                    //this is the point, prosecution says this, mistake-of-fact undo's it
                    IsKnowledgeOfWrongdoing = lp => lp is MickieEg
                }
            };

            var testResult = testCrime.IsValid(new MickieEg());
            Assert.IsTrue(testResult);

            var testSubject = new MistakeOfFact
            {
                IsBeliefNegateIntent = lp => lp is MickieEg,
                IsStrictLiability = testCrime.MensRea is StrictLiability
            };

            testResult = testSubject.IsValid(new MickieEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleMistakeOfFactIncorrect()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is TinaEg,
                    IsAction = lp => lp is TinaEg,
                },
                MensRea = StrictLiability.Value
            };

            var testResult = testCrime.IsValid(new TinaEg());
            Assert.IsTrue(testResult);

            var testSubject = new MistakeOfFact
            {
                IsBeliefNegateIntent = lp => lp is TinaEg,
                IsStrictLiability = testCrime.MensRea is StrictLiability
            };

            testResult = testSubject.IsValid(new TinaEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class TinaEg : LegalPerson, IDefendant
    {
        public TinaEg() : base("TINA SPEEDER") {}
    }

    public class MickieEg : LegalPerson, IDefendant
    {
        public MickieEg() : base("MICKIE BIKE") {}
    }
}
