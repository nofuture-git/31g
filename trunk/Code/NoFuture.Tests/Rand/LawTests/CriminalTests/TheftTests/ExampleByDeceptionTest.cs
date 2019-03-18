using System;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.TheftTests
{
    [TestFixture]
    public class ExampleByDeceptionTest
    {
        [Test]
        public void ExampleByFalseImpression()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new ByDeception
                {
                    SubjectProperty = new ActOfService("TUNE-UP SERVICE"),
                    IsFalseImpression = lp => lp is JeremyTheifEg,
                    IsAcquiredTitle = lp => lp is JeremyTheifEg
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is JeremyTheifEg,
                    IsIntentOnWrongdoing = lp => lp is JeremyTheifEg
                }
            };

            var testResult = testCrime.IsValid(new JeremyTheifEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleNotRelianceTest()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new ByDeception
                {
                    SubjectProperty = new ActOfService("TUNE-UP SERVICE"),
                    IsFalseImpression = lp => lp is JeremyTheifEg,
                    IsAcquiredTitle = lp => lp is JeremyTheifEg
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is JeremyTheifEg,
                    IsIntentOnWrongdoing = lp => lp is JeremyTheifEg
                }
            };
            //chuck was aware of the deception and attempted to use is for extortion
            var testReliance = new Reliance
            {
                IsReliantOnFalseRepresentation = lp => !(lp is ChuckUnrelianceEg)
            };

            testCrime.AttendantCircumstances.Add(testReliance);

            var testResult = testCrime.IsValid(new JeremyTheifEg(), new ChuckUnrelianceEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }

        [Test]
        public void ExampleRelianceTest()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new ByDeception
                {
                    SubjectProperty = new ActOfService("TUNE-UP SERVICE"),
                    IsFalseImpression = lp => lp is JeremyTheifEg,
                    IsAcquiredTitle = lp => lp is JeremyTheifEg
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is JeremyTheifEg,
                    IsIntentOnWrongdoing = lp => lp is JeremyTheifEg
                }
            };
            //chuck was aware of the deception and attempted to use is for extortion
            var testReliance = new Reliance
            {
                IsReliantOnFalseRepresentation = lp => lp is ChuckUnrelianceEg
            };

            testCrime.AttendantCircumstances.Add(testReliance);

            var testResult = testCrime.IsValid(new JeremyTheifEg(), new ChuckUnrelianceEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class ChuckUnrelianceEg : LegalPerson, IVictim, IDefendant
    {
        public ChuckUnrelianceEg() : base("CHUCK UNRELIANCE") { }
    }
}
