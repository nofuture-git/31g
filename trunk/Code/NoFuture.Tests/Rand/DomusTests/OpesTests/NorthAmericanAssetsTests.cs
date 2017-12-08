using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                new NorthAmericanAssets.AssestOptions {IsRenting = false, NumberOfVehicles = 1},
                DateTime.Today.AddYears(-1));

            var testResults = testSubject.GetAssetItemsForRange(75000.ToPecuniam(), DateTime.Today.AddYears(-1));

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);

            foreach(var tr in testResults)
                System.Diagnostics.Debug.WriteLine(tr);

            System.Diagnostics.Debug.WriteLine(Pondus.GetExpectedSum(testResults));
        }
    }
}
