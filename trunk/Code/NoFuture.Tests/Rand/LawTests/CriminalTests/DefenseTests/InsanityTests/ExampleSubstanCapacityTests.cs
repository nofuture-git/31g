using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense.Excuse.Insanity;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests.InsanityTests
{
    [TestFixture()]
    public class ExampleSubstanCapacityTests
    {
        [Test]
        public void ExampleSubstantialCapacity()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is LoreenEg,
                    IsAction = lp => lp is LoreenEg,
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is LoreenEg,
                }
            };

            var testResult = testCrime.IsValid(new LoreenEg());
            Assert.IsTrue(testResult);

            var testSubject = new SubstantialCapacity
            {
                IsMentalDefect = lp => lp is LoreenEg,
                IsMostlyWrongnessOfAware = lp => !(lp is LoreenEg),
                IsMostlyVolitional = lp => !(lp is LoreenEg)
            };

            testResult = testSubject.IsValid(new LoreenEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class LoreenEg : LegalPerson, IDefendant
    {
        public LoreenEg() : base("LOREEN MADHOUSE") { }
    }
}
