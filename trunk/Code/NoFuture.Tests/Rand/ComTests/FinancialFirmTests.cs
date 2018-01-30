using System;
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
            var testInput = new UsCityStateZip(new AddressData {City = "New York City", StateAbbrev = "NY"});
            var testResult = NoFuture.Rand.Com.Bank.RandomBank(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual("",testResult.Name);
            Assert.IsNotNull(testResult.BusinessAddress);
            Assert.IsNotNull(testResult.BusinessAddress.Item2);
            Assert.AreEqual("New York City",testResult.BusinessAddress.Item2.City);
            Assert.IsNotNull(testResult.BusinessAddress.Item2.StateName);
            Assert.AreEqual("NY", testResult.BusinessAddress.Item2.StateAbbrev);
            Console.WriteLine(testResult.Name);

            testResult = NoFuture.Rand.Com.Bank.RandomBank(null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual("", testResult.Name);
            Assert.IsNotNull(testResult.BusinessAddress);
            Assert.IsNotNull(testResult.BusinessAddress.Item2);
            Console.WriteLine(testResult.Name);
        }
    }
}
