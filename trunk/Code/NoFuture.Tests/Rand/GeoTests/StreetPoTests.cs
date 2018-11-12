using System;
using NoFuture.Rand.Core.Enums;
using NUnit.Framework;
using NoFuture.Rand.Geo;

namespace NoFuture.Rand.Tests.GeoTests
{
    [TestFixture]
    public class StreetPoTests
    {
        [Test]
        public void AmericanTest()
        {
            var testResult = StreetPo.RandomAmericanStreet();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.PostBox));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.StreetName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.StreetKind));

            Console.WriteLine(testResult);
        }

        [Test]
        public void TestToData()
        {
            var testSubject = StreetPo.RandomAmericanStreet();
            testSubject.GetData().SecondaryUnitDesignator = "Apt.";
            testSubject.GetData().SecondaryUnitId = "553";
            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach (var tr in testResult.Keys)
                Console.WriteLine($"{tr}, {testResult[tr]}");
        }

    }
}
