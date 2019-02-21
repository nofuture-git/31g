using System;
using NoFuture.Rand.Law.Criminal.Homicide.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HomicideTests
{
    [TestFixture]
    public class ExampleFirstDegreeTests
    {
        [Test]
        public void ExampleFirstDegree()
        {
            var testCrime = new Felony
            {
                ActusReus = new MurderFirstDegree
                {
                    IsPremediated = JoanniePremedEg.IsPremeditated,
                    IsCorpusDelicti = lp => lp is JoanniePremedEg
                },
                MensRea = new DeadlyWeapon("handgun")
                {
                    IsUtilized = lp => lp is JoanniePremedEg,
                    IsCanCauseDeath = lp => lp is JoanniePremedEg
                }
            };

            var testResult = testCrime.IsValid(new JoanniePremedEg());
            Console.WriteLine(testCrime.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class JoanniePremedEg : LegalPerson
    {
        public JoanniePremedEg() : base("JOANNIE PREMED") { }

        public bool IsCalm { get; set; } = true;
        public bool IsMethodical { get; set; } = true;
        public bool IsManufacturedExcuse { get; set; } = true;
        public bool IsFakingActions { get; set; } = true;
        public bool IsMutlipleShots { get; set; } = true;

        public bool IsDeliberate => IsCalm && IsMethodical;
        public bool IsPlanned => IsManufacturedExcuse && IsFakingActions;
        public bool IsSpecificIntent => IsMutlipleShots;

        public static bool IsPremeditated(ILegalPerson lp)
        {
            var joannie = lp as JoanniePremedEg;
            if (joannie == null)
                return false;
            return joannie.IsDeliberate || joannie.IsPlanned || joannie.IsSpecificIntent;
        }
    }
}
