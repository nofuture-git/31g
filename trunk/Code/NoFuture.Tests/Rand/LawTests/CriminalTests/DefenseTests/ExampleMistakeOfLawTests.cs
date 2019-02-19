using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense.Excuse;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.DefenseTests
{
    [TestFixture]
    public class ExampleMistakeOfLawTests
    {
        [Test]
        public void ExampleMistakeOfLaw()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is ShelbyEg,
                    IsVoluntary = lp => lp is ShelbyEg,
                },
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is ShelbyEg,
                }
            };

            var testResult = testCrime.IsValid(new ShelbyEg());
            Assert.IsTrue(testResult);

            var testSubject = new MistakeOfLaw(testCrime)
            {
                IsRelianceOnStatementOfLaw = lp => lp is ShelbyEg,
                IsStatementOfLawNowInvalid = lp => lp is ShelbyEg
            };

            testResult = testSubject.IsValid(new ShelbyEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class ShelbyEg : LegalPerson
    {
        public ShelbyEg() : base("SHELBY ATTORNEY") { }
    }
}
