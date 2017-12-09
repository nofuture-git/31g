using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Opes;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestClass]
    public class NorthAmericanAssetsTests
    {
        [TestMethod]
        public void TestGetAssetItemsForRange()
        {
            var testSubject = new NorthAmericanAssets(null,
                new NorthAmericanAssets.AssestOptions {IsRenting = false, NumberOfVehicles = 1});

            var testResults = testSubject.GetAssetItemsForRange(75000.ToPecuniam(), DateTime.Today.AddYears(-1));

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);

            System.Diagnostics.Debug.WriteLine(Pondus.GetExpectedSum(testResults));
        }

        [TestMethod]
        public void TestGetGroupPortionsFromByFactorTables()
        {
            var testSubject = new NorthAmericanAssets(null,
                new NorthAmericanAssets.AssestOptions { IsRenting = false, NumberOfVehicles = 1 });

            var testResult = testSubject.GetGroupPortionsFromByFactorTables(75000.ToPecuniam(),null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Item2).Sum();
            System.Diagnostics.Debug.WriteLine(testResultSum);

        }
    }
}
