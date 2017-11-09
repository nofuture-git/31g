using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class FinancialFirmTests
    {
        [TestMethod]
        public void TestCommercialBankData()
        {
            var testResults = NoFuture.Rand.Data.TreeData.CommercialBankData;
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
    }
}
