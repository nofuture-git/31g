using System;
using NoFuture.Rand.Law.US.Criminal;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US.Criminal.Inchoate;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.CriminalTests.InchoateTests
{
    [TestFixture]
    public class ExampleAttemptTests
    {
        [Test(Description = "test that having reckless intent is valid for an attempt")]
        public void ExampleRecklessIntent()
        {
            var testCrime = new Misdemeanor()
            {
                MensRea = new Recklessly
                {
                    IsDisregardOfRisk = lp => lp is MelissaEg,
                    IsUnjustifiableRisk = lp => lp is MelissaEg
                },
                ActusReus = new Attempt
                {
                    IsProximity = lp => lp is MelissaEg
                }
            };
            var testResult = testCrime.IsValid(new MelissaEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }

        [Test(Description = "one of four attempt tests must be true to be an attempt")]
        public void ExampleAttemptAllFalse()
        {
            var testCrime = new Misdemeanor()
            {
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is MelissaEg,
                    IsKnowledgeOfWrongdoing = lp => lp is MelissaEg,
                },
                ActusReus = new Attempt()
            };

            var testResult = testCrime.IsValid(new MelissaEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }

        /// <summary>
        /// Example person poisons bait and throws over fence to kill dog, dog not present, takes back poison bait
        /// </summary>
        [Test]
        public void ExampleProximityTest()
        {
            var testCrime = new Misdemeanor()
            {
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is MelissaEg,
                    IsKnowledgeOfWrongdoing = lp => lp is MelissaEg,
                },
                ActusReus = new Attempt
                {
                    IsProximity = lp => lp is MelissaEg
                }
            };

            var testResult = testCrime.IsValid(new MelissaEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleResIpsaLoquiturTest()
        {
            var testCrime = new Felony
            {
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is HarryEg,
                    IsKnowledgeOfWrongdoing = lp => lp is HarryEg,
                }, 
                ActusReus = new Attempt
                {
                    IsResIpsaLoquitur = lp => ((lp as HarryEg)?.IsMeetWithHitman ?? false)
                                               && (((HarryEg)lp).IsPayWithCash2Hitman)
                }
            };

            var testResult = testCrime.IsValid(new HarryEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleProbableDesistanceTest()
        {
            var testCrime = new Felony
            {
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is JudyEg,
                    IsKnowledgeOfWrongdoing = lp => lp is JudyEg,
                },
                ActusReus = new Attempt
                {
                    IsProbableDesistance = JudyEg.IsJudyEgStealing
                }
            };

            var testResult = testCrime.IsValid(new JudyEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleSubstantialStepsTest()
        {
            var testCrime = new Felony
            {
                MensRea = new GeneralIntent
                {
                    IsIntentOnWrongdoing = lp => lp is KevinEg,
                    IsKnowledgeOfWrongdoing = lp => lp is KevinEg,
                },
                ActusReus = new Attempt
                {
                    IsSubstantial = KevinEg.IsKevinRobbingArmoredCar
                }
            };

            var testResult = testCrime.IsValid(new KevinEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class KevinEg : LegalPerson
    {
        public KevinEg() : base("KEVIN ROBBER") { }

        public bool IsCasingBank => true;
        public bool IsWrittenPlan => true;
        public bool IsCarryHiddenWeapon => true;
        public bool IsWaitingInAlley => true;
        public bool IsReachForDoor => true;

        public static bool IsKevinRobbingArmoredCar(ILegalPerson lp)
        {
            var kevin = lp as KevinEg;

            if (kevin == null)
                return false;
            return kevin.IsCasingBank
                   && kevin.IsWrittenPlan
                   && kevin.IsCarryHiddenWeapon
                   && kevin.IsWaitingInAlley
                   && kevin.IsReachForDoor
                ;
        }
    }

    public class MelissaEg : LegalPerson
    {
        public MelissaEg() : base("MELISSA POISON") { }
    }

    public class HarryEg : LegalPerson
    {
        public HarryEg() : base("HARRY HITMAN") { }

        public bool IsMeetWithHitman => true;
        public bool IsPayWithCash2Hitman => true;
    }

    public class JudyEg : LegalPerson
    {
        public JudyEg() : base("") { }

        public bool IsComm2Friends2Steal => true;
        public bool IsEnterBuildingAfterHours => true;
        public bool IsDeactivateSecurityCameras => true;
        public bool IsAttemptingEnterCombo => true;

        public static bool IsJudyEgStealing(ILegalPerson lp)
        {
            var judy = lp as JudyEg;
            if (judy == null)
                return false;
            return judy.IsComm2Friends2Steal
                   && judy.IsEnterBuildingAfterHours
                   && judy.IsDeactivateSecurityCameras
                   && judy.IsAttemptingEnterCombo
                ;
        }
    }
}
