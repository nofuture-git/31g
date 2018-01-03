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
        public void TestGetGroupPortionsFromByFactorTables()
        {
            var testSubject = new NorthAmericanAssets(null,
                new OpesOptions() { IsRenting = false, NumberOfVehicles = 1, SumTotal = 75000.ToPecuniam() });

            var testResult = testSubject.GetGroupNames2Portions(null);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Item2).Sum();
            System.Diagnostics.Debug.WriteLine(testResultSum);

        }

        [TestMethod]
        public void TestGetRealPropertyName2RandomRates()
        {
            var testSubject = new NorthAmericanAssets(null, null);

            var testResult = testSubject.GetRealPropertyName2RandomRates(testSubject.MyOptions);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [TestMethod]
        public void TestGetPersonalPropertyAssetNames2Rates()
        {
            var testSubject = new NorthAmericanAssets(null, null);

            var testResult = testSubject.GetPersonalPropertyAssetNames2Rates(testSubject.MyOptions);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [TestMethod]
        public void TestGetInstitutionalAssetName2Rates()
        {
            var testSubject = new NorthAmericanAssets(null, null);

            var testResult = testSubject.GetInstitutionalAssetName2Rates(testSubject.MyOptions);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }


        [TestMethod]
        public void TestGetSecuritiesAssetNames2RandomRates()
        {
            var testSubject = new NorthAmericanAssets(null, null);

            var testResult = testSubject.GetSecuritiesAssetNames2RandomRates(testSubject.MyOptions);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }


        [TestMethod]
        public void TestGetGroupNames()
        {
            var testNames = WealthBase.GetGroupNames(WealthBase.DomusOpesDivisions.Assets);
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("assets");
            var expectations = allNames.Select(n => n.Item1).Distinct();

            foreach (var tn in testNames)
            {
                Assert.IsTrue(expectations.Any(e => string.Equals(e, tn, StringComparison.OrdinalIgnoreCase)));
                System.Diagnostics.Debug.WriteLine(tn);
            }
        }

        [TestMethod]
        public void TestGetItemNames()
        {
            var testNames = WealthBase.GetItemNames(WealthBase.DomusOpesDivisions.Assets);
            var allNames = WealthBaseTests.GetExpectedNamesFromXml("assets");
            var expectations = allNames.Select(n => n.Item2).Distinct();

            foreach (var tn in testNames)
            {
                Assert.IsTrue(expectations.Any(e => string.Equals(e, tn.Name, StringComparison.OrdinalIgnoreCase)));
                System.Diagnostics.Debug.WriteLine(tn);
            }
        }

        [TestMethod]
        public void TestResolveItems()
        {
            var testSubject = new NorthAmericanAssets(null, null);

            testSubject.ResolveItems(new OpesOptions(){Inception = new DateTime(DateTime.Today.Year, 1,1)});

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            foreach (var item in testSubject.MyItems)
                System.Diagnostics.Debug.WriteLine(item);
        }
    }
}
