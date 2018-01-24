using NUnit.Framework;
using NoFuture.Rand.Gov.US.Fed;

namespace NoFuture.Rand.Tests.GovTests
{
    [TestFixture]
    public class FedTests
    {
        [Test]
        public void TestRoutingTransitNumber()
        {
            var testSubject = RoutingTransitNumber.RandomRoutingNumber();
            Assert.IsNotNull(testSubject);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testSubject.Value));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testSubject.FedDistrictFullName));
            Assert.AreEqual(9, testSubject.Value.Length);

            System.Diagnostics.Debug.WriteLine(testSubject.Value);
            System.Diagnostics.Debug.WriteLine(testSubject.FedDistrictFullName);

            testSubject.Value = "053000196";

            Assert.AreEqual("05", testSubject.FedDistrict);
            Assert.AreEqual("30", testSubject.CheckProcCenter);
            Assert.AreEqual("0019", testSubject.AbaInstitutionId);
            Assert.AreEqual(6, testSubject.CheckDigit);
        }
    }
}
