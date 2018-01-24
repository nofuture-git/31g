using System;
using NUnit.Framework;
using NoFuture.Rand.Gov.US;

namespace NoFuture.Rand.Tests.GovTests
{
    [TestFixture]
    public class DriversLicenseTests
    {
        [Test]
        public void TestRandomDriversLicense()
        {
            var testResult = DriversLicense.RandomDriversLicense();
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Contains(" "));
            System.Diagnostics.Debug.WriteLine(testResult);
        }
    }
}
