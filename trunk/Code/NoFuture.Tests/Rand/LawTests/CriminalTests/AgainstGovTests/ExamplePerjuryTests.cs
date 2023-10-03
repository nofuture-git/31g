using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.AgainstGov;
using NoFuture.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.AgainstGovTests
{
    [TestFixture]
    public class ExamplePerjuryTests
    {
        [Test]
        public void PerjuryTest()
        {
            var testCrime = new Felony
            {
                ActusReus = new Perjury
                {
                    IsFalseTestimony = lp => lp is MarcusWitnessEg,
                    IsJudicialProceeding = lp => lp is MarcusWitnessEg,
                    IsUnderOath = lp => lp is MarcusWitnessEg,
                    //lied about some immaterial stuff
                    IsMaterialIssue = lp => false
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is MarcusWitnessEg
                }
            };

            var testResult = testCrime.IsValid(new MarcusWitnessEg());
            Console.WriteLine(testCrime);
            Assert.IsFalse(testResult);
        }
    }

    public class MarcusWitnessEg : LegalPerson, IDefendant
    {
        public MarcusWitnessEg() : base("MARCUS WITNESS") { }
    }
}
