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

namespace NoFuture.Law.Criminal.Tests.DefenseTests.DefenseOfOtherTests
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

            var testSubject = new DefenseOfProperty
            {
                Imminence = new Imminence(ExtensionMethods.Defendant)
                {
                    GetResponseTime = lp => Imminence.NormalReactionTimeToDanger
                },
                Provocation = new Provocation(ExtensionMethods.Defendant)
                {
                    IsInitiatorOfAttack = lp => lp is MichaelTEustler,

                },
                Proportionality = new Proportionality<ITermCategory>(ExtensionMethods.Defendant)
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
