using System;
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

    }
}
