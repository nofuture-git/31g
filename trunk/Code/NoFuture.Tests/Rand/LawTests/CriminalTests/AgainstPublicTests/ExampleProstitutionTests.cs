using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstPublic;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US.Persons;
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
                    Acceptance = sex => new LegalProperty("money") { PropertyValue = dt => 500m},
                    Assent = new Deal
                    {
                        IsApprovalExpressed = lp => lp is JohnPayerEg || lp is SueProstitueEg
                    },
                    Offer = new SexBipartitie()
                    {
                        IsSexualIntercourse = lp => true,
                        IsOneOfTwo = lp => lp is JohnPayerEg || lp is SueProstitueEg
                    }
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

    public class JohnPayerEg : LegalPerson, IDefendant
    {
        public JohnPayerEg() : base("THE JOHN") {  }
    }

    public class SueProstitueEg : LegalPerson, IDefendant
    {
        public SueProstitueEg() : base("SUE PROSTITUE") {  }
    }

    public class UpgraddPimpEg : LegalPerson, IDefendant
    {
        public UpgraddPimpEg() : base("UPGRADD$") { }
    }
}
