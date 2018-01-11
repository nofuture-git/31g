using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Geo;

namespace NoFuture.Rand.Tests.GeoTests
{
    [TestClass]
    public class StreetPoTests
    {
        [TestMethod]
        public void AmericanTest()
        {
            var testResult = StreetPo.American();
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
