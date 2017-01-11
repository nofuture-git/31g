using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Domus;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class NAmerUtilTests
    {

        [TestMethod]
        public void RandomUsZipWithRespectToPopTest()
        {
            var testResult = NAmerUtil.RandomAmericanZipWithRespectToPop();
            Assert.IsNotNull(testResult);

            for (var i = 0; i < 100; i++)
            {
                testResult = NAmerUtil.RandomAmericanZipWithRespectToPop();
                Assert.IsNotNull(testResult);
                System.Diagnostics.Debug.WriteLine(testResult);
            }
        }
        [TestMethod]
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

        [TestMethod]
        public void TestSolvePercentHsGradByStateAndRace()
        {
            //test resolves to simple natl average given no input
            var testResult = NAmerUtil.SolvePercentGradByStateAndRace(null, null);

            //test resolves to natl avg's when no state
            Assert.AreEqual(82.0, testResult);
            testResult = NAmerUtil.SolvePercentGradByStateAndRace(null, NorthAmericanRace.Mixed);
            Assert.AreEqual(82.0, testResult);
            testResult = NAmerUtil.SolvePercentGradByStateAndRace(null, NorthAmericanRace.AmericanIndian);
            Assert.AreEqual(70.0, testResult);

            var testState = NoFuture.Rand.Gov.UsState.GetStateByPostalCode("CA");
            var testStateData = testState.GetStateData();
            Assert.IsNotNull(testStateData);

            //percent-highschool-grad="80.6"
            //asian="89.0" natl-percent="82.0"
            // 89 - 82 = 7
            testResult = NAmerUtil.SolvePercentGradByStateAndRace(testState, NorthAmericanRace.Asian);
            Assert.AreEqual(87.6, testResult);

            //american-indian="70.0"
            // 70 - 82 = -12
            testResult = NAmerUtil.SolvePercentGradByStateAndRace(testState, NorthAmericanRace.AmericanIndian);
            Assert.AreEqual(68.6, testResult);

            testState = NoFuture.Rand.Gov.UsState.GetStateByPostalCode("ID");
            Assert.IsNotNull(testState);
            Assert.IsNotNull(testState.GetStateData());

            testResult = NAmerUtil.SolvePercentGradByStateAndRace(testState, NorthAmericanRace.Asian, OccidentalEdu.Bachelor | OccidentalEdu.Grad);
            //percent-college-grad="23.9"
            //
            //asian="54.0" natl-percent="30.0"
            // 54.0 - 30.0 = 24, 23.9+24 = 47.9
            //black="16.5"
            Assert.AreEqual(47.9, testResult);
        }
    }
}
