using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests.DefenseOfOtherTests
{
    /// <summary>
    /// COMMONWEALTH v. Maria A. MIRANDA. No. 08-P-2094. Decided: June 21, 2010
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, defense of others is subjective test
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class CommonwealthvMirandaTests
    {
        [Test]
        public void CommonwealthvMiranda()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is TrooperSweet,
                    IsVoluntary = lp => lp is TrooperSweet
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is TrooperSweet,
                    IsIntentOnWrongdoing = lp => lp is TrooperSweet
                }
            };

            var testResult = testCrime.IsValid(new Miranda());
            Assert.IsFalse(testResult);

            var testSubject = new DefenseOfOthers
            {
                //Maria attempted to defend Demetria based on the reasonably appearance that she was in danger from trooper Sweet
                IsReasonablyAppearedInjuryOrDeath = lp => lp is Miranda,
                Imminence = new Imminence(ExtensionMethods.Defendant)
                {
                    GetResponseTime = lp => Imminence.NormalReactionTimeToDanger
                },
                Provocation = new Provocation(ExtensionMethods.Defendant)
                {
                    IsInitiatorOfAttack = lp => lp is TrooperSweet,
                },
                Proportionality = new Proportionality<ITermCategory>(ExtensionMethods.Defendant)
                {
                    GetChoice = lp => new NondeadlyForce()
                }
            };

            testResult = testSubject.IsValid(new Miranda(), new DemetriaBattle());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Miranda : LegalPerson, IDefendant
    {
        public Miranda() : base("MARIA A. MIRANDA") {}
    }

    public class DemetriaBattle : LegalPerson { }

    public class TrooperSweet : LegalPerson { }

}
