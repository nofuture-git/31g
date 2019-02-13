using System;
using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Defense;
using NoFuture.Rand.Law.US.Criminal.Defense.Justification;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US.Criminal.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests
{
    [TestFixture]
    public class ExampleAttackNotImminentTests
    {
        [Test]
        public void ExampleAttackNotImminent()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is FionaEg,
                    IsVoluntary = lp => lp is FionaEg
                },
                MensRea = new Knowingly
                {
                    IsIntentOnWrongdoing = lp => lp is FionaEg,
                    IsKnowledgeOfWrongdoing = lp => lp is FionaEg
                },
                OtherParties = () => new []{new VinnyEg() }
            };

            var testResult = testCrime.IsValid(new FionaEg());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfSelf(testCrime)
            {
                Imminence = new Imminence(testCrime)
                {
                    GetResponseTime = lp =>
                        lp is FionaEg || lp is VinnyEg ? new TimeSpan(365, 0, 0, 0) : TimeSpan.Zero
                },
                Proportionality = new Proportionality<ITermCategory>(testCrime)
                {
                    GetContribution = lp => new DeadlyForce(),
                },
                Provacation = new Provacation(testCrime)
                {
                    IsInitiatorOfAttack = lp => lp is VinnyEg,
                }
            };

            testResult = testSubject.IsValid(new FionaEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class VinnyEg : LegalPerson
    {
        public VinnyEg() : base("VINNY") {}
    }

    public class FionaEg : LegalPerson
    {
        public FionaEg(): base("FIONA") { }
    }
}
