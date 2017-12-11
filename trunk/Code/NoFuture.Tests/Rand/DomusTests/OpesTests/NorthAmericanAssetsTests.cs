using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Opes;
using NoFuture.Rand.Domus.Opes.Options;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestClass]
    public class NorthAmericanAssetsTests
    {
        [TestMethod]
        public void TestGetAssetItemsForRange()
        {
            var testSubject = new NorthAmericanAssets(null,
                new OpesOptions() {IsRenting = false, NumberOfVehicles = 1});

            var testResults = testSubject.GetAssetItemsForRange(75000.ToPecuniam(), DateTime.Today.AddYears(-1));

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Length);

            System.Diagnostics.Debug.WriteLine(Pondus.GetExpectedSum(testResults));
        }

        [TestMethod]
        public void TestGetGroupPortionsFromByFactorTables()
        {
            var testSubject = new NorthAmericanAssets(null,
                new OpesOptions() { IsRenting = false, NumberOfVehicles = 1 });

            var testResult = testSubject.GetGroupPortionsByFactorTables(75000.ToPecuniam());

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Item2).Sum();
            System.Diagnostics.Debug.WriteLine(testResultSum);

        }

        [TestMethod]
        public void TestResolveAssets()
        {
            var testSubject = new NorthAmericanAssets(null,
                new OpesOptions() { IsRenting = false, NumberOfVehicles = 1, SumTotal = 75000.ToPecuniam()});
            testSubject.ResolveAssets();

            Assert.IsNotNull(testSubject.Assets);
            Assert.AreNotEqual(0, testSubject.Assets.Count);

            System.Diagnostics.Debug.WriteLine(testSubject.TotalCurrentExpectedValue);

            testSubject = new NorthAmericanAssets(null,
                new OpesOptions() { IsRenting = true, NumberOfVehicles = 1, SumTotal = 75000.ToPecuniam() });
            testSubject.ResolveAssets();

            Assert.IsNotNull(testSubject.Assets);
            Assert.AreNotEqual(0, testSubject.Assets.Count);

            System.Diagnostics.Debug.WriteLine(testSubject.TotalCurrentExpectedValue);

        }

        [TestMethod]
        public void TestAssets()
        {
            var testSubject = new NorthAmericanAssets(null,
                new OpesOptions() { IsRenting = false, NumberOfVehicles = 1 });
            testSubject.ResolveAssets();

            var testResults = testSubject.Assets;

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Count);

            var expectations = WealthBaseTests.GetExpectedNamesFromXml("assets");
            
            foreach (var expect in expectations)
            {
                System.Diagnostics.Debug.WriteLine(expect);
                Assert.IsTrue(testResults.Any(x =>
                    x.My.Name == expect.Item2 && x.My.GetName(KindsOfNames.Group) == expect.Item1));
            }
        }

        [TestMethod]
        public void TestCarPayment()
        {
            var testSubject = new NorthAmericanAssets(null,
                new OpesOptions() { IsRenting = false, NumberOfVehicles = 1 });

            testSubject.ResolveAssets();

            Assert.IsNotNull(testSubject.CarPayment);
            Assert.AreNotEqual(Pecuniam.Zero, testSubject.CarPayment.ExpectedValue);
            System.Diagnostics.Debug.WriteLine(testSubject.CarPayment);
        }
    }
}
