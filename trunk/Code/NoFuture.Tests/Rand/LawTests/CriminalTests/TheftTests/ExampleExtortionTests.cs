using System;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.TheftTests
{
    [TestFixture]
    public class ExampleExtortionTests
    {
        [Test]
        public void ExampleExtortionAct()
        {
            var testAct = new ByExtortion
            {
                IsTakenPossession = lp => lp is RodneyBlackmailEg,
                Threatening = new ByExtortion.ByThreatening
                {
                    IsToAccuseOfCrime = lp => lp is RodneyBlackmailEg
                },
                SubjectProperty = new LegalProperty("fifteen thousand dollars"){ PropretyValue = 15000m } ,
            };

            var testResult = testAct.IsValid(new RodneyBlackmailEg(), new LindseyDealinEg());
            Console.WriteLine(testAct.ToString());
            Assert.IsTrue(testResult);
        }

        [Test]
        public void ExampleThreatenHonestlyDue()
        {
            var trent = new TrentThreatenEg();
            var thousandDollars = new LegalProperty("thousand dollars") {EntitledTo = trent, PropretyValue = 10000m };
            var testAct = new ByExtortion
            {
                IsTakenPossession = lp => lp is TrentThreatenEg,
                Threatening = new ByExtortion.ByThreatening
                {
                    IsToExposeHurtfulSecret = lp => lp is TrentThreatenEg
                },
                SubjectProperty = thousandDollars,
                
            };

            var testResult = testAct.IsValid(new TrentThreatenEg(), new TaraLyingEg());
            Console.WriteLine(testAct.ToString());
            Assert.IsFalse(testResult);
        }

        [Test]
        public void ExampleCaveBasedOnThreat()
        {
            var testAct = new ByExtortion
            {
                IsTakenPossession = lp => lp is RodneyBlackmailEg,
                Threatening = new ByExtortion.ByThreatening
                {
                    IsToAccuseOfCrime = lp => lp is RodneyBlackmailEg
                },
                SubjectProperty = new LegalProperty("fifteen thousand dollars"){ PropretyValue = 15000m },
                Consent = new Consent
                {
                    IsApprovalExpressed = lp => true,
                    IsCapableThereof = lp => lp is LindseyDealinEg
                }
            };

            var testResult = testAct.IsValid(new RodneyBlackmailEg(), new LindseyDealinEg());
            Console.WriteLine(testAct.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class RodneyBlackmailEg : LegalPerson
    {
        public RodneyBlackmailEg() : base("RODNEY BLACKMAIL") { }
    }

    public class LindseyDealinEg : Victim
    {
        public LindseyDealinEg() : base("LINDSEY DEALIN") { }
    }

    public class TrentThreatenEg : LegalPerson
    {
        public TrentThreatenEg() :base("TRENT THREATEN") {  }
    }

    public class TaraLyingEg : Victim
    {
        public TaraLyingEg() : base("TARA LYING") { }
    }
}
