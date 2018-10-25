using System;
using System.Collections.Generic;
using NoFuture.Rand.Com;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.ComTests
{
    [TestFixture]
    public class FinancialFirmTests
    {

        [Test]
        public void TestGetRandomBank()
        {
            var testInput = new UsCityStateZip(new AddressData {Locality = "New York City", RegionAbbrev = "NY"});
            var testResult = NoFuture.Rand.Com.Bank.RandomBank(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual("",testResult.Name);
            Assert.IsNotNull(testResult.BusinessAddress);
            Assert.IsNotNull(testResult.BusinessAddress.CityArea);
            Assert.IsInstanceOf(typeof(UsCityStateZip),testResult.BusinessAddress.CityArea);
            var city = (UsCityStateZip) testResult.BusinessAddress.CityArea;

            Assert.AreEqual("New York City", city.City);
            Assert.IsNotNull(city.StateName);
            Assert.AreEqual("NY", city.StateAbbrev);
            Console.WriteLine(testResult.Name);

            testResult = NoFuture.Rand.Com.Bank.RandomBank(null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual("", testResult.Name);
            Assert.IsNotNull(testResult.BusinessAddress);
            Assert.IsNotNull(testResult.BusinessAddress.CityArea);
            Console.WriteLine(testResult.Name);
        }

        [Test]
        public void TestCtorBank()
        {
            var cyear = DateTime.Today.Year;
            var testSubject = new Com.Bank
            {
                Name = "Bank of Banks",
                Assets = new Dictionary<DateTime, BankAssetsSummary>()
                {
                    {new DateTime(cyear - 3, 1, 1), new BankAssetsSummary {DomesticAssets = 50, TotalLiabilities = 25}},
                    {new DateTime(cyear - 2, 1, 1), new BankAssetsSummary {DomesticAssets = 55, TotalLiabilities = 30}},
                    {new DateTime(cyear - 1, 1, 1), new BankAssetsSummary {DomesticAssets = 60, TotalLiabilities = 32}},
                }
            };

            Assert.IsNotNull(testSubject.Assets);
            Assert.AreEqual("Bank of Banks", testSubject.Name);
        }
    }
}
