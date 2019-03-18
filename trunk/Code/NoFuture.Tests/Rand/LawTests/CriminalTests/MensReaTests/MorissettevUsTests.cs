using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.MensReaTests
{
    /// <summary>
    /// 342 U.S. 246 (1952) MORISSETTE v. UNITED STATES. No. 12. Supreme Court of United States. Argued October 9-10, 1951. Decided January 7, 1952.
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, in depth look at what kinds of crimes which don't require intent
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class MorissettevUsTests
    {
        [Test]
        public void MorrisettevUs()
        {
            var testSubject = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is Morrisette,
                    IsVoluntary = lp => lp is Morrisette
                }
            };

            //this is what the prosectution asserted, that intent was not required
            testSubject.MensRea = null;

            var testResult = testSubject.IsValid(new Morrisette());
            Assert.IsTrue(testResult);

            //the court ruled that intent was required
            testSubject.MensRea = new GeneralIntent();
            testResult = testSubject.IsValid(new Morrisette());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Morrisette : LegalPerson, IDefendant
    {
        public Morrisette(): base ("MORISSETTE")  { }
    }
}
