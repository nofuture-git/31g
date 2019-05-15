using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstPublic;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.AgainstPublicTests
{
    [TestFixture]
    public class ExampleFailureToDisperseTests
    {
        [Test]
        public void ExampleUnlawfullAssembly()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new UnlawfulAssembly
                {
                    IsUnreasonablyLoud = lp => lp is BuckShouterEg,
                    IsCombative = lp => lp is BuckShouterEg,
                    IsGroupMember = lp => lp is BuckShouterEg
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is BuckShouterEg
                }
            };
            var testResult = testCrime.IsValid(new BuckShouterEg(), new BuckBudoneEg(), new BuckBudtwoEg(),
                new BuckBudthreeEg(), new BuckBudfourEg(), new BuckBudfiveEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleOrderToDisperse()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new FailureToDisperse
                {
                    IsUnreasonablyLoud = lp => lp is BuckShouterEg,
                    IsCombative = lp => lp is BuckShouterEg,
                    IsGroupMember = lp => lp is BuckShouterEg,
                    IsOrderedToDisperse = lp => lp is BuckShouterEg
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is BuckShouterEg
                }
            };
            var testResult = testCrime.IsValid(new BuckShouterEg(), new BuckBudoneEg(), new BuckBudtwoEg(),
                new BuckBudthreeEg(), new BuckBudfourEg(), new BuckBudfiveEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleRiotTest()
        {
            var testCrime = new Felony
            {
                ActusReus = new Riot
                {
                    IsUnreasonablyLoud = lp => lp is BuckShouterEg,
                    IsCombative = lp => lp is BuckShouterEg,
                    IsGroupMember = lp => lp is BuckShouterEg,
                    IsByViolence = lp => lp is BuckShouterEg
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is BuckShouterEg
                }
            };
            var testResult = testCrime.IsValid(new BuckShouterEg(), new BuckBudoneEg(), new BuckBudtwoEg(),
                new BuckBudthreeEg(), new BuckBudfourEg(), new BuckBudfiveEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class BuckShouterEg : LegalPerson, IDefendant
    {
        public BuckShouterEg(): base("BUCK SHOUTER") { }
    }

    public class BuckBudoneEg : BuckShouterEg { }
    public class BuckBudtwoEg : BuckShouterEg { }
    public class BuckBudthreeEg : BuckShouterEg { }
    public class BuckBudfourEg : BuckShouterEg { }
    public class BuckBudfiveEg : BuckShouterEg { }

}
