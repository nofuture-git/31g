using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests.DefenseOfSelfTests
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
                }
            };

            var testResult = testCrime.IsValid(new PattyEg());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfSelf(testCrime)
            {
                IsReasonableFearOfInjuryOrDeath = lp => true,
                Imminence = new Imminence(testCrime.GetDefendant)
                {
                    GetResponseTime = lp => lp is PattyEg ? Imminence.NormalReactionTimeToDanger : TimeSpan.Zero,
                },
                Provacation = new Provacation(testCrime.GetDefendant)
                {
                    IsInitiatorOfAttack = lp => lp is PaigeEg,
                    IsInitiatorWithdraws = lp => false,
                    IsInitiatorRespondingToExcessiveForce = lp => lp is PattyEg,
                },
                Proportionality = new Proportionality<ITermCategory>(testCrime.GetDefendant)
                {
                    GetChoice = lp => lp is PaigeEg ? new DeadlyForce() : new NondeadlyForce(),
                }
            };
            testResult = testSubject.IsValid(new PattyEg(), new PaigeEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class PattyEg : LegalPerson, IDefendant
    {
        public PattyEg(): base("PATTY") { }
    }

    public class PaigeEg : LegalPerson, IVictim
    {
        public PaigeEg(): base("PAIGE") { }
    }
}
