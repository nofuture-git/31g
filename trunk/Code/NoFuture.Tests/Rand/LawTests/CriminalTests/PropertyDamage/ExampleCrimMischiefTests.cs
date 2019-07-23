using System;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Damage;
using NoFuture.Rand.Law.Criminal.US.Elements.AttendantCircumstances;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.PropertyDestruction
{
    [TestFixture]
    public class ExampleCrimMischiefTests
    {
        [Test]
        public void TestCriminalMischiefAct()
        {
            var testAct = new CriminalMischief
            {
                Consent = new VictimConsent
                {
                    IsApprovalExpressed = lp => false,
                    IsCapableThereof = lp => lp is SueBrokenstuffEg
                },
                IsCauseOfDamage = lp => lp is JohnnyDestroyerEg,
                IsDamaged = prop => prop?.Name == "sue's stuff",
                SubjectProperty = new LegalProperty("sue's stuff")
                {
                    IsInPossessionOf = lp => lp is SueBrokenstuffEg,
                    IsEntitledTo = lp => lp is SueBrokenstuffEg
                }
            };
            var testResult = testAct.IsValid(new JohnnyDestroyerEg(), new SueBrokenstuffEg());
            Console.WriteLine(testAct.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class JohnnyDestroyerEg : LegalPerson, IDefendant
    {
        public JohnnyDestroyerEg() : base("JOHNNY DESTROYER") { }
    }

    public class SueBrokenstuffEg : LegalPerson, IVictim
    {
        public SueBrokenstuffEg(): base("SUE BROKENSTUFF") {  }
    }
}
