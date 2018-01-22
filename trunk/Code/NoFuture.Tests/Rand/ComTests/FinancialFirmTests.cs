using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;

namespace NoFuture.Rand.Tests.ComTests
{
    [TestClass]
    public class FinancialFirmTests
    {

        [TestMethod]
        public void TestGetRandomBank()
        {
            var testInput = new UsCityStateZip(new AddressData {City = "New York City", StateAbbrev = "NY"});
            var testResult = NoFuture.Rand.Com.Bank.RandomBank(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual("",testResult.Name);
            Assert.IsNotNull(testResult.BusinessAddress);
            Assert.IsNotNull(testResult.BusinessAddress.Item2);
            Assert.AreEqual("New York City",testResult.BusinessAddress.Item2.City);
            Assert.IsNotNull(testResult.BusinessAddress.Item2.StateName);
            Assert.AreEqual("NY", testResult.BusinessAddress.Item2.StateAbbrev);
            System.Diagnostics.Debug.WriteLine(testResult.Name);

            testResult = NoFuture.Rand.Com.Bank.RandomBank(null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual("", testResult.Name);
            Assert.IsNotNull(testResult.BusinessAddress);
            Assert.IsNotNull(testResult.BusinessAddress.Item2);
            System.Diagnostics.Debug.WriteLine(testResult.Name);
        }
    }
}
