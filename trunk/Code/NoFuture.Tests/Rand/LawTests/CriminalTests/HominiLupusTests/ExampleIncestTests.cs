using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HominiLupusTests
{
    [TestFixture]
    public class ExampleIncestTests
    {
        [Test]
        public void ExampleIncest()
        {
            var testCrime = new Felony
            {
                ActusReus = new Incest
                {
                    GetVictim = lps => lps.FirstOrDefault(lp => lp.Name == "HARRIET INCEST"),
                    IsFamilyRelation = (lp1, lp2) => (lp1 is HalIncestEg || lp1 is HarrietIncestEg)
                                                     && (lp2 is HalIncestEg || lp2 is HarrietIncestEg)
                },
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is HalIncestEg || lp is HarrietIncestEg
                }
            };

            var testResult = testCrime.IsValid(new HalIncestEg(), new HarrietIncestEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestIntentIsNull()
        {
            var testCrime = new Felony
            {
                ActusReus = new Incest
                {
                    GetVictim = lps => lps.FirstOrDefault(lp => lp.Name == "HARRIET INCEST"),
                    IsFamilyRelation = (lp1, lp2) => (lp1 is HalIncestEg || lp1 is HarrietIncestEg)
                                                     && (lp2 is HalIncestEg || lp2 is HarrietIncestEg)
                },
                MensRea = null
            };

            var testResult = testCrime.IsValid(new HalIncestEg(), new HarrietIncestEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class HalIncestEg : LegalPerson
    {
        public HalIncestEg() : base("HAL INCEST") { }
    }

    public class HarrietIncestEg : LegalPerson
    {
        public HarrietIncestEg() : base("HARRIET INCEST") { }
    }
}
