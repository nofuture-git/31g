using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests.DefenseOfOtherTests
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

            var testSubject = new DefenseOfOthers(testCrime)
            {
                //Maria attempted to defend Demetria based on the reasonably appearance that she was in danger from trooper Sweet
                IsReasonablyAppearedInjuryOrDeath = lp => lp is Miranda,
                Imminence = new Imminence(testCrime)
                {
                    GetResponseTime = lp => Imminence.NormalReactionTimeToDanger
                },
                Provacation = new Provacation(testCrime)
                {
                    IsInitiatorOfAttack = lp => lp is TrooperSweet,
                },
                Proportionality = new Proportionality<ITermCategory>(testCrime)
                {
                    GetChoice = lp => new NondeadlyForce()
                }
            };

            testResult = testSubject.IsValid(new Miranda(), new DemetriaBattle());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Miranda : LegalPerson
    {
        public Miranda() : base("MARIA A. MIRANDA") {}
    }

    public class DemetriaBattle : LegalPerson { }

    public class TrooperSweet : LegalPerson { }

}
