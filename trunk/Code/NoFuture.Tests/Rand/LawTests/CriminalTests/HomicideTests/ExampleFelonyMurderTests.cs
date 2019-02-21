using System;
using NoFuture.Rand.Law.Criminal.Homicide.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HomicideTests
{
    [TestFixture()]
    public class ExampleFelonyMurderTests
    {
        [Test]
        public void ExampleFelonyMurder()
        {
            var testFirstCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is JouquinBurnerEg,
                    IsAction = lp => lp is JouquinBurnerEg
                },
                MensRea = new Recklessly
                {
                    IsUnjustifiableRisk = lp => lp is JouquinBurnerEg,
                    IsDisregardOfRisk = lp => lp is JouquinBurnerEg
                }
            };

            var testResult = testFirstCrime.IsValid(new JouquinBurnerEg());
            Console.WriteLine(testFirstCrime.ToString());
            Assert.IsTrue(testResult);

            var testCrime = new Felony
            {
                ActusReus = new FelonyMurder(testFirstCrime)
                {
                    IsCorpusDelicti = lp => lp is JouquinBurnerEg
                },
                MensRea = testFirstCrime.MensRea
            };

            testResult = testCrime.IsValid(new JouquinBurnerEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleDeathOccursBefore()
        {
            var testFirstCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is JouquinBurnerEg,
                    IsAction = lp => lp is JouquinBurnerEg
                },
                MensRea = new Recklessly
                {
                    IsUnjustifiableRisk = lp => lp is JouquinBurnerEg,
                    IsDisregardOfRisk = lp => lp is JouquinBurnerEg
                }
            };

            var testResult = testFirstCrime.IsValid(new JouquinBurnerEg());
            Console.WriteLine(testFirstCrime.ToString());
            Assert.IsTrue(testResult);
            var yyyy = DateTime.Today.Year;

            var testCrime = new Felony
            {
                ActusReus = new FelonyMurder(testFirstCrime)
                {
                    IsCorpusDelicti = lp => lp is JouquinBurnerEg,
                    Inception = new DateTime(yyyy, 3, 15, 9, 0, 0),
                    Terminus = new DateTime(yyyy, 3, 15, 9, 45, 0),
                    TimeOfTheDeath = new DateTime(yyyy, 3, 15, 10, 0, 0),
                },
                MensRea = testFirstCrime.MensRea
            };

            testResult = testCrime.IsValid(new JouquinBurnerEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class JouquinBurnerEg : LegalPerson
    {
        public JouquinBurnerEg() : base("JOUQUIN BURNER") { }
    }
}
