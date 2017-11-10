using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Exo.NfText;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class FinancialFirmTests
    {
        [TestMethod]
        public void TestCommercialBankData()
        {
            var testResults = FedLrgBnk.CommercialBankData;
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults);

            foreach (var bank in testResults)
            {
                var usState = bank.BusinessAddress?.Item2?.PostalState ?? "";
                var city = bank.BusinessAddress?.Item2?.City ?? "";
                var line = string.Format(
                    "            <com name=\"{0}\" abbrev=\"{1}\" rssd=\"{2}\" us-state=\"{3}\" city=\"{4}\"/>\n",
                    bank.Name, bank.GetName(KindsOfNames.Abbrev), bank.Rssd.Value, usState, city);
                System.IO.File.AppendAllText(@"C:\Projects\31g\trunk\Code\NoFuture\Rand\Data\Source\US_Banks.txt", line);

            }

        }

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
