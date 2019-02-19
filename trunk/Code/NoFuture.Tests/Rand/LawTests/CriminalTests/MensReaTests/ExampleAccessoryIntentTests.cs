using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.MensReaTests
{
    [TestFixture]
    public class ExampleAccessoryIntentTests
    {
        [Test]
        public void ExampleAccessoryTests()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is JimEg,
                    IsAction = lp => lp is JimEg
                },
                MensRea = new Accessory
                {
                    IsAwareOfCrime = lp => lp is JimEg,
                    IsAssistToEvadeProsecution = lp => lp is JimEg
                }
            };

            var testResult = testCrime.IsValid(new JimEg());
            Assert.IsTrue(testResult);
        }

        public class JimEg : LegalPerson
        {
            public JimEg() : base("JIM EVADER") { }
        }
    }
}
