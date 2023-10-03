using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Defense;
using NoFuture.Law.Criminal.US.Defense.Justification;
using NoFuture.Law.Criminal.US.Elements;
using NoFuture.Law.Criminal.US.Elements.Act;
using NoFuture.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Law.Criminal.US.Terms;
using NoFuture.Law.Criminal.US.Terms.Violence;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.DefenseTests.DefenseOfSelfTests
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

            var testSubject = new DefenseOfSelf
            {
                IsReasonableFearOfInjuryOrDeath = lp => true,
                Imminence = new Imminence(ExtensionMethods.Defendant)
                {
                    GetResponseTime = lp => Imminence.NormalReactionTimeToDanger
                },
                Proportionality = new Proportionality<ITermCategory>(ExtensionMethods.Defendant)
                {
                    GetChoice = lp => new DeadlyForce(),
                },
                Provocation = new Provocation(ExtensionMethods.Defendant)
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

    public class Rodriguez : LegalPerson, IDefendant
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
