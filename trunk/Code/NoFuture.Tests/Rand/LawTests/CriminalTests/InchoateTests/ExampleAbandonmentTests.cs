using System;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Defense;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.InchoateTests
{
    [TestFixture]
    public class ExampleAbandonmentTests
    {
        [Test]
        public void VoluntaryAbandonmentTest()
        {
            var testCrime = new Misdemeanor
            {
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is MelissaEg3,
                    IsIntentOnWrongdoing = lp => lp is MelissaEg3
                },
                ActusReus = new Attempt
                {
                    SubstantialSteps = new SubstantialSteps
                    {
                        IsPossessCriminalMaterial = lp => lp is MelissaEg3
                    }
                }
            };

            var testResult = testCrime.IsValid(new MelissaEg3());
            Assert.IsTrue(testResult);

            var testSubject = new Abandonment(testCrime)
            {
                IsMotivatedByFearOfGettingCaught = lp => !MelissaEg3.IsAbandon(lp),
                IsMotivatedByNewDifficulty = lp => !MelissaEg3.IsAbandon(lp)
            };

            testResult = testSubject.IsValid(new MelissaEg3());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class MelissaEg3 : MelissaEg2
    {
        public bool IsAwwedOverByPuppy { get; set; } = true;

        public static bool IsAbandon(ILegalPerson lp)
        {
            var melissa = lp as MelissaEg3;
            if (melissa == null)
                return false;
            return melissa.IsAwwedOverByPuppy;
        }
    }
}
