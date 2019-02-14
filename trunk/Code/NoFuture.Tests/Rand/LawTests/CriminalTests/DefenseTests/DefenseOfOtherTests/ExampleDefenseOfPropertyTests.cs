using System;
using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Defense;
using NoFuture.Rand.Law.US.Criminal.Defense.Justification;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US.Criminal.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests.DefenseOfOtherTests
{
    [TestFixture]
    public class ExampleDefenseOfPropertyTests
    {
        [Test]
        public void ExampleDefenseOfProperty()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is KelseyEg,
                    IsVoluntary = lp => lp is KelseyEg
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is KelseyEg,
                    IsIntentOnWrongdoing = lp => lp is KelseyEg
                },
                OtherParties = () => new[] {new KeithEg(),}
            };

            var testResult = testCrime.IsValid(new KelseyEg());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfProperty(testCrime)
            {
                IsBeliefProtectProperty = lp => lp is KelseyEg,
                Imminence = new Imminence(testCrime)
                {
                    GetResponseTime = lp => Imminence.NormalReactionTimeToDanger
                },
                Provacation = new Provacation(testCrime)
                {
                    IsInitiatorOfAttack = lp => lp is KeithEg
                },
                Proportionality = new Proportionality<ITermCategory>(testCrime)
                {
                    GetContribution = lp => new NondeadlyForce()
                }
            };

            testResult = testSubject.IsValid(new KelseyEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class KelseyEg : LegalPerson
    {
        public KelseyEg() : base("") { }
    }

    public class KeithEg : LegalPerson
    {
        public KeithEg() : base("") { }
    }
}
