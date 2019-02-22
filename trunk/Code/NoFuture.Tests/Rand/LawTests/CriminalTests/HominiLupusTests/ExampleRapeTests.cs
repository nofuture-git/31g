using System;
using NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HominiLupusTests
{
    [TestFixture]
    public class ExampleRapeTests
    {
        [Test]
        public void ExampleRape()
        {
            var testCrime = new Felony
            {
                ActusReus = new Rape
                {
                    IsByThreatOfForce = lp => lp is AlexGamerEg,
                    IsSexualIntercourse = lp => lp is AlexGamerEg
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is AlexGamerEg
                }
            };

            var testResult = testCrime.IsValid(new AlexGamerEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class AlexGamerEg : LegalPerson
    {
        public AlexGamerEg() : base("ALEX GAMER") { }
    }

    public class BradAlsogamerEg : LegalPerson
    {
        public BradAlsogamerEg(): base("BRAD ALSOGAMER") { }
    }

    public class BrandySisterEg : LegalPerson
    {
        public BrandySisterEg() : base("BRANDY SISTER") { }
    }
}
