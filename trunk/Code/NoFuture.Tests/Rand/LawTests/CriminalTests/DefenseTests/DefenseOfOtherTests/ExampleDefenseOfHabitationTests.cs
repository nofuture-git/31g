using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests.DefenseOfOtherTests
{
    [TestFixture]
    public class ExampleDefenseOfHabitationTests
    {
        [Test]
        public void ExampleDefenseOfHabitation()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is BobEg,
                    IsVoluntary = lp => lp is BobEg
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is BobEg,
                    IsIntentOnWrongdoing = lp => lp is BobEg
                }
            };
            var testResult = testCrime.IsValid(new BobEg());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfHabitation(testCrime)
            {
                IsIntruderEnterResidence = lp => true,
                IsOccupiedResidence = lp => true,
            };

            testResult = testSubject.IsValid(new NateEg(), new NateEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class NateEg : LegalPerson
    {
        public NateEg():base("NATE EXAMPLE") {}
    }

    public class BobEg : LegalPerson
    {
        public BobEg() : base("BOB LOVERBOY") {}
    }
}
