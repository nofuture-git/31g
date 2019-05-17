using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NoFuture.Rand.Law.Criminal.US.Terms.Violence;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests.DefenseOfOtherTests
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
                }
            };

            var testResult = testCrime.IsValid(new KelseyEg());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfProperty
            {
                IsBeliefProtectProperty = lp => lp is KelseyEg,
                Imminence = new Imminence(ExtensionMethods.Defendant)
                {
                    GetResponseTime = lp => Imminence.NormalReactionTimeToDanger
                },
                Provocation = new Provocation(ExtensionMethods.Defendant)
                {
                    IsInitiatorOfAttack = lp => lp is KeithEg
                },
                Proportionality = new Proportionality<ITermCategory>(ExtensionMethods.Defendant)
                {
                    GetChoice = lp => new NondeadlyForce()
                }
            };

            testResult = testSubject.IsValid(new KelseyEg(), new KeithEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class KelseyEg : LegalPerson, IDefendant
    {
        public KelseyEg() : base("") { }
    }

    public class KeithEg : LegalPerson
    {
        public KeithEg() : base("") { }
    }
}
