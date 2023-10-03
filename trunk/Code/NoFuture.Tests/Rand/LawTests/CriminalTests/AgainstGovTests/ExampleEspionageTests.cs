using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.AgainstGov;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.AgainstGovTests
{
    [TestFixture]
    public class ExampleEspionageTests
    {
        [Test]
        public void TestEspionage()
        {
            var testCrime = new Felony
            {
                ActusReus = new Espionage("nuclear secrets")
                {
                    IsTransmitor = lp => lp is EthelRosenberg || lp is JuliusRosenberg,
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is EthelRosenberg || lp is JuliusRosenberg
                }
            };

            var testResult = testCrime.IsValid(new EthelRosenberg(), new JuliusRosenberg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class EthelRosenberg : LegalPerson, IDefendant
    {
        public EthelRosenberg(): base ("ETHEL ROSENBERG") {  }
    }

    public class JuliusRosenberg : LegalPerson, IDefendant
    {
        public JuliusRosenberg():base("JULIUS ROSENBERG") { }
    }

}
