using System;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
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
                Consent = new Consent
                {
                    IsApprovalExpressed = lp => false,
                    IsCapableThereof = lp => lp is SueBrokenstuffEg
                },
                IsCauseOfDamage = lp => lp is JohnnyDestroyerEg,
                IsDamaged = prop => prop?.Name == "sue's stuff",
                SubjectProperty = new LegalProperty("sue's stuff")
                {
                    InPossessionOf = new SueBrokenstuffEg(),
                    EntitledTo = new SueBrokenstuffEg()
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

    public class SueBrokenstuffEg : Victim
    {
        public SueBrokenstuffEg(): base("SUE BROKENSTUFF") {  }
    }
}
