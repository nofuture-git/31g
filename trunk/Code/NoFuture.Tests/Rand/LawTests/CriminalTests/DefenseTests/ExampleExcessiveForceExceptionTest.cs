using System;
using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Defense;
using NoFuture.Rand.Law.US.Criminal.Defense.Justification;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US.Criminal.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests
{
    [TestFixture]
    public class ExampleExcessiveForceExceptionTest
    {
        [Test]
        public void ExampleExcessiveForceException()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is PattyEg,
                    IsVoluntary = lp => lp is PattyEg
                },
                MensRea = new MaliceAforethought
                {
                    IsKnowledgeOfWrongdoing = lp => lp is PattyEg,
                    IsIntentOnWrongdoing = lp => lp is PattyEg
                },
                OtherParties = () => new []{new PaigeEg()}
            };

            var testResult = testCrime.IsValid(new PattyEg());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfSelf(testCrime)
            {
                Imminence = new Imminence(testCrime)
                {
                    GetMinimumResponseTime = lp => lp is PattyEg ? Imminence.NormalReactionTimeToDanger : TimeSpan.Zero,
                },
                Provacation = new Provacation(testCrime)
                {
                    IsInitiatorOfAttack = lp => lp is PaigeEg,
                    IsWithdraws = lp => false,
                    IsResponseToExcessiveForce = lp => lp is PattyEg,
                },
                Proportionality = new Proportionality<Force>(testCrime)
                {
                    GetContribution = lp => lp is PaigeEg ? new DeadlyForce() : new NondeadlyForce(),
                }
            };
            testResult = testSubject.IsValid(new PattyEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class PattyEg : LegalPerson
    {
        public PattyEg(): base("PATTY") { }
    }

    public class PaigeEg : LegalPerson
    {
        public PaigeEg(): base("PAIGE") { }
    }
}
