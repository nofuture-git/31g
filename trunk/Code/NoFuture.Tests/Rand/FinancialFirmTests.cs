using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class FinancialFirmTests
    {
        [TestMethod]
        public void TestCtor()
        {
            var testResult =
                new NoFuture.Rand.Com.FinancialFirm(
                    " JPMORGAN CHASE BK NA            1      852218     COLUMBUS, OH           NAT 2,074,9521,501,633  72   15   5,610  30   Y    0",
                    DateTime.Today);
            Assert.AreEqual("852218", testResult.Rssd.Value);
            Assert.IsNotNull(testResult.Assets);
            Assert.AreNotEqual(0, testResult.Assets.Count);
            Assert.AreNotEqual(0L, testResult.Assets.First().Value.ConsolidatedAssets);
            Assert.AreNotEqual(0L, testResult.Assets.First().Value.DomesticAssets);
            Assert.AreNotEqual(0, testResult.Assets.First().Value.DomesticBranches);
            Assert.AreEqual(0, testResult.Assets.First().Value.PercentForeignOwned);
            Assert.AreNotEqual(0, testResult.Assets.First().Value.ForeignBranches);
            Assert.IsTrue(testResult.IsInternational);

            System.Diagnostics.Debug.WriteLine(testResult.Assets.First().Value.ConsolidatedAssets);
            System.Diagnostics.Debug.WriteLine(testResult.Assets.First().Value.DomesticAssets);
        }

        [TestMethod]
        public void TestCommercialBankData()
        {
            NoFuture.BinDirectories.Root = @"C:\Projects\31g\trunk\bin";
            var testResults = NoFuture.Rand.Data.TreeData.CommercialBankData;
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults);

            foreach (var v in testResults)
            {
                foreach (var d in v.Assets.Keys)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("{0} : {1}", d, v.Assets[d]));
                }
            }
        }
    }
}
