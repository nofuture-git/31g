using System;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.Criminal.AgainstGov.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.AgainstGovTests
{
    [TestFixture]
    public class ExampleTreasonTests
    {
        [Test]
        public void TreasonTest()
        {
            var testCrime = new Felony
            {
                ActusReus = new Treason
                {
                    //taken oath of allegiance to Hitler
                    IsAdheringToEnemy = lp => lp is MildredGillars,
                    WitnessOne = new Recordings00(),
                    WitnessTwo = new Recordings01()
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is MildredGillars
                }
            };

            var testResult = testCrime.IsValid(new MildredGillars());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestTreasonLevyGeneralIntent()
        {
            var testCrime = new Felony
            {
                ActusReus = new Treason
                {
                    //taken oath of allegiance to Hitler
                    IsLevyingWar = lp => lp is MildredGillars,
                    WitnessOne = new Recordings00(),
                    WitnessTwo = new Recordings01()
                },
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is MildredGillars
                }
            };

            var testResult = testCrime.IsValid(new MildredGillars());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Recordings00 : LegalPerson { }

    public class Recordings01 : LegalPerson { }

    public class MildredGillars : LegalPerson
    {
        public MildredGillars() : base("MILDRED GILLARS")
        {
            Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Colloquial, "Axis Sally"));
        }
    }
}
