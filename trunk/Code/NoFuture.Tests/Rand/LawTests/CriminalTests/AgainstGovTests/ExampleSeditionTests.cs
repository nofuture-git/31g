using System;
using NoFuture.Rand.Law.Criminal.AgainstGov.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.AgainstGovTests
{
    [TestFixture]
    public class ExampleSeditionTests
    {
        [Test]
        public void TestSedition()
        {
            var testCrime = new Felony
            {
                ActusReus = new Sedition
                {
                    IsByThreatOfViolence = lp => lp is MoDisgruntledEg,
                    IsToOverthrowGovernment = lp => lp is MoDisgruntledEg,
                    IsInWrittenForm = lp => lp is MoDisgruntledEg
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is MoDisgruntledEg
                }
            };
            var testResult = testCrime.IsValid(new MoDisgruntledEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class MoDisgruntledEg : LegalPerson
    {
        public MoDisgruntledEg(): base("MO DISGRUNTLED") {  }
    }
}
