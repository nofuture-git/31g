using System;
using NoFuture.Rand.Law.Criminal.Homicide.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HomicideTests
{
    [TestFixture]
    public class ExampleNegligentInvolManslaughterTests
    {
        [Test]
        public void ExampleNegligentInvoluntaryManslaughter()
        {
            var testCrime = new Felony
            {
                ActusReus = new ManslaughterInvoluntary
                {
                    IsCorpusDelicti = lp => lp is StevenSheriffEg
                },
                MensRea = new Negligently
                {
                    IsUnawareOfRisk = lp => lp is StevenSheriffEg,
                    IsUnjustifiableRisk = lp => lp is StevenSheriffEg
                }
            };

            var testResult = testCrime.IsValid(new StevenSheriffEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class StevenSheriffEg : LegalPerson
    {
        public StevenSheriffEg() : base("STEVEN SHERIFF") { }
    }
}
