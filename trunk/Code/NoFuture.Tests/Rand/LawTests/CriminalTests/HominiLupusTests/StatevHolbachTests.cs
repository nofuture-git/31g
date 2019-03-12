using System;
using NoFuture.Rand.Law.Criminal.HominiLupus.US;
using NoFuture.Rand.Law.Criminal.HominiLupus.US.Credible;
using NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HominiLupusTests
{
    /// <summary>
    /// State v. Holbach, 2009 ND 37 (2009)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the agitation must induce a real fear
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class StatevHolbachTests
    {
        [Test]
        public void StatevHolbach()
        {
            var testCrime = new Felony
            {
                ActusReus = new Stalking
                {
                    IsApparentAbility = lp => lp is Holbach,
                    Occasions = new IAgitate[]
                    {
                        new ThreateningWords
                        {
                            IsCauseToFearSafety = lp => lp is Holbach,
                        }, 
                        new ThreateningWords
                        {
                            IsCauseToFearSafety = lp => lp is Holbach
                        }
                    }
                },
                MensRea = new Purposely
                {
                    IsKnowledgeOfWrongdoing = lp => lp is Holbach
                }
            };

            var testResult = testCrime.IsValid(new Holbach());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Holbach : LegalPerson, IDefendant
    {
        public Holbach() : base("SCOTT THOMAS BOYLE") { }
    }
}
