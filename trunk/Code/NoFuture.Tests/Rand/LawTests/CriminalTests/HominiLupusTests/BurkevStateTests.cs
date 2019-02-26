using System;
using NoFuture.Rand.Law.Criminal.HominiLupus.US;
using NoFuture.Rand.Law.Criminal.HominiLupus.US.Credible;
using NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HominiLupusTests
{
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

    public class Burke : LegalPerson
    {
        public Burke() : base("SEAN M. BURKE") { }
    }
}
