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
    [TestFixture]
    public class ExampleIrrImpulseTests
    {
        [Test]
        public void ExampleIrrImpulseFake()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is JoleneEg,
                    IsVoluntary = lp => lp is JoleneEg,
                },
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is JoleneEg,
                }
            };

            var testResult = testCrime.IsValid(new JoleneEg());
            Assert.IsTrue(testResult);

            var testSubject = new IrresistibleImpluse
            {
                IsMentalDefect = lp => lp is JoleneEg,
                //having not acted on in one case is obvious is volitional
                IsVolitional = lp => true
            };

            testResult = testSubject.IsValid(new JoleneEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class JoleneEg : LegalPerson, IDefendant
    {
        public JoleneEg() : base("JOLENE CUTTER") {}
    }
}
