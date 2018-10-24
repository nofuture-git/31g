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
            Console.WriteLine(testResult);

            testResult = DriversLicense.RandomDriversLicense("NY");
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);

        }

        [Test]
        public void TestDriversLicenseCtor()
        {
            var testResult = new DriversLicense("A635011103018289", "NY");
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.IssuingState);
            Assert.IsNotNull(testResult.Value);
            Assert.AreEqual("A635011103018289", testResult.Value);
        }
    }
}
