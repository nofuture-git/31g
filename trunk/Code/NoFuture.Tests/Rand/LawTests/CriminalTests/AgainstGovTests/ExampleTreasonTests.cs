using System;
using NoFuture.Law.Enums;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.AgainstGov;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.AgainstGovTests
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
                MensRea = new GeneralIntent
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
                    IsByViolence = lp => lp is MildredGillars,
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

    public class MildredGillars : LegalPerson, IDefendant
    {
        public MildredGillars() : base("MILDRED GILLARS")
        {
            Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Colloquial, "Axis Sally"));
        }
    }
}
