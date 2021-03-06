﻿using System;
using NUnit.Framework;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Edu.US;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;

namespace NoFuture.Rand.Tests.EduTests
{
    [TestFixture]
    public class AmericanSchoolBaseTests
    {

        [Test]
        public void TestSolvePercentHsGradByStateAndRace()
        {
            //test resolves to simple natl average given no input
            var testResult = AmericanSchoolBase.SolvePercentGradByStateAndRace(null, null);

            //test resolves to natl avg's when no state
            Assert.AreEqual(89.6, testResult);
            testResult = AmericanSchoolBase.SolvePercentGradByStateAndRace(null, NorthAmericanRace.Mixed);
            Assert.AreEqual(89.6, testResult);
            testResult = AmericanSchoolBase.SolvePercentGradByStateAndRace(null, NorthAmericanRace.AmericanIndian);
            Assert.AreEqual(77.6, testResult);
            var testState = UsState.GetStateByPostalCode("CA");
            var testStateData = UsStateData.GetStateData(testState.ToString());
            Assert.IsNotNull(testStateData);

            //percent-highschool-grad="80.6"
            //asian="89.0" natl-percent="82.0"
            // 89 - 82 = 7
            testResult = AmericanSchoolBase.SolvePercentGradByStateAndRace(testState.ToString(), NorthAmericanRace.Asian);
            Assert.AreEqual(87.6, testResult);

            //american-indian="70.0"
            // 70 - 82 = -12
            testResult = AmericanSchoolBase.SolvePercentGradByStateAndRace(testState.ToString(), NorthAmericanRace.AmericanIndian);
            Assert.AreEqual(68.6, testResult);

            testState = UsState.GetStateByPostalCode("ID");
            Assert.IsNotNull(testState);
            Assert.IsNotNull(UsStateData.GetStateData(testState.ToString()));

            testResult = AmericanSchoolBase.SolvePercentGradByStateAndRace(testState.ToString(), NorthAmericanRace.Asian, OccidentalEdu.Bachelor | OccidentalEdu.Grad);
            //percent-college-grad="23.9"
            //
            //asian="54.0" natl-percent="30.0"
            // 54.0 - 30.0 = 24, 23.9+24 = 47.9
            Assert.AreEqual(47.9, testResult);
        }
    }
}
