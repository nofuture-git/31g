using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Rand.Tests.DomusTests.PneumaTests
{
    [TestClass]
    public class TraitTests
    {
        [TestMethod]
        public void TestEquals()
        {
            var test00 = new NoFuture.Rand.Domus.Pneuma.Openness();
            var test01 = new Domus.Pneuma.Openness();

            test01.Value = test00.Value;

            Assert.IsTrue(test00.Equals(test01));
        }
    }
}
