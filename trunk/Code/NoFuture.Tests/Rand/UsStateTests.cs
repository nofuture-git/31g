using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Gov.TheStates;

namespace NoFuture.Rand.Tests
{
    [TestClass]
    public class UsStateTests
    {
        [TestMethod]
        public void GetStateTest()
        {
            var testState = "AZ";
            var testResult = Gov.UsState.GetStateByPostalCode(testState);
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType(testResult, typeof(Arizona));

            Assert.AreEqual(testState,testResult.StateAbbrv);

            var testDlResult = testResult.RandomDriversLicense;
            Assert.IsNotNull(testDlResult);
            System.Diagnostics.Debug.WriteLine(testDlResult);

            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(testDlResult,"^[ABDY][0-9]{8}$"));
            Assert.IsTrue(testResult.ValidDriversLicense("B70350018"));
            Assert.IsTrue(testResult.ValidDriversLicense("686553072"));
            Assert.IsFalse(testResult.ValidDriversLicense("68655307"));

            testState = "AR";
            testResult = Gov.UsState.GetStateByPostalCode(testState);
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType(testResult, typeof(Arkansas));

            Assert.AreEqual(testState, testResult.StateAbbrv);

            testDlResult = testResult.RandomDriversLicense;
            Assert.IsNotNull(testDlResult);
            System.Diagnostics.Debug.WriteLine(testDlResult);

            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(testDlResult, "^[0-9]{9}$"));
            Assert.IsFalse(testResult.ValidDriversLicense("U7376050"));
            Assert.IsTrue(testResult.ValidDriversLicense("686553072"));
            Assert.IsTrue(testResult.ValidDriversLicense("68655307"));

            testState = "CA";
            testResult = Gov.UsState.GetStateByPostalCode(testState);
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType(testResult, typeof(California));

            Assert.AreEqual(testState, testResult.StateAbbrv);

            testDlResult = testResult.RandomDriversLicense;
            Assert.IsNotNull(testDlResult);
            System.Diagnostics.Debug.WriteLine(testDlResult);

            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(testDlResult, "^[A-Z][0-9]{7}$"));
            Assert.IsTrue(testResult.ValidDriversLicense("U7376050"));
            Assert.IsFalse(testResult.ValidDriversLicense("686553072"));

            testState = "CT";
            testResult = Gov.UsState.GetStateByPostalCode(testState);
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType(testResult, typeof(Connecticut));

            Assert.AreEqual(testState, testResult.StateAbbrv);

            testDlResult = testResult.RandomDriversLicense;
            Assert.IsNotNull(testDlResult);
            System.Diagnostics.Debug.WriteLine(testDlResult);

        }

        [TestMethod]
        public void TestGetStateByName()
        {
            var testResult = Gov.UsState.GetStateByName("New York");
            Assert.IsNotNull(testResult);

            testResult = Gov.UsState.GetStateByName("NewYork");
            Assert.IsNotNull(testResult);

            testResult = Gov.UsState.GetStateByName("Kansas");
            Assert.IsNotNull(testResult);

        }

        [TestMethod]
        public void TestGetStateAll()
        {
            var noUse = Gov.UsState.GetStateByPostalCode("AZ");//the internal member is a singleton, this gets it pop'ed
            var theStates = Gov.UsState._theStates;

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
            var testSubject = Gov.UsState.GetStateByPostalCode("AZ");
            var testResults = testSubject.GetUniversities();

            Assert.IsNotNull(testResults);
            Assert.IsTrue(testResults.Any());
            Assert.AreNotEqual(0, testResults.Length);
            Assert.IsTrue(testResults.Any(univ => univ.Name == "Arizona State University"));
        }

        [TestMethod]
        public void TestGetHighSchools()
        {
            var testSubject = Gov.UsState.GetStateByPostalCode("AZ");
            var testResults = testSubject.GetHighSchools();

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);
            
        }

        [TestMethod]
        public void TestGetStateData()
        {
            var testStateName = "Maryland";
            var testState = Gov.UsState.GetStateByName(testStateName);
            var testResult = testState.GetStateData();
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.AverageEarnings);
            Assert.AreNotEqual(0,testResult.EmploymentSectors.Count);
            Assert.AreNotEqual(0, testResult.PercentOfGrads.Count);
            Assert.AreNotEqual(0, testResult.PropertyCrimeRate.Count);
            Assert.AreNotEqual(0, testResult.ViolentCrimeRate.Count);
            var percentCollegeGrad =
                testResult.PercentOfGrads.FirstOrDefault(x => x.Item1 == (OccidentalEdu.Bachelor | OccidentalEdu.Grad));
            Assert.AreNotEqual(0 , percentCollegeGrad);
            System.Diagnostics.Debug.WriteLine(percentCollegeGrad);
            for (var i = 0; i < 100; i++)
            {
                var attempt = Etx.TryAboveOrAt((int) Math.Round(percentCollegeGrad.Item2*2), Etx.Dice.OneHundred);
                System.Diagnostics.Debug.WriteLine(attempt);
            }
            
        }
    }
}
