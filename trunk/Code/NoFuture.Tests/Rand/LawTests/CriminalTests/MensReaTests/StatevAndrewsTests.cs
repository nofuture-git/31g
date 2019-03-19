using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.MensReaTests
{
    /// <summary>
    /// State v. Andrews, 572 S.E.2d 798 (2002)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, transferred intent is anyone else hurt by the intent and suffices as the intent element of the crime charged
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class StatevAndrewsTests
    {
        [Test]
        public void StatevAndrews()
        {
            var testIntent = new Knowingly
            {
                IsKnowledgeOfWrongdoing = lp => lp is Andrews,
                IsIntentOnWrongdoing = lp => lp is Andrews
            };

            var testSubject = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is Andrews,
                    IsAction = lp => lp is Andrews
                },
                MensRea = new Transferred(testIntent)
            };

            var testResult = testSubject.IsValid(new Andrews());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class Andrews : LegalPerson, IDefendant
    {

    }
}
