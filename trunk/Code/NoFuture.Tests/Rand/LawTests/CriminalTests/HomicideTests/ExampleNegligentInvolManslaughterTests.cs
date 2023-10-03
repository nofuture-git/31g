using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.Homicide;
using NoFuture.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.HomicideTests
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

    public class StevenSheriffEg : LegalPerson, IDefendant
    {
        public StevenSheriffEg() : base("STEVEN SHERIFF") { }
    }
}
