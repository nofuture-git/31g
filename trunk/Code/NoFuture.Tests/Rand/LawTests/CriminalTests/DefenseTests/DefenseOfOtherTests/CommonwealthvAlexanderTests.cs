using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests.DefenseOfOtherTests
{
    /// <summary>
    /// Commonwealth v. Alexander, 531 S.E.2d 567 (2000)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, defense of property with deadly force is not lawful
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class CommonwealthvAlexanderTests
    {
        [Test]
        public void CommonwealthvAlexander()
        {
            var testCrime = new Infraction
            {
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is Alexander,
                    IsKnowledgeOfWrongdoing = lp => lp is MichaelTEustler
                },
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is Alexander,
                    IsAction = lp => lp is Alexander
                }
            };
            var testResult = testCrime.IsValid(new Alexander());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfProperty(testCrime)
            {
                Imminence = new Imminence(testCrime.GetDefendant)
                {
                    GetResponseTime = lp => Imminence.NormalReactionTimeToDanger
                },
                Provacation = new Provacation(testCrime.GetDefendant)
                {
                    IsInitiatorOfAttack = lp => lp is MichaelTEustler,

                },
                Proportionality = new Proportionality<ITermCategory>(testCrime.GetDefendant)
                {
                    GetChoice = lp =>
                    {
                        if(lp is MichaelTEustler)
                            return new NondeadlyForce();
                        if(lp is Alexander)
                            return new DeadlyForce();
                        return null;
                    }
                }
            };
            testResult = testSubject.IsValid(new Alexander(), new MichaelTEustler());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Alexander : LegalPerson, IDefendant
    {
        public Alexander() : base("JON DOUGLAS ALEXANDER") { }
    }

    public class MichaelTEustler : LegalPerson, IVictim
    {
        public MichaelTEustler() : base("MICHAEL T. EUSTLER") { }
    }
}
