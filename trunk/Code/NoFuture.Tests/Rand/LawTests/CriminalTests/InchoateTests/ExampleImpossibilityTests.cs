using System;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Defense;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.InchoateTests
{
    [TestFixture]
    public class ExampleImpossibilityTests
    {
        [Test(Description = "Impossibility is only applicable to Attempt")]
        public void TestOnlyForAttempt()
        {
            var testCrime = new Misdemeanor
            {
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is MelissaEg2,
                    IsIntentOnWrongdoing = lp => lp is MelissaEg2
                },
                ActusReus = new ActusReus
                {
                    IsVoluntary = lp => lp is MelissaEg2,
                    IsAction = lp => lp is MelissaEg2
                }
            };

            var testResult = testCrime.IsValid(new MelissaEg2());
            Assert.IsTrue(testResult);

            var testSubject = new Impossibility(testCrime)
            {
                IsLegalImpossibility = MelissaEg2.IsLegal
            };

            var melissa = new MelissaEg2 { ItActuallyIsRatThatsBarking = true };
            testResult = testSubject.IsValid(melissa);
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }

        [Test(Description = "Melissa trys to poison neighbors dog for " +
                            "barking but it was actually a giant rat that " +
                            "was barking. Killing the rat is not illegal.")]
        public void LegalImpossibilityTest()
        {
            var testCrime = new Misdemeanor
            {
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is MelissaEg2,
                    IsIntentOnWrongdoing = lp => lp is MelissaEg2
                },
                ActusReus = new Attempt
                {
                    SubstantialSteps = new SubstantialSteps
                    {
                        IsPossessCriminalMaterial = lp => lp is MelissaEg2,
                    }
                }
            };

            var testResult = testCrime.IsValid(new MelissaEg2());
            Assert.IsTrue(testResult);

            var testSubject = new Impossibility(testCrime)
            {
                IsLegalImpossibility = MelissaEg2.IsLegal
            };

            var melissa = new MelissaEg2 {ItActuallyIsRatThatsBarking = true};
            testResult = testSubject.IsValid(melissa);
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);
        }

        [Test(Description = "Melissa trys to poison neighbors dog " +
                            "for barking but its not in the yard at the time.")]
        public void FactualImpossibilityTest()
        {
            var testCrime = new Misdemeanor
            {
                MensRea = new GeneralIntent
                {
                    IsKnowledgeOfWrongdoing = lp => lp is MelissaEg2,
                    IsIntentOnWrongdoing = lp => lp is MelissaEg2
                },
                ActusReus = new Attempt
                {
                    SubstantialSteps = new SubstantialSteps
                    {
                        IsInvestigatingPotentialScene = lp => lp is MelissaEg2,
                        IsPossessCriminalMaterial = lp => lp is MelissaEg2
                    }
                }
            };

            var testResult = testCrime.IsValid(new MelissaEg2());
            Assert.IsTrue(testResult);

            var testSubject = new Impossibility(testCrime)
            {
                IsFactualImpossibility = MelissaEg2.IsFactual
            };
            
            testResult = testSubject.IsValid(new MelissaEg2());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class MelissaEg2 : MelissaEg
    {
        public bool ItActuallyIsRatThatsBarking { get; set; }
        public bool IsRatAPest { get; set; } = true;
        public bool IsDogPresent { get; set; }

        public static bool IsLegal(ILegalPerson lp)
        {
            var melissa = lp as MelissaEg2;
            if (melissa == null)
                return false;
            return melissa.ItActuallyIsRatThatsBarking && melissa.IsRatAPest;
        }

        public static bool IsFactual(ILegalPerson lp)
        {
            var melissa = lp as MelissaEg2;
            if (melissa == null)
                return false;
            return !melissa.IsDogPresent;
        }
    }
}
