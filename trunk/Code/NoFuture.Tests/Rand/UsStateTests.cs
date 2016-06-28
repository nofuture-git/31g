using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class UsStateTests
    {
        [TestMethod]
        public void GetStateTest()
        {
            var testState = "AZ";
            var testResult = NoFuture.Rand.Gov.UsState.GetStateByPostalCode(testState);
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType(testResult, typeof(NoFuture.Rand.Gov.Arizona));

            Assert.AreEqual(testState,testResult.StateAbbrv);

            var testDlResult = testResult.RandomDriversLicense;
            Assert.IsNotNull(testDlResult);
            System.Diagnostics.Debug.WriteLine(testDlResult);

            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(testDlResult,"^[ABDY][0-9]{8}$"));
            Assert.IsTrue(testResult.ValidDriversLicense("B70350018"));
            Assert.IsTrue(testResult.ValidDriversLicense("686553072"));
            Assert.IsFalse(testResult.ValidDriversLicense("68655307"));

            testState = "AR";
            testResult = NoFuture.Rand.Gov.UsState.GetStateByPostalCode(testState);
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType(testResult, typeof(NoFuture.Rand.Gov.Arkansas));

            Assert.AreEqual(testState, testResult.StateAbbrv);

            testDlResult = testResult.RandomDriversLicense;
            Assert.IsNotNull(testDlResult);
            System.Diagnostics.Debug.WriteLine(testDlResult);

            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(testDlResult, "^[0-9]{9}$"));
            Assert.IsFalse(testResult.ValidDriversLicense("U7376050"));
            Assert.IsTrue(testResult.ValidDriversLicense("686553072"));
            Assert.IsTrue(testResult.ValidDriversLicense("68655307"));

            testState = "CA";
            testResult = NoFuture.Rand.Gov.UsState.GetStateByPostalCode(testState);
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType(testResult, typeof(NoFuture.Rand.Gov.California));

            Assert.AreEqual(testState, testResult.StateAbbrv);

            testDlResult = testResult.RandomDriversLicense;
            Assert.IsNotNull(testDlResult);
            System.Diagnostics.Debug.WriteLine(testDlResult);

            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(testDlResult, "^[A-Z][0-9]{7}$"));
            Assert.IsTrue(testResult.ValidDriversLicense("U7376050"));
            Assert.IsFalse(testResult.ValidDriversLicense("686553072"));

            testState = "CT";
            testResult = NoFuture.Rand.Gov.UsState.GetStateByPostalCode(testState);
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType(testResult, typeof(NoFuture.Rand.Gov.Connecticut));

            Assert.AreEqual(testState, testResult.StateAbbrv);

            testDlResult = testResult.RandomDriversLicense;
            Assert.IsNotNull(testDlResult);
            System.Diagnostics.Debug.WriteLine(testDlResult);

        }

        [TestMethod]
        public void TestGetStateAll()
        {
            var noUse = NoFuture.Rand.Gov.UsState.GetStateByPostalCode("AZ");//the internal member is a singleton, this gets it pop'ed
            var theStates = NoFuture.Rand.Gov.UsState._theStates;

            foreach (var state in theStates)
            {
                System.Diagnostics.Debug.WriteLine(state.StateAbbrv);
                var randOut = state.RandomDriversLicense;
                System.Diagnostics.Debug.WriteLine("{0} '{1}'", state.StateAbbrv, randOut);
                var randIn = state.ValidDriversLicense(randOut);

                Assert.IsTrue(randIn);
            }
        }

        [TestMethod]
        public void TestGetUniversities()
        {
            var testSubject = NoFuture.Rand.Gov.UsState.GetStateByPostalCode("AZ");
            var testResults = testSubject.GetUniversities();

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);
            Assert.IsTrue(testResults.Any(univ => univ.Name == "Arizona State University"));
        }

        [TestMethod]
        public void TestGetHighSchools()
        {
            var testSubject = NoFuture.Rand.Gov.UsState.GetStateByPostalCode("AZ");
            var testResults = testSubject.GetHighSchools();

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);
            
        }

        [TestMethod]
        public void TestGetStateData()
        {
            var testStateName = "Maryland";
            var testState = NoFuture.Rand.Gov.UsState.GetStateByName(testStateName);
            var testResult = testState.GetStateData();
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.AverageEarnings);
            Assert.AreNotEqual(0,testResult.EmploymentSectors.Count);
            Assert.AreNotEqual(0, testResult.PercentOfGrads.Count);
            Assert.AreNotEqual(0, testResult.PropertyCrimeRate.Count);
            Assert.AreNotEqual(0, testResult.ViolentCrimeRate.Count);


        }
    }
}
