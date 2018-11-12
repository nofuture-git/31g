using System;
using NoFuture.Rand.Core.Enums;
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

        [Test]
        public void TestToData()
        {
            var testSubject = new DriversLicense("A635011103018289", "NY");
            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach (var k in testResult.Keys)
                Console.WriteLine($"{k}: {testResult[k]}");

        }
    }
}
