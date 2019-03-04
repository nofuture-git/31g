using System;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.TheftTests
{
    [TestFixture()]
    public class ExampleBurglaryTests
    {
        [Test]
        public void ExampleBurglaryIntentTest()
        {
            var testAct = new Burglary
            {
                //removing a simple barrier and placing hands beyond threshold is sufficient
                IsBreakingForce = lp => lp is JedBurglarToolEg,
                IsEntry = lp => lp is JedBurglarToolEg,
                IsStructuredEnclosure = lp => lp is SomeonesApartment,
                SubjectProperty = new SomeonesApartment()
            };

            var testResult = testAct.IsValid(new JedBurglarToolEg());
            Console.WriteLine(testAct.ToString());
            Assert.IsTrue(testResult);

            testAct = new Burglary
            {
                IsEntry = lp => lp is JedBurglarToolEg,
                IsStructuredEnclosure = lp => lp is SomeonesApartment,
                SubjectProperty = new SomeonesApartment()
            };

            testResult = testAct.IsValid(new JedBurglarToolEg());
            Console.WriteLine(testAct.ToString());
            Assert.IsTrue(testResult);

            testAct = new Burglary
            {
                IsBreakingForce = lp => lp is JedBurglarToolEg,
                IsEntry = lp => lp is JedBurglarToolEg,
                IsStructuredEnclosure = lp => lp is SomeonesApartment,
                SubjectProperty = new SomeonesApartment(),
                Consent = new Consent
                {
                    IsDenialExpressed = lp => true,
                    IsCapableThereof = lp => true,
                }
            };

            testResult = testAct.IsValid(new JedBurglarToolEg());
            Console.WriteLine(testAct.ToString());
            Assert.IsTrue(testResult);

        }

        [Test]
        public void TestBurglaryIntent()
        {
            var testCrime = new Misdemeanor
            {
                ActusReus = new Burglary
                {
                    IsBreakingForce = lp => lp is ChristianShovesEg,
                    IsEntry = lp => lp is ChristianShovesEg,
                    IsStructuredEnclosure = lp => lp is SupposedHauntedHouse
                },
                MensRea = new IsHouseHaunted()
            };
            var testResult = testCrime.IsValid(new ChristianShovesEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class SomeonesApartment : LegalProperty
    {

    }

    public class SupposedHauntedHouse : LegalProperty
    {

    }

    public class IsHouseHaunted : MensRea
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }

        public override int CompareTo(object obj)
        {
            return 1;
        }
    }

    public class JedBurglarToolEg : LegalPerson
    {
        public JedBurglarToolEg() : base("JED BURGLARTOOL") {  }
    }

    public class HansDaresEg : LegalPerson
    {
        public HansDaresEg(): base("HANS DARES") {  }
    }

    public class ChristianShovesEg : LegalPerson
    {
        public ChristianShovesEg() : base("CHRISTIAN SHOVES") {  }
    }
}
