using System;
using NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HominiLupusTests
{
    /// <summary>
    /// Commonwealth v. Henson, 259 N.E.2d 769 (1970)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, an threatened battery is valid when it very much appears the threat it real
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class CommonwealthvHensonTests
    {
        [Test]
        public void CommonwealthvHenson()
        {
            var testCrime = new Felony
            {
                ActusReus = new ThreatenedBattery
                {
                    //the fake gun fired only blanks so it was no real threat
                    IsPresentAbility = lp => false,
                    //it looked and felt real to the victim
                    IsApparentAbility = lp => lp is Henson,
                    IsByThreatOfForce = lp => lp is Henson
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is Henson
                }
            };

            var testResult = testCrime.IsValid(new Henson());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Henson : LegalPerson
    {
        public Henson() : base("ALBERT J. HENSON") { }
    }
}
