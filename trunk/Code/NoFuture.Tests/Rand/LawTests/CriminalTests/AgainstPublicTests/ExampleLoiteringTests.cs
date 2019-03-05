using System;
using NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.AgainstPublicTests
{
    [TestFixture]
    public class ExampleLoiteringTests
    {
        [Test]
        public void TestLoiteringIsValid()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new Loitering
                {
                    IsBegging = lp => lp is SomeBumEg,
                },
                MensRea = new Purposely
                {
                    IsIntentOnWrongdoing = lp => lp is SomeBumEg
                }
            };

            var testResult = testCrime.IsValid(new SomeBumEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class SomeBumEg : LegalPerson
    {
        public SomeBumEg() : base("ARRRAGHH! GRINPNKLOOP...") { }
    }
}
