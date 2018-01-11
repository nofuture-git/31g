using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Tests.EduTests
{
    [TestClass]
    public class AmericanEduBaseTests
    {

        [TestMethod]
        public void TestSolvePercentHsGradByStateAndRace()
        {
            //test resolves to simple natl average given no input
            var testResult = AmericanEduBase.SolvePercentGradByStateAndRace(null, null);

            //test resolves to natl avg's when no state
            Assert.AreEqual(82.0, testResult);
            testResult = AmericanEduBase.SolvePercentGradByStateAndRace(null, NorthAmericanRace.Mixed);
            Assert.AreEqual(82.0, testResult);
            testResult = AmericanEduBase.SolvePercentGradByStateAndRace(null, NorthAmericanRace.AmericanIndian);
            Assert.AreEqual(70.0, testResult);

            var testState = NoFuture.Rand.Gov.UsState.GetStateByPostalCode("CA");
            var testStateData = UsStateData.GetStateData(testState.ToString());
            Assert.IsNotNull(testStateData);

            //percent-highschool-grad="80.6"
            //asian="89.0" natl-percent="82.0"
            // 89 - 82 = 7
            testResult = AmericanEduBase.SolvePercentGradByStateAndRace(testState, NorthAmericanRace.Asian);
            Assert.AreEqual(87.6, testResult);

            //american-indian="70.0"
            // 70 - 82 = -12
            testResult = AmericanEduBase.SolvePercentGradByStateAndRace(testState, NorthAmericanRace.AmericanIndian);
            Assert.AreEqual(68.6, testResult);

            testState = NoFuture.Rand.Gov.UsState.GetStateByPostalCode("ID");
            Assert.IsNotNull(testState);
            Assert.IsNotNull(UsStateData.GetStateData(testState.ToString()));

            testResult = AmericanEduBase.SolvePercentGradByStateAndRace(testState, NorthAmericanRace.Asian, OccidentalEdu.Bachelor | OccidentalEdu.Grad);
            //percent-college-grad="23.9"
            //
            //asian="54.0" natl-percent="30.0"
            // 54.0 - 30.0 = 24, 23.9+24 = 47.9
            Assert.AreEqual(47.9, testResult);
        }
    }
}
