using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;
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

            System.Diagnostics.Debug.WriteLine(testResult);
        }
        [TestMethod]
        public void AmericanRaceRatioByZipCodeTests()
        {
            const string TEST_ZIP = "92071";
            var testResult = NAmerUtil.RandomAmericanRaceWithRespectToZip(TEST_ZIP);
            Assert.IsNotNull(testResult);
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

            testResult = NAmerUtil.SolvePercentGradByStateAndRace(testState, NorthAmericanRace.Asian, OccidentalEdu.College);
            //percent-college-grad="23.9"
            //
            //asian="54.0" natl-percent="30.0"
            // 54.0 - 30.0 = 24, 23.9+24 = 47.9
            //black="16.5"
            Assert.AreEqual(47.9, testResult);
        }
    }
}
