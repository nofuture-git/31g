using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;
using NUnit.Framework.Internal;

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
