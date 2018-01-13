using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Rand.Tests.DomusTests.PneumaTests
{
    [TestClass]
    public class DimensionTests
    {
        [TestMethod]
        public void TestEquals()
        {
            var test00 = new Domus.Pneuma.Dimension(0.31D,0.125D);
            var test01 = new Domus.Pneuma.Dimension(0.31D, 0.125D);

            Assert.IsTrue(test00.Equals(test01));
        }
    }
}
