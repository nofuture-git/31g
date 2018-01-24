using System;
using System.Linq;
using NUnit.Framework;
using NoFuture.Rand.Core;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Gov.US.TheStates;

namespace NoFuture.Rand.Tests.GovTests
{
    [TestFixture]
    public class UsStateTests
    {
        [Test]
        public void GetStateTest()
        {
            var testState = "AZ";
            var testResult = UsState.GetStateByPostalCode(testState);
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<Arizona>(testResult);

            Assert.AreEqual(testState,testResult.StateAbbrev);

            var testDlResult = testResult.GetRandomDriversLicense();
            Assert.IsNotNull(testDlResult);
            System.Diagnostics.Debug.WriteLine(testDlResult);

            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(testDlResult,"^[ABDY][0-9]{8}$"));
            Assert.IsTrue(testResult.ValidDriversLicense("B70350018"));
            Assert.IsTrue(testResult.ValidDriversLicense("686553072"));
            Assert.IsFalse(testResult.ValidDriversLicense("68655307"));

            testState = "AR";
            testResult = UsState.GetStateByPostalCode(testState);
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<Arkansas>(testResult);

            Assert.AreEqual(testState, testResult.StateAbbrev);

            testDlResult = testResult.GetRandomDriversLicense();
            Assert.IsNotNull(testDlResult);
            System.Diagnostics.Debug.WriteLine(testDlResult);

            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(testDlResult, "^[0-9]{9}$"));
            Assert.IsFalse(testResult.ValidDriversLicense("U7376050"));
            Assert.IsTrue(testResult.ValidDriversLicense("686553072"));
            Assert.IsTrue(testResult.ValidDriversLicense("68655307"));

            testState = "CA";
            testResult = UsState.GetStateByPostalCode(testState);
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<California>(testResult);

            Assert.AreEqual(testState, testResult.StateAbbrev);

            testDlResult = testResult.GetRandomDriversLicense();
            Assert.IsNotNull(testDlResult);
            System.Diagnostics.Debug.WriteLine(testDlResult);

            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(testDlResult, "^[A-Z][0-9]{7}$"));
            Assert.IsTrue(testResult.ValidDriversLicense("U7376050"));
            Assert.IsFalse(testResult.ValidDriversLicense("686553072"));

            testState = "CT";
            testResult = UsState.GetStateByPostalCode(testState);
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<Connecticut>(testResult);

            Assert.AreEqual(testState, testResult.StateAbbrev);

            testDlResult = testResult.GetRandomDriversLicense();
            Assert.IsNotNull(testDlResult);
            System.Diagnostics.Debug.WriteLine(testDlResult);

        }

        [Test]
        public void TestGetStateByName()
        {
            var testResult = UsState.GetStateByName("New York");
            Assert.IsNotNull(testResult);

            testResult = UsState.GetStateByName("NewYork");
            Assert.IsNotNull(testResult);

            testResult = UsState.GetStateByName("Kansas");
            Assert.IsNotNull(testResult);

        }

        [Test]
        public void TestGetStateAll()
        {
            var noUse = UsState.GetStateByPostalCode("AZ");//the internal member is a singleton, this gets it pop'ed
            var theStates = UsState._theStates;

            foreach (var state in theStates)
            {
                System.Diagnostics.Debug.WriteLine(state.StateAbbrev);
                var randOut = state.GetRandomDriversLicense();
                System.Diagnostics.Debug.WriteLine("{0} '{1}'", state.StateAbbrev, randOut);
                var randIn = state.ValidDriversLicense(randOut);

                Assert.IsTrue(randIn);
            }
        }

        [Test]
        public void TestGetStateData()
        {
            var testStateName = "Maryland";
            var testState = UsState.GetStateByName(testStateName);
            var testResult = UsStateData.GetStateData(testState.ToString());
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
                var attempt = Etx.RandomRollAboveOrAt((int) Math.Round(percentCollegeGrad.Item2*2), Etx.Dice.OneHundred);
                System.Diagnostics.Debug.WriteLine(attempt);
            }
            
        }

        [Test]
        public void TestRandomState()
        {
            var testResult = UsState.RandomUsState();
            Assert.IsNotNull(testResult);

        }
    }
}
