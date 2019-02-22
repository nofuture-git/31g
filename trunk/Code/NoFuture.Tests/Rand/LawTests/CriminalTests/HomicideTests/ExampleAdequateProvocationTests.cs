using System;
using NoFuture.Rand.Law.Criminal.Homicide.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HomicideTests
{
    [TestFixture]
    public class ExampleAdequateProvocationTests
    {
        [Test]
        public void ExampleIndequateProvocation()
        {
            var testCrime = new Felony
            {
                ActusReus = new ManslaughterVoluntary
                {
                    IsCorpusDelicti = DillonRager.IsKilledFrank
                },
                MensRea = new AdequateProvocation
                {
                    //getting fired is not reason enough
                    IsReasonableToInciteKilling = lp => false,
                    IsDefendantActuallyProvoked = lp => lp is DillonRager
                }
            };

            var testResult = testCrime.IsValid(new DillonRager());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }

        [Test]
        public void ExampleAdequateProvation()
        {
            var testCrime = new Felony
            {
                ActusReus = new ManslaughterVoluntary
                {
                    IsCorpusDelicti = JoseRager.IsKilledWife
                },
                MensRea = new AdequateProvocation
                {
                    IsReasonableToInciteKilling = lp => lp is JoseRager,
                    IsDefendantActuallyProvoked = lp => lp is JoseRager
                }
            };

            var testResult = testCrime.IsValid(new JoseRager());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void DoesntWorkWithOtherIntent()
        {
            var testCrime = new Felony
            {
                ActusReus = new ManslaughterVoluntary
                {
                    IsCorpusDelicti = JoseRager.IsKilledWife
                },
                MensRea = new SpecificIntent
                {
                    IsIntentOnWrongdoing = lp => lp is JoseRager
                }
            };

            var testResult = testCrime.IsValid(new JoseRager());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }

        [Test]
        public void TestBadTiming()
        {
            var yyyy = DateTime.Today.Year;
            var testCrime = new Felony
            {
                ActusReus = new ManslaughterVoluntary
                {
                    IsCorpusDelicti = JoseRager.IsKilledWife
                },
                MensRea = new AdequateProvocation
                {
                    IsReasonableToInciteKilling = lp => lp is JoseRager,
                    IsDefendantActuallyProvoked = lp => lp is JoseRager,
                    Inception = new DateTime(yyyy, 3, 15, 14,0,0),
                    Terminus = new DateTime(yyyy, 3,15, 14,5,0),
                    //heat of passion doesn't work over time
                    TimeOfTheDeath = new DateTime(yyyy, 3, 16, 6, 0, 0),
                }
            };

            var testResult = testCrime.IsValid(new JoseRager());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class DillonRager : LegalPerson
    {
        public DillonRager() : base("DILLON RAGER") { }

        public static bool IsKilledFrank(ILegalPerson lp)
        {
            return lp is DillonRager;
        }
    }

    public class JoseRager : LegalPerson
    {
        public JoseRager() : base("JOSE RAGER") { }

        public static bool IsKilledWife(ILegalPerson lp)
        {
            return lp is JoseRager;
        }
    }
}
