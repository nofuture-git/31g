using System;
using NoFuture.Rand.Law.Criminal.Homicide.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HomicideTests
{
    [TestFixture]
    public class ExampleSecondDegreeTests
    {
        [Test]
        public void ExampleSecondDegree()
        {
            var testCrime = new Felony
            {
                ActusReus = new MurderSecondDegree
                {
                    IsCorpusDelicti = lp => lp is DougCrackheadEg,
                    IsExtremeIndifferenceToHumanLife = lp => DougCrackheadEg.IsSecondDegree(lp)
                },
                MensRea = new Recklessly
                {
                    IsUnjustifiableRisk = lp => lp is DougCrackheadEg,
                    IsDisregardOfRisk = lp => lp is DougCrackheadEg
                }
            };

            var testResult = testCrime.IsValid(new DougCrackheadEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);

        }
    }

    public class DougCrackheadEg : LegalPerson, IDefendant
    {
        public DougCrackheadEg() : base("DOUG CRACKHEAD") { }

        public bool IsDrunkDriving { get; set; } = true;
        public bool IsHighOnCrack { get; set; } = true;
        public bool IsTextDriving { get; set; } = true;
        public bool IsLeaveScene { get; set; } = true;

        //typically this would mean manslaughter
        public bool IsInvolvedInAutoAccident { get; set; } = true;

        public static bool IsSecondDegree(ILegalPerson lp)
        {
            var doug = lp as DougCrackheadEg;
            if (doug == null)
                return false;

            return doug.IsInvolvedInAutoAccident
                   && doug.IsDrunkDriving
                   && doug.IsHighOnCrack
                   && doug.IsTextDriving
                   && doug.IsLeaveScene
                ;
        }
    }
}
