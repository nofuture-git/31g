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

            testResult.Data.SecondaryUnitDesignator = "UNIT DESG";
            testResult.Data.SecondaryUnitId = "4451";

            System.Diagnostics.Debug.WriteLine(testResult);
        }

    }
}
