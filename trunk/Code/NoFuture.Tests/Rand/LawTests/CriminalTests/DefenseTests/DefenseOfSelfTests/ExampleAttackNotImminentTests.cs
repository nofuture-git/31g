using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NoFuture.Rand.Law.Criminal.US.Terms.Violence;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests.DefenseOfSelfTests
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
                }
            };

            var testResult = testCrime.IsValid(new FionaEg());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfSelf
            {
                IsReasonableFearOfInjuryOrDeath = lp => true,
                Imminence = new Imminence(ExtensionMethods.Defendant)
                {
                    GetResponseTime = lp =>
                        lp is FionaEg || lp is VinnyEg ? new TimeSpan(365, 0, 0, 0) : TimeSpan.Zero
                },
                Proportionality = new Proportionality<ITermCategory>(ExtensionMethods.Defendant)
                {
                    GetChoice = lp => new DeadlyForce(),
                },
                Provocation = new Provocation(ExtensionMethods.Defendant)
                {
                    IsInitiatorOfAttack = lp => lp is VinnyEg,
                }
            };

            testResult = testSubject.IsValid(new FionaEg(), new VinnyEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class VinnyEg : LegalPerson
    {
        public VinnyEg() : base("VINNY") {}
    }

    public class FionaEg : LegalPerson, IDefendant
    {
        public FionaEg(): base("FIONA") { }
    }
}
