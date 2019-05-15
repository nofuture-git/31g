using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Damage;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.PropertyDestruction
{
    [TestFixture]
    public class ExampleArsonTests
    {
        [Test]
        public void TestArsonAct()
        {
            var testAct = new Arson
            {
                SubjectProperty = new LegalProperty("patch of grass"),
                IsBurned = lp => lp?.Name == "patch of grass",
                IsFireStarter = lp => lp is ClarkBoredburnEg || lp is MannyBoredtwobrunEg
            };
            var testResult = testAct.IsValid(new ClarkBoredburnEg(), new MannyBoredtwobrunEg());
            Console.WriteLine(testAct.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestArsonAsOwner()
        {
            var testCrime = new Felony
            {
                ActusReus = new Arson
                {
                    SubjectProperty = new LegalProperty("ex's stuff")
                    {
                        EntitledTo = new TimBrokenheartEg()
                    },
                    IsBurned = lp => lp?.Name == "ex's stuff",
                    IsFireStarter = lp => lp is TimBrokenheartEg
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is TimBrokenheartEg
                }
            };

            var testResult = testCrime.IsValid(new TimBrokenheartEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class ClarkBoredburnEg : LegalPerson, IDefendant
    {
        public ClarkBoredburnEg() : base("CLARK BOREDBURN") { }
    }

    public class MannyBoredtwobrunEg : LegalPerson, IDefendant
    {
        public MannyBoredtwobrunEg() : base("MANNY BOREDTWOBRUN") { }
    }

    public class TimBrokenheartEg : LegalPerson, IDefendant
    {
        public TimBrokenheartEg() : base("TIM BROKENHEART") { }
    }
}
