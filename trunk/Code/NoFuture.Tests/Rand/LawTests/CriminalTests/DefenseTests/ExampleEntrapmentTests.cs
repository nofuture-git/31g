using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense.Excuse;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.DefenseTests
{
    [TestFixture]
    public class ExampleEntrapmentTests
    {
        [Test]
        public void ExampleEntrapment()
        {
            var testCrime = new Felony
            {
                ActusReus = new ActusReus
                {
                    IsAction = lp => lp is WinifredEg,
                    IsVoluntary = lp => lp is WinifredEg,
                },
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is WinifredEg,
                }
            };

            var testResult = testCrime.IsValid(new WinifredEg());
            Assert.IsTrue(testResult);

            var testSubject = new Entrapment(testCrime)
            {
                IsIntentOriginFromLawEnforcement = lp => lp is WinifredEg
            };

            testResult = testSubject.IsValid(new WinifredEg());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class WinifredEg : LegalPerson
    {
        public WinifredEg() : base("WINIFRED NA") { }
    }
}
