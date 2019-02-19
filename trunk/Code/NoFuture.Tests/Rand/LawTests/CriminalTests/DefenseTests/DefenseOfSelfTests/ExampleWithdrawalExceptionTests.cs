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
    public class ExampleWithdrawalExceptionTests
    {
        [Test]
        public void ExampleWithdrawalException()
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
                    GetResponseTime = lp => lp is PattyEg ? Imminence.NormalReactionTimeToDanger : TimeSpan.Zero
                },
                Provacation = new Provacation(testCrime)
                {
                    //in example, patty slaps paige, paige pummels on patty, patty runs away, paige pursues, patty defends, paige loses
                    IsInitiatorOfAttack = lp => lp is PattyEg,
                    IsInitiatorWithdraws = lp => lp is PattyEg,
                    //in example, these are both non-deadly force responses
                    IsInitiatorRespondingToExcessiveForce = lp => false
                },
                Proportionality = new Proportionality<ITermCategory>(testCrime)
                {
                    GetChoice = lp => new NondeadlyForce()
                }
            };
            testResult = testSubject.IsValid(new PattyEg(), new PaigeEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }
}
