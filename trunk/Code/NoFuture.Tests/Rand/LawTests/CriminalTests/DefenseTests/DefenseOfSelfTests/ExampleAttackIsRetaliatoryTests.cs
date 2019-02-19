using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests.DefenseOfSelfTests
{
    [TestFixture]
    public class ExampleAttackIsRetaliatoryTests
    {
        [Test]
        public void ExampleAttackIsRetaliatory()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is DwightEg,
                    IsAction = lp => lp is DwightEg
                },
                MensRea = new MaliceAforethought
                {
                    IsKnowledgeOfWrongdoing = lp => lp is DwightEg,
                    IsIntentOnWrongdoing = lp => lp is DwightEg
                }
            };
            var testResult = testCrime.IsValid(new DwightEg());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfSelf(testCrime)
            {
                IsReasonableFearOfInjuryOrDeath = lp => true,

                Imminence = new Imminence(testCrime)
                {
                    GetResponseTime = lp => lp is DwightEg ? new TimeSpan(0, 0, 2, 0) : TimeSpan.Zero
                },
                Proportionality = new Proportionality<ITermCategory>(testCrime)
                {
                    //example seems to assume that the fist fight is deadly
                    GetChoice = lp => new DeadlyForce()
                },
                Provacation = new Provacation(testCrime)
                {
                    //example doesn't say who started it
                    IsInitiatorOfAttack = lp => true
                }
            };

            testResult = testSubject.IsValid(new DwightEg(), new AbelEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class DwightEg : LegalPerson
    {
        public DwightEg() : base("DWIGHT") {}
    }

    public class AbelEg : LegalPerson
    {
        public AbelEg() : base("ABEL") {}
    }
}
