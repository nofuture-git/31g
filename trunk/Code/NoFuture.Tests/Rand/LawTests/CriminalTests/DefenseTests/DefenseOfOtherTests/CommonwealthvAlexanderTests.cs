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
                },
                OtherParties = () => new []{new MichaelTEustler(), }
            };
            var testResult = testCrime.IsValid(new Alexander());
            Assert.IsTrue(testResult);

            var testSubject = new DefenseOfProperty(testCrime)
            {
                Imminence = new Imminence(testCrime)
                {
                    GetResponseTime = lp => Imminence.NormalReactionTimeToDanger
                },
                Provacation = new Provacation(testCrime)
                {
                    IsInitiatorOfAttack = lp => lp is MichaelTEustler,

                },
                Proportionality = new Proportionality<ITermCategory>(testCrime)
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
            testResult = testSubject.IsValid(new Alexander());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Alexander : LegalPerson
    {
        public Alexander() : base("JON DOUGLAS ALEXANDER") { }
    }

    public class MichaelTEustler : LegalPerson
    {
        public MichaelTEustler() : base("MICHAEL T. EUSTLER") { }
    }
}
