using System;
using NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.AgainstPublicTests
{
    [TestFixture]
    public class ExampleProstitutionTests
    {
        [Test]
        public void TestProstitution()
        {
            var testCrime = new Felony
            {
                ActusReus = new Prostitution
                {
                    IsSexualIntercourse = lp => lp is JohnPayerEg || lp is SueProstitueEg,
                    IsKnowinglyReceived = lp => lp is JohnPayerEg || lp is SueProstitueEg,
                    IsKnowinglyProcured = lp => lp is JohnPayerEg || lp is SueProstitueEg,
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is JohnPayerEg || lp is SueProstitueEg
                }
            };
            var testResult = testCrime.IsValid(new SueProstitueEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);

            testResult = testCrime.IsValid(new JohnPayerEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestPimping()
        {
            var testCrime = new Felony
            {
                ActusReus = new Pimping
                {
                    IsKnowinglyReceived = lp => lp is UpgraddPimpEg || lp is SueProstitueEg,
                    IsFromProstitute = lp => lp is UpgraddPimpEg || lp is SueProstitueEg
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is UpgraddPimpEg || lp is SueProstitueEg
                }
            };
            var testResult = testCrime.IsValid(new UpgraddPimpEg(), new SueProstitueEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class JohnPayerEg : LegalPerson
    {
        public JohnPayerEg() : base("THE JOHN") {  }
    }

    public class SueProstitueEg : LegalPerson
    {
        public SueProstitueEg() : base("SUE PROSTITUE") {  }
    }

    public class UpgraddPimpEg : LegalPerson
    {
        public UpgraddPimpEg() : base("UPGRADD$") { }
    }
}
