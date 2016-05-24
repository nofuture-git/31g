using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            foreach (var v in testResults)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0} {1} {2}",v.Name, v.Rssd, v.BankType));
            }
        }
    }
}
