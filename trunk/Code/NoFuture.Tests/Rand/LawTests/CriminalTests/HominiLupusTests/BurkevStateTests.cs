using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.AgainstPersons;
using NoFuture.Law.Criminal.US.Elements.AgainstPersons.Credible;
using NoFuture.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.HominiLupusTests
{
    /// <summary>
    /// Burke v. State, 676 S.E.2d 766 (2009).
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, stalking requires two or more violations 
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class BurkevStateTests
    {
        [Test]
        public void BurkevState()
        {
            var testCrime = new Felony
            {
                ActusReus = new Stalking
                {
                    IsApparentAbility = lp => lp is Burke,
                    Occasions = new IAgitate[]
                    {
                        new Harass
                        {
                            IsSubstantialEmotionalDistress = lp => lp is Burke
                        },
                        //this is not illegal
                        new DeclareLove
                        {
                            IsCauseToFearSafety = lp => lp is Burke
                        }
                    }
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is Burke
                }
            };

            var testResult = testCrime.IsValid(new Burke());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Burke : LegalPerson, IDefendant
    {
        public Burke() : base("SEAN M. BURKE") { }
    }
}
