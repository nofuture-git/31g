using System;
using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Defense.Excuse;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests.InsanityTests
{
    /// <summary>
    /// State v. Hornsby, 484 S.E.2d 869 (1997)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, guilty-but-mentally-ill is a half-baked insanity where the only part that is 
    /// true is the mental defect part
    /// ]]>
    /// </remarks>
    [TestFixture()]
    public class StatevHornsbyTests
    {
        [Test]
        public void StatevHornsby()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is Hornsby,
                    IsVoluntary = lp => lp is Hornsby,
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is Hornsby,
                    IsIntentOnWrongdoing = lp => lp is Hornsby,
                }
            };

            var testResult = testCrime.IsValid(new Hornsby());
            Assert.IsTrue(testResult);

            var testSubject = new MNaghten(testCrime)
            {
                IsMentalDefect = lp => lp is Hornsby,
            };
            testResult = testSubject.IsValid(new Hornsby());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Hornsby : LegalPerson
    {
        public Hornsby() : base("BRENT HORNSBY") { }
    }
}
