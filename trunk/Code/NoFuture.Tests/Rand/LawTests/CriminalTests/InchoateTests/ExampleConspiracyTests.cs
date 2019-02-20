using System;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
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
    }
}
