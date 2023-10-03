using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Defense;
using NoFuture.Law.Criminal.US.Defense.Justification;
using NoFuture.Law.Criminal.US.Elements;
using NoFuture.Law.Criminal.US.Elements.Act;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.Criminal.US.Terms;
using NoFuture.Law.Criminal.US.Terms.Violence;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.DefenseTests.DefenseOfSelfTests
{
    [TestFixture]
    public class ExampleBatteredWifeDefenseTests
    {
        [Test]
        public void ExampleBatteredWifeDefense()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is VeronicaEg,
                    IsAction = lp => lp is VeronicaEg
                },
                MensRea = new MaliceAforethought
                {
                    IsIntentOnWrongdoing = lp => lp is VeronicaEg,
                    IsKnowledgeOfWrongdoing = lp => lp is VeronicaEg
                }
            };
            var testResult = testCrime.IsValid(new VeronicaEg());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfSelf
            {
                IsReasonableFearOfInjuryOrDeath = lp => true,
                Imminence = new BatteredWomanSyndrome(ExtensionMethods.Defendant),
                Proportionality = new Proportionality<ITermCategory>(ExtensionMethods.Defendant)
                {
                    GetChoice = lp => new DeadlyForce(),
                },
                Provocation = new Provocation(ExtensionMethods.Defendant)
                {
                    IsInitiatorOfAttack = lp => lp is SpikeEg
                }
            };

            testResult = testSubject.IsValid(new VeronicaEg(), new SpikeEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class SpikeEg : LegalPerson, IVictim
    {
        public SpikeEg() : base("SPIKE") { }
    }

    public class VeronicaEg : LegalPerson, IDefendant
    {
        public VeronicaEg() : base("VERONICA") { }
    }
}
