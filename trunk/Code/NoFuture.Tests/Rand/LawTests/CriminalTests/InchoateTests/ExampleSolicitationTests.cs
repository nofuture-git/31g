using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Inchoate;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.InchoateTests
{
    [TestFixture()]
    public class ExampleSolicitationTests
    {
        [Test]
        public void ExampleSolicitation()
        {
            var testCrime = new Felony
            {
                ActusReus = new Solicitation
                {
                    IsInduceAnotherToCrime = lp => lp is JimmyRequestorEg
                },
                MensRea = new SpecificIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is JimmyRequestorEg
                }
            };

            var testResult = testCrime.IsValid(new JimmyRequestorEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class JimmyRequestorEg : LegalPerson, IDefendant
    {
        public JimmyRequestorEg() : base("JIMMY REQUESTOR") {}
    }
}
