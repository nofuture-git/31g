using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.Inchoate;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.InchoateTests
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
