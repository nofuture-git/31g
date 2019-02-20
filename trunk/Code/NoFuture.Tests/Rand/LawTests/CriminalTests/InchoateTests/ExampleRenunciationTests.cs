using System;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Defense;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.InchoateTests
{
    [TestFixture]
    public class ExampleRenunciationTests
    {
        [Test]
        public void ExampleRenunciation()
        {
            var testCrime = new Felony
            {
                ActusReus = new Conspiracy
                {
                    IsAgreementToCommitCrime = lp => lp is ShellyDriverEg
                },
                MensRea = new SpecificIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is ShellyDriverEg,
                    IsIntentOnWrongdoing = lp => lp is ShellyDriverEg
                }
            };
            var testResult = testCrime.IsValid(new ShellyDriverEg());
            Assert.IsTrue(testResult);

            var testSubject = new Renunciation(testCrime)
            {
                IsCompletely = lp => lp is ShellyDriverEg,
                IsResultCrimeThwarted = lp => lp is ShellyDriverEg,
                IsVoluntarily = lp => lp is ShellyDriverEg
            };

            testResult = testSubject.IsValid(new ShellyDriverEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }
}
