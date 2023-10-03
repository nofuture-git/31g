using System;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.AgainstGov;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.AgainstGovTests
{
    [TestFixture]
    public class ExampleBiberyTests
    {
        [Test]
        public void TestBibery()
        {
            var testCrime = new Felony
            {
                ActusReus = new Bribery
                {
                    IsKnowinglyProcured = lp => lp is IsabelBriberEg,
                    IsPublicOfficial = lp => lp is IsabelBriberEg
                },
                MensRea = new SpecificIntent
                {
                    IsIntentOnWrongdoing = lp => true
                }
            };

            var testResult = testCrime.IsValid(new IsabelBriberEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class IsabelBriberEg : LegalPerson, IDefendant
    {
        public IsabelBriberEg(): base("ISABEL BRIBER") { }
    }
}
