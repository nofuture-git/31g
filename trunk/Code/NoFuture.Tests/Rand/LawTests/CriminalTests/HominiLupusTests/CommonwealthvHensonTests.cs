using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.AgainstPersons;
using NoFuture.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.HominiLupusTests
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
                    IsByThreatOfViolence = lp => lp is Henson,
                    Imminence = new Imminence { IsImmediatePresent = ts => true}
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

    public class Henson : LegalPerson, IDefendant
    {
        public Henson() : base("ALBERT J. HENSON") { }
    }
}
