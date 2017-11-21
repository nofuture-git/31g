using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Endo;

namespace NoFuture.Rand.Tests
{
    [TestClass]
    public class FinancialFirmTests
    {

        [TestMethod]
        public void TestGetRandomBank()
        {
            var testInput = new UsCityStateZip(new AddressData {City = "New York City", StateAbbrv = "NY"});
            var testResult = NoFuture.Rand.Com.Bank.GetRandomBank(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual("",testResult.Name);
            Assert.IsNotNull(testResult.BusinessAddress);
            Assert.IsNotNull(testResult.BusinessAddress.Item2);
            Assert.AreEqual("New York City",testResult.BusinessAddress.Item2.City);
            Assert.IsNotNull(testResult.BusinessAddress.Item2.State);
            Assert.AreEqual("NY", testResult.BusinessAddress.Item2.State.StateAbbrv);
            System.Diagnostics.Debug.WriteLine(testResult.Name);

            testResult = NoFuture.Rand.Com.Bank.GetRandomBank(null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual("", testResult.Name);
            Assert.IsNotNull(testResult.BusinessAddress);
            Assert.IsNotNull(testResult.BusinessAddress.Item2);
            Assert.IsNotNull(testResult.BusinessAddress.Item2.State);
            System.Diagnostics.Debug.WriteLine(testResult.Name);
        }
    }
}
