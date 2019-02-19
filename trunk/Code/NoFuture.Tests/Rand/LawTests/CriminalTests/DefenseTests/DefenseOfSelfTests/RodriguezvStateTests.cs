using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests.DefenseOfSelfTests
{
    /// <summary>
    /// Rodriguez v. State, 212 S.W.3d 819 (2006).
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, self-defense is invalid when defendant is the insigator
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class RodriguezvStateTests
    {
        [Test]
        public void RodriguezvState()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is Rodriguez,
                    IsVoluntary = lp => lp is Rodriguez
                },
                MensRea = new Knowingly
                {
                    IsIntentOnWrongdoing = lp => lp is Rodriguez,
                    IsKnowledgeOfWrongdoing = lp => lp is Rodriguez
                }
            };

            var testResult = testCrime.IsValid(new Rodriguez());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfSelf(testCrime)
            {
                IsReasonableFearOfInjuryOrDeath = lp => true,
                Imminence = new Imminence(testCrime)
                {
                    GetResponseTime = lp => Imminence.NormalReactionTimeToDanger
                },
                Proportionality = new Proportionality<ITermCategory>(testCrime)
                {
                    GetChoice = lp => new DeadlyForce(),
                },
                Provacation = new Provacation(testCrime)
                {
                    IsInitiatorOfAttack = lp => lp is Rodriguez,
                    IsInitiatorWithdraws = lp => false,
                    IsInitiatorRespondingToExcessiveForce = lp => false
                }
            };
            testResult = testSubject.IsValid(new Rodriguez());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Rodriguez : LegalPerson
    {
        public Rodriguez() : base("JUAN ROBERTO RAMOS RODRIGUEZ") { }
    }

    public class AlejandroRomero : LegalPerson
    {

    }

    public class BaldemarArzola : LegalPerson
    {

    }

    public class JuanMoncivais : LegalPerson
    {

    }
}
