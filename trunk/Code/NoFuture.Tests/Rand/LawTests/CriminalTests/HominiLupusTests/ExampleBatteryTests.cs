using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HominiLupusTests
{
    [TestFixture]
    public class ExampleBatteryTests
    {
        [Test]
        public void ExampleBattery()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new Battery
                {
                    IsPhysicalContact = lp => lp is HarrietIncestEg
                },
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is HarrietIncestEg
                }
            };

            var testResult = testCrime.IsValid(new HarrietIncestEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleMutualCombat()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new Battery
                {
                    IsPhysicalContact = lp => lp is HarrietIncestEg,
                    Consent = new Consent
                    {
                        GetVictim = lps => lps.FirstOrDefault(lp => lp.Name == new HalIncestEg().Name),
                        IsCapableThereof = lp => true,
                        IsFirmDenial = lp => false,
                    }
                },
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is HarrietIncestEg
                },
            };
            var testResult = testCrime.IsValid(new HarrietIncestEg(), new HalIncestEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }
}
