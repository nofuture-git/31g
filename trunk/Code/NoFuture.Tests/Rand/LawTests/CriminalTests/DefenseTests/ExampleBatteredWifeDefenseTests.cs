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
                },
                OtherParties = () => new []{new SpikeEg(), }
            };
            var testResult = testCrime.IsValid(new VeronicaEg());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfSelf(testCrime)
            {
                Imminence = new BatteredWomanSyndrome(testCrime),
                Proportionality = new Proportionality<ITermCategory>(testCrime)
                {
                    GetContribution = lp => new DeadlyForce(),
                },
                Provacation = new Provacation(testCrime)
                {
                    IsInitiatorOfAttack = lp => lp is SpikeEg
                }
            };

            testResult = testSubject.IsValid(new VeronicaEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class SpikeEg : LegalPerson
    {
        public SpikeEg() : base("SPIKE") { }
    }

    public class VeronicaEg : LegalPerson
    {
        public VeronicaEg() : base("VERONICA") { }
    }
}
