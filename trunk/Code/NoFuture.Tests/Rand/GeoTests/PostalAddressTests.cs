using System;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.GeoTests
{
    [TestFixture]
    public class PostalAddressTests
    {
        [Test]
        public void TestRandomAmericanAddress()
        {
            var testResult = PostalAddress.RandomAmericanAddress();
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Street);
            Assert.IsNotNull(testResult.CityArea);
        }

        [Test]
        public void TestToData()
        {
            var testSubject = PostalAddress.RandomAmericanAddress();
            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach(var tr in testResult.Keys)
                Console.WriteLine($"{tr}, {testResult[tr]}");
        }
    }
}
