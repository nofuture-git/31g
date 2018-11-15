using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NUnit.Framework;
using NoFuture.Rand.Geo;

namespace NoFuture.Rand.Tests.GeoTests
{
    [TestFixture]
    public class CityAreaTests
    {
        [Test]
        public void AmericanTest()
        {
            var testResult = CityArea.RandomAmericanCity();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.City));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ZipCode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.StateAbbrev));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.PostalCodeAddonFour));

            Console.WriteLine(testResult.City);
            Console.WriteLine(testResult.StateAbbrev);
            Console.WriteLine(testResult.ZipCode);
            Console.WriteLine(testResult.CbsaCode);
            Console.WriteLine(testResult.Msa);
            Console.WriteLine(testResult.AverageEarnings);
        }

        [Test]
        public void CanadianTest()
        {
            var testResult = CityArea.RandomCanadianCity();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.City));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.PostalCode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Providence));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ProvidenceAbbrv));

            Console.WriteLine(testResult.City);
            Console.WriteLine(testResult.ProvidenceAbbrv);
            Console.WriteLine(testResult.Providence);
            Console.WriteLine(testResult.PostalCode);
        }

        [Test]
        public void TestToData()
        {
            IObviate testSubject = CityArea.RandomAmericanCity();
            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach(var tr in testResult.Keys)
                Console.WriteLine($"{tr}, {testResult[tr]}");

            testSubject = CityArea.RandomCanadianCity();
            testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach (var tr in testResult.Keys)
                Console.WriteLine($"{tr}, {testResult[tr]}");
        }
    }
}
