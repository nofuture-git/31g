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
    public class ExampleAppropriateDeadlyForceTests
    {
        [Test]
        public void ExampleAppropriateDeadlyForce()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is WandaEg,
                    IsAction = lp => lp is WandaEg
                },
                MensRea = new Knowingly
                {
                    IsKnowledgeOfWrongdoing = lp => lp is WandaEg,
                    IsIntentOnWrongdoing = lp => lp is WandaEg
                },
                OtherParties = () => new[]{new NicholasEg(), }
            };

            var testResult = testCrime.IsValid(new WandaEg());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfSelf(testCrime)
            {
                Imminence = new Imminence(testCrime)
                {
                    GetResponseTime = lp => lp is WandaEg ? Imminence.NormalReactionTimeToDanger : TimeSpan.Zero
                },
                Provacation = new Provacation(testCrime)
                {
                    IsInitiatorOfAttack = lp => lp is NicholasEg
                },
                Proportionality = new Proportionality<ITermCategory>(testCrime)
                {
                    //deadly force is appropriate to serious bodily harm
                    GetContribution = lp =>
                    {
                        if (lp is WandaEg)
                            return new DeadlyForce();
                        if (lp is NicholasEg)
                            return new SeriousBodilyInjury();
                        return null;
                    }
                }
            };
            testResult = testSubject.IsValid(new WandaEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class NicholasEg : LegalPerson
    {
        public NicholasEg() :base("NICHOLAS") {}
    }

    public class WandaEg : LegalPerson
    {
        public WandaEg() : base("WANDA") {}
    }
}
