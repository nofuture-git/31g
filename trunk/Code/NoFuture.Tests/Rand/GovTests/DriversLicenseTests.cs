using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Gov.US;

namespace NoFuture.Rand.Tests.GovTests
{
    [TestClass]
    public class DriversLicenseTests
    {
        [TestMethod]
        public void TestRandomDriversLicense()
        {
            var testResult = DriversLicense.RandomDriversLicense();
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Contains(" "));
            System.Diagnostics.Debug.WriteLine(testResult);
        }
    }
}
