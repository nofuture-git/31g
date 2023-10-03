using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.Act;
using NoFuture.Law.Criminal.US.Elements.Intent;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Criminal.Tests.MensReaTests
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

        public class JimEg : LegalPerson, IDefendant
        {
            public JimEg() : base("JIM EVADER") { }
        }
    }
}
