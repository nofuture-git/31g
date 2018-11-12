using System.Diagnostics;
using NUnit.Framework;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Tests.GeoTests
{
    [TestFixture]
    public class UsCityStateZipTests
    {
        [Test]
        public void RandomUsZipWithRespectToPopTest()
        {
            var testResult = UsCityStateZip.RandomAmericanPartialZipCode();
            Assert.IsNotNull(testResult);

            for (var i = 0; i < 100; i++)
            {
                testResult = UsCityStateZip.RandomAmericanPartialZipCode();
                Assert.IsNotNull(testResult);
                System.Diagnostics.Debug.WriteLine(testResult);
            }
        }
        [Test]
        public void AmericanRaceRatioByZipCodeTests()
        {
            const string TEST_ZIP = "92071";
            var testResult = UsCityStateZip.RandomAmericanRaceWithRespectToZip(TEST_ZIP);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);

            var testZipPrefix = new string[]
            {
                "945","089","945","330","785","970","974","402",
                "920","799","761","023","750","021","840","782",
                "605","925","549","801","731","503","806","378",
                "770","173","076","440","754","917","460","926",
                "317","921","016","437","299","900","357","114",
                "070","028","953","600","112","913","923","496",
                "731","785","882","601","307","850","631","223",
                "208","275","112","967","356","600","829","761",
                "322","481","463","301","902","377","776",
                "372","073","802","752","360","454","735","119",
                "127","206","104","341","481","331","925","970",
                "435","321","851","600","719","781","336","349",
                "800","802","744","481"
            };

            foreach (var t in testZipPrefix)
            {
                System.Diagnostics.Debug.WriteLine(t);
                testResult = UsCityStateZip.RandomAmericanRaceWithRespectToZip(t);
                Assert.IsNotNull(testResult);
                System.Diagnostics.Debug.WriteLine(testResult);
            }
        }


        [Test]
        public void AmericanRaceTests()
        {
            const string TEST_ZIP = "92071";
            var testResult = UsCityStateZip.GetAmericanRace(TEST_ZIP);
            Assert.AreNotEqual(string.Empty, testResult);
            Debug.WriteLine(testResult);

            testResult = UsCityStateZip.GetAmericanRace("68415");
            Assert.AreEqual(NorthAmericanRace.White, testResult);
        }
    }
}
