using System;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft;
using NoFuture.Rand.Law.Criminal.US;
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

    }

    public class ChewingGum : LegalProperty
    {

    }

    public class JeremyTheifEg : LegalPerson
    {
        public JeremyTheifEg() : base("JEREMY THEIF") {}
    }
}
