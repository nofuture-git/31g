using System;
using NoFuture.Rand.Law.Criminal.AgainstGov.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.AgainstGovTests
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
