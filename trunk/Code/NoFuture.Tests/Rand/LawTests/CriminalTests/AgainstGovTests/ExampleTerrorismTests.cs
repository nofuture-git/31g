using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.AgainstGov;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.AgainstGovTests
{
    /// <summary>
    /// (U.S. v. Moussaoui, 2011)
    /// </summary>
    [TestFixture]
    public class ExampleTerrorismTests
    {
        [Test]
        public void TerrorismTest()
        {
            var testCrime = new Felony
            {
                ActusReus = new Terrorism
                {
                    IsByViolence = lp => lp is ZacariasMoussaoui,
                    IsSocioPoliticalObjective = lp => lp is ZacariasMoussaoui
                },
                MensRea = new MaliceAforethought
                {
                    IsIntentOnWrongdoing = lp => lp is ZacariasMoussaoui,
                    IsKnowledgeOfWrongdoing = lp => lp is ZacariasMoussaoui
                }
            };
            var testResult = testCrime.IsValid(new ZacariasMoussaoui());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class ZacariasMoussaoui : LegalPerson, IDefendant
    {
        public ZacariasMoussaoui(): base("ZACARIAS MOUSSAOUI") { }
    }
}
