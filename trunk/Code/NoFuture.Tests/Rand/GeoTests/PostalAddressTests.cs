using System;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.CA;
using NoFuture.Rand.Geo.US;
using NoFuture.Shared.Core;
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
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestRandomCanadianAddress()
        {
            var testResult = PostalAddress.RandomCanadianAddress();
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Street);
            Assert.IsNotNull(testResult.CityArea);
        }

        [Test]
        public void TestValueSetter()
        {
            var crlf = new string(new[] { Constants.CR, Constants.LF });
            var testInput = $"1600 Pennsylvania Avenue NW{crlf}Washington, DC 20500";
            var testSubject = new PostalAddress {Value = testInput};
            Assert.IsNotNull(testSubject.CityArea);
            Assert.IsNotNull(testSubject.Street);
            Assert.IsInstanceOf(typeof(UsCityStateZip), testSubject.CityArea);
            Console.WriteLine(testSubject.ToString());

            testInput = $"80 Wellington St{crlf}Ottawa, ON K1A 0A2";
            testSubject = new PostalAddress { Value = testInput };
            Assert.IsNotNull(testSubject.CityArea);
            Assert.IsNotNull(testSubject.Street);
            Assert.IsInstanceOf(typeof(CaCityProvidencePost), testSubject.CityArea);
            Console.WriteLine(testSubject.ToString());
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
