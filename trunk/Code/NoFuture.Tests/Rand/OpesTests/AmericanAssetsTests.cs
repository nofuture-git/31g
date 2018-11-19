﻿using System;
using System.Linq;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Opes;
using NoFuture.Rand.Opes.US;
using NoFuture.Rand.Sp;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.OpesTests
{
    [TestFixture]
    public class AmericanAssetsTests
    {
        [Test]
        public void TestCtor()
        {
            var testSubject = new AmericanAssets();
            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreEqual(0, testSubject.MyItems.Count);

            //var futures =
            //    NamedReceivable.RandomNamedReceivalbleWithHistoryToSum("Futures", "Securities", 9000D.ToPecuniam());
            //testSubject.AddItem(futures);

            testSubject.AddItem("Futures", "Securities", 9000D.ToPecuniam());
            var testResultSum = testSubject.TotalCurrentValue;

            Assert.IsNotNull(testResultSum);
            Assert.AreNotEqual(Pecuniam.Zero, testResultSum);
            Assert.AreEqual(9000D.ToPecuniam(), testResultSum);

            var stocks =
                NamedReceivable.RandomNamedReceivalbleWithHistoryToSum("Stocks", "Securities", 2000D.ToPecuniam());
            testSubject.AddItem(stocks);

            testResultSum = testSubject.TotalCurrentValue.GetWholeNumber();
            Assert.AreNotEqual(Pecuniam.Zero, testResultSum);
            Assert.AreEqual(11000D.ToPecuniam(), testResultSum);
        }

        [Test]
        public void TestGetGroupPortionsFromByFactorTables()
        {
            var testSubject = new AmericanAssets();

            var testResult = testSubject.GetGroupNames2Portions(new OpesOptions() { IsRenting = false, NumberOfVehicles = 1, SumTotal = 75000.ToPecuniam() });

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Item2).Sum();
            System.Diagnostics.Debug.WriteLine(testResultSum);

        }

        [Test]
        public void TestGetRealPropertyName2RandomRates()
        {
            var testSubject = new AmericanAssets();

            var testResult = testSubject.GetRealPropertyName2RandomRates(new OpesOptions());

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [Test]
        public void TestGetPersonalPropertyAssetNames2Rates()
        {
            var testSubject = new AmericanAssets();

            var testResult = testSubject.GetPersonalPropertyAssetNames2Rates(new OpesOptions());

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }

        [Test]
        public void TestGetInstitutionalAssetName2Rates()
        {
            var testSubject = new AmericanAssets();

            var testResult = testSubject.GetInstitutionalAssetName2Rates(new OpesOptions());

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }


        [Test]
        public void TestGetSecuritiesAssetNames2RandomRates()
        {
            var testSubject = new AmericanAssets();

            var testResult = testSubject.GetSecuritiesAssetNames2RandomRates(new OpesOptions());

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var testResultSum = testResult.Select(kv => kv.Value).Sum();
            Assert.AreEqual(1D, Math.Round(testResultSum));

            foreach (var rate in testResult)
                System.Diagnostics.Debug.WriteLine(rate);
        }


        [Test]
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

        [Test]
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

        [Test]
        public void TestResolveItems()
        {
            var testSubject = new AmericanAssets();

            testSubject.RandomizeAllItems(new OpesOptions(){Inception = new DateTime(DateTime.Today.Year, 1,1)});

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);

            foreach (var item in testSubject.MyItems)
                System.Diagnostics.Debug.WriteLine(item);
        }

        [Test]
        public void TestRandomAssets()
        {
            var testSubject = AmericanAssets.RandomAssets(new OpesOptions { Inception = new DateTime(DateTime.Today.Year, 1, 1) });
            Assert.IsNotNull(testSubject);

            Assert.IsNotNull(testSubject.MyItems);
            Assert.AreNotEqual(0, testSubject.MyItems.Count);
        }

        [Test]
        public void TestAddItems()
        {
            var testSubject = new AmericanAssets();
            var testResult = testSubject.TotalCurrentValue;
            Assert.AreEqual(Pecuniam.Zero, testResult);

            
            testSubject.AddItem("Home",null,120000.0D);
            testResult = testSubject.TotalCurrentValue;
            Assert.AreEqual(120000.0D.ToPecuniam(), testResult);

            testSubject.AddItem("Car", null, 25000);
            testResult = testSubject.TotalCurrentValue;
            Assert.AreEqual((120000.0D + 25000).ToPecuniam(), testResult);

        }

        [Test]
        public void TestToData()
        {
            var testSubject = AmericanAssets.RandomAssets();
            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach(var tr in testResult.Keys)
                Console.WriteLine($"{tr}, {testResult[tr]}");
        }
    }
}
