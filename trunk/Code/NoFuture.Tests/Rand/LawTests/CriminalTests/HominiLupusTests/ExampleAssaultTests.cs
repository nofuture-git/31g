using System;
using NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HominiLupusTests
{
    [TestFixture]
    public class ExampleAssaultTests
    {
        [Test]
        public void ExampleAssault()
        {
            var testCrime = new Felony
            {
                ActusReus = new Assault
                {
                    IsProbableDesistance = lp => ((lp as DianaPistolEg)?.IsAimPistol ?? false)
                                                 && ((DianaPistolEg) lp).IsPullTrigger,
                    IsPresentAbility = lp => (lp as DianaPistolEg)?.IsPistolLoaded ?? false
                },
                MensRea = new DeadlyWeapon("loaded pistol")
            };

            var testResult = testCrime.IsValid(new DianaPistolEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestNegligentIntent()
        {
            var testCrime = new Felony
            {
                ActusReus = new Assault
                {
                    IsProbableDesistance = lp => ((lp as DianaPistolEg)?.IsAimPistol ?? false)
                                                 && ((DianaPistolEg)lp).IsPullTrigger,
                    IsPresentAbility = lp => (lp as DianaPistolEg)?.IsPistolLoaded ?? false
                },
                MensRea = new DeadlyWeapon("loaded pistol", new Negligently())
            };

            var testResult = testCrime.IsValid(new DianaPistolEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class DianaPistolEg : LegalPerson
    {
        public DianaPistolEg() : base("DIANA PISTOL") {}
        public bool IsAimPistol { get; set; } = true;
        public bool IsPullTrigger { get; set; } = true;
        public bool IsPistolLoaded { get; set; } = true;
    }
}
