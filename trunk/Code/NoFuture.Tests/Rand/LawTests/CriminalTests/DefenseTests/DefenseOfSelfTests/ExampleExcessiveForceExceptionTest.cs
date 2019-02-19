using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests.DefenseOfSelfTests
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
                Imminence = new Imminence(testCrime)
                {
                    GetResponseTime = lp => lp is PattyEg ? Imminence.NormalReactionTimeToDanger : TimeSpan.Zero,
                },
                Provacation = new Provacation(testCrime)
                {
                    IsInitiatorOfAttack = lp => lp is PaigeEg,
                    IsInitiatorWithdraws = lp => false,
                    IsInitiatorRespondingToExcessiveForce = lp => lp is PattyEg,
                },
                Proportionality = new Proportionality<ITermCategory>(testCrime)
                {
                    GetChoice = lp => lp is PaigeEg ? new DeadlyForce() : new NondeadlyForce(),
                }
            };
            testResult = testSubject.IsValid(new PattyEg(), new PaigeEg());
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
