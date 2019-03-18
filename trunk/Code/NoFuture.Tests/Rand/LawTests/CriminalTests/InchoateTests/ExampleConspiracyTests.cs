using System;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Elements;
using NoFuture.Rand.Law.Criminal.Tests.DefenseTests;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.InchoateTests
{
    [TestFixture]
    public class ExampleConspiracyTests
    {
        [Test]
        public void ExampleConspiracyActNoOvert()
        {
            var testSubject = new Conspiracy
            {
                IsAgreementToCommitCrime = lp => lp is MelissaEg,
                IsOvertActRequired = null
            };

            var testResult = testSubject.IsValid(new MelissaEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleConspiracyActOvert()
        {
            var testSubject = new Conspiracy
            {
                IsAgreementToCommitCrime = lp => lp is MelissaEg,
                IsOvertActRequired = lp => lp is MelissaEg
            };

            var testResult = testSubject.IsValid(new MelissaEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleRequiresSpecificIntent()
        {
            var testCrime = new Felony
            {
                ActusReus = new Conspiracy
                {
                    IsAgreementToCommitCrime = lp => lp is MelissaEg
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is MelissaEg,
                    IsIntentOnWrongdoing = lp => lp is MelissaEg
                }
            };

            var testResult = testCrime.IsValid(new MelissaEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }

        [Test]
        public void ExamplePinkertonRuleTest()
        {
            var testCrime = new Felony
            {
                ActusReus = new PinkertonRule(new Conspiracy
                {
                    IsAgreementToCommitCrime = lp => lp is ShellyDriverEg,
                }),
                MensRea = new SpecificIntent
                {
                    IsIntentOnWrongdoing = lp => lp is ShellyDriverEg || lp is SamRobberEg
                },
            };

            var testResult = testCrime.IsValid(new ShellyDriverEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestWhartonRule()
        {
            var testCrime = new Felony
            {
                ActusReus = new Conspiracy
                {
                    IsAgreementToCommitCrime = lp => lp is ShellyDriverEg,
                    IsConcertOfAction = true
                },
                MensRea = new SpecificIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is ShellyDriverEg,
                    IsIntentOnWrongdoing = lp => lp is ShellyDriverEg
                }
            };
            var testResult = testCrime.IsValid(new ShellyDriverEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class ShellyDriverEg : LegalPerson, IDefendant
    {
        public ShellyDriverEg() : base("SHELLY DRIVER") { }
    }

    public class SamRobberEg : LegalPerson, IDefendant
    {
        public SamRobberEg() : base("SAM ROBBER") { }
    }
}
